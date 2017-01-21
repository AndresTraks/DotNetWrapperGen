using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Parser;
using NUnit.Framework;

namespace DotNetWrapperGen.Tests.CodeModel
{
    [TestFixture(Category = "Name Decorator")]
    public class VcppNameDecoratorTests
    {
        [Test]
        public void Simple()
        {
            var header = new HeaderDefinition("test.h");
            var @namespace = new NamespaceDefinition("");
            var @class = new ClassDefinition("TestClass", @namespace);
            var method = new MethodDefinition("method", @class);

            Assert.AreEqual("method", method.GetMangledName());
        }
    }
}
