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
            var @enum = new EnumDefinition("CppClass");

            root.AddChild(header);

            @namespace.AddChild(@class);
            header.AddNode(@class);

            @namespace.AddChild(@enum);
            header.AddNode(@enum);

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
            Assert.AreEqual(2, headerClone.Nodes.Count);

            var classClone = (ClassDefinition)headerClone.Nodes[0];
            Assert.AreNotSame(@class, classClone);
            Assert.AreSame(headerClone, classClone.Header);
            Assert.AreEqual(@class.Name, classClone.Name);
            Assert.AreEqual(@class.Children.Count, classClone.Children.Count);
            Assert.AreSame(rootNamespaceClone, classClone.Parent);

            var enumClone = (EnumDefinition)headerClone.Nodes[1];
            Assert.AreNotSame(@enum, enumClone);
            Assert.AreSame(headerClone, enumClone.Header);
            Assert.AreEqual(@enum.Name, enumClone.Name);
            Assert.AreEqual(@enum.Children.Count, enumClone.Children.Count);
            Assert.AreSame(rootNamespaceClone, enumClone.Parent);

            Assert.AreNotSame(@namespace, rootNamespaceClone);
            Assert.AreEqual(@namespace.Children.Count, rootNamespaceClone.Children.Count);
        }
    }
}
