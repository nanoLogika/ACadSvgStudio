#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using ACadSvgStudio.Defs;

namespace ACadSvgStudio {

	public partial class ExportSVGForm : Form {

		public string SelectedPath {
			get {
				return Path.Combine(_directoryTextBox.Text, _filenameTextBox.Text);
			}
		}


		public bool ResolveDefs {
			get {
				return _resolveDefsCheckBox.Checked;
			}
		}


		public string[] SelectedDefsIds {
			get {
				HashSet<string> result = new HashSet<string>();

				foreach (DefsListViewItem item in _checkedListBox.CheckedItems) {
					result.Add(item.DefsId);
				}

				return result.ToArray();
			}
		}


		private SortedSet<string> _defsIds;


		public ExportSVGForm() {
			InitializeComponent();
		}


		public ExportSVGForm(HashSet<string> defsIds) : this() {
            _directoryTextBox.Text = Settings.Default.SvgDirectory;
            
			_defsIds = new SortedSet<string>();
			foreach (string id in defsIds) {
				_defsIds.Add(id);
				if (!id.StartsWith("_")) {
					_filenameTextBox.Text = id + ".g.svg";
				}
			}

			foreach (string defsId in _defsIds) {
				DefsListViewItem defsListViewItem = new DefsListViewItem(defsId);
				_checkedListBox.Items.Add(defsListViewItem);
			}

			for (int x = 0; x < _checkedListBox.Items.Count; x++) {
				_checkedListBox.SetItemChecked(x, true);
			}
            _exportButton.Enabled = Directory.Exists(_directoryTextBox.Text) && !string.IsNullOrWhiteSpace(_filenameTextBox.Text);
        }


        private void ExportSVGForm_Load(object sender, EventArgs e) {
			_resolveDefsCheckBox.Checked = Settings.Default.ResolveDefs;
			_exportButton.Enabled = File.Exists(SelectedPath);
        }


        private void browseButton_Click(object sender, EventArgs e) {
			_saveFileDialog.InitialDirectory = Settings.Default.SvgDirectory;
			_saveFileDialog.FileName = string.Empty;

			if (_saveFileDialog.ShowDialog() == DialogResult.OK) {
                _filenameTextBox.Text = Path.GetFileName(_saveFileDialog.FileName);
				_directoryTextBox.Text = Path.GetDirectoryName(_saveFileDialog.FileName);
                Settings.Default.SvgDirectory = _directoryTextBox.Text;
				Settings.Default.Save();
                _exportButton.Enabled = Directory.Exists(_directoryTextBox.Text) && !string.IsNullOrWhiteSpace(_filenameTextBox.Text);
            }
        }


		private void eventResolveDefsCheckBox_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.ResolveDefs = _resolveDefsCheckBox.Checked;
			Settings.Default.Save();
		}


        private void eventFilenameTextBox_TextChanged(object sender, EventArgs e) {
			try {
				_exportButton.Enabled = Directory.Exists(_directoryTextBox.Text) && !string.IsNullOrWhiteSpace(_filenameTextBox.Text);
			}
			catch {
				_exportButton.Enabled = false;
			}
        }
    }
}
