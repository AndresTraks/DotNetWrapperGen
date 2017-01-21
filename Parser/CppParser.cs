using ClangSharp;
using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System.Collections.Generic;
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
            string[] clangOptions = new[] {
                "-x", "c++-header",
                "-I", "." };

            using (_context.TranslationUnit = _context.Index.CreateTranslationUnit(
                header.FullPath,
                clangOptions,
                unsavedFiles,
                TranslationUnitFlags.SkipFunctionBodies))
            {
                var tuCursor = _context.TranslationUnit.Cursor;
                tuCursor.VisitChildren(HeaderVisitor);
                _context.TranslationUnit = null;
            }

            _context.Header = null;
        }

        private Cursor.ChildVisitResult HeaderVisitor(Cursor cursor, Cursor parent)
        {
            string headerPath = cursor.Extent.Start.File.Name;
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
                        ParseMethod(cursor);
                        break;
                }
            }

            return Cursor.ChildVisitResult.Continue;
        }

        private bool IsExternalHeader(string headerPath)
        {
            return !headerPath.StartsWith(_context.RootFolder.FullPath);
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
            ModelNodeDefinition parent = GetCurrentParent();

            if (parent.Children.Any(c => c.Name == className))
            {
                return;
            }

            _context.Class = new ClassDefinition(className, parent);

            cursor.VisitChildren(HeaderVisitor);

            _context.Class = parent as ClassDefinition;
        }

        private void ParseTypedefCursor(Cursor cursor)
        {
            var underlying = cursor.TypedefDeclUnderlyingType.Canonical;

            //throw new NotImplementedException();
        }

        private void ParseMethod(Cursor cursor)
        {
            string methodName = cursor.Spelling;
            ModelNodeDefinition parent = GetCurrentParent();

            var parameters = new ParameterDefinition[cursor.NumArguments];
            for (uint i = 0; i < cursor.NumArguments; i++)
            {
                Cursor parameterCursor = cursor.GetArgument(i);
                parameters[i] = ParseParameter(parameterCursor);
            }

            _context.Method = new MethodDefinition(methodName, parent, parameters)
            {
                IsConstructor = cursor.Kind == CursorKind.Constructor,
                IsStatic = cursor.IsStaticCxxMethod
            };

            IList<Token> tokens = _context.TranslationUnit.Tokenize(cursor.Extent).ToList();
            if (tokens.Count > 3)
            {
                if (tokens[tokens.Count - 3].Spelling.Equals("=") &&
                    tokens[tokens.Count - 2].Spelling.Equals("0") &&
                    tokens[tokens.Count - 1].Spelling.Equals(";"))
                {
                    _context.Method.IsAbstract = true;
                }
            }

            _context.Method = null;
        }

        private ParameterDefinition ParseParameter(Cursor cursor)
        {
            string parameterName = cursor.Spelling;

            IEnumerable<Token> tokens = _context.TranslationUnit.Tokenize(cursor.Extent);
            bool isOptional = tokens.Any(t => t.Spelling == "=");

            return new ParameterDefinition(parameterName, null, isOptional);
        }

        private ModelNodeDefinition GetCurrentParent()
        {
            return _context.Class != null
                ? _context.Class as ModelNodeDefinition
                : _context.Namespace as ModelNodeDefinition;
        }
    }
}
