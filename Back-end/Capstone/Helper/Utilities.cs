using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Helper
{
    public static class Utilities
    {
        public static string RandomString(int range)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

            var randomIns = new Random();
            var rndChars = Enumerable.Range(0, range) //Tạo 1 chuỗi ký từ có độ dài là range
                            .Select(c => chars[randomIns.Next(chars.Length)]) //Lấy random từ chuỗi chars
                            .ToArray();

            var randomstr = "";

            foreach (var item in rndChars)
            {
                randomstr += item.ToString();
            }
            return randomstr;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
