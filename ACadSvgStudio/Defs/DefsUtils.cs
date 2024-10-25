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


        internal static void FindAllDefs(XElement xElement, DefsItem parentDefsItem, List<DefsItem> defsItems) {
            foreach (XElement child in xElement.Elements()) {
                string childName = child.Name.ToString().ToLower();
                if (childName == "defs") {
                    FindAllDefsGroups(child, null, defsItems);
                }
                else {
                    FindAllDefs(child, parentDefsItem, defsItems);
                }
            }
        }


        internal static void FindAllDefsGroups(XElement xElement, DefsItem parentDefsItem, List<DefsItem> defsItems, int level = 1) {
            foreach (XElement child in xElement.Elements()) {
                string childName = child.Name.ToString().ToLower();
                if (childName == "g" && child.Attribute("id") != null && (level > 1 || (level == 1 && child.Attribute("class") != null && child.Attribute("class")!.Value == "block-record"))) {
                    string id = child.Attribute("id")!.Value;
                    DefsItem defsItem = new DefsItem(id);

                    if (parentDefsItem == null) {
                        defsItems.Add(defsItem);
                    }
                    else {
                        parentDefsItem.Children.Add(defsItem);
                    }

                    FindAllDefsGroups(child, defsItem, defsItems, level + 1);
                }
                else {
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
    }
}
