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
                TransformNodeMethodsAndFields(node);
            }

            foreach (ModelNodeDefinition child in node.Children)
            {
                TransformNode(child);
            }
        }

        private void TransformNodeMethodsAndFields(ModelNodeDefinition node)
        {
            var nodesToRemove = new List<ModelNodeDefinition>();

            List<MethodDefinition> allMethods = node.Children.OfType<MethodDefinition>().ToList();
            foreach (MethodDefinition method in allMethods)
            {
                TransformGetterMethod(method, nodesToRemove);
            }

            foreach (MethodDefinition method in allMethods)
            {
                TransformSetterMethod(method, nodesToRemove);
            }

            // If the node has both a field and a getter method with the same
            // wrapper property, prefer to call the getter.

            List<FieldDefinition> allFields = node.Children.OfType<FieldDefinition>().ToList();
            foreach (FieldDefinition method in allFields)
            {
                TransformField(method, nodesToRemove);
            }

            foreach (ModelNodeDefinition child in nodesToRemove)
            {
                child.Parent.RemoveChild(child);
            }
        }

        private void TransformGetterMethod(MethodDefinition method, IList<ModelNodeDefinition> nodesToRemove)
        {
            string propertyName = GetPropertyName(method);
            if (propertyName != null)
            {
                new PropertyDefinition(method, propertyName);
                nodesToRemove.Add(method);
            }
        }

        private void TransformSetterMethod(MethodDefinition method, List<ModelNodeDefinition> nodesToRemove)
        {
            string verblessName = GetSetterVerblessName(method);
            if (verblessName != null)
            {
                PropertyDefinition property = method.Parent.Children
                    .OfType<PropertyDefinition>()
                    .FirstOrDefault(p => p.Name == verblessName);
                if (property == null)
                {
                    return;
                }
                property.Setter = (MethodDefinition)method.ClonedFrom;
                nodesToRemove.Add(method);
            }
        }

        private void TransformField(FieldDefinition field, List<ModelNodeDefinition> nodesToRemove)
        {
            //throw new NotImplementedException();
        }

        private static string GetPropertyName(MethodDefinition getter)
        {
            if (getter.Parameters.Length != 0)
            {
                return null;
            }

            string name = getter.Name;
            string propertyName;
            if (name.StartsWith("get", StringComparison.InvariantCultureIgnoreCase))
            {
                propertyName = name.Substring(3);
            }
            else if (name.StartsWith("is", StringComparison.InvariantCultureIgnoreCase))
            {
                propertyName = "Is" + name.Substring(2);
            }
            else if (name.StartsWith("has", StringComparison.InvariantCultureIgnoreCase))
            {
                propertyName = "Has" + name.Substring(3);
            }
            else
            {
                return null;
            }

            return ToCamelCase(propertyName);
        }

        private static string GetSetterVerblessName(MethodDefinition method)
        {
            if (method.Parameters.Length != 1)
            {
                return null;
            }

            string name = method.Name;
            if (name.StartsWith("set", StringComparison.InvariantCultureIgnoreCase))
            {
                string verblessName = name.Substring(3);
                return ToCamelCase(verblessName);
            }

            return null;
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