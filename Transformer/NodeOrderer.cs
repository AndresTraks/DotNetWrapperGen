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
            var @enum = node as EnumDefinition;
            if (@enum != null)
            {
                return;
            }

            var @class = node as ClassDefinition;
            if (@class != null && @class.Name == "UnsafeNativeMethods")
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
