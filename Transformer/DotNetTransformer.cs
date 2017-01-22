using DotNetWrapperGen.CodeModel;
using System.Linq;

namespace DotNetWrapperGen.Transformer
{
    public class DotNetTransformer
    {
        public static NamespaceDefinition Clone(NamespaceDefinition globalNamespace)
        {
            return globalNamespace.Clone() as NamespaceDefinition;
        }

        // In C#, namespaces cannot contain fields or methods, move them into classes.
        public static void MoveSymbolsToClasses(NamespaceDefinition @namespace)
        {
            var methods = @namespace.Children.OfType<MethodDefinition>().ToList();
            foreach (var method in methods)
            {
                @namespace.Children.Remove(method);
                method.Parent = null;
            }

            var childNamespaces = @namespace.Children.Where(c => c is NamespaceDefinition);
            foreach (var childNamespace in @namespace.Children.OfType<NamespaceDefinition>())
            {
                MoveSymbolsToClasses(childNamespace);
            }
        }
    }
}
