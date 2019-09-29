using DotNetWrapperGen.Tokenizer;
using System.Collections.Generic;
using System.IO;

namespace DotNetWrapperGen.Writer
{
    public class TokenWriter
    {
        private StreamWriter _writer;
        private TokenizerContext _writerContext;

        private int _indent = 0;
        private readonly IDictionary<int, string> _indentStrings = new Dictionary<int, string>();

        public void Write(string fullPath, TokenizerContext context)
        {
            _writerContext = context;

            using (var stream = File.Create(fullPath))
            {
                using (_writer = new StreamWriter(stream))
                {
                    WriteTokens();
                }
            }
        }

        private void WriteTokens()
        {
            IToken precedingToken = null;
            foreach (IToken token in _writerContext.RootTokens)
            {
                WriteToken(token, precedingToken);
                precedingToken = token;
            }
        }

        private void WriteToken(IToken token)
        {
            WriteToken(token, null);
        }

        private void WriteToken(IToken token, IToken precedingToken)
        {
            switch (token)
            {
                case StringToken @string:
                    if (token is WordToken && precedingToken != null)
                    {
                        _writer.Write(' ');
                    }
                    _writer.Write(@string.Value);
                    break;
                case LineToken line:
                    WriteLine(line);
                    break;
                case BlockToken block:
                    if (precedingToken != null)
                    {
                        _writer.WriteLine();
                    }
                    WriteBlock(block);
                    break;
                case ExpressionBodyToken body:
                    if (precedingToken != null)
                    {
                        _writer.WriteLine();
                    }
                    WriteLine(body.Line);
                    break;
                case LinesToken lines:
                    {
                        foreach (LineToken lineToken in lines.Lines)
                        {
                            WriteLine(lineToken);
                        }
                        break;
                    }

                case ListToken list:
                    WriteList(list);
                    break;
            }
        }

        private void WriteBlock(BlockToken block)
        {
            WriteToken(block.Header);

            WriteIndent();
            _writer.WriteLine('{');
            _indent++;

            IToken precedingToken = null;
            foreach (IToken child in block.Children)
            {
                WriteToken(child, precedingToken);
                precedingToken = child;
            }

            _indent--;
            WriteIndent();
            _writer.WriteLine('}');
        }

        private void WriteLine(LineToken line)
        {
            WriteIndent();
            IToken precedingToken = null;
            foreach (IToken element in line.Elements)
            {
                WriteToken(element, precedingToken);
                precedingToken = element;
            }
            _writer.WriteLine();
        }

        private void WriteList(ListToken list)
        {
            IToken precedingToken = null;
            var lastParameterIndex = list.Elements.Count - 1;
            for (int i = 0; i < list.Elements.Count; i++)
            {
                IToken element = list.Elements[i];
                WriteToken(element, precedingToken);
                if (i != lastParameterIndex)
                {
                    _writer.Write(',');
                }
                precedingToken = element;
            }
        }

        private void WriteIndent()
        {
            _writer.Write(GetIndentString());
        }

        private string GetIndentString()
        {
            if (!_indentStrings.TryGetValue(_indent, out string value))
            {
                value = new string('\t', _indent);
                _indentStrings[_indent] = value;
            }
            return value;
        }
    }
}
