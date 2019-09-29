using DotNetWrapperGen.CodeStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.CodeModel
{
    public class NamespaceDefinition : ModelNodeDefinition
    {
        public NamespaceDefinition()
            : base(string.Empty)
        {
        }

        public NamespaceDefinition(string name)
            : base(name)
        {
        }

        public IEnumerable<NamespaceDefinition> Namespaces => Children.OfType<NamespaceDefinition>();
        public bool IsGlobal => string.Equals(Name, string.Empty);

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

        // Namespaces have no specific header
        public override HeaderDefinition Header
        {
            get { throw new InvalidOperationException(); }
            set { throw new InvalidOperationException(); }
        }

        public override string ToString()
        {
            return IsGlobal
                ? "<global>"
                : Name;
        }

        public override object Clone()
        {
            var node = new NamespaceDefinition(Name)
            {
                ClonedFrom = this
            };
            foreach (ModelNodeDefinition childClone in Children.Select(c => c.Clone()))
            {
                node.AddChild(childClone);
            }
            return node;
        }
    }
}
