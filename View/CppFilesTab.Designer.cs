namespace DotNetWrapperGen.View
{
    partial class CppFilesTab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.flatViewButton = new System.Windows.Forms.CheckBox();
            this.showExcludedButton = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fileTree = new System.Windows.Forms.TreeView();
            this.sourceItemSettings = new System.Windows.Forms.PropertyGrid();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flatViewButton
            // 
            this.flatViewButton.AutoSize = true;
            this.flatViewButton.Location = new System.Drawing.Point(3, 27);
            this.flatViewButton.Name = "flatViewButton";
            this.flatViewButton.Size = new System.Drawing.Size(69, 17);
            this.flatViewButton.TabIndex = 10;
            this.flatViewButton.Text = "Flat View";
            this.flatViewButton.UseVisualStyleBackColor = true;
            this.flatViewButton.CheckedChanged += new System.EventHandler(this.flatViewButton_CheckedChanged);
            // 
            // showExcludedButton
            // 
            this.showExcludedButton.AutoSize = true;
            this.showExcludedButton.Checked = true;
            this.showExcludedButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showExcludedButton.Location = new System.Drawing.Point(3, 3);
            this.showExcludedButton.Name = "showExcludedButton";
            this.showExcludedButton.Size = new System.Drawing.Size(100, 17);
            this.showExcludedButton.TabIndex = 9;
            this.showExcludedButton.Text = "Show Excluded";
            this.showExcludedButton.UseVisualStyleBackColor = true;
            this.showExcludedButton.CheckedChanged += new System.EventHandler(this.showExcludedButton_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 50);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fileTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sourceItemSettings);
            this.splitContainer1.Size = new System.Drawing.Size(683, 302);
            this.splitContainer1.SplitterDistance = 385;
            this.splitContainer1.TabIndex = 11;
            // 
            // fileTree
            // 
            this.fileTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTree.ImageIndex = 0;
            this.fileTree.ImageList = this.imageList1;
            this.fileTree.Location = new System.Drawing.Point(0, 0);
            this.fileTree.Margin = new System.Windows.Forms.Padding(0);
            this.fileTree.Name = "fileTree";
            this.fileTree.SelectedImageIndex = 0;
            this.fileTree.Size = new System.Drawing.Size(382, 299);
            this.fileTree.TabIndex = 2;
            this.fileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sourceTree_AfterSelect);
            // 
            // sourceItemSettings
            // 
            this.sourceItemSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceItemSettings.CommandsBorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.sourceItemSettings.Enabled = false;
            this.sourceItemSettings.Location = new System.Drawing.Point(0, 0);
            this.sourceItemSettings.Name = "sourceItemSettings";
            this.sourceItemSettings.Size = new System.Drawing.Size(294, 299);
            this.sourceItemSettings.TabIndex = 5;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CppFilesTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.flatViewButton);
            this.Controls.Add(this.showExcludedButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CppFilesTab";
            this.Size = new System.Drawing.Size(689, 355);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox flatViewButton;
        private System.Windows.Forms.CheckBox showExcludedButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView fileTree;
        private System.Windows.Forms.PropertyGrid sourceItemSettings;
        private System.Windows.Forms.ImageList imageList1;
    }
}
