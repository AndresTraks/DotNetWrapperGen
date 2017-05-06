using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DotNetWrapperGen.View
{
    public partial class CppFilesTab : UserControl
    {
        private RootFolderDefinition _rootFolder;

        public CppFilesTab()
        {
            InitializeComponent();

            LoadIcon("folder");
            LoadIcon("header");

            fileTree.MouseUp += sourceTree_MouseUp;
            fileTree.NodeMouseDoubleClick += sourceTree_NodeMouseDoubleClick;
            fileTree.KeyPress += sourceTree_KeyPress;

            sourceItemSettings.PropertyValueChanged += sourceItemSettings_PropertyValueChanged;
        }

        public TreeView SourceTree => fileTree;

        private void LoadIcon(string imageName)
        {
            Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"DotNetWrapperGen.Images.{imageName}.png");
            Image icon = Image.FromStream(iconStream);
            imageList.Images.Add(imageName, icon);
        }

        void sourceItemSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            RefreshNode((sourceItemSettings.SelectedObject as SourceItemPropertyPage).SourceNode);
        }

        static void OpenSourceItem(SourceItemDefinition item)
        {
            var proc = new Process();
            proc.StartInfo.FileName = item.FullPath;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        void sourceTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var node = fileTree.SelectedNode;
            if (node == null)
            {
                return;
            }
            var item = node.Tag as SourceItemDefinition;
            if (!item.IsFolder)
            {
                OpenSourceItem(item);
            }
        }

        void sourceTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            var p = new Point(e.X, e.Y);
            var node = fileTree.GetNodeAt(p);
            if (node == null)
            {
                return;
            }
            var item = node.Tag as SourceItemDefinition;
            if (item == null)
            {
                return;
            }
            var menu = new ContextMenu();
            menu.MenuItems.Add(new MenuItem("Open", (o, i) => OpenSourceItem(item)));
            menu.MenuItems.Add(new MenuItem(item.IsExcluded ? "Include" : "Exclude", (o, i) =>
            {
                if (showExcludedButton.Checked)
                {
                    item.IsExcluded = !item.IsExcluded;
                    node.ForeColor = item.IsExcluded ? Color.Gray : Color.Black;
                }
                else
                {
                    item.IsExcluded = true;
                    node.Remove();
                }
            }));
            menu.Show(fileTree, p);
        }

        void sourceTree_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                var node = fileTree.SelectedNode;
                if (node == null)
                {
                    return;
                }
                OpenSourceItem(node.Tag as SourceItemDefinition);
                e.Handled = true;
            }
        }

        private void sourceTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            sourceItemSettings.Enabled = true;
            var item = e.Node.Tag as SourceItemDefinition;
            sourceItemSettings.SelectedObject = new SourceItemPropertyPage
            {
                SourceItem = item,
                SourceNode = e.Node
            };
        }

        private void showExcludedButton_CheckedChanged(object sender, EventArgs e)
        {
            if (_rootFolder != null)
            {
                SetData(_rootFolder);
            }
        }

        public void SetData(RootFolderDefinition rootFolder)
        {
            _rootFolder = rootFolder;
            if (rootFolder != null)
            {
                RefreshItemNodes(new[] { rootFolder }, fileTree.Nodes);
                fileTree.Nodes[0].Expand();
            }
            else
            {
                fileTree.Nodes.Clear();
            }
        }


        private void RefreshItemNodes(IEnumerable<SourceItemDefinition> items, TreeNodeCollection nodes)
        {
            var existingItems = new Dictionary<SourceItemDefinition, TreeNode>();

            int nodeIndex = 0;
            while (nodeIndex < nodes.Count)
            {
                TreeNode node = nodes[nodeIndex];
                var item = items.FirstOrDefault(i => i == node.Tag);
                if (item != null && (showExcludedButton.Checked || !item.IsExcluded))
                {
                    existingItems.Add(item, node);
                    nodeIndex++;
                }
                else
                {
                    node.Remove();
                }
            }

            foreach (SourceItemDefinition item in items)
            {
                if (showExcludedButton.Checked || !item.IsExcluded)
                {
                    TreeNode node;
                    if (!existingItems.TryGetValue(item, out node))
                    {
                        node = nodes.Add(item.Name);
                        node.Tag = item;
                    }
                    SetNodeProperties(item, node);
                    RefreshItemNodes(InputFileOrder(item.Children), node.Nodes);
                }
            }
        }

        private void SetNodeProperties(SourceItemDefinition item, TreeNode node)
        {
            if (item.IsExcluded)
            {
                node.ForeColor = Color.Gray;
            }
            if (item.IsFolder)
            {
                node.ImageKey = "folder";
                node.SelectedImageKey = "folder";
            }
            else
            {
                node.ImageKey = "header";
                node.SelectedImageKey = "header";
            }
        }

        private IOrderedEnumerable<SourceItemDefinition> InputFileOrder(IEnumerable<SourceItemDefinition> item)
        {
            return item
                .OrderBy(i => !i.IsFolder)
                .ThenBy(i => i.IsExcluded);
        }

        private static TreeNode GetTreeNodeByTag(TreeNodeCollection nodes, SourceItemDefinition item)
        {
            foreach (TreeNode node in nodes)
            {
                if (ReferenceEquals(node.Tag, item))
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Caller should not continue to iterate the node collection
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Was item removed?</returns>
        private bool RefreshNode(TreeNode node)
        {
            var item = node.Tag as SourceItemDefinition;
            if (item.IsExcluded)
            {
                if (showExcludedButton.Checked)
                {
                    node.ForeColor = Color.Gray;
                }
                else
                {
                    if (node.Parent != null)
                    {
                        node.Remove();
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                bool hasError = (item as HeaderDefinition)?.HasError ?? false;
                node.ForeColor = hasError ? Color.Red : Color.Black;
            }

            //throw new NotImplementedException();
            //RefreshRoot(item, node.Nodes);
            return false;
        }
    }
}
