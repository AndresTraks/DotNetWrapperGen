using DotNetWrapperGen.CodeModel;
using System.Collections.Generic;

namespace DotNetWrapperGen.Tokenizer.CSharp
{
    public class CSharpMethodTokenizer : INodeTokenizer
    {
        public IToken Tokenize(ModelNodeDefinition node)
        {
            return Tokenize((MethodDefinition)node);
        }

        private IToken Tokenize(MethodDefinition method)
        {
            if (method.IsExcluded)
            {
                return NullToken.Instance;
            }

            LineToken header = TokenizeHeader(method);

            IToken methodToken;
            if (method.IsExtern)
            {
                methodToken = new LinesToken(new LineToken[] {
                    new LineToken("[DllImport(Native.Dll, CallingConvention = Native.Conv)]"),
                    header });
            }
            else
            {
                IToken[] bodyTokens = new IToken[0];
                methodToken = new BlockToken(header, bodyTokens);
            }
            return methodToken;
        }

        private LineToken TokenizeHeader(MethodDefinition method)
        {
            var headerTokens = new List<IToken>();

            headerTokens.Add(new StringToken("public "));
            if (method.IsStatic)
            {
                headerTokens.Add(new StringToken("static "));
            }
            if (method.IsExtern)
            {
                headerTokens.Add(new StringToken("extern "));
            }
            if (method.IsExtern || !method.IsConstructor)
            {
                string returnType = method.ReturnType.ManagedTypeRefName;
                headerTokens.Add(new StringToken(returnType + " "));
            }

            string managedName = method.Name.Substring(0, 1).ToUpper() + method.Name.Substring(1);
            string name = method.IsExtern ? method.Name : managedName;
            headerTokens.Add(new StringToken(name + "("));

            var lastParameterIndex = method.Parameters.Length - 1;
            for (int i = 0; i < method.Parameters.Length; i++)
            {
                WriteParameter(method.Parameters[i], headerTokens);
                if (i != lastParameterIndex)
                {
                    headerTokens.Add(new StringToken(", "));
                }
            }
            headerTokens.Add(new StringToken(")"));

            if (method.IsExtern)
            {
                headerTokens.Add(new StringToken(";"));
            }

            return new LineToken(headerTokens);
        }

        private void WriteParameter(ParameterDefinition parameter, List<IToken> headerTokens)
        {
            headerTokens.Add(new StringToken($"{parameter.Type.ManagedTypeRefName} {parameter.Name}"));
        }
    }
}
