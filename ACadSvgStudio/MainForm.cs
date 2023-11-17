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
using SvgElements;

namespace ACadSvgStudio {

    public partial class MainForm : Form {

        public const string AppName = "ACad SVG Studio";
        private const string SvgKeywords = "circle defs ellipse g path pattern rect text";

        private SvgProperties _svgProperties;

        private ConversionContext _conversionContext;

        private Scintilla _scintillaSvgGroupEditor;
        private Scintilla _scintillaCss;
        private Scintilla _scintillaScales;
        private ChromiumWebBrowser _webBrowser;
        private DevToolsContext _devToolsContext;
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

            initPropertyGrid();
            initFindReplace();

            setEditorFont(Settings.Default.EditorFont);

            _scintillaScales.Text = Settings.Default.ScalesSvg;
            _scintillaCss.Text = Settings.Default.CSSPreview;

            CreateHTML();
        }

        private void initFindReplace() {
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
        }


        private void initPropertyGrid() {
            _svgProperties = new SvgProperties(this);
            _propertyGrid.SelectedObject = _svgProperties;
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
                _centerToFitOnLoad = true;
                _scintillaSvgGroupEditor.Text = File.ReadAllText(filename);
                _loadedFilename = filename;
                this.Text = $"{AppName} - {filename}";
                break;

            case ".dwg":
                readACadFile(filename, "DWG");
                _contentChanged = true;
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
            _centerToFitOnLoad = true;
            _scintillaSvgGroupEditor.Text = svgText;
            _loadedDwgFilename = filename;
            this.Text = $"{AppName} - Converted {fileFormat}: {filename}";
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
            _scintillaSvgGroupEditor.BorderStyle = ScintillaNET.BorderStyle.FixedSingle;
            _scintillaSvgGroupEditor.TextChanged += eventScintilla_TextChanged;
            updateLineMargin(_scintillaSvgGroupEditor);
            _mainGroupTabPage.Controls.Add(_scintillaSvgGroupEditor);

            _scintillaSvgGroupEditor.Font = new Font("CourierNew", 12);

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

            _scintillaSvgGroupEditor.SetKeywords(0, SvgKeywords);
        }


        private void initScintillaScales() {
            _scintillaScales = new ScintillaNET.Scintilla();
            _scintillaScales.Dock = DockStyle.Fill;
            _scintillaScales.BorderStyle = ScintillaNET.BorderStyle.FixedSingle;
            _scintillaScales.TextChanged += eventScintillaScales_TextChanged;
            updateLineMargin(_scintillaScales);
            _scalesTabPage.Controls.Add(_scintillaScales);

            _scintillaScales.Lexer = ScintillaNET.Lexer.Xml;

            _scintillaScales.Styles[ScintillaNET.Style.Xml.Tag].ForeColor = Color.Violet;
            _scintillaScales.Styles[ScintillaNET.Style.Xml.TagUnknown].ForeColor = Color.Red;
            _scintillaScales.Styles[ScintillaNET.Style.Xml.AttributeUnknown].ForeColor = Color.MediumBlue;
            _scintillaScales.Styles[ScintillaNET.Style.Xml.Comment].ForeColor = Color.Green;

            _scintillaScales.SetKeywords(0, SvgKeywords);
        }


