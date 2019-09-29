using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System.Linq;

namespace DotNetWrapperGen.Transformer
{
    public class NodeOrderer : ITransformer
    {
        public void Transform(NamespaceDefinition globalNamespace, RootFolderDefinition rootFolder)
        {
            Order(globalNamespace);
        }

        private void Order(ModelNodeDefinition node)
        {
            if (node is EnumDefinition @enum)
            {
                return;
            }

            if (node is ClassDefinition @class && @class.Name == "UnsafeNativeMethods")
            {
                return;
            }

            var newOrder = node.Children.OrderBy(n => n.Name).ToList();
            for (int i = 0; i < node.Children.Count; i++)
            {
                node.Children[i] = newOrder[i];
                Order(node.Children[i]);
            }
        }
    }
}
