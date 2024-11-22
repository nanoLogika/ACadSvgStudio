namespace ACadSvgStudio {
	partial class AboutForm {
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
			titleLabel = new Label();
			label2 = new Label();
			label3 = new Label();
			listBox = new ListBox();
			okButton = new Button();
			detailsLabel = new Label();
			linkLabel = new LinkLabel();
			companyLinkLabel = new LinkLabel();
			versionLabel = new Label();
			licenseLabel = new Label();
			projectLinkLabel = new LinkLabel();
			SuspendLayout();
			// 
			// titleLabel
			// 
			titleLabel.AutoSize = true;
			titleLabel.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
			titleLabel.ForeColor = Color.DodgerBlue;
			titleLabel.Location = new Point(12, 9);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new Size(158, 37);
			titleLabel.TabIndex = 0;
			titleLabel.Text = "About Form";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(12, 60);
			label2.Name = "label2";
			label2.Size = new Size(148, 15);
			label2.TabIndex = 1;
			label2.Text = "© 2023 nanoLogika GmbH";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(12, 104);
			label3.Name = "label3";
			label3.Size = new Size(98, 15);
			label3.TabIndex = 2;
			label3.Text = "Installed Libraries";
			// 
			// listBox
			// 
			listBox.FormattingEnabled = true;
			listBox.IntegralHeight = false;
			listBox.ItemHeight = 15;
			listBox.Location = new Point(12, 134);
			listBox.Name = "listBox";
			listBox.Size = new Size(560, 94);
			listBox.TabIndex = 3;
			listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
			// 
			// okButton
			// 
			okButton.Location = new Point(497, 326);
			okButton.Name = "okButton";
			okButton.Size = new Size(75, 23);
			okButton.TabIndex = 4;
			okButton.Text = "OK";
			okButton.UseVisualStyleBackColor = true;
			okButton.Click += okButton_Click;
			// 
			// detailsLabel
			// 
			detailsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			detailsLabel.Location = new Point(12, 260);
			detailsLabel.Name = "detailsLabel";
			detailsLabel.Size = new Size(560, 63);
			detailsLabel.TabIndex = 5;
			detailsLabel.Text = "Product Details";
			// 
			// linkLabel
			// 
			linkLabel.AutoSize = true;
			linkLabel.Location = new Point(12, 231);
			linkLabel.Name = "linkLabel";
			linkLabel.Size = new Size(60, 15);
			linkLabel.TabIndex = 6;
			linkLabel.TabStop = true;
			linkLabel.Text = "Link Label";
			linkLabel.LinkClicked += linkLabel_LinkClicked;
			// 
			// companyLinkLabel
			// 
			companyLinkLabel.AutoSize = true;
			companyLinkLabel.Location = new Point(12, 75);
			companyLinkLabel.Name = "companyLinkLabel";
			companyLinkLabel.Size = new Size(59, 15);
			companyLinkLabel.TabIndex = 6;
			companyLinkLabel.TabStop = true;
			companyLinkLabel.Text = "Company";
			companyLinkLabel.LinkClicked += companyLinkLabel_LinkClicked;
			// 
			// versionLabel
			// 
			versionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			versionLabel.Location = new Point(426, 9);
			versionLabel.Name = "versionLabel";
			versionLabel.RightToLeft = RightToLeft.No;
			versionLabel.Size = new Size(146, 37);
			versionLabel.TabIndex = 1;
			versionLabel.Text = "Version";
			// 
			// licenseLabel
			// 
			licenseLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			licenseLabel.AutoSize = true;
			licenseLabel.Location = new Point(425, 60);
			licenseLabel.Name = "licenseLabel";
			licenseLabel.Size = new Size(46, 15);
			licenseLabel.TabIndex = 1;
			licenseLabel.Text = "License";
			// 
			// projectLinkLabel
			// 
			projectLinkLabel.AutoSize = true;
			projectLinkLabel.Location = new Point(426, 104);
			projectLinkLabel.Name = "projectLinkLabel";
			projectLinkLabel.Size = new Size(102, 15);
			projectLinkLabel.TabIndex = 6;
			projectLinkLabel.TabStop = true;
			projectLinkLabel.Text = "Project on GitHub";
			projectLinkLabel.LinkClicked += projectLinkLabel_LinkClicked;
			// 
			// AboutForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(584, 361);
			Controls.Add(projectLinkLabel);
			Controls.Add(companyLinkLabel);
			Controls.Add(linkLabel);
			Controls.Add(detailsLabel);
			Controls.Add(okButton);
			Controls.Add(listBox);
			Controls.Add(label3);
			Controls.Add(licenseLabel);
			Controls.Add(versionLabel);
			Controls.Add(label2);
			Controls.Add(titleLabel);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "AboutForm";
			ShowIcon = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "About Form";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label titleLabel;
		private Label label2;
		private Label label3;
		private ListBox listBox;
		private Button okButton;
		private Label detailsLabel;
		private LinkLabel linkLabel;
		private LinkLabel companyLinkLabel;
		private Label versionLabel;
		private Label licenseLabel;
		private LinkLabel projectLinkLabel;
	}
}