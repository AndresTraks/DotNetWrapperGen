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
                        rootFolder = ReadProject(reader);
                    }
                }
            }
            return rootFolder;
        }

        private RootFolderDefinition ReadProject(XmlReader reader)
        {
            RootFolderDefinition rootFolder = null;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("CppCodeModel"))
                    {
                        rootFolder = ReadRootFolder(reader);
                    }
                }
            }
            return rootFolder;
        }

        private RootFolderDefinition ReadRootFolder(XmlReader reader)
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

                        ReadSourceItems(reader, rootFolder);
                    }
                }
            }
            return rootFolder;
        }

        private void ReadSourceItems(XmlReader reader, SourceItemDefinition parent)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Folder":
                        case "Header":
                            {
                                SourceItemDefinition item;
                                var name = reader.GetAttribute("Name");
                                var isExcluded = "true".Equals(reader.GetAttribute("IsExcluded"));

                                if (reader.Name.Equals("Folder"))
                                {
                                    item = new FolderDefinition(name);
                                }
                                else // if (reader.Name.Equals("Header"))
                                {
                                    item = new HeaderDefinition(name);
                                }

                                parent.AddChild(item);

                                item.IsExcluded = isExcluded;

                                if (item.IsFolder)
                                {
                                    ReadSourceItems(reader, item);
                                }
                            }
                            break;
                        case "IncludeFolder":
                            ReadIncludeFolder(reader, parent);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    return;
                }
            }
        }

        public void ReadIncludeFolder(XmlReader reader, SourceItemDefinition item)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    if (item.HasDefaultIncludeFolders)
                    {
                        item.IncludeFolders.Clear();
                        item.IncludeFolders.Add(reader.Value);
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
