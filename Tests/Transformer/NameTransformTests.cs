using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.Transformer;
using NUnit.Framework;
using System.Linq;

namespace DotNetWrapperGen.Tests.Transformer
{
    [TestFixture(Category = "Transform")]
    public class NameTransformTests
    {
        [Test]
        public void TransformName()
        {
            var @namespace = new NamespaceDefinition();
            var @enum = new EnumDefinition("CppEnum");

            @enum.AddChild(new EnumeratorDefinition("COMMON_PREFIX_NONE", "0"));
            @enum.AddChild(new EnumeratorDefinition("COMMON_PREFIX_VALUE1", "1"));
            @enum.AddChild(new EnumeratorDefinition("COMMON_PREFIX_VALUE2", "COMMON_PREFIX_VALUE1*2"));

            @namespace.AddChild(@enum);

            new EnumTransformer().Transform(@namespace, null);

            Assert.AreEqual(3, @enum.Children.Count());
            Assert.AreEqual(3, @enum.Enumerators.Count());

            EnumeratorDefinition enumerator0 = (EnumeratorDefinition)@enum.Children[0];
            Assert.AreEqual("None", enumerator0.Name);
            Assert.AreEqual("0", enumerator0.Value);

            EnumeratorDefinition enumerator1 = (EnumeratorDefinition)@enum.Children[1];
            Assert.AreEqual("Value1", enumerator1.Name);
            Assert.AreEqual("1", enumerator1.Value);

            EnumeratorDefinition enumerator2 = (EnumeratorDefinition)@enum.Children[2];
            Assert.AreEqual("Value2", enumerator2.Name);
            Assert.AreEqual("Value1*2", enumerator2.Value);
        }
    }
}
