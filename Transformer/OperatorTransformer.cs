using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;

namespace DotNetWrapperGen.Transformer
{
    public sealed class OperatorTransformer : ITransformer
    {
        public void Transform(NamespaceDefinition globalNamespace, RootFolderDefinition rootFolder)
        {
            Transform(globalNamespace);
        }

        private static void Transform(ModelNodeDefinition node)
        {
            if (node is MethodDefinition method)
            {
                if (IsOperator(method))
                {
                    method.IsExcluded = true;
                }
                return;
            }

            foreach (ModelNodeDefinition child in node.Children)
            {
                Transform(child);
            }
        }

        private static bool IsOperator(MethodDefinition method)
        {
            if (method.Name.StartsWith("operator"))
            {
                string operatorName = method.Name.Substring("operator".Length);
                switch (operatorName)
                {
                    case "=":
                    case "[]":
                    case "()":
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                    case "+=":
                    case "-=":
                    case "*=":
                    case "/=":
                    case "==":
                    case "!=":
                    case "^":
                    case "<":
                    case ">":
                    case " new":
                    case " delete":
                    case " new[]":
                    case " delete[]":
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }
    }
}
