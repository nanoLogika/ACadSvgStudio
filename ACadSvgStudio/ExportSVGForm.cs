#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using ACadSvgStudio.Defs;

namespace ACadSvgStudio {

	public partial class ExportSVGForm : Form {

		public string FileName {
			get {
				return filenameTextBox.Text;
			}
			set {
				filenameTextBox.Text = value;
			}
		}


		public bool ResolveDefs {
			get {
				return resolveDefsCheckBox.Checked;
			}
			set {
				resolveDefsCheckBox.Checked = value;
			}
		}


		public string[] SelectedDefsIds {
			get {
				HashSet<string> result = new HashSet<string>();

				foreach (DefsListViewItem item in checkedListBox.CheckedItems) {
					result.Add(item.DefsId);
				}

				return result.ToArray();
			}
		}


		private SortedSet<string> _defsIds;


		public ExportSVGForm() {
			InitializeComponent();
		}


		public ExportSVGForm(string filename, HashSet<string> defsIds) : this() {
			FileName = filename;

			_defsIds = new SortedSet<string>();
			foreach (string id in defsIds) {
				_defsIds.Add(id);
			}

			foreach (string defsId in _defsIds) {
				DefsListViewItem defsListViewItem = new DefsListViewItem(defsId);
				checkedListBox.Items.Add(defsListViewItem);
			}

			for (int x = 0; x < checkedListBox.Items.Count; x++) {
				checkedListBox.SetItemChecked(x, true);
			}
		}


		private void ExportSVGForm_Load(object sender, EventArgs e) {
			resolveDefsCheckBox.Checked = Settings.Default.ResolveDefs;
		}


		private void filenameTextBox_TextChanged(object sender, EventArgs e) {
			FileName = filenameTextBox.Text;
		}

		private void browseButton_Click(object sender, EventArgs e) {
			_saveFileDialog.FileName = FileName;

			if (!string.IsNullOrEmpty(FileName) && FileName.ToLower().EndsWith(".g.svg")) {
				_saveFileDialog.FilterIndex = 2;
			}
			else {
				_saveFileDialog.FilterIndex = 1;
			}

			if (_saveFileDialog.ShowDialog() == DialogResult.OK) {
				FileName = _saveFileDialog.FileName;
			}
		}

		private void resolveDefsCheckBox_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.ResolveDefs = resolveDefsCheckBox.Checked;
			Settings.Default.Save();
		}
	}
}
