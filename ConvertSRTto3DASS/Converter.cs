using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// SSA/ASS specification v4+ http://www.tcax.org/docs/ass-specs.htm
// SRT (SubRip) specification https://en.wikipedia.org/wiki/SubRip

namespace ConvertSRTto3DASS
{
    public class Converter
    {
        private enum StereoMode
        {
            SBS,
            OU,
            RG // Experimental
        }

        public static class ConverterDefaults
        {
            /// <summary>
            ///720 as default is arbitary,it's my favorite resolution for space saving and most mobile 3d viewing doesn't benefit from much higher (IE google cardboard or Meta Quest Headset).
            /// </summary>
            public const int ResX = 1280;
            public const int ResY = 720;

            public const int BaseResX = 1280;
            public const int BaseResY = 720;

            public const int FontSize = 16;
            public const int OffsetX = 4;
            public const int BottomOffset = 18;
            public const int SbsSideMargin = 640;
            public const int OuTopMargin = 385;
            public const int VerticalMargin = 25;

            public const string DefaultMode = "sbs";
        }

        private class Options
        {
            public string InputPath { get; set; }
            public string OutputPath { get; set; }

            public StereoMode Mode { get; set; } = StereoMode.SBS;

            public int ResX { get; set; } = ConverterDefaults.ResX;
            public int ResY { get; set; } = ConverterDefaults.ResY;
            public int BaseResX { get; set; } = ConverterDefaults.BaseResX;
            public int BaseResY { get; set; } = ConverterDefaults.BaseResY;
            public int FontSize { get; set; } = ConverterDefaults.FontSize;
            public int OffsetX { get; set; } = ConverterDefaults.OffsetX;
            public int BottomOffset { get; set; } = ConverterDefaults.BottomOffset;
            public int SbsSideMargin { get; set; } = ConverterDefaults.SbsSideMargin;
            public int OuTopMargin { get; set; } = ConverterDefaults.OuTopMargin;
            public int VerticalMargin { get; set; } = ConverterDefaults.VerticalMargin;
        }

        private static readonly Dictionary<Regex, string> RegexReplacementDict =
            new Dictionary<Regex, string>
            {
                { new Regex("<b>", RegexOptions.IgnoreCase), "{\\b1}" },
                { new Regex("</b>", RegexOptions.IgnoreCase), "{\\b0}" },
                { new Regex("<i>", RegexOptions.IgnoreCase), "{\\i1}" },
                { new Regex("</i>", RegexOptions.IgnoreCase), "{\\i0}" },
                { new Regex("<u>", RegexOptions.IgnoreCase), "{\\u1}" },
                { new Regex("</u>", RegexOptions.IgnoreCase), "{\\u0}" },
                { new Regex("</font>", RegexOptions.IgnoreCase), "{\\c&HFFFFFF&}" }
            };

        private static readonly Regex ColorRegex =
            new Regex("<font color=\"#([0-9A-Fa-f]{6})\">", RegexOptions.IgnoreCase);

        private static readonly Regex RemoveFormattingRegex =
            new Regex("<.+?>|(\\r)", RegexOptions.IgnoreCase);

