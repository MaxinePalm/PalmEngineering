using Microsoft.AspNetCore.Html;

namespace PalmEngineering.Models
{
    /// <summary>Inline SVG line icons (24x24, inherit currentColor). Decorative, so hidden from screen readers.</summary>
    public static class Icons
    {
        private static readonly Dictionary<string, string> Paths = new()
        {
            ["gear"] = "<circle cx=\"12\" cy=\"12\" r=\"3\"/><circle cx=\"12\" cy=\"12\" r=\"7\"/><path d=\"M12 2v3M12 19v3M2 12h3M19 12h3M4.9 4.9l2.1 2.1M17 17l2.1 2.1M19.1 4.9L17 7M7 17l-2.1 2.1\"/>",
            ["mill"] = "<path d=\"M9 2h6v6H9z\"/><path d=\"M8 8h8l-1 10-3 4-3-4z\"/><path d=\"M10 12l4-2M10 16l4-2\"/>",
            ["turn"] = "<rect x=\"3\" y=\"7\" width=\"6\" height=\"10\"/><path d=\"M9 9h8v6H9\"/><path d=\"M17 12h4M21 10v4\"/>",
            ["grind"] = "<circle cx=\"9\" cy=\"12\" r=\"6\"/><circle cx=\"9\" cy=\"12\" r=\"1.5\"/><path d=\"M16 8l4-2M17 12h4M16 16l4 2\"/>",
            ["bolt"] = "<path d=\"M13 2L4 14h7l-1 8 9-12h-7z\"/>",
            ["inspect"] = "<circle cx=\"11\" cy=\"11\" r=\"7\"/><path d=\"M21 21l-4.3-4.3M8 11l2 2 4-4\"/>",
            ["cube"] = "<path d=\"M12 2l9 5v10l-9 5-9-5V7z\"/><path d=\"M12 22V12M3 7l9 5 9-5\"/>",
            ["target"] = "<circle cx=\"12\" cy=\"12\" r=\"9\"/><circle cx=\"12\" cy=\"12\" r=\"5\"/><circle cx=\"12\" cy=\"12\" r=\"1\"/>",
            ["bulb"] = "<path d=\"M9 18h6M10 21h4M12 3a6 6 0 0 0-4 10.5c.7.7 1 1.5 1 2.5h6c0-1 .3-1.8 1-2.5A6 6 0 0 0 12 3z\"/>",
            ["shield"] = "<path d=\"M12 2l8 3v6c0 5-3.5 9-8 11-4.5-2-8-6-8-11V5z\"/><path d=\"M8.5 12l2.5 2.5 4.5-5\"/>",
            ["clock"] = "<circle cx=\"12\" cy=\"12\" r=\"9\"/><path d=\"M12 7v5l3 2\"/>",
            ["pin"] = "<path d=\"M12 22s7-6.5 7-12a7 7 0 0 0-14 0c0 5.5 7 12 7 12z\"/><circle cx=\"12\" cy=\"10\" r=\"2.5\"/>",
            ["phone"] = "<path d=\"M5 3h4l2 5-2.5 1.5a11 11 0 0 0 6 6L16 13l5 2v4a2 2 0 0 1-2 2A16 16 0 0 1 3 5a2 2 0 0 1 2-2z\"/>",
            ["mail"] = "<rect x=\"3\" y=\"5\" width=\"18\" height=\"14\"/><path d=\"M3 7l9 6 9-6\"/>",
            ["zoom"] = "<circle cx=\"11\" cy=\"11\" r=\"7\"/><path d=\"M21 21l-4.3-4.3M11 8v6M8 11h6\"/>",
            ["people"] = "<circle cx=\"9\" cy=\"8\" r=\"3.5\"/><path d=\"M2 21a7 7 0 0 1 14 0\"/><path d=\"M16 4.5a3.5 3.5 0 0 1 0 7M18 21a7 7 0 0 0-3-5.7\"/>",
            ["check"] = "<path d=\"M4 12l5 5L20 6\"/>"
        };

        public static IHtmlContent Svg(string name, string cssClass = "icon")
        {
            var inner = Paths.TryGetValue(name, out var p) ? p : string.Empty;
            return new HtmlString(
                $"<svg class=\"{cssClass}\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"1.7\" stroke-linecap=\"square\" stroke-linejoin=\"miter\" aria-hidden=\"true\" focusable=\"false\">{inner}</svg>");
        }
    }
}
