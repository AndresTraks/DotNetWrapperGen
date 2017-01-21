using System.Collections.Generic;

namespace DotNetWrapperGen.CodeModel
{
    public class ModelNodeDefinition
    {
        public ModelNodeDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public ModelNodeDefinition Parent { get; set; }
        public IList<ModelNodeDefinition> Children { get; } = new List<ModelNodeDefinition>();
    }
}
