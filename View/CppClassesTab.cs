using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.Project;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DotNetWrapperGen.View
{
    public partial class CppClassesTab : UserControl
    {
        private WrapperProject _project;

        public CppClassesTab()
        {
            InitializeComponent();

            LoadIcon("folder");
            LoadIcon("class");
            LoadIcon("method");
            LoadIcon("enum");
            LoadIcon("typeref");
        }

        private void LoadIcon(string imageName)
        {
            Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"DotNetWrapperGen.Images.{imageName}.png");
            Image icon = Image.FromStream(iconStream);
            imageList.Images.Add(imageName, icon);
        }

        private void parseFilesButton_Click(object sender, EventArgs e)
        {
            _project.ParseAsync();
        }

        public void SetData(WrapperProject project)
        {
            _project = project;

            if (project != null)
            {
                parseFilesButton.Enabled = true;
                AddModelNode(project.GlobalNamespace, null);
            }
            else
            {
                parseFilesButton.Enabled = false;
            }
        }

        private void AddModelNode(ModelNodeDefinition modelNode, TreeNode parentTreeNode)
        {
            if (parentTreeNode == null)
            {
                parentTreeNode = classTree.Nodes.Add("global");
                parentTreeNode.Expand();
            }
            else
            {
                parentTreeNode = parentTreeNode.Nodes.Add(modelNode.Name);
            }
            SetNodeProperties(modelNode, parentTreeNode);

            foreach (var child in modelNode.Children
                .OrderBy(GetNodeTypeOrder)
                .ThenBy(c => c.Name))
            {
                AddModelNode(child, parentTreeNode);
            }
        }

        private int GetNodeTypeOrder(ModelNodeDefinition node)
        {
            if (node is NamespaceDefinition) return 0;
            if (node is ClassDefinition) return 1;
            var method = node as MethodDefinition;
            if (method != null)
            {
                if (method.IsConstructor) return 2;
                return 3;
            }
            return 4;
        }

        private void SetNodeProperties(ModelNodeDefinition modelNode, TreeNode treeNode)
        {
            if (modelNode is NamespaceDefinition)
            {
                treeNode.ImageKey = "folder";
                treeNode.SelectedImageKey = "folder";
            }
            else if (modelNode is ClassDefinition)
            {
                treeNode.ImageKey = "class";
                treeNode.SelectedImageKey = "class";
            }
            else if (modelNode is MethodDefinition)
            {
                treeNode.ImageKey = "method";
                treeNode.SelectedImageKey = "method";
            }
            else if (modelNode is TypeRefDefinition)
            {
                treeNode.ImageKey = "typeref";
                treeNode.SelectedImageKey = "typeref";
            }
            else
            {
                treeNode.ImageIndex = 5;
                treeNode.SelectedImageIndex = 5;
            }
        }
    }
}
