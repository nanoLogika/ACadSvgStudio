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
using ACadSvgStudio.Defs;
using ACadSvgStudio.BatchProcessing;


namespace ACadSvgStudio {

    public partial class MainForm : Form {

        public const string AppName = "ACad SVG Studio";
        private const string SvgKeywords = "circle defs ellipse g path pattern rect text tspan";
        private const string BatchKeywords = "export -i --input -o --output -d --defs-groups -r --resolve-defs";

        private RecentlyOpenedFilesManager recentlyOpenedFilesManager;

        private SvgProperties _svgProperties;

        private ConversionContext _conversionContext;

        private Scintilla _scintillaSvgGroupEditor;
        private Scintilla _scintillaCss;
        private Scintilla _scintillaScales;
        private Scintilla _scintillaBatchEditor;
        private TextBox _batchConsoleLog;
        private ChromiumWebBrowser _webBrowser;
        private DevToolsContext _devToolsContext;
        private IncrementalSearcher _incrementalSearcher;
        private FindReplace _findReplace;
        private int _maxLineNumberCharLength;

        private bool _centerToFitOnLoad = true;
        private bool _executingPanScriptFailed;
        private bool _updatingHTMLEnabled = false;

        private string? _loadedFilename;
        private string? _loadedDwgFilename;

        //  Current-conversion Info
        private string _conversionLog;
        private bool _contentChanged;
        private ISet<string> _occurringEntities;

        private bool _suppressOnChecked = false;

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
            initBatchEditor();
            initWebBrowser();

            initPropertyGrid();
            initFindReplace();

            setEditorFont(Settings.Default.EditorFont);

            recentlyOpenedFilesManager = new RecentlyOpenedFilesManager();
            updateRecentlyOpenedFiles();

            UpdateSvgViewer();
        }


        private bool isFileSupported(string filename) {
            string[] supportedFiles = new string[] {
                "dwg",
                "dxf",
                "svg",
                "g.svg"
            };

            string filenameLower = filename.ToLower();

            foreach (string supportedFile in supportedFiles) {
                if (filenameLower.EndsWith($".{supportedFile}")) {
                    return true;
                }
            }

            return false;
        }


