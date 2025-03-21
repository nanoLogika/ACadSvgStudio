#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using System.Xml.Linq;
using SvgElements;


namespace ACadSvgStudio.Defs {

    internal static class DefsTreeUtils {

        public static IDictionary<string, TreeNode> CollectFlatListOfSelectedTreeNodes(TreeView devsTreeView) {
            IDictionary<string, TreeNode> nodes = new Dictionary<string, TreeNode>();
            collectFlatListOfTreeNodes(devsTreeView.Nodes, nodes, true);
            return nodes;
        }


        public static void UpdateDefsTreeView(string xmlValue, TreeView defsTreeView) {
            if (string.IsNullOrWhiteSpace(xmlValue)) {
                defsTreeView.Nodes.Clear();
                return;
            }

            XElement xElement = XElement.Parse(xmlValue);

            List<DefsItem> defsItems = new List<DefsItem>();
            DefsUtils.FindAllDefs(xElement, null, defsItems);

            if (defsItems.Count == 0) {
                defsTreeView.Nodes.Clear();
                return;
            }

            IDictionary<string, TreeNode> prevTreeNodes = collectFlatListOfTreeNodes(defsTreeView);

            List<TreeNode> newTreeNodes = new List<TreeNode>();

            foreach (DefsItem defsItem in defsItems) {
                TreeNode node = createDefsTreeNode(defsItem);
                newTreeNodes.Add(node);
            }

            //	Build new tree but restore tags and expanded/collapsed from previous
            //	all nodes are unchecked.
            defsTreeView.Nodes.Clear();
            foreach (TreeNode node in newTreeNodes) {
                defsTreeView.Nodes.Add(node);
                applyFlatListOfTreeNodes(node, prevTreeNodes);
            }

            //	Scan list of <use> tags
            IList<UseElement> usedDefsItems = DefsUtils.FindAllUsedDefs(xElement);
            IList<string> assignedDefIds = new List<string>();
            foreach (UseElement useElement in usedDefsItems) {
                string id = useElement.GroupId.Substring(1);

                TreeNode node = findNode(defsTreeView.Nodes, id);
                if (node != null) {
                    if (useElement.X != 0 || useElement.Y != 0) {
                        node.Text = $"{id} ({useElement.X}, {useElement.Y})";
                    }
                    //	Only expand to List of UseElement when Tag was set here before
                    if (assignedDefIds.Contains(id)) {
                        if (node.Tag is UseElement tagUseElement) {
                            node.Tag = new List<UseElement>() { tagUseElement, useElement };
                        }
                        else if (node.Tag is IList<UseElement>) {
                            ((List<UseElement>)node.Tag).Add(useElement);
                        }
                        node.Text = $"{id} (multiple pos)";
                    }
                    else {
                        //	This may override a tag from the previous tree vie
                        node.Tag = useElement;
                        assignedDefIds.Add(id);
                    }

                    node.Checked = true;
                }
            }
        }


        private static IDictionary<string, TreeNode> collectFlatListOfTreeNodes(TreeView devsTreeView) {
            IDictionary<string, TreeNode> nodes = new Dictionary<string, TreeNode>();
            collectFlatListOfTreeNodes(devsTreeView.Nodes, nodes);
            return nodes;
        }


        private static void collectFlatListOfTreeNodes(TreeNodeCollection nodes, IDictionary<string, TreeNode> flatListOfTreeNodes, bool selectedOnly = false) {
            foreach (TreeNode node in nodes) {
                if (selectedOnly) {
                    if (node.Checked) {
                        flatListOfTreeNodes.Add(node.FullPath, node);
                    }
                }
                else {
                    flatListOfTreeNodes.Add(node.FullPath, node);
                }

                collectFlatListOfTreeNodes(node.Nodes, flatListOfTreeNodes, selectedOnly);
            }
        }


        //private static string getTreeNodeKey(TreeNode node) {
        //    TreeNode currentNode = node;
        //    string key = currentNode.Name;
        //    while (currentNode.Parent != null) {
        //        key = $"{currentNode.Parent.Name}/{key}";
        //        currentNode = currentNode.Parent;
        //    }
        //    return key;
        //}


        private static TreeNode createDefsTreeNode(DefsItem defsItem) {
            TreeNode treeNode = new TreeNode() {
                Name = defsItem.Id,
                Text = defsItem.Id,
                Tag = new UseElement().WithGroupId(defsItem.Id)
            };

            foreach (DefsItem childDefsItem in defsItem.Children) {
                TreeNode childTreeNode = createDefsTreeNode(childDefsItem);
                treeNode.Nodes.Add(childTreeNode);
            }

            return treeNode;
        }


        private static TreeNode findNode(TreeNodeCollection nodes, string id) {
            foreach (TreeNode node in nodes) {
                if (node.Name == id) {
                    return node;
                }
                else {
                    TreeNode foundNode = findNode(node.Nodes, id);
                    if (foundNode != null) {
                        return foundNode;
                    }
                }
            }
            return null;
        }


        private static void applyFlatListOfTreeNodes(TreeNode newTreeNode, IDictionary<string, TreeNode> prevTreeNodes) {
            if (prevTreeNodes.TryGetValue(newTreeNode.Name, out TreeNode prevNode)) {
                if (prevNode.IsExpanded) {
                    newTreeNode.Expand();
                }
                else {
                    newTreeNode.Collapse();
                }
                //	Extended Text and UseElements may be present at previous tree node
                newTreeNode.Text = prevNode.Text;
                newTreeNode.Tag = prevNode.Tag;
            }

            foreach (TreeNode node in newTreeNode.Nodes) {
                applyFlatListOfTreeNodes(node, prevTreeNodes);
            }
        }
    }
}
