using System.Security.Cryptography;
using System.Text;

namespace MAC
{
    public static class ByteArrayExtensions
    {
        public static byte[] ToMD5(this byte[] inputBytes)
        {
            var md5 = MD5.Create();
            return md5.ComputeHash(inputBytes);
        }

        public static string ToString(this byte[] inputBytes)
        {
            return Encoding.UTF8.GetString(inputBytes);
        }
    }
}
