using DotNetWrapperGen.CodeModel;
using System;

namespace DotNetWrapperGen.Utility
{
    public static class ClassExtensions
    {
        public static string GetFullName(this ModelNodeDefinition node)
        {
            if (!(node is ClassDefinition) || !(node is NamespaceDefinition))
            {
                throw new NotImplementedException();
            }

            ModelNodeDefinition parent = node.Parent;
            if (parent != null)
            {
                return $"{parent.GetFullName()}::{node.Name}";
            }
            return node.Name;
        }
    }
}
