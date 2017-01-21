using DotNetWrapperGen.CodeStructure;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DotNetWrapperGen.View
{
    public partial class CSharpFilesTab : UserControl
    {
        public CSharpFilesTab()
        {
            InitializeComponent();

            LoadIcon("folder");
        }

        private void LoadIcon(string imageName)
        {
            Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"DotNetWrapperGen.Images.{imageName}.png");
            Image icon = Image.FromStream(iconStream);
            imageList.Images.Add(imageName, icon);
        }

        public void SetData(RootFolderDefinition rootFolder)
        {
            if (rootFolder != null)
            {
                if (fileTree.Nodes.Count == 1 &&
                    fileTree.Nodes[0].Tag == rootFolder)
                {
                    //RefreshRoot(rootFolder, fileTree.Nodes[0].Nodes);
                }
                else
                {
                    //AddSourceItem(fileTree.Nodes, rootFolder);
                }
            }
        }
    }
}
