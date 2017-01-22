namespace DotNetWrapperGen.View
{
    partial class ProjectView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectView));
            this.logWindow = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.cppFilesTabPlaceHolder = new System.Windows.Forms.TabPage();
            this.cppFilesTab = new DotNetWrapperGen.View.CppFilesTab();
            this.cppClassesPlaceHolder = new System.Windows.Forms.TabPage();
            this.cppClassesTab = new DotNetWrapperGen.View.CppClassesTab();
            this.csharpFilesPlaceHolder = new System.Windows.Forms.TabPage();
            this.csharpFilesTab = new DotNetWrapperGen.View.CSharpFilesTab();
            this.csharpClassesPlaceHolder = new System.Windows.Forms.TabPage();
            this.csharpClassesTab = new DotNetWrapperGen.View.CSharpClassesTab();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.cppFilesTabPlaceHolder.SuspendLayout();
            this.cppClassesPlaceHolder.SuspendLayout();
            this.csharpFilesPlaceHolder.SuspendLayout();
            this.csharpClassesPlaceHolder.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // logWindow
            // 
            this.logWindow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logWindow.Location = new System.Drawing.Point(1, 337);
            this.logWindow.Multiline = true;
            this.logWindow.Name = "logWindow";
            this.logWindow.ReadOnly = true;
            this.logWindow.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logWindow.Size = new System.Drawing.Size(782, 134);
            this.logWindow.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.cppFilesTabPlaceHolder);
            this.tabControl1.Controls.Add(this.cppClassesPlaceHolder);
            this.tabControl1.Controls.Add(this.csharpFilesPlaceHolder);
            this.tabControl1.Controls.Add(this.csharpClassesPlaceHolder);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(784, 304);
            this.tabControl1.TabIndex = 5;
            // 
            // cppFilesTabPlaceHolder
            // 
            this.cppFilesTabPlaceHolder.Controls.Add(this.cppFilesTab);
            this.cppFilesTabPlaceHolder.Location = new System.Drawing.Point(4, 22);
            this.cppFilesTabPlaceHolder.Name = "cppFilesTabPlaceHolder";
            this.cppFilesTabPlaceHolder.Padding = new System.Windows.Forms.Padding(3);
            this.cppFilesTabPlaceHolder.Size = new System.Drawing.Size(776, 278);
            this.cppFilesTabPlaceHolder.TabIndex = 0;
            this.cppFilesTabPlaceHolder.Text = "C++ Files";
            this.cppFilesTabPlaceHolder.UseVisualStyleBackColor = true;
            // 
            // cppFilesTab
            // 
            this.cppFilesTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cppFilesTab.Location = new System.Drawing.Point(0, 0);
            this.cppFilesTab.Margin = new System.Windows.Forms.Padding(0);
            this.cppFilesTab.Name = "cppFilesTab";
            this.cppFilesTab.Size = new System.Drawing.Size(776, 278);
            this.cppFilesTab.TabIndex = 9;
            // 
            // cppClassesPlaceHolder
            // 
            this.cppClassesPlaceHolder.Controls.Add(this.cppClassesTab);
            this.cppClassesPlaceHolder.Location = new System.Drawing.Point(4, 22);
            this.cppClassesPlaceHolder.Name = "cppClassesPlaceHolder";
            this.cppClassesPlaceHolder.Padding = new System.Windows.Forms.Padding(3);
            this.cppClassesPlaceHolder.Size = new System.Drawing.Size(776, 278);
            this.cppClassesPlaceHolder.TabIndex = 1;
            this.cppClassesPlaceHolder.Text = "C++ Classes";
            this.cppClassesPlaceHolder.UseVisualStyleBackColor = true;
            // 
            // cppClassesTab
            // 
            this.cppClassesTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cppClassesTab.Location = new System.Drawing.Point(0, 0);
            this.cppClassesTab.Name = "cppClassesTab";
            this.cppClassesTab.Size = new System.Drawing.Size(776, 278);
            this.cppClassesTab.TabIndex = 0;
            // 
            // csharpFilesPlaceHolder
            // 
            this.csharpFilesPlaceHolder.Controls.Add(this.csharpFilesTab);
            this.csharpFilesPlaceHolder.Location = new System.Drawing.Point(4, 22);
            this.csharpFilesPlaceHolder.Name = "csharpFilesPlaceHolder";
            this.csharpFilesPlaceHolder.Padding = new System.Windows.Forms.Padding(3);
            this.csharpFilesPlaceHolder.Size = new System.Drawing.Size(776, 278);
            this.csharpFilesPlaceHolder.TabIndex = 2;
            this.csharpFilesPlaceHolder.Text = "C# Files";
            this.csharpFilesPlaceHolder.UseVisualStyleBackColor = true;
            // 
            // csharpFilesTab
            // 
            this.csharpFilesTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.csharpFilesTab.Location = new System.Drawing.Point(0, 0);
            this.csharpFilesTab.Margin = new System.Windows.Forms.Padding(0);
            this.csharpFilesTab.Name = "csharpFilesTab";
            this.csharpFilesTab.Size = new System.Drawing.Size(776, 278);
            this.csharpFilesTab.TabIndex = 0;
            // 
            // csharpClassesPlaceHolder
            // 
            this.csharpClassesPlaceHolder.Controls.Add(this.csharpClassesTab);
            this.csharpClassesPlaceHolder.Location = new System.Drawing.Point(4, 22);
            this.csharpClassesPlaceHolder.Name = "csharpClassesPlaceHolder";
            this.csharpClassesPlaceHolder.Size = new System.Drawing.Size(776, 278);
            this.csharpClassesPlaceHolder.TabIndex = 3;
            this.csharpClassesPlaceHolder.Text = "C# Classes";
            this.csharpClassesPlaceHolder.UseVisualStyleBackColor = true;
            // 
            // csharpClassesTab
            // 
            this.csharpClassesTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.csharpClassesTab.Location = new System.Drawing.Point(0, 0);
            this.csharpClassesTab.Margin = new System.Windows.Forms.Padding(0);
            this.csharpClassesTab.Name = "csharpClassesTab";
            this.csharpClassesTab.Size = new System.Drawing.Size(332, 278);
            this.csharpClassesTab.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // ProjectView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(784, 473);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.logWindow);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ProjectView";
            this.Text = ".NET Wrapper Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.cppFilesTabPlaceHolder.ResumeLayout(false);
            this.cppClassesPlaceHolder.ResumeLayout(false);
            this.csharpFilesPlaceHolder.ResumeLayout(false);
            this.csharpClassesPlaceHolder.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox logWindow;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage cppClassesPlaceHolder;
        private System.Windows.Forms.TabPage csharpFilesPlaceHolder;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.TabPage cppFilesTabPlaceHolder;
        private CppFilesTab cppFilesTab;
        private CppClassesTab cppClassesTab;
        private System.Windows.Forms.TabPage csharpClassesPlaceHolder;
        private CSharpFilesTab csharpFilesTab;
        private CSharpClassesTab csharpClassesTab;
    }
}