        public static void Main(string[] args)
        {
            try
            {
                var options = ParseArguments(args);

                if (options == null)
                    return;

                if (!File.Exists(options.InputPath))
                {
                    Console.Error.WriteLine("Input file not found: " + options.InputPath);
                    return;
                }

                if (string.IsNullOrWhiteSpace(options.OutputPath))
                {
                    options.OutputPath = Path.Combine(
                        Path.GetDirectoryName(options.InputPath) ?? string.Empty,
                        Path.GetFileNameWithoutExtension(options.InputPath) + ".ass");
                }

                Console.WriteLine("Input:           " + options.InputPath);
                Console.WriteLine("Output:          " + options.OutputPath);
                Console.WriteLine("Mode:            " + options.Mode);
                Console.WriteLine("Resolution:      " + options.ResX + "x" + options.ResY);
                Console.WriteLine("Base Res:        " + options.BaseResX + "x" + options.BaseResY);
                Console.WriteLine("Font size:       " + options.FontSize);
                Console.WriteLine("OffsetX:         " + options.OffsetX);
                Console.WriteLine("BottomOffset:    " + options.BottomOffset);
                Console.WriteLine("SbsSideMargin:   " + options.SbsSideMargin);
                Console.WriteLine("OuTopMargin:     " + options.OuTopMargin);
                Console.WriteLine("VerticalMargin:  " + options.VerticalMargin);

                var extracted = ExtractSubFromSRT(options.InputPath);
                Console.WriteLine("Subtitle blocks parsed: " + extracted.Count);

                var style = CreateStandardStyle(options);
                var header = CreateHeader(options.InputPath, options.ResY, options.ResX);
                var eventsText = ProcessSubs(extracted, options);

                var finished =
                    "[Script Info]\n" +
                    header +
                    "\n\n[V4+ Styles]\n" +
                    style +
                    "\n\n[Events]\n" +
                    eventsText;

                File.WriteAllText(
                    options.OutputPath,
                    finished,
                    new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

                Console.WriteLine("Conversion complete.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR: " + ex.Message);
                Console.Error.WriteLine(ex.ToString());
            }
        }

        private static Options ParseArguments(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                PrintUsage();
                return null;
            }

            var options = new Options();
            int i = 0;

            if (args[0].StartsWith("--"))
            {
                Console.Error.WriteLine("First argument must be the input .srt file.");
                PrintUsage();
                return null;
            }

            options.InputPath = args[0];
            i = 1;

            while (i < args.Length)
            {
                string arg = args[i].Trim();

                switch (arg.ToLowerInvariant())
                {
                    case "--mode":
                        EnsureValueExists(args, i, "--mode");
                        options.Mode = ParseStereoMode(args[i + 1]);
                        i += 2;
                        break;

                    case "--resx":
                        EnsureValueExists(args, i, "--resx");
                        options.ResX = ParsePositiveInt(args[i + 1], "--resx");
                        i += 2;
                        break;

                    case "--resy":
                        EnsureValueExists(args, i, "--resy");
                        options.ResY = ParsePositiveInt(args[i + 1], "--resy");
                        i += 2;
                        break;

                    case "--baseresx":
                        EnsureValueExists(args, i, "--baseresx");
                        options.BaseResX = ParsePositiveInt(args[i + 1], "--baseresx");
                        i += 2;
                        break;

                    case "--baseresy":
                        EnsureValueExists(args, i, "--baseresy");
                        options.BaseResY = ParsePositiveInt(args[i + 1], "--baseresy");
                        i += 2;
                        break;

                    case "--fontsize":
                        EnsureValueExists(args, i, "--fontsize");
                        options.FontSize = ParsePositiveInt(args[i + 1], "--fontsize");
                        i += 2;
                        break;

                    case "--offsetx":
                        EnsureValueExists(args, i, "--offsetx");
                        options.OffsetX = ParseNonNegativeInt(args[i + 1], "--offsetx");
                        i += 2;
                        break;

                    case "--bottomoffset":
                        EnsureValueExists(args, i, "--bottomoffset");
                        options.BottomOffset = ParseNonNegativeInt(args[i + 1], "--bottomoffset");
                        i += 2;
                        break;

                    case "--sbssidemargin":
                        EnsureValueExists(args, i, "--sbssidemargin");
                        options.SbsSideMargin = ParseNonNegativeInt(args[i + 1], "--sbssidemargin");
                        i += 2;
                        break;

                    case "--outopmargin":
                        EnsureValueExists(args, i, "--outopmargin");
                        options.OuTopMargin = ParseNonNegativeInt(args[i + 1], "--outopmargin");
                        i += 2;
                        break;

                    case "--verticalmargin":
                        EnsureValueExists(args, i, "--verticalmargin");
                        options.VerticalMargin = ParseNonNegativeInt(args[i + 1], "--verticalmargin");
                        i += 2;
                        break;

                    case "--output":
                        EnsureValueExists(args, i, "--output");
                        options.OutputPath = args[i + 1];
                        i += 2;
                        break;

                    case "--help":
                    case "-h":
                    case "/?":
                        PrintUsage();
                        return null;

                    default:
                        throw new ArgumentException("Unknown argument: " + arg);
                }
            }

            return options;
        }

