using System;
using System.Collections.Generic;

namespace DotNetWrapperGen.CodeModel
{
    public abstract class ModelNodeDefinition : ICloneable
    {
        public ModelNodeDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public ModelNodeDefinition Parent { get; set; }
        public IList<ModelNodeDefinition> Children { get; } = new List<ModelNodeDefinition>();

        public virtual void AddChild(ModelNodeDefinition child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public abstract object Clone();
    }
}
