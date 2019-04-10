using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Ft.ImageServer.Core.Utils
{
    /// <summary>
    /// <see cref="String"/>
    /// </summary>
    public static class StringExtensions
    {
        private static string urlPattern = @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";
        private static Regex urlRegex = new Regex(urlPattern);

        /// <summary>
        /// 计算Md5哈希值
        /// </summary>
        public static byte[] ToMD5Hash(this string @string)
        {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(@string));
        }

        /// <summary>
        /// 计算Md5哈希值
        /// </summary>
        public static string ToMD5HashString(this string @string)
        {
            var stringBuilder = new StringBuilder();
            foreach (byte b in ToMD5Hash(@string))
                stringBuilder.Append(b.ToString("X2"));

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 是否Url
        /// </summary>
        public static bool IsUrl(this string @this)
        {
            return urlRegex.IsMatch(@this);
        }
    }
}
