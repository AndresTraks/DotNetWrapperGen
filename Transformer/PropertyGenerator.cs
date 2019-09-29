using DotNetWrapperGen.CodeModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.Transformer
{
    // Generates properties for getters and setters
    public class PropertyGenerator
    {
        public void Transform(NamespaceDefinition globalNamespace)
        {
            TransformNode(globalNamespace);
        }

        public void TransformNode(ModelNodeDefinition node)
        {
            if (node is ClassDefinition || node is NamespaceDefinition)
            {
                TransformNodeMethods(node);
            }

            foreach (ModelNodeDefinition child in node.Children)
            {
                TransformNode(child);
            }
        }

        private void TransformNodeMethods(ModelNodeDefinition node)
        {
            var methodsToRemove = new List<MethodDefinition>();
            List<MethodDefinition> allMethods = node.Children.OfType<MethodDefinition>().ToList();
            foreach (MethodDefinition method in allMethods)
            {
                TransformMethod(method, methodsToRemove);
            }

            foreach (MethodDefinition method in methodsToRemove)
            {
                method.Parent.RemoveChild(method);
            }
        }

        private void TransformMethod(MethodDefinition method, IList<MethodDefinition> methodsToRemove)
        {
            string verblessName = GetGetterVerblessName(method);
            if (verblessName != null)
            {
                MethodDefinition getter = method;
                var property = new PropertyDefinition(getter, verblessName);
                MethodDefinition setter = FindSetter(method);
                if (setter != null)
                {
                    property.Setter = (MethodDefinition)setter.ClonedFrom;
                    methodsToRemove.Add(setter);
                }
                methodsToRemove.Add(getter);
            }
        }

        private MethodDefinition FindSetter(MethodDefinition method)
        {
            return null;
        }

        private static string GetGetterVerblessName(MethodDefinition method)
        {
            if (method.Parameters.Length > 0)
            {
                return null;
            }

            string name = method.Name;
            string verblessName;
            if (name.StartsWith("get", StringComparison.InvariantCultureIgnoreCase))
            {
                verblessName = name.Substring(3);
            }
            else if (name.StartsWith("is", StringComparison.InvariantCultureIgnoreCase))
            {
                verblessName = name.Substring(2);
            }
            else if (name.StartsWith("has", StringComparison.InvariantCultureIgnoreCase))
            {
                verblessName = name.Substring(3);
            }
            else
            {
                return null;
            }

            return ToCamelCase(verblessName);
        }

        private static string ToCamelCase(string name)
        {
            // one_two_three -> oneTwoThree
            while (name.Contains("_"))
            {
                int pos = name.IndexOf('_');
                name = name.Substring(0, pos) + name.Substring(pos + 1, 1).ToUpper() + name.Substring(pos + 2);
            }
            return name;
        }
    }
}