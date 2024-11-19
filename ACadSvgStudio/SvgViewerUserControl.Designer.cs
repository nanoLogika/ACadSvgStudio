namespace ACadSvgStudio {
	partial class SvgViewerUserControl {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			contextMenuStrip = new ContextMenuStrip(components);
			centerToFitToolStripMenuItem = new ToolStripMenuItem();
			contextMenuStrip.SuspendLayout();
			SuspendLayout();
			// 
			// contextMenuStrip
			// 
			contextMenuStrip.Items.AddRange(new ToolStripItem[] { centerToFitToolStripMenuItem });
			contextMenuStrip.Name = "contextMenuStrip";
			contextMenuStrip.Size = new Size(140, 26);
			// 
			// centerToFitToolStripMenuItem
			// 
			centerToFitToolStripMenuItem.Name = "centerToFitToolStripMenuItem";
			centerToFitToolStripMenuItem.Size = new Size(139, 22);
			centerToFitToolStripMenuItem.Text = "Center to Fit";
			centerToFitToolStripMenuItem.Click += centerToFitToolStripMenuItem_Click;
			// 
			// SvgViewerUserControl
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ContextMenuStrip = contextMenuStrip;
			DoubleBuffered = true;
			Name = "SvgViewerUserControl";
			Size = new Size(690, 540);
			Paint += SvgViewerUserControl_Paint;
			MouseDown += SvgViewerUserControl_MouseDown;
			MouseMove += SvgViewerUserControl_MouseMove;
			MouseUp += SvgViewerUserControl_MouseUp;
			Resize += SvgViewerUserControl_Resize;
			contextMenuStrip.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private ContextMenuStrip contextMenuStrip;
		private ToolStripMenuItem centerToFitToolStripMenuItem;
	}
}
