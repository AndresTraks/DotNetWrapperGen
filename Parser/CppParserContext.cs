using ClangSharp;
using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;

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
    }
}
