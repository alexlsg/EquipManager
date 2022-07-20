using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Tools
{
    /// <summary>
    /// 加密类
    /// </summary>
    public class DES
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string data)
        {
            string KEY_64 = "12345678";
            string IV_64 = "12345678";//64位的键值和IV值要为8位
            try
            {
                byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
                byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                int i = cryptoProvider.KeySize;
                MemoryStream ms = new MemoryStream();
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cst);
                sw.Write(data);
                sw.Flush();
                cst.FlushFinalBlock();
                sw.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }

            catch (Exception x)

            {

                return x.Message;

            }

        }
        /// <summary>
        /// MD5解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string MD5Dencrypt(string data)
        {
            string KEY_64 = "12345678";
            string IV_64 = "12345678";//64位的键值和IV值要为8位
            try
            {
                byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
                byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
                byte[] byEnc;
                byEnc = Convert.FromBase64String(data); //把需要解密的字符串转为8位无符号数组
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream(byEnc);
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cst);
                return sr.ReadToEnd();
            }
            catch (Exception x)
            {
                return x.Message;
            }
        }
    }
}
