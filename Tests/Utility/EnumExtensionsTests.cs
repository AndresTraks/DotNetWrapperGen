using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.Utility;
using NUnit.Framework;

namespace DotNetWrapperGen.Tests.Utility
{
    [TestFixture(Category = "Utility")]
    public class EnumExtensionsTests
    {
        [Test]
        public void IsFlags()
        {
            var @enum = new EnumDefinition("CppEnum");

            @enum.AddChild(new EnumeratorDefinition("None", "0x00"));
            @enum.AddChild(new EnumeratorDefinition("Value1", "0x01"));
            @enum.AddChild(new EnumeratorDefinition("Value2", "0x02"));
            @enum.AddChild(new EnumeratorDefinition("Value3", "4"));
            @enum.AddChild(new EnumeratorDefinition("Value4", "0x40000000"));

            Assert.IsTrue(@enum.IsFlags());
        }
    }
}
