using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Transformer;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotNetWrapperGen.Tests.CodeStructure
{
    [TestFixture(Category = "Structure")]
    public class StructureClonerTests
    {
        [Test]
        public void Clone()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path);

            var root = new RootFolderDefinition(Path.Combine(path, "CppTestProject"));
            var header = new HeaderDefinition("header1.h");
            var @namespace = new NamespaceDefinition();
            var @class = new ClassDefinition("CppClass");

            root.AddChild(header);

            @namespace.AddChild(@class);
            header.AddNode(@class);

            var cloner = new StructureCloner();
            cloner.Clone(@namespace);
            var rootClone = cloner.RootFolderClone;
            var rootNamespaceClone = cloner.RootNamespaceClone;


            Assert.AreNotSame(root, rootClone);
            Assert.AreEqual(root.FullPath, rootClone.FullPath);
            Assert.AreEqual(root.Children.Count, rootClone.Children.Count);

            var headerClone = (HeaderDefinition)rootClone.Children.First();
            Assert.AreNotSame(header, headerClone);
            Assert.AreSame(rootClone, headerClone.Parent);
            Assert.AreEqual(1, headerClone.Nodes.Count);

            var classClone = (ClassDefinition)headerClone.Nodes.First();
            Assert.AreNotSame(@class, classClone);
            Assert.AreSame(headerClone, classClone.Header);
            Assert.AreEqual(@class.Name, classClone.Name);
            Assert.AreEqual(@class.Children.Count, classClone.Children.Count);
            Assert.AreSame(rootNamespaceClone, classClone.Parent);

            Assert.AreNotSame(@namespace, rootNamespaceClone);
            Assert.AreEqual(@namespace.Children.Count, rootNamespaceClone.Children.Count);
        }
    }
}
