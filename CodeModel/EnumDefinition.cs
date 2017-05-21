using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.CodeModel
{
    public class EnumDefinition : ModelNodeDefinition
    {
        public EnumDefinition(string name)
            : base(name)
        {
        }

        public IEnumerable<EnumeratorDefinition> Enumerators => Children.Cast<EnumeratorDefinition>();

        public override void AddChild(ModelNodeDefinition child)
        {
            if (!(child is EnumeratorDefinition))
            {
                throw new NotSupportedException("Enum can only contain enumerators");
            }

            base.AddChild(child);
        }

        public override object Clone()
        {
            var newEnum = new EnumDefinition(Name)
            {
                Header = Header
            };

            foreach (ModelNodeDefinition child in Children)
            {
                newEnum.AddChild((ModelNodeDefinition)child.Clone());
            }

            return newEnum;
        }
    }

    public class EnumeratorDefinition : ModelNodeDefinition
    {
        public EnumeratorDefinition(string name, string value)
            : base(name)
        {
            Value = value;
        }

        public string Value { get; set; }

        public override object Clone()
        {
            return new EnumeratorDefinition(Name, Value);
        }

        public override string ToString()
        {
            return Value != null
                ? $"{Name} = {Value}"
                : Name;
        }
    }
}
