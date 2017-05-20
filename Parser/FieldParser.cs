using ClangSharp;
using DotNetWrapperGen.CodeModel;

namespace DotNetWrapperGen.Parser
{
    public class FieldParser : IParser
    {
        public void Parse(Cursor cursor, CppParserContext context)
        {
            string fieldName = cursor.Spelling;
            var field = new FieldDefinition(fieldName, new TypeRefDefinition(cursor.Type));

            ModelNodeDefinition parent = context.GetCurrentParent();
            if (parent is NamespaceDefinition)
            {
                field.Header = context.Header;
            }
            parent.AddChild(field);
        }
    }
}