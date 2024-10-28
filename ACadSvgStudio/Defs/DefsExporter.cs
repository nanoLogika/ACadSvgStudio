#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using System.Xml.Linq;

namespace ACadSvgStudio.Defs {

	internal class DefsExporter {

		private XDocument _doc;
		private ICollection<string> _selectedDefsIds;


		public DefsExporter(string doc, ICollection<string> selectedDefsIds) {
			_doc = XDocument.Parse(doc);
			_selectedDefsIds = selectedDefsIds;
		}


		private XElement createRootElement() {
			XElement xElement = new XElement("g");
			foreach (XAttribute xAttribute in _doc.Root!.Attributes()) {
				xElement.Add(xAttribute);
			}
			return xElement;
		}


		private XElement? findGroupById(string id, XElement e) {
			string elementName = e.Name.ToString().ToLower();
			if (elementName == "defs") {
				foreach (XElement xElement in e.Elements()) {
					XElement? g = findGroupById(id, xElement);
					if (g != null) {
						return g;
					}
				}
			}
			else if (elementName == "g") {
				XAttribute? idXAttribute = e.Attribute("id");
				if (idXAttribute != null && idXAttribute.Value == id) {
					return e;
				}

				foreach (XElement xElement in e.Elements()) {
					XElement? g = findGroupById(id, xElement);
					if (g != null) {
						return g;
					}
				}
			}

			return null;
		}


		private void collectUseElements(XElement e, HashSet<string> useElements) {
			string elementName = e.Name.ToString().ToLower();
			if (elementName == "use") {
				XAttribute? href = e.Attribute("href");
				if (href != null && !string.IsNullOrEmpty(href.Value)) {
					useElements.Add(href.Value);
				}
			}
			else {
				foreach (XElement child in e.Elements()) {
					collectUseElements(child, useElements);
				}
			}
		}


		public void Export(string path) {
			XDocument doc = new XDocument();

			XElement root = createRootElement();

			foreach (string defsId in _selectedDefsIds) {
				XElement? g = findGroupById(defsId, _doc.Root!);
				if (g != null) {
					root.Add(g);
				}
			}

			HashSet<string> useElements = new HashSet<string>();
			collectUseElements(root, useElements);

			XElement defs = new XElement("defs");
			foreach (string useElement in useElements) {
				string id = useElement.Substring(1);
				XElement? def = findGroupById(id, _doc.Root!);
				if (def != null) {
					defs.Add(def);
				}
			}
			if (defs.HasElements) {
				root.Add(defs);
			}

			doc.Add(root);

			string xml = doc.ToString();

			File.WriteAllText(path, xml);
		}
	}
}
