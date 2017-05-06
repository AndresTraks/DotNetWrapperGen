using DotNetWrapperGen.CodeStructure;
using System;
using System.Collections.Generic;

namespace DotNetWrapperGen.CodeModel
{
    public abstract class ModelNodeDefinition : ICloneable
    {
        private HeaderDefinition _header;

        public ModelNodeDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public ModelNodeDefinition Parent { get; set; }
        public IList<ModelNodeDefinition> Children { get; } = new List<ModelNodeDefinition>();

        public virtual HeaderDefinition Header
        {
            get
            {
                if (_header != null) return _header;

                if (Parent != null)
                {
                    try
                    {
                        return Parent.Header;
                    }
                    catch (InvalidOperationException)
                    {
                        return null;
                    }
                }

                return null;
            }

            set
            {
                _header = value;
            }
        }

        public virtual void AddChild(ModelNodeDefinition child)
        {
            if (child.Parent != null)
            {
                child.Parent.RemoveChild(child);
            }

            child.Parent = this;
            Children.Add(child);
        }

        public virtual void RemoveChild(ModelNodeDefinition child)
        {
            Children.Remove(child);
            child.Parent = null;
        }

        public abstract object Clone();
    }
}
