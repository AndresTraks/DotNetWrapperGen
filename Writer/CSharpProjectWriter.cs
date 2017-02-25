using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Project;
using System.IO;

namespace DotNetWrapperGen.Writer
{
    public class CSharpWriter
    {
        private WrapperProject _project;

        public CSharpWriter(WrapperProject project)
        {
            _project = project;
        }

        public void Write()
        {
            WriteItem(_project.RootFolderCSharp);
        }

        private void WriteItem(SourceItemDefinition item)
        {
            var folder = item as FolderDefinition;
            if (folder != null)
            {
                Directory.CreateDirectory(folder.FullPath);
            }

            var rootFolder = item as RootFolderDefinition;
            if (rootFolder != null)
            {
                Directory.CreateDirectory(rootFolder.FullPath);
            }

            var header = item as HeaderDefinition;
            if (header != null)
            {
                WriteHeader(header);
            }

            foreach (SourceItemDefinition child in item.Children)
            {
                WriteItem(child);
            }
        }

        private void WriteHeader(HeaderDefinition header)
        {
            if (header.Nodes.Count == 0)
            {
                return;
            }

            var fileWriter = new CSharpFileWriter(header);
            fileWriter.Write();
        }
    }
}
