using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.Transformer
{
    public class NativeMethodImporter : ITransformer
    {
        public void Transform(NamespaceDefinition globalNamespace, RootFolderDefinition rootFolder)
        {
            var methodsHeader = new HeaderDefinition("UnsafeNativeMethods.cs");
            var methodsClass = new ClassDefinition("UnsafeNativeMethods");

            CreateExternMethods(globalNamespace, methodsClass);

            methodsHeader.AddNode(methodsClass);
            globalNamespace.AddChild(methodsClass);
            rootFolder.AddChild(methodsHeader);
        }

        private void CreateExternMethods(NamespaceDefinition globalNamespace, ClassDefinition methodsClass)
        {
            foreach (var @class in EnumerateClasses(globalNamespace).OrderBy(c => c.Name))
            {
                foreach (var method in EnumerateMethods(@class))
                {
                    string name = GetExternMethodName(method);
                    ParameterDefinition[] parameters = GetExternMethodParameters(method);
                    var nativeMethod = new MethodDefinition(name, parameters)
                    {
                        IsExtern = true,
                        IsStatic = true
                    };
                    if (method.IsConstructor)
                    {
                        nativeMethod.ReturnType = TypeRefDefinition.IntPtr;
                    }
                    methodsClass.AddChild(nativeMethod);
                }
            }
        }

        private IEnumerable<ClassDefinition> EnumerateClasses(ModelNodeDefinition node)
        {
            foreach (var child in node.Children)
            {
                foreach (var childClass in EnumerateClasses(child))
                {
                    yield return childClass;
                }
            }

            if (node is ClassDefinition @class)
            {
                yield return @class;
            }
        }

        private IEnumerable<MethodDefinition> EnumerateMethods(ClassDefinition @class)
        {
            return @class.Methods
                .OrderBy(m => !m.IsConstructor)
                .ThenBy(m => m.Name);
        }

        private static string GetExternMethodName(MethodDefinition method)
        {
            var sourceMethod = (method.ClonedFrom as MethodDefinition) ?? method;
            string methodName;
            if (sourceMethod.IsConstructor)
            {
                methodName = "new";
            }
            else
            {
                methodName = sourceMethod.Name;
            }
            return $"{sourceMethod.Parent.Name}_{methodName}";
        }

        private static ParameterDefinition[] GetExternMethodParameters(MethodDefinition method)
        {
            if (method.IsConstructor)
            {
                return method.Parameters;
            }
            var parameters = new ParameterDefinition[method.Parameters.Length + 1];
            method.Parameters.CopyTo(parameters, 1);
            parameters[0] = new ParameterDefinition("obj", TypeRefDefinition.IntPtr);
            return parameters;
        }
    }
}
