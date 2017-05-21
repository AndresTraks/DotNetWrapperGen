using DotNetWrapperGen.CodeModel;
using System.IO;

namespace DotNetWrapperGen.Writer.CSharp
{
    public class EnumWriter : INodeWriter
    {
        public void Write(ModelNodeDefinition node, StreamWriter writer)
        {
            var @enum = (EnumDefinition)node;

            writer.WriteLine($"\tpublic enum {@enum.Name}");
            writer.WriteLine("\t{");

            foreach (EnumeratorDefinition enumerator in @enum.Enumerators)
            {
                if (enumerator.Value != null)
                {
                    writer.WriteLine($"\t\t{enumerator.Name} = {enumerator.Value};");
                }
                else
                {
                    writer.WriteLine($"\t\t{enumerator.Name};");
                }
            }

            writer.WriteLine("\t}");
        }
    }
}
