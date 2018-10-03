using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Ecard
{ 
    public static class StringExtensions
    {
        private static readonly Regex cleanWhitespace = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.Singleline);

        public static string FormatForJavascript(this string s)
        {
            return s;
        }

        public static string CleanHtmlTags(this string s)
        {
            Regex exp = new Regex(
                "<[^<>]*>",
                RegexOptions.Compiled
                );

            return exp.Replace(s, "");
        }
         

        public static string CleanWhitespace(this string s)
        {
            return cleanWhitespace.Replace(s, " ");
        }

        public static string ComputeHash(this string value)
        {
            string hash = value;

            if (value != null)
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] data = Encoding.ASCII.GetBytes(value);
                data = md5.ComputeHash(data);
                hash = "";
                for (int i = 0; i < data.Length; i++)
                {
                    hash += data[i].ToString("x2");
                }
            }

            return hash;
        }
    }
}