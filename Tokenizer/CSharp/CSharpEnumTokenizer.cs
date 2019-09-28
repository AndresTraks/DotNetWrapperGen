using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.Utility;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.Tokenizer.CSharp
{
    public class CSharpEnumTokenizer : INodeTokenizer
    {
        public IToken Tokenize(ModelNodeDefinition node)
        {
            var @enum = (EnumDefinition)node;

            var definition = new LineToken($"public enum {@enum.Name}");
            IToken headerToken;
            if (@enum.IsFlags())
            {
                headerToken = new LinesToken(new LineToken[] {
                    new LineToken("[Flags]"),
                    definition}
                );
            }
            else
            {
                headerToken = definition;
            }

            var enumerators = new List<IToken>();
            var lastEnumerator = @enum.Enumerators.Last();
            foreach (EnumeratorDefinition enumerator in @enum.Enumerators)
            {
                string comma = enumerator == lastEnumerator ? "" : ",";
                if (enumerator.Value != null)
                {
                    enumerators.Add(new LineToken($"{enumerator.Name} = {enumerator.Value}{comma}"));
                }
                else
                {
                    enumerators.Add(new LineToken(enumerator.Name + comma));
                }
            }

            BlockToken enumToken = new BlockToken(headerToken, enumerators);
            return enumToken;
        }
    }
}
