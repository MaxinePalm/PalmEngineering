/* Theme: follows the system setting until the visitor picks one, then remembers it on this device.
   Loaded in <head> (not deferred) so the right theme is applied before first paint. */
(function () {
    'use strict';
    var KEY = 'palm.theme.v1';
    var root = document.documentElement;
    var mq = window.matchMedia('(prefers-color-scheme: dark)');

    function stored() {
        try { var v = localStorage.getItem(KEY); return v === 'light' || v === 'dark' ? v : null; } catch (e) { return null; }
    }

    function apply(theme) {
        root.setAttribute('data-theme', theme);
        var meta = document.querySelector('meta[name="theme-color"]');
        if (meta) meta.setAttribute('content', theme === 'dark' ? '#0b1220' : '#0f2557');
        var label = theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode';
        Array.prototype.forEach.call(document.querySelectorAll('[data-theme-toggle]'), function (b) {
            b.setAttribute('aria-label', label);
        });
    }

    function toggle() {
        var next = root.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
        apply(next);
        try { localStorage.setItem(KEY, next); } catch (e) { }
    }

    apply(stored() || (mq.matches ? 'dark' : 'light'));
    document.addEventListener('DOMContentLoaded', function () { apply(root.getAttribute('data-theme')); });
    document.addEventListener('click', function (e) {
        if (e.target.closest && e.target.closest('[data-theme-toggle]')) toggle();
    });
    var follow = function (e) { if (!stored()) apply(e.matches ? 'dark' : 'light'); };
    if (mq.addEventListener) mq.addEventListener('change', follow); else if (mq.addListener) mq.addListener(follow);

    window.palmToggleTheme = toggle;
})();
