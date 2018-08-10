﻿using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ZipUploader.Common.Helpers
{
    /// <summary>
    /// String encryption.
    /// </summary>
    public static class StringCipher
    {
        private const int BlockSize = 128;
        private const int DerivationIterations = 1000;
        private const int KeySize = 128;
        private const CipherMode Mode = CipherMode.ECB;
        private const PaddingMode Padding = PaddingMode.PKCS7;

        public static string Encrypt(string plainText, string passPhrase)
        {
            var saltStringBytes = Generate128BitsOfRandomEntropy();
            var ivStringBytes = Generate128BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(KeySize / 8);

                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = BlockSize;
                    symmetricKey.Mode = Mode;
                    symmetricKey.Padding = Padding;

                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();

                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();

                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);

            var saltStringBytes = cipherTextBytesWithSaltAndIv
                .Take(KeySize / 8)
                .ToArray();

            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(KeySize / 8)
                .Take(KeySize / 8)
                .ToArray();

            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((KeySize / 8) * 2)
                .Take(cipherTextBytesWithSaltAndIv.Length - ((KeySize / 8) * 2))
                .ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(KeySize / 8);

                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = BlockSize;
                    symmetricKey.Mode = Mode;
                    symmetricKey.Padding = Padding;

                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                                memoryStream.Close();
                                cryptoStream.Close();

                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate128BitsOfRandomEntropy()
        {
            var randomBytes = new byte[16];

            using (var rngCsp = new RNGCryptoServiceProvider())
                rngCsp.GetBytes(randomBytes);

            return randomBytes;
        }
    }
}
