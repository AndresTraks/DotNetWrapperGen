using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace DotNetWrapperGen.Tests.CodeStructure
{
    [TestFixture(Category = "Structure")]
    public class StructureNodeReplacerTests
    {
        [Test]
        public void ReplaceRootNode()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path);

            var root = new RootFolderDefinition(Path.Combine(path, "CppTestProject"));
            var header = new HeaderDefinition("header1.h");
            root.AddChild(header);
            var @class = new ClassDefinition("CppClass");
            header.AddNode(@class);

            var replacement = new RootFolderDefinition(Path.Combine(path, "CppTestProject2"));

            StructureNodeReplacer.Replace(root, replacement);

            Assert.IsNull(root.Parent);
            Assert.AreEqual(0, root.Children.Count);

            Assert.IsNull(replacement.Parent);
            Assert.AreEqual(1, replacement.Children.Count);

            Assert.AreEqual(replacement, header.Parent);
        }
    }
}
