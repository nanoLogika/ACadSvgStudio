#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


namespace ACadSvgStudio {

	public partial class ConversionInfoForm : Form {

		public ConversionInfoForm()
		{
			InitializeComponent();
		}


		public void Open(string filename, string log, ISet<string> occurringEntities)
		{
			this.Text = "DWG Conversion Info: " + filename;

			_conversionLogScintilla.ReadOnly = false;
			_conversionLogScintilla.Text = log;
			_conversionLogScintilla.ReadOnly = true;

			_occurringEntitiesListBox.Items.Clear();
			_occurringEntitiesListBox.Items.AddRange(occurringEntities.ToArray<string>());
			this.Show();
		}
	}
}
