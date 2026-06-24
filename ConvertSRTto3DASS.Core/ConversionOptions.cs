namespace ConvertSRTto3DASS;

public class ConversionOptions
{
    public string InputPath { get; set; } = "";
    public string OutputPath { get; set; } = "";

    public string Mode { get; set; } = "sbs"; // sbs, ou, rg

    public int ResX { get; set; } = 1280;
    public int ResY { get; set; } = 720;
    public int BaseResX { get; set; } = 1280;
    public int BaseResY { get; set; } = 720;

    public int FontSize { get; set; } = 16;
    public int OffsetX { get; set; } = 4;
    public int BottomOffset { get; set; } = 18;
    public int SbsSideMargin { get; set; } = 640;
    public int OuTopMargin { get; set; } = 385;
    public int VerticalMargin { get; set; } = 25;
}
