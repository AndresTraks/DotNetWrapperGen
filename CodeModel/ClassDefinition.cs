using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.CodeModel
{
    public class ClassDefinition : ModelNodeDefinition
    {
        public ClassDefinition(string name, ModelNodeDefinition parent)
            : base(name)
        {
            if (!(parent is NamespaceDefinition || parent is ClassDefinition))
            {
                throw new ArgumentException("Class parent can only be a namespace or class", nameof(parent));
            }

            Parent = parent;
            parent.Children.Add(this);
        }

        public TypeRefDefinition BaseClass { get; set; }

        public IEnumerable<ClassDefinition> Classes => Children.OfType<ClassDefinition>();
        public IEnumerable<MethodDefinition> Methods => Children.OfType<MethodDefinition>();
        public IEnumerable<PropertyDefinition> Properties => Children.OfType<PropertyDefinition>();
        public IEnumerable<FieldDefinition> Fields => Children.OfType<FieldDefinition>();

        public bool IsAbstract { get; set; }
        public bool IsStruct { get; set; }
        public bool IsTemplate { get; set; }

        // For function prototypes IsTypeDef == true, but TypedefUnderlyingType == null
        public bool IsTypedef { get; set; }
        public TypeRefDefinition TypedefUnderlyingType { get; set; }

        public string ManagedName { get; set; }

        public string FullName
        {
            get
            {
                var parentClass = Parent as ClassDefinition;
                if (parentClass != null)
                {
                    return $"{parentClass.FullName}::{Name}";
                }
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
                var parentClass = Parent as ClassDefinition;
                if (parentClass != null)
                {
                    return $"{parentClass.FullNameCSharp}::{Name}";
                }
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
    }
}
