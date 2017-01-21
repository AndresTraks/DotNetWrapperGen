namespace DotNetWrapperGen.View
{
    partial class CppClassesTab
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
            this.parseFilesButton = new System.Windows.Forms.Button();
            this.classTree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // parseFilesButton
            // 
            this.parseFilesButton.Location = new System.Drawing.Point(3, 10);
            this.parseFilesButton.Name = "parseFilesButton";
            this.parseFilesButton.Size = new System.Drawing.Size(129, 41);
            this.parseFilesButton.TabIndex = 8;
            this.parseFilesButton.Text = "Parse Files";
            this.parseFilesButton.UseVisualStyleBackColor = true;
            this.parseFilesButton.Click += new System.EventHandler(this.parseFilesButton_Click);
            // 
            // classTree
            // 
            this.classTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.classTree.ImageIndex = 0;
            this.classTree.ImageList = this.imageList1;
            this.classTree.Location = new System.Drawing.Point(0, 57);
            this.classTree.Name = "classTree";
            this.classTree.SelectedImageIndex = 0;
            this.classTree.Size = new System.Drawing.Size(616, 284);
            this.classTree.TabIndex = 7;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CppClassesTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.parseFilesButton);
            this.Controls.Add(this.classTree);
            this.Name = "CppClassesTab";
            this.Size = new System.Drawing.Size(619, 341);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button parseFilesButton;
        private System.Windows.Forms.TreeView classTree;
        private System.Windows.Forms.ImageList imageList1;
    }
}
