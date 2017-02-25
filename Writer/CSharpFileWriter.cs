using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Parser;
using System.IO;

namespace DotNetWrapperGen.Writer
{
    public class CSharpFileWriter
    {
        private HeaderDefinition _header;
        private StreamWriter _writer;

        public CSharpFileWriter(HeaderDefinition header)
        {
            _header = header;
        }

        public void Write()
        {
            var globalNamespace = HeaderNamespaceTree.GetTree(_header);

            using (var stream = File.Create(_header.FullPath))
            {
                using (_writer = new StreamWriter(stream))
                {
                    WriteNamespace(globalNamespace);
                }
            }
        }

        private void WriteNamespace(NamespaceTreeNode node)
        {
            var @namespace = node.Namespace;

            if (!@namespace.IsGlobal)
            {
                _writer.Write("namespace ");
                _writer.WriteLine(@namespace.Name);
                _writer.WriteLine("{");
                _writer.WriteLine("}");
            }

            foreach (var childNamespace in node.Children)
            {
                WriteNamespace(childNamespace);
            }
        }
    }
}
