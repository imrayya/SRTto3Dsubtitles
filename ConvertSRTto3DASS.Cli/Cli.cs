using System.Text;

namespace ConvertSRTto3DASS;

internal static class Cli
{
    public static bool Run(string[] args)
    {
        if (args.Length == 0 || args[0] is "--help" or "-h" or "/?")
        {
            PrintUsage();
            return false;
        }

        var inputPath = args[0];
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return true;
        }

        // Parse options
        var options = new ConversionOptions
        {
            InputPath = inputPath,
            Mode = "sbs",
            ResX = 1280,
            ResY = 720,
            BaseResX = 1280,
            BaseResY = 720,
            FontSize = 16,
            OffsetX = 4,
            BottomOffset = 18,
            SbsSideMargin = 640,
            OuTopMargin = 385,
            VerticalMargin = 25,
        };

        int i = 1;
        while (i < args.Length)
        {
            var arg = args[i].Trim().ToLowerInvariant();
            switch (arg)
            {
                case "--mode":
                    options.Mode = GetNextArg(i++);
                    break;
                case "--resx":
                    options.ResX = ParseInt(i++);
                    break;
                case "--resy":
                    options.ResY = ParseInt(i++);
                    break;
                case "--baseresx":
                    options.BaseResX = ParseInt(i++);
                    break;
                case "--baseresy":
                    options.BaseResY = ParseInt(i++);
                    break;
                case "--fontsize":
                    options.FontSize = ParseInt(i++);
                    break;
                case "--offsetx":
                    options.OffsetX = ParseInt(i++);
                    break;
                case "--bottomoffset":
                    options.BottomOffset = ParseInt(i++);
                    break;
                case "--sbssidemargin":
                    options.SbsSideMargin = ParseInt(i++);
                    break;
                case "--outopmargin":
                    options.OuTopMargin = ParseInt(i++);
                    break;
                case "--verticalmargin":
                    options.VerticalMargin = ParseInt(i++);
                    break;
                case "--output":
                    options.OutputPath = GetNextArg(i++);
                    break;
                default:
                    Console.Error.WriteLine($"Unknown argument: {args[i]}");
                    return true;
            }
            i++;
        }

        // Default output path
        if (string.IsNullOrEmpty(options.OutputPath))
        {
            options.OutputPath = Path.Combine(
                Path.GetDirectoryName(options.InputPath) ?? ".",
                Path.GetFileNameWithoutExtension(options.InputPath) + ".ass");
        }

        // Run conversion
        try
        {
            Console.WriteLine($"Input:            {options.InputPath}");
            Console.WriteLine($"Output:           {options.OutputPath}");
            Console.WriteLine($"Mode:             {options.Mode}");
            Console.WriteLine($"Resolution:       {options.ResX}x{options.ResY}");
            Console.WriteLine($"Base resolution:  {options.BaseResX}x{options.BaseResY}");
            Console.WriteLine($"Font size:        {options.FontSize}");

            var result = SrtConverter.Convert(options);
            Console.WriteLine($"Parsed:           {result.SubtitleCount} subtitle blocks");
            Console.WriteLine("Conversion complete.");
            return false;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"ERROR: {ex.Message}");
            Console.Error.WriteLine(ex.ToString());
            return true;
        }

        string GetNextArg(int index) => index + 1 < args.Length ? args[index + 1] : throw new ArgumentException($"Missing value for {args[index]}");
        int ParseInt(int index)
        {
            var val = index + 1 < args.Length ? args[index + 1] : throw new ArgumentException($"Missing value for {args[index]}");
            return int.TryParse(val, out var n) ? n : throw new ArgumentException($"Invalid value for {args[index]}: {val}");
        }
    }

    static void PrintUsage()
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
}