        private void initScintillaCss() {
            _scintillaCss = new ScintillaNET.Scintilla();
            _scintillaCss.Dock = DockStyle.Fill;
            _scintillaCss.BorderStyle = ScintillaNET.BorderStyle.FixedSingle;
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
                    //_centerToFitOnLoad = false;
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
            try {
                _propertyGridToolStripMenuItem.Checked = Settings.Default.ViewPropertyGrid;
                _splitContainer2.Panel2Collapsed = !_propertyGridToolStripMenuItem.Checked;


                if (Settings.Default.WindowPositionInitialized) {
                    Point loc = Settings.Default.WindowPosition;
                    Size size = Settings.Default.WindowSize;
                    bool mustUseDefault = true;
                    Rectangle windowRect = new Rectangle(loc, size);
                    if (size.Width > 0 && size.Height > 0) {
                        Screen targetScreen = findScreen(windowRect);
                        mustUseDefault = targetScreen == null;
                    }

                    if (mustUseDefault) {
                        return;
                    }

                    Location = loc;
                    Size = size;

                    int splitter1Dist = Settings.Default.SplitContainer1Distance;
                    int splitter2Dist = Settings.Default.SplitContainer2Distance;
                    if (splitter1Dist > _splitContainer1.Panel1MinSize &&
                        splitter1Dist < _splitContainer1.Width - _splitContainer1.Panel2MinSize) {
                        _splitContainer1.SplitterDistance = splitter1Dist;
                    }
                    if (splitter2Dist > _splitContainer2.Panel1MinSize &&
                        splitter2Dist < _splitContainer2.Width - _splitContainer2.Panel2MinSize) {
                        _splitContainer2.SplitterDistance = splitter2Dist;
                    }
                }
                else {
                    Settings.Default.WindowPositionInitialized = true;
                    saveWindowState();
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private Screen findScreen(Rectangle windowRect) {
            foreach (Screen screen in Screen.AllScreens) {
                Rectangle screenRectangle = screen.Bounds;
                if (screenRectangle.IntersectsWith(windowRect) && windowRect.Top + 8 >= screen.Bounds.Top) {
                    return screen;
                }
            }
            return null;
        }


        protected override void OnFormClosing(FormClosingEventArgs e) {
            try {
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
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        protected override void OnClosed(EventArgs e) {
            try {
                saveWindowState();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
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


        private void eventTextChangedTimer_Tick(object sender, EventArgs e) {
            try {
                if (_updatingHTMLEnabled) {
                    UpdateHTML();
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
            finally {
                _textChangedTimer.Stop();
            }
        }


        private void eventScintilla_TextChanged(object? sender, EventArgs e) {
            try {
                updateLineMargin(sender);
                _contentChanged = true;

                _textChangedTimer.Start();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventScintillaScales_TextChanged(object? sender, EventArgs e) {
            try {
                Settings.Default.ScalesSvg = _scintillaScales.Text;
                Settings.Default.Save();

                updateLineMargin(sender);

                _textChangedTimer.Start();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventScintillaCss_TextChanged(object? sender, EventArgs e) {
            try {
                Settings.Default.CSSPreview = _scintillaCss.Text;
                Settings.Default.Save();

                updateLineMargin(sender);

                _textChangedTimer.Start();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void editorDragEnter(object sender, DragEventArgs e) {
            try {
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
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void editorDragDrop(object sender, DragEventArgs e) {
            try {
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
                _contentChanged = true;
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }

        #endregion

        #region -  Events File Menu + Open, Save Dialog 

        private void eventOpenClick(object sender, EventArgs e) {
            try {
                _openFileDialog.ShowDialog();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventSaveSvgGroupClick(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrEmpty(_loadedFilename)) {
                    File.WriteAllText(_loadedFilename, _scintillaSvgGroupEditor.Text);
                    _contentChanged = false;
                    return;
                }

                eventSaveSvgGroupAsClick(sender, e);
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventSaveSvgGroupAsClick(object sender, EventArgs e) {
            try {
                _saveFileDialog.FilterIndex = 1;
                _saveFileDialog.ShowDialog();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventSaveSvgGroupFileAsDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                _saveFileDialog.FilterIndex = 2;
                _saveFileDialog.ShowDialog();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventSaveSvgFileClick(object sender, EventArgs e) {
            try {
                _saveFileDialog.FilterIndex = 1;
                _saveFileDialog.ShowDialog();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventExit_Click(object sender, EventArgs e) {
            try {
                Close();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventOpenFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                int filterIndex = _openFileDialog.FilterIndex;
                string filename = _openFileDialog.FileName;


                switch (filterIndex) {
                case 1: // ".dwg";
                    readACadFile(filename, "DWG");
                    _contentChanged = true;
                    break;
                case 2: // ".svg"
                    _centerToFitOnLoad = true;
                    _scintillaSvgGroupEditor.Text = File.ReadAllText(filename);
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
            try {
                bool hasSelection = !string.IsNullOrEmpty(_scintillaSvgGroupEditor.SelectedText);

                _undoMenuItem.Enabled = _scintillaSvgGroupEditor.CanUndo;
                _cutMenuItem.Enabled = hasSelection;
                _copyMenuItem.Enabled = hasSelection;
                _redoMenuItem.Enabled = _scintillaSvgGroupEditor.CanRedo;
                _pasteMenuItem.Enabled = _scintillaSvgGroupEditor.CanPaste;
                _deleteMenuItem.Enabled = hasSelection;
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
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
            try {
                _incrementalSearcher.Visible = true;
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventQuickFindClose_Click(object sender, EventArgs e) {
            try {
                _incrementalSearcher.Visible = false;
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventFindAndReplace_Click(object sender, EventArgs e) {
            try {
                _findReplace.Scintilla = _scintillaSvgGroupEditor;
                _findReplace.ShowFind();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void enableUpdatingHTML(object sender, EventArgs e) {
            try {
                _updatingHTMLEnabled = true;
                UpdateHTML();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void disableUpdatingHTML(object sender, EventArgs e) {
            _updatingHTMLEnabled = false;
        }

        #endregion
        #region -  Events View Menu

        private void eventPropertyGridMenuItem_CheckedChanged(object sender, EventArgs e) {
            try {
                _splitContainer2.Panel2Collapsed = !_propertyGridToolStripMenuItem.Checked;

                Settings.Default.ViewPropertyGrid = _propertyGridToolStripMenuItem.Checked;
                Settings.Default.Save();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventCenterToFitMenuItem_Click(object sender, EventArgs e) {
            try {
                centerToFit();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventCollapseAllMenuItem_Click(object sender, EventArgs e) {
            try {
                _scintillaSvgGroupEditor.CollapseChildren();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventExpandAllMenuItem_Click(object sender, EventArgs e) {
            try {
                _scintillaSvgGroupEditor.ExpandChildren();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
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
            try {
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
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void removeStyles(XElement element) {
            try {
                XAttribute? styleAttribute = element.Attribute("style");
                if (styleAttribute != null) {
                    styleAttribute.Remove();
                }

                foreach (XElement el in element.Elements()) {
                    removeStyles(el);
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventShowDeveloperToolsMenuItem_Click(object sender, EventArgs e) {
            _webBrowser.ShowDevTools();
        }


        private void eventEditorFontToolStripMenuItem_Click(object sender, EventArgs e) {
            _fontDialog.Font = Settings.Default.EditorFont;

            if (_fontDialog.ShowDialog() == DialogResult.OK) {
                Font font = _fontDialog.Font;
                setEditorFont(font);

                Settings.Default.EditorFont = font;
                Settings.Default.Save();
            }
        }

        private void setEditorFont(Font font) {
            _scintillaSvgGroupEditor.Font = font;
            _scintillaScales.Font = font;
            _scintillaCss.Font = font;
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
            try {
                using (AboutForm aboutForm = new AboutForm()) {
                    aboutForm.ShowDialog();
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }

        #endregion

        #region -  SVG nd HTML

        private string buildSVG(bool showScales, bool addCss, bool createFile = false) {
            if (_conversionContext == null) {
                _conversionContext = new ConversionContext();
            }

            _conversionContext.UpdateSettings(
                _svgProperties.GetConversionOptions(),
                _svgProperties.GetViewbox(),
                _svgProperties.GetGlobalAttributeData());

            SvgElement svgElement = EntitySvg.CreateSVG(_conversionContext);

			if (createFile) {
                svgElement.Style = "background-color:black;";
                svgElement.Width = _svgProperties.GetViewbox().Width.ToString();
                svgElement.Height = _svgProperties.GetViewbox().Height.ToString();
                svgElement.WithViewbox(null, null, null, null);
            }

            XElement svg = svgElement.GetXml();

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
                if (createFile) {
                    XElement xElement = XElement.Parse(editorText);
                    xElement.SetAttributeValue("transform", $"scale(1, -1) translate(0, {-_svgProperties.GetViewbox().Height})");
                    csb.AppendLine(xElement.ToString());
				}
                else {
					csb.AppendLine(editorText);
				}
            }

            svg.Value = csb.ToString();

            StringBuilder sb = new StringBuilder();
            if (createFile) {
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
            var svg = buildSVG(Settings.Default.ScalesEnabled, Settings.Default.CSSPreviewEnabled, false);
            string html = HTMLBuilder.Build(svg, backgroundColor);
            CefSharp.WebBrowserExtensions.LoadHtml(_webBrowser, html);
            Task.Delay(300).Wait();
            runPanZoomScript();
            centerToFit();
        }


        public void UpdateHTML() {
            if (_centerToFitOnLoad) {
                CreateHTML();
                _centerToFitOnLoad = false;
                return;
            }

            string backgroundColor = ColorTranslator.ToHtml(Settings.Default.BackgroundColor);
            var svg = buildSVG(Settings.Default.ScalesEnabled, Settings.Default.CSSPreviewEnabled, false);

            if (_devToolsContext == null) {
                Task<DevToolsContext> t = _webBrowser.CreateDevToolsContextAsync();
                t.Wait();

                _devToolsContext = t.Result;
            }

            updateBrowserContent(svg, backgroundColor);
            runPanZoomScript();
        }


        private void updateBrowserContent(string svg, string backgroundColor) {
            Task<HtmlBodyElement> tBody = _devToolsContext.QuerySelectorAsync<HtmlBodyElement>("body");
            tBody.Wait();
            Task tBodyResult = tBody.Result.SetAttributeAsync("style", $"background-color:{backgroundColor};");
            tBodyResult.Wait();

            Task<HtmlDivElement> tSvgViewerDivElement = _devToolsContext.QuerySelectorAsync<HtmlDivElement>("#svg-viewer");
            tSvgViewerDivElement.Wait();
            Task tSvgViewerDivElementResult = tSvgViewerDivElement.Result.SetInnerHtmlAsync(svg);
            tSvgViewerDivElementResult.Wait();
        }


        private void runPanZoomScript() {
            if (!_webBrowser.CanExecuteJavascriptInMainFrame) {
                return;
            }

            Task<JavascriptResponse> tScript = _webBrowser.EvaluateScriptAsync(HTMLBuilder.GetPanZoomScript(), new TimeSpan(0, 0, 0, 0, 500));
            tScript.Wait();

            if (!tScript.Result.Success) {
                _statusLabel.Text = "Executing Pan Zoom script failed.";
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