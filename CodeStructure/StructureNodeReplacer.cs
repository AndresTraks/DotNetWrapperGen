using System;

namespace DotNetWrapperGen.CodeStructure
{
    public class StructureNodeReplacer
    {
        public static void Replace(SourceItemDefinition replace, SourceItemDefinition with)
        {
            if (replace.GetType() != with.GetType())
            {
                throw new InvalidOperationException();
            }

            with.Parent = replace.Parent;
            foreach (SourceItemDefinition child in replace.Children)
            {
                with.AddChild(child);
            }

            replace.Parent = null;
            replace.Children.Clear();

            // TODO: replace references in model nodes
        }
    }
}
