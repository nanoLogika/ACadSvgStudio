#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using System.Text;
using System.Xml.Linq;

using CefSharp;
using CefSharp.WinForms;
using ScintillaNET;

using ACadSvg;
using System.Xml;

namespace ACadSvgStudio {

	public partial class MainForm : Form {

		private const string AppName = "ACad SVG Studio";

		private SvgProperties _svgProperties;

		private ConversionContext _conversionContext;

		private Scintilla _scintillaSvgGroupEditor;
		private Scintilla _scintillaCss;
		private Scintilla _scintillaScales;
		private ChromiumWebBrowser _webBrowser;
		private bool centerToFitOnLoad = false;

		private SearchForm _searchForm;

		private int _maxLineNumberCharLength;
		private string? _loadedFilename;
		private string? _loadedDwgFilename;


		//  Current-conversion Info
		private bool _contentChanged;
		private ISet<string> _occurringEntities;
		private string _conversionLog;

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
			propertyGrid.SelectedObject = _svgProperties;

			_scintillaSvgGroupEditor.BorderStyle = BorderStyle.FixedSingle;
			_scintillaScales.Text = Settings.Default.ScalesSvg;
			_scintillaCss.Text = Settings.Default.CSSPreview;

			_searchForm = new SearchForm(this);

			UpdateHTML();
		}


