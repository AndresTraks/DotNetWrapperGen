using ClangSharp;
using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using DotNetWrapperGen.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetWrapperGen.Parser
{
    public class CppParser
    {
        private CppParserContext _context;

        public NamespaceDefinition ParseRootFolder(RootFolderDefinition rootFolder)
        {
            InitializeContext(rootFolder);

            using (_context.Index = new Index())
            {
                foreach (var sourceItem in rootFolder.Children)
                {
                    ParseSourceItem(sourceItem);
                }
                _context.Index = null;
            }

            return _context.Namespace;
        }

        private void InitializeContext(RootFolderDefinition rootFolder)
        {
            _context = new CppParserContext(rootFolder)
            {
                NodeVisitor = HeaderVisitor
            };

            var classParser = new ClassParser();
            var methodParser = new MethodParser();

            _context.DefinitionParsers.Add(CursorKind.ClassDecl, classParser);
            _context.DefinitionParsers.Add(CursorKind.ClassTemplate, classParser);
            _context.DefinitionParsers.Add(CursorKind.EnumDecl, classParser);
            _context.DefinitionParsers.Add(CursorKind.StructDecl, classParser);
            _context.DefinitionParsers.Add(CursorKind.Namespace, new NamespaceParser());
            _context.DeclarationParsers.Add(CursorKind.FieldDecl, new FieldParser());
            _context.DeclarationParsers.Add(CursorKind.CxxMethod, methodParser);
            _context.DeclarationParsers.Add(CursorKind.Constructor, methodParser);

            var globalNamespace = new NamespaceDefinition("");
            _context.Namespace = globalNamespace;
        }

        private void ParseSourceItem(SourceItemDefinition sourceItem)
        {
            if (!sourceItem.IsExcluded)
            {
                if (sourceItem.IsFolder)
                {
                    ParseFolder(sourceItem as FolderDefinition);
                }
                else
                {
                    ParseHeader(sourceItem as HeaderDefinition);
                }
            }
        }

        private void ParseFolder(FolderDefinition folder)
        {
            foreach (var sourceItem in folder.Children)
            {
                ParseSourceItem(sourceItem);
            }
        }

        private void ParseHeader(HeaderDefinition header)
        {
            _context.Header = header;

            var unsavedFiles = new UnsavedFile[] { };
            using (_context.TranslationUnit = _context.Index.CreateTranslationUnit(
                header.FullPath,
                GetClangOptions(),
                unsavedFiles,
                TranslationUnitFlags.SkipFunctionBodies))
            {
                var tuCursor = _context.TranslationUnit.Cursor;
                tuCursor.VisitChildren(HeaderVisitor);
                _context.TranslationUnit = null;
            }

            _context.Header = null;
        }

        private string[] GetClangOptions()
        {
            var options = new List<string> { "-x", "c++-header" };
            HeaderDefinition header = _context.Header;
            foreach (string includeFolder in GetItemIncludeFolders(header))
            {
                options.Add("-I");
                options.Add(includeFolder);
            }
            return options.ToArray();
        }

        private IEnumerable<string> GetItemIncludeFolders(SourceItemDefinition item)
        {
            if (item == null)
            {
                return new string[0];
            }

            return item.IncludeFolders.SelectMany(i =>
            {
                if (string.Equals(i, "$(Inherited)"))
                {
                    return GetItemIncludeFolders(item.Parent);
                }
                return new[] { Path.Combine(item.FullPath, i) };
            });
        }

        private Cursor.ChildVisitResult HeaderVisitor(Cursor cursor, Cursor parent)
        {
            string headerPath = cursor.Location.File.Name;
            if (IsExternalHeader(headerPath))
            {
                return Cursor.ChildVisitResult.Continue;
            }

            if (cursor.IsDefinition)
            {
                IParser parser;
                if (_context.DefinitionParsers.TryGetValue(cursor.Kind, out parser))
                {
                    parser.Parse(cursor, _context);
                    return Cursor.ChildVisitResult.Continue;
                }
            }

            if (cursor.IsDeclaration)
            {
                IParser parser;
                if (_context.DeclarationParsers.TryGetValue(cursor.Kind, out parser))
                {
                    parser.Parse(cursor, _context);
                    return Cursor.ChildVisitResult.Continue;
                }
            }

            if (cursor.Kind == CursorKind.CxxBaseSpecifier)
            {
                _context.Class.BaseClass = new TypeRefDefinition(cursor.Type);
            }

            return Cursor.ChildVisitResult.Continue;
        }

        private bool IsExternalHeader(string headerPath)
        {
            return !ArePathsEqual(headerPath, _context.Header.FullPath);
        }

        private bool ArePathsEqual(string path1, string path2)
        {
            return string.Compare(
                Path.GetFullPath(path1).TrimEnd('\\'),
                Path.GetFullPath(path2).TrimEnd('\\'),
                StringComparison.InvariantCultureIgnoreCase) == 0;
        }
    }
}
