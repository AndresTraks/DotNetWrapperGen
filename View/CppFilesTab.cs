using DotNetWrapperGen.CodeStructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

            var iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DotNetWrapperGen.Images.folder.png");
            var icon = Image.FromStream(iconStream);
            imageList1.Images.Add("folder", icon);

            iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DotNetWrapperGen.Images.header.png");
            icon = Image.FromStream(iconStream);
            imageList1.Images.Add("header", icon);

            fileTree.MouseUp += sourceTree_MouseUp;
            fileTree.NodeMouseDoubleClick += sourceTree_NodeMouseDoubleClick;
            fileTree.KeyPress += sourceTree_KeyPress;

            sourceItemSettings.PropertyValueChanged += sourceItemSettings_PropertyValueChanged;
        }

        public TreeView SourceTree => fileTree;

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
            sourceItemSettings.SelectedObject = new SourceItemPropertyPage(item, e.Node);
        }

        private void showExcludedButton_CheckedChanged(object sender, EventArgs e)
        {
            if (_rootFolder != null)
            {
                SetData(_rootFolder);
            }
        }

        private void flatViewButton_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode node in fileTree.Nodes)
            {
                node.Nodes.Clear();
            }
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
                if (fileTree.Nodes.Count == 1 &&
                    fileTree.Nodes[0].Tag == rootFolder)
                {
                    RefreshRoot(rootFolder, fileTree.Nodes[0].Nodes);
                }
                else
                {
                    AddSourceItem(fileTree.Nodes, rootFolder);
                }
            }
        }


        private void RefreshRoot(RootFolderDefinition rootFolder, TreeNodeCollection nodes)
        {
            foreach (SourceItemDefinition item in rootFolder.Children)
            {
                TreeNode node = GetTreeNodeByTag(nodes, item);
                if (item.IsFolder)
                {
                    if (node != null && flatViewButton.Checked)
                    {
                        node.Remove();
                    }
                    else if (node == null && !flatViewButton.Checked)
                    {
                        AddSourceItem(nodes, item as FolderDefinition);
                    }
                }
                else
                {
                    if (node == null)
                    {
                        AddSourceItem(nodes, item as HeaderDefinition);
                    }
                }
            }

            if (flatViewButton.Checked)
            {
                return;
            }

            bool reiterate;
            do
            {
                reiterate = false;
                foreach (TreeNode node in nodes)
                {
                    if (rootFolder.Children.Contains(node.Tag as SourceItemDefinition))
                    {
                        if (RefreshNode(node))
                        {
                            reiterate = true;
                            break;
                        }
                    }
                    else
                    {
                        node.Remove();
                        reiterate = true;
                        break;
                    }
                }
            } while (reiterate);
        }

        private void AddSourceItem(TreeNodeCollection nodes, SourceItemDefinition item)
        {
            if (!showExcludedButton.Checked && item.IsExcluded)
            {
                return;
            }

            if (item.IsFolder && flatViewButton.Checked)
            {
                foreach (var child in InputFileOrder(item.Children))
                {
                    AddSourceItem(nodes, child);
                }
                return;
            }

            var newNode = nodes.Add(item.Name);
            newNode.Tag = item;
            if (item.IsExcluded)
            {
                newNode.ForeColor = Color.Gray;
            }
            if (item.IsFolder)
            {
                newNode.ImageKey = "folder";
                newNode.SelectedImageKey = "folder";

                foreach (var child in InputFileOrder(item.Children))
                {
                    AddSourceItem(newNode.Nodes, child);
                }
            }
            else
            {
                newNode.ImageKey = "header";
                newNode.SelectedImageKey = "header";
            }
        }

        private IOrderedEnumerable<SourceItemDefinition> InputFileOrder(IEnumerable<SourceItemDefinition> sourceItems)
        {
            return sourceItems
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
