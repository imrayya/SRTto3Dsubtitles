namespace ConvertSRTto3DASS;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitle;
    private GroupBox grpFile;
    private Label lblInputFile;
    private TextBox txtInputFile;
    private Button btnBrowseInput;
    private Label lblOutputFile;
    private TextBox txtOutputFile;
    private Button btnBrowseOutput;
    private GroupBox grpMode;
    private Label lblMode;
    private ComboBox cmbMode;
    private GroupBox grpResolution;
    private Label lblResX;
    private NumericUpDown nudResX;
    private Label lblResY;
    private NumericUpDown nudResY;
    private Label lblBaseResX;
    private NumericUpDown nudBaseResX;
    private Label lblBaseResY;
    private NumericUpDown nudBaseResY;
    private GroupBox grpSettings;
    private Label lblFontSize;
    private NumericUpDown nudFontSize;
    private Label lblOffsetX;
    private NumericUpDown nudOffsetX;
    private Label lblBottomOffset;
    private NumericUpDown nudBottomOffset;
    private Label lblSbsSideMargin;
    private NumericUpDown nudSbsSideMargin;
    private Label lblOuTopMargin;
    private NumericUpDown nudOuTopMargin;
    private Label lblVerticalMargin;
    private NumericUpDown nudVerticalMargin;
    private Button btnConvert;
    private TextBox txtStatus;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        var segoe = new Font("Segoe UI", 9f);

        SuspendLayout();

        // ---- Title ----
        lblTitle = new Label
        {
            Text = "SRT to 3D Subtitles",
            Font = new Font("Segoe UI", 14f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 30),
            Location = new Point(20, 15),
            AutoSize = true
        };

        // ---- File section ----
        grpFile = new GroupBox
        {
            Text = "Files",
            Font = segoe,
            Location = new Point(20, 60),
            Size = new Size(1040, 80)
        };
        lblInputFile = new Label { Text = "Input SRT:", Font = segoe, Location = new Point(15, 25), AutoSize = true };
        txtInputFile = new TextBox { Location = new Point(100, 22), Size = new Size(780, 23), ReadOnly = true };
        btnBrowseInput = new Button { Text = "Browse...", Location = new Point(890, 20), Size = new Size(90, 25), Font = segoe, FlatStyle = FlatStyle.Flat };
        lblOutputFile = new Label { Text = "Output ASS:", Font = segoe, Location = new Point(15, 55), AutoSize = true };
        txtOutputFile = new TextBox { Location = new Point(100, 52), Size = new Size(780, 23), ReadOnly = true };
        btnBrowseOutput = new Button { Text = "Browse...", Location = new Point(890, 50), Size = new Size(90, 25), Font = segoe, FlatStyle = FlatStyle.Flat };
        grpFile.Controls.AddRange([lblInputFile, txtInputFile, btnBrowseInput, lblOutputFile, txtOutputFile, btnBrowseOutput]);

        // ---- Mode section ----
        grpMode = new GroupBox
        {
            Text = "3D Mode",
            Font = segoe,
            Location = new Point(20, 150),
            Size = new Size(320, 60)
        };
        lblMode = new Label { Text = "Format:", Font = segoe, Location = new Point(15, 22), AutoSize = true };
        cmbMode = new ComboBox
        {
            Location = new Point(85, 19),
            Size = new Size(210, 25),
            Font = segoe,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbMode.Items.AddRange(["SBS (Side-by-Side)", "OU (Over-Under)", "RG (Red-Green)"]);
        grpMode.Controls.AddRange([lblMode, cmbMode]);

        // ---- Resolution section ----
        grpResolution = new GroupBox
        {
            Text = "Resolution",
            Font = segoe,
            Location = new Point(355, 150),
            Size = new Size(320, 60)
        };
        lblResX = new Label { Text = "Output:", Font = segoe, Location = new Point(15, 22), AutoSize = true };
        nudResX = new NumericUpDown { Location = new Point(65, 20), Size = new Size(80, 23), Minimum = 1, Maximum = 10000, Value = 1280 };
        lblResY = new Label { Text = "x", Font = segoe, Location = new Point(150, 22), AutoSize = true };
        nudResY = new NumericUpDown { Location = new Point(165, 20), Size = new Size(80, 23), Minimum = 1, Maximum = 10000, Value = 720 };
        lblBaseResX = new Label { Text = "Base:", Font = segoe, Location = new Point(255, 22), AutoSize = true };
        nudBaseResX = new NumericUpDown { Location = new Point(290, 20), Size = new Size(80, 23), Minimum = 1, Maximum = 10000, Value = 1280 };
        lblBaseResY = new Label { Text = "x", Font = segoe, Location = new Point(375, 22), AutoSize = true };
        nudBaseResY = new NumericUpDown { Location = new Point(390, 20), Size = new Size(80, 23), Minimum = 1, Maximum = 10000, Value = 720 };
        grpResolution.Controls.AddRange([lblResX, nudResX, lblResY, nudResY, lblBaseResX, nudBaseResX, lblBaseResY, nudBaseResY]);

        // ---- Settings section ----
        grpSettings = new GroupBox
        {
            Text = "Settings",
            Font = segoe,
            Location = new Point(20, 220),
            Size = new Size(1040, 120)
        };

        var col1 = 15;
        var col2 = 160;
        var col3 = 310;
        var col4 = 460;
        var col5 = 610;
        var col6 = 760;
        var row = 25;
        var rowStep = 30;

        lblFontSize = new Label { Text = "Font Size:", Font = segoe, Location = new Point(col1, row), AutoSize = true };
        nudFontSize = new NumericUpDown { Location = new Point(col2, row - 2), Size = new Size(80, 23), Minimum = 1, Maximum = 200, Value = 16 };
        lblOffsetX = new Label { Text = "Offset X:", Font = segoe, Location = new Point(col3, row), AutoSize = true };
        nudOffsetX = new NumericUpDown { Location = new Point(col4, row - 2), Size = new Size(80, 23), Minimum = 0, Maximum = 1000, Value = 4 };
        lblBottomOffset = new Label { Text = "Bottom Offset:", Font = segoe, Location = new Point(col5, row), AutoSize = true };
        nudBottomOffset = new NumericUpDown { Location = new Point(col6, row - 2), Size = new Size(80, 23), Minimum = 0, Maximum = 1000, Value = 18 };
        row += rowStep;

        lblSbsSideMargin = new Label { Text = "SBS Margin:", Font = segoe, Location = new Point(col1, row), AutoSize = true };
        nudSbsSideMargin = new NumericUpDown { Location = new Point(col2, row - 2), Size = new Size(80, 23), Minimum = 0, Maximum = 5000, Value = 640 };
        lblOuTopMargin = new Label { Text = "OU Margin:", Font = segoe, Location = new Point(col3, row), AutoSize = true };
        nudOuTopMargin = new NumericUpDown { Location = new Point(col4, row - 2), Size = new Size(80, 23), Minimum = 0, Maximum = 5000, Value = 385 };
        lblVerticalMargin = new Label { Text = "V Margin:", Font = segoe, Location = new Point(col5, row), AutoSize = true };
        nudVerticalMargin = new NumericUpDown { Location = new Point(col6, row - 2), Size = new Size(80, 23), Minimum = 0, Maximum = 5000, Value = 25 };

        grpSettings.Controls.AddRange([
            lblFontSize, nudFontSize, lblOffsetX, nudOffsetX, lblBottomOffset, nudBottomOffset,
            lblSbsSideMargin, nudSbsSideMargin, lblOuTopMargin, nudOuTopMargin, lblVerticalMargin, nudVerticalMargin
        ]);

        // ---- Convert button ----
        btnConvert = new Button
        {
            Text = "Convert",
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Location = new Point(20, 355),
            Size = new Size(140, 36),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(0, 120, 215),
            ForeColor = Color.White,
            FlatAppearance = { BorderSize = 0 }
        };

        // ---- Status ----
        txtStatus = new TextBox
        {
            Location = new Point(20, 405),
            Size = new Size(1040, 180),
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 8.5f),
            BackColor = Color.FromArgb(245, 245, 245)
        };

        // ---- Form ----
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1080, 610);
        MinimumSize = new Size(1100, 650);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        AllowDrop = true;
        Text = "SRT to 3D Subtitles";
        Font = segoe;
        StartPosition = FormStartPosition.CenterScreen;

        Controls.AddRange([lblTitle, grpFile, grpMode, grpResolution, grpSettings, btnConvert, txtStatus]);

        ResumeLayout(false);
        PerformLayout();
    }
}