        private static void EnsureValueExists(string[] args, int index, string optionName)
        {
            if (index + 1 >= args.Length || args[index + 1].StartsWith("--"))
                throw new ArgumentException("Missing value for " + optionName);
        }

        private static int ParsePositiveInt(string value, string optionName)
        {
            if (!int.TryParse(value, out int parsed) || parsed <= 0)
                throw new ArgumentException("Invalid value for " + optionName + ": " + value);

            return parsed;
        }

        private static int ParseNonNegativeInt(string value, string optionName)
        {
            if (!int.TryParse(value, out int parsed) || parsed < 0)
                throw new ArgumentException("Invalid value for " + optionName + ": " + value);

            return parsed;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  ConvertSRTto3DASS.exe <input.srt> [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --mode sbs|ou|rg");
            Console.WriteLine("  --resx <number>");
            Console.WriteLine("  --resy <number>");
            Console.WriteLine("  --baseresx <number>      (scaling reference width, default 1280)");
            Console.WriteLine("  --baseresy <number>      (scaling reference height, default 720)");
            Console.WriteLine("  --fontsize <number>");
            Console.WriteLine("  --offsetx <number>       (RG eye separation)");
            Console.WriteLine("  --bottomoffset <number>  (RG bottom offset)");
            Console.WriteLine("  --sbssidemargin <number> (SBS side margin)");
            Console.WriteLine("  --outopmargin <number>   (OU top subtitle margin)");
            Console.WriteLine("  --verticalmargin <number> (general vertical/bottom margin)");
            Console.WriteLine("  --output <path>");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine(@"  ConvertSRTto3DASS.exe ""movie.srt"" --mode sbs");
            Console.WriteLine(@"  ConvertSRTto3DASS.exe ""movie.srt"" --mode ou --resx 1280 --resy 720");
            Console.WriteLine(@"  ConvertSRTto3DASS.exe ""movie.srt"" --mode rg --offsetx 6 --bottomoffset 24");
            Console.WriteLine(@"  ConvertSRTto3DASS.exe ""movie.srt"" --resx 1920 --resy 1080 --baseresx 1280 --baseresy 720");
            Console.WriteLine(@"  ConvertSRTto3DASS.exe ""movie.srt"" --sbssidemargin 220 --verticalmargin 16");
            Console.WriteLine(@"  ConvertSRTto3DASS.exe ""movie.srt"" --outopmargin 180 --fontsize 20 --output ""movie_custom.ass""");
        }

        private static StereoMode ParseStereoMode(string mode)
        {
            if (string.IsNullOrWhiteSpace(mode))
                return StereoMode.SBS;

            switch (mode.Trim().ToLowerInvariant())
            {
                case "sbs":
                case "sidebyside":
                case "side-by-side":
                    return StereoMode.SBS;

                case "ou":
                case "overunder":
                case "over-under":
                case "tab":
                case "topandbottom":
                case "top-and-bottom":
                    return StereoMode.OU;

                case "rg":
                case "redgreen":
                case "red-green":
                case "anaglyph":
                case "anaglyph-rg":
                    return StereoMode.RG;

                default:
                    throw new ArgumentException(
                        "Invalid 3D type '" + mode + "'. Valid values are: sbs, ou, rg");
            }
        }

        private static int ScaleX(int value, Options options)
        {
            return (int)Math.Round(value * (options.ResX / (double)options.BaseResX));
        }

        private static int ScaleY(int value, Options options)
        {
            return (int)Math.Round(value * (options.ResY / (double)options.BaseResY));
        }

        private static int ScaleFont(int fontSize, Options options)
        {
            return Math.Max(1, ScaleY(fontSize, options));
        }

        private static string ChangeFormatting(string line)
        {
            foreach (var tuple in RegexReplacementDict)
            {
                line = tuple.Key.Replace(line, tuple.Value);
            }

            while (ColorRegex.IsMatch(line))
            {
                var match = ColorRegex.Match(line);
                string rgb = match.Groups[1].Value; // RRGGBB

                string rr = rgb.Substring(0, 2);
                string gg = rgb.Substring(2, 2);
                string bb = rgb.Substring(4, 2);

                // ASS expects BBGGRR
                string assColor = bb + gg + rr;

                line = ColorRegex.Replace(line, "{\\c&H" + assColor + "&}", 1);
            }

            return RemoveFormatting(line);
        }

        private static string RemoveFormatting(string line)
        {
            return RemoveFormattingRegex.Replace(line, "");
        }

        private static string ProcessSubs(List<Tuple<string, string, string, string>> srt, Options options)
        {
            string endResult = "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text\n";

            foreach (var events in srt)
            {
                var start = ConvertTimeStamp(events.Item1);
                var end = ConvertTimeStamp(events.Item2);
                var text = events.Item3;

                if (options.Mode == StereoMode.SBS)
                {
                    var line = "Dialogue: 0," + start + "," + end + ",Right,,0,0,0,," + text + "\n";
                    line += "Dialogue: 1," + start + "," + end + ",Left,,0,0,0,," + text;
                    endResult += line + "\n";
                }
                else if (options.Mode == StereoMode.OU)
                {
                    var line = "Dialogue: 0," + start + "," + end + ",Top,,0,0,0,," + text + "\n";
                    line += "Dialogue: 1," + start + "," + end + ",Bottom,,0,0,0,," + text;
                    endResult += line + "\n";
                }
                else if (options.Mode == StereoMode.RG)
                {
                    int centerX = options.ResX / 2;
                    int scaledBottomOffset = ScaleY(options.BottomOffset, options);
                    int y = options.ResY - scaledBottomOffset;

                    int scaledOffsetX = ScaleX(options.OffsetX, options);
                    int redX = centerX - scaledOffsetX;
                    int greenX = centerX + scaledOffsetX;

                    string redText = "{\\an2\\pos(" + redX + "," + y + ")}" + text;
                    string greenText = "{\\an2\\pos(" + greenX + "," + y + ")}" + text;

                    var line = "Dialogue: 0," + start + "," + end + ",RedEye,,0,0,0,," + redText + "\n";
                    line += "Dialogue: 1," + start + "," + end + ",GreenEye,,0,0,0,," + greenText;
                    endResult += line + "\n";
                }
            }

            return endResult;
        }

        private static string ConvertTimeStamp(string timeStamp)
        {
            var tmp = timeStamp.Substring(1);
            tmp = tmp.Substring(0, tmp.Length - 1);
            return tmp.Replace(",", ".");
        }

        private static string CreateStandardStyle(Options options)
        {
            int scaledFontSize = ScaleFont(options.FontSize, options);

            switch (options.Mode)
            {
                case StereoMode.SBS:
                    return CreateSbsStyle(scaledFontSize, options);

                case StereoMode.OU:
                    return CreateOuStyle(scaledFontSize, options);

                case StereoMode.RG:
                    return CreateRgStyle(scaledFontSize, options);

                default:
                    throw new ArgumentOutOfRangeException(nameof(options.Mode));
            }
        }

        private static string CreateSbsStyle(int fontSize, Options options)
        {
            int sideMargin = ScaleX(options.SbsSideMargin, options);
            int verticalMargin = ScaleY(options.VerticalMargin, options);

            string style =
                "Format: " +
                "Name, " +
                "Fontname, " +
                "Fontsize, " +
                "PrimaryColour, " +
                "SecondaryColour, " +
                "OutlineColour, " +
                "BackColour, " +
                "Bold, " +
                "Italic, " +
                "Underline, " +
                "StrikeOut, " +
                "ScaleX, " +
                "ScaleY, " +
                "Spacing, " +
                "Angle, " +
                "BorderStyle, " +
                "Outline, " +
                "Shadow, " +
                "Alignment, " +
                "MarginL, " +
                "MarginR, " +
                "MarginV, " +
                "Encoding\n" +

                "Style: " +
                "Right," +
                "Arial," +
                fontSize + "," +
                "&HFFFFFF," +
                "&HFFFFFF," +
                "&H000000," +
                "&H000000," +
                "0," +
                "0," +
                "0," +
                "0," +
                "100," +
                "100," +
                "0," +
                "0," +
                "1," +
                "1," +
                "0," +
                "2," +
                sideMargin + "," +
                "0," +
                verticalMargin + "," +
                "0\n" +

                "Style: " +
                "Left," +
                "Arial," +
                fontSize + "," +
                "&HFFFFFF," +
                "&HFFFFFF," +
                "&H000000," +
                "&H000000," +
                "0," +
                "0," +
                "0," +
                "0," +
                "100," +
                "100," +
                "0," +
                "0," +
                "1," +
                "1," +
                "0," +
                "2," +
                "0," +
                sideMargin + "," +
                verticalMargin + "," +
                "0";

            return style;
        }

        private static string CreateOuStyle(int fontSize, Options options)
        {
            int bottomMargin = ScaleY(options.VerticalMargin, options);
            int topMargin = ScaleY(options.OuTopMargin, options);

            string style =
                "Format: " +
                "Name, " +
                "Fontname, " +
                "Fontsize, " +
                "PrimaryColour, " +
                "SecondaryColour, " +
                "OutlineColour, " +
                "BackColour, " +
                "Bold, " +
                "Italic, " +
                "Underline, " +
                "StrikeOut, " +
                "ScaleX, " +
                "ScaleY, " +
                "Spacing, " +
                "Angle, " +
                "BorderStyle, " +
                "Outline, " +
                "Shadow, " +
                "Alignment, " +
                "MarginL, " +
                "MarginR, " +
                "MarginV, " +
                "Encoding\n" +

                "Style: " +
                "Top," +
                "Arial," +
                fontSize + "," +
                "&HFFFFFF," +
                "&HFFFFFF," +
                "&H000000," +
                "&H000000," +
                "0," +
                "0," +
                "0," +
                "0," +
                "100," +
                "100," +
                "0," +
                "0," +
                "1," +
                "1," +
                "0," +
                "2," +
                "0," +
                "0," +
                topMargin + "," +
                "0\n" +

                "Style: " +
                "Bottom," +
                "Arial," +
                fontSize + "," +
                "&HFFFFFF," +
                "&HFFFFFF," +
                "&H000000," +
                "&H000000," +
                "0," +
                "0," +
                "0," +
                "0," +
                "100," +
                "100," +
                "0," +
                "0," +
                "1," +
                "1," +
                "0," +
                "2," +
                "0," +
                "0," +
                bottomMargin + "," +
                "0";

            return style;
        }

        private static string CreateRgStyle(int fontSize, Options options)
        {
            int verticalMargin = ScaleY(options.VerticalMargin, options);

            string style =
                "Format: " +
                "Name, " +
                "Fontname, " +
                "Fontsize, " +
                "PrimaryColour, " +
                "SecondaryColour, " +
                "OutlineColour, " +
                "BackColour, " +
                "Bold, " +
                "Italic, " +
                "Underline, " +
                "StrikeOut, " +
                "ScaleX, " +
                "ScaleY, " +
                "Spacing, " +
                "Angle, " +
                "BorderStyle, " +
                "Outline, " +
                "Shadow, " +
                "Alignment, " +
                "MarginL, " +
                "MarginR, " +
                "MarginV, " +
                "Encoding\n" +

                // ASS color format is BBGGRR
                // Red = &H0000FF
                "Style: " +
                "RedEye," +
                "Arial," +
                fontSize + "," +
                "&H0000FF," +
                "&H0000FF," +
                "&H000000," +
                "&H000000," +
                "0," +
                "0," +
                "0," +
                "0," +
                "100," +
                "100," +
                "0," +
                "0," +
                "1," +
                "1," +
                "0," +
                "2," +
                "0," +
                "0," +
                verticalMargin + "," +
                "0\n" +

                // Green = &H00FF00
                "Style: " +
                "GreenEye," +
                "Arial," +
                fontSize + "," +
                "&H00FF00," +
                "&H00FF00," +
                "&H000000," +
                "&H000000," +
                "0," +
                "0," +
                "0," +
                "0," +
                "100," +
                "100," +
                "0," +
                "0," +
                "1," +
                "1," +
                "0," +
                "2," +
                "0," +
                "0," +
                verticalMargin + "," +
                "0";

            return style;
        }

        private static string CreateHeader(string file, int resY = 720, int resX = 1280)
        {
            var name = Path.GetFileNameWithoutExtension(file);

            string scriptInfo =
                "; Generated by ConvertSRTto3D\n" +
                "Title: " + name + "\n" +
                "ScriptType: v4.00+\n" +
                "Collisions: Normal\n" +
                "PlayResX: " + resX + "\n" +
                "PlayResY: " + resY + "\n" +
                "ScaledBorderAndShadow: yes";

            return scriptInfo;
        }

        private static string ReadTextSmart(string path)
        {
            using (var reader = new StreamReader(path, Encoding.UTF8, detectEncodingFromByteOrderMarks: true))
            {
                string text = reader.ReadToEnd();

                if (text.Contains('\uFFFD'))
                    text = File.ReadAllText(path, Encoding.GetEncoding(1252));

                return text;
            }
        }

        private static List<Tuple<string, string, string, string>> ExtractSubFromSRT(string file)
        {
            var results = new List<Tuple<string, string, string, string>>();
            string srt = ReadTextSmart(file);

            srt = srt.Replace("\r\n", "\n").Replace("\r", "\n");

            var blocks = Regex.Split(srt.Trim(), @"\n\s*\n");

            int expectedDialogNumber = 1;

            foreach (var block in blocks)
            {
                var lines = block
                    .Split(new[] { '\n' }, StringSplitOptions.None)
                    .Select(l => l.Trim())
                    .Where(l => l.Length > 0)
                    .ToList();

                if (lines.Count < 3)
                    continue;

                if (!int.TryParse(lines[0], out int dialogNumber))
                {
                    Console.Error.WriteLine($"Skipping malformed block: expected subtitle number, got [{lines[0]}]");
                    continue;
                }

                if (dialogNumber != expectedDialogNumber)
                {
                    Console.Error.WriteLine(
                        $"Warning: expected subtitle number {expectedDialogNumber}, but found {dialogNumber}.");
                    expectedDialogNumber = dialogNumber;
                }

                expectedDialogNumber++;

                var timeMatch = Regex.Match(
                    lines[1],
                    @"^(?<start>\d{2}:\d{2}:\d{2},\d{3})\s*-->\s*(?<end>\d{2}:\d{2}:\d{2},\d{3})$");

                if (!timeMatch.Success)
                {
                    Console.Error.WriteLine($"Skipping malformed timestamp line: [{lines[1]}]");
                    continue;
                }

                string timestampStart = timeMatch.Groups["start"].Value;
                string timestampEnd = timeMatch.Groups["end"].Value;
                string subtitleText = string.Join("\\N", lines.Skip(2));

                results.Add(new Tuple<string, string, string, string>(
                    timestampStart,
                    timestampEnd,
                    ChangeFormatting(subtitleText),
                    ""));
            }

            return results;
        }
    }
}