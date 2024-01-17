using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Amn.Utilities
{
    /// <summary>
    /// رمزنگاری داده های مهم با الگوریتم AES
    /// </summary>
    public static class EncryptionExtension
    {
        /// <summary>
        /// رمزنگاری داده ها با الگوریتم AES
        /// </summary>
        public static string EncryptString(this string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText))
                return null;

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                aes.Key = keyBytes.Take(aes.KeySize / 8).ToArray();
                aes.IV = keyBytes.Take(aes.BlockSize / 8).ToArray();
                string IvString = Convert.ToBase64String(iv);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        /// <summary>
        /// رمزگشایی داده ها با الگوریتم AES
        /// </summary>
        public static string DecryptString(this string cipherText, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText))
                    return null;

                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;

                    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                    aes.Key = keyBytes.Take(aes.KeySize / 8).ToArray();
                    aes.IV = keyBytes.Take(aes.BlockSize / 8).ToArray();
                    string IvString = Convert.ToBase64String(iv);


                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return cipherText;
            }
        }
    }


}
