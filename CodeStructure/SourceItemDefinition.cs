using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DotNetWrapperGen.CodeStructure
{
    public abstract class SourceItemDefinition
    {
        private SourceItemDefinition _parent;

        public SourceItemDefinition(string name)
        {
            Name = name;
        }

        public abstract bool IsFolder { get; }
        public bool IsExcluded { get; set; }
        public string Name { get; }
        public IList<SourceItemDefinition> Children { get; } = new List<SourceItemDefinition>();

        public SourceItemDefinition Parent
        {
            get { return _parent; }
            set
            {
                Debug.Assert(value is FolderDefinition || value is RootFolderDefinition || value == null);
                _parent = value;
            }
        }

        public string FullPath
        {
            get
            {
                if (Parent != null)
                {
                    return Parent.FullPath + '/' + Name;
                }
                return Name;
            }
        }

        public virtual void AddChild(SourceItemDefinition child)
        {
            child.Parent = this;
            Children.Add(child);
        }
    }
}
