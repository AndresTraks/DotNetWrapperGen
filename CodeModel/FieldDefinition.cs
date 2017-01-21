using System;

namespace DotNetWrapperGen.CodeModel
{
    public class FieldDefinition : ModelNodeDefinition
    {
        public TypeRefDefinition Type { get; private set; }

        public FieldDefinition(string name, TypeRefDefinition type, ModelNodeDefinition parent)
            : base(name)
        {
            Type = type;

            if (!(parent is NamespaceDefinition || parent is ClassDefinition))
            {
                throw new ArgumentException("Field parent can only be a namespace or class", nameof(parent));
            }

            Parent = parent;
            parent.Children.Add(this);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
