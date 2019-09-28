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
            if (token is StringToken @string)
            {
                _writer.Write(@string.Value);
            }
            else if (token is LineToken line)
            {
                WriteLine(line);
            }
            else if (token is BlockToken block)
            {
                if (precedingToken != null)
                {
                    _writer.WriteLine();
                }
                WriteBlock(block);
            }
            else if (token is LinesToken lines)
            {
                foreach (LineToken lineToken in lines.Lines)
                {
                    WriteLine(lineToken);
                }
            }
        }

        private void WriteBlock(BlockToken block)
        {
            WriteToken(block.Header);

            WriteIndent();
            _writer.WriteLine('{');
            _indent++;

            IToken precedingChildToken = null;
            foreach (IToken child in block.Children)
            {
                WriteToken(child, precedingChildToken);
                precedingChildToken = child;
            }

            _indent--;
            WriteIndent();
            _writer.WriteLine('}');
        }

        private void WriteLine(LineToken line)
        {
            WriteIndent();
            foreach (IToken element in line.Elements)
            {
                WriteToken(element);
            }
            _writer.WriteLine();
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
