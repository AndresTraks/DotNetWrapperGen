namespace DotNetWrapperGen.Tokenizer
{
    public class ExpressionBodyToken : IToken
    {
        public ExpressionBodyToken(LineToken line)
        {
            Line = line;
        }

        public LineToken Line { get; }
    }
}
