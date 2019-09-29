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
                IList<IToken> bodyTokens = TokenizeBody(method);
                methodToken = new BlockToken(header, bodyTokens);
            }
            return methodToken;
        }

        private LineToken TokenizeHeader(MethodDefinition method)
        {
            var headerTokens = new List<IToken>
            {
                new WordToken("public")
            };
            if (method.IsStatic)
            {
                headerTokens.Add(new WordToken("static"));
            }
            if (method.IsExtern)
            {
                headerTokens.Add(new WordToken("extern"));
            }
            if (method.IsExtern || !method.IsConstructor)
            {
                string returnType = method.ReturnType.ManagedTypeRefName;
                headerTokens.Add(new WordToken(returnType));
            }

            string managedName = method.Name.Substring(0, 1).ToUpper() + method.Name.Substring(1);
            string name = method.IsExtern ? method.Name : managedName;
            headerTokens.Add(new WordToken(name + "("));
            
            headerTokens.Add(TokenizeParameterDefinitions(method));

            if (method.IsExtern)
            {
                headerTokens.Add(new StringToken(");"));
            }
            else
            {
                headerTokens.Add(new StringToken(")"));
            }

            return new LineToken(headerTokens);
        }

        private IToken TokenizeParameterDefinitions(MethodDefinition method)
        {
            int paramCount = method.Parameters.Length;
            if (paramCount == 0)
            {
                return NullToken.Instance;
            }

            var parameters = new IToken[paramCount];
            for (int i = 0; i < paramCount; i++)
            {
                IToken parameter = TokenizeParameterDefinition(method.Parameters[i]);
                parameters[i] = parameter;
            }
            return new ListToken(parameters);
        }

        private WordToken TokenizeParameterDefinition(ParameterDefinition parameter)
        {
            return new WordToken($"{parameter.Type.ManagedTypeRefName} {parameter.Name}");
        }

        private List<IToken> TokenizeBody(MethodDefinition method)
        {
            var lines = new List<IToken>();
            IToken parametersList = TokenizeParameters(method);
            if (method.IsConstructor)
            {
                var nativePtrParts = new List<IToken>
                {
                    new WordToken("IntPtr"),
                    new WordToken("native"),
                    new WordToken("="),
                    new WordToken(method.ClonedFrom.Parent.Name + "_new("),
                    parametersList,
                    new StringToken(");")
                };
                var nativePtr = new LineToken(nativePtrParts);
                lines.Add(nativePtr);
                lines.Add(new LineToken("InitializeUserOwned(native);"));
            }
            else
            {
                var lineParts = new List<IToken>
                {
                    new WordToken(method.ClonedFrom.Parent.Name + "_" + method.ClonedFrom.Name + "("),
                    parametersList,
                    new StringToken(");")
                };
                lines.Add(new LineToken(lineParts));
            }
            return lines;
        }

        private static IToken TokenizeParameters(MethodDefinition method)
        {
            IToken[] list;
            int listIndex;
            if (method.IsStatic || method.IsConstructor)
            {
                if (method.Parameters.Length == 0)
                {
                    return NullToken.Instance;
                }
                list = new IToken[method.Parameters.Length];
                listIndex = 0;
            }
            else
            {
                list = new IToken[method.Parameters.Length + 1];
                list[0] = new WordToken("Native");
                listIndex = 1;
            }
            for (int i = 0; i < method.Parameters.Length; i++)
            {
                ParameterDefinition parameter = method.Parameters[i];
                list[listIndex++] = new WordToken(parameter.Name);
            }
            return new ListToken(list);
        }
    }
}
