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
		private bool _resolveDefs;


		public DefsExporter(string doc, bool resolveDefs) {
			_doc = XDocument.Parse(doc);
			_resolveDefs = resolveDefs;
		}


		private XElement createRootElement() {
			XElement xElement = new XElement("g");
			foreach (XAttribute xAttribute in _doc.Root!.Attributes()) {
				xElement.Add(xAttribute);
			}
			return xElement;
		}


		public void Export(string path, string[] selectedDefsIds) {
            
			XDocument doc = new XDocument();

			XElement root = createRootElement();

			HashSet<string> useElements = new HashSet<string>();
			DefsUtils.CollectUseElements(_doc.Root!, useElements);

			XElement defs = new XElement("defs");
			foreach (string id in selectedDefsIds) {
				XElement? def = DefsUtils.FindGroupById(id, _doc.Root!);
				if (def != null) {
					defs.Add(def);
					continue;
				}
				def = DefsUtils.FindPatternById(id, _doc.Root!);
				if (def != null) {
					defs.Add(def);
					continue;
				}

				throw new InvalidOperationException($"Specified block with id={id} not found.");
			}

			if (defs.HasElements) {
				if (_resolveDefs) {
					foreach (XElement defsElement in defs.Elements()) {
                        IEnumerable<XElement> defsElements = defs.Elements();
                        DefsUtils.RemoveUseElements(defsElement, defsElements);
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
