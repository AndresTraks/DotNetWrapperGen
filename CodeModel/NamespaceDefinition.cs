using System.Collections.Generic;
using System.Linq;

namespace DotNetWrapperGen.CodeModel
{
    public class NamespaceDefinition : ModelNodeDefinition
    {
        public NamespaceDefinition(string name)
            : base(name)
        {
        }

        public IEnumerable<NamespaceDefinition> Namespaces => Children.OfType<NamespaceDefinition>();

        public string FullName
        {
            get
            {
                var parentNamespace = Parent as NamespaceDefinition;
                if (parentNamespace != null)
                {
                    return $"{parentNamespace.FullName}::{Name}";
                }
                return Name;
            }
        }

        public string FullNameCSharp
        {
            get
            {
                var parentNamespace = Parent as NamespaceDefinition;
                if (parentNamespace != null)
                {
                    return $"{parentNamespace.FullNameCSharp}::{Name}";
                }
                return Name;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
