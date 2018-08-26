using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using log4net;

namespace EIP.Common.Pay.Unionpay
{
    public class AcpService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AcpService));

        /// <summary>
        /// 对待签名数据计算签名并赋值certid,signature字段返回签名后的报文
        /// </summary>
        /// <param name="reqData"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static void Sign(Dictionary<string, string> reqData, Encoding encoding)
        {
            Sign(reqData, encoding, SDKConfig.SignCertPath, SDKConfig.SignCertPwd);
        }

        /// <summary>
        /// 对待签名数据计算签名并赋值certid,signature字段返回签名后的报文
        /// </summary>
        /// <param name="reqData"></param>
        /// <param name="encoding">编码</param>
        /// <param name="certPath">证书路径</param>
        /// <param name="certPwd">证书密码</param>
        /// <returns></returns>
        public static void Sign(Dictionary<string, string> reqData, Encoding encoding, string certPath, string certPwd)
        {
            reqData["certId"] = CertUtil.GetSignCertId(certPath, certPwd);
            //把分转换成元
            reqData["txnAmt"] = (Convert.ToInt32(reqData["txnAmt"])).ToString();
            //将Dictionary信息转换成key1=value1&key2=value2的形式
            string stringData = SDKUtil.CreateLinkString(reqData, true, false);
            log.Info("待签名排序串：[" + stringData + "]");

            string stringSign = null;

            byte[] signDigest = SecurityUtil.Sha1X16(stringData, encoding);

            string stringSignDigest = BitConverter.ToString(signDigest).Replace("-", "").ToLower();
            log.Info("sha1结果：[" + stringSignDigest + "]");

            byte[] byteSign = SecurityUtil.SignBySoft(CertUtil.GetSignProviderFromPfx(certPath, certPwd), encoding.GetBytes(stringSignDigest));

            stringSign = Convert.ToBase64String(byteSign);
            log.Info("签名结果：[" + stringSign + "]");

            //设置签名域值
            reqData["signature"] = stringSign;

        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="rspData"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public static bool Validate(Dictionary<string, string> rspData, Encoding encoding)
        {
            //获取签名
            string signValue = rspData["signature"];
            log.Info("签名原文：[" + signValue + "]");
            byte[] signByte = Convert.FromBase64String(signValue);
            rspData.Remove("signature");
            string stringData = SDKUtil.CreateLinkString(rspData, true, false);
            log.Info("排序串：[" + stringData + "]");
            byte[] signDigest = SecurityUtil.Sha1X16(stringData, encoding);
            string stringSignDigest = BitConverter.ToString(signDigest).Replace("-", "").ToLower();
            log.Debug("sha1结果：[" + stringSignDigest + "]");
            RSACryptoServiceProvider provider = CertUtil.GetValidateProviderFromPath(rspData["certId"]);
            if (null == provider)
            {
                log.Error("未找到证书，无法验签，验签失败。");
                return false;
            }
            bool result = SecurityUtil.ValidateBySoft(provider, signByte, encoding.GetBytes(stringSignDigest));
            if (result)
            {
                log.Info("验签成功");
            }
            else
            {
                log.Info("验签失败");
            }
            return result;
        }

        /// <summary>
        /// 对控件支付成功返回的结果信息中data域进行验签（控件端获取的应答信息）
        /// </summary>
        /// <param name="jsonData">json格式数据，例如：{"sign" : "J6rPLClQ64szrdXCOtV1ccOMzUmpiOKllp9cseBuRqJ71pBKPPkZ1FallzW18gyP7CvKh1RxfNNJ66AyXNMFJi1OSOsteAAFjF5GZp0Xsfm3LeHaN3j/N7p86k3B1GrSPvSnSw1LqnYuIBmebBkC1OD0Qi7qaYUJosyA1E8Ld8oGRZT5RR2gLGBoiAVraDiz9sci5zwQcLtmfpT5KFk/eTy4+W9SsC0M/2sVj43R9ePENlEvF8UpmZBqakyg5FO8+JMBz3kZ4fwnutI5pWPdYIWdVrloBpOa+N4pzhVRKD4eWJ0CoiD+joMS7+C0aPIEymYFLBNYQCjM0KV7N726LA==",  "data" : "pay_result=success&tn=201602141008032671528&cert_id=68759585097"}</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool ValidateAppResponse(string jsonData, Encoding encoding)
        {
            log.Info("控件返回报文验签：[" + jsonData + "]");
            //获取签名
            Dictionary<string, object> data = SDKUtil.JsonToDictionary(jsonData);

            string stringData = (string)data["data"];
            string signValue = (string)data["sign"];
            Dictionary<string, string> dataMap = SDKUtil.parseQString(stringData, encoding);

            byte[] signByte = Convert.FromBase64String(signValue);
            byte[] signDigest = SecurityUtil.Sha1X16(stringData, encoding);
            string stringSignDigest = BitConverter.ToString(signDigest).Replace("-", "").ToLower();
            log.Debug("sha1结果：[" + stringSignDigest + "]");
            RSACryptoServiceProvider provider = CertUtil.GetValidateProviderFromPath(dataMap["cert_id"]);
            if (null == provider)
            {
                log.Error("未找到证书，无法验签，验签失败。");
                return false;
            }
            bool result = SecurityUtil.ValidateBySoft(provider, signByte, encoding.GetBytes(stringSignDigest));
            if (result)
            {
                log.Info("验签成功");
            }
            else
            {
                log.Info("验签失败");
            }
            return result;
        }


        /// <summary>
        /// 前台交易构造HTTP POST自动提交的交易表单
        /// </summary>
        /// <param name="reqUrl"></param>
        /// <param name="reqData"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string CreateAutoFormHtml(string reqUrl, Dictionary<string, string> reqData, Encoding encoding)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />", encoding);
            html.AppendLine("</head>");
            html.AppendLine("<body onload=\"OnLoadSubmit();\">");
            html.AppendFormat("<form id=\"pay_form\" action=\"{0}\" method=\"post\">", reqUrl);
            foreach (KeyValuePair<string, string> kvp in reqData)
            {
                html.AppendFormat("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", kvp.Key, kvp.Value);
            }
            html.AppendLine("</form>");
            html.AppendLine("<script type=\"text/javascript\">");
            html.AppendLine("<!--");
            html.AppendLine("function OnLoadSubmit()");
            html.AppendLine("{");
            html.AppendLine("document.getElementById(\"pay_form\").submit();");
            html.AppendLine("}");
            html.AppendLine("//-->");
            html.AppendLine("</script>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            string result = html.ToString();
            log.Info("生成自动跳转的HTML：[" + result + "]");
            return result;
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// http的基本post方法
        /// </summary>
        /// <param name="reqData">请求数据</param>
        /// <param name="url">URL地址</param>
        /// <param name="encoding">编码</param>
        /// <returns>服务器返回的数据</returns>
        public static Dictionary<String, String> Post(Dictionary<String, String> reqData, String reqUrl, Encoding encoding)
        {
            string postData = SDKUtil.CreateLinkString(reqData, false, true);
            byte[] byteArray = encoding.GetBytes(postData);
            try
            {
                if ("false".Equals(SDKConfig.IfValidateRemoteCert)) //测试环境不验https证书
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                log.Info("发送post请求，url=[" + reqUrl + "]，data=[" + postData + "]");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(reqUrl);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.ContentLength = byteArray.Length;
                request.ServicePoint.Expect100Continue = false;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(byteArray, 0, byteArray.Length);

                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(webResponse.GetResponseStream(), encoding);
                String sResult = reader.ReadToEnd();

                requestStream.Close();
                reader.Close();
                webResponse.Close();
                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    log.Info("收到后台应答，data=[" + sResult + "]");
                    return SDKUtil.CoverStringToDictionary(sResult, encoding);
                }
                else
                {
                    string httpStatus = Enum.GetName(typeof(HttpStatusCode), webResponse.StatusCode);
                    log.Info("非200HTTP状态，httpStatus=" + httpStatus + "，data=[" + sResult + "]");
                    return new Dictionary<string, string>();
                }
            }
            catch (Exception ex)
            {
                log.Error("post失败，异常：" + ex.Message);
                return new Dictionary<string, string>();
            }

        }


        /// <summary>
        /// get方法
        /// </summary>
        /// <param name="reqData">请求数据</param>
        /// <param name="url">URL地址</param>
        /// <param name="encoding">编码</param>
        /// <returns>服务器返回的数据</returns>
        public static String Get(String reqUrl, Encoding encoding)
        {
            log.Debug("发送get请求：" + reqUrl); //get先debug，缴费相关前端接口数据量貌似还挺大
            try
            {
                if ("false".Equals(SDKConfig.IfValidateRemoteCert)) //测试环境不验https证书
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(reqUrl);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "GET";
                request.ServicePoint.Expect100Continue = false;

                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(webResponse.GetResponseStream(), encoding);
                String sResult = reader.ReadToEnd();
                reader.Close();
                webResponse.Close();

                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    log.Debug("收到后台应答，data=[" + sResult + "]");
                    return sResult;
                }
                else
                {
                    string httpStatus = Enum.GetName(typeof(HttpStatusCode), webResponse.StatusCode);
                    log.Debug("非200HTTP状态，httpStatus=" + httpStatus + "，data=[" + sResult + "]");
                    return "";
                }
            }
            catch (Exception ex)
            {
                log.Error("get失败，异常：" + ex.Message);
                return ex.Message;
            }

        }

        /// <summary>
        /// 不勾选对敏感信息加密权限使用旧的构造customerInfo域方式，不对敏感信息进行加密（对 phoneNo，cvn2， expired不加密）
        /// </summary>
        /// <param name="customerInfo">Dictionary的customerInfo</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        /// <returns>string类型结果</returns>
        public static string GetCustomerInfo(Dictionary<string, string> customerInfo, Encoding encoding)
        {
            if (customerInfo == null || customerInfo.Count == 0)
            {
                return "";
            }
            string customerInfoStr = "{" + SDKUtil.CreateLinkString(customerInfo, false, false) + "}";
            return Convert.ToBase64String(encoding.GetBytes(customerInfoStr));
        }

        /// <summary>
        /// 持卡人信息域customerInfo构造，当勾选对敏感信息加密权限，启用新加密规范（对phoneNo，cvn2，expired加密）适用
        /// </summary>
        /// <param name="customerInfo">Dictionary的customerInfo</param>
        /// <param name="encoding">编码</param>
        /// <returns>string类型结果</returns>
        public static string GetCustomerInfoWithEncrypt(Dictionary<string, string> customerInfo, Encoding encoding)
        {
            if (customerInfo == null || customerInfo.Count == 0)
            {
                return "";
            }
            Dictionary<string, string> encryptedInfo = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in customerInfo)
            {
                if (pair.Key == "phoneNo" || pair.Key == "cvn2" || pair.Key == "expired")
                {
                    encryptedInfo[pair.Key] = pair.Value;
                }
            }
            customerInfo.Remove("phoneNo");
            customerInfo.Remove("cvn2");
            customerInfo.Remove("expired");
            if (encryptedInfo.Count != 0)
            {
                string encryptedInfoStr = SDKUtil.CreateLinkString(encryptedInfo, false, false);
                encryptedInfoStr = SecurityUtil.EncryptData(encryptedInfoStr, encoding);
                customerInfo["encryptedInfo"] = encryptedInfoStr;
            }
            string customerInfoStr = "{" + SDKUtil.CreateLinkString(customerInfo, false, false) + "}";
            return Convert.ToBase64String(encoding.GetBytes(customerInfoStr));
        }


        /// <summary>
        /// 将批量文件内容使用DEFLATE压缩算法压缩，Base64编码生成字符串
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        public static string EnCodeFileContent(string filePath, Encoding encoding)
        {

            string fileContent;
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                fileContent = sr.ReadToEnd();
                byte[] fileContentByte = SecurityUtil.deflater(encoding.GetBytes(fileContent));
                fileContent = Convert.ToBase64String(fileContentByte);
                sr.Close();
                fs.Close();
                return fileContent;
            }
            else
            {
                log.Error(filePath + "文件不存在，无法得到fileContent");
                return null;
            }
        }

        /// <summary>
        /// 解析交易返回的fileContent字符串并落地 (解base64，解DEFLATE压缩并落地) 适用：对账文件下载，批量交易状态查询的文件落地
        /// </summary>
        /// <param name="resData"></param>
        /// <param name="savePath"></param>
        public static bool DeCodeFileContent(Dictionary<string, string> resData, string fileDirectory)
        {
            string fileContent = resData["fileContent"];
            string fileName;
            if ( resData.ContainsKey("fileName")) 
                fileName = resData["fileName"];
            else 
                fileName = resData["merId"] + "_" + resData["batchNo"] + "_" + resData["txnTime"] + ".txt";
            try
            {
                //Base64解码
                byte[] dBase64Byte = Convert.FromBase64String(fileContent);
                //解压缩
                byte[] fileByte = SecurityUtil.inflater(dBase64Byte);

                //保存
                string path = Path.Combine(fileDirectory, fileName);
                FileStream fs = new FileStream(path, FileMode.Create);
                fs.Write(fileByte, 0, fileByte.Length);
                fs.Close();
                fs.Dispose();

                return true;
            }
            catch (Exception e)
            {
                log.Error("保存fileContent出错：" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 敏感信息加密并做base64(卡号，手机号，cvn2,有效期）
        /// </summary>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public static string EncryptData(string data, Encoding encoding)
        {
            return SecurityUtil.EncryptData(data, encoding);
        }

        /// <summary>
        /// 敏感信息解密
        /// </summary>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public static string DecryptData(string data, Encoding encoding)
        {
            return SecurityUtil.DecryptData(data, encoding);
        }


        /// <summary>
        /// 敏感信息解密，多证书
        /// </summary>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public static string DecryptData(string data, Encoding encoding, string certPath, string certPwd)
        {
            return SecurityUtil.DecryptData(data, encoding, certPath, certPwd);
        }

        //获取敏感信息加密证书的物理序列号
        public static String GetEncryptCertId()
        {
            return CertUtil.GetEncryptCertId();
        }

        /// <summary>
        /// 将customerInfo转化为Dictionary，为方便处理，encryptedInfo下面的信息也均转换为customerInfo子域一样方式处理
        /// </summary>
        /// <param name="customerInfoStr">string的customerInfo</param>
        /// <param name="encoding">编码</param>
        /// <returns>Dictionary类型结果</returns>
        public static Dictionary<string, string> ParseCustomerInfo(string customerInfoStr, Encoding encoding)
        {
            return ParseCustomerInfo(customerInfoStr, encoding, SDKConfig.SignCertPath, SDKConfig.SignCertPwd);
        }

        /// <summary>
        /// 将customerInfo转化为Dictionary，为方便处理，encryptedInfo下面的信息也均转换为customerInfo子域一样方式处理
        /// </summary>
        /// <param name="customerInfoStr">string的customerInfo</param>
        /// <param name="encoding">编码</param>
        /// <returns>Dictionary类型结果</returns>
        public static Dictionary<string, string> ParseCustomerInfo(string customerInfoStr, Encoding encoding, string certPath, string certPwd)
        {
            if (customerInfoStr == null || customerInfoStr.Trim().Equals(""))
            {
                return new Dictionary<string, string>();
            }
            string s = null;
            try
            {
                s = encoding.GetString(Convert.FromBase64String(customerInfoStr));
            }
            catch (Exception e)
            {
                log.Error("customerInfoStr解析失败，异常：" + e.Message);
                return new Dictionary<string, string>();
            }
            s = s.Substring(1, s.Length - 2);
            Dictionary<string, string> customerInfo = SDKUtil.parseQString(s, encoding);
            if (customerInfo.Keys.Contains("encryptedInfo"))
            {
                string encryptedInfoStr = customerInfo["encryptedInfo"];
                customerInfo.Remove("encryptedInfo");
                encryptedInfoStr = SecurityUtil.DecryptData(encryptedInfoStr, encoding, certPath, certPwd);
                Dictionary<string, string> encryptedInfo = SDKUtil.parseQString(encryptedInfoStr, encoding);
                foreach (KeyValuePair<string, string> pair in encryptedInfo)
                {
                    customerInfo[pair.Key] = pair.Value;
                }
            }
            return customerInfo;
        }
    }
}