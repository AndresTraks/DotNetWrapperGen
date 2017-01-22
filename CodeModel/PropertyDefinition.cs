using System;

namespace DotNetWrapperGen.CodeModel
{
    public class PropertyDefinition
    {
        public MethodDefinition Getter { get; private set; }
        public MethodDefinition Setter { get; set; }
        public ClassDefinition Parent { get; private set; }
        public string VerblessName { get; private set; }

        public TypeRefDefinition Type
        {
            get
            {
                return Getter.ReturnType;
            }
        }

        // Property from getter method
        public PropertyDefinition(MethodDefinition getter)
        {
            Getter = getter;
            Parent = getter.Parent as ClassDefinition;
            throw new NotImplementedException();
            //Parent.Children.Add(this);
            getter.Property = this;

            string name = getter.Name;
            
            // one_two_three -> oneTwoThree
            while (name.Contains("_"))
            {
                int pos = name.IndexOf('_');
                name = name.Substring(0, pos) + name.Substring(pos + 1, 1).ToUpper() + name.Substring(pos + 2);
            }

            if (name.StartsWith("get", StringComparison.InvariantCultureIgnoreCase))
            {
                VerblessName = name.Substring(3);
            }
            else if (name.StartsWith("is", StringComparison.InvariantCultureIgnoreCase))
            {
                VerblessName = name.Substring(2);
            }
            else if (name.StartsWith("has", StringComparison.InvariantCultureIgnoreCase))
            {
                VerblessName = name.Substring(3);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // Property from field
        public PropertyDefinition(FieldDefinition field)
        {
            //Name = field.ManagedName;
            Parent = field.Parent as ClassDefinition;
            throw new NotImplementedException();
            //Parent.Properties.Add(this);

            string name = field.Name;
            if (name.StartsWith("m_"))
            {
                name = name.Substring(2);
            }
            name = name.Substring(0, 1).ToUpper() + name.Substring(1); // capitalize

            // one_two_three -> oneTwoThree
            while (name.Contains("_"))
            {
                int pos = name.IndexOf('_');
                name = name.Substring(0, pos) + name.Substring(pos + 1, 1).ToUpper() + name.Substring(pos + 2);
            }

            VerblessName = name;

            // Generate getter/setter methods
            string getterName, setterName;
            if (name.StartsWith("has"))
            {
                getterName = name;
                setterName = "set" + name.Substring(3);
            }
            else if (name.StartsWith("is"))
            {
                getterName = name;
                setterName = "set" + name.Substring(2);
            }
            else
            {
                getterName = "get" + name;
                setterName = "set" + name;
            }

            Getter = new MethodDefinition(getterName);
            Getter.ReturnType = field.Type;
            Getter.Field = field;
            Getter.Property = this;
            Parent.AddChild(Getter);

            Setter = new MethodDefinition(setterName, new ParameterDefinition("value", field.Type));
            Setter.ReturnType = new TypeRefDefinition();
            Setter.Field = field;
            Setter.Property = this;
            Parent.AddChild(Setter);
        }

        public override string ToString()
        {
            return VerblessName;
        }
    }
}
