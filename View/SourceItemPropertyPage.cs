using DotNetWrapperGen.CodeStructure;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotNetWrapperGen.View
{
    public class SourceItemPropertyPage
    {
        [Browsable(false)]
        public SourceItemDefinition SourceItem { get; set; }
        [Browsable(false)]
        public TreeNode SourceNode { get; set; }

        [Description("Excluded source items will not be parsed."), Category("General")]
        public bool IsExcluded
        {
            get { return SourceItem.IsExcluded; }
            set { SourceItem.IsExcluded = value; }
        }

        [Description("Folders where to search for included header files."), Category("General")]
        public BindingList<string> IncludeFolders
        {
            get { return new BindingList<string>(SourceItem.IncludeFolders); }
        }
    }
}
