using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Parser;
using System.IO;
using System.Linq;

namespace DotNetWrapperGen.Writer
{
    public class CSharpFileWriter
    {
        private readonly HeaderDefinition _header;
        private StreamWriter _writer;

        public CSharpFileWriter(HeaderDefinition header)
        {
            _header = header;
        }

        public void Write()
        {
            NamespaceTreeNode namespaceTree = HeaderNamespaceTree.GetTree(_header);

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

            _writer.WriteLine($"namespace {name}");
            _writer.WriteLine("{");

            foreach (ModelNodeDefinition childNode in node.Nodes)
            {
                WriteNode(childNode);
            }

            _writer.WriteLine("}");

            foreach (var childNamespace in node.Children)
            {
                WriteNamespace(childNamespace);
            }
        }

        private void WriteNode(ModelNodeDefinition childNode)
        {
            var @class = childNode as ClassDefinition;
            if (@class != null)
            {
                var abstractSpecifier = @class.IsAbstract ? "abstract " : null;
                var baseClassSpecifier = @class.BaseClass != null ? @class.BaseClass.ManagedName : null;

                _writer.WriteLine($"\tpublic {abstractSpecifier}class {childNode.Name}");
                _writer.WriteLine("\t{");

                var constructors = @class.Methods.Where(m => m.IsConstructor);
                foreach (MethodDefinition constructor in constructors)
                {
                    WriteMethod(constructor);
                }

                var methods = @class.Methods.Where(m => !m.IsConstructor);
                foreach (MethodDefinition method in methods)
                {
                    WriteMethod(method);
                }

                _writer.WriteLine("\t}");
            }
        }

        private void WriteMethod(MethodDefinition method)
        {
            if (method.IsExcluded)
            {
                return;
            }

            _writer.WriteLine($"\t\t{method.ManagedName}()");
            _writer.WriteLine("\t\t{");
            _writer.WriteLine("\t\t}");
        }
    }
}
