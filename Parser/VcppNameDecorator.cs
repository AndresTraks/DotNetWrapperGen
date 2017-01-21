using DotNetWrapperGen.CodeModel;
using System;

namespace DotNetWrapperGen.Parser
{
    public static class VcppNameDecorator
    {
        // https://en.wikiversity.org/wiki/Visual_C%2B%2B_name_mangling
        public static string GetMangledName(this MethodDefinition method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
           } 
            return method.Name;
        }
    }
}
