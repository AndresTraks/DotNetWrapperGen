using DotNetWrapperGen.CodeModel;

namespace DotNetWrapperGen.Tokenizer.CSharp
{
    public class CSharpPropertyTokenizer : INodeTokenizer
    {
        public IToken Tokenize(ModelNodeDefinition node)
        {
            return Tokenize((PropertyDefinition)node);
        }

        private IToken Tokenize(PropertyDefinition property)
        {
            if (property.Setter == null)
            {
                var expressionBody = new ExpressionBodyToken(new LineToken(new IToken[] {
                    new WordToken($"public {property.Getter.ReturnType} {property.Name} =>"),
                    new WordToken($"{property.Getter.Parent.Name}_{property.Getter.Name}(Native);")}));
                return expressionBody;
            }

            var header = new LineToken($"public {property.Getter.ReturnType} {property.Name}");
            var propertyToken = new BlockToken(header, new IToken[]{
                new LineToken($"get => {property.Getter.Parent.Name}_{property.Getter.Name}(Native);"),
                new LineToken($"set => {property.Setter.Parent.Name}_{property.Setter.Name}(Native, value);")
            });
            return propertyToken;
        }
    }
}