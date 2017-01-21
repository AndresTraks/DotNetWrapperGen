namespace DotNetWrapperGen.CodeStructure
{
    public class FolderDefinition : SourceItemDefinition
    {
        public FolderDefinition(string name)
            : base(name)
        {
        }

        public override bool IsFolder => true;

        public override string ToString()
        {
            return Name;
        }
    }
}
