using System;
using System.IO;
using System.Windows.Forms;
using ConvertSRTto3DASS;

namespace ConvertSRTto3DASSGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (cmbMode.Items.Count > 0)
                cmbMode.SelectedIndex = 0;

            UpdateModeUI();
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
                string mode = cmbMode.SelectedItem?.ToString() ?? "sbs";

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
                int fontSize = (int)nudFontSize.Value;
                int offsetX = (int)nudOffsetX.Value;

                AppendStatus("Starting conversion...");
                AppendStatus("Input: " + inputPath);
                AppendStatus("Output: " + outputPath);
                AppendStatus("Mode: " + mode);
                AppendStatus("Resolution: " + resX + "x" + resY);
                AppendStatus("Font Size: " + fontSize);
                AppendStatus("OffsetX: " + offsetX);

                ConvertSRTto3DASS.Converter.Main(new[]
                {
                    txtInputFile.Text,
                    "--mode", GetSelectedModeValue(),
                    "--resx", nudResX.Value.ToString(),
                    "--resy", nudResY.Value.ToString(),
                    "--fontsize", nudFontSize.Value.ToString(),
                    "--offsetx", nudOffsetX.Value.ToString(),
                    "--output", txtOutputFile.Text
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
            string mode = cmbMode.SelectedItem?.ToString() ?? "sbs";
            bool isRg = mode.Equals("rg", StringComparison.OrdinalIgnoreCase);

            nudOffsetX.Enabled = isRg;
            lblOffsetX.Enabled = isRg;
        }

        private void AppendStatus(string message)
        {
            if (txtStatus.TextLength > 0)
                txtStatus.AppendText(Environment.NewLine);

            txtStatus.AppendText(message);
        }
    }
}