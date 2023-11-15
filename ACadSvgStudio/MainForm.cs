#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using System.Text;
using System.Xml.Linq;
using File = System.IO.File;

using ACadSvg;

using CefSharp;
using CefSharp.WinForms;
using CefSharp.Dom;
using ScintillaNET;
using ScintillaNET_FindReplaceDialog;


namespace ACadSvgStudio {

    public partial class MainForm : Form {

        public const string AppName = "ACad SVG Studio";

        private SvgProperties _svgProperties;

        private ConversionContext _conversionContext;

        private Scintilla _scintillaSvgGroupEditor;
        private Scintilla _scintillaCss;
        private Scintilla _scintillaScales;
        private ChromiumWebBrowser _webBrowser;
        private IncrementalSearcher _incrementalSearcher;
        private FindReplace _findReplace;
        private int _maxLineNumberCharLength;

        private bool _centerToFitOnLoad = false;

        private bool _updatingHTMLEnabled = true;

        private string? _loadedFilename;
        private string? _loadedDwgFilename;

        //  Current-conversion Info
        private string _conversionLog;
        private bool _contentChanged;
        private ISet<string> _occurringEntities;

        //  Flipped data and info
        private string _flippedFilename;
        private string _flippedSvg;
        private ISet<string> _flippedOccurringEntities;
        private string _flippedConversionLog;
        private bool _flippedContentChanged;

        public MainForm() {

            InitializeComponent();

            this.Text = AppName;

            initScintillaSVGGroupEditor();
            initScintillaScales();
            initScintillaCss();
            initWebBrowser();

            _svgProperties = new SvgProperties(this);
            _propertyGrid.SelectedObject = _svgProperties;

            _scintillaSvgGroupEditor.BorderStyle = ScintillaNET.BorderStyle.FixedSingle;
            _scintillaScales.Text = Settings.Default.ScalesSvg;
            _scintillaCss.Text = Settings.Default.CSSPreview;

            _findReplace = new FindReplace();
            _findReplace.Scintilla = _scintillaSvgGroupEditor;
            _findReplace.Window.FormClosing += (s, e) => enableUpdatingHTML(s, e);
            _findReplace.ReplaceAllResults += (s, e) => enableUpdatingHTML(s, e);

            Button replaceAllButton = findReplaceAllButton();
            replaceAllButton.MouseDown += (s, e) => disableUpdatingHTML(s, e);
            replaceAllButton.Click += (s, e) => disableUpdatingHTML(s, e);

            _incrementalSearcher = new IncrementalSearcher(true);
            _incrementalSearcher.FindReplace = _findReplace;
            _incrementalSearcher.Dock = DockStyle.Top;
            _tabControl.TabPages[0].Controls.Add(_incrementalSearcher);
            _incrementalSearcher.Visible = false;

            FlowLayoutPanel flowLayoutPanel = findIncrementalSearchFlowLayoutPanel();
            Button closeButton = new Button();
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Text = "Close";
            closeButton.Click += (s, e) => eventQuickFindClose_Click(s, e);
            closeButton.Margin = new Padding(closeButton.Margin.Left, closeButton.Margin.Top - 3, closeButton.Margin.Right, closeButton.Margin.Bottom);
            flowLayoutPanel.Controls.Add(closeButton);

            CreateHTML();
        }


