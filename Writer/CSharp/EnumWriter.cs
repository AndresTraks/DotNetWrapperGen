using DotNetWrapperGen.CodeModel;
using System.IO;
using System.Linq;

namespace DotNetWrapperGen.Writer.CSharp
{
    public class EnumWriter : INodeWriter
    {
        public void Write(ModelNodeDefinition node, StreamWriter writer)
        {
            var @enum = (EnumDefinition)node;

            writer.WriteLine($"\tpublic enum {@enum.Name}");
            writer.WriteLine("\t{");

            var lastEnumerator = @enum.Enumerators.Last();
            foreach (EnumeratorDefinition enumerator in @enum.Enumerators)
            {
                if (enumerator.Value != null)
                {
                    writer.Write($"\t\t{enumerator.Name} = {enumerator.Value}");
                }
                else
                {
                    writer.Write($"\t\t{enumerator.Name}");
                }

                if (enumerator != lastEnumerator)
                {
                    writer.WriteLine(",");
                }
                else
                {
                    writer.WriteLine();
                }
            }

            writer.WriteLine("\t}");
        }
    }
}
