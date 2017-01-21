using DotNetWrapperGen.CodeStructure;
using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace DotNetWrapperGen.Tests.CodeStructure
{
    [TestFixture(Category="Structure")]
    public class CodeStructureTests
    {
        [Test]
        public void CodeStructureParse()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path);
            path = Path.Combine(path, "CppTestProject");

            RootFolderDefinition rootFolder = CodeStructureParser.Parse(path);
            Assert.AreEqual(1, rootFolder.Children.Count);
            Assert.AreEqual("header1.h", rootFolder.Children[0].Name);
        }
    }
}
