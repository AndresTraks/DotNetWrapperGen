using System;

namespace DotNetWrapperGen.CodeModel
{
    public class FieldDefinition : ModelNodeDefinition
    {
        public FieldDefinition(string name, TypeRefDefinition type)
            : base(name)
        {
            Type = type;
        }

        public TypeRefDefinition Type { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public override void AddChild(ModelNodeDefinition child)
        {
            throw new NotSupportedException("Field cannot have children");
        }

        public override object Clone()
        {
            return new FieldDefinition(Name, new TypeRefDefinition());
        }
    }
}
