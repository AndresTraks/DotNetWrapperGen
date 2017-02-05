using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Parser;
using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace DotNetWrapperGen.Tests.CodeModel
{
    [TestFixture(Category = "Model")]
    public class CodeModelTests
    {
        [Test]
        public void CodeModelParse()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path);
            path = Path.Combine(path, "CppTestProject");

            var rootFolder = new RootFolderDefinition(path);

            var header = new HeaderDefinition("header1.h");
            rootFolder.AddChild(header);

            NamespaceDefinition globalNamespace = new CppParser().ParseRootFolder(rootFolder);

            Assert.AreEqual(1, globalNamespace.Children.Count);
        }
    }
}
