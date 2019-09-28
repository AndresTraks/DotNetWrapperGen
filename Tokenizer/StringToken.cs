namespace DotNetWrapperGen.Tokenizer
{
    public class StringToken : IToken
    {
        public string Value { get; }

        public StringToken(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
