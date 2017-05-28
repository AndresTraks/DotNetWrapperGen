using DotNetWrapperGen.CodeModel;
using System.Linq;

namespace DotNetWrapperGen.View
{
    public class NodeTitleProvider
    {
        public static string Get(ModelNodeDefinition node)
        {
            var method = node as MethodDefinition;
            if (method != null)
            {
                var name = method.Name;
                var parameters = string.Join(", ", method.Parameters.Select(p => p.Name));
                return $"{name}({parameters})";
            }
            return node.Name;
        }
    }
}
