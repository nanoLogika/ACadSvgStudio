#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using System.Text;

namespace ACadSvgStudio {

	internal class HTMLBuilder {

		public static string Build(string svg, string backgroundColor)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("<!DOCTYPE html>");
			sb.AppendLine("<html>");

			sb.AppendLine("<head>");
			sb.AppendLine("<meta charset=\"utf-8\" />");

			sb.AppendLine("<style>html, body { margin: 0; padding: 0; } body { width: 100%; height: 100%; }</style>");

			sb.AppendLine(@"<script language=""JavaScript"">" + Resources.svg_pan_zoom + "</script>");
			sb.AppendLine("</head>");

			sb.AppendLine($"<body style=\"background-color:{backgroundColor};\">");

			sb.AppendLine("<div id=\"svg-viewer\">");
			sb.AppendLine(svg);
			sb.AppendLine("</div>");

			sb.AppendLine("</body>");
			sb.AppendLine("</html>");
			return sb.ToString();
		}

		public static string GetPanZoomScript() {
			string script = string.Empty;

			// Destroy panZoom if already exists
			script += "window.panZoomWasDefined = window.panZoom != null && window.panZoom != undefined;";
			script += "window.prevPan = (window.panZoomWasDefined ? window.panZoom.getPan() : undefined);";
			script += "window.prevZoom = (window.panZoomWasDefined ? window.panZoom.getZoom() : undefined);";

			script += "if(window.panZoomWasDefined && window.panZoom != undefined) {window.panZoom.destroy();}";

			script += "window.panZoom = svgPanZoom('#svg-element', { minZoom: 0.0001, maxZoom: 1000 });";

			script += "if(window.panZoomWasDefined) {";
			script += "window.panZoom.zoom(window.prevZoom);";
			script += "window.panZoom.pan({x: window.prevPan.x, y: window.prevPan.y});";
			script += "}";

			return script;
		}

	}
}
