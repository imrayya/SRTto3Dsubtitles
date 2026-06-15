using ConvertSRTto3DASS;
using System;
using System.IO;
using System.Windows.Forms;
using static ConvertSRTto3DASS.Converter;

namespace ConvertSRTto3DASSGUI
{
    public partial class Form1 : Form
    {
        // Keep these in sync with Converter.cs defaults
        private const int DefaultResX = Converter.ConverterDefaults.ResX;//1280
        private const int DefaultResY = 720;
        private const int DefaultBaseResX = 1280;
        private const int DefaultBaseResY = 720;
        private const int DefaultFontSize = 16;
        private const int DefaultOffsetX = 4;
        private const int DefaultBottomOffset = 18;
        private const int DefaultSbsSideMargin = 192;
        private const int DefaultOuTopMargin = 154;
        private const int DefaultVerticalMargin = 10;

        public Form1()
        {
            InitializeComponent();
            InitializeDefaults();

            if (cmbMode.Items.Count > 0)
                cmbMode.SelectedIndex = 0;

            UpdateModeUI();
        }

        private void InitializeDefaults()
        {
            txtInputFile.Text = string.Empty;
            txtOutputFile.Text = string.Empty;

            nudResX.Value = ConverterDefaults.ResX;
            nudResY.Value = ConverterDefaults.ResY;
            nudBaseResX.Value = ConverterDefaults.BaseResX;
            nudBaseResY.Value = ConverterDefaults.BaseResY;
            nudFontSize.Value = ConverterDefaults.FontSize;
            nudOffsetX.Value = ConverterDefaults.OffsetX;
            nudBottomOffset.Value = ConverterDefaults.BottomOffset;
            nudSbsSideMargin.Value = ConverterDefaults.SbsSideMargin;
            nudOuTopMargin.Value = ConverterDefaults.OuTopMargin;
            nudVerticalMargin.Value = ConverterDefaults.VerticalMargin;

            switch (ConverterDefaults.DefaultMode.ToLowerInvariant())
            {
                case "sbs":
                    cmbMode.SelectedIndex = 0;
                    break;
                case "ou":
                    cmbMode.SelectedIndex = 1;
                    break;
                case "rg":
                    cmbMode.SelectedIndex = 2;
                    break;
                default:
                    cmbMode.SelectedIndex = 0;
                    break;
            }
        }

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "SRT files (*.srt)|*.srt|All files (*.*)|*.*";
                ofd.Title = "Select input SRT file";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtInputFile.Text = ofd.FileName;

                    if (string.IsNullOrWhiteSpace(txtOutputFile.Text))
                    {
                        txtOutputFile.Text = Path.ChangeExtension(ofd.FileName, ".ass");
                    }

                    AppendStatus("Selected input file: " + ofd.FileName);
                }
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "ASS files (*.ass)|*.ass|All files (*.*)|*.*";
                sfd.Title = "Select output ASS file";
                sfd.DefaultExt = "ass";

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
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateModeUI();
        }

        private string GetSelectedModeValue()
        {
            string selected = cmbMode.SelectedItem?.ToString() ?? "sbs";
            int spaceIndex = selected.IndexOf(' ');

            if (spaceIndex > 0)
                return selected.Substring(0, spaceIndex).Trim().ToLowerInvariant();

            return selected.Trim().ToLowerInvariant();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                txtStatus.Clear();

                string inputPath = txtInputFile.Text.Trim();
                string outputPath = txtOutputFile.Text.Trim();
                string mode = GetSelectedModeValue();

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

                int resX = (int)nudResX.Value;
                int resY = (int)nudResY.Value;
                int baseResX = (int)nudBaseResX.Value;
                int baseResY = (int)nudBaseResY.Value;
                int fontSize = (int)nudFontSize.Value;
                int offsetX = (int)nudOffsetX.Value;
                int bottomOffset = (int)nudBottomOffset.Value;
                int sbsSideMargin = (int)nudSbsSideMargin.Value;
                int ouTopMargin = (int)nudOuTopMargin.Value;
                int verticalMargin = (int)nudVerticalMargin.Value;

                AppendStatus("Starting conversion...");
                AppendStatus("Input: " + inputPath);
                AppendStatus("Output: " + outputPath);
                AppendStatus("Mode: " + mode);
                AppendStatus("Resolution: " + resX + "x" + resY);
                AppendStatus("Base Resolution: " + baseResX + "x" + baseResY);
                AppendStatus("Font Size: " + fontSize);
                AppendStatus("OffsetX: " + offsetX);
                AppendStatus("BottomOffset: " + bottomOffset);
                AppendStatus("SbsSideMargin: " + sbsSideMargin);
                AppendStatus("OuTopMargin: " + ouTopMargin);
                AppendStatus("VerticalMargin: " + verticalMargin);

                Converter.Main(new[]
                {
                    inputPath,
                    "--mode", mode,
                    "--resx", resX.ToString(),
                    "--resy", resY.ToString(),
                    "--baseresx", baseResX.ToString(),
                    "--baseresy", baseResY.ToString(),
                    "--fontsize", fontSize.ToString(),
                    "--offsetx", offsetX.ToString(),
                    "--bottomoffset", bottomOffset.ToString(),
                    "--sbssidemargin", sbsSideMargin.ToString(),
                    "--outopmargin", ouTopMargin.ToString(),
                    "--verticalmargin", verticalMargin.ToString(),
                    "--output", outputPath
                });

                AppendStatus("Conversion complete.");
                MessageBox.Show("Conversion complete.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendStatus("ERROR: " + ex.Message);
                MessageBox.Show(ex.Message, "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateModeUI()
        {
            string mode = GetSelectedModeValue();

            bool isSbs = mode == "sbs";
            bool isOu = mode == "ou";
            bool isRg = mode == "rg";

            nudOffsetX.Enabled = isRg;
            lblOffsetX.Enabled = isRg;

            nudBottomOffset.Enabled = isRg;
            lblBottomOffset.Enabled = isRg;

            nudSbsSideMargin.Enabled = isSbs;
            lblSbsSideMargin.Enabled = isSbs;

            nudOuTopMargin.Enabled = isOu;
            lblOuTopMargin.Enabled = isOu;

            nudVerticalMargin.Enabled = true;
            lblVerticalMargin.Enabled = true;
        }

        private void AppendStatus(string message)
        {
            if (txtStatus.TextLength > 0)
                txtStatus.AppendText(Environment.NewLine);

            txtStatus.AppendText(message);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}