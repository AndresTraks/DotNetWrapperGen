using DotNetWrapperGen.CodeModel;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.CodeStructure
{
    public class HeaderDefinition : SourceItemDefinition
    {
        public HeaderDefinition(string name)
            : base(name)
        {
            Includes = new List<HeaderDefinition>();
        }

        public override bool IsFolder => false;
        public bool HasError { get; set; }
        public List<ModelNodeDefinition> Nodes { get; } = new List<ModelNodeDefinition>();
        public List<HeaderDefinition> Includes { get; } = new List<HeaderDefinition>();

        public IEnumerable<ClassDefinition> Classes => Children.OfType<ClassDefinition>();

        string _managedName;
        public string ManagedName
        {
            get { return _managedName ?? Name; }
            set { _managedName = value; }
        }

        public void AddNode(ModelNodeDefinition node)
        {
            Nodes.Add(node);
            node.Header = this;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
