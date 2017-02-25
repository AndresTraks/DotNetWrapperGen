using DotNetWrapperGen.CodeStructure;

namespace DotNetWrapperGen.Transformer
{
    public class BulletTransformer
    {
        public static void RenameHeaders(SourceItemDefinition item)
        {
            if (item.Name.StartsWith("bt"))
            {
                item.Name = item.Name.Substring(2);
            }

            foreach (var child in item.Children)
            {
                RenameHeaders(child);
            }
        }
    }
}
