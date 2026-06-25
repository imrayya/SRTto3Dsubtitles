namespace ConvertSRTto3DASS;

internal static class Program
{
    static void Main(string[] args)
    {
        var hadError = Cli.Run(args);
        if (hadError)
            Console.ReadLine();
    }
}
