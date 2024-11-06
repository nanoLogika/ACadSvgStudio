#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using System.Globalization;
using System.Xml.Linq;

using SvgElements;

namespace ACadSvgStudio.Defs {

    internal static class DefsUtils {

        internal static List<XElement> FindUseElements(string id, XElement xElement) {
            List<XElement> list = new List<XElement>();

            foreach (XElement child in xElement.Elements()) {
                string childName = child.Name.ToString().ToLower();
                if (childName == "use" && child.Attribute("href") != null && child.Attribute("href")!.Value == $"#{id}") {
                    list.Add(child);
                }
            }

            return list;
        }


		internal static void CollectUseElements(XElement xElement, HashSet<string> useElements) {
			string name = xElement.Name.ToString().ToLower();

			if (name == "use") {
				XAttribute? href = xElement.Attribute("href");
				if (href != null && !string.IsNullOrEmpty(href.Value)) {
					useElements.Add(href.Value);
				}
			}
			else {
				foreach (XElement child in xElement.Elements()) {
					CollectUseElements(child, useElements);
				}
			}
		}


		internal static void CollectUsedDefsIds(XElement xElement, HashSet<string> defsIds, bool searchDefs = false) {
			string name = xElement.Name.ToString().ToLower();

			if (name == "use" && xElement.Attribute("href") != null && !string.IsNullOrEmpty(xElement.Attribute("href")!.Value)) {
				string id = xElement.Attribute("href")!.Value;
				id = id.StartsWith("#") ? id.Substring(1) : id;
				defsIds.Add(id);
			}
			else if (name == "defs" && searchDefs) {
                foreach (XElement child in xElement.Elements()) {
					CollectUsedDefsIds(child, defsIds, searchDefs);
				}
			}
			else if (name != "defs") {
                foreach (XElement child in xElement.Elements()) {
					CollectUsedDefsIds(child, defsIds, searchDefs);
				}
			}
        }

        internal static void CollectUsedDefsIds(string id, XElement xElement, HashSet<string> defsIds) {
            string name = xElement.Name.ToString().ToLower();

			if (name == "g" && xElement.Attribute("id") != null && xElement.Attribute("id").Value == id) {
				foreach (XElement child in xElement.Elements()) {
					CollectUsedDefsIds(child, defsIds, true);
				}
			}

			foreach (XElement child in xElement.Elements()) {
                CollectUsedDefsIds(id, child, defsIds);
            }
        }


        internal static void FindAllDefs(XElement xElement, DefsItem parentDefsItem, List<DefsItem> defsItems) {
			string name = xElement.Name.ToString().ToLower();

            if (name == "defs") {
                foreach (XElement child in xElement.Elements()) {
					FindAllDefsGroups(child, null, defsItems);
				}
            }
            else {
                foreach (XElement child in xElement.Elements()) {
					FindAllDefs(child, parentDefsItem, defsItems);
				}
            }
        }


        internal static void FindAllDefsGroups(XElement xElement, DefsItem parentDefsItem, List<DefsItem> defsItems, int level = 1) {
			string name = xElement.Name.ToString().ToLower();

			if (name == "g" && xElement.Attribute("id") != null && (level > 1 || (level == 1 && xElement.Attribute("class") != null && xElement.Attribute("class")!.Value == "block-record"))) {
				string id = xElement.Attribute("id")!.Value;
				DefsItem defsItem = new DefsItem(id);

				if (parentDefsItem == null) {
					defsItems.Add(defsItem);
				}
				else {
					parentDefsItem.Children.Add(defsItem);
				}

				foreach (XElement child in xElement.Elements()) {
					FindAllDefsGroups(child, defsItem, defsItems, level + 1);
				}
			}
			else {
				foreach (XElement child in xElement.Elements()) {
					FindAllDefsGroups(child, parentDefsItem, defsItems, 1);
				}
			}
        }


        internal static IList<UseElement> FindAllUsedDefs(XElement xElement, bool searchChildren = false) {
            IList<UseElement> useElements = new List<UseElement>();
            foreach (XElement child in xElement.Elements()) {
                string childName = child.Name.ToString().ToLower();
                if (childName == "use" && child.Attribute("href") != null) {
                    string id = child.Attribute("href")!.Value;
                    string xAtt = child.Attribute("x")?.Value;
                    string yAtt = child.Attribute("y")?.Value;
                    string transformAtt = child.Attribute("transform")?.Value;
                    if (!double.TryParse(xAtt, NumberStyles.Any, CultureInfo.InvariantCulture, out double x)) {
                        x = 0;
                    }
                    if (!double.TryParse(yAtt, NumberStyles.Any, CultureInfo.InvariantCulture, out double y)) {
                        y = 0;
                    }
                    UseElement useElement = new UseElement()
                        .WithGroupId(id)
                        .WithXY(x, y);
                    useElement.Transform = transformAtt;
                    useElements.Add(useElement);
                }
                else if (searchChildren) {
                    FindAllUsedDefs(child);
                }
            }
            return useElements;
        }


		internal static XElement? FindElementById(string elementName, string id, XElement xElement) {
			string name = xElement.Name.ToString().ToLower();

			if (name == "defs") {
				foreach (XElement child in xElement.Elements()) {
					XElement? elm = FindElementById(elementName, id, child);
					if (elm != null) {
						return elm;
					}
				}
			}
			else if (name == elementName) {
				XAttribute? idXAttribute = xElement.Attribute("id");
				if (idXAttribute != null && idXAttribute.Value == id) {
					return xElement;
				}

				foreach (XElement child in xElement.Elements()) {
					XElement? elm = FindElementById(elementName, id, child);
					if (elm != null) {
						return elm;
					}
				}
			}

			return null;
		}

		internal static XElement? FindGroupById(string id, XElement xElement) {
			return FindElementById("g", id, xElement);
		}

		internal static XElement? FindPatternById(string id, XElement xElement) {
			return FindElementById("pattern", id, xElement);
		}


		internal static void RemoveUseElements(XElement xElement) {
            xElement.Elements("use").Remove();

            foreach (XElement child in xElement.Elements()) {
                RemoveUseElements(child);
            }
        }
    }
}
