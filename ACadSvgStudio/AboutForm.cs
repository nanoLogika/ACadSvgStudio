#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ACadSvgStudio {

	public partial class AboutForm : Form {

		internal class AboutItem {

			public string Name { get; set; }


			public string Version { get; set; }


			public string Description { get; set; }


			public string License { get; set; }


			public string URL { get; set; }


			public override string ToString() {
				StringBuilder sb = new StringBuilder();
				sb.Append(Name);
				if (!string.IsNullOrEmpty(Version)) {
					sb.Append($" ({Version})");
				}
				return sb.ToString();
			}
		}


		public AboutForm() {
			InitializeComponent();

			Text = $"About {MainForm.AppName}";
			titleLabel.Text = MainForm.AppName;

			versionLabel.Text = $"Version: {Application.ProductVersion}";
			licenseLabel.Text = "License: LGPL-3.0";

			companyLinkLabel.Text = "https://www.nanologika.de/";


			addItem("ACadSvg", "C# library to convert AutoCAD drawings from DWG to SVG. AutoCAD files are read using ACadSharp.", "https://github.com/nanoLogika/ACadSvg", "LGPL-3.0");
			addItem("SvgElements", "C# library providing an object model to create SVG/XML elements and attributes.", "https://github.com/nanoLogika/SvgElements", "LGPL-3.0");
			addItem("ACadSharp", "C# library to read/write cad files like dxf/dwg.", "https://github.com/DomCR/ACadSharp", "MIT");
			addItem("CommandLine", "The best C# command line parser that brings standardized *nix getopt style, for .NET. Includes F# support", "https://github.com/commandlineparser/commandline", "MIT");
			addItem("Scintilla.NET", "Source Editing Component based on Scintilla 5 series.", "https://github.com/VPKSoft/Scintilla.NET", "MIT");
			addItem("ScintillaNET_FindReplaceDialog", "Find and replace dialog for the Scintilla.NET.", "https://github.com/VPKSoft/ScintillaNET-FindReplaceDialog", "MIT");
			addItem("Svg", "SVG.NET is a C# library to read, write and render SVG 1.1 images in applications based on the .NET framework.", "https://github.com/svg-net/SVG", "MS-PL");

			updateAboutItem();
		}


		private void addItem(string name, string description, string url, string license) {
			AboutItem aboutItem = new AboutItem();
			aboutItem.Name = name;
			aboutItem.Description = description;
			aboutItem.URL = url;
			aboutItem.License = license;

			Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in loadedAssemblies) {
				AssemblyName assemblyName = assembly.GetName();
				if (assemblyName.Name == name) {
					if (assemblyName.Version != null) {
						aboutItem.Version = assemblyName.Version.ToString();
					}
				}
			}

			listBox.Items.Add(aboutItem);
		}

		private void listBox_SelectedIndexChanged(object sender, EventArgs e) {
			updateAboutItem();
		}

		private void companyLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			openLink(companyLinkLabel.Text);
		}

		private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			AboutItem aboutItem = (AboutItem)listBox.SelectedItem;
			openLink(aboutItem.URL);
		}

		private void projectLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			openLink("https://github.com/nanoLogika/ACadSvgStudio");
		}

		private void okButton_Click(object sender, EventArgs e) {
			Close();
		}


		private void openLink(string url) {
			Process.Start(new ProcessStartInfo(url) {
				UseShellExecute = true
			});
		}


		private void updateAboutItem() {
			if (listBox.SelectedIndex == -1) {
				linkLabel.Visible = false;
				detailsLabel.Visible = false;
				return;
			}


			AboutItem aboutItem = (AboutItem)listBox.SelectedItem;
			linkLabel.Text = aboutItem.URL;
			linkLabel.Visible = true;


			StringBuilder detailsSb = new StringBuilder();
			detailsSb.AppendLine(aboutItem.Description);
			detailsSb.AppendLine();
			detailsSb.AppendLine($"License: {aboutItem.License}");

			detailsLabel.Text = detailsSb.ToString();
			detailsLabel.Visible = true;
		}
	}
}
