using System.IO;

namespace ConvertSRTto3DASS;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        comboBox_Mode.SelectedIndex = 0;
        UpdateModeUI();

        // Drag-and-drop
        DragEnter += (s, e) =>
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        };

        DragDrop += (s, e) =>
        {
            var files = (string[])e.Data!.GetData(DataFormats.FileDrop)!;
            if (files.Length > 0 && files[0].EndsWith(".srt", StringComparison.OrdinalIgnoreCase))
            {
                textBox_InputFile.Text = files[0];
                if (string.IsNullOrWhiteSpace(textBox_OutputFile.Text))
                    textBox_OutputFile.Text = Path.ChangeExtension(files[0], ".ass");
                AppendStatus("Dropped: " + files[0]);
            }
        };
    }

    private void button_BrowseInput_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "SRT files (*.srt)|*.srt|All files (*.*)|*.*",
            Title = "Select input SRT file"
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            textBox_InputFile.Text = ofd.FileName;

            if (string.IsNullOrWhiteSpace(textBox_OutputFile.Text))
                textBox_OutputFile.Text = Path.ChangeExtension(ofd.FileName, ".ass");

            AppendStatus("Selected input file: " + ofd.FileName);
        }
    }

    private void button_BrowseOutput_Click(object sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog
        {
            Filter = "ASS files (*.ass)|*.ass|All files (*.*)|*.*",
            Title = "Select output ASS file",
            DefaultExt = "ass"
        };

        if (!string.IsNullOrWhiteSpace(textBox_InputFile.Text))
        {
            sfd.FileName = Path.GetFileNameWithoutExtension(textBox_InputFile.Text) + ".ass";
            sfd.InitialDirectory = Path.GetDirectoryName(textBox_InputFile.Text);
        }

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            textBox_OutputFile.Text = sfd.FileName;
            AppendStatus("Selected output file: " + sfd.FileName);
        }
    }

    private void comboBox_Mode_SelectedIndexChanged(object sender, EventArgs e) => UpdateModeUI();

    private string GetSelectedModeValue()
    {
        var selected = comboBox_Mode.SelectedItem?.ToString() ?? "sbs";
        var spaceIndex = selected.IndexOf(' ');
        return spaceIndex > 0 ? selected[..spaceIndex].Trim().ToLowerInvariant() : selected.Trim().ToLowerInvariant();
    }

    private void button_Convert_Click(object sender, EventArgs e)
    {
        try
        {
            textBox_Status.Clear();

            var inputPath = textBox_InputFile.Text.Trim();
            var outputPath = textBox_OutputFile.Text.Trim();
            var mode = GetSelectedModeValue();

            if (string.IsNullOrWhiteSpace(inputPath))
            {
                MessageBox.Show("Please select an input SRT file.", "Missing Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(inputPath))
            {
                MessageBox.Show("The selected input file does not exist.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                outputPath = Path.ChangeExtension(inputPath, ".ass");
                textBox_OutputFile.Text = outputPath;
            }

            var options = new ConversionOptions
            {
                InputPath = inputPath,
                OutputPath = outputPath,
                Mode = mode,
                ResX = (int)numericUpDown_ResX.Value,
                ResY = (int)numericUpDown_ResY.Value,
                BaseResX = (int)numericUpDown_BaseResX.Value,
                BaseResY = (int)numericUpDown_BaseResY.Value,
                FontSize = (int)numericUpDown_FontSize.Value,
                OffsetX = (int)numericUpDown_OffsetX.Value,
                BottomOffset = (int)numericUpDown_BottomOffset.Value,
                SbsSideMargin = (int)numericUpDown_SbsSideMargin.Value,
                OuTopMargin = (int)numericUpDown_OuTopMargin.Value,
                VerticalMargin = (int)numericUpDown_VerticalMargin.Value
            };

            AppendStatus("Starting conversion...");
            AppendStatus($"Input:            {options.InputPath}");
            AppendStatus($"Output:           {options.OutputPath}");
            AppendStatus($"Mode:             {options.Mode}");
            AppendStatus($"Resolution:       {options.ResX}x{options.ResY}");
            AppendStatus($"Base resolution:  {options.BaseResX}x{options.BaseResY}");
            AppendStatus($"Font size:        {options.FontSize}");

            var result = SrtConverter.Convert(options);

            AppendStatus($"Parsed:           {result.SubtitleCount} subtitle blocks");
            AppendStatus("Conversion complete.");
            MessageBox.Show("Conversion complete.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            AppendStatus($"ERROR: {ex.Message}");
            MessageBox.Show(ex.Message, "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateModeUI()
    {
        var mode = GetSelectedModeValue();

        numericUpDown_OffsetX.Enabled = mode == "rg";
        label_OffsetX.Enabled = mode == "rg";
        numericUpDown_BottomOffset.Enabled = mode == "rg";
        label_BottomOffset.Enabled = mode == "rg";
        numericUpDown_SbsSideMargin.Enabled = mode == "sbs";
        label_SbsSideMargin.Enabled = mode == "sbs";
        numericUpDown_OuTopMargin.Enabled = mode == "ou";
        label_OuTopMargin.Enabled = mode == "ou";
    }

    private void AppendStatus(string message)
    {
        if (textBox_Status.TextLength > 0)
            textBox_Status.AppendText(Environment.NewLine);

        textBox_Status.AppendText(message);
    }
}
