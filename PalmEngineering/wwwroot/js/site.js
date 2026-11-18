(function () {
    'use strict';

    var $ = function (s, r) { return (r || document).querySelector(s); };
    var $$ = function (s, r) { return Array.prototype.slice.call((r || document).querySelectorAll(s)); };
    var reduceMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    var isApple = /Mac|iPhone|iPad/.test(navigator.platform || navigator.userAgent);

    $$('[data-mod]').forEach(function (el) { el.textContent = isApple ? '\u2318' : 'Ctrl'; });

    function isTyping(e) {
        var t = e.target;
        return !!t && (/^(INPUT|TEXTAREA|SELECT)$/.test(t.tagName) || t.isContentEditable);
    }

    /* ---------- Safe storage ---------- */
    var store = {
        get: function (k, d) { try { var v = localStorage.getItem(k); return v ? JSON.parse(v) : d; } catch (e) { return d; } },
        set: function (k, v) { try { localStorage.setItem(k, JSON.stringify(v)); return true; } catch (e) { return false; } }
    };

    /* ---------- Toasts (with optional undo) ---------- */
    var toastBox = $('#toasts');
    var lastUndo = null;

    function toast(message, opts) {
        opts = opts || {};
        if (!toastBox) return { dismiss: function () { } };
        var el = document.createElement('div');
        el.className = 'toast';
        var text = document.createElement('span');
        text.textContent = message;
        el.appendChild(text);
        var timer;
        var undoRef = null;

        function dismiss() {
            clearTimeout(timer);
            if (lastUndo === undoRef && undoRef) lastUndo = null;
            el.classList.remove('show');
            setTimeout(function () { if (el.parentNode) el.parentNode.removeChild(el); }, 260);
        }
        function start() { timer = setTimeout(dismiss, opts.duration || 6000); }

        if (opts.action) {
            var b = document.createElement('button');
            b.type = 'button';
            b.className = 'toast-action hit-slop';
            b.textContent = opts.action.label;
            undoRef = function () { opts.action.run(); dismiss(); };
            b.addEventListener('click', undoRef);
            el.appendChild(b);
            lastUndo = undoRef;
        }
        toastBox.appendChild(el);
        while (toastBox.children.length > 3) toastBox.removeChild(toastBox.firstElementChild);
        requestAnimationFrame(function () { el.classList.add('show'); });
        el.addEventListener('pointerenter', function () { clearTimeout(timer); });
        el.addEventListener('pointerleave', start);
        start();
        return { dismiss: dismiss };
    }

    document.addEventListener('keydown', function (e) {
        if ((e.metaKey || e.ctrlKey) && !e.shiftKey && e.key.toLowerCase() === 'z' && lastUndo && !isTyping(e)) {
            e.preventDefault();
            lastUndo();
        }
    });

    /* ---------- Sheets: interruptible, swipe to dismiss, back-gesture aware ---------- */
    var activeSheet = null;
    var swipeClose = false;

    function showSheet(s) {
        clearTimeout(s._t);
        s.classList.remove('is-closing');
        s.style.removeProperty('--drag');
        if (!s.open) s.showModal();
        requestAnimationFrame(function () { requestAnimationFrame(function () { s.classList.add('is-open'); }); });
    }

    function hideSheet(s, swipe) {
        clearTimeout(s._t);
        if (swipe && !reduceMotion) {
            s.style.setProperty('--drag', '110vh');
            s.classList.add('is-closing');
        }
        s.classList.remove('is-open');
        s._t = setTimeout(function () {
            if (s.open && !s.classList.contains('is-open')) s.close();
            s.classList.remove('is-closing');
            s.style.removeProperty('--drag');
        }, reduceMotion ? 0 : 280);
    }

    function openSheet(s) {
        if (activeSheet === s) return;
        if (activeSheet) { hideSheet(activeSheet); activeSheet = null; }
        else { history.pushState({ sheet: true }, ''); }
        activeSheet = s;
        showSheet(s);
    }

    function finishClose() {
        if (!activeSheet) return;
        var s = activeSheet;
        activeSheet = null;
        hideSheet(s, swipeClose);
        swipeClose = false;
    }

    function requestClose(s, swipe) {
        if (activeSheet !== s) return;
        swipeClose = !!swipe;
        if (history.state && history.state.sheet) history.back();
        else finishClose();
    }

    window.addEventListener('popstate', finishClose);
    window.addEventListener('pageshow', function (e) { if (e.persisted && activeSheet) finishClose(); });

    $$('dialog.sheet').forEach(function (s) {
        s.addEventListener('cancel', function (e) { e.preventDefault(); requestClose(s); });
        s.addEventListener('click', function (e) { if (e.target === s) requestClose(s); });
        $$('[data-sheet-close]', s).forEach(function (b) { b.addEventListener('click', function () { requestClose(s); }); });
        enableSwipe(s, $$('[data-drag]', s));
    });

    function enableSwipe(sheet, handles) {
        handles.forEach(function (h) {
            var id = null, startY = 0, base = 0, lastY = 0, lastT = 0, vel = 0;

            h.addEventListener('pointerdown', function (e) {
                if (e.pointerType === 'mouse' || e.button !== 0) return;
                id = e.pointerId;
                var m = new DOMMatrix(getComputedStyle(sheet).transform);
                base = Math.max(0, m.m42 || 0);
                sheet.style.setProperty('--drag', base + 'px');
                sheet.classList.add('dragging');
                startY = lastY = e.clientY;
                lastT = e.timeStamp;
                vel = 0;
                try { h.setPointerCapture(id); } catch (err) { }
            });

            h.addEventListener('pointermove', function (e) {
                if (id !== e.pointerId) return;
                var dt = Math.max(1, e.timeStamp - lastT);
                vel = (e.clientY - lastY) / dt;
                lastY = e.clientY;
                lastT = e.timeStamp;
                sheet.style.setProperty('--drag', Math.max(0, base + e.clientY - startY) + 'px');
            });

            function end(e) {
                if (id !== e.pointerId) return;
                id = null;
                sheet.classList.remove('dragging');
                var dy = Math.max(0, base + e.clientY - startY);
                if (e.type !== 'pointercancel' && (dy > 120 || vel > 0.6)) requestClose(sheet, true);
                else sheet.style.setProperty('--drag', '0px');
            }
            h.addEventListener('pointerup', end);
            h.addEventListener('pointercancel', end);
        });
    }

    /* ---------- Clipboard with optimistic feedback ---------- */
    function copyText(text, label, btn) {
        var prev = btn ? btn.textContent : null;
        var optimistic = null;
        if (btn) { btn.textContent = 'Copied'; btn.classList.add('is-done'); }
        else optimistic = toast(label + ' copied');

        function revert() { if (btn) { btn.textContent = prev; btn.classList.remove('is-done'); } }

        var p = (navigator.clipboard && navigator.clipboard.writeText)
            ? navigator.clipboard.writeText(text)
            : Promise.reject(new Error('unavailable'));
        p.then(function () {
            if (btn) setTimeout(revert, 2000);
        }).catch(function () {
            revert();
            if (optimistic) optimistic.dismiss();
            toast('Could not copy ' + label.toLowerCase() + '. Please copy it manually.');
        });
    }

    $$('[data-copy-btn]').forEach(function (b) {
        b.addEventListener('click', function () { copyText(b.getAttribute('data-copy-btn'), b.getAttribute('data-label') || 'Text', b); });
    });

    /* ---------- Shortlist (optimistic, undoable, stored on this device only) ---------- */
    var KEY = 'palm.shortlist.v1';
    var shortlist = new Map(store.get(KEY, []));

    function persist() { return store.set(KEY, Array.from(shortlist.entries())); }

    function renderShortlist() {
        $$('.save-btn').forEach(function (b) {
            var on = shortlist.has(b.getAttribute('data-id'));
            b.setAttribute('aria-pressed', on ? 'true' : 'false');
            var label = $('.save-label', b);
            var icon = $('.save-icon', b);
            if (label) label.textContent = on ? 'Saved' : 'Save';
            if (icon) icon.textContent = on ? '\u2665' : '\u2661';
        });
        var bar = $('#shortlist-bar');
        if (!bar) return;
        bar.hidden = shortlist.size === 0;
        var count = $('#shortlist-count');
        if (count) count.textContent = shortlist.size + (shortlist.size === 1 ? ' item' : ' items');
        var mail = $('#shortlist-mail');
        if (mail) {
            var lines = Array.from(shortlist.values()).map(function (t) { return '- ' + t; }).join('\r\n');
            var body = 'Hi Palm Engineering,\r\n\r\nI am interested in work similar to:\r\n' + lines + '\r\n\r\nPlease get in touch.\r\n';
            mail.href = 'mailto:' + mail.getAttribute('data-email') + '?subject=' + encodeURIComponent('Enquiry from your gallery') + '&body=' + encodeURIComponent(body);
        }
    }

    function setSaved(id, title, on, announce) {
        var before = new Map(shortlist);
        if (on) shortlist.set(id, title); else shortlist.delete(id);
        renderShortlist();
        if (!persist()) {
            shortlist = before;
            renderShortlist();
            toast('Could not save your shortlist on this device.');
            return;
        }
        if (announce) {
            toast(on ? 'Saved "' + title + '" to your shortlist' : 'Removed "' + title + '"', {
                action: { label: 'Undo', run: function () { setSaved(id, title, !on, false); } }
            });
        }
    }

    function clearShortlist() {
        if (!shortlist.size) { toast('Your shortlist is already empty.'); return; }
        var before = new Map(shortlist);
        shortlist.clear();
        persist();
        renderShortlist();
        toast('Shortlist cleared', {
            action: { label: 'Undo', run: function () { shortlist = new Map(before); persist(); renderShortlist(); } }
        });
    }

    $$('.save-btn').forEach(function (b) {
        b.addEventListener('click', function () {
            var id = b.getAttribute('data-id');
            setSaved(id, b.getAttribute('data-title'), !shortlist.has(id), true);
        });
    });
    var clearBtn = $('#shortlist-clear');
    if (clearBtn) clearBtn.addEventListener('click', clearShortlist);
    renderShortlist();

    /* ---------- Command menu ---------- */
    var cmd = $('#cmd');
    var cmdInput = $('#cmd-input');
    var cmdList = $('#cmd-list');
    var cmdItems = cmdList ? $$('[role="option"]', cmdList) : [];
    var cmdEmpty = $('#cmd-empty');
    var activeIdx = -1;

    cmdItems.forEach(function (li, i) { li.id = 'cmd-opt-' + i; });

    function visibleItems() { return cmdItems.filter(function (li) { return !li.hidden; }); }

    function setActive(li) {
        cmdItems.forEach(function (x) { x.setAttribute('aria-selected', 'false'); });
        if (li) {
            li.setAttribute('aria-selected', 'true');
            cmdInput.setAttribute('aria-activedescendant', li.id);
            li.scrollIntoView({ block: 'nearest' });
        } else {
            cmdInput.removeAttribute('aria-activedescendant');
        }
    }

    function filterCmd() {
        var q = cmdInput.value.trim().toLowerCase();
        cmdItems.forEach(function (li) {
            var hay = ((li.getAttribute('data-kw') || '') + ' ' + li.textContent).toLowerCase();
            li.hidden = !!q && hay.indexOf(q) === -1;
        });
        var groupHeads = $$('.cmd-group', cmdList);
        groupHeads.forEach(function (g) {
            var n = g.nextElementSibling, any = false;
            while (n && !n.classList.contains('cmd-group')) { if (!n.hidden) any = true; n = n.nextElementSibling; }
            g.hidden = !any;
        });
        var vis = visibleItems();
        if (cmdEmpty) cmdEmpty.hidden = vis.length > 0;
        setActive(vis[0] || null);
    }

    function runItem(li) {
        if (!li) return;
        var href = li.getAttribute('data-href');
        var copy = li.getAttribute('data-copy');
        var action = li.getAttribute('data-action');
        if (href) {
            if (/^(tel|mailto):/.test(href)) { window.location.href = href; return; }
            // Replace the sheet's history entry so Back returns to the page, not to a duplicate of it.
            var viaSheet = activeSheet && history.state && history.state.sheet;
            if (activeSheet) finishClose();
            if (viaSheet) window.location.replace(href);
            else window.location.href = href;
        } else if (copy) {
            requestClose(cmd);
            copyText(copy, li.getAttribute('data-label') || 'Text', null);
        } else if (action === 'clear-shortlist') {
            requestClose(cmd);
            clearShortlist();
        }
    }

    function openCmd() {
        if (!cmd) return;
        cmdInput.value = '';
        filterCmd();
        openSheet(cmd);
        setTimeout(function () { cmdInput.focus(); }, 30);
    }

    if (cmd) {
        $$('#cmd-open, [data-cmd-open]').forEach(function (b) { b.addEventListener('click', openCmd); });
        cmdInput.addEventListener('input', filterCmd);
        cmdInput.addEventListener('keydown', function (e) {
            var vis = visibleItems();
            var cur = vis.indexOf($('[aria-selected="true"]', cmdList));
            if (e.key === 'ArrowDown') { e.preventDefault(); setActive(vis[Math.min(vis.length - 1, cur + 1)]); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); setActive(vis[Math.max(0, cur - 1)]); }
            else if (e.key === 'Enter') { e.preventDefault(); runItem(vis[cur]); }
        });
        cmdList.addEventListener('click', function (e) {
            var li = e.target.closest('[role="option"]');
            if (li) runItem(li);
        });
        cmdList.addEventListener('pointermove', function (e) {
            var li = e.target.closest('[role="option"]');
            if (li && li.getAttribute('aria-selected') !== 'true') setActive(li);
        });
    }

    /* ---------- Keyboard shortcuts ("g" then a key, plus Ctrl/Cmd+K and "/") ---------- */
    var seqMap = {};
    cmdItems.forEach(function (li) {
        var keys = li.getAttribute('data-keys');
        if (keys && li.getAttribute('data-href')) seqMap[keys.toLowerCase()] = li.getAttribute('data-href');
    });
    var seq = '', seqTimer;

    document.addEventListener('keydown', function (e) {
        if ((e.metaKey || e.ctrlKey) && e.key.toLowerCase() === 'k') { e.preventDefault(); openCmd(); return; }
        if (e.metaKey || e.ctrlKey || e.altKey || isTyping(e) || activeSheet) return;
        if (e.key === '/') { e.preventDefault(); openCmd(); return; }
        var k = e.key.length === 1 ? e.key.toLowerCase() : '';
        if (!k) return;
        clearTimeout(seqTimer);
        seq = seq ? seq + ' ' + k : k;
        if (seqMap[seq]) { e.preventDefault(); var dest = seqMap[seq]; seq = ''; window.location.href = dest; return; }
        if (seq !== 'g') seq = (k === 'g') ? 'g' : '';
        if (seq) seqTimer = setTimeout(function () { seq = ''; }, 1200);
    });

    /* ---------- BlurHash placeholders + skeleton -> image fade ---------- */
    function paintAll(root) {
        $$('[data-blurhash]', root).forEach(function (box) {
            var canvas = $('canvas.bh', box);
            if (canvas && window.paintBlurHash && window.paintBlurHash(canvas, box.getAttribute('data-blurhash'))) {
                box.classList.add('is-ready');
            }
        });
    }
    paintAll(document);

    $$('img.fade-img').forEach(function (img) {
        if (img.complete && img.naturalWidth) return;
        img.classList.add('is-loading');
        var done = function () { img.classList.remove('is-loading'); };
        img.addEventListener('load', done, { once: true });
        img.addEventListener('error', done, { once: true });
    });

    /* ---------- Gallery lightbox ---------- */
    var lb = $('#lightbox');
    var items = $$('.gallery-open');
    var lbIndex = -1;

    function showItem(i) {
        if (!items.length) return;
        lbIndex = (i + items.length) % items.length;
        var b = items[lbIndex];
        var img = $('#lb-img');
        var media = $('#lb-media');
        $('#lb-title').textContent = b.getAttribute('data-title');
        $('#lb-desc').textContent = b.getAttribute('data-desc');
        media.setAttribute('data-blurhash', b.getAttribute('data-hash'));
        media.classList.remove('is-ready');
        var canvas = $('canvas.bh', media);
        if (canvas && window.paintBlurHash && window.paintBlurHash(canvas, b.getAttribute('data-hash'))) media.classList.add('is-ready');
        img.width = parseInt(b.getAttribute('data-w'), 10) || img.width;
        img.height = parseInt(b.getAttribute('data-h'), 10) || img.height;
        img.alt = b.getAttribute('data-alt');
        img.classList.add('is-loading');
        var done = function () { img.classList.remove('is-loading'); };
        img.onload = done;
        img.onerror = done;
        img.src = b.getAttribute('data-src');
        if (img.complete && img.naturalWidth) done();
    }

    function openItem(i) { showItem(i); openSheet(lb); }

    if (lb && items.length) {
        items.forEach(function (b, i) { b.addEventListener('click', function () { openItem(i); }); });
        $('#lb-prev').addEventListener('click', function () { showItem(lbIndex - 1); });
        $('#lb-next').addEventListener('click', function () { showItem(lbIndex + 1); });
        lb.addEventListener('keydown', function (e) {
            if (e.key === 'ArrowLeft') { e.preventDefault(); showItem(lbIndex - 1); }
            else if (e.key === 'ArrowRight') { e.preventDefault(); showItem(lbIndex + 1); }
        });

        var fromHash = function () {
            var slug = decodeURIComponent(location.hash.slice(1));
            if (!slug) return;
            for (var i = 0; i < items.length; i++) {
                if (items[i].getAttribute('data-slug') === slug) { openItem(i); return; }
            }
        };
        fromHash();
        window.addEventListener('hashchange', fromHash);
    }

    /* ---------- Prefetch fallback for browsers without speculation rules ---------- */
    var supportsSpec = window.HTMLScriptElement && HTMLScriptElement.supports && HTMLScriptElement.supports('speculationrules');
    if (!supportsSpec) {
        var seen = {};
        var prefetch = function (a) {
            if (!a || a.origin !== location.origin || a.pathname === location.pathname || seen[a.href]) return;
            seen[a.href] = true;
            var l = document.createElement('link');
            l.rel = 'prefetch';
            l.href = a.href;
            document.head.appendChild(l);
        };
        var hoverTimer;
        document.addEventListener('pointerover', function (e) {
            var a = e.target.closest && e.target.closest('a[href]');
            if (!a || e.pointerType === 'touch') return;
            clearTimeout(hoverTimer);
            hoverTimer = setTimeout(function () { prefetch(a); }, 65);
        });
        document.addEventListener('touchstart', function (e) {
            prefetch(e.target.closest && e.target.closest('a[href]'));
        }, { passive: true });
    }

    /* ---------- Cookie notice (informational: no non-essential cookies are used) ---------- */
    var notice = $('#cookie-notice');
    if (notice) {
        var NKEY = 'palm.cookie-notice.v1';
        if (!store.get(NKEY, false)) {
            notice.hidden = false;
            document.body.classList.add('has-cookie-notice');
            $('#cookie-ok').addEventListener('click', function () {
                store.set(NKEY, true);
                notice.hidden = true;
                document.body.classList.remove('has-cookie-notice');
            });
        }
    }
})();
