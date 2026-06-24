using System.IO;

namespace ConvertSRTto3DASS;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        cmbMode.SelectedIndex = 0;
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
                txtInputFile.Text = files[0];
                if (string.IsNullOrWhiteSpace(txtOutputFile.Text))
                    txtOutputFile.Text = Path.ChangeExtension(files[0], ".ass");
                AppendStatus("Dropped: " + files[0]);
            }
        };
    }

    private void btnBrowseInput_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "SRT files (*.srt)|*.srt|All files (*.*)|*.*",
            Title = "Select input SRT file"
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            txtInputFile.Text = ofd.FileName;

            if (string.IsNullOrWhiteSpace(txtOutputFile.Text))
                txtOutputFile.Text = Path.ChangeExtension(ofd.FileName, ".ass");

            AppendStatus("Selected input file: " + ofd.FileName);
        }
    }

    private void btnBrowseOutput_Click(object sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog
        {
            Filter = "ASS files (*.ass)|*.ass|All files (*.*)|*.*",
            Title = "Select output ASS file",
            DefaultExt = "ass"
        };

        if (!string.IsNullOrWhiteSpace(txtInputFile.Text))
        {
            sfd.FileName = Path.GetFileNameWithoutExtension(txtInputFile.Text) + ".ass";
            sfd.InitialDirectory = Path.GetDirectoryName(txtInputFile.Text);
        }

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            txtOutputFile.Text = sfd.FileName;
            AppendStatus("Selected output file: " + sfd.FileName);
        }
    }

    private void cmbMode_SelectedIndexChanged(object sender, EventArgs e) => UpdateModeUI();

    private string GetSelectedModeValue()
    {
        var selected = cmbMode.SelectedItem?.ToString() ?? "sbs";
        var spaceIndex = selected.IndexOf(' ');
        return spaceIndex > 0 ? selected[..spaceIndex].Trim().ToLowerInvariant() : selected.Trim().ToLowerInvariant();
    }

    private void btnConvert_Click(object sender, EventArgs e)
    {
        try
        {
            txtStatus.Clear();

            var inputPath = txtInputFile.Text.Trim();
            var outputPath = txtOutputFile.Text.Trim();
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
                txtOutputFile.Text = outputPath;
            }

            var options = new ConversionOptions
            {
                InputPath = inputPath,
                OutputPath = outputPath,
                Mode = mode,
                ResX = (int)nudResX.Value,
                ResY = (int)nudResY.Value,
                BaseResX = (int)nudBaseResX.Value,
                BaseResY = (int)nudBaseResY.Value,
                FontSize = (int)nudFontSize.Value,
                OffsetX = (int)nudOffsetX.Value,
                BottomOffset = (int)nudBottomOffset.Value,
                SbsSideMargin = (int)nudSbsSideMargin.Value,
                OuTopMargin = (int)nudOuTopMargin.Value,
                VerticalMargin = (int)nudVerticalMargin.Value
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

        nudOffsetX.Enabled = mode == "rg";
        lblOffsetX.Enabled = mode == "rg";
        nudBottomOffset.Enabled = mode == "rg";
        lblBottomOffset.Enabled = mode == "rg";
        nudSbsSideMargin.Enabled = mode == "sbs";
        lblSbsSideMargin.Enabled = mode == "sbs";
        nudOuTopMargin.Enabled = mode == "ou";
        lblOuTopMargin.Enabled = mode == "ou";
    }

    private void AppendStatus(string message)
    {
        if (txtStatus.TextLength > 0)
            txtStatus.AppendText(Environment.NewLine);

        txtStatus.AppendText(message);
    }
}
