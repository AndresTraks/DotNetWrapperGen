namespace DotNetWrapperGen.CodeModel
{
    public class ParameterDefinition
    {
        public ParameterDefinition(string name, TypeRefDefinition type, bool isOptional = false)
        {
            Name = name;
            Type = type;
            IsOptional = isOptional;
        }

        public string Name { get; private set; }
        public TypeRefDefinition Type { get; private set; }
        public bool IsOptional { get; set; }

        public override string ToString()
        {
            return Type.ToString() + ' ' + Name;
        }

        public ParameterDefinition Clone()
        {
            return new ParameterDefinition(Name, Type?.Clone() as TypeRefDefinition, IsOptional);
        }
    }
}
