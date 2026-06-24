namespace ConvertSRTto3DASSGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblInputFile;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.Button btnBrowseInput;

        private System.Windows.Forms.Label lblOutputFile;
        private System.Windows.Forms.TextBox txtOutputFile;
        private System.Windows.Forms.Button btnBrowseOutput;

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ComboBox cmbMode;

        private System.Windows.Forms.Label lblResX;
        private System.Windows.Forms.NumericUpDown nudResX;

        private System.Windows.Forms.Label lblResY;
        private System.Windows.Forms.NumericUpDown nudResY;

        private System.Windows.Forms.Label lblBaseResX;
        private System.Windows.Forms.NumericUpDown nudBaseResX;

        private System.Windows.Forms.Label lblBaseResY;
        private System.Windows.Forms.NumericUpDown nudBaseResY;

        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.NumericUpDown nudFontSize;

        private System.Windows.Forms.Label lblOffsetX;
        private System.Windows.Forms.NumericUpDown nudOffsetX;

        private System.Windows.Forms.Label lblBottomOffset;
        private System.Windows.Forms.NumericUpDown nudBottomOffset;

        private System.Windows.Forms.Label lblSbsSideMargin;
        private System.Windows.Forms.NumericUpDown nudSbsSideMargin;

        private System.Windows.Forms.Label lblOuTopMargin;
        private System.Windows.Forms.NumericUpDown nudOuTopMargin;

        private System.Windows.Forms.Label lblVerticalMargin;
        private System.Windows.Forms.NumericUpDown nudVerticalMargin;

        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox txtStatus;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblInputFile = new System.Windows.Forms.Label();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseInput = new System.Windows.Forms.Button();
            this.lblOutputFile = new System.Windows.Forms.Label();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.lblMode = new System.Windows.Forms.Label();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.lblResX = new System.Windows.Forms.Label();
            this.nudResX = new System.Windows.Forms.NumericUpDown();
            this.lblResY = new System.Windows.Forms.Label();
            this.nudResY = new System.Windows.Forms.NumericUpDown();
            this.lblBaseResX = new System.Windows.Forms.Label();
            this.nudBaseResX = new System.Windows.Forms.NumericUpDown();
            this.lblBaseResY = new System.Windows.Forms.Label();
            this.nudBaseResY = new System.Windows.Forms.NumericUpDown();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.nudFontSize = new System.Windows.Forms.NumericUpDown();
            this.lblOffsetX = new System.Windows.Forms.Label();
            this.nudOffsetX = new System.Windows.Forms.NumericUpDown();
            this.lblBottomOffset = new System.Windows.Forms.Label();
            this.nudBottomOffset = new System.Windows.Forms.NumericUpDown();
            this.lblSbsSideMargin = new System.Windows.Forms.Label();
            this.nudSbsSideMargin = new System.Windows.Forms.NumericUpDown();
            this.lblOuTopMargin = new System.Windows.Forms.Label();
            this.nudOuTopMargin = new System.Windows.Forms.NumericUpDown();
            this.lblVerticalMargin = new System.Windows.Forms.Label();
            this.nudVerticalMargin = new System.Windows.Forms.NumericUpDown();
            this.btnConvert = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudResX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudResY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseResX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseResY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBottomOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSbsSideMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuTopMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVerticalMargin)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInputFile
            // 
            this.lblInputFile.AutoSize = true;
            this.lblInputFile.Location = new System.Drawing.Point(24, 28);
            this.lblInputFile.Name = "lblInputFile";
            this.lblInputFile.Size = new System.Drawing.Size(94, 16);
            this.lblInputFile.TabIndex = 0;
            this.lblInputFile.Text = "Input SRT File:";
            // 
            // txtInputFile
            // 
            this.txtInputFile.Location = new System.Drawing.Point(140, 25);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(780, 22);
            this.txtInputFile.TabIndex = 1;
            // 
            // btnBrowseInput
            // 
            this.btnBrowseInput.Location = new System.Drawing.Point(940, 23);
            this.btnBrowseInput.Name = "btnBrowseInput";
            this.btnBrowseInput.Size = new System.Drawing.Size(120, 28);
            this.btnBrowseInput.TabIndex = 2;
            this.btnBrowseInput.Text = "Browse...";
            this.btnBrowseInput.UseVisualStyleBackColor = true;
            this.btnBrowseInput.Click += new System.EventHandler(this.btnBrowseInput_Click);
            // 
            // lblOutputFile
            // 
            this.lblOutputFile.AutoSize = true;
            this.lblOutputFile.Location = new System.Drawing.Point(24, 72);
            this.lblOutputFile.Name = "lblOutputFile";
            this.lblOutputFile.Size = new System.Drawing.Size(103, 16);
            this.lblOutputFile.TabIndex = 3;
            this.lblOutputFile.Text = "Output ASS File:";
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Location = new System.Drawing.Point(140, 69);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(780, 22);
            this.txtOutputFile.TabIndex = 4;
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(940, 67);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(120, 28);
            this.btnBrowseOutput.TabIndex = 5;
            this.btnBrowseOutput.Text = "Browse...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(24, 125);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(65, 16);
            this.lblMode.TabIndex = 6;
            this.lblMode.Text = "3D Mode:";
            // 
            // cmbMode
            // 
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.FormattingEnabled = true;
            this.cmbMode.Items.AddRange(new object[] {
            "sbs (Side-By-Side)",
            "ou (Over-Under)",
            "rg (Red-Green)"});
            this.cmbMode.Location = new System.Drawing.Point(140, 122);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Size = new System.Drawing.Size(180, 24);
            this.cmbMode.TabIndex = 7;
            this.cmbMode.SelectedIndexChanged += new System.EventHandler(this.cmbMode_SelectedIndexChanged);
            // 
            // lblResX
            // 
            this.lblResX.AutoSize = true;
            this.lblResX.Location = new System.Drawing.Point(24, 178);
            this.lblResX.Name = "lblResX";
            this.lblResX.Size = new System.Drawing.Size(46, 16);
            this.lblResX.TabIndex = 8;
            this.lblResX.Text = "Res X:";
            // 
            // nudResX
            // 
            this.nudResX.Location = new System.Drawing.Point(140, 176);
            this.nudResX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudResX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudResX.Name = "nudResX";
            this.nudResX.Size = new System.Drawing.Size(120, 22);
            this.nudResX.TabIndex = 9;
            this.nudResX.Value = new decimal(new int[] {
            1280,
            0,
            0,
            0});
            // 
            // lblResY
            // 
            this.lblResY.AutoSize = true;
            this.lblResY.Location = new System.Drawing.Point(300, 178);
            this.lblResY.Name = "lblResY";
            this.lblResY.Size = new System.Drawing.Size(47, 16);
            this.lblResY.TabIndex = 10;
            this.lblResY.Text = "Res Y:";
            // 
            // nudResY
            // 
            this.nudResY.Location = new System.Drawing.Point(360, 176);
            this.nudResY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudResY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudResY.Name = "nudResY";
            this.nudResY.Size = new System.Drawing.Size(120, 22);
            this.nudResY.TabIndex = 11;
            this.nudResY.Value = new decimal(new int[] {
            720,
            0,
            0,
            0});
            // 
            // lblBaseResX
            // 
            this.lblBaseResX.AutoSize = true;
            this.lblBaseResX.Location = new System.Drawing.Point(520, 178);
            this.lblBaseResX.Name = "lblBaseResX";
            this.lblBaseResX.Size = new System.Drawing.Size(81, 16);
            this.lblBaseResX.TabIndex = 12;
            this.lblBaseResX.Text = "Base Res X:";
            // 
            // nudBaseResX
            // 
            this.nudBaseResX.Location = new System.Drawing.Point(620, 176);
            this.nudBaseResX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudBaseResX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBaseResX.Name = "nudBaseResX";
            this.nudBaseResX.Size = new System.Drawing.Size(120, 22);
            this.nudBaseResX.TabIndex = 13;
            this.nudBaseResX.Value = new decimal(new int[] {
            1280,
            0,
            0,
            0});
            // 
            // lblBaseResY
            // 
            this.lblBaseResY.AutoSize = true;
            this.lblBaseResY.Location = new System.Drawing.Point(760, 178);
            this.lblBaseResY.Name = "lblBaseResY";
            this.lblBaseResY.Size = new System.Drawing.Size(82, 16);
            this.lblBaseResY.TabIndex = 14;
            this.lblBaseResY.Text = "Base Res Y:";
            // 
            // nudBaseResY
            // 
            this.nudBaseResY.Location = new System.Drawing.Point(860, 176);
            this.nudBaseResY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudBaseResY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBaseResY.Name = "nudBaseResY";
            this.nudBaseResY.Size = new System.Drawing.Size(120, 22);
            this.nudBaseResY.TabIndex = 15;
            this.nudBaseResY.Value = new decimal(new int[] {
            720,
            0,
            0,
            0});
            // 
            // lblFontSize
            // 
            this.lblFontSize.AutoSize = true;
            this.lblFontSize.Location = new System.Drawing.Point(24, 228);
            this.lblFontSize.Name = "lblFontSize";
            this.lblFontSize.Size = new System.Drawing.Size(65, 16);
            this.lblFontSize.TabIndex = 16;
            this.lblFontSize.Text = "Font Size:";
            // 
            // nudFontSize
            // 
            this.nudFontSize.Location = new System.Drawing.Point(140, 226);
            this.nudFontSize.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudFontSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFontSize.Name = "nudFontSize";
            this.nudFontSize.Size = new System.Drawing.Size(120, 22);
            this.nudFontSize.TabIndex = 17;
            this.nudFontSize.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // lblOffsetX
            // 
            this.lblOffsetX.AutoSize = true;
            this.lblOffsetX.Location = new System.Drawing.Point(300, 228);
            this.lblOffsetX.Name = "lblOffsetX";
            this.lblOffsetX.Size = new System.Drawing.Size(55, 16);
            this.lblOffsetX.TabIndex = 18;
            this.lblOffsetX.Text = "Offset X:";
            // 
            // nudOffsetX
            // 
            this.nudOffsetX.Location = new System.Drawing.Point(360, 226);
            this.nudOffsetX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudOffsetX.Name = "nudOffsetX";
            this.nudOffsetX.Size = new System.Drawing.Size(120, 22);
            this.nudOffsetX.TabIndex = 19;
            this.nudOffsetX.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // lblBottomOffset
            // 
            this.lblBottomOffset.AutoSize = true;
            this.lblBottomOffset.Location = new System.Drawing.Point(520, 228);
            this.lblBottomOffset.Name = "lblBottomOffset";
            this.lblBottomOffset.Size = new System.Drawing.Size(89, 16);
            this.lblBottomOffset.TabIndex = 20;
            this.lblBottomOffset.Text = "Bottom Offset:";
            // 
            // nudBottomOffset
            // 
            this.nudBottomOffset.Location = new System.Drawing.Point(620, 226);
            this.nudBottomOffset.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudBottomOffset.Name = "nudBottomOffset";
            this.nudBottomOffset.Size = new System.Drawing.Size(120, 22);
            this.nudBottomOffset.TabIndex = 21;
            this.nudBottomOffset.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // lblSbsSideMargin
            // 
            this.lblSbsSideMargin.AutoSize = true;
            this.lblSbsSideMargin.Location = new System.Drawing.Point(24, 278);
            this.lblSbsSideMargin.Name = "lblSbsSideMargin";
            this.lblSbsSideMargin.Size = new System.Drawing.Size(112, 16);
            this.lblSbsSideMargin.TabIndex = 22;
            this.lblSbsSideMargin.Text = "SBS Side Margin:";
            // 
            // nudSbsSideMargin
            // 
            this.nudSbsSideMargin.Location = new System.Drawing.Point(140, 276);
            this.nudSbsSideMargin.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudSbsSideMargin.Name = "nudSbsSideMargin";
            this.nudSbsSideMargin.Size = new System.Drawing.Size(120, 22);
            this.nudSbsSideMargin.TabIndex = 23;
            this.nudSbsSideMargin.Value = new decimal(new int[] {
            192,
            0,
            0,
            0});
            // 
            // lblOuTopMargin
            // 
            this.lblOuTopMargin.AutoSize = true;
            this.lblOuTopMargin.Location = new System.Drawing.Point(300, 278);
            this.lblOuTopMargin.Name = "lblOuTopMargin";
            this.lblOuTopMargin.Size = new System.Drawing.Size(102, 16);
            this.lblOuTopMargin.TabIndex = 24;
            this.lblOuTopMargin.Text = "OU Top Margin:";
            // 
            // nudOuTopMargin
            // 
            this.nudOuTopMargin.Location = new System.Drawing.Point(410, 276);
            this.nudOuTopMargin.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudOuTopMargin.Name = "nudOuTopMargin";
            this.nudOuTopMargin.Size = new System.Drawing.Size(120, 22);
            this.nudOuTopMargin.TabIndex = 25;
            this.nudOuTopMargin.Value = new decimal(new int[] {
            154,
            0,
            0,
            0});
            // 
            // lblVerticalMargin
            // 
            this.lblVerticalMargin.AutoSize = true;
            this.lblVerticalMargin.Location = new System.Drawing.Point(560, 278);
            this.lblVerticalMargin.Name = "lblVerticalMargin";
            this.lblVerticalMargin.Size = new System.Drawing.Size(99, 16);
            this.lblVerticalMargin.TabIndex = 26;
            this.lblVerticalMargin.Text = "Vertical Margin:";
            // 
            // nudVerticalMargin
            // 
            this.nudVerticalMargin.Location = new System.Drawing.Point(670, 276);
            this.nudVerticalMargin.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudVerticalMargin.Name = "nudVerticalMargin";
            this.nudVerticalMargin.Size = new System.Drawing.Size(120, 22);
            this.nudVerticalMargin.TabIndex = 27;
            this.nudVerticalMargin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(27, 329);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(145, 40);
            this.btnConvert.TabIndex = 28;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(27, 392);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(1033, 225);
            this.txtStatus.TabIndex = 29;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 640);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.nudVerticalMargin);
            this.Controls.Add(this.lblVerticalMargin);
            this.Controls.Add(this.nudOuTopMargin);
            this.Controls.Add(this.lblOuTopMargin);
            this.Controls.Add(this.nudSbsSideMargin);
            this.Controls.Add(this.lblSbsSideMargin);
            this.Controls.Add(this.nudBottomOffset);
            this.Controls.Add(this.lblBottomOffset);
            this.Controls.Add(this.nudOffsetX);
            this.Controls.Add(this.lblOffsetX);
            this.Controls.Add(this.nudFontSize);
            this.Controls.Add(this.lblFontSize);
            this.Controls.Add(this.nudBaseResY);
            this.Controls.Add(this.lblBaseResY);
            this.Controls.Add(this.nudBaseResX);
            this.Controls.Add(this.lblBaseResX);
            this.Controls.Add(this.nudResY);
            this.Controls.Add(this.lblResY);
            this.Controls.Add(this.nudResX);
            this.Controls.Add(this.lblResX);
            this.Controls.Add(this.cmbMode);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.txtOutputFile);
            this.Controls.Add(this.lblOutputFile);
            this.Controls.Add(this.btnBrowseInput);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.lblInputFile);
            this.Name = "Form1";
            this.Text = "Convert SRT to 3D ASS";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudResX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudResY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseResX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseResY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBottomOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSbsSideMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuTopMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVerticalMargin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}