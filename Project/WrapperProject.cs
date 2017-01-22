using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Parser;
using DotNetWrapperGen.Transformer;
using System;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace DotNetWrapperGen.Project
{
    public enum WrapperProjectEvent
    {
        NameSpaceNameChanged,
        StatusChanged,
        LogMessage
    }

    public enum WrapperStatus
    {
        Idle,
        ParsingHeaders,
        ParsingHeadersDone,
        ReadingHeaders,
        ReadingHeadersDone,
        TransformingCpp,
        TransformingCppDone
    }

    public class WrapperProject
    {
        public string FullSourcePath { get; set; }
        public string FullProjectPath { get; set; }
        public string NamespaceName { get; set; }
        public RootFolderDefinition RootFolder { get; set; }
        public NamespaceDefinition GlobalNamespaceCpp { get; private set; }
        public NamespaceDefinition GlobalNamespaceCSharp { get; private set; }

        public WrapperStatus Status { get; private set; }

        public event EventHandler<WrapperProjectEventArgs> WrapperEvent;

        public WrapperProject()
        {

        }

        public WrapperProject(string sourceFolder)
        {
            FullSourcePath = Path.GetFullPath(sourceFolder).Replace('\\', '/');
            if (FullSourcePath.EndsWith("/"))
            {
                FullSourcePath = FullSourcePath.Substring(0, FullSourcePath.Length - 1);
            };

            var namespaceName = Path.GetFileName(FullSourcePath);
            NamespaceName = ConventionConverter.Convert(namespaceName);
        }

        public void WorkAsync(DoWorkEventHandler workHandler)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += workHandler;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        public void ReadAsync()
        {
            WorkAsync((s, e) =>
            {
                SetStatus(WrapperStatus.ReadingHeaders);
                Read();
                SetStatus(WrapperStatus.ReadingHeadersDone);
            });
        }

        public void ParseAsync()
        {
            WorkAsync((s, e) =>
            {
                SetStatus(WrapperStatus.ParsingHeaders);
                try
                {
                    GlobalNamespaceCpp = new CppParser().ParseRootFolder(RootFolder);
                }
                catch (Exception ex)
                {
                    //WrapperEvent.Invoke(this, new WrapperProjectEventArgs(WrapperProjectEvent.LogMessage, ex.ToString()));
                }
                SetStatus(WrapperStatus.ParsingHeadersDone);
            });
        }

        public void TransformAsync()
        {
            WorkAsync((s, e) =>
            {
                SetStatus(WrapperStatus.TransformingCpp);
                try
                {
                    GlobalNamespaceCSharp = DotNetTransformer.Clone(GlobalNamespaceCpp);
                    DotNetTransformer.MoveSymbolsToClasses(GlobalNamespaceCSharp);
                }
                catch (Exception ex)
                {
                    //WrapperEvent.Invoke(this, new WrapperProjectEventArgs(WrapperProjectEvent.LogMessage, ex.ToString()));
                }
                SetStatus(WrapperStatus.TransformingCppDone);
            });
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Status == WrapperStatus.ReadingHeadersDone)
            {
                //WrapperEvent.Invoke(sender, new WrapperProjectEventArgs(WrapperProjectEvent.ReadCompleted));
            }
        }


        public void Log(string message)
        {
            WrapperEvent.Invoke(this, new WrapperProjectEventArgs(WrapperProjectEvent.LogMessage, message));
        }

        public void SetStatus(WrapperStatus status)
        {
            Status = status;
            WrapperEvent.Invoke(this, new WrapperProjectEventArgs(WrapperProjectEvent.StatusChanged, status));
        }

        public void Read()
        {
            RootFolder = CodeStructureParser.Parse(FullSourcePath);
        }

        public void Load(string inputPath)
        {
            FullProjectPath = Path.GetDirectoryName(inputPath);

            ProjectFileLoader loader = new ProjectFileLoader();
            RootFolder = loader.Load(inputPath);
            FullSourcePath = RootFolder.FullPath;
            SetStatus(WrapperStatus.ReadingHeadersDone);
        }
        
        public void WriteSourceItemXml(XmlWriter writer, SourceItemDefinition item, string xmlFilePath = "")
        {
            string name = item.IsFolder
                ? ((item is RootFolderDefinition) ? "RootFolder" : "Folder")
                : "Header";

            writer.WriteStartElement(name);
            if (item == RootFolder)
            {
                writer.WriteAttributeString("Name", MakeRelativePath(xmlFilePath, item.Name));
            }
            else
            {
                writer.WriteAttributeString("Name", item.Name);
            }

            if (item.IsExcluded)
            {
                writer.WriteAttributeString("IsExcluded", "true");
            }

            foreach (var child in item.Children)
            {
                WriteSourceItemXml(writer, child);
            }

            writer.WriteEndElement();
        }

        public static string MakeRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            var fromUri = new Uri(fromPath);
            var toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            var relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.ToUpperInvariant() == "FILE")
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
    }

    public class WrapperProjectEventArgs : EventArgs
    {
        public WrapperProjectEventArgs(WrapperProjectEvent e)
        {
            Event = e;
        }

        public WrapperProjectEventArgs(WrapperProjectEvent e, string text)
        {
            Event = e;
            Text = text;
        }

        public WrapperProjectEventArgs(WrapperProjectEvent e, WrapperStatus status)
        {
            Event = e;
            Status = status;
        }

        public WrapperProjectEvent Event { get; set; }
        public string Text { get; set; }
        public WrapperStatus Status { get; set; }
    }
}
