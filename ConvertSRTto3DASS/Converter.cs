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
    class Converter
    {
        private enum StereoMode
        {
            SBS,
            OU
        }

        private class Options
        {
            public string InputPath { get; set; }
            public string OutputPath { get; set; }
            public StereoMode Mode { get; set; } = StereoMode.SBS;
            public int ResX { get; set; } = 384;
            public int ResY { get; set; } = 288;
            public int FontSize { get; set; } = 16;
        }

        static void Main(string[] args)
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
                        Path.GetDirectoryName(options.InputPath) ?? "",
                        Path.GetFileNameWithoutExtension(options.InputPath) + ".ass");
                }

                Console.WriteLine("Input:     " + options.InputPath);
                Console.WriteLine("Output:    " + options.OutputPath);
                Console.WriteLine("Mode:      " + options.Mode);
                Console.WriteLine("Resolution:" + " " + options.ResX + "x" + options.ResY);
                Console.WriteLine("Font size: " + options.FontSize);

                var extracted = ExtractSubFromSRT(options.InputPath);
                Console.WriteLine("Subtitle blocks parsed: " + extracted.Count);

                var style = CreateStandardStyle(options.Mode, options.FontSize);
                var header = CreateHeader(options.InputPath, options.ResY, options.ResX);
                var eventsText = ProcessSubs(extracted, options.Mode);

                var finished =
                    "[Script Info]\n" +
                    header +
                    "\n\n[V4+ Styles]\n" +
                    style +
                    "\n\n[Events]\n" +
                    eventsText;

                File.WriteAllText(options.OutputPath, finished, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

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

            // First non-option argument is input file
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

                    case "--fontsize":
                        EnsureValueExists(args, i, "--fontsize");
                        options.FontSize = ParsePositiveInt(args[i + 1], "--fontsize");
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

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  ConvertSRTto3DASS.exe <input.srt> [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --mode sbs|ou");
            Console.WriteLine("  --resx <number>");
            Console.WriteLine("  --resy <number>");
            Console.WriteLine("  --fontsize <number>");
            Console.WriteLine("  --output <path>");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine(@"  ConvertSRTto3DASS.exe ""movie.srt"" --mode sbs");
            Console.WriteLine(@"  ConvertSRTto3DASS.exe ""movie.srt"" --mode ou --resx 384 --resy 288");
            Console.WriteLine(@"  ConvertSRTto3DASS.exe ""movie.srt"" --mode sbs --fontsize 20");
            Console.WriteLine(@"  ConvertSRTto3DASS.exe ""movie.srt"" --mode ou --output ""movie_ou.ass""");
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

                default:
                    throw new ArgumentException(
                        "Invalid 3D type '" + mode + "'. Valid values are: sbs, ou");
            }
        }

        private static Dictionary<Regex, string> regexReplacementDict =
            new Dictionary<Regex, string> {
                { new Regex("<b>", RegexOptions.IgnoreCase), "{\\b1}" },
                { new Regex("</b>", RegexOptions.IgnoreCase), "{\\b0}" },
                { new Regex("<i>", RegexOptions.IgnoreCase), "{\\i1}" },
                { new Regex("</i>", RegexOptions.IgnoreCase), "{\\i0}" },
                { new Regex("<u>", RegexOptions.IgnoreCase), "{\\u1}" },
                { new Regex("</u>", RegexOptions.IgnoreCase), "{\\u0}" },
                { new Regex("</font>", RegexOptions.IgnoreCase), "{\\c&HFFFFFF&}" }
            };

        private static Regex color = new Regex("<font color=\"#([0-9A-Fa-f]{6})\">", RegexOptions.IgnoreCase);

        private static string ChangeFormatting(string line)
        {
            foreach (var tuple in regexReplacementDict)
            {
                line = tuple.Key.Replace(line, tuple.Value);
            }

            while (color.IsMatch(line))
            {
                var match = color.Match(line);
                string rgb = match.Groups[1].Value; // RRGGBB

                string rr = rgb.Substring(0, 2);
                string gg = rgb.Substring(2, 2);
                string bb = rgb.Substring(4, 2);

                // ASS expects BBGGRR
                string assColor = bb + gg + rr;

                line = color.Replace(line, "{\\c&H" + assColor + "&}", 1);
            }

            return RemoveFormatting(line);
        }

        private static Regex reg = new Regex("<.+?>|(\\r)", RegexOptions.IgnoreCase);

        private static string RemoveFormatting(string line)
        {
            var replacement = reg.Replace(line, "");
            return replacement;
        }

        private static string ProcessSubs(List<Tuple<string, string, string, string>> srt, StereoMode mode)
        {
            string endResult = "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text\n";

            string style1 = mode == StereoMode.SBS ? "Right" : "Top";
            string style2 = mode == StereoMode.SBS ? "Left" : "Bottom";

            foreach (var events in srt)
            {
                var start = ConvertTimeStamp(events.Item1);
                var end = ConvertTimeStamp(events.Item2);

                var line = "Dialogue: 0," + start + "," + end + "," + style1 + ",,0,0,0,," + events.Item3 + "\n";
                line += "Dialogue: 1," + start + "," + end + "," + style2 + ",,0,0,0,," + events.Item3;

                endResult += line + "\n";
            }

            return endResult;
        }

        private static string ConvertTimeStamp(string timeStamp)
        {
            var tmp = timeStamp.Substring(1);
            tmp = tmp.Substring(0, tmp.Length - 1);
            return tmp.Replace(",", ".");
        }

        private static string CreateStandardStyle(StereoMode stereoMode, int fontSize)
        {
            if (stereoMode == StereoMode.SBS)
                return CreateSbsStyle(fontSize);

            return CreateOuStyle(fontSize);
        }

        private static string CreateSbsStyle(int fontSize)
        {
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
                "&Hffffff," +
                "&Hffffff," +
                "&H0," +
                "&H0," +
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
                "192," +
                "0," +
                "10," +
                "0\n" +

                "Style: " +
                "Left," +
                "Arial," +
                fontSize + "," +
                "&Hffffff," +
                "&Hffffff," +
                "&H0," +
                "&H0," +
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
                "192," +
                "10," +
                "0";

            return style;
        }

        private static string CreateOuStyle(int fontSize)
        {
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
                "&Hffffff," +
                "&Hffffff," +
                "&H0," +
                "&H0," +
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
                "154," +
                "0\n" +

                "Style: " +
                "Bottom," +
                "Arial," +
                fontSize + "," +
                "&Hffffff," +
                "&Hffffff," +
                "&H0," +
                "&H0," +
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
                "10," +
                "0";

            return style;
        }

        private static string CreateHeader(string file, int resY = 288, int resX = 384)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            string scriptinfo =
                "; Generated by ConvertSRTto3D\n" +
                "Title: " + name + "\n" +
                "ScriptType: v4.00+\n" +
                "Collisions: Normal\n" +
                "PlayResX: " + resX + "\n" +
                "PlayResY: " + resY + "\n" +
                "ScaledBorderAndShadow: yes";

            return scriptinfo;
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