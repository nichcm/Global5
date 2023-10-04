using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Global5.Domain.Extensions
{
    public static class CryptoExtensions
    {
        private static string CalculatePass(string pass)
        {
            string calculatePass = "";

            if (!string.IsNullOrWhiteSpace(pass))
            {
                if (pass.Trim().Length < 4)
                {
                    pass = StringExtensions.AddZeroToLeft(pass, 4);
                }
                calculatePass = "04" + pass.Substring(0, 4) + "FFFFFFFFFF";
            }
            return calculatePass;
        }
        private static string CalculateUser(string user)
        {
            string calculateUser = "";

            if (!string.IsNullOrWhiteSpace(user))
            {
                if (user.Trim().Length < 15)
                {
                    user = StringExtensions.AddZeroToLeft(user, 15);
                }
                calculateUser = "0000" + user.Substring(3, 12);
            }
            return calculateUser;

        }
        private static byte[] CalculateXOR(string strC1, string strC2)
        {
            byte[] arrC1 = StrToByteArr(strC1);
            byte[] arrC2 = StrToByteArr(strC2);
            byte[] retorno = new byte[arrC1.Length];
            for (int i = 0; i < arrC1.Length; ++i)
                retorno[i] = (byte)(arrC1[i] ^ arrC2[i]);
            return retorno;
        }

        private static byte[] CalculateTDES(byte[] arrXOR, string key)
        {
            byte[] b1;
            byte[] b2;
            using (var TDES = new TripleDESCryptoServiceProvider())
            {
                TDES.Mode = CipherMode.ECB;
                TDES.Padding = PaddingMode.None;
                TDES.Key = StrHexToByteArr(key);
                TDES.GenerateIV();
                using var memoryStream = new MemoryStream();
                b1 = memoryStream.ToArray();
                using (var cryptoStream = new CryptoStream(memoryStream, TDES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(arrXOR, 0, arrXOR.Length);
                }
                b2 = memoryStream.ToArray();
            }
            return b2;
        }
        private static byte[] StrToByteArr(string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }
        private static string ByteArrToStrHex(byte[] arr)
        {
            var sb = new StringBuilder(arr.Length * 2);
            foreach (byte b in arr)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
        private static byte[] StrHexToByteArr(string str)
        {
            int len = str.Length / 2;
            byte[] retorno = new byte[len];
            for (int i = 0; i < len; ++i)
                retorno[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            return retorno;
        }
        public static string GeneratePinblock(string user, string pass, string key)
        {
            string strHexTDES = "";

            if (!string.IsNullOrWhiteSpace(pass))
            {
                string strC1 = CalculatePass(pass);
                string strC2 = CalculateUser(user);

                //Calcula xor
                byte[] arrXOR = CalculateXOR(strC1, strC2);

                //Calcula tdes
                byte[] arrTDES = CalculateTDES(arrXOR, key);
                strHexTDES = ByteArrToStrHex(arrTDES);
            }
            return strHexTDES;
        }
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new();
            Random random = new();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new();
            return random.Next(min, max);
        }

        public static string Encrypt(string message)
        {
            string salt = "AAAAB3NzaC1yc2EAAAABIwAAAQEAklOUpkDHrfHY17SbrmTIpNLTGK9Tjom/BWDSUGPl + nafzlHDTYW7hdI4yZ5ew18JH4JW9jbhUFrviQzM7xlELEVf4h9lFX5QVkbPppSwg0cda3Pbv7kOdJ / MTyBlWXFCR + HAo3FXRitBqxiX1nKhXpHAZsMciLq8V6RjsNAQwdsdMFvSlVK / 7XAt3FaoJoAsncM1Q9x5 + 3V0Ww68 / eIFmb1zuUFljQJKprrX88XypNDvjYNby6vw / Pb0rwert / EnmZ + AW4OZPnTPI89ZPmVMLuayrD2cE86Z / il8b + gw3r3 + 1nKatmIkjn2so1d01QraTlMqVSsbxNrRFi9wrf + M7Q ==";

            byte[] utfData = UTF8Encoding.UTF8.GetBytes(message);

            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            string encryptedString = string.Empty;

            using (AesManaged aes = new())
            {

                Rfc2898DeriveBytes rfc = new(salt, saltBytes);

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;

                aes.KeySize = aes.LegalKeySizes[0].MaxSize;

                aes.Key = rfc.GetBytes(aes.KeySize / 8);

                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                using ICryptoTransform encryptTransform = aes.CreateEncryptor();
                using MemoryStream encryptedStream = new();
                using CryptoStream encryptor = new(encryptedStream, encryptTransform, CryptoStreamMode.Write);

                encryptor.Write(utfData, 0, utfData.Length);

                encryptor.Flush();

                encryptor.Close();

                byte[] encryptBytes = encryptedStream.ToArray();

                encryptedString = Convert.ToBase64String(encryptBytes);
            }
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encryptedString);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Decrypt(string message)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(message);
            message = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            string salt = "AAAAB3NzaC1yc2EAAAABIwAAAQEAklOUpkDHrfHY17SbrmTIpNLTGK9Tjom/BWDSUGPl + nafzlHDTYW7hdI4yZ5ew18JH4JW9jbhUFrviQzM7xlELEVf4h9lFX5QVkbPppSwg0cda3Pbv7kOdJ / MTyBlWXFCR + HAo3FXRitBqxiX1nKhXpHAZsMciLq8V6RjsNAQwdsdMFvSlVK / 7XAt3FaoJoAsncM1Q9x5 + 3V0Ww68 / eIFmb1zuUFljQJKprrX88XypNDvjYNby6vw / Pb0rwert / EnmZ + AW4OZPnTPI89ZPmVMLuayrD2cE86Z / il8b + gw3r3 + 1nKatmIkjn2so1d01QraTlMqVSsbxNrRFi9wrf + M7Q ==";

            // string se = WebUtility.HtmlDecode();
            byte[] encryptedBytes = Convert.FromBase64String(message.Replace(" ", "+"));
            byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(salt);
            string decryptedString = string.Empty;

            using (var aes = new AesManaged())
            {

                Rfc2898DeriveBytes rfc = new(salt, saltBytes);

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;

                aes.KeySize = aes.LegalKeySizes[0].MaxSize;

                aes.Key = rfc.GetBytes(aes.KeySize / 8);

                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                using ICryptoTransform decryptTransform = aes.CreateDecryptor();

                using MemoryStream decryptedStream = new();

                CryptoStream decryptor = new(decryptedStream, decryptTransform, CryptoStreamMode.Write);

                decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);

                decryptor.Flush();

                decryptor.Close();

                byte[] decryptBytes = decryptedStream.ToArray();

                decryptedString = UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
            }
            return decryptedString;
        }
    }
}