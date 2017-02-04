using DotNetWrapperGen.Project;
using System.IO;

namespace DotNetWrapperGen.Writer
{
    public class CSharpWriter
    {
        private WrapperProject _project;

        public CSharpWriter(WrapperProject project)
        {
            _project = project;
        }

        public void Write()
        {
            var root = _project.RootFolderCSharp;
            Directory.CreateDirectory(root.FullPath);
        }
    }
}
