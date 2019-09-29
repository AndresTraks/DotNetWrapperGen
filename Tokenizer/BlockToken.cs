using System;
using System.Collections.Generic;

namespace DotNetWrapperGen.Tokenizer
{
    public class BlockToken : IToken
    {
        public BlockToken(IToken header, IList<IToken> children)
        {
            if (!(header is LineToken) && !(header is LinesToken))
            {
                throw new ArgumentException(nameof(header));
            }

            Header = header;
            Children = children;
        }

        public IToken Header { get; }
        public IList<IToken> Children { get; }

        public override string ToString()
        {
            return Header.ToString();
        }
    }
}
