using DotNetWrapperGen.CodeModel;
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
            var method = new MethodDefinition("method");

            Assert.AreEqual("method", method.GetMangledName());
        }
    }
}
