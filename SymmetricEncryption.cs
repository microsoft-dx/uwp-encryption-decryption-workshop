using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace EncryptionDecryption
{
    /// <summary>
    /// Provides symmetric encryption and decryption services
    /// 
    /// Original source: https://goo.gl/arhInA
    /// </summary>
    public class SymmetricEncryption
    {
        private readonly IBuffer randomBuffer;
        private readonly IBuffer randomBufferCBC;
        private readonly CryptographicKey cryptographicKey;

        private readonly string algorithmName;
        private readonly SymmetricKeyAlgorithmProvider cryptingProvider;

        /// <summary>
        /// Instantiate with a random generated buffer (not an option if
        /// you want to persist the encryption to disk)
        /// </summary>
        public SymmetricEncryption()
        {
            algorithmName = SymmetricAlgorithmNames.AesEcbPkcs7;
            cryptingProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(algorithmName);

            randomBuffer = CryptographicBuffer.GenerateRandom(cryptingProvider.BlockLength);
            randomBufferCBC = null;

            cryptographicKey = cryptingProvider.CreateSymmetricKey(randomBuffer);
        }
        
        /// <summary>
        /// Instantiate with a custom generated buffer (good for
        /// persisting the encryption to disk)
        /// </summary>
        /// <param name="randomBuffer">The custom generated buffer</param>
        public SymmetricEncryption(IBuffer randomBuffer)
            : this()
        {
            this.randomBuffer = randomBuffer;

            cryptographicKey = cryptingProvider.CreateSymmetricKey(randomBuffer);
        }

        /// <summary>
        /// Instantiate with a custom generated buffer (good for
        /// persisting the encryption to disk) and with a custom
        /// generated CBC buffer (is using CBC algorithms)
        /// </summary>
        /// <param name="randomBuffer">The custom generated buffer</param>
        /// <param name="randomBufferCBC">The custom generated CBC buffer</param>
        public SymmetricEncryption(IBuffer randomBuffer, IBuffer randomBufferCBC)
            : this(randomBuffer)
        {
            this.randomBufferCBC = randomBufferCBC;
        }

        private bool IsMultipleOfBlockLength(IBuffer binaryData)
        {
            return (binaryData.Length % cryptingProvider.BlockLength) != 0;
        }

        /// <summary>
        /// Encrypts a given string
        /// </summary>
        /// <param name="data">Data to be encrypted</param>
        /// <returns>An encrypted string in Unicode</returns>
        public string Encrypt(string data)
        {
            var binaryData = Encoding.Unicode.GetBytes(data).AsBuffer();

            if (!algorithmName.Contains("PKCS7") && IsMultipleOfBlockLength(binaryData))
                throw new Exception("Message buffer length must be multiple of block length !!");

            var encryptedBinaryData = CryptographicEngine.Encrypt(cryptographicKey, binaryData, randomBufferCBC);

            return Encoding.Unicode.GetString(encryptedBinaryData.ToArray());
        }

        /// <summary>
        /// Decrypts a string in Unicode
        /// </summary>
        /// <param name="encryptedData">An encrypted string in Unicode</param>
        /// <returns>The decrypted string in Unicode</returns>
        public string Decrypt(string encryptedData)
        {
            var encryptedBinaryData = Encoding.Unicode.GetBytes(encryptedData).AsBuffer();

            var decryptedData = CryptographicEngine.Decrypt(cryptographicKey, encryptedBinaryData, randomBufferCBC);

            return Encoding.Unicode.GetString(decryptedData.ToArray());
        }
    }
}
