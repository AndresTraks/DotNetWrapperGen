using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Project;
using DotNetWrapperGen.Tokenizer;
using DotNetWrapperGen.Tokenizer.CSharp;
using System.IO;

namespace DotNetWrapperGen.Writer.CSharp
{
    public class CSharpProjectWriter
    {
        private readonly WrapperProject _project;

        public CSharpProjectWriter(WrapperProject project)
        {
            _project = project;
        }

        public void Write()
        {
            WriteItem(_project.RootFolderCSharp);
        }

        private void WriteItem(SourceItemDefinition item)
        {
            if (item is FolderDefinition folder)
            {
                Directory.CreateDirectory(folder.FullPath);
            }

            if (item is RootFolderDefinition rootFolder)
            {
                Directory.CreateDirectory(rootFolder.FullPath);
            }

            if (item is HeaderDefinition header)
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

            var fileWriter = new CSharpFileTokenizer(header);
            TokenizerContext context = fileWriter.Tokenize();

            var tokenWriter = new TokenWriter();
            tokenWriter.Write(header.FullPath, context);
        }
    }
}
