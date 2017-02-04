using DotNetWrapperGen.Project;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace DotNetWrapperGen.View
{
    public partial class ProjectView : Form
    {
        public WrapperProject Project { get; set; }

        public ProjectView()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var wizard = new StartWizard();
            wizard.ShowDialog();
            switch (wizard.Action)
            {
                case WizardAction.Exit:
                    Close();
                    break;
                case WizardAction.New:
                    Project = new WrapperProject(wizard.InputPath);
                    Project.WrapperEvent += project_WrapperEvent;
                    Text = wizard.InputPath;
                    Project.ReadAsync();
                    break;
                case WizardAction.Open:
                    Project = new WrapperProject();
                    Project.WrapperEvent += project_WrapperEvent;
                    Text = wizard.InputPath;
                    Project.Load(wizard.InputPath);
                    break;
            }
        }

        void project_WrapperEvent(object sender, WrapperProjectEventArgs e)
        {
            if (e.Event == WrapperProjectEvent.StatusChanged)
            {
                BeginInvoke((Action)(() => HandleEvent(e.Status)));
            }
            else if (e.Event == WrapperProjectEvent.LogMessage)
            {
                Log(e.Text);
            }
        }

        private void HandleEvent(WrapperStatus status)
        {
            switch (status)
            {
                case WrapperStatus.ReadingHeaders:
                    Log("Finding input files...\r\n");
                    break;
                case WrapperStatus.ReadingHeadersDone:
                    cppFilesTab.SetData(Project.RootFolder);
                    Application.UserAppDataRegistry.SetValue("SourceFolder", Project.FullSourcePath);
                    Project.ParseAsync();
                    break;
                case WrapperStatus.ParsingHeaders:
                    cppFilesTab.SetData(null);
                    cppClassesTab.SetData(null);
                    Log("Parsing input files...\r\n");
                    break;
                case WrapperStatus.ParsingHeadersDone:
                    cppFilesTab.SetData(Project.RootFolder);
                    cppClassesTab.SetData(Project);
                    csharpFilesTab.SetData(Project.RootFolder);

                    Log("Transforming C++ to C#...\r\n");
                    Project.TransformAsync();
                    break;
                case WrapperStatus.TransformingCppDone:
                    csharpFilesTab.SetData(Project.RootFolderCSharp);
                    csharpClassesTab.SetData(Project);

                    Log("Writing wrapper...\r\n");
                    Project.WriteWrapperAsync();
                    break;
                case WrapperStatus.WritingWrapperDone:
                    Log("Done\r\n");
                    break;
            }
        }

        void Log(string message)
        {
            logWindow.Invoke((MethodInvoker)(() => {
                logWindow.Text += message;
                logWindow.Select(logWindow.Text.Length, 0);
            }));
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = Project.NamespaceName + ".xml";
            dialog.InitialDirectory = Project.FullProjectPath;
            dialog.Filter = "XML files|*.xml";
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            using (var writer = XmlWriter.Create(dialog.FileName))
            {
                writer.WriteStartElement("WrapperProject");
                writer.WriteStartElement("CppCodeModel");
                Project.WriteSourceItemXml(writer, Project.RootFolder, Path.GetDirectoryName(dialog.FileName) + Path.DirectorySeparatorChar);
            }
        }
    }
}