        /// <summary>
        /// Find the Replace-All button of the Find/Replace Dialog.
        /// We need the button-click event to disable updating the HTML during
        /// the find/replace process.
        /// </summary>
        /// <returns></returns>
        private Button findReplaceAllButton() {
            foreach (Control control in _findReplace.Window.Controls) {
                if (control is TabControl tabControl) {
                    // Tab page with index 1 is replace tab
                    Control.ControlCollection replaceTabPageControls = tabControl.TabPages[1].Controls;
                    foreach (Control ctl in replaceTabPageControls) {
                        if (ctl is Button btn && btn.Name == "btnReplaceAll") {
                            return btn;
                        }
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Find the FlowLayoutPanel of the QuickFind Dialog.
        /// We need the button to hide the dialog.
        /// </summary>
        /// <returns></returns>
        private FlowLayoutPanel findIncrementalSearchFlowLayoutPanel() {
            foreach (Control control in _incrementalSearcher.Controls) {
                if (control is FlowLayoutPanel flowLayoutPanel) {
                    return flowLayoutPanel;
                }
            }

            return null;
        }


        internal void LoadFile(string filename) {
            string ext = Path.GetExtension(filename).ToLower();

            switch (ext) {
            case ".svg":
                _scintillaSvgGroupEditor.Text = File.ReadAllText(filename);
                _loadedFilename = filename;
                this.Text = $"{AppName} - {filename}";
                _contentChanged = false;
                _centerToFitOnLoad = true;
                break;

            case ".dwg":
                readACadFile(filename, "DWG");
                return;
            }
        }


        private void readACadFile(string filename, string fileFormat) {

            _conversionContext = new ConversionContext() {
                ConversionOptions = _svgProperties.GetConversionOptions(),
                ViewboxData = _svgProperties.GetViewbox(),
                GlobalAttributeData = _svgProperties.GetGlobalAttributeData()
            };

            string svgText;
            ACadLoader loader = new ACadLoader();
            switch (fileFormat) {
            case "DWG":
                DocumentSvg docSvg = loader.LoadDwg(filename, _conversionContext);
                svgText = docSvg.ToSvg();
                break;
            case "DXF":
            default:
                throw new InvalidOperationException($"File format {fileFormat} not supported");
            }

            _flippedFilename = _loadedDwgFilename;
            _flippedSvg = _scintillaSvgGroupEditor.Text;
            _flippedConversionLog = _conversionLog;
            _flippedOccurringEntities = _occurringEntities;

            ConversionInfo conversionInfo = _conversionContext.ConversionInfo;
            _conversionLog = conversionInfo.GetLog();
            _occurringEntities = conversionInfo.OccurringEntities;
            _scintillaSvgGroupEditor.Text = svgText;
            _loadedDwgFilename = filename;
            this.Text = $"{AppName} - Converted {fileFormat}: {filename}";
            _contentChanged = true;
            _centerToFitOnLoad = true;
        }


        internal void SetSelection(bool clear, int index, int length) {
            if (clear) {
                _scintillaSvgGroupEditor.ClearSelections();
            }
            _scintillaSvgGroupEditor.SetSelection(index, index + length);
            _scintillaSvgGroupEditor.ScrollRange(index, index + length);
        }

        #region -  Init Scintilla editors and Web Browser                                           -

        private void initScintillaSVGGroupEditor() {
            _scintillaSvgGroupEditor = new ScintillaNET.Scintilla();
            _scintillaSvgGroupEditor.Dock = DockStyle.Fill;
            _scintillaSvgGroupEditor.TextChanged += eventScintilla_TextChanged;
            updateLineMargin(_scintillaSvgGroupEditor);
            _mainGroupTabPage.Controls.Add(_scintillaSvgGroupEditor);

            //_scintillaSvgGroupEditor.Font = TODO Set monospace Font

            // Drag and Drop
            _scintillaSvgGroupEditor.AllowDrop = true;
            _scintillaSvgGroupEditor.DragEnter += (s, e) => editorDragEnter(s, e);
            _scintillaSvgGroupEditor.DragDrop += (s, e) => editorDragDrop(s, e);


            // Recipe for XML
            _scintillaSvgGroupEditor.Lexer = ScintillaNET.Lexer.Xml;

            // Show line numbers
            //scintillaSVGGroupEditor.Margins[0].Width = 40;

            // Enable folding
            _scintillaSvgGroupEditor.SetProperty("fold", "1");
            _scintillaSvgGroupEditor.SetProperty("fold.compact", "1");
            _scintillaSvgGroupEditor.SetProperty("fold.html", "1");

            // Use Margin 2 for fold markers
            _scintillaSvgGroupEditor.Margins[2].Type = MarginType.Symbol;
            _scintillaSvgGroupEditor.Margins[2].Mask = Marker.MaskFolders;
            _scintillaSvgGroupEditor.Margins[2].Sensitive = true;
            _scintillaSvgGroupEditor.Margins[2].Width = 20;

            // Reset folder markers
            for (int i = Marker.FolderEnd; i <= Marker.FolderOpen; i++) {
                _scintillaSvgGroupEditor.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                _scintillaSvgGroupEditor.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Style the folder markers
            _scintillaSvgGroupEditor.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            _scintillaSvgGroupEditor.Markers[Marker.Folder].SetBackColor(SystemColors.ControlText);
            _scintillaSvgGroupEditor.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            _scintillaSvgGroupEditor.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            _scintillaSvgGroupEditor.Markers[Marker.FolderEnd].SetBackColor(SystemColors.ControlText);
            _scintillaSvgGroupEditor.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            _scintillaSvgGroupEditor.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            _scintillaSvgGroupEditor.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            _scintillaSvgGroupEditor.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            _scintillaSvgGroupEditor.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;

            _scintillaSvgGroupEditor.Styles[ScintillaNET.Style.Xml.Tag].ForeColor = Color.Violet;
            _scintillaSvgGroupEditor.Styles[ScintillaNET.Style.Xml.TagUnknown].ForeColor = Color.Red;
            _scintillaSvgGroupEditor.Styles[ScintillaNET.Style.Xml.AttributeUnknown].ForeColor = Color.MediumBlue;
            _scintillaSvgGroupEditor.Styles[ScintillaNET.Style.Xml.Comment].ForeColor = Color.Green;

            _scintillaSvgGroupEditor.SetKeywords(0, @"g path title text circle ellipse pattern defs");
        }


        private void initScintillaScales() {
            _scintillaScales = new ScintillaNET.Scintilla();
            _scintillaScales.Dock = DockStyle.Fill;
            _scintillaScales.TextChanged += eventScintillaScales_TextChanged;
            updateLineMargin(_scintillaScales);
            _scalesTabPage.Controls.Add(_scintillaScales);

            _scintillaScales.Lexer = ScintillaNET.Lexer.Xml;

            _scintillaScales.Styles[ScintillaNET.Style.Xml.Tag].ForeColor = Color.Violet;
            _scintillaScales.Styles[ScintillaNET.Style.Xml.TagUnknown].ForeColor = Color.Red;
            _scintillaScales.Styles[ScintillaNET.Style.Xml.AttributeUnknown].ForeColor = Color.MediumBlue;
            _scintillaScales.Styles[ScintillaNET.Style.Xml.Comment].ForeColor = Color.Green;
        }


        private void initScintillaCss() {
            _scintillaCss = new ScintillaNET.Scintilla();
            _scintillaCss.Dock = DockStyle.Fill;
            _scintillaCss.TextChanged += eventScintillaCss_TextChanged;
            updateLineMargin(_scintillaCss);
            _cssTabPage.Controls.Add(_scintillaCss);


            // Recipe for CSS

            _scintillaCss.Lexer = ScintillaNET.Lexer.Css;

            _scintillaCss.Styles[ScintillaNET.Style.Css.Class].ForeColor = Color.Violet;
            _scintillaCss.Styles[ScintillaNET.Style.Css.ExtendedPseudoClass].ForeColor = Color.Violet;
            _scintillaCss.Styles[ScintillaNET.Style.Css.PseudoClass].ForeColor = Color.Violet;
            _scintillaCss.Styles[ScintillaNET.Style.Css.UnknownPseudoClass].ForeColor = Color.Violet;

            _scintillaCss.Styles[ScintillaNET.Style.Css.PseudoElement].ForeColor = Color.MediumBlue;
            _scintillaCss.Styles[ScintillaNET.Style.Css.ExtendedPseudoElement].ForeColor = Color.MediumBlue;

            _scintillaCss.Styles[ScintillaNET.Style.Css.Value].ForeColor = Color.Blue;

            _scintillaCss.Styles[ScintillaNET.Style.Css.Tag].ForeColor = Color.Violet;

            _scintillaCss.Styles[ScintillaNET.Style.Css.Comment].ForeColor = Color.Green;
        }


        private void initWebBrowser() {
            _webBrowser = new ChromiumWebBrowser();
            _webBrowser.Dock = DockStyle.Fill;
            _splitContainer2.Panel1.Controls.Add(_webBrowser);

            _webBrowser.MenuHandler = new MyContextMenuHandler(this);


            _webBrowser.LoadingStateChanged += (s, e) => {
                if (!e.IsLoading && _centerToFitOnLoad) {
                    centerToFit();
                    _centerToFitOnLoad = false;
                }
            };
        }


        private void updateLineMargin(object? sender) {
            if (sender == null) {
                return;
            }

            Scintilla scintilla = (Scintilla)sender;

            var lineMarginChars = scintilla.Lines.Count.ToString().Length;
            if (lineMarginChars != this._maxLineNumberCharLength) {
                doUpdateLineMargin(scintilla, lineMarginChars);
            }
        }


        private void doUpdateLineMargin(ScintillaNET.Scintilla scintilla, int lineMarginChars) {
            const int padding = 2;
            scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', lineMarginChars + 1)) + padding;
            _maxLineNumberCharLength = lineMarginChars;
        }

        #endregion
        #region -  Events MainForm

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            _propertyGridToolStripMenuItem.Checked = Settings.Default.ViewPropertyGrid;
            _splitContainer2.Panel2Collapsed = !_propertyGridToolStripMenuItem.Checked;


            if (Settings.Default.WindowPositionInitialized) {
                Location = Settings.Default.WindowPosition;
                Size = Settings.Default.WindowSize;

                _splitContainer1.SplitterDistance = Settings.Default.SplitContainer1Distance;
                _splitContainer2.SplitterDistance = Settings.Default.SplitContainer2Distance;
            }
            else {
                Settings.Default.WindowPositionInitialized = true;
                saveWindowState();
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (_contentChanged) {
                switch (MessageBox.Show("Content has been changed, save changes?", "Close", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)) {
                case DialogResult.Yes:
                    if (!string.IsNullOrEmpty(_loadedFilename)) {
                        File.WriteAllText(_loadedFilename, _scintillaSvgGroupEditor.Text);
                        return;
                    }
                    _saveFileDialog.FilterIndex = 1;
                    e.Cancel = _saveFileDialog.ShowDialog() == DialogResult.Cancel;
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                }
            }
        }


        protected override void OnClosed(EventArgs e) {
            saveWindowState();
        }


        private void saveWindowState() {
            Settings.Default.WindowPosition = Location;

            if (WindowState == FormWindowState.Normal) {
                Settings.Default.WindowSize = Size;
            }
            else {
                Settings.Default.WindowSize = RestoreBounds.Size;
            }

            Settings.Default.SplitContainer1Distance = _splitContainer1.SplitterDistance;
            Settings.Default.SplitContainer2Distance = _splitContainer2.SplitterDistance;

            Settings.Default.Save();
        }

        #endregion
        #region -  Events Scinlilla

        private void eventScintilla_TextChanged(object? sender, EventArgs e) {
            if (!_updatingHTMLEnabled) {
                return;
            }

            UpdateHTML();

            updateLineMargin(sender);
            _contentChanged = true;
        }


        private void eventScintillaScales_TextChanged(object? sender, EventArgs e) {
            Settings.Default.ScalesSvg = _scintillaScales.Text;
            Settings.Default.Save();

            UpdateHTML();
            updateLineMargin(sender);
        }


        private void eventScintillaCss_TextChanged(object? sender, EventArgs e) {
            Settings.Default.CSSPreview = _scintillaCss.Text;
            Settings.Default.Save();

            UpdateHTML();
            updateLineMargin(sender);
        }


        private void editorDragEnter(object sender, DragEventArgs e) {
            if (e.Data == null) {
                return;
            }

            object fileDropData = e.Data.GetData(DataFormats.FileDrop);
            if (fileDropData == null) {
                return;
            }

            string[] files = (string[])fileDropData;

            // Check for unsupported files
            foreach (string file in files) {
                if (!file.ToLower().EndsWith(".dwg")) {
                    return;
                }
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            }
        }


        private void editorDragDrop(object sender, DragEventArgs e) {
            if (e.Data == null) {
                return;
            }

            object fileDropData = e.Data.GetData(DataFormats.FileDrop);
            if (fileDropData == null) {
                return;
            }

            string[] files = (string[])fileDropData;
            if (files.Length == 0) {
                return;
            }

            string filename = files[0];
            //	Extension is always ".dwg"
            readACadFile(filename, "DWG");
        }

        #endregion

        #region -  Events File Menu + Open, Save Dialog 

        private void eventOpenClick(object sender, EventArgs e) {
            _openFileDialog.ShowDialog();
        }


        private void eventSaveSvgGroupClick(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(_loadedFilename)) {
                File.WriteAllText(_loadedFilename, _scintillaSvgGroupEditor.Text);
                _contentChanged = false;
                return;
            }

            eventSaveSvgGroupAsClick(sender, e);
        }


        private void eventSaveSvgGroupAsClick(object sender, EventArgs e) {
            _saveFileDialog.FilterIndex = 1;
            _saveFileDialog.ShowDialog();
        }


        private void eventSaveSvgGroupFileAsDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            _saveFileDialog.FilterIndex = 2;
            _saveFileDialog.ShowDialog();
        }


        private void eventSaveSvgFileClick(object sender, EventArgs e) {
            _saveFileDialog.FilterIndex = 1;
            _saveFileDialog.ShowDialog();
        }


        private void eventExit_Click(object sender, EventArgs e) {
            Close();
        }


        private void eventOpenFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                int filterIndex = _openFileDialog.FilterIndex;
                string filename = _openFileDialog.FileName;


                switch (filterIndex) {
                case 1: // ".dwg";
                    readACadFile(filename, "DWG");
                    break;
                case 2: // ".svg"
                    _scintillaSvgGroupEditor.Text = File.ReadAllText(filename);
                    _centerToFitOnLoad = true;
                    break;
                default:
                    break;
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventSaveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                int filterIndex = _saveFileDialog.FilterIndex;
                string filename = _saveFileDialog.FileName;

                switch (filterIndex) {
                case 1:
                    File.WriteAllText(filename, buildSVG(false, false, true));
                    _loadedFilename = filename;
                    this.Text = $"{AppName} - {_loadedFilename}";
                    _contentChanged = false;
                    break;
                case 2:
                    File.WriteAllText(filename, _scintillaSvgGroupEditor.Text);
                    _loadedFilename = filename;
                    this.Text = $"{AppName} - {_loadedFilename}";
                    _contentChanged = false;
                    break;
                default:
                    break;
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }

        #endregion
        #region -  Events Edit Menu

        private void eventEdit_DropDownOpening(object sender, EventArgs e) {
            bool hasSelection = !string.IsNullOrEmpty(_scintillaSvgGroupEditor.SelectedText);

            _undoMenuItem.Enabled = _scintillaSvgGroupEditor.CanUndo;
            _cutMenuItem.Enabled = hasSelection;
            _copyMenuItem.Enabled = hasSelection;
            _redoMenuItem.Enabled = _scintillaSvgGroupEditor.CanRedo;
            _pasteMenuItem.Enabled = _scintillaSvgGroupEditor.CanPaste;
            _deleteMenuItem.Enabled = hasSelection;
        }


        private void eventUndo_Click(object sender, EventArgs e) {
            try {
                Scintilla editor = getCurrentEditor();
                editor.Undo();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventRedo_Click(object sender, EventArgs e) {
            try {
                Scintilla editor = getCurrentEditor();
                editor.Redo();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventCut_Click(object sender, EventArgs e) {
            try {
                Scintilla editor = getCurrentEditor();
                editor.Cut();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventCopy_Click(object sender, EventArgs e) {
            try {
                Scintilla editor = getCurrentEditor();
                editor.Copy();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventPaste_Click(object sender, EventArgs e) {
            try {
                Scintilla editor = getCurrentEditor();
                editor.Paste();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventDelete_Click(object sender, EventArgs e) {
            try {
                Scintilla editor = getCurrentEditor();
                //editor.SelectedText = string.Empty;
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventSelectAll_Click(object sender, EventArgs e) {
            try {
                Scintilla editor = getCurrentEditor();
                editor.SelectAll();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private Scintilla getCurrentEditor() {
            return (Scintilla)_tabControl.SelectedTab.Controls[0];
        }

        #endregion
        #region -  Events Search Menu and Search Dialog

        private void eventQuickFind_Click(object sender, EventArgs e) {
            _incrementalSearcher.Visible = true;
        }


        private void eventQuickFindClose_Click(object sender, EventArgs e) {
            _incrementalSearcher.Visible = false;
        }


        private void eventFindAndReplace_Click(object sender, EventArgs e) {
            _findReplace.Scintilla = _scintillaSvgGroupEditor;
            _findReplace.ShowFind();
        }


        private void enableUpdatingHTML(object sender, EventArgs e) {
            _updatingHTMLEnabled = true;
            UpdateHTML();
        }


        private void disableUpdatingHTML(object sender, EventArgs e) {
            _updatingHTMLEnabled = false;
        }

        #endregion
        #region -  Events View Menu

        private void eventPropertyGridMenuItem_CheckedChanged(object sender, EventArgs e) {
            _splitContainer2.Panel2Collapsed = !_propertyGridToolStripMenuItem.Checked;

            Settings.Default.ViewPropertyGrid = _propertyGridToolStripMenuItem.Checked;
            Settings.Default.Save();
        }


        private void eventCenterToFitMenuItem_Click(object sender, EventArgs e) {
            centerToFit();
        }


        private void eventCollapseAllMenuItem_Click(object sender, EventArgs e) {
            _scintillaSvgGroupEditor.FoldAll(FoldAction.Contract);
        }


        private void eventExpandAllMenuItem_Click(object sender, EventArgs e) {
            _scintillaSvgGroupEditor.FoldAll(FoldAction.Expand);
        }

        #endregion
        #region -  Events Content

        private void eventFlipContent_Click(object sender, EventArgs e) {
            try {
                string flipFilename = _loadedDwgFilename;
                _loadedDwgFilename = _flippedFilename;
                _flippedFilename = flipFilename;

                string flipSvg = _scintillaSvgGroupEditor.Text;
                _scintillaSvgGroupEditor.Text = _flippedSvg;
                _flippedSvg = flipSvg;

                var flipLog = _conversionLog;
                _conversionLog = _flippedConversionLog;
                _flippedConversionLog = flipLog;

                var flipOccurringEntities = _occurringEntities;
                _occurringEntities = _flippedOccurringEntities;
                _flippedOccurringEntities = flipOccurringEntities;

                bool flipContentChanged = _contentChanged;
                _contentChanged = _flippedContentChanged;
                _flippedContentChanged = flipContentChanged;
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }

        #endregion
        #region -  Events Extras Menu

        private void eventRemoveStyles_Click(object sender, EventArgs e) {
            XElement doc = XElement.Parse("<root>" + _scintillaSvgGroupEditor.Text + "</root>", LoadOptions.PreserveWhitespace);
            foreach (XElement el in doc.Elements()) {
                removeStyles(el);
            }

            StringBuilder sb = new StringBuilder();

            foreach (XElement el in doc.Elements()) {
                sb.Append(el.ToString());
            }

            _scintillaSvgGroupEditor.Text = sb.ToString();
        }


        private void removeStyles(XElement element) {
            XAttribute? styleAttribute = element.Attribute("style");
            if (styleAttribute != null) {
                styleAttribute.Remove();
            }

            foreach (XElement el in element.Elements()) {
                removeStyles(el);
            }
        }



        private void eventShowDeveloperToolsMenuItem_Click(object sender, EventArgs e) {
            _webBrowser.ShowDevTools();
        }

        #endregion
        #region -  Events Conversion Info Menu

        private void showConversionLog_Click(object sender, EventArgs e) {
            try {
                var conversionLogForm = new ConversionInfoForm();
                conversionLogForm.Open(_loadedDwgFilename, _conversionLog, _occurringEntities);
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }

        #endregion
        #region -  Events About Menu

        private void eventAbout_Click(object sender, EventArgs e) {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        #endregion

        #region -  SVG nd HTML

        private string buildSVG(bool showScales, bool addCss, bool addDeclAndType = false) {
            if (_conversionContext == null) {
                _conversionContext = new ConversionContext();
            }

            _conversionContext.UpdateSettings(
                _svgProperties.GetConversionOptions(),
                _svgProperties.GetViewbox(),
                _svgProperties.GetGlobalAttributeData());

            XElement svg = EntitySvg.CreateSVG(_conversionContext).GetXml();

            StringBuilder csb = new StringBuilder();
            string css = _scintillaCss.Text;
            if (addCss && !string.IsNullOrEmpty(css)) {
                XElement styleXElement = new XElement("style");
                styleXElement.Add(new XAttribute("type", "text/css"));
                styleXElement.Value = css;
                csb.AppendLine(styleXElement.ToString());
            }

            string scales = _scintillaScales.Text;
            if (showScales && !string.IsNullOrEmpty(scales)) {
                csb.AppendLine(scales);
            }

            string editorText = _scintillaSvgGroupEditor.Text;
            if (!string.IsNullOrEmpty(editorText)) {
                csb.AppendLine(editorText);
            }

            svg.Value = csb.ToString();

            StringBuilder sb = new StringBuilder();
            if (addDeclAndType) {
                var declaration = new XDeclaration("1.0", null, null);
                var doctype = new XDocumentType("svg", " -//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", string.Empty).ToString();
                sb.AppendLine(declaration.ToString());
                sb.AppendLine(doctype.ToString());
                sb.AppendLine(svg.ToString());
            }
            else {
                sb.AppendLine(svg.ToString());
            }

            string svgText = sb.ToString().Replace("&gt;", ">").Replace("&lt;", "<");
            return svgText;
        }

        public void CreateHTML() {
            string backgroundColor = ColorTranslator.ToHtml(Settings.Default.BackgroundColor);
            var svg = buildSVG(Settings.Default.ScalesEnabled, Settings.Default.CSSPreviewEnabled);
            string html = HTMLBuilder.Build(svg, backgroundColor);
            CefSharp.WebBrowserExtensions.LoadHtml(_webBrowser, html);
            centerToFit();
        }


        public void UpdateHTML() {
            if (_centerToFitOnLoad) {
                CreateHTML();
                _centerToFitOnLoad = false;
                return;
            }


			string backgroundColor = ColorTranslator.ToHtml(Settings.Default.BackgroundColor);
			var svg = buildSVG(Settings.Default.ScalesEnabled, Settings.Default.CSSPreviewEnabled);


            try {
                Task.Delay(100).ContinueWith(async task1 => {
                    if (!_webBrowser.CanExecuteJavascriptInMainFrame) {
                        return;
                    }

                    TimeSpan timeout = new TimeSpan(0, 0, 0, 0, 500);

					using var devToolsContext = await _webBrowser.CreateDevToolsContextAsync();

                    var body = await devToolsContext.QuerySelectorAsync<HtmlBodyElement>("body");
                    await body.SetAttributeAsync("style", $"background-color:{backgroundColor};").ContinueWith(async task2 =>
                    {
						var svgViewerDivElement = await devToolsContext.QuerySelectorAsync<HtmlDivElement>("#svg-viewer");
						await svgViewerDivElement.SetInnerHtmlAsync(svg).ContinueWith(async task3 => {
							var response = _webBrowser.EvaluateScriptAsync(HTMLBuilder.GetPanZoomScript(), timeout).Result;
							if (response.Success)
							{
								//	string value = response.Result.ToString();
								//	Debug.WriteLine(value);
							}
						});
					});
                });
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }

        #endregion
        #region -  WebBrowser functions

        public void centerToFit() {
            if (!_webBrowser.CanExecuteJavascriptInMainFrame) {
                return;
            }

            _webBrowser.EvaluateScriptAsync
            (@"
                panZoom.resize();
                panZoom.center();
                panZoom.fit();
                panZoom.zoomBy(0.8);
            ");
            //  System.Diagnostics.Debug.WriteLine($"Centered to Fit");
        }

        #endregion
    }
}