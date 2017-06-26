using DotNetWrapperGen.CodeModel;
using System.IO;
using System;

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

            if (method.IsExtern)
            {
                _writer.Write("\t\t");
                _writer.WriteLine("[DllImport(Native.Dll, CallingConvention = Native.Conv)]");
            }

            string managedName = method.Name.Substring(0, 1).ToUpper() + method.Name.Substring(1);
            string name = method.IsExtern ? method.Name : managedName;

            _writer.Write("\t\t");

            string accessSpecifier = "public";
            _writer.Write(accessSpecifier + " ");

            if (method.IsStatic)
            {
                _writer.Write("static ");
            }

            if (method.IsExtern)
            {
                _writer.Write("extern ");
            }

            if (method.IsExtern || !method.IsConstructor)
            {
                string returnType = method.ReturnType.ManagedTypeRefName;
                _writer.Write(returnType);
                _writer.Write(" ");
            }

            _writer.Write($"{name}(");

            var lastParameterIndex = method.Parameters.Length - 1;
            for (int i = 0; i < method.Parameters.Length; i++)
            {
                WriteParameter(method.Parameters[i]);
                if (i != lastParameterIndex)
                {
                    _writer.Write(", ");
                }
            }

            if (method.IsExtern)
            {
                _writer.WriteLine(");");
            }
            else
            {
                _writer.WriteLine(")");
                _writer.WriteLine("\t\t{");
                _writer.WriteLine("\t\t}");
            }
        }

        private void WriteParameter(ParameterDefinition parameter)
        {
            _writer.Write(parameter.Type.ManagedTypeRefName);
            _writer.Write(" ");
            _writer.Write(parameter.Name);
        }
    }
}
