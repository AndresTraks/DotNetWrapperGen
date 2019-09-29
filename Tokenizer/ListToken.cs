using System.Collections.Generic;
using System.Text;

namespace DotNetWrapperGen.Tokenizer
{
    public class ListToken : IToken
    {
        public ListToken(IList<IToken> elements)
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
            var lastParameterIndex = Elements.Count - 1;
            for (int i = 0; i < Elements.Count; i++)
            {
                IToken element = Elements[i];
                builder.Append(element.ToString());
                if (i != lastParameterIndex)
                {
                    builder.Append(", ");
                }
            }
            return builder.ToString();
        }
    }
}
