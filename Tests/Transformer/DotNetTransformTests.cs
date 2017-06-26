using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Transformer;
using NUnit.Framework;
using System.Linq;

namespace DotNetWrapperGen.Tests.Transformer
{
    [TestFixture(Category = "Transform")]
    public class DotNetTransformTests
    {
        [Test]
        public void CreatesNativeMethodImports()
        {
            var @namespace = new NamespaceDefinition();
            var @class = new ClassDefinition("CppClass");
            var method = new MethodDefinition("method");

            @namespace.AddChild(@class);
            @class.AddChild(method);

            var rootFolder = new RootFolderDefinition("root");
            var header = new HeaderDefinition("header.h");
            rootFolder.AddChild(header);

            header.AddNode(@class);

            new DotNetTransformer().Transform(@namespace, rootFolder);

            var methodsHeader = rootFolder.Children.FirstOrDefault(c => c.Name == "UnsafeNativeMethods.cs");
            Assert.IsNotNull(methodsHeader);

            var methodsClass = @namespace.Children.FirstOrDefault(c => c.Name == "UnsafeNativeMethods") as ClassDefinition;
            Assert.IsNotNull(methodsClass);

            Assert.That(methodsClass.Children, Has.Count.EqualTo(1));
            var nativeMethod = methodsClass.Methods.First();

            Assert.AreEqual("CppClass_method", nativeMethod.Name);
            Assert.IsTrue(nativeMethod.IsStatic);
            Assert.IsTrue(nativeMethod.IsExtern);

            Assert.That(nativeMethod.Parameters, Has.Length.EqualTo(1));
            var objParameter = nativeMethod.Parameters[0];

            Assert.AreEqual("obj", objParameter.Name);
        }
    }
}
