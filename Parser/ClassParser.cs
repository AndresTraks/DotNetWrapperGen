using ClangSharp;
using DotNetWrapperGen.CodeModel;
using System.Linq;

namespace DotNetWrapperGen.Parser
{
    public class ClassParser : IParser
    {
        public void Parse(Cursor cursor, CppParserContext context)
        {
            string className = cursor.Spelling;
            ModelNodeDefinition parent = context.GetContainingClassOrNamespace();

            if (HasNameConflict(className, parent))
            {
                return;
            }

            context.Class = new ClassDefinition(className);
            parent.AddChild(context.Class);
            if (parent is NamespaceDefinition)
            {
                context.Header.AddNode(context.Class);
            }

            cursor.VisitChildren(context.NodeVisitor);

            context.Class = parent as ClassDefinition;
        }

        private static bool HasNameConflict(string className, ModelNodeDefinition parent)
        {
            return parent.Children.Any(c => c.Name == className);
        }
    }
}