        private void updateRecentlyOpenedFiles() {
            bool hasRecentlyOpenedFiles = recentlyOpenedFilesManager.HasRecentlyOpenedFiles();
            _fileMenuSeparator1.Visible = hasRecentlyOpenedFiles;
            _recentlyOpenedFilesToolStripMenuItem.Visible = hasRecentlyOpenedFiles;

            _recentlyOpenedFilesToolStripMenuItem.DropDownItems.Clear();

            if (hasRecentlyOpenedFiles) {
                List<string> recentlyOpenedFiles = recentlyOpenedFilesManager.RecentlyOpenedFiles();
                int counter = 1;
                foreach (string file in recentlyOpenedFiles) {
                    ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
                    toolStripMenuItem.Text = $"{counter} {file}";
                    toolStripMenuItem.Click += (s, e) => {
                        LoadFile(file);
                    };
                    _recentlyOpenedFilesToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
                    counter++;
                }
            }
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
            case ".g.svg":
                readSvgFile(filename);
                _contentChanged = false;
                break;

            case ".dwg":
                readDwgFile(filename);
                _contentChanged = true;
                return;

            case ".dxf":
                readDxfFile(filename);
                _contentChanged = true;
                return;
            }
        }


        private void createConversionContext() {
            _conversionContext = new ConversionContext() {
                ConversionOptions = _svgProperties.GetConversionOptions(),
                ViewboxData = _svgProperties.GetViewbox(),
                GlobalAttributeData = _svgProperties.GetGlobalAttributeData()
            };
        }


        private void updateConversionInfo(string filename, string fileFormat, string svgText, string scalesSvgText) {
            _flippedFilename = _loadedDwgFilename;
            _flippedSvg = _scintillaSvgGroupEditor.Text;
            _flippedConversionLog = _conversionLog;
            _flippedOccurringEntities = _occurringEntities;

            ConversionInfo conversionInfo = _conversionContext.ConversionInfo;
            _conversionLog = conversionInfo.GetLog();
            _occurringEntities = conversionInfo.OccurringEntities;
            //_svgProperties.SetViewbox(_conversionContext.ViewboxData);

            _centerToFitOnLoad = true;
            _scintillaSvgGroupEditor.Text = svgText;
            if (_conversionContext.ConversionOptions.CreateScaleFromModelSpaceExtent) {
                _scintillaScales.Text = scalesSvgText;
            }

            _loadedDwgFilename = filename;
            this.Text = $"{AppName} - {fileFormat}: {new FileInfo(filename).Name}";
        }

        #region -  Devs tree view																	-

        private void eventDefsTreeViewAfterCheck(object sender, TreeViewEventArgs e) {
            if (_suppressOnChecked) {
                return;
            }


            // Save collapsed/expanded states
            List<int> prevFolderExpanded = new List<int>();
            for (int x = 0; x < _scintillaSvgGroupEditor.Lines.Count; x++) {
                prevFolderExpanded.Add(_scintillaSvgGroupEditor.GetFoldExpanded(x));
            }


            TreeNode treeNode = e.Node!;

            XDocument xDocument = XDocument.Parse(_scintillaSvgGroupEditor.Text);

            if (treeNode.Checked) {
                if (treeNode.Tag is UseElement useElement) {
                    xDocument.Root!.AddFirst(useElement.GetXml());
                    prevFolderExpanded.Insert(1, 1);
                }
                else if (treeNode.Tag is IList<UseElement> useElements) {
                    foreach (UseElement ue in useElements) {
                        xDocument.Root!.AddFirst(ue.GetXml());
                        prevFolderExpanded.Insert(1, 1);
                    }
                }
            }
            else {
                List<XElement> useElements = DefsUtils.FindUseElements(treeNode.Name, xDocument.Root!);
                foreach (XElement useElement in useElements) {
                    useElement.Remove();
                    prevFolderExpanded.RemoveAt(1);
                }
            }

            //	TODO This should be optimized
            //	Update xml display only once!
            _scintillaSvgGroupEditor.Text = xDocument.ToString();


            // Restore collapsed/expanded states
            for (int x = 0; x < _scintillaSvgGroupEditor.Lines.Count; x++) {
                if (prevFolderExpanded[x] == 1) {
                    _scintillaSvgGroupEditor.SetFoldExpanded(x, prevFolderExpanded[x]);
                }
            }
            for (int x = 0; x < _scintillaSvgGroupEditor.Lines.Count; x++) {
                if (prevFolderExpanded[x] == 0) {
                    _scintillaSvgGroupEditor.SetFoldExpanded(x, prevFolderExpanded[x]);
                }
            }


            TreeNode parent = treeNode.Parent;
            if (treeNode.Checked) {
                if (parent != null) {
                    parent.Checked = false;
                }
                foreach (TreeNode childTreeNode in treeNode.Nodes) {
                    childTreeNode.Checked = false;
                }
            }
        }


        private void collectFlatListOfTreeNodes(TreeNodeCollection nodes, IDictionary<string, TreeNode> flatListOfTreeNodes, bool selectedOnly = false) {
            foreach (TreeNode node in nodes) {
                if (selectedOnly) {
                    if (node.Checked) {
                        flatListOfTreeNodes.Add(node.Name, node);
                    }
                }
                else {
                    flatListOfTreeNodes.Add(node.Name, node);
                }

                collectFlatListOfTreeNodes(node.Nodes, flatListOfTreeNodes, selectedOnly);
            }
        }

        private void applyFlatListOfTreeNodes(TreeNode newTreeNode, IDictionary<string, TreeNode> prevTreeNodes) {
            if (prevTreeNodes.TryGetValue(newTreeNode.Name, out TreeNode prevNode)) {
                if (prevNode.IsExpanded) {
                    newTreeNode.Expand();
                }
                else {
                    newTreeNode.Collapse();
                }
                //	Extended Text and UseElements may be present at previous tree node
                newTreeNode.Text = prevNode.Text;
                newTreeNode.Tag = prevNode.Tag;
            }

            foreach (TreeNode node in newTreeNode.Nodes) {
                applyFlatListOfTreeNodes(node, prevTreeNodes);
            }
        }


        private bool updateDefs(string xmlValue) {
            if (string.IsNullOrWhiteSpace(xmlValue)) {
                return false;
            }

            XElement xElement = XElement.Parse(xmlValue);

            List<DefsItem> defsItems = new List<DefsItem>();
            DefsUtils.FindAllDefs(xElement, null, defsItems);

            if (defsItems.Count == 0) {
                return false;
            }

            IDictionary<string, TreeNode> prevTreeNodes = new Dictionary<string, TreeNode>();
            collectFlatListOfTreeNodes(_defsTreeView.Nodes, prevTreeNodes);

            List<TreeNode> newTreeNodes = new List<TreeNode>();

            foreach (DefsItem defsItem in defsItems) {
                TreeNode node = createDefsTreeNode(defsItem);
                newTreeNodes.Add(node);
            }

            //	Build new tree but restore tags and expanded/collapsed from previous
            //	all nodes are unchecked.
            _defsTreeView.Nodes.Clear();
            foreach (TreeNode node in newTreeNodes) {
                _defsTreeView.Nodes.Add(node);
                applyFlatListOfTreeNodes(node, prevTreeNodes);
            }

            //	Scan list of <use> tags
            IList<UseElement> usedDefsItems = DefsUtils.FindAllUsedDefs(xElement);
            IList<string> assignedDefIds = new List<string>();
            foreach (UseElement useElement in usedDefsItems) {
                string id = useElement.GroupId.Substring(1);

                TreeNode node = findNode(_defsTreeView.Nodes, id);
                if (node != null) {
                    if (useElement.X != 0 || useElement.Y != 0) {
                        node.Text = $"{id} ({useElement.X}, {useElement.Y})";
                    }
                    //	Only expand to List of UseElement when Tag was set here before
                    if (assignedDefIds.Contains(id)) {
                        if (node.Tag is UseElement tagUseElement) {
                            node.Tag = new List<UseElement>() { tagUseElement, useElement };
                        }
                        else if (node.Tag is IList<UseElement>) {
                            ((List<UseElement>)node.Tag).Add(useElement);
                        }
                        node.Text = $"{id} (multiple pos)";
                    }
                    else {
                        //	This may override a tag from the previous tree vie
                        node.Tag = useElement;
                        assignedDefIds.Add(id);
                    }

                    _suppressOnChecked = true;
                    node.Checked = true;
                    _suppressOnChecked = false;
                }
            }

            return true;
        }


        private TreeNode findNode(TreeNodeCollection nodes, string id) {
            foreach (TreeNode node in nodes) {
                if (node.Name == id) {
                    return node;
                }
                else {
                    TreeNode foundNode = findNode(node.Nodes, id);
                    if (foundNode != null) {
                        return foundNode;
                    }
                }
            }
            return null;
        }


        private TreeNode createDefsTreeNode(DefsItem defsItem) {
            TreeNode treeNode = new TreeNode() {
                Name = defsItem.Id,
                Text = defsItem.Id,
                Tag = new UseElement().WithGroupId(defsItem.Id)
            };

            foreach (DefsItem childDefsItem in defsItem.Children) {
                TreeNode childTreeNode = createDefsTreeNode(childDefsItem);
                treeNode.Nodes.Add(childTreeNode);
            }

            return treeNode;
        }

        #endregion

        private void readDwgFile(string filename) {
            createConversionContext();

            DocumentSvg docSvg = ACadLoader.LoadDwg(filename, _conversionContext);
            string svgText = docSvg.ToSvg();
            string scalesSvgText = docSvg.GetModelSpaceRectangle().ToString();

            updateConversionInfo(filename, "Converted DWG", svgText, scalesSvgText);

            recentlyOpenedFilesManager.RegisterFile(filename);
            updateRecentlyOpenedFiles();

            _loadedFilename = null;
        }


        private void readDxfFile(string filename) {
            createConversionContext();

            DocumentSvg docSvg = ACadLoader.LoadDxf(filename, _conversionContext);
            string svgText = docSvg.ToSvg();
            string scalesSvgText = docSvg.GetModelSpaceRectangle().ToString();

            updateConversionInfo(filename, "Converted DXF", svgText, scalesSvgText);

            recentlyOpenedFilesManager.RegisterFile(filename);
            updateRecentlyOpenedFiles();

            _loadedFilename = null;
        }


        private void readSvgFile(string filename) {
            createConversionContext();

            string svgText = System.IO.File.ReadAllText(filename);

            updateConversionInfo(filename, "SVG", svgText, string.Empty);

            recentlyOpenedFilesManager.RegisterFile(filename);
            updateRecentlyOpenedFiles();

            _loadedFilename = filename;
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
            _scintillaSvgGroupEditor.DragEnter += (s, e) => eventEditorDragEnter(s, e);
            _scintillaSvgGroupEditor.DragDrop += (s, e) => eventEditorDragDrop(s, e);
            svgViewerUserControl.AllowDrop = true;
            svgViewerUserControl.DragEnter += (s, e) => eventEditorDragEnter(s, e);
            svgViewerUserControl.DragDrop += (s, e) => eventEditorDragDrop(s, e);


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


        private void initBatchEditor() {
			_scintillaBatchEditor = new ScintillaNET.Scintilla();
			_scintillaBatchEditor.Dock = DockStyle.Fill;
			_scintillaBatchEditor.BorderStyle = ScintillaNET.BorderStyle.FixedSingle;
			_scintillaBatchEditor.TextChanged += eventScintillaBatchEditor_TextChanged;
			updateLineMargin(_scintillaBatchEditor);
			_batchTabPage.Controls.Add(_scintillaBatchEditor);


			// Recipe for Batch
			_scintillaBatchEditor.Lexer = ScintillaNET.Lexer.Batch;

			_scintillaBatchEditor.Styles[ScintillaNET.Style.Batch.Word].ForeColor = Color.Violet;
			_scintillaBatchEditor.Styles[ScintillaNET.Style.Batch.Hide].ForeColor = Color.Red;
			_scintillaBatchEditor.Styles[ScintillaNET.Style.Batch.Label].ForeColor = Color.MediumBlue;
			_scintillaBatchEditor.Styles[ScintillaNET.Style.Batch.Comment].ForeColor = Color.Green;

			_scintillaBatchEditor.SetKeywords(0, BatchKeywords);


            // Textbox for Console Log
            _batchConsoleLog = new TextBox();
            _batchConsoleLog.ReadOnly = true;
            _batchConsoleLog.Dock = DockStyle.Bottom;
            _batchConsoleLog.Multiline = true;
            _batchConsoleLog.Height = 200;
            _batchTabPage.Controls.Add(_batchConsoleLog);
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

                _scintillaScales.Text = Settings.Default.ScalesSvg;
                _scintillaCss.Text = Settings.Default.CSSPreview;
                _updatingHTMLEnabled = true;

                CreateHTML();
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
                        if (string.IsNullOrEmpty(_loadedFilename)) {
                            _saveFileDialog.FileName = _loadedFilename;
                            _saveFileDialog.FilterIndex = 1;
                            e.Cancel = _saveFileDialog.ShowDialog() == DialogResult.Cancel;
                        }
                        if (e.Cancel) {
                            return;
                        }
                        File.WriteAllText(_loadedFilename, _scintillaSvgGroupEditor.Text);
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                    }
                }
                Batch batch = BatchController.CurrentBatch;
                if (batch != null && batch.HasChanges) {
                    string name = batch.Name;
                    switch (MessageBox.Show($"Current command batch {name} has been changed, save changes?", "Close", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)) {
                    case DialogResult.Yes:
                        if (string.IsNullOrEmpty(name)) {
                            _saveFileDialog.FileName = _loadedFilename;
                            _saveFileDialog.FilterIndex = 1;
                            e.Cancel = _saveFileDialog.ShowDialog() == DialogResult.Cancel;
                        }
                        if (e.Cancel) {
                            return;
                        }
                        batch.Save();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
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
                clearStatusLabel();
                updateLineMargin(sender);
                _contentChanged = true;

                if (_updatingHTMLEnabled) {
                    _textChangedTimer.Restart();
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventScintillaScales_TextChanged(object? sender, EventArgs e) {
            try {
                clearStatusLabel();
                Settings.Default.ScalesSvg = _scintillaScales.Text;
                Settings.Default.Save();

                updateLineMargin(sender);

                if (_updatingHTMLEnabled) {
                    _textChangedTimer.Restart();
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventScintillaCss_TextChanged(object? sender, EventArgs e) {
            try {
                clearStatusLabel();
                Settings.Default.CSSPreview = _scintillaCss.Text;
                Settings.Default.Save();

                updateLineMargin(sender);

                if (_updatingHTMLEnabled) {
                    _textChangedTimer.Restart();
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


		private void eventScintillaBatchEditor_TextChanged(object? sender, EventArgs e) {
            if (string.IsNullOrEmpty(_scintillaBatchEditor.Text)) {
                return;
            }

            BatchController.UpdateBatch(_scintillaBatchEditor.Text);

			_scintillaBatchEditor.Markers[0].SetBackColor(Color.Pink);

			if (BatchController.CurrentBatch != null) {
                foreach (var line in  _scintillaBatchEditor.Lines) {
                    line.MarkerDelete(0);
                }

				foreach (int index in BatchController.CurrentBatch.GetErrorLines()) {
					_scintillaBatchEditor.Lines[index].MarkerAdd(0);
				}

                _batchConsoleLog.Text = BatchController.CurrentBatch.GetParseErrorInfos();
			}
        }


		private void eventEditorDragEnter(object sender, DragEventArgs e) {
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
                    if (!isFileSupported(file)) {
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


        private void eventEditorDragDrop(object sender, DragEventArgs e) {
            try {
                if (e.Data == null) {
                    return;
                }

                object fileDropData = e.Data.GetData(DataFormats.FileDrop);
                if (fileDropData == null) {
                    return;
                }

                string[] files = (string[])fileDropData;
                if (files.Length != 1) {
                    return;
                }

                _updatingHTMLEnabled = false;
                LoadFile(files[0]);

                UpdateHTML();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
            finally {
                _updatingHTMLEnabled = true;
            }
        }


        private void clearStatusLabel() {
            _statusLabel.Text = string.Empty;
        }

        #endregion
        #region -  Events File Menu + Open, Save Dialog 

        private void eventLoadAutoCadFile_Click(object sender, EventArgs e) {
            try {
                _loadAutoCadFileDialog.InitialDirectory = Settings.Default.AutoCadDirectory;
                _loadAutoCadFileDialog.ShowDialog();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventOpenClick(object sender, EventArgs e) {
            try {
                _openFileDialog.InitialDirectory = Settings.Default.SvgDirectory;
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
                _saveFileDialog.FileName = _loadedFilename;
                if (!string.IsNullOrEmpty(_loadedFilename) && _loadedFilename.EndsWith(".g.svg")) {
                    _saveFileDialog.FilterIndex = 2;
                }
                else {
                    _saveFileDialog.FilterIndex = 1;
                }
                _saveFileDialog.InitialDirectory = Settings.Default.SvgDirectory;
                _saveFileDialog.ShowDialog();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventExportSelectedDefs_Click(object sender, EventArgs e) {
            try {
                IDictionary<string, TreeNode> selectedTreeNodes = new Dictionary<string, TreeNode>();
                collectFlatListOfTreeNodes(_defsTreeView.Nodes, selectedTreeNodes, true);

                XElement xElement = XElement.Parse(_scintillaSvgGroupEditor.Text);

                HashSet<string> defsIds = new HashSet<string>();
                foreach (KeyValuePair<string, TreeNode> selectedTreeNode in selectedTreeNodes) {
                    string id = selectedTreeNode.Key;
                    defsIds.Add(id);

                    DefsUtils.CollectUsedDefsIds(id, xElement, defsIds);
                }

                DefsUtils.CollectUsedDefsIds(xElement, defsIds, false);

                ExportSVGForm exportSvgForm = new ExportSVGForm(defsIds);
                if (exportSvgForm.ShowDialog() == DialogResult.OK) {
                    string outputPath = exportSvgForm.SelectedPath;
                    if (File.Exists(outputPath)) {
                        if (MessageBox.Show($"File {outputPath} exists, overwite?", "Export Selected Defs", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                            return;
                        }
                    }

                    DefsExporter exporter = new DefsExporter(_scintillaSvgGroupEditor.Text, exportSvgForm.SelectedDefsIds, exportSvgForm.ResolveDefs);
                    exporter.Export(outputPath);

                    if (exportSvgForm.AddExportToCurrentBatch) {
                        Batch batch = BatchController.CurrentBatch;
                        if (batch == null) {
                            _loadCommandBatchDialog.InitialDirectory = Settings.Default.CommandBatchDirectory;
                            _loadCommandBatchDialog.FileName = string.Empty;
                            _loadCommandBatchDialog.ShowDialog();
                            string batchPath = _loadCommandBatchDialog.FileName;
                            Settings.Default.CommandBatchDirectory = Path.GetDirectoryName(batchPath);
                            Settings.Default.Save();
                            batch = BatchController.LoadOrCreateBatch(batchPath);
                        }

						batch.AddCommand(new ExportCommand(_loadedDwgFilename, outputPath, exportSvgForm.ResolveDefs, false, exportSvgForm.SelectedDefsIds));

						_batchTabPage.Text = $"Batch: {batch.Name}";
                        _scintillaBatchEditor.Text = batch.ToString();
					}

                    if (exportSvgForm.OpenAfterExport) {
                        LoadFile(outputPath);
                    }
                }
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



        private void eventLoadAutoCadFile_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                string filename = _loadAutoCadFileDialog.FileName;
                Settings.Default.AutoCadDirectory = Path.GetDirectoryName(filename);
                Settings.Default.Save();

                _updatingHTMLEnabled = false;
                LoadFile(filename);

                UpdateHTML();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
            finally {
                _updatingHTMLEnabled = true;
            }
        }


        private void eventOpenFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                string filename = _openFileDialog.FileName;
                Settings.Default.SvgDirectory = Path.GetDirectoryName(filename);
                Settings.Default.Save();

                _updatingHTMLEnabled = false;
                LoadFile(filename);

                UpdateHTML();
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
            finally {
                _updatingHTMLEnabled = true;
            }
        }


        private void eventSaveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                int filterIndex = _saveFileDialog.FilterIndex;
                string filename = _saveFileDialog.FileName;

                switch (filterIndex) {
                case 1:
                    File.WriteAllText(filename, buildSVG(false, false, out _, true));
                    _loadedFilename = filename;
                    this.Text = $"{AppName} - {_loadedFilename}";
                    _contentChanged = false;

                    recentlyOpenedFilesManager.RegisterFile(filename);
                    updateRecentlyOpenedFiles();
                    break;
                case 2:
                    File.WriteAllText(filename, _scintillaSvgGroupEditor.Text);
                    _loadedFilename = filename;
                    this.Text = $"{AppName} - {_loadedFilename}";
                    _contentChanged = false;

                    recentlyOpenedFilesManager.RegisterFile(filename);
                    updateRecentlyOpenedFiles();
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
            try {
                _fontDialog.Font = Settings.Default.EditorFont;

                if (_fontDialog.ShowDialog() == DialogResult.OK) {
                    Font font = _fontDialog.Font;
                    setEditorFont(font);

                    Settings.Default.EditorFont = font;
                    Settings.Default.Save();
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void setEditorFont(Font font) {
            _scintillaSvgGroupEditor.Font = font;
            _scintillaScales.Font = font;
            _scintillaCss.Font = font;
            _scintillaBatchEditor.Font = font;
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
        #region -  Events Export Menu

        private void eventExecuteExportBatch_Click(object sender, EventArgs e) {
            try {
                //  TODO select tab
                Batch currentBatch = BatchController.CurrentBatch;
                if (currentBatch == null) {
                    string msg = "There is no current batch to be executed.";
					_statusLabel.Text = msg;
                    _batchConsoleLog.AppendText($"{msg}{Environment.NewLine}");
                    return;
                }
                if (currentBatch.IsEmpty) {
                    string msg = "The current batch does not yet contain any command.";
                    _statusLabel.Text = msg;
                    _batchConsoleLog.AppendText($"{msg}{Environment.NewLine}");
                    return;
                }
                if (currentBatch.HasErrors) {
                    string msg = "The current batch has parse errors.";
                    _statusLabel.Text = msg;
                    _batchConsoleLog.AppendText($"{msg}{Environment.NewLine}");
                    return;
                }

                _batchConsoleLog.Text = string.Empty;
                createConversionContext();
                currentBatch.Execute(_conversionContext, out string batchMsg);
				_batchConsoleLog.AppendText($"{batchMsg}{Environment.NewLine}");
			}
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventSaveExportBatch_Click(object sender, EventArgs e) {
            try {
                Batch currentBatch = BatchController.CurrentBatch;
                if (currentBatch == null) {
                    _statusLabel.Text = "There is no current batch to save.";
                    return;
                }

                if (string.IsNullOrEmpty(currentBatch.Name)) {
                    //  TODO open save dialog;
                    return;
                }
                currentBatch.Save();

                _statusLabel.Text = $"Batch: {currentBatch.Name} saved.";
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventLoadExportBatch_Click(object sender, EventArgs e) {
            try {
                Batch currentBatch = BatchController.CurrentBatch;
                if (currentBatch != null && currentBatch.HasChanges) {
                    var ret = MessageBox.Show(
                        $"CurrentBatch {currentBatch.Name} has changed has changed, save changes before loading or creating new batch?",
                        "Load Export Batch", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    switch (ret) {
                    case DialogResult.Yes:
                        currentBatch.Save();
                        _statusLabel.Text = "Current batch saved.";
                        break;
                    case DialogResult.No:
                        _statusLabel.Text = "Current batch will be discarded.";
                        break;
                    default:
                        return;
                    }
                }

                _loadCommandBatchDialog.InitialDirectory = Settings.Default.CommandBatchDirectory;
                _loadCommandBatchDialog.FileName = string.Empty;
                _loadCommandBatchDialog.ShowDialog();
            }
            catch (Exception ex){
                _statusLabel.Text = ex.Message;
            }
        }


        private void eventLoadCommandBatch_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                if (e.Cancel) {
                    return;
                }

                string path = _loadCommandBatchDialog.FileName;
                Settings.Default.CommandBatchDirectory = Path.GetDirectoryName(path);
                Settings.Default.Save();

                Batch batch = BatchController.LoadOrCreateBatch(path);
				_batchTabPage.Text = $"Batch: {batch.Name}";
				_scintillaBatchEditor.Text = batch.ToString();

                _statusLabel.Text = $"Batch: {batch.Name} loaded.";

                _tabControl.SelectedTab = _batchTabPage;
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

        #region -  SVG and HTML

        private string buildSVG(bool showScales, bool addCss, out bool isSvgEmpty, bool createFile = false) {
            isSvgEmpty = true;

            if (_conversionContext == null) {
                _conversionContext = new ConversionContext();
            }

            _conversionContext.UpdateSettings(
                _svgProperties.GetConversionOptions(),
                _svgProperties.GetViewbox(),
                _svgProperties.GetGlobalAttributeData());

            SvgElement svgElement = DocumentSvg.CreateSVG(_conversionContext);

            if (createFile) {
                svgElement.Style = "background-color:black;";
                svgElement.Width = _svgProperties.ViewBoxWidth.ToString();
                svgElement.Height = _svgProperties.ViewBoxHeight.ToString();
                svgElement.WithViewbox(null, null, null, null);
            }

            svgElement.AddCss(_scintillaCss.Text, addCss);
            svgElement.AddValue(_scintillaScales.Text, showScales);

            string editorText = _scintillaSvgGroupEditor.Text;
            if (!string.IsNullOrEmpty(editorText)) {
                if (createFile) {
                    try {
                        XElement xElement = XElement.Parse(editorText);
                        int factor = _svgProperties.ReverseY ? -1 : 1;
                        xElement.SetAttributeValue("transform", $"scale(1, {factor}) translate({_svgProperties.ViewBoxMinX}, {factor * _svgProperties.ViewBoxMinY})");
                        editorText = xElement.ToString();
                    }
                    catch (Exception e) {
                        _statusLabel.Text = e.Message;
                        throw;
                    }
                }
                svgElement.AddValue(editorText);
            }

            isSvgEmpty = string.IsNullOrEmpty(editorText);

            StringBuilder sb = new StringBuilder();
            if (createFile) {
                var declaration = new XDeclaration("1.0", null, null);
                var doctype = new XDocumentType("svg", " -//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", string.Empty).ToString();
                sb.AppendLine(declaration.ToString());
                sb.AppendLine(doctype.ToString());
            }

            XElement svg = svgElement.GetXml();
            sb.AppendLine(svgElement.ToString().Replace("&gt;", ">").Replace("&lt;", "<"));

            bool hasDefs = updateDefs(svg.Value);
            if (!hasDefs) {
                _defsTreeView.Nodes.Clear();
            }

            string svgText = sb.ToString();
            return svgText;
        }


        internal void ProposeUpdateHTML() {
            try {
                if (_updatingHTMLEnabled) {
                    UpdateHTML();
                }
            }
            catch (Exception ex) {
                _statusLabel.Text = ex.Message;
            }
        }


        public void CreateHTML() {
            string backgroundColor = ColorTranslator.ToHtml(Settings.Default.BackgroundColor);
            var svg = buildSVG(Settings.Default.ScalesEnabled, Settings.Default.CSSPreviewEnabled, out bool isSvgEmpty, false);
            string html = HTMLBuilder.Build(svg, backgroundColor);
            CefSharp.WebBrowserExtensions.LoadHtml(_webBrowser, html);
            updateBrowserContent(svg, isSvgEmpty, backgroundColor);
            centerToFit();

            svgViewerUserControl.BackColor = Settings.Default.BackgroundColor;
            svgViewerUserControl.LoadSvgContent(svg, true);
        }


        public void UpdateHTML() {
            if (_centerToFitOnLoad || _executingPanScriptFailed) {
                CreateHTML();
                _executingPanScriptFailed = false;
                _centerToFitOnLoad = false;
                return;
            }

            string backgroundColor = ColorTranslator.ToHtml(Settings.Default.BackgroundColor);
            var svg = buildSVG(Settings.Default.ScalesEnabled, Settings.Default.CSSPreviewEnabled, out bool isSvgEmpty, false);

            updateBrowserContent(svg, isSvgEmpty, backgroundColor);

            svgViewerUserControl.BackColor = Settings.Default.BackgroundColor;
            svgViewerUserControl.LoadSvgContent(svg, false);
        }


        private void updateBrowserContent(string svg, bool isSvgEmpty, string backgroundColor) {
            _webBrowser.WaitForInitialLoadAsync().Wait();

            if (_devToolsContext == null) {
                Task<DevToolsContext> t = _webBrowser.CreateDevToolsContextAsync();
                t.Wait();

                _devToolsContext = t.Result;
            }

            _devToolsContext.QuerySelectorAsync<HtmlBodyElement>("body")
                .Result.SetAttributeAsync("style", $"background-color:{backgroundColor};")
                .ContinueWith(t => {
                    _devToolsContext.QuerySelectorAsync<HtmlDivElement>("#svg-viewer")
                        .Result.SetInnerHtmlAsync(svg)
                        .ContinueWith(t => {
                            runPanZoomScript(isSvgEmpty);
                        });
                });

            _executingPanScriptFailed = false;
        }


        private void runPanZoomScript(bool isSvgEmpty) {
            if (!_webBrowser.CanExecuteJavascriptInMainFrame || isSvgEmpty) {
                return;
            }

            _webBrowser.ExecuteScriptAsyncWhenPageLoaded("resetZoomAndPan();");
        }

        #endregion
        #region -  WebBrowser functions

        public void centerToFit() {
            svgViewerUserControl.CenterToFit();

            if (!_webBrowser.CanExecuteJavascriptInMainFrame) {
                return;
            }

            _webBrowser.ExecuteScriptAsyncWhenPageLoaded("centerToFit();");
            //  System.Diagnostics.Debug.WriteLine($"Centered to Fit");
        }

        #endregion

        private void sVGViewerUserControlExperimentalToolStripMenuItem_Click(object sender, EventArgs e) {
            svgViewerUserControl.Show();
            _webBrowser.Hide();
        }

        private void webBrowserToolStripMenuItem_Click(object sender, EventArgs e) {
            svgViewerUserControl.Hide();
            _webBrowser.Show();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            svgViewerUserControl.Show();
            _webBrowser.Hide();
        }


        internal void UpdateSvgViewer() {
            svgViewerUserControl.DebugEnabled = Settings.Default.SvgViewerDebugEnabled;
        }
    }
}