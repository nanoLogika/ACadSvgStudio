#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


namespace ACadSvgStudio {

	internal static class Program {
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {

			string filename = args != null && args.Length > 0 ? args[0] : null;

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			MainForm form = new MainForm();
			if (!string.IsNullOrEmpty(filename) && File.Exists(filename)) {
				form.LoadFile(filename);
			}
			Application.Run(form);
		}
	}
}