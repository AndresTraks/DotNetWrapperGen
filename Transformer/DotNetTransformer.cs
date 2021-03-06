﻿using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System;
using System.IO;
using System.Linq;

namespace DotNetWrapperGen.Transformer
{
    public class DotNetTransformer : ITransformer
    {
        public void Transform(NamespaceDefinition globalNamespace, RootFolderDefinition rootFolder)
        {
            new PropertyGenerator().Transform(globalNamespace);
            new NativeMethodImporter().Transform(globalNamespace, rootFolder);
            RenameCodeFiles(rootFolder);
            MoveGlobalSymbolsToClasses(globalNamespace);
            new OperatorTransformer().Transform(globalNamespace, rootFolder);
            new EnumTransformer().Transform(globalNamespace, rootFolder);
            new NodeOrderer().Transform(globalNamespace, rootFolder);
        }

        // In C#, namespaces cannot contain fields or methods, move them into classes.
        public static void MoveGlobalSymbolsToClasses(NamespaceDefinition @namespace)
        {
            var methods = @namespace.Children.OfType<MethodDefinition>().ToList();
            foreach (var method in methods)
            {
                @namespace.Children.Remove(method);
                method.Parent = null;
                throw new NotImplementedException();
            }

            var childNamespaces = @namespace.Children.Where(c => c is NamespaceDefinition);
            foreach (var childNamespace in @namespace.Children.OfType<NamespaceDefinition>())
            {
                MoveGlobalSymbolsToClasses(childNamespace);
            }
        }

        public static void RenameCodeFiles(SourceItemDefinition item)
        {
            if (item is HeaderDefinition header)
            {
                header.Name = Path.GetFileNameWithoutExtension(header.FullPath) + ".cs";
            }
            else
            {
                foreach (SourceItemDefinition child in item.Children)
                {
                    RenameCodeFiles(child);
                }
            }
        }
    }
}
