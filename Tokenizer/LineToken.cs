using System.Collections.Generic;
using System.Text;

namespace DotNetWrapperGen.Tokenizer
{
    public class LineToken : IToken
    {
        public LineToken(string line)
        {
            Elements = new IToken[] { new StringToken(line) };
        }

        public LineToken(IToken element)
        {
            Elements = new IToken[] { element };
        }

        public LineToken(IList<IToken> elements)
        {
            Elements = elements;
        }

        public IList<IToken> Elements { get; private set; }

        public override string ToString()
        {
            if (Elements.Count == 0)
            {
                return string.Empty;
            }

            if (Elements.Count == 1)
            {
                return Elements[0].ToString();
            }
            
            var builder = new StringBuilder();
            foreach (IToken element in Elements)
            {
                builder.Append(element.ToString());
            }
            return builder.ToString();
        }
    }
}
