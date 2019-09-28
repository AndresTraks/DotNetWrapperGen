using DotNetWrapperGen.CodeModel;
using System;
using System.Globalization;
using System.Linq;

namespace DotNetWrapperGen.Utility
{
    public static class EnumExtensions
    {
        public static bool IsFlags(this EnumDefinition @enum)
        {
            if (@enum.Name.EndsWith("Flags"))
            {
                return true;
            }
            return @enum.Enumerators.All(IsFlag);
        }

        public static bool ParseValue(this EnumeratorDefinition enumerator, out int result)
        {
            string value = enumerator.Value;

            if (string.IsNullOrEmpty(value))
            {
                result = 0;
                return false;
            }

            if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return int.TryParse(value.Substring(2),
                    NumberStyles.HexNumber,
                    CultureInfo.CurrentCulture,
                    out result);
            }

            return int.TryParse(value, out result);
        }

        private static bool IsFlag(EnumeratorDefinition enumerator)
        {
            if (ParseValue(enumerator, out int intValue))
            {
                return intValue == 0 || IsPowerOfTwo(intValue);
            }
            return false;
        }

        private static bool IsPowerOfTwo(int x)
        {
            return (x != 0) && ((x & (~x + 1)) == x);
        }
    }
}
