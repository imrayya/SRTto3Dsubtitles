namespace ConvertSRTto3DASS;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private Label label_Title;
    private GroupBox groupBox_File;
    private Label label_InputFile;
    private TextBox textBox_InputFile;
    private Button button_BrowseInput;
    private Label label_OutputFile;
    private TextBox textBox_OutputFile;
    private Button button_BrowseOutput;
    private GroupBox groupBox_Mode;
    private Label label_Mode;
    private ComboBox comboBox_Mode;
    private GroupBox groupBox_Resolution;
    private Label label_ResX;
    private NumericUpDown numericUpDown_ResX;
    private Label label_ResY;
    private NumericUpDown numericUpDown_ResY;
    private Label label_BaseResX;
    private NumericUpDown numericUpDown_BaseResX;
    private Label label_BaseResY;
    private NumericUpDown numericUpDown_BaseResY;
    private GroupBox groupBox_Settings;
    private Label label_FontSize;
    private NumericUpDown numericUpDown_FontSize;
    private Label label_OffsetX;
    private NumericUpDown numericUpDown_OffsetX;
    private Label label_BottomOffset;
    private NumericUpDown numericUpDown_BottomOffset;
    private Label label_SbsSideMargin;
    private NumericUpDown numericUpDown_SbsSideMargin;
    private Label label_OuTopMargin;
    private NumericUpDown numericUpDown_OuTopMargin;
    private Label label_VerticalMargin;
    private NumericUpDown numericUpDown_VerticalMargin;
    private Button button_Convert;
    private TextBox textBox_Status;

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
        label_Title = new Label
        {
            Text = "SRT to 3D Subtitles",
            Font = new Font("Segoe UI", 14f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 30),
            Location = new Point(20, 15),
            AutoSize = true
        };

        // ---- File section ----
        groupBox_File = new GroupBox
        {
            Text = "Files",
            Font = segoe,
            Location = new Point(20, 60),
            Size = new Size(1040, 80)
        };
        label_InputFile = new Label { Text = "Input SRT:", Font = segoe, Location = new Point(15, 25), AutoSize = true };
        textBox_InputFile = new TextBox { Location = new Point(100, 22), Size = new Size(780, 23), ReadOnly = true };
        button_BrowseInput = new Button { Text = "Browse...", Location = new Point(890, 20), Size = new Size(90, 25), Font = segoe, FlatStyle = FlatStyle.Flat };
        label_OutputFile = new Label { Text = "Output ASS:", Font = segoe, Location = new Point(15, 55), AutoSize = true };
        textBox_OutputFile = new TextBox { Location = new Point(100, 52), Size = new Size(780, 23), ReadOnly = true };
        button_BrowseOutput = new Button { Text = "Browse...", Location = new Point(890, 50), Size = new Size(90, 25), Font = segoe, FlatStyle = FlatStyle.Flat };
        groupBox_File.Controls.AddRange([label_InputFile, textBox_InputFile, button_BrowseInput, label_OutputFile, textBox_OutputFile, button_BrowseOutput]);

        // ---- Mode section ----
        groupBox_Mode = new GroupBox
        {
            Text = "3D Mode",
            Font = segoe,
            Location = new Point(20, 150),
            Size = new Size(320, 60)
        };
        label_Mode = new Label { Text = "Format:", Font = segoe, Location = new Point(15, 22), AutoSize = true };
        comboBox_Mode = new ComboBox
        {
            Location = new Point(85, 19),
            Size = new Size(210, 25),
            Font = segoe,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        comboBox_Mode.Items.AddRange(["SBS (Side-by-Side)", "OU (Over-Under)", "RG (Red-Green)"]);
        groupBox_Mode.Controls.AddRange([label_Mode, comboBox_Mode]);

        // ---- Resolution section ----
        groupBox_Resolution = new GroupBox
        {
            Text = "Resolution",
            Font = segoe,
            Location = new Point(355, 150),
            Size = new Size(320, 60)
        };
        label_ResX = new Label { Text = "Output:", Font = segoe, Location = new Point(15, 22), AutoSize = true };
        numericUpDown_ResX = new NumericUpDown { Location = new Point(65, 20), Size = new Size(80, 23), Minimum = 1, Maximum = 10000, Value = 1280 };
        label_ResY = new Label { Text = "x", Font = segoe, Location = new Point(150, 22), AutoSize = true };
        numericUpDown_ResY = new NumericUpDown { Location = new Point(165, 20), Size = new Size(80, 23), Minimum = 1, Maximum = 10000, Value = 720 };
        label_BaseResX = new Label { Text = "Base:", Font = segoe, Location = new Point(255, 22), AutoSize = true };
        numericUpDown_BaseResX = new NumericUpDown { Location = new Point(290, 20), Size = new Size(80, 23), Minimum = 1, Maximum = 10000, Value = 1280 };
        label_BaseResY = new Label { Text = "x", Font = segoe, Location = new Point(375, 22), AutoSize = true };
        numericUpDown_BaseResY = new NumericUpDown { Location = new Point(390, 20), Size = new Size(80, 23), Minimum = 1, Maximum = 10000, Value = 720 };
        groupBox_Resolution.Controls.AddRange([label_ResX, numericUpDown_ResX, label_ResY, numericUpDown_ResY, label_BaseResX, numericUpDown_BaseResX, label_BaseResY, numericUpDown_BaseResY]);

        // ---- Settings section ----
        groupBox_Settings = new GroupBox
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

        label_FontSize = new Label { Text = "Font Size:", Font = segoe, Location = new Point(col1, row), AutoSize = true };
        numericUpDown_FontSize = new NumericUpDown { Location = new Point(col2, row - 2), Size = new Size(80, 23), Minimum = 1, Maximum = 200, Value = 16 };
        label_OffsetX = new Label { Text = "Offset X:", Font = segoe, Location = new Point(col3, row), AutoSize = true };
        numericUpDown_OffsetX = new NumericUpDown { Location = new Point(col4, row - 2), Size = new Size(80, 23), Minimum = 0, Maximum = 1000, Value = 4 };
        label_BottomOffset = new Label { Text = "Bottom Offset:", Font = segoe, Location = new Point(col5, row), AutoSize = true };
        numericUpDown_BottomOffset = new NumericUpDown { Location = new Point(col6, row - 2), Size = new Size(80, 23), Minimum = 0, Maximum = 1000, Value = 18 };
        row += rowStep;

        label_SbsSideMargin = new Label { Text = "SBS Margin:", Font = segoe, Location = new Point(col1, row), AutoSize = true };
        numericUpDown_SbsSideMargin = new NumericUpDown { Location = new Point(col2, row - 2), Size = new Size(80, 23), Minimum = 0, Maximum = 5000, Value = 640 };
        label_OuTopMargin = new Label { Text = "OU Margin:", Font = segoe, Location = new Point(col3, row), AutoSize = true };
        numericUpDown_OuTopMargin = new NumericUpDown { Location = new Point(col4, row - 2), Size = new Size(80, 23), Minimum = 0, Maximum = 5000, Value = 385 };
        label_VerticalMargin = new Label { Text = "V Margin:", Font = segoe, Location = new Point(col5, row), AutoSize = true };
        numericUpDown_VerticalMargin = new NumericUpDown { Location = new Point(col6, row - 2), Size = new Size(80, 23), Minimum = 0, Maximum = 5000, Value = 25 };

        groupBox_Settings.Controls.AddRange([
            label_FontSize, numericUpDown_FontSize, label_OffsetX, numericUpDown_OffsetX, label_BottomOffset, numericUpDown_BottomOffset,
            label_SbsSideMargin, numericUpDown_SbsSideMargin, label_OuTopMargin, numericUpDown_OuTopMargin, label_VerticalMargin, numericUpDown_VerticalMargin
        ]);

        // ---- Convert button ----
        button_Convert = new Button
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
        textBox_Status = new TextBox
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

        Controls.AddRange([label_Title, groupBox_File, groupBox_Mode, groupBox_Resolution, groupBox_Settings, button_Convert, textBox_Status]);

        ResumeLayout(false);
        PerformLayout();
    }
}
