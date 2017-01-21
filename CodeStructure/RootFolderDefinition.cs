namespace DotNetWrapperGen.CodeStructure
{
    public class RootFolderDefinition : SourceItemDefinition
    {
        public RootFolderDefinition(string name)
            : base(name)
        {
        }

        public override bool IsFolder => true;

        public bool Contains(string item)
        {
            foreach (var child in Children)
            {
                if (child.Name.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
