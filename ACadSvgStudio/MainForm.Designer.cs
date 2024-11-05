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
			if (_devToolsContext != null)
			{
				_devToolsContext.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent() {
			components = new System.ComponentModel.Container();
			_splitContainer1 = new SplitContainer();
			_tabControl = new TabControl();
			_mainGroupTabPage = new TabPage();
			_scalesTabPage = new TabPage();
			_cssTabPage = new TabPage();
			_splitContainer2 = new SplitContainer();
			_rightTabControl = new TabControl();
			_propertiesTabPage = new TabPage();
			_propertyGrid = new PropertyGrid();
			_defsTabPage = new TabPage();
			_defsTreeView = new TreeView();
			_menuStrip = new MenuStrip();
			_fileToolStripMenuItem = new ToolStripMenuItem();
			_openMenuItem = new ToolStripMenuItem();
			_saveMenuItem = new ToolStripMenuItem();
			_saveAsMenuItem = new ToolStripMenuItem();
			_recentlyOpenedFilesToolStripSeparator = new ToolStripSeparator();
			_recentlyOpenedFilesToolStripMenuItem = new ToolStripMenuItem();
			_toolStripSeparator1 = new ToolStripSeparator();
			_exitMenuItem = new ToolStripMenuItem();
			_editMenuItem = new ToolStripMenuItem();
			_undoMenuItem = new ToolStripMenuItem();
			_redoMenuItem = new ToolStripMenuItem();
			_toolStripSeparator11 = new ToolStripSeparator();
			_cutMenuItem = new ToolStripMenuItem();
			_copyMenuItem = new ToolStripMenuItem();
			_pasteMenuItem = new ToolStripMenuItem();
			_deleteMenuItem = new ToolStripMenuItem();
			_toolStripSeparator13 = new ToolStripSeparator();
			_selectAllMenuItem = new ToolStripMenuItem();
			_searchToolStripMenuItem = new ToolStripMenuItem();
			_quickFindMenuItem = new ToolStripMenuItem();
			_findAndReplaceMenuItem = new ToolStripMenuItem();
			_viewMenuItem = new ToolStripMenuItem();
			_centerToFitMenuItem = new ToolStripMenuItem();
			_toolStripSeparator3 = new ToolStripSeparator();
			_propertyGridToolStripMenuItem = new ToolStripMenuItem();
			_toolStripSeparator5 = new ToolStripSeparator();
			_collapseAllMenuItem = new ToolStripMenuItem();
			_expandAllToolStripMenuItem = new ToolStripMenuItem();
			_contentMenuItem = new ToolStripMenuItem();
			_restorePreviousMenuItem = new ToolStripMenuItem();
			_extrasMenuItem = new ToolStripMenuItem();
			_removeStylesMenuItem = new ToolStripMenuItem();
			toolStripSeparator2 = new ToolStripSeparator();
			_showDeveloperToolsMenuItem = new ToolStripMenuItem();
			editorFontToolStripMenuItem = new ToolStripMenuItem();
			_converisonInfoMenuItem = new ToolStripMenuItem();
			_showConversionLogMenuItem = new ToolStripMenuItem();
			_aboutACadSVGStudioMenuItem = new ToolStripMenuItem();
			_openFileDialog = new OpenFileDialog();
			_saveFileDialog = new SaveFileDialog();
			_webBrowserContextMenuStrip = new ContextMenuStrip(components);
			_centerToFitContextMenuItem = new ToolStripMenuItem();
			_toolStripSeparator4 = new ToolStripSeparator();
			_showDeveloperToolsContextMenuItem = new ToolStripMenuItem();
			_statusStrip = new StatusStrip();
			_statusLabel = new ToolStripStatusLabel();
			_textChangedTimer = new System.Windows.Forms.Timer(components);
			_fontDialog = new FontDialog();
			exportSelectedDefsToolStripMenuItem = new ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)_splitContainer1).BeginInit();
			_splitContainer1.Panel1.SuspendLayout();
			_splitContainer1.Panel2.SuspendLayout();
			_splitContainer1.SuspendLayout();
			_tabControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)_splitContainer2).BeginInit();
			_splitContainer2.Panel2.SuspendLayout();
			_splitContainer2.SuspendLayout();
			_rightTabControl.SuspendLayout();
			_defsTabPage.SuspendLayout();
			_menuStrip.SuspendLayout();
			_webBrowserContextMenuStrip.SuspendLayout();
			_statusStrip.SuspendLayout();
			SuspendLayout();
			// 
			// _splitContainer1
			// 
			_splitContainer1.Dock = DockStyle.Fill;
			_splitContainer1.Location = new Point(0, 24);
			_splitContainer1.Name = "_splitContainer1";
			// 
			// _splitContainer1.Panel1
			// 
			_splitContainer1.Panel1.Controls.Add(_tabControl);
			// 
			// _splitContainer1.Panel2
			// 
			_splitContainer1.Panel2.Controls.Add(_splitContainer2);
			_splitContainer1.Size = new Size(1204, 716);
			_splitContainer1.SplitterDistance = 456;
			_splitContainer1.TabIndex = 1;
			// 
			// _tabControl
			// 
			_tabControl.Controls.Add(_mainGroupTabPage);
			_tabControl.Controls.Add(_scalesTabPage);
			_tabControl.Controls.Add(_cssTabPage);
			_tabControl.Dock = DockStyle.Fill;
			_tabControl.Location = new Point(0, 0);
			_tabControl.Name = "_tabControl";
			_tabControl.SelectedIndex = 0;
			_tabControl.Size = new Size(456, 716);
			_tabControl.TabIndex = 0;
			// 
			// _mainGroupTabPage
			// 
			_mainGroupTabPage.Location = new Point(4, 24);
			_mainGroupTabPage.Name = "_mainGroupTabPage";
			_mainGroupTabPage.Padding = new Padding(3);
			_mainGroupTabPage.Size = new Size(448, 688);
			_mainGroupTabPage.TabIndex = 0;
			_mainGroupTabPage.Text = "Main Group";
			_mainGroupTabPage.UseVisualStyleBackColor = true;
			// 
			// _scalesTabPage
			// 
			_scalesTabPage.Location = new Point(4, 24);
			_scalesTabPage.Name = "_scalesTabPage";
			_scalesTabPage.Size = new Size(448, 688);
			_scalesTabPage.TabIndex = 2;
			_scalesTabPage.Text = "Scales";
			_scalesTabPage.UseVisualStyleBackColor = true;
			// 
			// _cssTabPage
			// 
			_cssTabPage.Location = new Point(4, 24);
			_cssTabPage.Name = "_cssTabPage";
			_cssTabPage.Padding = new Padding(3);
			_cssTabPage.Size = new Size(448, 688);
			_cssTabPage.TabIndex = 1;
			_cssTabPage.Text = "CSS for Preview";
			_cssTabPage.UseVisualStyleBackColor = true;
			// 
			// _splitContainer2
			// 
			_splitContainer2.Dock = DockStyle.Fill;
			_splitContainer2.FixedPanel = FixedPanel.Panel2;
			_splitContainer2.Location = new Point(0, 0);
			_splitContainer2.Name = "_splitContainer2";
			// 
			// _splitContainer2.Panel2
			// 
			_splitContainer2.Panel2.Controls.Add(_rightTabControl);
			_splitContainer2.Size = new Size(744, 716);
			_splitContainer2.SplitterDistance = 455;
			_splitContainer2.TabIndex = 0;
			// 
			// _rightTabControl
			// 
			_rightTabControl.Controls.Add(_defsTabPage);
			_rightTabControl.Controls.Add(_propertiesTabPage);
			_rightTabControl.Dock = DockStyle.Fill;
			_rightTabControl.Location = new Point(0, 0);
			_rightTabControl.Name = "_rightTabControl";
			_rightTabControl.SelectedIndex = 0;
			_rightTabControl.Size = new Size(285, 716);
			_rightTabControl.TabIndex = 2;
			// 
            // _propertiesTabPage
            // 
            _propertiesTabPage.Controls.Add(_propertyGrid);
            _propertiesTabPage.Location = new Point(4, 24);
            _propertiesTabPage.Name = "_propertiesTabPage";
            _propertiesTabPage.Padding = new Padding(3);
            _propertiesTabPage.Size = new Size(277, 688);
            _propertiesTabPage.TabIndex = 0;
            _propertiesTabPage.Text = "Properties";
            _propertiesTabPage.UseVisualStyleBackColor = true;
            // 
            // _propertyGrid
            // 
            _propertyGrid.Dock = DockStyle.Fill;
            _propertyGrid.Location = new Point(3, 3);
            _propertyGrid.Name = "_propertyGrid";
            _propertyGrid.Size = new Size(271, 682);
            _propertyGrid.TabIndex = 0;
            // 
			// _defsTabPage
			// 
			_defsTabPage.Controls.Add(_defsTreeView);
			_defsTabPage.Location = new Point(4, 24);
			_defsTabPage.Name = "_defsTabPage";
			_defsTabPage.Padding = new Padding(3);
			_defsTabPage.Size = new Size(277, 688);
			_defsTabPage.TabIndex = 1;
			_defsTabPage.Text = "Defs";
			_defsTabPage.UseVisualStyleBackColor = true;
			// 
			// _defsTreeView
			// 
			_defsTreeView.CheckBoxes = true;
			_defsTreeView.Dock = DockStyle.Fill;
			_defsTreeView.Location = new Point(3, 3);
			_defsTreeView.Name = "_defsTreeView";
			_defsTreeView.Size = new Size(271, 682);
			_defsTreeView.TabIndex = 1;
			_defsTreeView.AfterCheck += eventDefsTreeViewAfterCheck;
			// 
			// _menuStrip
			// 
			_menuStrip.Items.AddRange(new ToolStripItem[] { _fileToolStripMenuItem, _editMenuItem, _searchToolStripMenuItem, _viewMenuItem, _contentMenuItem, _extrasMenuItem, _converisonInfoMenuItem, _aboutACadSVGStudioMenuItem });
			_menuStrip.Location = new Point(0, 0);
			_menuStrip.Name = "_menuStrip";
			_menuStrip.Size = new Size(1204, 24);
			_menuStrip.TabIndex = 2;
			_menuStrip.Text = "menuStrip1";
			// 
			// _fileToolStripMenuItem
			// 
			_fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _openMenuItem, _saveMenuItem, _saveAsMenuItem, exportSelectedDefsToolStripMenuItem, _recentlyOpenedFilesToolStripSeparator, _recentlyOpenedFilesToolStripMenuItem, _toolStripSeparator1, _exitMenuItem });
			_fileToolStripMenuItem.Name = "_fileToolStripMenuItem";
			_fileToolStripMenuItem.Size = new Size(37, 20);
			_fileToolStripMenuItem.Text = "File";
			// 
			// _openMenuItem
			// 
			_openMenuItem.Name = "_openMenuItem";
			_openMenuItem.ShortcutKeys = Keys.Control | Keys.O;
			_openMenuItem.Size = new Size(196, 22);
			_openMenuItem.Text = "Open";
			_openMenuItem.Click += eventOpenClick;
			// 
			// _saveMenuItem
			// 
			_saveMenuItem.Name = "_saveMenuItem";
			_saveMenuItem.ShortcutKeys = Keys.Control | Keys.S;
			_saveMenuItem.Size = new Size(196, 22);
			_saveMenuItem.Text = "Save";
			_saveMenuItem.Click += eventSaveSvgGroupClick;
			// 
			// _saveAsMenuItem
			// 
			_saveAsMenuItem.Name = "_saveAsMenuItem";
			_saveAsMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
			_saveAsMenuItem.Size = new Size(196, 22);
			_saveAsMenuItem.Text = "Save as ...";
			_saveAsMenuItem.Click += eventSaveSvgGroupAsClick;
			// 
			// _recentlyOpenedFilesToolStripSeparator
			// 
			_recentlyOpenedFilesToolStripSeparator.Name = "_recentlyOpenedFilesToolStripSeparator";
			_recentlyOpenedFilesToolStripSeparator.Size = new Size(193, 6);
			// 
			// _recentlyOpenedFilesToolStripMenuItem
			// 
			_recentlyOpenedFilesToolStripMenuItem.Name = "_recentlyOpenedFilesToolStripMenuItem";
			_recentlyOpenedFilesToolStripMenuItem.Size = new Size(196, 22);
			_recentlyOpenedFilesToolStripMenuItem.Text = "Recently Opened Files";
			// 
			// _toolStripSeparator1
			// 
			_toolStripSeparator1.Name = "_toolStripSeparator1";
			_toolStripSeparator1.Size = new Size(193, 6);
			// 
			// _exitMenuItem
			// 
			_exitMenuItem.Name = "_exitMenuItem";
			_exitMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
			_exitMenuItem.Size = new Size(196, 22);
			_exitMenuItem.Text = "Exit";
			_exitMenuItem.Click += eventExit_Click;
			// 
			// _editMenuItem
			// 
			_editMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _undoMenuItem, _redoMenuItem, _toolStripSeparator11, _cutMenuItem, _copyMenuItem, _pasteMenuItem, _deleteMenuItem, _toolStripSeparator13, _selectAllMenuItem });
			_editMenuItem.Name = "_editMenuItem";
			_editMenuItem.Size = new Size(39, 20);
			_editMenuItem.Text = "Edit";
			_editMenuItem.DropDownOpening += eventEdit_DropDownOpening;
			// 
			// _undoMenuItem
			// 
			_undoMenuItem.Name = "_undoMenuItem";
			_undoMenuItem.Size = new Size(122, 22);
			_undoMenuItem.Text = "Undo";
			_undoMenuItem.Click += eventUndo_Click;
			// 
			// _redoMenuItem
			// 
			_redoMenuItem.Name = "_redoMenuItem";
			_redoMenuItem.Size = new Size(122, 22);
			_redoMenuItem.Text = "Redo";
			_redoMenuItem.Click += eventRedo_Click;
			// 
			// _toolStripSeparator11
			// 
			_toolStripSeparator11.Name = "_toolStripSeparator11";
			_toolStripSeparator11.Size = new Size(119, 6);
			// 
			// _cutMenuItem
			// 
			_cutMenuItem.Name = "_cutMenuItem";
			_cutMenuItem.Size = new Size(122, 22);
			_cutMenuItem.Text = "Cut";
			_cutMenuItem.Click += eventCut_Click;
			// 
			// _copyMenuItem
			// 
			_copyMenuItem.Name = "_copyMenuItem";
			_copyMenuItem.Size = new Size(122, 22);
			_copyMenuItem.Text = "Copy";
			_copyMenuItem.Click += eventCopy_Click;
			// 
			// _pasteMenuItem
			// 
			_pasteMenuItem.Name = "_pasteMenuItem";
			_pasteMenuItem.Size = new Size(122, 22);
			_pasteMenuItem.Text = "Paste";
			_pasteMenuItem.Click += eventPaste_Click;
			// 
			// _deleteMenuItem
			// 
			_deleteMenuItem.Name = "_deleteMenuItem";
			_deleteMenuItem.Size = new Size(122, 22);
			_deleteMenuItem.Text = "Delete";
			_deleteMenuItem.Click += eventDelete_Click;
			// 
			// _toolStripSeparator13
			// 
			_toolStripSeparator13.Name = "_toolStripSeparator13";
			_toolStripSeparator13.Size = new Size(119, 6);
			// 
			// _selectAllMenuItem
			// 
			_selectAllMenuItem.Name = "_selectAllMenuItem";
			_selectAllMenuItem.Size = new Size(122, 22);
			_selectAllMenuItem.Text = "Select All";
			_selectAllMenuItem.Click += eventSelectAll_Click;
			// 
			// _searchToolStripMenuItem
			// 
			_searchToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _quickFindMenuItem, _findAndReplaceMenuItem });
			_searchToolStripMenuItem.Name = "_searchToolStripMenuItem";
			_searchToolStripMenuItem.Size = new Size(54, 20);
			_searchToolStripMenuItem.Text = "Search";
			// 
			// _quickFindMenuItem
			// 
			_quickFindMenuItem.Name = "_quickFindMenuItem";
			_quickFindMenuItem.ShortcutKeys = Keys.Control | Keys.F;
			_quickFindMenuItem.Size = new Size(236, 22);
			_quickFindMenuItem.Text = "Quick Find";
			_quickFindMenuItem.Click += eventQuickFind_Click;
			// 
			// _findAndReplaceMenuItem
			// 
			_findAndReplaceMenuItem.Name = "_findAndReplaceMenuItem";
			_findAndReplaceMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F;
			_findAndReplaceMenuItem.Size = new Size(236, 22);
			_findAndReplaceMenuItem.Text = "Find and Replace";
			_findAndReplaceMenuItem.Click += eventFindAndReplace_Click;
			// 
			// _viewMenuItem
			// 
			_viewMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _centerToFitMenuItem, _toolStripSeparator3, _propertyGridToolStripMenuItem, _toolStripSeparator5, _collapseAllMenuItem, _expandAllToolStripMenuItem });
			_viewMenuItem.Name = "_viewMenuItem";
			_viewMenuItem.Size = new Size(44, 20);
			_viewMenuItem.Text = "View";
			// 
			// _centerToFitMenuItem
			// 
			_centerToFitMenuItem.Name = "_centerToFitMenuItem";
			_centerToFitMenuItem.ShortcutKeys = Keys.Control | Keys.E;
			_centerToFitMenuItem.Size = new Size(179, 22);
			_centerToFitMenuItem.Text = "Center to Fit";
			_centerToFitMenuItem.Click += eventCenterToFitMenuItem_Click;
			// 
			// _toolStripSeparator3
			// 
			_toolStripSeparator3.Name = "_toolStripSeparator3";
			_toolStripSeparator3.Size = new Size(176, 6);
			// 
			// _propertyGridToolStripMenuItem
			// 
			_propertyGridToolStripMenuItem.Checked = true;
			_propertyGridToolStripMenuItem.CheckOnClick = true;
			_propertyGridToolStripMenuItem.CheckState = CheckState.Checked;
			_propertyGridToolStripMenuItem.Name = "_propertyGridToolStripMenuItem";
			_propertyGridToolStripMenuItem.Size = new Size(179, 22);
			_propertyGridToolStripMenuItem.Text = "Property Grid";
			_propertyGridToolStripMenuItem.CheckedChanged += eventPropertyGridMenuItem_CheckedChanged;
			// 
			// _toolStripSeparator5
			// 
			_toolStripSeparator5.Name = "_toolStripSeparator5";
			_toolStripSeparator5.Size = new Size(176, 6);
			// 
			// _collapseAllMenuItem
			// 
			_collapseAllMenuItem.Name = "_collapseAllMenuItem";
			_collapseAllMenuItem.Size = new Size(179, 22);
			_collapseAllMenuItem.Text = "Collapse All";
			_collapseAllMenuItem.Click += eventCollapseAllMenuItem_Click;
			// 
			// _expandAllToolStripMenuItem
			// 
			_expandAllToolStripMenuItem.Name = "_expandAllToolStripMenuItem";
			_expandAllToolStripMenuItem.Size = new Size(179, 22);
			_expandAllToolStripMenuItem.Text = "Expand All";
			_expandAllToolStripMenuItem.Click += eventExpandAllMenuItem_Click;
			// 
			// _contentMenuItem
			// 
			_contentMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _restorePreviousMenuItem });
			_contentMenuItem.Name = "_contentMenuItem";
			_contentMenuItem.Size = new Size(62, 20);
			_contentMenuItem.Text = "Content";
			// 
			// _restorePreviousMenuItem
			// 
			_restorePreviousMenuItem.Name = "_restorePreviousMenuItem";
			_restorePreviousMenuItem.Size = new Size(161, 22);
			_restorePreviousMenuItem.Text = "Restore Previous";
			_restorePreviousMenuItem.Click += eventFlipContent_Click;
			// 
			// _extrasMenuItem
			// 
			_extrasMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _removeStylesMenuItem, toolStripSeparator2, _showDeveloperToolsMenuItem, editorFontToolStripMenuItem });
			_extrasMenuItem.Name = "_extrasMenuItem";
			_extrasMenuItem.Size = new Size(50, 20);
			_extrasMenuItem.Text = "Extras";
			// 
			// _removeStylesMenuItem
			// 
			_removeStylesMenuItem.Name = "_removeStylesMenuItem";
			_removeStylesMenuItem.Size = new Size(214, 22);
			_removeStylesMenuItem.Text = "Remove Styles";
			_removeStylesMenuItem.Click += eventRemoveStyles_Click;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(211, 6);
			// 
			// _showDeveloperToolsMenuItem
			// 
			_showDeveloperToolsMenuItem.Name = "_showDeveloperToolsMenuItem";
			_showDeveloperToolsMenuItem.ShortcutKeys = Keys.F12;
			_showDeveloperToolsMenuItem.Size = new Size(214, 22);
			_showDeveloperToolsMenuItem.Text = "Show Developer Tools";
			_showDeveloperToolsMenuItem.Click += eventShowDeveloperToolsMenuItem_Click;
			// 
			// editorFontToolStripMenuItem
			// 
			editorFontToolStripMenuItem.Name = "editorFontToolStripMenuItem";
			editorFontToolStripMenuItem.Size = new Size(214, 22);
			editorFontToolStripMenuItem.Text = "Editor Font";
			editorFontToolStripMenuItem.Click += eventEditorFontToolStripMenuItem_Click;
			// 
			// _converisonInfoMenuItem
			// 
			_converisonInfoMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _showConversionLogMenuItem });
			_converisonInfoMenuItem.Name = "_converisonInfoMenuItem";
			_converisonInfoMenuItem.Size = new Size(103, 20);
			_converisonInfoMenuItem.Text = "Converison Info";
			// 
			// _showConversionLogMenuItem
			// 
			_showConversionLogMenuItem.Name = "_showConversionLogMenuItem";
			_showConversionLogMenuItem.ShortcutKeys = Keys.F2;
			_showConversionLogMenuItem.Size = new Size(208, 22);
			_showConversionLogMenuItem.Text = "Show Conversion Log";
			_showConversionLogMenuItem.Click += showConversionLog_Click;
			// 
			// _aboutACadSVGStudioMenuItem
			// 
			_aboutACadSVGStudioMenuItem.Name = "_aboutACadSVGStudioMenuItem";
			_aboutACadSVGStudioMenuItem.Size = new Size(145, 20);
			_aboutACadSVGStudioMenuItem.Text = "About ACad SVG Studio";
			_aboutACadSVGStudioMenuItem.Click += eventAbout_Click;
			// 
			// _openFileDialog
			// 
			_openFileDialog.Filter = "DWG files|*.dwg|DXF files|*.dxf|SVG group files|*.g.svg|All files|*.*";
			_openFileDialog.FileOk += eventOpenFileDialog_FileOk;
			// 
			// _saveFileDialog
			// 
			_saveFileDialog.Filter = "SVG files|*.svg|SVG group files|*.g.svg";
			_saveFileDialog.FileOk += eventSaveFileDialog_FileOk;
			// 
			// _webBrowserContextMenuStrip
			// 
			_webBrowserContextMenuStrip.Items.AddRange(new ToolStripItem[] { _centerToFitContextMenuItem, _toolStripSeparator4, _showDeveloperToolsContextMenuItem });
			_webBrowserContextMenuStrip.Name = "_webBrowserContextMenuStrip";
			_webBrowserContextMenuStrip.Size = new Size(190, 54);
			// 
			// _centerToFitContextMenuItem
			// 
			_centerToFitContextMenuItem.Name = "_centerToFitContextMenuItem";
			_centerToFitContextMenuItem.ShortcutKeys = Keys.Control | Keys.E;
			_centerToFitContextMenuItem.Size = new Size(189, 22);
			_centerToFitContextMenuItem.Text = "Center to Fit";
			_centerToFitContextMenuItem.Click += eventCenterToFitMenuItem_Click;
			// 
			// _toolStripSeparator4
			// 
			_toolStripSeparator4.Name = "_toolStripSeparator4";
			_toolStripSeparator4.Size = new Size(186, 6);
			// 
			// _showDeveloperToolsContextMenuItem
			// 
			_showDeveloperToolsContextMenuItem.Name = "_showDeveloperToolsContextMenuItem";
			_showDeveloperToolsContextMenuItem.Size = new Size(189, 22);
			_showDeveloperToolsContextMenuItem.Text = "Show Developer Tools";
			_showDeveloperToolsContextMenuItem.Click += eventShowDeveloperToolsMenuItem_Click;
			// 
			// _statusStrip
			// 
			_statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel });
			_statusStrip.Location = new Point(0, 740);
			_statusStrip.Name = "_statusStrip";
			_statusStrip.Size = new Size(1204, 22);
			_statusStrip.TabIndex = 3;
			// 
			// _statusLabel
			// 
			_statusLabel.ForeColor = Color.Red;
			_statusLabel.Name = "_statusLabel";
			_statusLabel.Size = new Size(1189, 17);
			_statusLabel.Spring = true;
			// 
			// _textChangedTimer
			// 
			_textChangedTimer.Interval = 500;
			_textChangedTimer.Tick += eventTextChangedTimer_Tick;
			// 
			// exportSelectedDefsToolStripMenuItem
			// 
			exportSelectedDefsToolStripMenuItem.Name = "exportSelectedDefsToolStripMenuItem";
			exportSelectedDefsToolStripMenuItem.Size = new Size(196, 22);
			exportSelectedDefsToolStripMenuItem.Text = "Export Selected Defs";
			exportSelectedDefsToolStripMenuItem.Click += eventExportSelectedDefs_Click;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1204, 762);
			Controls.Add(_splitContainer1);
			Controls.Add(_menuStrip);
			Controls.Add(_statusStrip);
			MainMenuStrip = _menuStrip;
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "ACadSvgStudio";
			_splitContainer1.Panel1.ResumeLayout(false);
			_splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)_splitContainer1).EndInit();
			_splitContainer1.ResumeLayout(false);
			_tabControl.ResumeLayout(false);
			_splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)_splitContainer2).EndInit();
			_splitContainer2.ResumeLayout(false);
			_rightTabControl.ResumeLayout(false);
			_propertiesTabPage.ResumeLayout(false);
            _defsTabPage.ResumeLayout(false);
			_menuStrip.ResumeLayout(false);
			_menuStrip.PerformLayout();
			_webBrowserContextMenuStrip.ResumeLayout(false);
			_statusStrip.ResumeLayout(false);
			_statusStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private SplitContainer _splitContainer1;
		private MenuStrip _menuStrip;
		private ToolStripMenuItem _fileToolStripMenuItem;
		private ToolStripMenuItem _exitToolStripMenuItem;
		private SplitContainer _splitContainer2;
		private PropertyGrid _propertyGrid;
		private ToolStripSeparator _toolStripSeparator1;
		private TabControl _tabControl;
		private TabPage _mainGroupTabPage;
		private TabPage _cssTabPage;
		private TabPage _scalesTabPage;
		private ToolStripMenuItem _openToolStripMenuItem;
		private ToolStripMenuItem _saveToolStripMenuItem1;
		private ToolStripMenuItem _saveAsToolStripMenuItem;
		private ToolStripMenuItem _contentToolStripMenuItem;
		private ToolStripMenuItem _restorePreviousToolStripMenuItem;
		private ToolStripMenuItem _converisonInfoToolStripMenuItem;
		private ToolStripMenuItem _showConversionLogToolStripMenuItem;
		private ToolStripMenuItem _viewToolStripMenuItem;
		private ToolStripMenuItem _centerToFitToolStripMenuItem;
		private ToolStripMenuItem _extrasToolStripMenuItem;
		private ToolStripMenuItem _removeStylesToolStripMenuItem;
		private ToolStripMenuItem _searchToolStripMenuItem;
		private ToolStripMenuItem _findToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem _showDeveloperToolsMenuItem;
		private OpenFileDialog _openFileDialog;
		private SaveFileDialog _saveFileDialog;
		private ToolStripMenuItem _propertyGridToolStripMenuItem;
		private ToolStripSeparator _toolStripSeparator3;
		private ContextMenuStrip _webBrowserContextMenuStrip;
		private ToolStripMenuItem _centerToFitContextMenuItem;
		private ToolStripSeparator _toolStripSeparator4;
		private ToolStripMenuItem _showDeveloperToolsContextMenuItem;
		private ToolStripMenuItem _collapseAllMenuItem;
		private ToolStripSeparator _toolStripSeparator5;
		private ToolStripMenuItem _expandAllToolStripMenuItem;
		private ToolStripMenuItem _aboutACadSVGStudioMenuItem;
		private ToolStripMenuItem _findAndReplaceToolStripMenuItem;
		private ToolStripMenuItem _findAndReplaceMenuItem;
		private ToolStripMenuItem _quickFindMenuItem;
		private ToolStripMenuItem _openMenuItem;
		private ToolStripMenuItem _saveMenuItem;
		private ToolStripMenuItem _saveAsMenuItem;
		private ToolStripMenuItem _exitMenuItem;
		private ToolStripMenuItem _viewMenuItem;
		private ToolStripMenuItem _centerToFitMenuItem;
		private ToolStripMenuItem _contentMenuItem;
		private ToolStripMenuItem _restorePreviousMenuItem;
		private ToolStripMenuItem _extrasMenuItem;
		private ToolStripMenuItem _removeStylesMenuItem;
		private ToolStripMenuItem _converisonInfoMenuItem;
		private ToolStripMenuItem _showConversionLogMenuItem;
		private ToolStripMenuItem _editMenuItem;
		private ToolStripMenuItem _undoMenuItem;
		private ToolStripSeparator _toolStripSeparator11;
		private ToolStripMenuItem _cutMenuItem;
		private ToolStripMenuItem _copyMenuItem;
		private ToolStripMenuItem _pasteMenuItem;
		private ToolStripSeparator _toolStripSeparator13;
		private ToolStripMenuItem _selectAllMenuItem;
		private ToolStripMenuItem _redoMenuItem;
		private ToolStripMenuItem _deleteMenuItem;
		private StatusStrip _statusStrip;
		private ToolStripStatusLabel _statusLabel;
		private System.Windows.Forms.Timer _textChangedTimer;
		private FontDialog _fontDialog;
		private ToolStripMenuItem editorFontToolStripMenuItem;
		private ToolStripSeparator _recentlyOpenedFilesToolStripSeparator;
		private ToolStripMenuItem _recentlyOpenedFilesToolStripMenuItem;
		private TabControl _rightTabControl;
		private TabPage _propertiesTabPage;
		private TabPage _defsTabPage;
		private TreeView _defsTreeView;
		private ToolStripMenuItem exportSelectedDefsToolStripMenuItem;
	}
}