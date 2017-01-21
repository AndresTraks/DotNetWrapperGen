namespace DotNetWrapperGen.Parser
{
    public class ConventionConverter
    {
        public static string Convert(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            value = RemoveVersionNumber(value);
            value = RemoveUnderscore(value);
            value = value.Substring(0, 1).ToUpper() + value.Substring(1);
            return value;
        }

        public static string RemoveUnderscore(string value)
        {
            while (value.Contains("_"))
            {
                int pos = value.IndexOf('_');
                if (pos + 1 == value.Length)
                {
                    value = value.Substring(0, pos);
                }
                else
                {
                    value = value.Substring(0, pos) + value.Substring(pos + 1, 1).ToUpper() + value.Substring(pos + 2);
                }
            }
            return value;
        }

        public static string RemoveVersionNumber(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            char lastChar = value[value.Length - 1];
            while (lastChar.Equals('.') || (lastChar >= '0' && lastChar <= '9'))
            {
                value = value.Substring(0, value.Length - 1);
                lastChar = value[value.Length - 1];
            }
            if (lastChar.Equals('-'))
            {
                value = value.Substring(0, value.Length - 1);
            }
            return value;
        }
    }
}
