/* Minimal BlurHash decoder (https://blurha.sh). Exposes window.paintBlurHash. */
(function () {
    'use strict';
    var DIGITS = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#$%*+,-.:;=?@[]^_{|}~';

    function decode83(str) {
        var v = 0;
        for (var i = 0; i < str.length; i++) v = v * 83 + DIGITS.indexOf(str[i]);
        return v;
    }
    function toLinear(v) {
        v /= 255;
        return v <= 0.04045 ? v / 12.92 : Math.pow((v + 0.055) / 1.055, 2.4);
    }
    function toSRGB(v) {
        v = Math.max(0, Math.min(1, v));
        return v <= 0.0031308 ? Math.round(v * 12.92 * 255 + 0.5) : Math.round((1.055 * Math.pow(v, 1 / 2.4) - 0.055) * 255 + 0.5);
    }
    function signPow(v, e) { return (v < 0 ? -1 : 1) * Math.pow(Math.abs(v), e); }

    function decodeDC(value) {
        return [toLinear(value >> 16), toLinear((value >> 8) & 255), toLinear(value & 255)];
    }
    function decodeAC(value, max) {
        var r = Math.floor(value / (19 * 19)), g = Math.floor(value / 19) % 19, b = value % 19;
        return [signPow((r - 9) / 9, 2) * max, signPow((g - 9) / 9, 2) * max, signPow((b - 9) / 9, 2) * max];
    }

    function decodeBlurHash(hash, width, height) {
        if (!hash || hash.length < 6) return null;
        var sizeFlag = decode83(hash[0]);
        var numY = Math.floor(sizeFlag / 9) + 1, numX = (sizeFlag % 9) + 1;
        if (hash.length !== 4 + 2 * numX * numY) return null;
        var maxVal = (decode83(hash[1]) + 1) / 166;
        var colors = [], i, j, x, y;
        for (i = 0; i < numX * numY; i++) {
            colors.push(i === 0 ? decodeDC(decode83(hash.substring(2, 6))) : decodeAC(decode83(hash.substring(4 + i * 2, 6 + i * 2)), maxVal));
        }
        var bytes = new Uint8ClampedArray(width * height * 4);
        for (y = 0; y < height; y++) {
            for (x = 0; x < width; x++) {
                var r = 0, g = 0, b = 0;
                for (j = 0; j < numY; j++) {
                    for (i = 0; i < numX; i++) {
                        var basis = Math.cos(Math.PI * x * i / width) * Math.cos(Math.PI * y * j / height);
                        var c = colors[i + j * numX];
                        r += c[0] * basis; g += c[1] * basis; b += c[2] * basis;
                    }
                }
                var p = 4 * (x + y * width);
                bytes[p] = toSRGB(r); bytes[p + 1] = toSRGB(g); bytes[p + 2] = toSRGB(b); bytes[p + 3] = 255;
            }
        }
        return bytes;
    }

    function paintBlurHash(canvas, hash) {
        var w = canvas.width || 32, h = canvas.height || 32;
        var bytes = decodeBlurHash(hash, w, h);
        if (!bytes) return false;
        var ctx = canvas.getContext('2d');
        if (!ctx) return false;
        ctx.putImageData(new ImageData(bytes, w, h), 0, 0);
        return true;
    }

    window.paintBlurHash = paintBlurHash;
})();
