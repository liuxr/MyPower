/* 
   ======================================================================== 
    文 件 名：     
	功能描述：                
    作    者:	Cumin           
    创建时间： 	2016/11/2 15:23:34
	版    本:	V1.0.0
   ------------------------------------------------------------------------
	历史更新纪录
   ------------------------------------------------------------------------
	版    本：           修改时间：           修改人：          
	修改内容：
   ------------------------------------------------------------------------
	Copyright (C) 2016   北京荣大科技有限公司
   ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MyPay
{
    public class Crypt3Des
    {
        private string key = string.Empty;
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
            }
        }
        public Crypt3Des(string key) {
            this.key = key;
        }
        public string Encrypt(string input)
        {
            try
            {
                byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };      //当模式为ECB时，IV无用
                byte[] data = Encoding.UTF8.GetBytes(input);

                string k = key.PadRight(24, '0');

                string encode = Convert.ToBase64String(Encoding.UTF8.GetBytes(k));
                byte[] keytemp = Convert.FromBase64String(encode);
                byte[] key1 = new byte[24];
                for (int i = 0; i < 24; i++)
                {
                    key1[i] = keytemp[i];
                }
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();

                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key1, iv),
                    CryptoStreamMode.Write);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                byte[] ret = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                // Return the encrypted buffer.
                return UrlSafe(Convert.ToBase64String(ret));
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return "";
            }
        }

        private string UrlSafe(string data)
        {
            return data.Replace('+', '-').Replace('/', '_').Replace('=', char.MinValue);
        }


        /// <summary>
        /// 一种补充算法
        /// </summary>
        /// <param name="text"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        public string Pkcs5_pad(string text, int blockSize) {
            int pad = blockSize - (text.Length % blockSize);
            byte[] array = new byte[1];
            array[0] = (byte)(pad);
            string ch = Convert.ToString(System.Text.Encoding.ASCII.GetString(array));
          
             return text + Repeat(ch, pad);
        }

        /// <summary>
        /// 字符串重复次数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public string Repeat(string str, int multiplier) {
            string result=string.Empty;
            for (int i = 0; i < multiplier; i++) {
                result = str;
            }
            return result;
        }
        /// <summary>
        /// DES3 ECB模式加密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>
        /// <param name="str">明文的byte数组</param>
        /// <returns>密文的byte数组</returns>
        public static byte[] Des3EncodeECB(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();

                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                byte[] ret = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                // Return the encrypted buffer.
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }

        }


      
    }
}
