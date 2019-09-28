using ClangSharp;
using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System.Collections.Generic;

namespace DotNetWrapperGen.Parser
{
    public class CppParserContext
    {
        public CppParserContext(RootFolderDefinition rootFolder)
        {
            RootFolder = rootFolder;
        }

        public Index Index { get; set; }
        public RootFolderDefinition RootFolder { get; }

        public TranslationUnit TranslationUnit { get; set; }
        public HeaderDefinition Header { get; set; }

        public NamespaceDefinition Namespace { get; set; }
        public ClassDefinition Class { get; set; }
        public MethodDefinition Method { get; internal set; }

        public Cursor.CursorVisitor NodeVisitor { get; internal set; }

        public IDictionary<CursorKind, IParser> DefinitionParsers { get; } = new Dictionary<CursorKind, IParser>();
        public IDictionary<CursorKind, IParser> DeclarationParsers { get; } = new Dictionary<CursorKind, IParser>();

        public ModelNodeDefinition GetContainingClassOrNamespace()
        {
            return Class != null
                ? Class as ModelNodeDefinition
                : Namespace as ModelNodeDefinition;
        }
    }
}
