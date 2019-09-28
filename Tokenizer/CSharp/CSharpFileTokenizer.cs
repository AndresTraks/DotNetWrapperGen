using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Parser;
using System;
using System.Collections.Generic;

namespace DotNetWrapperGen.Tokenizer.CSharp
{
    public class CSharpFileTokenizer
    {
        private TokenizerContext _context;
        private readonly CSharpClassTokenizer _classTokenizer;
        private readonly CSharpEnumTokenizer _enumTokenizer;

        public CSharpFileTokenizer()
        {
            _enumTokenizer = new CSharpEnumTokenizer();
            _classTokenizer = new CSharpClassTokenizer(_enumTokenizer);
        }

        public TokenizerContext Tokenize(HeaderDefinition header)
        {
            _context = new TokenizerContext();

            NamespaceTreeNode namespaceTree = HeaderNamespaceTree.GetTree(header);
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
                IToken childToken = TokenizeNode(childNode);
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

        private IToken TokenizeNode(ModelNodeDefinition node)
        {
            if (node is ClassDefinition @class)
            {
                return _classTokenizer.Tokenize(@class);
            }

            if (node is EnumDefinition @enum)
            {
                return _enumTokenizer.Tokenize(@enum);
            }

            throw new NotImplementedException();
        }
    }
}
