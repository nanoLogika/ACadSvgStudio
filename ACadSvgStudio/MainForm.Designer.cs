namespace ACadSvgStudio {
	partial class MainForm {
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			splitContainer1 = new SplitContainer();
			tabControl = new TabControl();
			mainGroupTabPage = new TabPage();
			scalesTabPage = new TabPage();
			cssTabPage = new TabPage();
			splitContainer2 = new SplitContainer();
			propertyGrid = new PropertyGrid();
			menuStrip = new MenuStrip();
			fileToolStripMenuItem = new ToolStripMenuItem();
			openToolStripMenuItem = new ToolStripMenuItem();
			saveToolStripMenuItem1 = new ToolStripMenuItem();
			saveAsToolStripMenuItem = new ToolStripMenuItem();
			toolStripSeparator1 = new ToolStripSeparator();
			exitToolStripMenuItem = new ToolStripMenuItem();
			searchToolStripMenuItem = new ToolStripMenuItem();
			_findToolStripMenuItem = new ToolStripMenuItem();
			viewToolStripMenuItem = new ToolStripMenuItem();
			centerToFitToolStripMenuItem = new ToolStripMenuItem();
			toolStripSeparator3 = new ToolStripSeparator();
			propertyGridToolStripMenuItem = new ToolStripMenuItem();
			toolStripSeparator5 = new ToolStripSeparator();
			collapseAllToolStripMenuItem = new ToolStripMenuItem();
			expandAllToolStripMenuItem = new ToolStripMenuItem();
			contentToolStripMenuItem = new ToolStripMenuItem();
			_restorePreviousToolStripMenuItem = new ToolStripMenuItem();
			extrasToolStripMenuItem = new ToolStripMenuItem();
			removeStylesToolStripMenuItem = new ToolStripMenuItem();
			toolStripSeparator2 = new ToolStripSeparator();
			showDeveloperToolsToolStripMenuItem = new ToolStripMenuItem();
			converisonInfoToolStripMenuItem = new ToolStripMenuItem();
			_showConversionLogToolStripMenuItem = new ToolStripMenuItem();
			openFileDialog = new OpenFileDialog();
			saveFileDialog = new SaveFileDialog();
			webBrowserContextMenuStrip = new ContextMenuStrip(components);
			centerToFitToolStripMenuItem1 = new ToolStripMenuItem();
			toolStripSeparator4 = new ToolStripSeparator();
			showDeveloperToolsToolStripMenuItem1 = new ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			tabControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
			splitContainer2.Panel2.SuspendLayout();
			splitContainer2.SuspendLayout();
			menuStrip.SuspendLayout();
			webBrowserContextMenuStrip.SuspendLayout();
			SuspendLayout();
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.Location = new Point(0, 24);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(tabControl);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(splitContainer2);
			splitContainer1.Size = new Size(1204, 738);
			splitContainer1.SplitterDistance = 456;
			splitContainer1.TabIndex = 1;
			// 
			// tabControl
			// 
			tabControl.Controls.Add(mainGroupTabPage);
			tabControl.Controls.Add(scalesTabPage);
			tabControl.Controls.Add(cssTabPage);
			tabControl.Dock = DockStyle.Fill;
			tabControl.Location = new Point(0, 0);
			tabControl.Name = "tabControl";
			tabControl.SelectedIndex = 0;
			tabControl.Size = new Size(456, 738);
			tabControl.TabIndex = 0;
			// 
			// mainGroupTabPage
			// 
			mainGroupTabPage.Location = new Point(4, 24);
			mainGroupTabPage.Name = "mainGroupTabPage";
			mainGroupTabPage.Padding = new Padding(3);
			mainGroupTabPage.Size = new Size(448, 710);
			mainGroupTabPage.TabIndex = 0;
			mainGroupTabPage.Text = "Main Group";
			mainGroupTabPage.UseVisualStyleBackColor = true;
			// 
			// scalesTabPage
			// 
			scalesTabPage.Location = new Point(4, 24);
			scalesTabPage.Name = "scalesTabPage";
			scalesTabPage.Size = new Size(448, 710);
			scalesTabPage.TabIndex = 2;
			scalesTabPage.Text = "Scales";
			scalesTabPage.UseVisualStyleBackColor = true;
			// 
			// cssTabPage
			// 
			cssTabPage.Location = new Point(4, 24);
			cssTabPage.Name = "cssTabPage";
			cssTabPage.Padding = new Padding(3);
			cssTabPage.Size = new Size(448, 710);
			cssTabPage.TabIndex = 1;
			cssTabPage.Text = "CSS for Preview";
			cssTabPage.UseVisualStyleBackColor = true;
			// 
			// splitContainer2
			// 
			splitContainer2.Dock = DockStyle.Fill;
			splitContainer2.Location = new Point(0, 0);
			splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel2
			// 
			splitContainer2.Panel2.Controls.Add(propertyGrid);
			splitContainer2.Size = new Size(744, 738);
			splitContainer2.SplitterDistance = 455;
			splitContainer2.TabIndex = 0;
			// 
			// propertyGrid
			// 
			propertyGrid.Dock = DockStyle.Fill;
			propertyGrid.Location = new Point(0, 0);
			propertyGrid.Name = "propertyGrid";
			propertyGrid.Size = new Size(285, 738);
			propertyGrid.TabIndex = 0;
			// 
			// menuStrip
			// 
			menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, searchToolStripMenuItem, viewToolStripMenuItem, contentToolStripMenuItem, extrasToolStripMenuItem, converisonInfoToolStripMenuItem });
			menuStrip.Location = new Point(0, 0);
			menuStrip.Name = "menuStrip";
			menuStrip.Size = new Size(1204, 24);
			menuStrip.TabIndex = 2;
			menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem1, saveAsToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new Size(37, 20);
			fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			openToolStripMenuItem.Name = "openToolStripMenuItem";
			openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
			openToolStripMenuItem.Size = new Size(249, 22);
			openToolStripMenuItem.Text = "Open";
			openToolStripMenuItem.Click += eventOpenClick;
			// 
			// saveToolStripMenuItem1
			// 
			saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
			saveToolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.S;
			saveToolStripMenuItem1.Size = new Size(249, 22);
			saveToolStripMenuItem1.Text = "Save";
			saveToolStripMenuItem1.Click += eventSaveSvgGroupClick;
			// 
			// saveAsToolStripMenuItem
			// 
			saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			saveAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
			saveAsToolStripMenuItem.Size = new Size(249, 22);
			saveAsToolStripMenuItem.Text = "Save as ...";
			saveAsToolStripMenuItem.Click += eventSaveSvgGroupAsClick;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(246, 6);
			// 
			// exitToolStripMenuItem
			// 
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
			exitToolStripMenuItem.Size = new Size(249, 22);
			exitToolStripMenuItem.Text = "Exit";
			exitToolStripMenuItem.Click += eventExit_Click;
			// 
			// searchToolStripMenuItem
			// 
			searchToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _findToolStripMenuItem });
			searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			searchToolStripMenuItem.Size = new Size(54, 20);
			searchToolStripMenuItem.Text = "Search";
			// 
			// _findToolStripMenuItem
			// 
			_findToolStripMenuItem.Name = "_findToolStripMenuItem";
			_findToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
			_findToolStripMenuItem.Size = new Size(139, 22);
			_findToolStripMenuItem.Text = "Find";
			_findToolStripMenuItem.Click += eventSearch_Click;
			// 
			// viewToolStripMenuItem
			// 
			viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { centerToFitToolStripMenuItem, toolStripSeparator3, propertyGridToolStripMenuItem, toolStripSeparator5, collapseAllToolStripMenuItem, expandAllToolStripMenuItem });
			viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			viewToolStripMenuItem.Size = new Size(44, 20);
			viewToolStripMenuItem.Text = "View";
			// 
			// centerToFitToolStripMenuItem
			// 
			centerToFitToolStripMenuItem.Name = "centerToFitToolStripMenuItem";
			centerToFitToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;
			centerToFitToolStripMenuItem.Size = new Size(181, 22);
			centerToFitToolStripMenuItem.Text = "Center to Fit";
			centerToFitToolStripMenuItem.Click += centerToFitToolStripMenuItem_Click;
			// 
			// toolStripSeparator3
			// 
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new Size(178, 6);
			// 
			// propertyGridToolStripMenuItem
			// 
			propertyGridToolStripMenuItem.Checked = true;
			propertyGridToolStripMenuItem.CheckOnClick = true;
			propertyGridToolStripMenuItem.CheckState = CheckState.Checked;
			propertyGridToolStripMenuItem.Name = "propertyGridToolStripMenuItem";
			propertyGridToolStripMenuItem.Size = new Size(181, 22);
			propertyGridToolStripMenuItem.Text = "Property Grid";
			propertyGridToolStripMenuItem.CheckedChanged += propertyGridToolStripMenuItem_CheckedChanged;
			// 
			// toolStripSeparator5
			// 
			toolStripSeparator5.Name = "toolStripSeparator5";
			toolStripSeparator5.Size = new Size(178, 6);
			// 
			// collapseAllToolStripMenuItem
			// 
			collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
			collapseAllToolStripMenuItem.Size = new Size(181, 22);
			collapseAllToolStripMenuItem.Text = "Collapse All";
			collapseAllToolStripMenuItem.Click += collapseAllToolStripMenuItem_Click;
			// 
			// expandAllToolStripMenuItem
			// 
			expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
			expandAllToolStripMenuItem.Size = new Size(181, 22);
			expandAllToolStripMenuItem.Text = "Expand All";
			expandAllToolStripMenuItem.Click += expandAllToolStripMenuItem_Click;
			// 
			// contentToolStripMenuItem
			// 
			contentToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _restorePreviousToolStripMenuItem });
			contentToolStripMenuItem.Name = "contentToolStripMenuItem";
			contentToolStripMenuItem.Size = new Size(62, 20);
			contentToolStripMenuItem.Text = "Content";
			// 
			// _restorePreviousToolStripMenuItem
			// 
			_restorePreviousToolStripMenuItem.Name = "_restorePreviousToolStripMenuItem";
			_restorePreviousToolStripMenuItem.Size = new Size(161, 22);
			_restorePreviousToolStripMenuItem.Text = "Restore Previous";
			_restorePreviousToolStripMenuItem.Click += flipContent_Click;
			// 
			// extrasToolStripMenuItem
			// 
			extrasToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { removeStylesToolStripMenuItem, toolStripSeparator2, showDeveloperToolsToolStripMenuItem });
			extrasToolStripMenuItem.Name = "extrasToolStripMenuItem";
			extrasToolStripMenuItem.Size = new Size(50, 20);
			extrasToolStripMenuItem.Text = "Extras";
			// 
			// removeStylesToolStripMenuItem
			// 
			removeStylesToolStripMenuItem.Name = "removeStylesToolStripMenuItem";
			removeStylesToolStripMenuItem.Size = new Size(214, 22);
			removeStylesToolStripMenuItem.Text = "Remove Styles";
			removeStylesToolStripMenuItem.Click += eventRemoveStyles_Click;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(211, 6);
			// 
			// showDeveloperToolsToolStripMenuItem
			// 
			showDeveloperToolsToolStripMenuItem.Name = "showDeveloperToolsToolStripMenuItem";
			showDeveloperToolsToolStripMenuItem.ShortcutKeys = Keys.F12;
			showDeveloperToolsToolStripMenuItem.Size = new Size(214, 22);
			showDeveloperToolsToolStripMenuItem.Text = "Show Developer Tools";
			showDeveloperToolsToolStripMenuItem.Click += showDeveloperToolsToolStripMenuItem_Click;
			// 
			// converisonInfoToolStripMenuItem
			// 
			converisonInfoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _showConversionLogToolStripMenuItem });
			converisonInfoToolStripMenuItem.Name = "converisonInfoToolStripMenuItem";
			converisonInfoToolStripMenuItem.Size = new Size(103, 20);
			converisonInfoToolStripMenuItem.Text = "Converison Info";
			// 
			// _showConversionLogToolStripMenuItem
			// 
			_showConversionLogToolStripMenuItem.Name = "_showConversionLogToolStripMenuItem";
			_showConversionLogToolStripMenuItem.ShortcutKeys = Keys.F2;
			_showConversionLogToolStripMenuItem.Size = new Size(208, 22);
			_showConversionLogToolStripMenuItem.Text = "Show Conversion Log";
			_showConversionLogToolStripMenuItem.Click += showConversionLog_Click;
			// 
			// openFileDialog
			// 
			openFileDialog.Filter = "DWG files|*.dwg|Files with SVG group|*.svg";
			openFileDialog.FileOk += openFileDialog_FileOk;
			// 
			// saveFileDialog
			// 
			saveFileDialog.Filter = "Normal SVG file|*.svg|Files with SVG group|*.svg";
			saveFileDialog.FileOk += saveFileDialog_FileOk;
			// 
			// webBrowserContextMenuStrip
			// 
			webBrowserContextMenuStrip.Items.AddRange(new ToolStripItem[] { centerToFitToolStripMenuItem1, toolStripSeparator4, showDeveloperToolsToolStripMenuItem1 });
			webBrowserContextMenuStrip.Name = "webBrowserContextMenuStrip";
			webBrowserContextMenuStrip.Size = new Size(190, 54);
			// 
			// centerToFitToolStripMenuItem1
			// 
			centerToFitToolStripMenuItem1.Name = "centerToFitToolStripMenuItem1";
			centerToFitToolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.E;
			centerToFitToolStripMenuItem1.Size = new Size(189, 22);
			centerToFitToolStripMenuItem1.Text = "Center to Fit";
			centerToFitToolStripMenuItem1.Click += centerToFitToolStripMenuItem1_Click;
			// 
			// toolStripSeparator4
			// 
			toolStripSeparator4.Name = "toolStripSeparator4";
			toolStripSeparator4.Size = new Size(186, 6);
			// 
			// showDeveloperToolsToolStripMenuItem1
			// 
			showDeveloperToolsToolStripMenuItem1.Name = "showDeveloperToolsToolStripMenuItem1";
			showDeveloperToolsToolStripMenuItem1.Size = new Size(189, 22);
			showDeveloperToolsToolStripMenuItem1.Text = "Show Developer Tools";
			showDeveloperToolsToolStripMenuItem1.Click += showDeveloperToolsToolStripMenuItem1_Click;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1204, 762);
			Controls.Add(splitContainer1);
			Controls.Add(menuStrip);
			MainMenuStrip = menuStrip;
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "ACadSvgStudio";
			FormClosing += MainForm_FormClosing;
			FormClosed += MainForm_FormClosed;
			Load += MainForm_Load;
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			tabControl.ResumeLayout(false);
			splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
			splitContainer2.ResumeLayout(false);
			menuStrip.ResumeLayout(false);
			menuStrip.PerformLayout();
			webBrowserContextMenuStrip.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private SplitContainer splitContainer1;
		private MenuStrip menuStrip;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem exitToolStripMenuItem;
		private SplitContainer splitContainer2;
		private PropertyGrid propertyGrid;
		private ToolStripSeparator toolStripSeparator1;
		private TabControl tabControl;
		private TabPage mainGroupTabPage;
		private TabPage cssTabPage;
		private TabPage scalesTabPage;
		private ToolStripMenuItem openToolStripMenuItem;
		private ToolStripMenuItem saveToolStripMenuItem1;
		private ToolStripMenuItem saveAsToolStripMenuItem;
		private ToolStripMenuItem contentToolStripMenuItem;
		private ToolStripMenuItem _restorePreviousToolStripMenuItem;
		private ToolStripMenuItem converisonInfoToolStripMenuItem;
		private ToolStripMenuItem _showConversionLogToolStripMenuItem;
		private ToolStripMenuItem viewToolStripMenuItem;
		private ToolStripMenuItem centerToFitToolStripMenuItem;
		private ToolStripMenuItem extrasToolStripMenuItem;
		private ToolStripMenuItem removeStylesToolStripMenuItem;
		private ToolStripMenuItem searchToolStripMenuItem;
		private ToolStripMenuItem _findToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem showDeveloperToolsToolStripMenuItem;
		private OpenFileDialog openFileDialog;
		private SaveFileDialog saveFileDialog;
		private ToolStripMenuItem propertyGridToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator3;
		private ContextMenuStrip webBrowserContextMenuStrip;
		private ToolStripMenuItem centerToFitToolStripMenuItem1;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripMenuItem showDeveloperToolsToolStripMenuItem1;
		private ToolStripMenuItem collapseAllToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator5;
		private ToolStripMenuItem expandAllToolStripMenuItem;
	}
}