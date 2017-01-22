using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.CodeModel
{
    public class NamespaceDefinition : ModelNodeDefinition
    {
        public NamespaceDefinition(string name)
            : base(name)
        {
        }

        public IEnumerable<NamespaceDefinition> Namespaces => Children.OfType<NamespaceDefinition>();

        public string FullName
        {
            get
            {
                var parentNamespace = Parent as NamespaceDefinition;
                if (parentNamespace != null)
                {
                    return $"{parentNamespace.FullName}::{Name}";
                }
                return Name;
            }
        }

        public string FullNameCSharp
        {
            get
            {
                var parentNamespace = Parent as NamespaceDefinition;
                if (parentNamespace != null)
                {
                    return $"{parentNamespace.FullNameCSharp}::{Name}";
                }
                return Name;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public override void AddChild(ModelNodeDefinition child)
        {
            base.AddChild(child);
        }

        public override object Clone()
        {
            var node = new NamespaceDefinition(Name);
            foreach (ModelNodeDefinition childClone in Children.Select(c => c.Clone()))
            {
                node.AddChild(childClone);
            }
            return node;
        }
    }
}
