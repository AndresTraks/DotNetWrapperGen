using DotNetWrapperGen.CodeStructure;
using System;
using System.IO;
using System.Xml;

namespace DotNetWrapperGen.Project
{
    public class ProjectFileLoader
    {
        public RootFolderDefinition Load(string inputPath)
        {
            RootFolderDefinition rootFolder = null;
            using (var reader = XmlReader.Create(inputPath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("WrapperProject"))
                    {
                        rootFolder = LoadProject(reader);
                    }
                }
            }
            return rootFolder;
        }

        private RootFolderDefinition LoadProject(XmlReader reader)
        {
            RootFolderDefinition rootFolder = null;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("CppCodeModel"))
                    {
                        rootFolder = LoadRootFolder(reader);
                    }
                }
            }
            return rootFolder;
        }

        private RootFolderDefinition LoadRootFolder(XmlReader reader)
        {
            RootFolderDefinition rootFolder = null;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var name = reader.GetAttribute("Name");
                    var isExcluded = "true".Equals(reader.GetAttribute("IsExcluded"));

                    if (reader.Name.Equals("RootFolder"))
                    {
                        var uri = new Uri(Path.GetDirectoryName(new Uri(reader.BaseURI).LocalPath) + '\\' + name);
                        rootFolder = new RootFolderDefinition(uri.AbsolutePath)
                        {
                            IsExcluded = isExcluded
                        };

                        LoadSourceItems(reader, rootFolder);
                    }
                }
            }
            return rootFolder;
        }

        private void LoadSourceItems(XmlReader reader, SourceItemDefinition parentFolder)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    SourceItemDefinition item;
                    var name = reader.GetAttribute("Name");
                    var isExcluded = "true".Equals(reader.GetAttribute("IsExcluded"));

                    if (reader.Name.Equals("Folder"))
                    {
                        item = new FolderDefinition(name)
                        {
                            Parent = parentFolder
                        };
                    }
                    else if (reader.Name.Equals("Header"))
                    {
                        item = new HeaderDefinition(name)
                        {
                            Parent = parentFolder
                        };
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    parentFolder.Children.Add(item);

                    item.IsExcluded = isExcluded;

                    if (item.IsFolder)
                    {
                        LoadSourceItems(reader, item);
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    return;
                }
            }
        }
    }
}
