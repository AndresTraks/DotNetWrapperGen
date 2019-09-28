using DotNetWrapperGen.CodeModel;

namespace DotNetWrapperGen.Tokenizer
{
    public interface INodeTokenizer
    {
        IToken Tokenize(ModelNodeDefinition node);
    }
}
