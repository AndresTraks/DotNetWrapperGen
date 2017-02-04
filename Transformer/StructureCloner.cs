using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.Transformer
{
    public class StructureCloner
    {
        private IDictionary<SourceItemDefinition, SourceItemDefinition> _oldToNewMapping =
            new Dictionary<SourceItemDefinition, SourceItemDefinition>();

        public NamespaceDefinition RootNamespaceClone { get; private set; }
        public RootFolderDefinition RootFolderClone { get; private set; }

        public void Clone(NamespaceDefinition globalNamespace)
        {
            var clone = globalNamespace.Clone() as NamespaceDefinition;
            CloneNodeWithStructure(clone);
            RootNamespaceClone = clone;
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
                RootFolderClone = RootFolderClone ?? clone as RootFolderDefinition;
            }
            else
            {
                throw new NotSupportedException();
            }

            _oldToNewMapping[item] = clone;

            clone.Parent = CloneSourceItem(item.Parent);
            foreach (SourceItemDefinition child in item.Children.Where(c => !c.IsExcluded))
            {
                var childClone = CloneSourceItem(child);
                clone.Children.Add(childClone);
            }

            return clone;
        }

        private static bool IsTopLevelNodeInNamespace(ModelNodeDefinition node)
        {
            return !(node is NamespaceDefinition) && node.Parent is NamespaceDefinition;
        }
    }
}
