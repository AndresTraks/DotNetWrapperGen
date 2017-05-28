using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.Transformer;
using NUnit.Framework;
using System.Linq;

namespace DotNetWrapperGen.Tests.Transformer
{
    [TestFixture(Category = "Transform")]
    public class EnumTransformTests
    {
        [Test]
        public void AddNoneValueToEnum()
        {
            var @namespace = new NamespaceDefinition();
            var @enum = new EnumDefinition("CppEnum");

            @enum.AddChild(new EnumeratorDefinition("VALUE1", "1"));
            @enum.AddChild(new EnumeratorDefinition("VALUE2", "2"));
            @enum.AddChild(new EnumeratorDefinition("VALUE3", "4"));

            @namespace.AddChild(@enum);

            new EnumTransformer().Transform(@namespace, null);

            Assert.AreEqual(4, @enum.Enumerators.Count());

            EnumeratorDefinition enumerator0 = (EnumeratorDefinition)@enum.Children[0];
            Assert.AreEqual("None", enumerator0.Name);
            Assert.AreEqual("0", enumerator0.Value);
        }
    }
}
