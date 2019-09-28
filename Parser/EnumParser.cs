using ClangSharp;
using DotNetWrapperGen.CodeModel;
using System.Linq;

namespace DotNetWrapperGen.Parser
{
    public class EnumParser : IParser
    {
        public void Parse(Cursor cursor, CppParserContext context)
        {
            var parent = context.GetContainingClassOrNamespace();
            var @enum = new EnumDefinition(cursor.Spelling);

            foreach (var constantDecl in cursor.Children
                    .Where(c => c.Kind == CursorKind.EnumConstantDecl))
            {
                string name = constantDecl.Spelling;
                string value = GetEnumeratorValue(constantDecl, context);
                @enum.AddChild(new EnumeratorDefinition(name, value));
            }

            parent.AddChild(@enum);
            if (parent is NamespaceDefinition)
            {
                context.Header.AddNode(@enum);
            }
        }

        private static string GetEnumeratorValue(Cursor constantDecl, CppParserContext context)
        {
            var value = constantDecl.Children.FirstOrDefault();
            if (value != null)
            {
                var valueTokens = context.TranslationUnit.Tokenize(value.Extent)
                    .Where(IsValueToken);
                string spelling = string.Join("", valueTokens.Select(t => t.Spelling));
                return string.IsNullOrEmpty(spelling) ? null : spelling;
            }
            return null;
        }

        private static bool IsValueToken(Token token)
        {
            return token.Kind != TokenKind.Comment &&
                !token.Spelling.Equals(",") &&
                !token.Spelling.Equals("}");
        }
    }
}
