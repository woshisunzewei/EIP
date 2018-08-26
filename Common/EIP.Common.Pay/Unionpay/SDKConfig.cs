using System.Configuration;
using System.Web.Configuration;

namespace EIP.Common.Pay.Unionpay
{
    public class SDKConfig
    {
        private static Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

        private static string signCertPath = config.AppSettings.Settings["sdk.signCert.path"].Value;  //功能：读取配置文件获取签名证书路径
        private static string signCertPwd = config.AppSettings.Settings["sdk.signCert.pwd"].Value;//功能：读取配置文件获取签名证书密码
        private static string validateCertDir = config.AppSettings.Settings["sdk.validateCert.dir"].Value;//功能：读取配置文件获取验签目录
        public static string encryptCert = config.AppSettings.Settings["sdk.encryptCert.path"].Value;  //功能：加密公钥证书路径
        
        private static string cardRequestUrl = config.AppSettings.Settings["sdk.cardRequestUrl"].Value;  //功能：有卡交易路径;
        private static string appRequestUrl = config.AppSettings.Settings["sdk.appRequestUrl"].Value;  //功能：appj交易路径;
        private static string singleQueryUrl = config.AppSettings.Settings["sdk.singleQueryUrl"].Value; //功能：读取配置文件获取交易查询地址
        private static string fileTransUrl = config.AppSettings.Settings["sdk.fileTransUrl"].Value;  //功能：读取配置文件获取文件传输类交易地址
        private static string frontTransUrl = config.AppSettings.Settings["sdk.frontTransUrl"].Value; //功能：读取配置文件获取前台交易地址
        private static string backTransUrl = config.AppSettings.Settings["sdk.backTransUrl"].Value;//功能：读取配置文件获取后台交易地址
        private static string batTransUrl = config.AppSettings.Settings["sdk.batTransUrl"].Value;//功能：读取配批量交易地址

        private static string frontUrl = config.AppSettings.Settings["frontUrl"].Value;//功能：读取配置文件获取前台通知地址
        private static string backUrl = config.AppSettings.Settings["backUrl"].Value;//功能：读取配置文件获取前台通知地址

        private static string jfCardRequestUrl = config.AppSettings.Settings["sdk.jf.cardRequestUrl"].Value;  //功能：缴费产品有卡交易路径;
        private static string jfAppRequestUrl = config.AppSettings.Settings["sdk.jf.appRequestUrl"].Value;  //功能：缴费产品app交易路径;
        private static string jfSingleQueryUrl = config.AppSettings.Settings["sdk.jf.singleQueryUrl"].Value; //功能：读取配置文件获取缴费产品交易查询地址
        private static string jfFrontTransUrl = config.AppSettings.Settings["sdk.jf.frontTransUrl"].Value; //功能：读取配置文件获取缴费产品前台交易地址
        private static string jfBackTransUrl = config.AppSettings.Settings["sdk.jf.backTransUrl"].Value;//功能：读取配置文件获取缴费产品后台交易地址

        private static string ifValidateRemoteCert = config.AppSettings.Settings["ifValidateRemoteCert"].Value;//功能：是否验证后台https证书
        
        public static string CardRequestUrl
        {
            get { return cardRequestUrl; }
            set { cardRequestUrl = value; }
        }
        public static string AppRequestUrl
        {
            get { return appRequestUrl; }
            set { appRequestUrl = value; }
        }

        public static string FrontTransUrl
        {
            get { return frontTransUrl; }
            set { frontTransUrl = value; }
        }
        public static string EncryptCert
        {
            get { return encryptCert; }
            set { encryptCert = value; }
        }


        public static string BackTransUrl
        {
            get { return backTransUrl; }
            set { backTransUrl = value; }
        }

        public static string SingleQueryUrl
        {
            get { return singleQueryUrl; }
            set { singleQueryUrl = value; }
        }

        public static string FileTransUrl
        {
            get { return fileTransUrl; }
            set { fileTransUrl = value; }
        }

        public static string SignCertPath
        {
            get { return signCertPath; }
            set { signCertPath = value; }
        }

        public static string SignCertPwd
        {
            get { return signCertPwd; }
            set { signCertPwd = value; }
        }

        public static string ValidateCertDir
        {
            get { return validateCertDir; }
            set { validateCertDir = value; }
        }
        public static string BatTransUrl
        {
            get { return batTransUrl; }
            set { batTransUrl = value; }
        }
        public static string BackUrl
        {
            get { return backUrl; }
            set { backUrl = value; }
        }
        public static string FrontUrl
        {
            get { return frontUrl; }
            set { frontUrl = value; }
        }
        public static string JfCardRequestUrl
        {
            get { return cardRequestUrl; }
            set { cardRequestUrl = value; }
        }
        public static string JfAppRequestUrl
        {
            get { return jfAppRequestUrl; }
            set { jfAppRequestUrl = value; }
        }

        public static string JfFrontTransUrl
        {
            get { return jfFrontTransUrl; }
            set { jfFrontTransUrl = value; }
        }
        public static string JfBackTransUrl
        {
            get { return jfBackTransUrl; }
            set { jfBackTransUrl = value; }
        }
        public static string JfSingleQueryUrl
        {
            get { return jfSingleQueryUrl; }
            set { jfSingleQueryUrl = value; }
        }

        public static string IfValidateRemoteCert
        {
            get { return ifValidateRemoteCert; }
            set { ifValidateRemoteCert = value; }
        }
    }
}