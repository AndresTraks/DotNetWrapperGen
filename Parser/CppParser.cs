using ClangSharp;
using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
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
            _context = new CppParserContext(rootFolder);

            var globalNamespace = new NamespaceDefinition("");
            _context.Namespace = globalNamespace;

            using (_context.Index = new Index())
            {
                foreach (var sourceItem in rootFolder.Children)
                {
                    ParseSourceItem(sourceItem);
                }
                _context.Index = null;
            }

            return globalNamespace;
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
                switch (cursor.Kind)
                {
                    case CursorKind.Namespace:
                        ParseNamespace(cursor);
                        break;
                    case CursorKind.ClassDecl:
                    case CursorKind.ClassTemplate:
                    case CursorKind.EnumDecl:
                    case CursorKind.StructDecl:
                        ParseClass(cursor);
                        break;
                    case CursorKind.TypedefDecl:
                        ParseTypedefCursor(cursor);
                        break;

                }
            }

            if (cursor.IsDeclaration)
            {
                switch (cursor.Kind)
                {
                    case CursorKind.CxxMethod:
                    case CursorKind.Constructor:
                        MethodParser.Parse(cursor, _context);
                        break;
                    case CursorKind.FieldDecl:
                        FieldParser.Parse(cursor, _context);
                        break;
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

        private void ParseNamespace(Cursor cursor)
        {
            string namespaceName = cursor.Spelling;
            NamespaceDefinition @namespace = CreateOrGetNamespace(namespaceName);

            NamespaceDefinition previousNamespace = _context.Namespace;
            _context.Namespace = @namespace;
            cursor.VisitChildren(HeaderVisitor);
            _context.Namespace = previousNamespace;
        }

        private NamespaceDefinition CreateOrGetNamespace(string name)
        {
            var @namespace = _context.Namespace.Namespaces
                                     .FirstOrDefault(n => n.Name == name);
            if (@namespace == null)
            {
                @namespace = new NamespaceDefinition(name)
                {
                    Parent = _context.Namespace
                };
                _context.Namespace.Children.Add(@namespace);
            }
            return @namespace;
        }

        private void ParseClass(Cursor cursor)
        {
            string className = cursor.Spelling;
            ModelNodeDefinition parent = _context.GetCurrentParent();

            if (parent.Children.Any(c => c.Name == className))
            {
                return;
            }

            _context.Class = new ClassDefinition(className);
            parent.AddChild(_context.Class);
            if (parent is NamespaceDefinition)
            {
                _context.Header.AddNode(_context.Class);
            }

            cursor.VisitChildren(HeaderVisitor);

            _context.Class = parent as ClassDefinition;
        }

        private void ParseTypedefCursor(Cursor cursor)
        {
            var underlying = cursor.TypedefDeclUnderlyingType.Canonical;

            //throw new NotImplementedException();
        }
    }
}
