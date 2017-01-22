using System;
using System.Linq;

namespace DotNetWrapperGen.CodeModel
{
    public class MethodDefinition : ModelNodeDefinition
    {
        public MethodDefinition(string name, params ParameterDefinition[] parameters)
            : base(name)
        {
            Parameters = parameters;
            ReturnType = new TypeRefDefinition();
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
            var m = obj as MethodDefinition;
            if (m == null)
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
                if (m.Parameters[i].Type == null && Parameters[i].Type == null)
                {
                    continue;
                }

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

        public override void AddChild(ModelNodeDefinition child)
        {
            throw new NotSupportedException("Method cannot have children");
        }

        public override object Clone()
        {
            return new MethodDefinition(Name, Parameters.Select(p => p.Clone()).ToArray());
        }
    }
}
