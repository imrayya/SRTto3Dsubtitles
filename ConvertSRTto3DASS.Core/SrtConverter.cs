using System.Text;
using System.Text.RegularExpressions;

namespace ConvertSRTto3DASS;

/// <summary>
/// Core SRT-to-ASS conversion logic for 3D subtitle formats.
/// </summary>
public static class SrtConverter
{
    public class ConversionResult
    {
        public string OutputText { get; set; } = "";
        public int SubtitleCount { get; set; }
    }

    public static ConversionResult Convert(ConversionOptions options)
    {
        var srtText = ReadSrt(options.InputPath);
        var subtitles = ParseSrt(srtText);
        var header = CreateHeader(options);
        var styles = CreateStyles(options);
        var events = GenerateEvents(subtitles, options);

        var output =
            "[Script Info]\n" +
            header +
            "\n\n[V4+ Styles]\n" +
            styles +
            "\n\n[Events]\n" +
            events;

        File.WriteAllText(options.OutputPath, output, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

        return new ConversionResult
        {
            OutputText = output,
            SubtitleCount = subtitles.Count
        };
    }

    #region SRT Parsing

    private static string ReadSrt(string path)
    {
        using var reader = new StreamReader(path, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        var text = reader.ReadToEnd();

        // Fallback to Windows-1252 if we see replacement characters
        if (text.Contains('\uFFFD'))
            text = File.ReadAllText(path, Encoding.GetEncoding(1252));

        return text;
    }

    private static List<SrtSubtitle> ParseSrt(string srtText)
    {
        var results = new List<SrtSubtitle>();
        srtText = srtText.Replace("\r\n", "\n").Replace("\r", "\n").Trim();

        var blocks = Regex.Split(srtText, @"\n\s*\n");
        int expectedNumber = 1;

        foreach (var block in blocks)
        {
            var lines = block.Split('\n', StringSplitOptions.TrimEntries)
                             .Where(l => l.Length > 0)
                             .ToList();

            if (lines.Count < 3) continue;

            if (!int.TryParse(lines[0], out var number)) continue;

            if (number != expectedNumber)
            {
                Console.Error.WriteLine($"Warning: expected subtitle number {expectedNumber}, but found {number}.");
                expectedNumber = number;
            }
            expectedNumber++;

            var timeMatch = Regex.Match(lines[1],
                @"^(?<start>\d{2}:\d{2}:\d{2},\d{3})\s*-->\s*(?<end>\d{2}:\d{2}:\d{2},\d{3})$");
            if (!timeMatch.Success)
            {
                Console.Error.WriteLine($"Skipping malformed timestamp: [{lines[1]}]");
                continue;
            }

            var text = string.Join("\\N", lines.Skip(2));
            text = NormalizeFormatting(text);

            results.Add(new SrtSubtitle
            {
                Start = timeMatch.Groups["start"].Value,
                End = timeMatch.Groups["end"].Value,
                Text = text
            });
        }

        return results;
    }

    #endregion

    #region Formatting

    private static readonly Dictionary<Regex, string> _tagReplacements = new()
    {
        { new Regex("<b>", RegexOptions.IgnoreCase), "{\\b1}" },
        { new Regex("</b>", RegexOptions.IgnoreCase), "{\\b0}" },
        { new Regex("<i>", RegexOptions.IgnoreCase), "{\\i1}" },
        { new Regex("</i>", RegexOptions.IgnoreCase), "{\\i0}" },
        { new Regex("<u>", RegexOptions.IgnoreCase), "{\\u1}" },
        { new Regex("</u>", RegexOptions.IgnoreCase), "{\\u0}" },
        { new Regex("</font>", RegexOptions.IgnoreCase), "{\\c&HFFFFFF&}" }
    };

    private static readonly Regex _colorRegex =
        new Regex("<font color=\"#([0-9A-Fa-f]{6})\">", RegexOptions.IgnoreCase);

    private static readonly Regex _markupRegex =
        new Regex("<.+?>|(\\r)", RegexOptions.IgnoreCase);

    private static string NormalizeFormatting(string text)
    {
        foreach (var (pattern, replacement) in _tagReplacements)
            text = pattern.Replace(text, replacement);

        // Convert HTML color to ASS BBGGRR format
        while (_colorRegex.IsMatch(text))
        {
            var match = _colorRegex.Match(text);
            var rgb = match.Groups[1].Value;
            var assColor = rgb.Substring(4, 2) + rgb.Substring(2, 2) + rgb.Substring(0, 2);
            text = _colorRegex.Replace(text, "{\\c&H" + assColor + "&}", 1);
        }

        return _markupRegex.Replace(text, "");
    }

    #endregion

    #region Time & Scaling

    private static string ConvertTimestamp(string timestamp)
    {
        var tmp = timestamp.Substring(1);
        tmp = tmp.Substring(0, tmp.Length - 1);
        return tmp.Replace(",", ".");
    }

    private static int ScaleX(int value, ConversionOptions o) =>
        (int)Math.Round(value * (o.ResX / (double)o.BaseResX));

    private static int ScaleY(int value, ConversionOptions o) =>
        (int)Math.Round(value * (o.ResY / (double)o.BaseResY));

    private static int ScaleFont(int fontSize, ConversionOptions o) =>
        Math.Max(1, ScaleY(fontSize, o));

    #endregion

    #region ASS Output Generation

    private static string CreateHeader(ConversionOptions o) =>
        $"; Generated by SRTto3Dsubtitles\n" +
        $"Title: {Path.GetFileNameWithoutExtension(o.InputPath)}\n" +
        "ScriptType: v4.00+\n" +
        "Collisions: Normal\n" +
        $"PlayResX: {o.ResX}\n" +
        $"PlayResY: {o.ResY}\n" +
        "ScaledBorderAndShadow: yes";

    private static string CreateStyles(ConversionOptions o)
    {
        var fontSize = ScaleFont(o.FontSize, o);

        return o.Mode.ToLowerInvariant() switch
        {
            "ou" => CreateOuStyles(fontSize, o),
            "rg" => CreateRgStyles(fontSize, o),
            _ => CreateSbsStyles(fontSize, o)
        };
    }

    private static string CreateSbsStyles(int fontSize, ConversionOptions o)
    {
        var sideMargin = ScaleX(o.SbsSideMargin, o);
        var verticalMargin = ScaleY(o.VerticalMargin, o);

        return FormatStyle("Right", fontSize, sideMargin, 0, verticalMargin) + "\n" +
               FormatStyle("Left", fontSize, 0, sideMargin, verticalMargin);
    }

    private static string CreateOuStyles(int fontSize, ConversionOptions o)
    {
        var topMargin = ScaleY(o.OuTopMargin, o);
        var bottomMargin = ScaleY(o.VerticalMargin, o);

        return FormatStyle("Top", fontSize, 0, 0, topMargin) + "\n" +
               FormatStyle("Bottom", fontSize, 0, 0, bottomMargin);
    }

    private static string CreateRgStyles(int fontSize, ConversionOptions o)
    {
        var verticalMargin = ScaleY(o.VerticalMargin, o);

        // Red = &H0000FF, Green = &H00FF00 (ASS uses BBGGRR)
        return FormatStyle("RedEye", fontSize, 0, 0, verticalMargin, primary: "&H0000FF") + "\n" +
               FormatStyle("GreenEye", fontSize, 0, 0, verticalMargin, primary: "&H00FF00");
    }

    private static string FormatStyle(string name, int fontSize, int marginL, int marginR, int marginV,
        string primary = "&HFFFFFF")
    {
        return $"Style: {name},Arial,{fontSize},{primary},{primary},&H000000,&H000000," +
               $"0,0,0,0,100,100,0,0,1,1,0,2," +
               $"{marginL},{marginR},{marginV},0";
    }

    private static string GenerateEvents(List<SrtSubtitle> subs, ConversionOptions o)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text");

        foreach (var sub in subs)
        {
            var start = ConvertTimestamp(sub.Start);
            var end = ConvertTimestamp(sub.End);
            var text = sub.Text;

            switch (o.Mode.ToLowerInvariant())
            {
                case "sbs":
                    sb.AppendLine($"Dialogue: 0,{start},{end},Right,,0,0,0,,{text}");
                    sb.AppendLine($"Dialogue: 1,{start},{end},Left,,0,0,0,,{text}");
                    break;

                case "ou":
                    sb.AppendLine($"Dialogue: 0,{start},{end},Top,,0,0,0,,{text}");
                    sb.AppendLine($"Dialogue: 1,{start},{end},Bottom,,0,0,0,,{text}");
                    break;

                case "rg":
                    var centerX = o.ResX / 2;
                    var scaledBottomOffset = ScaleY(o.BottomOffset, o);
                    var y = o.ResY - scaledBottomOffset;
                    var scaledOffsetX = ScaleX(o.OffsetX, o);
                    var redX = centerX - scaledOffsetX;
                    var greenX = centerX + scaledOffsetX;

                    sb.AppendLine($"Dialogue: 0,{start},{end},RedEye,,0,0,0,,{{\\an2\\pos({redX},{y})}}{text}");
                    sb.AppendLine($"Dialogue: 1,{start},{end},GreenEye,,0,0,0,,{{\\an2\\pos({greenX},{y})}}{text}");
                    break;
            }
        }

        return sb.ToString();
    }

    #endregion

    #region Data Classes

    private class SrtSubtitle
    {
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
        public string Text { get; set; } = "";
    }

    #endregion
}
