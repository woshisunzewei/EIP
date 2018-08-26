using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EIP.Common.Core.Utils
{
    /// <summary>
    /// 加解密工具类
    /// </summary>
    public static class DEncryptUtil
    {
        #region 使用 缺省密钥字符串 加密/解密string

        /// <summary>
        ///     使用缺省密钥字符串加密string(默认钥匙为:P@ssw0rd!@#)
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original)
        {
            return Encrypt(original, "P@ssw0rd!@#");
        }

        /// <summary>
        ///     使用缺省密钥字符串解密string(默认钥匙为:P@ssw0rd!@#)
        /// </summary>
        /// <param name="original">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original)
        {
            return Decrypt(original, "P@ssw0rd!@#", Encoding.Default);
        }

        #endregion

        #region 使用 给定密钥字符串 加密/解密string

        /// <summary>
        ///     使用给定密钥字符串加密string
        /// </summary>
        /// <param name="original">原始文字</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, 
            string key)
        {
            var buff = Encoding.Default.GetBytes(original);
            var kb = Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb)).Replace("+", "%2B");
        }

        /// <summary>
        ///     使用给定密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original,
            string key)
        {
            return Decrypt(original, key, Encoding.Default);
        }

        /// <summary>
        ///     使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encrypted,
            string key,
            Encoding encoding)
        {
            encrypted = encrypted.Replace("%2B", "+");
            var buff = Convert.FromBase64String(encrypted);
            var kb = Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }

        #endregion

        #region 使用 缺省密钥字符串 加密/解密/byte[]

        /// <summary>
        ///     使用缺省密钥字符串解密byte[]
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted)
        {
            var key = Encoding.Default.GetBytes("P@ssw0rd!@#");
            return Decrypt(encrypted, key);
        }

        /// <summary>
        ///     使用缺省密钥字符串加密
        /// </summary>
        /// <param name="original">原始数据</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original)
        {
            var key = Encoding.Default.GetBytes("P@ssw0rd!@#");
            return Encrypt(original, key);
        }

        #endregion

        #region  使用 给定密钥 加密/解密/byte[]

        /// <summary>
        ///     使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original,
            byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider { Key = MakeMd5(key), Mode = CipherMode.ECB };

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        /// <summary>
        ///     使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted,
            byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider { Key = MakeMd5(key), Mode = CipherMode.ECB };

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        #endregion

        #region Base64加密解密

        /// <summary>
        ///     Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input)
        {
            return Base64Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        ///     Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input,
            Encoding encode)
        {
            return Convert.ToBase64String(encode.GetBytes(input));
        }

        /// <summary>
        ///     Base64解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input)
        {
            return Base64Decrypt(input, new UTF8Encoding());
        }

        /// <summary>
        ///     Base64解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input,
            Encoding encode)
        {
            return encode.GetString(Convert.FromBase64String(input));
        }

        #endregion

        #region DES加密解密

        /// <summary>
        ///     DES加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <param name="key">8位字符的密钥字符串</param>
        /// <param name="iv">8位字符的初始化向量字符串</param>
        /// <returns></returns>
        public static string DesEncrypt(string data,
            string key,
            string iv)
        {
            var byKey = Encoding.ASCII.GetBytes(key);
            var byIv = Encoding.ASCII.GetBytes(iv);
            var cryptoProvider = new DESCryptoServiceProvider();
            var ms = new MemoryStream();
            var cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIv), CryptoStreamMode.Write);
            var sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        /// <summary>
        ///     DES解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="key">8位字符的密钥字符串(需要和加密时相同)</param>
        /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param>
        /// <returns></returns>
        public static string DesDecrypt(string data, 
            string key,
            string iv)
        {
            var byKey = Encoding.ASCII.GetBytes(key);
            var byIv = Encoding.ASCII.GetBytes(iv);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }
            var cryptoProvider = new DESCryptoServiceProvider();
            var ms = new MemoryStream(byEnc);
            var cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIv), CryptoStreamMode.Read);
            var sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }

        #endregion

        #region MD5加密

        /// <summary>
        ///     生成MD5摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>摘要</returns>
        public static byte[] MakeMd5(byte[] original)
        {
            var hashmd5 = new MD5CryptoServiceProvider();
            var keyhash = hashmd5.ComputeHash(original);
            return keyhash;
        }

        /// <summary>
        ///     MD5加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns></returns>
        public static string Md5Encrypt(string input)
        {
            return Md5Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        ///     MD5加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Md5Encrypt(string input,
            Encoding encode)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var t = md5.ComputeHash(encode.GetBytes(input));
            var sb = new StringBuilder(32);
            for (var i = 0; i < t.Length; i++)
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            return sb.ToString();
        }

        /// <summary>
        ///     MD5对文件流加密
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string Md5Encrypt(Stream stream)
        {
            var md5Serv = MD5.Create();
            var buffer = md5Serv.ComputeHash(stream);
            var sb = new StringBuilder();
            foreach (var var in buffer)
                sb.Append(var.ToString("x2"));
            return sb.ToString();
        }

        /// <summary>
        ///     MD5加密(返回16位加密串)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Md5Encrypt16(string input,
            Encoding encode)
        {
            var md5 = new MD5CryptoServiceProvider();
            var result = BitConverter.ToString(md5.ComputeHash(encode.GetBytes(input)), 4, 8);
            result = result.Replace("-", "");
            return result;
        }

        #endregion

        #region 3DES 加密解密

        public static string Des3Encrypt(string data, 
            string key)
        {
            var des = new TripleDESCryptoServiceProvider();

            des.Key = Encoding.ASCII.GetBytes(key);
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;

            var desEncrypt = des.CreateEncryptor();

            var buffer = Encoding.ASCII.GetBytes(data);
            return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public static string Des3Decrypt(string data,
            string key)
        {
            var des = new TripleDESCryptoServiceProvider();
            des.Key = Encoding.ASCII.GetBytes(key);
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            var desDecrypt = des.CreateDecryptor();
            var result = "";
            try
            {
                var buffer = Convert.FromBase64String(data);
                result = Encoding.ASCII.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception)
            {
                // ignored
            }
            return result;
        }

        #endregion

        #region JS编码
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string HttpUtilityUrlEncode(string value)
        {
            return HttpUtility.UrlDecode(HttpUtility.UrlDecode(value));
        }
        #endregion
    }
}