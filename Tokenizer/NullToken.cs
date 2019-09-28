namespace DotNetWrapperGen.Tokenizer
{
    public class NullToken : IToken
    {
        public static NullToken Instance { get; } = new NullToken();
    }
}
