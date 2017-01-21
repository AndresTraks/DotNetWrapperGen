using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DotNetWrapperGen.View
{
    public enum WizardAction
    {
        None,
        New,
        Open,
        Exit
    }

    public partial class StartWizard : Form
    {
        public WizardAction Action { get; private set; }
        public string InputPath { get; private set; }

        public StartWizard()
        {
            InitializeComponent();

            recentList.Controls.Add(CreateRecentLink("Bullet", "D:\\src\\DotNetWrapperGen\\bin\\Debug\\Bullet.xml", 0));

            sourceFolder.TextChanged += sourceFolder_TextChanged;
            sourceFolder.KeyDown += sourceFolder_KeyDown;
            sourceFolder.Text = Application.UserAppDataRegistry.GetValue("SourceFolder") as string;
            if (string.IsNullOrEmpty(sourceFolder.Text))
            {
                sourceFolder.Select();
            }
            else
            {
                sourceFolder.Select(sourceFolder.Text.Length, 0);
            }

            FormClosed += (o, i) =>
            {
                if (Action == WizardAction.None)
                {
                    Action = WizardAction.Exit;
                }
            };
        }

        private LinkLabel CreateRecentLink(string text, string url, int index)
        {
            var label = new LinkLabel
            {
                Text = text,
                Location = new Point(3, index * 22),
                Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 186),
                Tag = url
            };
            label.Click += (s, e) =>
            {
                InputPath = label.Tag as string;
                Action = WizardAction.Open;
                Close();
            };
            return label;
        }

        void sourceFolder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && createButton.Enabled)
            {
                createButton_Click(sender, EventArgs.Empty);
            }
        }

        void sourceFolder_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(sourceFolder.Text))
            {
                createButton.Enabled = true;
            }
            else
            {
                createButton.Enabled = false;
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            var folderBrowse = new FolderBrowserDialog();
            var path = Application.UserAppDataRegistry.GetValue("SourceFolder") as string;
            if (path != null)
            {
                path = path.Replace('/', '\\');
                folderBrowse.SelectedPath = path;
            }
            folderBrowse.ShowNewFolderButton = false;
            var result = folderBrowse.ShowDialog();
            if (result == DialogResult.OK)
            {
                sourceFolder.Text = folderBrowse.SelectedPath;
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            Action = WizardAction.New;
            InputPath = sourceFolder.Text;
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "XML files|*.xml";
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            InputPath = dialog.FileName;
            Action = WizardAction.Open;
            Close();
        }
    }
}
