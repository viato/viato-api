using System;
using System.Text;

namespace Viato.Api.Tor
{
    public static class Extensions
    {
        public static string ToHex(this byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        public static byte[] HexToByteArray(this string hex)
        {
            hex = Remove0x(hex);
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static byte[] Combine(byte[] a1, byte[] a2, byte[] a3)
        {
            byte[] ret = new byte[a1.Length + a2.Length + a3.Length];
            Array.Copy(a1, 0, ret, 0, a1.Length);
            Array.Copy(a2, 0, ret, a1.Length, a2.Length);
            Array.Copy(a3, 0, ret, a1.Length + a2.Length, a3.Length);
            return ret;
        }

        public static byte[] GetBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string Remove0x(this string hex)
        {
            return hex.Replace("0x", string.Empty);
        }

        public static string Add0x(this string hex)
        {
            return $"0x{Remove0x(hex)}";
        }
    }
}
