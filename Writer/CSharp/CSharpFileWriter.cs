using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Parser;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetWrapperGen.Writer.CSharp
{
    public class CSharpFileWriter
    {
        private readonly HeaderDefinition _header;
        private StreamWriter _writer;
        private CSharpMethodWriter _methodWriter;

        private IDictionary<string, INodeWriter> _writers = new Dictionary<string, INodeWriter>();

        private string[] _nodeTypeOrder = new[] {
            "constructor",
            "method",
            "class",
            "enum"
        };

        public CSharpFileWriter(HeaderDefinition header)
        {
            _header = header;

            _writers["enum"] = new EnumWriter();
        }

        public void Write()
        {
            NamespaceTreeNode namespaceTree = HeaderNamespaceTree.GetTree(_header);

            using (var stream = File.Create(_header.FullPath))
            {
                using (_writer = new StreamWriter(stream))
                {
                    _methodWriter = new CSharpMethodWriter(_writer);

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

        private void WriteNode(ModelNodeDefinition node)
        {
            var @class = node as ClassDefinition;
            if (@class != null)
            {
                var abstractSpecifier = @class.IsAbstract ? "abstract " : null;
                var baseClassSpecifier = @class.BaseClass != null ? @class.BaseClass.ManagedName : null;

                _writer.WriteLine($"\tpublic {abstractSpecifier}class {node.Name}");
                _writer.WriteLine("\t{");

                WriteChildren(@class);

                _writer.WriteLine("\t}");
            }

            var method = node as MethodDefinition;
            if (method != null)
            {
                _methodWriter.Write(method);
            }
        }

        private void WriteChildren(ModelNodeDefinition node)
        {
            var nodesByType = node.Children.ToLookup(GetNodeType);

            foreach (var nodeType in _nodeTypeOrder)
            {
                INodeWriter writer;
                if (_writers.TryGetValue(nodeType, out writer))
                {
                    foreach (var child in nodesByType[nodeType])
                    {
                        writer.Write(child, _writer);
                    }
                }
                else
                {
                    foreach (var child in nodesByType[nodeType])
                    {
                        WriteNode(child);
                    }
                }
            }
        }

        private string GetNodeType(ModelNodeDefinition node)
        {
            if (node is ClassDefinition)
            {
                return "class";
            }
            if (node is EnumDefinition)
            {
                return "enum";
            }
            var method = node as MethodDefinition;
            if (method != null)
            {
                if (method.IsConstructor)
                {
                    return "constructor";
                }
                return "method";
            }
            if (node is FieldDefinition)
            {
                return "field";
            }
            return "node";
        }
    }
}
