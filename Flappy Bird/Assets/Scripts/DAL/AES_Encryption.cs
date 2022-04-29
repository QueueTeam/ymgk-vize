using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
namespace Assets.Scripts.DAL
{
    public class AES_Encryption
    {
        static string enc = "";
        static string dec = "";
        public static StringBuilder pas = new StringBuilder();

        public static string Encrypt_Click(string text, string pas, byte[] IV)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            //Encrypt
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            crypt.BlockSize = 128;
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(pas));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                }

                enc = Convert.ToBase64String(memoryStream.ToArray());
                Console.WriteLine(enc);
                return enc;
            }
        }

        public static string Decrypt_Click(string text, string pas, byte[] IV)
        {
            //Decrypt
            byte[] bytes = Convert.FromBase64String(text);
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(pas));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] decryptedBytes = new byte[bytes.Length];
                    cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                    dec = Encoding.Unicode.GetString(decryptedBytes);
                    Console.WriteLine(dec);

                    return dec;
                }
            }
        }
    }
}
