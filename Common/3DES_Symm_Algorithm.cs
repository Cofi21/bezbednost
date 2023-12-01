using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Common.Manager;

namespace SymmetricAlgorithms
{
    public class TripleDES_Symm_Algorithm
    {
        public static byte[] EncryptMessage(byte[] message, string secretKey, CipherMode mode)
        {
            TripleDESCryptoServiceProvider tripleDesCryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = mode,
                Padding = PaddingMode.PKCS7 // Padding mode for messages
            };

            ICryptoTransform tripleDesEncryptTransform = tripleDesCryptoProvider.CreateEncryptor();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesEncryptTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(message, 0, message.Length);
                    cryptoStream.FlushFinalBlock();
                }

                return memoryStream.ToArray(); // Encrypted message
            }
        }
        public static byte[] DecryptMessage(byte[] encryptedMessage, string secretKey, CipherMode mode)
        {
            TripleDESCryptoServiceProvider tripleDesCryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = mode,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform tripleDesDecryptTransform = tripleDesCryptoProvider.CreateDecryptor();

            using (MemoryStream memoryStream = new MemoryStream(encryptedMessage))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesDecryptTransform, CryptoStreamMode.Read))
                {
                    using (MemoryStream decryptedStream = new MemoryStream())
                    {
                        cryptoStream.CopyTo(decryptedStream);
                        return decryptedStream.ToArray(); // Decrypted message
                    }
                }
            }
        }

    }
}
