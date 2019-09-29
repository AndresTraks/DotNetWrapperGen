using DotNetWrapperGen.CodeModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.Tokenizer.CSharp
{
    public class CSharpClassTokenizer : INodeTokenizer
    {
        private readonly IDictionary<NodeType, INodeTokenizer> _tokenizers = new Dictionary<NodeType, INodeTokenizer>();

        private readonly NodeType[] _nodeTypeOrder = new[] {
            NodeType.Constructor,
            NodeType.Method,
            NodeType.Property,
            NodeType.Enum,
            NodeType.Class
        };

        public CSharpClassTokenizer(CSharpEnumTokenizer enumTokenizer)
        {
            _tokenizers[NodeType.Constructor] = new CSharpMethodTokenizer();
            _tokenizers[NodeType.Method] = new CSharpMethodTokenizer();
            _tokenizers[NodeType.Property] = new CSharpPropertyTokenizer();
            _tokenizers[NodeType.Enum] = enumTokenizer;
            _tokenizers[NodeType.Class] = this;
        }

        public IToken Tokenize(ModelNodeDefinition node)
        {
            return Tokenize((ClassDefinition)node);
        }

        private IToken Tokenize(ClassDefinition @class)
        {
            var abstractSpecifier = @class.IsAbstract ? "abstract " : null;
            var baseClassSpecifier = @class.BaseClass != null ? (" : " + @class.BaseClass.ManagedName) : null;

            var header = new LineToken($"public {abstractSpecifier}class {@class.Name}{baseClassSpecifier}");
            IList<IToken> children = GetClassMembers(@class);
            return new BlockToken(header, children);
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
                    throw new NotImplementedException();
                }
            }
            return members;
        }

        private NodeType GetNodeType(ModelNodeDefinition node)
        {
            if (node is ClassDefinition)
            {
                return NodeType.Class;
            }
            if (node is EnumDefinition)
            {
                return NodeType.Enum;
            }
            if (node is MethodDefinition method)
            {
                if (method.IsConstructor)
                {
                    return NodeType.Constructor;
                }
                return NodeType.Method;
            }
            if (node is PropertyDefinition)
            {
                return NodeType.Property;
            }
            return NodeType.Unknown;
        }

        private enum NodeType
        {
            Constructor,
            Property,
            Method,
            Class,
            Enum,
            Unknown
        }
    }
}
