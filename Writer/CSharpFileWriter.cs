using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Parser;
using System.IO;
using System.Linq;

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
            var namespaceTree = HeaderNamespaceTree.GetTree(_header);

            using (var stream = File.Create(_header.FullPath))
            {
                using (_writer = new StreamWriter(stream))
                {
                    WriteNamespace(namespaceTree);
                }
            }
        }

        private void WriteNamespace(NamespaceTreeNode node)
        {
            string name;

            if (node.Children.Count == 1 && node.Nodes.Count == 0)
            {
                var child = node.Children[0];
                if (node.Namespace.IsGlobal)
                {
                    name = child.Namespace.Name;
                }
                else
                {
                    name = $"{node.Namespace.Name}.{child.Namespace.Name}";
                }
                node = child;
            }
            else
            {
                name = node.Namespace.Name;
            }

            var @namespace = node.Namespace;

            _writer.Write("namespace ");
            _writer.WriteLine(name);
            _writer.WriteLine("{");
            _writer.WriteLine("}");

            foreach (var childNamespace in node.Children)
            {
                WriteNamespace(childNamespace);
            }
        }
    }
}
