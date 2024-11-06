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
		private void InitializeComponent()
		{
			resolveDefsCheckBox = new CheckBox();
			label1 = new Label();
			filenameTextBox = new TextBox();
			browseButton = new Button();
			_saveFileDialog = new SaveFileDialog();
			exportButton = new Button();
			checkedListBox = new CheckedListBox();
			cancelButton = new Button();
			SuspendLayout();
			// 
			// resolveDefsCheckBox
			// 
			resolveDefsCheckBox.AutoSize = true;
			resolveDefsCheckBox.Location = new Point(12, 56);
			resolveDefsCheckBox.Name = "resolveDefsCheckBox";
			resolveDefsCheckBox.Size = new Size(92, 19);
			resolveDefsCheckBox.TabIndex = 0;
			resolveDefsCheckBox.Text = "Resolve Defs";
			resolveDefsCheckBox.UseVisualStyleBackColor = true;
			resolveDefsCheckBox.CheckedChanged += resolveDefsCheckBox_CheckedChanged;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(12, 9);
			label1.Name = "label1";
			label1.Size = new Size(55, 15);
			label1.TabIndex = 1;
			label1.Text = "Filename";
			// 
			// filenameTextBox
			// 
			filenameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			filenameTextBox.Location = new Point(12, 27);
			filenameTextBox.Name = "filenameTextBox";
			filenameTextBox.Size = new Size(481, 23);
			filenameTextBox.TabIndex = 2;
			filenameTextBox.TextChanged += filenameTextBox_TextChanged;
			// 
			// browseButton
			// 
			browseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			browseButton.Location = new Point(499, 27);
			browseButton.Name = "browseButton";
			browseButton.Size = new Size(75, 23);
			browseButton.TabIndex = 3;
			browseButton.Text = "Browse";
			browseButton.UseVisualStyleBackColor = true;
			browseButton.Click += browseButton_Click;
			// 
			// _saveFileDialog
			// 
			_saveFileDialog.Filter = "SVG files|*.svg|SVG group files|*.g.svg";
			_saveFileDialog.FilterIndex = 2;
			// 
			// exportButton
			// 
			exportButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			exportButton.DialogResult = DialogResult.OK;
			exportButton.Location = new Point(418, 415);
			exportButton.Name = "exportButton";
			exportButton.Size = new Size(75, 23);
			exportButton.TabIndex = 4;
			exportButton.Text = "Export";
			exportButton.UseVisualStyleBackColor = true;
			// 
			// checkedListBox
			// 
			checkedListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			checkedListBox.CheckOnClick = true;
			checkedListBox.FormattingEnabled = true;
			checkedListBox.IntegralHeight = false;
			checkedListBox.Location = new Point(12, 81);
			checkedListBox.Name = "checkedListBox";
			checkedListBox.Size = new Size(562, 328);
			checkedListBox.TabIndex = 5;
			// 
			// cancelButton
			// 
			cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(499, 415);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(75, 23);
			cancelButton.TabIndex = 6;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			// 
			// ExportSVGForm
			// 
			AcceptButton = exportButton;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = cancelButton;
			ClientSize = new Size(586, 450);
			Controls.Add(cancelButton);
			Controls.Add(checkedListBox);
			Controls.Add(exportButton);
			Controls.Add(browseButton);
			Controls.Add(filenameTextBox);
			Controls.Add(label1);
			Controls.Add(resolveDefsCheckBox);
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

		private CheckBox resolveDefsCheckBox;
		private Label label1;
		private TextBox filenameTextBox;
		private Button browseButton;
		private SaveFileDialog _saveFileDialog;
		private Button exportButton;
		private CheckedListBox checkedListBox;
		private Button cancelButton;
	}
}