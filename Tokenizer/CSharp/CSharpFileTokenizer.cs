using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.Tokenizer.CSharp
{
    public class CSharpFileTokenizer
    {
        private readonly HeaderDefinition _header;
        private TokenizerContext _context;

        private IDictionary<string, INodeTokenizer> _tokenizers = new Dictionary<string, INodeTokenizer>();

        private string[] _nodeTypeOrder = new[] {
            "constructor",
            "method",
            "class",
            "enum"
        };

        public CSharpFileTokenizer(HeaderDefinition header)
        {
            _header = header;

            _tokenizers["enum"] = new CSharpEnumTokenizer();
            _tokenizers["constructor"] = new CSharpMethodTokenizer();
            _tokenizers["method"] = new CSharpMethodTokenizer();
        }

        public TokenizerContext Tokenize()
        {
            _context = new TokenizerContext();

            NamespaceTreeNode namespaceTree = HeaderNamespaceTree.GetTree(_header);
            IToken namespaceToken = TokenizeNamespace(namespaceTree);
            _context.Add(namespaceToken);

            return _context;
        }

        private BlockToken TokenizeNamespace(NamespaceTreeNode node)
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

            LineToken header = new LineToken($"namespace {name}");

            var children = new List<IToken>();
            foreach (ModelNodeDefinition childNode in node.Nodes)
            {
                IToken childToken = GetNode(childNode);
                children.Add(childToken);
            }

            foreach (var childNamespace in node.Children)
            {
                IToken childNamespaceToken = TokenizeNamespace(childNamespace);
                children.Add(childNamespaceToken);
            }

            BlockToken @namespace = new BlockToken(header, children);
            return @namespace;
        }

        private IToken GetNode(ModelNodeDefinition node)
        {
            if (node is ClassDefinition @class)
            {
                var abstractSpecifier = @class.IsAbstract ? "abstract " : null;
                var baseClassSpecifier = @class.BaseClass != null ? @class.BaseClass.ManagedName : null;

                var header = new LineToken($"public {abstractSpecifier}class {node.Name}");
                IList<IToken> children = GetClassMembers(@class);
                return new BlockToken(header, children);
            }
            else
            {
                string nodeType = GetNodeType(node);
                if (_tokenizers.TryGetValue(nodeType, out INodeTokenizer tokenizer))
                {
                    IToken token = tokenizer.Tokenize(node);
                    return token;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private IList<IToken> GetClassMembers(ModelNodeDefinition node)
        {
            var members = new List<IToken>();
            var nodesByType = node.Children.ToLookup(GetNodeType);

            foreach (var nodeType in _nodeTypeOrder)
            {
                if (_tokenizers.TryGetValue(nodeType, out INodeTokenizer tokenizer))
                {
                    foreach (var child in nodesByType[nodeType])
                    {
                        IToken childToken = tokenizer.Tokenize(child);
                        members.Add(childToken);
                    }
                }
                else
                {
                    foreach (var child in nodesByType[nodeType])
                    {
                        IToken childToken = GetNode(child);
                        members.Add(childToken);
                    }
                }
            }
            return members;
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
            if (node is MethodDefinition method)
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
