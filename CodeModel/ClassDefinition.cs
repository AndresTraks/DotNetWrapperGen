using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.CodeModel
{
    public class ClassDefinition : ModelNodeDefinition
    {
        public ClassDefinition(string name)
            : base(name)
        {
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

        public override void AddChild(ModelNodeDefinition child)
        {
            if (child is NamespaceDefinition)
            {
                throw new ArgumentException("Cannot add namespace to class", nameof(child));
            }

            var method = child as MethodDefinition;
            if (method != null)
            {
                method.Header = Header;
            }

            base.AddChild(child);
        }

        public override object Clone()
        {
            var newClass = new ClassDefinition(Name)
            {
                Header = Header,
                Source = this
            };

            foreach (ModelNodeDefinition child in Children)
            {
                newClass.AddChild((ModelNodeDefinition)child.Clone());
            }

            return newClass;
        }
    }
}
