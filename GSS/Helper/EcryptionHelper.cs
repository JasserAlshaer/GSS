using System.Text;
using System.Security.Cryptography;
namespace GSS.Helper
{
    public static class EncryptionHelper
    {
        public static string Sha384(string data)
        {
            using (var sha384 = new SHA384Managed())
            {
                byte[] hash = sha384.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
