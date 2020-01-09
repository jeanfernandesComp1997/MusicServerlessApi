using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ReleaseMusicServerlessApi.Helpers
{
    public static class CryptHelper
    {
        #region Atributos

        private static string encryptionKey = Credentials.encryptionKey;

        #endregion

        #region Private methods

        private static string Encrypt(string text)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }

        private static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #endregion

        public static string ToCrypt(this string value)
        {
            return Encrypt(value);
        }

        public static string ToDecrypt(this string value)
        {
            return Decrypt(value);
        }

        public static DateTime BrazilEast(this DateTime now)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.Id == "E. South America Standard Time" || x.Id == "America/Sao_Paulo");
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone.Id));
        }
    }
}
