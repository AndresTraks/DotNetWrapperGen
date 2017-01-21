using System;

namespace DotNetWrapperGen.CodeModel
{
    public class MethodDefinition : ModelNodeDefinition
    {
        public MethodDefinition(string name, ModelNodeDefinition parent, params ParameterDefinition[] parameters)
            : base(name)
        {
            Parameters = parameters;

            if (!(parent is NamespaceDefinition || parent is ClassDefinition))
            {
                throw new ArgumentException("Method parent can only be a namespace or class", nameof(parent));
            }

            Parent = parent;
            parent.Children.Add(this);
        }

        public TypeRefDefinition ReturnType { get; set; }
        public ParameterDefinition[] Parameters { get; }
        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsConstructor { get; set; }
        public FieldDefinition Field { get; set; } // get/set method target
        public PropertyDefinition Property { get; set; } // property that wraps this get/set method

        public string ManagedName
        {
            get
            {
                return Name.Substring(0, 1).ToUpper() + Name.Substring(1);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var m = obj as MethodDefinition;
            if ((System.Object)m == null)
            {
                return false;
            }

            if (!m.Name.Equals(Name) || m.Parameters.Length != Parameters.Length)
            {
                return false;
            }

            if (!m.ReturnType.Equals(ReturnType))
            {
                return false;
            }

            for (int i = 0; i < Parameters.Length; i++)
            {
                // Parameter names can vary, but types must match
                if (!m.Parameters[i].Type.Equals(Parameters[i].Type))
                {
                    return false;
                }
            }

            return m.IsStatic == IsStatic;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
