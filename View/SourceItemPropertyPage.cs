using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Project;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotNetWrapperGen.View
{
    public class SourceItemPropertyPage
    {
        public SourceItemPropertyPage(SourceItemDefinition sourceItem, TreeNode sourceNode)
        {
            SourceItem = sourceItem;
            SourceNode = sourceNode;
        }

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
    }
}
