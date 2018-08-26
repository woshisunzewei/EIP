using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace EIP.Common.Pay.Unionpay
{
    public class SDKUtil
    {
        /// <summary>
        /// 将字符串key1=value1&key2=value2转换为Dictionary数据结构。
        /// deprecated：为兼容原始sdk没加中文编码，遇到中文乱码请改调用parseQString。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Dictionary<string, string> CoverStringToDictionary(string str,Encoding encoding)
        {
            return parseQString(str,encoding);
        }

        /**
        /// <summary>
        /// 将Dictionary内容排序后输出为键值对字符串,供打印报文使用
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string PrintDictionaryToString(Dictionary<string, string> data)
        {
            //如果不加stringComparer.Ordinal，排序方式和java treemap有差异
            SortedDictionary<string, string> treeMap = new SortedDictionary<string, string>(StringComparer.Ordinal); 

            foreach (KeyValuePair<string, string> kvp in data)
            {
                treeMap.Add(kvp.Key, kvp.Value);
            }

            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> element in treeMap)
            {
                builder.Append(element.Key + "=" + element.Value + "&");
            }

            return builder.ToString().Substring(0, builder.Length - 1);
        }
        */

        /// <summary>
        /// 把请求要素按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="para">请求要素</param>
        /// <param name="sort">是否需要根据key值作升序排列</param>
        /// <param name="encode">是否需要URL编码</param>
        /// <returns>拼接成的字符串</returns>
        public static String CreateLinkString(Dictionary<String, String> para, bool sort, bool encode)
        {
            if (para == null || para.Count == 0)
                return "";
            List<String> list = new List<String>(para.Keys);

            if (sort)
                list.Sort(StringComparer.Ordinal);

            StringBuilder sb = new StringBuilder();
            foreach (String key in list)
            {
                String value = para[key];
                if (encode && value != null)
                {
                    try
                    {
                        value = HttpUtility.UrlEncode(value);
                    }
                    catch (Exception ex)
                    {
                        //LogError(ex);
                        return "#ERROR: HttpUtility.UrlEncode Error!" + ex.Message;
                    }
                }

                sb.Append(key).Append("=").Append(value).Append("&");

            }

            return sb.Remove(sb.Length - 1, 1).ToString();
        }

 

        /// <summary>
        /// pinblock 16进制计算
        /// </summary>
        /// <param name="encoder"></param>
        /// <returns></returns>

        public static string PrintHexString(byte[] b)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < b.Length; i++)
            {
                string hex = Convert.ToString(b[i] & 0xFF, 16);
                if (hex.Length == 1)
                {
                    hex = '0' + hex;
                }
                sb.Append("0x");
                sb.Append(hex + " ");
            }
            sb.Append("");
            return sb.ToString();
        }



        /// <summary>
        /// 密码pinblock加密
        /// </summary>
        /// <param name="card"></param>
        /// <param name="pwd"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string EncryptPin(string card, string pwd)
        {

            /** 生成PIN Block **/
            byte[] pinBlock = SecurityUtil.pin2PinBlockWithCardNO(pwd, card);
            PrintHexString(pinBlock);


            X509Certificate2 pc = new X509Certificate2(SDKConfig.EncryptCert);


            RSACryptoServiceProvider p = new RSACryptoServiceProvider();

            p = (RSACryptoServiceProvider)pc.PublicKey.Key;

            byte[] enBytes = p.Encrypt(pinBlock, false);

            return Convert.ToBase64String(enBytes);


           // return SecurityUtil.EncryptPin(pwd, card, encoding);
        }


        /// <summary>
        /// 将字符串key1=value1&key2=value2转换为Dictionary数据结构。
        /// 这个故事告诉我们，应答报文不带url编码是一件无比蛋疼的事。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Dictionary<string, string> parseQString(string str, Encoding encoding)
        {
            Dictionary<String, String> Dictionary = new Dictionary<String, String>();
            int len = str.Length;
            StringBuilder temp = new StringBuilder();
            char curChar;
            String key = null;
            bool isKey = true;
            bool isOpen = false;//值里有嵌套
            char openName = '\0'; //关闭符

            for (int i = 0; i < len; i++)
            {// 遍历整个带解析的字符串
                curChar = str[i];// 取当前字符
                if (isOpen)
                {
                    if (curChar == openName)
                    {
                        isOpen = false;
                    }
                    temp.Append(curChar);
                }
                else if (curChar == '{')
                {
                    isOpen = true;
                    openName = '}';
                    temp.Append(curChar);
                }
                else if (curChar == '[')
                {
                    isOpen = true;
                    openName = ']';
                    temp.Append(curChar);
                }
                else if (isKey && curChar == '=')
                {// 如果当前生成的是key且如果读取到=分隔符
                    key = temp.ToString();
                    temp = new StringBuilder();
                    isKey = false;
                }
                else if (curChar == '&' && !isOpen)
                {// 如果读取到&分割符
                    putKeyValueToDictionary(temp, isKey, key, Dictionary, encoding);
                    temp = new StringBuilder();
                    isKey = true;
                }
                else
                {
                    temp.Append(curChar);
                }
            }
            if(key != null)
                putKeyValueToDictionary(temp, isKey, key, Dictionary, encoding);
            return Dictionary;
        }

        private static void putKeyValueToDictionary(StringBuilder temp, bool isKey, String key, Dictionary<String, String> Dictionary, Encoding encoding)
        {
            if (isKey)
            {
                key = temp.ToString();
                if (key.Length == 0)
                {
                    throw new Exception("QString format illegal");
                }
                Dictionary[key] = "";
            }
            else
            {
                if (key.Length == 0)
                {
                    throw new Exception("QString format illegal");
                }
                //Dictionary[key] = HttpUtility.UrlDecode(temp.ToString(), encoding);
                Dictionary[key] = temp.ToString();
            }
        }

        /// <summary>
        /// json->Dictionary
        /// </summary>
        /// <param name="jsonData">json数据</param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDictionary(string jsonData)
        {
            return new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(jsonData);
        }

        /// <summary>
        /// Dictionary->json
        /// </summary>
        /// <param name="dic">Dictionary数据</param>
        /// <returns>json数据</returns>
        public static string DictionaryToJson(Dictionary<string, object> dic)
        {
            return new JavaScriptSerializer().Serialize(dic);
        }
    }
}