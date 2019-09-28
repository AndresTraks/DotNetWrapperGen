using System.Collections.Generic;

namespace DotNetWrapperGen.Tokenizer
{
    public class TokenizerContext
    {
        public IList<IToken> RootTokens { get; } = new List<IToken>();

        public void Add(IToken token)
        {
            RootTokens.Add(token);
        }
    }
}
