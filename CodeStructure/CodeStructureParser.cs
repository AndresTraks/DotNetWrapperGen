using System.IO;
using System.Linq;

namespace DotNetWrapperGen.CodeStructure
{
    public class CodeStructureParser
    {
        public static RootFolderDefinition Parse(string rootFolderPath)
        {
            var rootFolder = new RootFolderDefinition(rootFolderPath);

            var headerFiles = Directory.EnumerateFiles(rootFolderPath, "*.h", SearchOption.AllDirectories);
            foreach (string headerPath in headerFiles)
            {
                var path = headerPath.Replace('\\', '/');
                path = path.Substring(rootFolderPath.Length + 1);

                var headerFolder = CreateFoldersOnPath(path, rootFolder);

                string fileName = Path.GetFileName(path);
                headerFolder.AddChild(new HeaderDefinition(fileName));
            }

            return rootFolder;
        }

        private static SourceItemDefinition CreateFoldersOnPath(string path, RootFolderDefinition rootFolder)
        {
            SourceItemDefinition folder = rootFolder;

            while (path.Contains('/'))
            {
                var index = path.IndexOf('/');
                string folderName = path.Substring(0, path.IndexOf('/'));
                var existingFolder = folder.Children.FirstOrDefault(c => c.Name == folderName);
                if (existingFolder != null)
                {
                    folder = existingFolder;
                }
                else
                {
                    var newFolder = new FolderDefinition(folderName) { Parent = folder };
                    folder.Children.Add(newFolder);
                    folder = newFolder;
                }
                path = path.Substring(index + 1);
            }

            return folder;
        }
    }
}
