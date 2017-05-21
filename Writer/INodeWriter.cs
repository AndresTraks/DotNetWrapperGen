using DotNetWrapperGen.CodeModel;
using System.IO;

namespace DotNetWrapperGen.Writer
{
    public interface INodeWriter
    {
        void Write(ModelNodeDefinition node, StreamWriter writer);
    }
}
