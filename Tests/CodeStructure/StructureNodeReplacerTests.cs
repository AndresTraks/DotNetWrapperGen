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
            var child = new FolderDefinition("child");
            root.AddChild(child);
            var replacement = new RootFolderDefinition(Path.Combine(path, "CppTestProject2"));

            StructureNodeReplacer.Replace(root, replacement);

            Assert.IsNull(root.Parent);
            Assert.AreEqual(0, root.Children.Count);

            Assert.IsNull(replacement.Parent);
            Assert.AreEqual(1, replacement.Children.Count);

            Assert.AreEqual(replacement, child.Parent);
        }
    }
}
