using DotNetWrapperGen.CodeModel;
using System.IO;

namespace DotNetWrapperGen.Writer.CSharp
{
    public class CSharpMethodWriter
    {
        private StreamWriter _writer;

        public CSharpMethodWriter(StreamWriter writer)
        {
            _writer = writer;
        }

        public void Write(MethodDefinition method)
        {
            if (method.IsExcluded)
            {
                return;
            }

            string returnType = method.ReturnType.ManagedTypeRefName;
            string name = method.ManagedName;

            _writer.WriteLine($"\t\tpublic {returnType} {name}()");
            _writer.WriteLine("\t\t{");
            _writer.WriteLine("\t\t}");
        }
    }
}
