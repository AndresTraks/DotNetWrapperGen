using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;
using System.Linq;

namespace DotNetWrapperGen.Transformer
{
    public sealed class EnumTransformer : ITransformer
    {
        public void Transform(NamespaceDefinition globalNamespace, RootFolderDefinition rootFolder)
        {
            Transform(globalNamespace);
        }

        private static void Transform(ModelNodeDefinition node)
        {
            EnumDefinition @enum = node as EnumDefinition;
            if (@enum != null)
            {
                TransformEnum(@enum);
                return;
            }

            foreach (ModelNodeDefinition child in node.Children)
            {
                Transform(child);
            }
        }

        private static void TransformEnum(EnumDefinition @enum)
        {
            EnumeratorDefinition[] enumerators = @enum.Enumerators.ToArray();
            string[][] names = GetNamesWithParts(enumerators);
            string[] newNames = CombineNameParts(names);
            ReplaceNameReferencesInValues(enumerators, newNames);
            ReplaceNames(enumerators, newNames);
        }

        private static string[][] GetNamesWithParts(EnumeratorDefinition[] enumerators)
        {
            return enumerators
                .Select(e => e.Name)
                .Select(GetNameParts)
                .ToArray();
        }

        private static string[] GetNameParts(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new string[0];
            }

            var parts = name.Split('_');

            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.Length > 0 && part.All(c => char.IsUpper(c) || char.IsDigit(c)))
                {
                    parts[i] = part[0] + part.Substring(1).ToLower();
                }
            }

            return parts;
        }

        private static string[] CombineNameParts(string[][] names)
        {
            int prefix = GetCommonPrefixPartIndex(names);

            string[] newNames = new string[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                string[] parts = names[i];
                newNames[i] = string.Join("", parts, prefix, parts.Length - prefix);
            }
            return newNames;
        }

        public static int GetCommonPrefixPartIndex(string[][] names)
        {
            if (names.Length <= 1)
            {
                return 0;
            }

            string[] firstName = names.First();
            int i = 0;
            while (true)
            {
                string firstPart = firstName[i];
                if (names.Any(parts => parts[i] != firstPart))
                {
                    break;
                }
                if (names.Any(parts => parts.Length == i))
                {
                    // Prefix is already one of the enumerators,
                    // can't use this prefix
                    return 0;
                }
                i++;
            }
            return i;
        }

        private static void ReplaceNameReferencesInValues(EnumeratorDefinition[] enumerators, string[] newNames)
        {
            foreach (EnumeratorDefinition enumerator in enumerators.Where(e => e.Value != null))
            {
                for (int i = 0; i < enumerators.Length; i++)
                {
                    EnumeratorDefinition oldEnumerator = enumerators[i];
                    string oldName = oldEnumerator.Name;
                    if (enumerator.Value.Contains(oldName))
                    {
                        enumerator.Value = enumerator.Value.Replace(oldName, newNames[i]);
                    }
                }
            }
        }

        private static void ReplaceNames(EnumeratorDefinition[] enumerators, string[] newNames)
        {
            for (int i = 0; i < newNames.Length; i++)
            {
                enumerators[i].Name = newNames[i];
            }
        }
    }
}
