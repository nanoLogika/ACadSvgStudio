
namespace ACadSvgStudio {
	partial class ExportSVGForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent() {
            _resolveDefsCheckBox = new CheckBox();
            _filenameLabel = new Label();
            _filenameTextBox = new TextBox();
            _browseButton = new Button();
            _saveFileDialog = new SaveFileDialog();
            _exportButton = new Button();
            _defsCheckedListBox = new CheckedListBox();
            _cancelButton = new Button();
            _directoryLabel = new Label();
            _directoryTextBox = new TextBox();
            _addExportToCurrentBatchCheckBox = new CheckBox();
            _exportAndOpenButton = new Button();
            SuspendLayout();
            // 
            // _resolveDefsCheckBox
            // 
            _resolveDefsCheckBox.AutoSize = true;
            _resolveDefsCheckBox.Location = new Point(12, 126);
            _resolveDefsCheckBox.Name = "_resolveDefsCheckBox";
            _resolveDefsCheckBox.Size = new Size(92, 19);
            _resolveDefsCheckBox.TabIndex = 0;
            _resolveDefsCheckBox.Text = "Resolve Defs";
            _resolveDefsCheckBox.UseVisualStyleBackColor = true;
            _resolveDefsCheckBox.CheckedChanged += eventResolveDefsCheckBox_CheckedChanged;
            // 
            // _filenameLabel
            // 
            _filenameLabel.AutoSize = true;
            _filenameLabel.Location = new Point(12, 60);
            _filenameLabel.Name = "_filenameLabel";
            _filenameLabel.Size = new Size(55, 15);
            _filenameLabel.TabIndex = 1;
            _filenameLabel.Text = "Filename";
            // 
            // _filenameTextBox
            // 
            _filenameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _filenameTextBox.Location = new Point(12, 78);
            _filenameTextBox.Name = "_filenameTextBox";
            _filenameTextBox.Size = new Size(481, 23);
            _filenameTextBox.TabIndex = 2;
            _filenameTextBox.TextChanged += eventFilenameTextBox_TextChanged;
            // 
            // _browseButton
            // 
            _browseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _browseButton.Location = new Point(499, 77);
            _browseButton.Name = "_browseButton";
            _browseButton.Size = new Size(75, 23);
            _browseButton.TabIndex = 3;
            _browseButton.Text = "Browse";
            _browseButton.UseVisualStyleBackColor = true;
            _browseButton.Click += browseButton_Click;
            // 
            // _saveFileDialog
            // 
            _saveFileDialog.CheckPathExists = false;
            _saveFileDialog.Filter = "SVG group files|*.g.svg";
            _saveFileDialog.FilterIndex = 2;
            _saveFileDialog.SupportMultiDottedExtensions = true;
            // 
            // _exportButton
            // 
            _exportButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _exportButton.DialogResult = DialogResult.OK;
            _exportButton.Location = new Point(418, 472);
            _exportButton.Name = "_exportButton";
            _exportButton.Size = new Size(75, 23);
            _exportButton.TabIndex = 4;
            _exportButton.Text = "E&xport";
            _exportButton.UseVisualStyleBackColor = true;
            _exportButton.Click += eventExport_Click;
            // 
            // _defsCheckedListBox
            // 
            _defsCheckedListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _defsCheckedListBox.CheckOnClick = true;
            _defsCheckedListBox.FormattingEnabled = true;
            _defsCheckedListBox.IntegralHeight = false;
            _defsCheckedListBox.Location = new Point(12, 151);
            _defsCheckedListBox.Name = "_defsCheckedListBox";
            _defsCheckedListBox.Size = new Size(562, 315);
            _defsCheckedListBox.TabIndex = 5;
            // 
            // _cancelButton
            // 
            _cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _cancelButton.DialogResult = DialogResult.Cancel;
            _cancelButton.Location = new Point(499, 472);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new Size(75, 23);
            _cancelButton.TabIndex = 6;
            _cancelButton.Text = "Cancel";
            _cancelButton.UseVisualStyleBackColor = true;
            // 
            // _directoryLabel
            // 
            _directoryLabel.AutoSize = true;
            _directoryLabel.Location = new Point(12, 9);
            _directoryLabel.Name = "_directoryLabel";
            _directoryLabel.Size = new Size(55, 15);
            _directoryLabel.TabIndex = 1;
            _directoryLabel.Text = "Directory";
            // 
            // _directoryTextBox
            // 
            _directoryTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _directoryTextBox.Location = new Point(12, 27);
            _directoryTextBox.Name = "_directoryTextBox";
            _directoryTextBox.ReadOnly = true;
            _directoryTextBox.Size = new Size(562, 23);
            _directoryTextBox.TabIndex = 2;
            _directoryTextBox.TextChanged += eventFilenameTextBox_TextChanged;
            // 
            // _addExportToCurrentBatchCheckBox
            // 
            _addExportToCurrentBatchCheckBox.AutoSize = true;
            _addExportToCurrentBatchCheckBox.Location = new Point(12, 476);
            _addExportToCurrentBatchCheckBox.Name = "_addExportToCurrentBatchCheckBox";
            _addExportToCurrentBatchCheckBox.Size = new Size(178, 19);
            _addExportToCurrentBatchCheckBox.TabIndex = 7;
            _addExportToCurrentBatchCheckBox.Text = "Add Export to Current Batch ";
            _addExportToCurrentBatchCheckBox.UseVisualStyleBackColor = true;
            // 
            // _exportAndOpenButton
            // 
            _exportAndOpenButton.DialogResult = DialogResult.OK;
            _exportAndOpenButton.Enabled = false;
            _exportAndOpenButton.Location = new Point(298, 472);
            _exportAndOpenButton.Name = "_exportAndOpenButton";
            _exportAndOpenButton.Size = new Size(114, 23);
            _exportAndOpenButton.TabIndex = 8;
            _exportAndOpenButton.Text = "Export and &Open";
            _exportAndOpenButton.UseVisualStyleBackColor = true;
            _exportAndOpenButton.Click += eventExportAndOpen_Click;
            // 
            // ExportSVGForm
            // 
            AcceptButton = _exportButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = _cancelButton;
            ClientSize = new Size(586, 507);
            Controls.Add(_exportAndOpenButton);
            Controls.Add(_addExportToCurrentBatchCheckBox);
            Controls.Add(_cancelButton);
            Controls.Add(_defsCheckedListBox);
            Controls.Add(_exportButton);
            Controls.Add(_browseButton);
            Controls.Add(_directoryTextBox);
            Controls.Add(_filenameTextBox);
            Controls.Add(_directoryLabel);
            Controls.Add(_filenameLabel);
            Controls.Add(_resolveDefsCheckBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExportSVGForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Export SVG";
            Load += ExportSVGForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox _resolveDefsCheckBox;
		private Label _filenameLabel;
		private TextBox _filenameTextBox;
		private Button _browseButton;
		private SaveFileDialog _saveFileDialog;
		private Button _exportButton;
		private CheckedListBox _defsCheckedListBox;
		private Button _cancelButton;
        private Label _directoryLabel;
        private TextBox _directoryTextBox;
        private CheckBox _addExportToCurrentBatchCheckBox;
        private Button _exportAndOpenButton;
    }
}