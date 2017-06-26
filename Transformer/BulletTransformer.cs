using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System.Linq;

namespace DotNetWrapperGen.Transformer
{
    public class BulletTransformer : ITransformer
    {
        private const string DefaultNamespace = "BulletSharp";

        public void Transform(NamespaceDefinition globalNamespace, RootFolderDefinition rootFolder)
        {
            RenameHeaders(rootFolder);
            RenameClasses(globalNamespace);
            MoveGlobalNodes(globalNamespace);
        }

        private static void RenameHeaders(SourceItemDefinition item)
        {
            if (item.Name.StartsWith("bt"))
            {
                item.Name = item.Name.Substring(2);
            }

            foreach (var child in item.Children)
            {
                RenameHeaders(child);
            }
        }

        private static void RenameClasses(ModelNodeDefinition node)
        {
            var @class = node as ClassDefinition;
            if (@class != null && @class.Name == "UnsafeNativeMethods")
            {
                return;
            }

            if (node.Name.StartsWith("bt"))
            {
                node.Name = node.Name.Substring(2);
            }

            foreach (var child in node.Children)
            {
                RenameClasses(child);
            }
        }

        private static void MoveGlobalNodes(NamespaceDefinition globalNamespace)
        {
            NamespaceDefinition defaultNamespace = null;

            var globalNodes = globalNamespace.Children.ToList();
            foreach (var node in globalNodes)
            {
                if (node is NamespaceDefinition)
                {
                    continue;
                }

                if (defaultNamespace == null)
                {
                    globalNamespace
                        .Namespaces
                        .FirstOrDefault(ns => string.Equals(ns.Name, DefaultNamespace));
                    if (defaultNamespace == null)
                    {
                        defaultNamespace = new NamespaceDefinition(DefaultNamespace);
                        globalNamespace.AddChild(defaultNamespace);
                    }
                }

                defaultNamespace.AddChild(node);
            }
        }
    }
}
