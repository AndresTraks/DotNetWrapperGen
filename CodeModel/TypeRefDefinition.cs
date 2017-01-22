using System;

namespace DotNetWrapperGen.CodeModel
{
    public class TypeRefDefinition : ModelNodeDefinition
    {
        public bool IsBasic { get; set; }
        public bool IsPointer { get; set; }
        public bool IsReference { get; set; }
        public bool IsConstantArray { get; set; }
        public bool IsConst { get; set; }
        public TypeRefDefinition Referenced { get; set; }
        public bool HasTemplateTypeParameter { get; set; }
        public TypeRefDefinition SpecializedTemplateType { get; set; }

        public ClassDefinition Target { get; set; }

        public string FullName
        {
            get
            {
                if (Target != null)
                {
                    return Target.FullName;
                }
                return Name;
            }
        }

        public string ManagedName
        {
            get
            {
                if (IsBasic)
                {
                    if (Name.Equals("unsigned char"))
                    {
                        return "byte";
                    }
                    if (Name.Equals("unsigned short"))
                    {
                        return "ushort";
                    }
                    if (Name.Equals("unsigned int"))
                    {
                        return "uint";
                    }
                    if (Name.Equals("unsigned long"))
                    {
                        return "ulong";
                    }
                    if (Referenced != null)
                    {
                        return Referenced.ManagedName;
                    }
                    return Name;
                }
                if (HasTemplateTypeParameter)
                {
                    return "T";
                }
                if (IsPointer || IsReference || IsConstantArray)
                {
                    return Referenced.ManagedName;
                }
                if (Target == null)
                {
                    Console.WriteLine("Unresolved reference to " + Name);
                    return Name;
                }
                if (SpecializedTemplateType != null)
                {
                    return Target.ManagedName + "<" + SpecializedTemplateType.ManagedName + ">";
                }
                return Target.ManagedName;
            }
        }
        public string ManagedTypeRefName
        {
            get
            {
                if (IsPointer || IsReference || IsConstantArray)
                {
                    if (IsBasic)
                    {
                        return ManagedName + '*';
                    }
                    return ManagedName + '^';
                }
                return ManagedName;
            }
        }

        public TypeRefDefinition(ClangSharp.Type type)
            : base(GetName(type))
        {
            switch (type.TypeKind)
            {
                case ClangSharp.Type.Kind.Void:
                case ClangSharp.Type.Kind.Bool:
                case ClangSharp.Type.Kind.CharS:
                case ClangSharp.Type.Kind.Double:
                case ClangSharp.Type.Kind.Float:
                case ClangSharp.Type.Kind.Int:
                case ClangSharp.Type.Kind.UChar:
                case ClangSharp.Type.Kind.UInt:
                case ClangSharp.Type.Kind.WChar:
                case ClangSharp.Type.Kind.SChar:
                case ClangSharp.Type.Kind.Long:
                case ClangSharp.Type.Kind.LongLong:
                case ClangSharp.Type.Kind.Short:
                case ClangSharp.Type.Kind.ULong:
                case ClangSharp.Type.Kind.ULongLong:
                case ClangSharp.Type.Kind.UShort:
                case ClangSharp.Type.Kind.Enum:
                    IsBasic = true;
                    break;
                case ClangSharp.Type.Kind.Typedef:
                    Referenced = new TypeRefDefinition(type.Canonical);
                    IsBasic = Referenced.IsBasic;
                    break;
                case ClangSharp.Type.Kind.Pointer:
                    Referenced = new TypeRefDefinition(type.Pointee);
                    IsPointer = true;
                    break;
                case ClangSharp.Type.Kind.LValueReference:
                    Referenced = new TypeRefDefinition(type.Pointee);
                    IsReference = true;
                    break;
                case ClangSharp.Type.Kind.ConstantArray:
                    Referenced = new TypeRefDefinition(type.ArrayElementType);
                    IsConstantArray = true;
                    break;
                case ClangSharp.Type.Kind.FunctionProto:
                case ClangSharp.Type.Kind.Record:
                case ClangSharp.Type.Kind.Unexposed:
                case ClangSharp.Type.Kind.DependentSizedArray:
                case ClangSharp.Type.Kind.IncompleteArray:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static string GetName(ClangSharp.Type type)
        {
            switch (type.TypeKind)
            {
                case ClangSharp.Type.Kind.Void:
                case ClangSharp.Type.Kind.Bool:
                case ClangSharp.Type.Kind.CharS:
                case ClangSharp.Type.Kind.Double:
                case ClangSharp.Type.Kind.Float:
                case ClangSharp.Type.Kind.Int:
                case ClangSharp.Type.Kind.UChar:
                case ClangSharp.Type.Kind.UInt:
                case ClangSharp.Type.Kind.WChar:
                    return type.Spelling;
                case ClangSharp.Type.Kind.SChar:
                    return "char";
                case ClangSharp.Type.Kind.Long:
                    return "long";
                case ClangSharp.Type.Kind.LongLong:
                    return "long long";
                case ClangSharp.Type.Kind.Short:
                    return "short";
                case ClangSharp.Type.Kind.ULong:
                    return "unsigned long";
                case ClangSharp.Type.Kind.ULongLong:
                    return "unsigned long long";
                case ClangSharp.Type.Kind.UShort:
                    return "unsigned short";
                case ClangSharp.Type.Kind.Typedef:
                    return type.Declaration.Spelling;
                case ClangSharp.Type.Kind.Enum:
                    return type.Canonical.Declaration.Spelling;
                case ClangSharp.Type.Kind.Record:
                    return type.Canonical.Declaration.Spelling;
                case ClangSharp.Type.Kind.Unexposed:
                    if (type.Canonical.Declaration.IsInvalid)
                    {
                        return "[unexposed type]";
                    }
                    else
                    {
                        return type.Canonical.Declaration.Spelling;
                    }
                case ClangSharp.Type.Kind.Pointer:
                case ClangSharp.Type.Kind.LValueReference:
                case ClangSharp.Type.Kind.FunctionProto:
                case ClangSharp.Type.Kind.DependentSizedArray:
                case ClangSharp.Type.Kind.ConstantArray:
                case ClangSharp.Type.Kind.IncompleteArray:
                    return null;
                default:
                    throw new NotImplementedException();
            }
        }

        public TypeRefDefinition(string name)
            : base(name)
        {
        }

        public TypeRefDefinition()
            : base("void")
        {
            IsBasic = true;
        }

        public override bool Equals(object obj)
        {
            TypeRefDefinition t = obj as TypeRefDefinition;
            if (t == null)
            {
                return false;
            }

            if (t.IsBasic != IsBasic ||
                t.IsConstantArray != IsConstantArray ||
                t.IsPointer != IsPointer ||
                t.IsReference != IsReference)
            {
                return false;
            }

            if (IsPointer || IsReference || IsConstantArray)
            {
                return t.Referenced.Equals(Referenced);
            }

            return string.Equals(t.Name, Name, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return ManagedName;
        }

        public override void AddChild(ModelNodeDefinition child)
        {
            throw new NotSupportedException("Typeref cannot have children");
        }

        public override object Clone()
        {
            return new TypeRefDefinition(Name);
        }
    }
}
