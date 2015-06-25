using System;
using System.IO;
using System.Security.Cryptography;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public class CryptoUtils
    {
        private readonly byte[] _key = { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
        private readonly byte[] _vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 251, 112, 79, 32, 114, 156 };
        #region Private functions
        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] vector)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (vector == null || vector.Length <= 0)
                throw new ArgumentNullException("key");
            byte[] encrypted;
            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = vector;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }
        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] key, byte[] vector)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (vector == null || vector.Length <= 0)
                throw new ArgumentNullException("key");

            // Declare the string used to hold the decrypted text. 
            string plaintext = null;

            // Create an Aes object with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = vector;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }
        private string ByteArrToString(byte[] byteArr)
        {
            byte val;
            string tempStr = "";
            for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
            {
                val = byteArr[i];
                if (val < (byte)10)
                    tempStr += "00" + val.ToString();
                else if (val < (byte)100)
                    tempStr += "0" + val.ToString();
                else
                    tempStr += val.ToString();
            }
            return tempStr;
        }
        private byte[] StrToByteArray(string str)
        {
            if (str.Length == 0)
                throw new Exception("Invalid string value in StrToByteArray");

            byte val;
            byte[] byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            }
            while (i < str.Length);
            return byteArr;
        }
        #endregion
        public string EncryptToString(string plainText)
        {
            return String.IsNullOrEmpty(plainText) ? null : ByteArrToString(EncryptStringToBytes_Aes(plainText, _key, _vector));
        }
        public string DecryptFromString(string encryptedText)
        {
            try
            {
                var byteArray = StrToByteArray(encryptedText);
                return String.IsNullOrEmpty(encryptedText)
                           ? null
                           : DecryptStringFromBytes_Aes(byteArray, _key, _vector);
            }
            catch (Exception ex)
            {
                LogWriter.Instance.WriteToLog(String.Format(@"Decrypt of ""{0}"" failed: {1}",encryptedText,ex),true);
            }
            return null;
        }
    }
}
