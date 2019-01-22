using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Ft.ImageServer.Core.Utils
{
    public static class StringExtensions
    {
        private static string urlPattern = @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";
        private static Regex urlRegex = new Regex(urlPattern);

        public static byte[] ToMD5Hash(this string @string)
        {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(@string));
        }

        public static string ToMD5HashString(this string @string)
        {
            var stringBuilder = new StringBuilder();
            foreach (byte b in ToMD5Hash(@string))
                stringBuilder.Append(b.ToString("X2"));

            return stringBuilder.ToString();
        }

        public static bool IsUrl(this string @this)
        {
            return urlRegex.IsMatch(@this);
        }
    }
}
