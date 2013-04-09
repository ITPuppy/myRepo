using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Chatter.MetroClient
{
    class SecurityUtil
    {

        #region 获取由SHA1加密的字符串
        internal static string EncryptToSHA1(string str)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(str);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }
        #endregion


        internal static bool IsPasswordLegal(string pwd)
        {
            string pattern = @"^[\w\.-]{6,16}$";
            return Regex.IsMatch(pwd, pattern);
        }


        internal static byte[] Encrypt(byte[] sourcebytes, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = sourcebytes;
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();
            CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            crypStream.Write(inputByteArray, 0, inputByteArray.Length);
            crypStream.FlushFinalBlock();
            return memStream.ToArray();
        }

        internal static byte[] Decrypt(byte[] encryptBytes, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = encryptBytes;
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();
            CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            crypStream.Write(inputByteArray, 0, inputByteArray.Length);
            crypStream.FlushFinalBlock();
            return memStream.ToArray();
        }
    }
}
