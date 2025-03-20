#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using System.Text;

namespace ACadSvgStudio {

	internal static class Program {
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {

			string? filename = null;
			string? blockRecordPath = null;

			if (args != null && args.Length > 0)
			{
				if (args.Length == 1)
				{
					filename = args[0];
				}
				else
				{
					StringBuilder inputSb = new StringBuilder();
					StringBuilder valueSb = new StringBuilder();
					StringBuilder[] sb = new StringBuilder[]
					{
						inputSb,
						valueSb
					};

					int readingArgId = -1;

					for (int i = 0; i < args.Length; i++)
					{
						switch (args[i])
						{
							case "-i":
								if (inputSb.Length == 0)
								{
									readingArgId = 0;
									continue;
								}
								break;
							case "-v":
								if (valueSb.Length == 0)
								{
									readingArgId = 1;
									continue;
								}
								break;
							default:
								break;
						}

						if (readingArgId >= 0 && readingArgId < sb.Length)
						{
							sb[readingArgId]
								.Append(args[i])
								.Append(" ");
						}
					}


					string value = valueSb.ToString().Trim();
					string input = inputSb.ToString().Trim();

					if (value.Contains("#"))
					{
						filename = value.Substring(0, value.IndexOf("#"));
						blockRecordPath = value.Substring(value.IndexOf("#") + 1);
					}
					else
					{
						filename = value;
					}

					filename = Path.Combine(input, filename);
				}
			}

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			MainForm form = new MainForm();
			if (!string.IsNullOrEmpty(filename)) {
				if (File.Exists(filename))
				{
					form.LoadFile(filename);

					if (!string.IsNullOrEmpty(blockRecordPath))
					{
						form.UpdateHTML();

						string blockRecordName;
						if (blockRecordPath.Contains("/"))
						{
							blockRecordName = blockRecordPath.Substring(blockRecordPath.IndexOf("/") + 1);
						}
						else
						{
							blockRecordName = blockRecordPath;
						}

						blockRecordName = "_" + blockRecordName.Replace("_", "__").Replace(" ", "_");

						if (form.TryGetTreeNode(blockRecordName, out TreeNode treeNode))
						{
							treeNode.Checked = true;
						}
						else
						{
							MessageBox.Show($"Block Record \"{blockRecordName}\" does not exist!", "Block Record Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
				else
				{
					MessageBox.Show($"File \"{filename}\" does not exist!", "Load File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			Application.Run(form);
		}
	}
}