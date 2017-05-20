using ClangSharp;
using DotNetWrapperGen.CodeModel;
using System.Linq;

namespace DotNetWrapperGen.Parser
{
    public class NamespaceParser : IParser
    {
        public void Parse(Cursor cursor, CppParserContext context)
        {
            string namespaceName = cursor.Spelling;
            NamespaceDefinition @namespace = CreateOrGetNamespace(namespaceName, context);

            NamespaceDefinition previousNamespace = context.Namespace;
            context.Namespace = @namespace;
            cursor.VisitChildren(context.NodeVisitor);
            context.Namespace = previousNamespace;
        }

        private static NamespaceDefinition CreateOrGetNamespace(string name, CppParserContext context)
        {
            var @namespace = context.Namespace.Namespaces
                                    .FirstOrDefault(n => n.Name == name);
            if (@namespace == null)
            {
                @namespace = new NamespaceDefinition(name)
                {
                    Parent = context.Namespace
                };
                context.Namespace.Children.Add(@namespace);
            }
            return @namespace;
        }
    }
}
