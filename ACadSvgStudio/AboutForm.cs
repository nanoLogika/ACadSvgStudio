#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using System.Diagnostics;
using System.Text;
using static ACadSvgStudio.AboutForm;

namespace ACadSvgStudio {

	public partial class AboutForm : Form {

		internal class AboutItem {

			public string Name { get; set; }


			public string Version { get; set; }


			public string Description { get; set; }


			public string License { get; set; }


			public string URL { get; set; }


			public override string ToString()
			{
				return $"{Name} ({Version})";
			}
		}


		public AboutForm()
		{
			InitializeComponent();

			Text = $"About {MainForm.AppName}";
			titleLabel.Text = MainForm.AppName;

			versionLabel.Text = $"Version: {Application.ProductVersion}";
			licenseLabel.Text = "License: LGPL-3.0";

			companyLinkLabel.Text = "https://www.nanologika.de/";


			addItem("ACadSvg", "0.4.0", "C# library to convert AutoCAD drawings from DWG to SVG. AutoCAD files are read using ACadSharp.", "https://github.com/nanoLogika/ACadSvg", "LGPL-3.0");
			addItem("SvgElements", "0.3.0", "C# library providing an object model to create SVG/XML elements and attributes.", "https://github.com/nanoLogika/SvgElements", "LGPL-3.0");
			addItem("ACadSharp", "1.5.1-alpha", "C# library to read/write cad files like dxf/dwg.", "https://github.com/DomCR/ACadSharp", "MIT");
			addItem("Scintilla.NET", "5.3.2.9", "Source Editing Component based on Scintilla 5 series.", "https://github.com/VPKSoft/Scintilla.NET", "MIT");
			addItem("ScintillaNET_FindReplaceDialog", "1.5.5", "Find and replace dialog for the Scintilla.NET.", "https://github.com/VPKSoft/ScintillaNET-FindReplaceDialog", "MIT");
			addItem("CefSharp.WinForms.NETCore", "113.1.40", "The CefSharp Chromium-based browser component (WinForms control).", "https://github.com/cefsharp/cefsharp", "...");
			addItem("CefSharp.Common.NETCore", "113.1.40", "The CefSharp Chromium-based browser component (WinForms control).", "https://github.com/cefsharp/cefsharp", "...");
			addItem("CefSharp.Dom", "2.0.86", "CefSharp.Dom - A strongly typed DOM API for use with with CefSharp, based on PuppeteerSharp.", "https://github.com/cefsharp/CefSharp.Dom", "MIT");
			addItem("chromiumembeddedframework.runtime.win-arm64", "113.1.4", "Chromium Embedded Framework (CEF) Release Distribution", "https://github.com/cefsharp/cef-binary", "...");
			addItem("chromiumembeddedframework.runtime.win-x64", "113.1.4", "Chromium Embedded Framework (CEF) Release Distribution", "https://github.com/cefsharp/cef-binary", "...");
			addItem("chromiumembeddedframework.runtime.win-x86", "113.1.4", "Chromium Embedded Framework (CEF) Release Distribution", "https://github.com/cefsharp/cef-binary", "...");
			addItem("chromiumembeddedframework.runtime", "113.1.4", "Chromium Embedded Framework (CEF) Release Distribution", "https://github.com/cefsharp/cef-binary", "...");
			addItem("svg-pan-zoom", "3.6.1", "JavaScript library that enables panning and zooming of an SVG in an HTML document, with mouse events or custom JavaScript hooks", "https://github.com/bumbu/svg-pan-zoom/tree/master", "MIT");

			updateAboutItem();
		}


		private void addItem(string name, string version, string description, string url, string license)
		{
			AboutItem aboutItem = new AboutItem();
			aboutItem.Name = name;
			aboutItem.Version = version;
			aboutItem.Description = description;
			aboutItem.URL = url;
			aboutItem.License = license;

			listBox.Items.Add(aboutItem);
		}

		private void listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateAboutItem();
		}

		private void companyLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			openLink(companyLinkLabel.Text);
		}

		private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			AboutItem aboutItem = (AboutItem)listBox.SelectedItem;
			openLink(aboutItem.URL);
		}

		private void projectLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			openLink("https://github.com/nanoLogika/ACadSvgStudio");
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			Close();
		}


		private void openLink(string url)
		{
			Process.Start(new ProcessStartInfo(url)
			{
				UseShellExecute = true
			});
		}


		private void updateAboutItem()
		{
			if (listBox.SelectedIndex == -1)
			{
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
