using ClangSharp;

namespace DotNetWrapperGen.Parser
{
    public interface IParser
    {
        void Parse(Cursor cursor, CppParserContext context);
    }
}
