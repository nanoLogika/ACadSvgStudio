namespace ACadSvgStudio.CommandLine {
	partial class CommandLineForm {
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
			panel1 = new Panel();
			commandTextBox = new TextBox();
			executeButton = new Button();
			outputTextBox = new TextBox();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// panel1
			// 
			panel1.Controls.Add(commandTextBox);
			panel1.Controls.Add(executeButton);
			panel1.Dock = DockStyle.Bottom;
			panel1.Location = new Point(0, 334);
			panel1.Name = "panel1";
			panel1.Size = new Size(484, 27);
			panel1.TabIndex = 0;
			// 
			// commandTextBox
			// 
			commandTextBox.Dock = DockStyle.Fill;
			commandTextBox.Location = new Point(0, 0);
			commandTextBox.Name = "commandTextBox";
			commandTextBox.Size = new Size(409, 23);
			commandTextBox.TabIndex = 0;
			commandTextBox.KeyDown += commandTextBox_KeyDown;
			// 
			// executeButton
			// 
			executeButton.Dock = DockStyle.Right;
			executeButton.Location = new Point(409, 0);
			executeButton.Name = "executeButton";
			executeButton.Size = new Size(75, 27);
			executeButton.TabIndex = 1;
			executeButton.Text = "Execute";
			executeButton.UseVisualStyleBackColor = true;
			executeButton.Click += executeButton_Click;
			// 
			// outputTextBox
			// 
			outputTextBox.Dock = DockStyle.Fill;
			outputTextBox.Location = new Point(0, 0);
			outputTextBox.Multiline = true;
			outputTextBox.Name = "outputTextBox";
			outputTextBox.ReadOnly = true;
			outputTextBox.ScrollBars = ScrollBars.Vertical;
			outputTextBox.Size = new Size(484, 334);
			outputTextBox.TabIndex = 1;
			// 
			// CommandLineForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(484, 361);
			Controls.Add(outputTextBox);
			Controls.Add(panel1);
			Name = "CommandLineForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Command Line";
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Panel panel1;
		private TextBox commandTextBox;
		private Button executeButton;
		public TextBox outputTextBox;
	}
}