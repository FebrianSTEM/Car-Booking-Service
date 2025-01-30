using System.Text.Json;
using System.Text;

namespace car_booking_service.Configuration
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

        public override string ConvertName(string name)
        {
            // Conversion to other naming convention goes here. Like SnakeCase, KebabCase etc.
            return name.ToSnakeCase();
        }
    }

    public static class StringUtils
    {
        public static string ToSnakeCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            s = s.Trim();

            var length = s.Length;
            var addedByLower = false;
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var currentChar = s[i];

                if (char.IsWhiteSpace(currentChar))
                {
                    continue;
                }

                bool isLastChar = i + 1 == length,
                        isFirstChar = i == 0,
                        nextIsUpper = false,
                        nextIsLower = false;

                if (isFirstChar && s[i] == '_')
                {
                    continue;
                }

                if (!isLastChar)
                {
                    nextIsUpper = char.IsUpper(s[i + 1]);
                    nextIsLower = !nextIsUpper && !s[i + 1].Equals('_');
                }

                if (!char.IsUpper(currentChar))
                {
                    stringBuilder.Append(char.ToLowerInvariant(currentChar));

                    if (nextIsUpper)
                    {
                        stringBuilder.Append('_');
                        addedByLower = true;
                    }

                    continue;
                }

                if (nextIsLower && !addedByLower && !isFirstChar)
                {
                    stringBuilder.Append('_');
                }

                addedByLower = false;

                stringBuilder.Append(char.ToLowerInvariant(currentChar));
            }

            return stringBuilder.ToString();
        }

    }
}
