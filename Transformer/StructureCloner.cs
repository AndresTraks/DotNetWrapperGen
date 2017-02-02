using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System;
using System.Collections.Generic;

namespace DotNetWrapperGen.Transformer
{
    public class StructureCloner
    {
        private IDictionary<SourceItemDefinition, SourceItemDefinition> _oldToNewMapping =
            new Dictionary<SourceItemDefinition, SourceItemDefinition>();

        private StructureCloner()
        {
        }

        public static NamespaceDefinition Clone(NamespaceDefinition globalNamespace)
        {
            var clone = globalNamespace.Clone() as NamespaceDefinition;
            new StructureCloner().CloneNodeWithStructure(clone);
            return clone;
        }

        private void CloneNodeWithStructure(ModelNodeDefinition node)
        {
            if (IsTopLevelNodeInNamespace(node))
            {
                node.Header = CloneSourceItem(node.Header) as HeaderDefinition;
            }

            foreach (var child in node.Children)
            {
                CloneNodeWithStructure(child);
            }
        }

        private SourceItemDefinition CloneSourceItem(SourceItemDefinition item)
        {
            if (item == null)
            {
                return null;
            }

            SourceItemDefinition clone;
            if (_oldToNewMapping.TryGetValue(item, out clone))
            {
                return clone;
            }

            if (item is HeaderDefinition)
            {
                clone = new HeaderDefinition(item.Name);
            }
            else if (item is FolderDefinition)
            {
                clone = new FolderDefinition(item.Name);
            }
            else if (item is RootFolderDefinition)
            {
                clone = new RootFolderDefinition(item.Name);
            }
            else
            {
                throw new NotSupportedException();
            }
            clone.Parent = CloneSourceItem(item.Parent);

            _oldToNewMapping[item] = clone;
            return clone;
        }

        private static bool IsTopLevelNodeInNamespace(ModelNodeDefinition node)
        {
            return !(node is NamespaceDefinition) && node.Parent is NamespaceDefinition;
        }
    }
}
