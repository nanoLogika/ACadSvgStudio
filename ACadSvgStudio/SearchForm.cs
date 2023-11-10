#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using System.Text.RegularExpressions;


namespace ACadSvgStudio {

	public partial class SearchForm : Form {

		private MainForm _mainForm;
		private int _searchIndex = 0;
		private string _lastSearch = null;


		public SearchForm() {
			InitializeComponent();
		}

		public SearchForm(MainForm mainForm) : this() {
			this._mainForm = mainForm;
			int l = _mainForm.Text.IndexOf(" -");
			if (l == -1) {
				l = _mainForm.Text.Length;
			}
			this.Text = _mainForm.Text.Substring(0, l) + " Search";
		}

		private void eventSearchForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (Visible) {
				e.Cancel = true;
			}

			Hide();
		}


		private void eventSearchButton_Click(object sender, EventArgs e) {
			search();
		}


		private void search() {
			string searchText = textBox1.Text;

			MatchCollection matchCollection = Regex.Matches(_mainForm.SvgText, searchText);
			int matchesCount = matchCollection.Count;

			if (matchesCount > 0) {
				if (searchText == _lastSearch) {
					if (_searchIndex == matchesCount - 1) {
						_searchIndex = 0;
					}
					else {
						_searchIndex++;
					}
				}
				else {
					_searchIndex = 0;
				}

				int index = matchCollection[_searchIndex].Index;

				_mainForm.SetSelection(true, index, searchText.Length);
			}

			_lastSearch = searchText;

			if (matchesCount > 1) {
				matchesToolStripStatusLabel.Text = $"Matches: {_searchIndex + 1} of {matchesCount}";
			}
			else {
				matchesToolStripStatusLabel.Text = $"Matches: {matchesCount}";
			}
		}
	}
}
