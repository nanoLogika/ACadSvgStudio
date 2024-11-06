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
		private bool _resolveDefs;


		public DefsExporter(string doc, ICollection<string> selectedDefsIds, bool resolveDefs) {
			_doc = XDocument.Parse(doc);
			_selectedDefsIds = selectedDefsIds;
			_resolveDefs = resolveDefs;
		}


		private XElement createRootElement() {
			XElement xElement = new XElement("g");
			foreach (XAttribute xAttribute in _doc.Root!.Attributes()) {
				xElement.Add(xAttribute);
			}
			return xElement;
		}


		public void Export(string path) {
			XDocument doc = new XDocument();

			XElement root = createRootElement();

			foreach (string defsId in _selectedDefsIds) {
				XElement? g = DefsUtils.FindGroupById(defsId, _doc.Root!);
				if (g != null) {
					root.Add(g);
				}
			}

			HashSet<string> useElements = new HashSet<string>();
			DefsUtils.CollectUseElements(_doc.Root!, useElements);

			XElement defs = new XElement("defs");
			foreach (string useElement in useElements) {
				string id = useElement.Substring(1);
				XElement? def = DefsUtils.FindGroupById(id, _doc.Root!);
				if (def != null) {
					defs.Add(def);
				}
				else {
					def = DefsUtils.FindPatternById(id, _doc.Root!);
					if (def != null) {
						defs.Add(def);
					}
				}
			}

			if (defs.HasElements) {
				if (_resolveDefs) {
					foreach (XElement defsElement in defs.Elements()) {
						root.Add(defsElement);
					}

					DefsUtils.RemoveUseElements(root);
				}
				else {
					root.Add(defs);
				}
			}

			doc.Add(root);

			string xml = doc.ToString();

			File.WriteAllText(path, xml);
		}
	}
}
