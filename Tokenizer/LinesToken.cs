using System.Collections.Generic;

namespace DotNetWrapperGen.Tokenizer
{
    public class LinesToken : IToken
    {
        public LinesToken(IList<LineToken> lines)
        {
            Lines = lines;
        }

        public IList<LineToken> Lines { get; private set; }
    }
}
