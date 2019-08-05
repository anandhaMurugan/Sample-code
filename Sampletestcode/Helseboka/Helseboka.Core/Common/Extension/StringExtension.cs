using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Helseboka.Core.Common.Extension
{
	public static class StringExtension
    {
		public static bool IsDigit(this String data)
		{
			if (data != null && data.Length > 0)
			{
				var regex = "^[0-9]*$";
				return Regex.IsMatch(data, regex);
			}
			else 
			{
				return false;
			}
		}

        public static string FirstCharacterToLower(this String str)
        {
            if (String.IsNullOrEmpty(str) || Char.IsLower(str, 0))
                return str;

            return Char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        public static String ToNameCase(this String name)
        {
            if (String.IsNullOrEmpty(name))
                return name;

            var nameParts = name.Split(' ');
            if (nameParts != null && nameParts.Length > 0)
            {
                List<String> result = new List<String>();
                foreach (var item in nameParts)
                {
                    var namePart = item.ToLower();
                    if (namePart.Length > 1)
                    {
                        namePart = Char.ToUpperInvariant(namePart[0]) + namePart.Substring(1);
                    }
                    result.Add(namePart);
                }

                return String.Join(" ", result).Trim();
            }
            else
            {
                return name;
            }
        }

        public static DateTime? ConvertTimeToDateTime(this String time)
        {
            if (DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out var frequency))
            {
                return frequency;
            }

            if (DateTime.TryParseExact(time, "HH.mm", CultureInfo.InvariantCulture,
                                       DateTimeStyles.None, out var freq))
            {
                return freq;
            }

            return null;
        }

        public static String ToURLEncodedString(this String data)
        {
            return System.Net.WebUtility.UrlEncode(data);
        }
    }
}
