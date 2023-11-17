namespace ACadSvgStudio {
	partial class ConversionInfoForm {
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
			_splitContainer = new SplitContainer();
			_conversionLogScintilla = new ScintillaNET.Scintilla();
			_conversionLoglabel = new Label();
			_occurringEntitiesListBox = new ListBox();
			_entitiesLabel = new Label();
			((System.ComponentModel.ISupportInitialize)_splitContainer).BeginInit();
			_splitContainer.Panel1.SuspendLayout();
			_splitContainer.Panel2.SuspendLayout();
			_splitContainer.SuspendLayout();
			SuspendLayout();
			// 
			// _splitContainer
			// 
			_splitContainer.Dock = DockStyle.Fill;
			_splitContainer.Location = new Point(0, 0);
			_splitContainer.Name = "_splitContainer";
			// 
			// _splitContainer.Panel1
			// 
			_splitContainer.Panel1.Controls.Add(_conversionLogScintilla);
			_splitContainer.Panel1.Controls.Add(_conversionLoglabel);
			// 
			// _splitContainer.Panel2
			// 
			_splitContainer.Panel2.Controls.Add(_occurringEntitiesListBox);
			_splitContainer.Panel2.Controls.Add(_entitiesLabel);
			_splitContainer.Size = new Size(946, 717);
			_splitContainer.SplitterDistance = 623;
			_splitContainer.TabIndex = 0;
			// 
			// _conversionLogScintilla
			// 
			_conversionLogScintilla.AutoCMaxHeight = 9;
			_conversionLogScintilla.BiDirectionality = ScintillaNET.BiDirectionalDisplayType.Disabled;
			_conversionLogScintilla.BorderStyle = ScintillaNET.BorderStyle.FixedSingle;
			_conversionLogScintilla.CaretLineBackColor = Color.Black;
			_conversionLogScintilla.CaretLineVisible = true;
			_conversionLogScintilla.Dock = DockStyle.Fill;
			_conversionLogScintilla.LexerName = null;
			_conversionLogScintilla.Location = new Point(0, 15);
			_conversionLogScintilla.Name = "_conversionLogScintilla";
			_conversionLogScintilla.ScrollWidth = 49;
			_conversionLogScintilla.Size = new Size(623, 702);
			_conversionLogScintilla.TabIndents = true;
			_conversionLogScintilla.TabIndex = 1;
			_conversionLogScintilla.Text = "scintilla1";
			_conversionLogScintilla.UseRightToLeftReadingLayout = false;
			_conversionLogScintilla.WrapMode = ScintillaNET.WrapMode.None;
			// 
			// _conversionLoglabel
			// 
			_conversionLoglabel.AutoSize = true;
			_conversionLoglabel.Dock = DockStyle.Top;
			_conversionLoglabel.Location = new Point(0, 0);
			_conversionLoglabel.Name = "_conversionLoglabel";
			_conversionLoglabel.Size = new Size(90, 15);
			_conversionLoglabel.TabIndex = 0;
			_conversionLoglabel.Text = "Conversion Log";
			// 
			// _occurringEntitiesListBox
			// 
			_occurringEntitiesListBox.Dock = DockStyle.Fill;
			_occurringEntitiesListBox.FormattingEnabled = true;
			_occurringEntitiesListBox.ItemHeight = 15;
			_occurringEntitiesListBox.Location = new Point(0, 15);
			_occurringEntitiesListBox.Name = "_occurringEntitiesListBox";
			_occurringEntitiesListBox.Size = new Size(319, 702);
			_occurringEntitiesListBox.TabIndex = 1;
			// 
			// _entitiesLabel
			// 
			_entitiesLabel.AutoSize = true;
			_entitiesLabel.Dock = DockStyle.Top;
			_entitiesLabel.Location = new Point(0, 0);
			_entitiesLabel.Name = "_entitiesLabel";
			_entitiesLabel.Size = new Size(101, 15);
			_entitiesLabel.TabIndex = 0;
			_entitiesLabel.Text = "Occurring Entities";
			// 
			// ConversionInfoForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(946, 717);
			Controls.Add(_splitContainer);
			Name = "ConversionInfoForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Conversion Info:";
			_splitContainer.Panel1.ResumeLayout(false);
			_splitContainer.Panel1.PerformLayout();
			_splitContainer.Panel2.ResumeLayout(false);
			_splitContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)_splitContainer).EndInit();
			_splitContainer.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private SplitContainer _splitContainer;
		private ScintillaNET.Scintilla _conversionLogScintilla;
		private Label _conversionLoglabel;
		private ListBox _occurringEntitiesListBox;
		private Label _entitiesLabel;
	}
}