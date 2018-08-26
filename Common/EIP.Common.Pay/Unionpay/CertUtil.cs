using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using log4net;

namespace EIP.Common.Pay.Unionpay
{

    public class Cert
    {
        public X509Certificate2 cert;
        public string certId;
        public RSACryptoServiceProvider key;
    }

    public class CertUtil
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(CertUtil));

        private static Dictionary<string, Cert> signCerts = new Dictionary<string,Cert>();
        private static Cert encryptCert = null;
        private static Dictionary<string, Cert> cerCerts = new Dictionary<string, Cert>();

        private static void initSignCert(string certPath, string certPwd)
        {
            log.Info("读取签名证书……");
            Cert signCert = new Cert();
            signCert.cert = new X509Certificate2(certPath, certPwd, X509KeyStorageFlags.MachineKeySet);
            signCert.key = (RSACryptoServiceProvider)signCert.cert.PrivateKey;
            signCert.certId = BigInteger.Parse(signCert.cert.SerialNumber, NumberStyles.HexNumber).ToString();
            //privateKeyCert.certId = BigNum.ToDecimalStr(BigNum.ConvertFromHex(privateKeyCert.cert.SerialNumber)); 低于4.0版本的.NET请使用此方法
            log.Info("签名证书读取成功，序列号：" + signCert.certId);
            signCerts[certPath] = signCert;
        }

        /// <summary>
        /// 获取签名证书私钥
        /// </summary>
        /// <returns></returns>
        public static RSACryptoServiceProvider GetSignProviderFromPfx()
        {
             log.Debug("取配置文件证书");
             return GetSignProviderFromPfx(SDKConfig.SignCertPath, SDKConfig.SignCertPwd);
        }

        /// <summary>
        /// 获取签名证书私钥
        /// </summary>
        /// <returns></returns>
        public static RSACryptoServiceProvider GetSignProviderFromPfx(string certPath, string certPwd)
        {
            log.Debug("传入证书");
            if (!signCerts.ContainsKey(certPath))
            {
                initSignCert(certPath, certPwd);
            }
            return signCerts[certPath].key;
        }


        /// <summary>
        /// 获取签名证书的证书序列号
        /// </summary>
        /// <returns></returns>
        public static string GetSignCertId(string certPath, string certPwd)
        {
            log.Debug("传入证书");
            if (!signCerts.ContainsKey(certPath))
            {
                initSignCert(certPath, certPwd);
            }
            return signCerts[certPath].certId;
        }

        /// <summary>
        /// 获取签名证书的证书序列号
        /// </summary>
        /// <returns></returns>
        public static string GetSignCertId()
        {
            log.Debug("取配置文件证书");
            return GetSignCertId(SDKConfig.SignCertPath, SDKConfig.SignCertPwd);
        }

        private static void initEncryptCert()
        {
            log.Info("读取加密证书……");
            encryptCert = new Cert();
            encryptCert.cert = new X509Certificate2(SDKConfig.EncryptCert);
            encryptCert.certId = BigInteger.Parse(encryptCert.cert.SerialNumber, NumberStyles.HexNumber).ToString();
            //encryptCert.certId = BigNum.ToDecimalStr(BigNum.ConvertFromHex(encryptCert.cert.SerialNumber)); 低于4.0版本的.NET请使用此方法
            encryptCert.key = (RSACryptoServiceProvider)encryptCert.cert.PublicKey.Key;
            log.Info("加密证书读取成功，序列号：" + encryptCert.certId);
        }

        /// <summary>
        /// 获取加密证书的证书序列号
        /// </summary>
        /// <returns></returns>
        public static string GetEncryptCertId()
        {
            if (encryptCert == null)
            {
                initEncryptCert();
            }
            return encryptCert.certId;
        }


        /// <summary>
        /// 获取加密证书的RSACryptoServiceProvider
        /// </summary>
        /// <returns></returns>
        public static RSACryptoServiceProvider GetEncryptKey()
        {
            if (encryptCert == null)
            {
                initEncryptCert();
            }
            return encryptCert.key;
        }

        private static void initCerCerts()
        {
            log.Info("读取验签证书文件夹下所有cer文件……");
            DirectoryInfo directory = new DirectoryInfo(SDKConfig.ValidateCertDir);
            FileInfo[] files = directory.GetFiles("*.cer");
            if (null == files || 0 == files.Length)
            {
                log.Info("请确定[" + SDKConfig.ValidateCertDir + "]路径下是否存在cer文件");
                return;
            }
            foreach (FileInfo file in files)
            {
                Cert cert = new Cert();
                cert.cert = new X509Certificate2(file.DirectoryName + "\\" + file.Name);
                //cert.certId = BigNum.ToDecimalStr(BigNum.ConvertFromHex(cert.cert.SerialNumber)); 低于4.0版本的.NET请使用此方法
                cert.certId = BigInteger.Parse(cert.cert.SerialNumber, NumberStyles.HexNumber).ToString();
                cert.key = (RSACryptoServiceProvider)cert.cert.PublicKey.Key;
                cerCerts[cert.certId] = cert;
                log.Info(file.Name + "读取成功，序列号：" + cert.certId);
            }
        }

        /// <summary>
        /// 通过证书id，获取验证签名的证书
        /// </summary>
        /// <param name="certId"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider GetValidateProviderFromPath(string certId)// 
        {
            if (cerCerts == null || cerCerts.Count <= 0)
            {
                initCerCerts();
            }
            if (cerCerts == null || cerCerts.Count <= 0)
            {
                log.Info("未读取到任何证书……");
                return null;
            }
            if (cerCerts.ContainsKey(certId))
            {
                return cerCerts[certId].key;
            }
            else
            {
                log.Info("未匹配到序列号为[" + certId + "]的证书");
                return null;
            }
        }

    }
}