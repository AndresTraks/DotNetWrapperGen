using ClangSharp;
using DotNetWrapperGen.CodeModel;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.Parser
{
    public class MethodParser : IParser
    {
        public void Parse(Cursor cursor, CppParserContext context)
        {
            string methodName = cursor.Spelling;

            var parameters = new ParameterDefinition[cursor.NumArguments];
            for (uint i = 0; i < cursor.NumArguments; i++)
            {
                Cursor parameterCursor = cursor.GetArgument(i);
                parameters[i] = ParseParameter(parameterCursor, context);
            }

            context.Method = new MethodDefinition(methodName, parameters)
            {
                IsConstructor = cursor.Kind == CursorKind.Constructor,
                IsStatic = cursor.IsStaticCxxMethod,
                IsAbstract = IsCursorAbstract(cursor, context),
                ReturnType = new TypeRefDefinition(cursor.ResultType)
            };

            ModelNodeDefinition parent = context.GetContainingClassOrNamespace();
            if (parent is NamespaceDefinition)
            {
                if (cursor.SemanticParent.Kind == CursorKind.ClassDecl ||
                    cursor.SemanticParent.Kind == CursorKind.ClassTemplate ||
                    cursor.SemanticParent.Kind == CursorKind.StructDecl)
                {
                    // FIXME: Clang reports a method definition as a method declaration
                    return;
                }
                context.Method.Header = context.Header;
            }
            parent.AddChild(context.Method);

            context.Method = null;
        }

        private static ParameterDefinition ParseParameter(Cursor cursor, CppParserContext context)
        {
            string parameterName = cursor.Spelling;

            IEnumerable<Token> tokens = context.TranslationUnit.Tokenize(cursor.Extent);
            bool isOptional = tokens.Any(t => t.Spelling == "=");

            return new ParameterDefinition(parameterName, new TypeRefDefinition(cursor.Type), isOptional);
        }

        private static bool IsCursorAbstract(Cursor cursor, CppParserContext context)
        {
            IList<Token> tokens = context.TranslationUnit.Tokenize(cursor.Extent).ToList();
            int count = tokens.Count;
            if (count > 3)
            {
                if (tokens[count - 3].Spelling.Equals("=") &&
                    tokens[count - 2].Spelling.Equals("0") &&
                    tokens[count - 1].Spelling.Equals(";"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