		internal void LoadFile(string filename) {
			string ext = Path.GetExtension(filename).ToLower();

			switch (ext) {
			case ".svg":
				_scintillaSvgGroupEditor.Text = File.ReadAllText(filename);
				_loadedFilename = filename;
				this.Text = $"{AppName} - {filename}";
				_contentChanged = false;
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
		}


		internal string SvgText {
			get {
				return _scintillaSvgGroupEditor.Text;
			}

			set {
				_scintillaSvgGroupEditor.Text = value;
				_contentChanged = true;
			}
		}


		internal void SetSelection(bool clear, int index, int length) {
			if (clear) {
				_scintillaSvgGroupEditor.ClearSelections();
			}
			_scintillaSvgGroupEditor.SetSelection(index, index + length);
			_scintillaSvgGroupEditor.ScrollCaret();
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

		#region -  Init Scintilla editors and Web Browser                                           -

		private void initScintillaSVGGroupEditor() {
			_scintillaSvgGroupEditor = new ScintillaNET.Scintilla();
			_scintillaSvgGroupEditor.Dock = DockStyle.Fill;
			_scintillaSvgGroupEditor.TextChanged += eventScintilla_TextChanged;
			updateLineMargin(_scintillaSvgGroupEditor);
			mainGroupTabPage.Controls.Add(_scintillaSvgGroupEditor);


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
			scalesTabPage.Controls.Add(_scintillaScales);

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
			cssTabPage.Controls.Add(_scintillaCss);


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
			splitContainer2.Panel1.Controls.Add(_webBrowser);

			_webBrowser.MenuHandler = new MyContextMenuHandler(this);


			_webBrowser.LoadingStateChanged += (s, e) => {
				if (!e.IsLoading && centerToFitOnLoad) {
					centerToFit();
					centerToFitOnLoad = false;
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


		private void eventExit_Click(object sender, EventArgs e) {
			Close();
		}


		private void eventScintilla_TextChanged(object? sender, EventArgs e) {
			string panX = string.Empty;
			string panY = string.Empty;
			string zoom = string.Empty;

			if (!centerToFitOnLoad) {
				getPan(out panX, out panY);
				getZoom(out zoom);
			}

			UpdateHTML();
			updateLineMargin(sender);
			_contentChanged = true;

			if (!centerToFitOnLoad) {
				Task.Delay(100).ContinueWith(x => {
					centerToFit();
					setZoom(zoom);
					setPan(panX, panY);
				});
			}
		}

		#endregion
		#region -  Events                                                                           -

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


		public string GetSVG(bool showScales, bool addCss, bool addDeclAndType = false) {
			if (_conversionContext == null) {
				_conversionContext = new ConversionContext();
			}

			_conversionContext.UpdateSettings(
				_svgProperties.GetConversionOptions(),
				_svgProperties.GetViewbox(),
				_svgProperties.GetGlobalAttributeData());

			XElement svg = EntitySvg.CreateSVG(_conversionContext).GetXml();


			string css = _scintillaCss.Text;
			if (addCss && !string.IsNullOrEmpty(css)) {
				XElement styleXElement = new XElement("style");
				styleXElement.Add(new XAttribute("type", "text/css"));
				styleXElement.Add(XElement.Parse(css));

				svg.Add(styleXElement);
			}

			string scales = _scintillaScales.Text;
			if (showScales && !string.IsNullOrEmpty(scales)) {
				svg.Add(XElement.Parse(scales));
			}

			string editorText = _scintillaSvgGroupEditor.Text;
			if (!string.IsNullOrEmpty(editorText)) {
				svg.Add(XElement.Parse(_scintillaSvgGroupEditor.Text));
			}

			StringBuilder sb = new StringBuilder();
			if (addDeclAndType) {
				var declaration = new XDeclaration("1.0", null, null);
				var doctype = new XDocumentType("svg", " -//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", string.Empty).ToString();
				sb.AppendLine(declaration.ToString());
				sb.AppendLine(doctype.ToString());
				sb.AppendLine(svg.ToString().Replace("&gt;", ">").Replace("&lt;", "<"));
            }
			sb.AppendLine(svg.ToString());

			return sb.ToString();
		}


		public void UpdateHTML() {
			string backgroundColor = ColorTranslator.ToHtml(Settings.Default.BackgroundColor);

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("<!DOCTYPE html>");
			sb.AppendLine("<html>");

			sb.AppendLine("<head>");
			sb.AppendLine("<meta charset=\"utf-8\" />");

			sb.AppendLine("<style>html, body { margin: 0; padding: 0; } body { width: 100%; height: 100%; }</style>");

			sb.AppendLine(@"<script language=""JavaScript"">" + Resources.svg_pan_zoom + "</script>");
			sb.AppendLine("</head>");

			sb.AppendLine($"<body style=\"background-color:{backgroundColor};\">");

			try {
				sb.AppendLine(GetSVG(Settings.Default.ScalesEnabled, Settings.Default.CSSPreviewEnabled));
			}
			catch (Exception ex) {

			}

			sb.AppendLine("<script>");
			sb.AppendLine("var panZoom = svgPanZoom('#svg-element', { minZoom: 0.0001, maxZoom: 1000 });");
			sb.AppendLine("</script>");

			sb.AppendLine("</body>");
			sb.AppendLine("</html>");

			CefSharp.WebBrowserExtensions.LoadHtml(_webBrowser, sb.ToString());
		}


		public void SetZoomLevel(double value) {
			CefSharp.WebBrowserExtensions.SetZoomLevel(_webBrowser, value);
		}


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


		private void eventSearch_Click(object sender, EventArgs e) {
			_searchForm.Show(this);
		}


		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (_contentChanged) {
				switch (MessageBox.Show("Content has been changed, save changes?", "Close", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)) {
				case DialogResult.Yes:
					eventSaveSvgGroupClick(sender, EventArgs.Empty);
					//  TODO Cancel save must be handeled.
					break;
				case DialogResult.Cancel:
					e.Cancel = true;
					break;
				}
			}
		}


		private void eventOpenClick(object sender, EventArgs e) {
			openFileDialog.ShowDialog();
		}


		private void eventSaveSvgGroupClick(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(_loadedFilename)) {
				eventSaveSvgGroupAsClick(sender, e);
				return;
			}
			File.WriteAllText(_loadedFilename, _scintillaSvgGroupEditor.Text);
			_contentChanged = false;
		}


		private void eventSaveSvgGroupAsClick(object sender, EventArgs e) {
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.ShowDialog();
		}



		private void eventSaveSvgGroupFileAsDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.ShowDialog();
		}


		private void eventSaveSvgFileClick(object sender, EventArgs e) {
			saveFileDialog.FilterIndex = 1;
			saveFileDialog.ShowDialog();
		}


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
		}


		private void getPan(out string panX, out string panY) {
			panX = string.Empty;
			panY = string.Empty;

			if (!_webBrowser.CanExecuteJavascriptInMainFrame) {
				return;
			}

			var response = _webBrowser.EvaluateScriptAsync("'' + panZoom.getPan().x + '|' + panZoom.getPan().y;").Result;
			if (response.Success && response.Result != null) {
				string[] result = response.Result.ToString().Split("|");

				panX = result[0].ToString();
				panY = result[1].ToString();
			}
		}


		private void setPan(string panX, string panY) {
			if (!_webBrowser.CanExecuteJavascriptInMainFrame) {
				return;
			}

			string panStr = "panZoom.pan({x: " + panX + ", y: " + panY + "});";
			var response = _webBrowser.EvaluateScriptAsync(panStr).Result;
			if (response.Success) {
				string value = response.Result.ToString();
			}
		}


		private void getZoom(out string zoom) {
			zoom = string.Empty;

			if (!_webBrowser.CanExecuteJavascriptInMainFrame) {
				return;
			}

			var response = _webBrowser.EvaluateScriptAsync("'' + panZoom.getZoom()").Result;
			if (response.Success && response.Result != null) {
				string result = response.Result.ToString();
				zoom = result;
			}
		}


		private void setZoom(string zoom) {
			if (!_webBrowser.CanExecuteJavascriptInMainFrame) {
				return;
			}

			string zoomStr = "panZoom.zoom(" + zoom + ");";
			var response = _webBrowser.EvaluateScriptAsync(zoomStr).Result;
			if (response.Success) {
				string value = response.Result.ToString();
			}
		}


		private void centerToFitToolStripMenuItem_Click(object sender, EventArgs e) {
			centerToFit();
		}


		private void showDeveloperToolsToolStripMenuItem_Click(object sender, EventArgs e) {
			_webBrowser.ShowDevTools();
		}


		private void showConversionLog_Click(object sender, EventArgs e) {
			try {
				var conversionLogForm = new ConversionInfoForm();
				conversionLogForm.Open(_loadedDwgFilename, _conversionLog, _occurringEntities);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}


		private void flipContent_Click(object sender, EventArgs e) {
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
				MessageBox.Show(ex.ToString());
			}
		}


		private void MainForm_Load(object sender, EventArgs e) {
			propertyGridToolStripMenuItem.Checked = Settings.Default.ViewPropertyGrid;
			splitContainer2.Panel2Collapsed = !propertyGridToolStripMenuItem.Checked;


			if (Settings.Default.WindowPositionInitialized) {
				Location = Settings.Default.WindowPosition;
				Size = Settings.Default.WindowSize;

				splitContainer1.SplitterDistance = Settings.Default.SplitContainer1Distance;
				splitContainer2.SplitterDistance = Settings.Default.SplitContainer2Distance;
			}
			else {
				Settings.Default.WindowPositionInitialized = true;
				saveWindowState();
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

			Settings.Default.SplitContainer1Distance = splitContainer1.SplitterDistance;
			Settings.Default.SplitContainer2Distance = splitContainer2.SplitterDistance;

			Settings.Default.Save();
		}


		private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
			saveWindowState();
		}


		private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
			try {
				int filterIndex = openFileDialog.FilterIndex;
				string filename = openFileDialog.FileName;

				centerToFitOnLoad = true;

				switch (filterIndex) {
				case 1: // ".dwg";
					readACadFile(filename, "DWG");
					break;
				case 2: // ".svg"
					_scintillaSvgGroupEditor.Text = File.ReadAllText(filename);
					break;
				default:
					break;
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}


		private void saveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
			try {
				int filterIndex = saveFileDialog.FilterIndex;
				string filename = saveFileDialog.FileName;

				switch (filterIndex) {
				case 1:
					File.WriteAllText(filename, GetSVG(false, false, true));
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
				MessageBox.Show(ex.Message);
			}
		}


		private void propertyGridToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
			splitContainer2.Panel2Collapsed = !propertyGridToolStripMenuItem.Checked;

			Settings.Default.ViewPropertyGrid = propertyGridToolStripMenuItem.Checked;
			Settings.Default.Save();
		}


		private void centerToFitToolStripMenuItem1_Click(object sender, EventArgs e) {
			centerToFitToolStripMenuItem_Click(sender, e);
		}


		private void showDeveloperToolsToolStripMenuItem1_Click(object sender, EventArgs e) {
			showDeveloperToolsToolStripMenuItem_Click(sender, e);
		}


		private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e) {
			_scintillaSvgGroupEditor.FoldAll(FoldAction.Contract);
		}


		private void expandAllToolStripMenuItem_Click(object sender, EventArgs e) {
			_scintillaSvgGroupEditor.FoldAll(FoldAction.Expand);
		}
	}

	#endregion
}