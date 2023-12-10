#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using System.Text;

namespace ACadSvgStudio {

	internal class HTMLBuilder {

		public static string Build(string svg, string backgroundColor) {
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("<!DOCTYPE html>");
			sb.AppendLine("<html>");

			sb.AppendLine("<head>");
			sb.AppendLine("<meta charset=\"utf-8\" />");

			sb.AppendLine("<style>html, body { margin: 0; padding: 0; } body { width: 100%; height: 100%; }</style>");

			sb.AppendLine(@"<script language=""JavaScript"">" + Resources.svg_pan_zoom + "</script>");
			sb.AppendLine(@"<script language=""JavaScript"">" + Resources.reset_pan_zoom + "</script>");

			sb.AppendLine("</head>");

			sb.AppendLine($"<body style=\"background-color:{backgroundColor};\">");

			sb.AppendLine("<div id=\"svg-viewer\">");
			sb.AppendLine(svg);
			sb.AppendLine("</div>");

			sb.AppendLine("</body>");
			sb.AppendLine("</html>");
			return sb.ToString();
		}
	}
}
