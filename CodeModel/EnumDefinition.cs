using System;
using System.Collections.Generic;

namespace DotNetWrapperGen.CodeModel
{
    public class EnumDefinition : ModelNodeDefinition
    {
        public List<string> EnumConstants { get; } = new List<string>();
        public List<string> EnumConstantValues { get; } = new List<string>();
        public List<EnumeratorDefinition> Enumerators { get; } = new List<EnumeratorDefinition>();

        public EnumDefinition(string name)
            : base(name)
        {
        }

        public override void AddChild(ModelNodeDefinition child)
        {
            throw new NotSupportedException("Enum cannot have children");
        }

        public override object Clone()
        {
            return new EnumDefinition(Name);
        }
    }

    public class EnumeratorDefinition
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
