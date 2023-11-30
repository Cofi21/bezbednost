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
        /// <summary>
        /// Function that encrypts the plaintext from inFile and stores cipher text to outFile
        /// </summary>
        /// <param name="inFile"> filepath where plaintext is stored </param>
        /// <param name="outFile"> filepath where cipher text is expected to be stored </param>
        /// <param name="secretKey"> symmetric encryption key </param>
        public static void EncryptFile(string inFile, string outFile, string secretKey, CipherMode mode)
        {
            byte[] header = null;   //image header (54 byte) should not be encrypted
            byte[] body = null;     //image body to be encrypted
            byte[] encryptedBody = null;

            Formatter.Decompose(File.ReadAllBytes(inFile), out header, out body);
            TripleDESCryptoServiceProvider tripleDesCryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = mode,
                Padding = PaddingMode.None
            };

            if (mode.Equals(CipherMode.ECB))
            {
                ICryptoTransform tripleDesEncryptTransform = tripleDesCryptoProvider.CreateEncryptor();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesEncryptTransform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(body, 0, body.Length);
                        encryptedBody = memoryStream.ToArray();                              //encrypted image body
                    }
                }
            }
            
            int outputLenght = header.Length + encryptedBody.Length;            //header.Length + body.Length
            Formatter.Compose(header, encryptedBody, outputLenght, outFile);

        }


        /// <summary>
        /// Function that decrypts the cipher text from inFile and stores as plaintext to outFile
        /// </summary>
        /// <param name="inFile"> filepath where cipher text is stored </param>
        /// <param name="outFile"> filepath where plain text is expected to be stored </param>
        /// <param name="secretKey"> symmetric encryption key </param>
        public static void DecryptFile(string inFile, string outFile, string secretKey, CipherMode mode)
        {
            byte[] header = null;       //image header (54 byte) should not be decrypted
            byte[] body = null;         //image body to be decrypted
            byte[] decryptedBody = null;

            Formatter.Decompose(File.ReadAllBytes(inFile), out header, out body);
            TripleDESCryptoServiceProvider tripleDesCryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = mode,
                Padding = PaddingMode.None
            };

            if (mode.Equals(CipherMode.ECB))
            {
                ICryptoTransform tripleDesDecryptTransform = tripleDesCryptoProvider.CreateDecryptor();
                using (MemoryStream memoryStream = new MemoryStream(body))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesDecryptTransform, CryptoStreamMode.Read))
                    {
                        decryptedBody = new byte[body.Length];       //decrypted image body - the same lenght as encrypted part
                        cryptoStream.Read(decryptedBody, 0, decryptedBody.Length);
                    }
                }
            }
            
            int outputLenght = header.Length + decryptedBody.Length;
            Formatter.Compose(header, decryptedBody, outputLenght, outFile);
        }
    }
}
