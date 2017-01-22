namespace DotNetWrapperGen.View
{
    partial class CSharpClassesTab
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
            this.classTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // classTree
            // 
            this.classTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.classTree.ImageIndex = 0;
            this.classTree.ImageList = this.imageList;
            this.classTree.Location = new System.Drawing.Point(0, 3);
            this.classTree.Name = "classTree";
            this.classTree.SelectedImageIndex = 0;
            this.classTree.Size = new System.Drawing.Size(617, 300);
            this.classTree.TabIndex = 8;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CSharpClassesTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.classTree);
            this.Name = "CSharpClassesTab";
            this.Size = new System.Drawing.Size(617, 306);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView classTree;
        private System.Windows.Forms.ImageList imageList;
    }
}
