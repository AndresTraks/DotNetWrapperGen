using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.Parser
{
    public class HeaderNamespaceTree
    {
        public static NamespaceTreeNode GetTree(HeaderDefinition header)
        {
            if (header.Nodes.Count == 0)
            {
                throw new InvalidOperationException("Header has no associated namespace");
            }

            var firstNode = header.Nodes.First();
            var global = GetNamespacePath((NamespaceDefinition)firstNode.Parent).First();

            foreach (ModelNodeDefinition node in header.Nodes)
            {
                var @namespace = (NamespaceDefinition)node.Parent;
                var path = GetNamespacePath(@namespace).ToList();
                global.Merge(node, path);
            }

            return global;
        }

        private static IEnumerable<NamespaceTreeNode> GetNamespacePath(NamespaceDefinition node)
        {
            var parent = node.Parent as NamespaceDefinition;
            if (parent != null)
            {
                foreach (NamespaceTreeNode parentNode in GetNamespacePath(parent))
                {
                    yield return parentNode;
                }
            }
            yield return new NamespaceTreeNode
            {
                Namespace = node
            };
        }
    }

    public class NamespaceTreeNode
    {
        public NamespaceDefinition Namespace { get; set; }
        public IList<ModelNodeDefinition> Nodes { get; } = new List<ModelNodeDefinition>();
        public IList<NamespaceTreeNode> Children { get; } = new List<NamespaceTreeNode>();

        public void Merge(ModelNodeDefinition node, IList<NamespaceTreeNode> path)
        {
            if (path.Count == 1)
            {
                Nodes.Add(node);
            }
            else
            {
                NamespaceTreeNode nextSource = path[1];
                NamespaceTreeNode nextTarget = Children
                    .FirstOrDefault(c => c.Namespace == nextSource.Namespace);
                if (nextTarget == null)
                {
                    nextTarget = new NamespaceTreeNode
                    {
                        Namespace = nextSource.Namespace
                    };
                    Children.Add(nextTarget);
                }
                nextTarget.Merge(node, path.Skip(1).ToList());
            }
        }

        public override string ToString()
        {
            return Namespace.ToString();
        }
    }
}
