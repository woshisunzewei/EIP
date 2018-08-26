using System;
using System.IO;
using System.Reflection;
using System.Web;
using EIP.Common.Core.Auth;
using EIP.Common.Core.Config;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace EIP.Common.Core.Log
{
    /// <summary>
    ///     日志记录:依赖Log4net
    /// </summary>
    public class LogWriter
    {
        #region 常量
        private static readonly object Lock = new object();

        /// <summary>
        ///     日志记录地址
        /// </summary>
        private static readonly string LogPath = GlobalParams.Get("logPath").ToString();
        #endregion

        #region 方法

        /// <summary>
        ///     记录日志
        /// </summary>
        /// <param name="folderName">文件夹名字</param>
        /// <param name="message">日志内容</param>
        /// <param name="path">日志存放磁盘路径</param>
        public static void WriteLog(string folderName,
            string message,
            string path = "")
        {
            try
            {
                PrincipalUser principalUser = new PrincipalUser()
                {
                    Name = "匿名用户",
                    UserId = Guid.Empty
                };
                var current = HttpContext.Current;
                if (current != null)
                {
                    principalUser = FormAuthenticationExtension.Current(HttpContext.Current.Request);
                }
                if (principalUser == null)
                {
                    principalUser = new PrincipalUser()
                   {
                       Name = "匿名用户",
                       UserId = Guid.Empty
                   };
                }
                var strPath = string.IsNullOrEmpty(path) ? LogPath : path;
                strPath = strPath + folderName + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                lock (Lock)
                {
                    var strFilename = strPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    var layout = new PatternLayout("%m%n");
                    var appender = new FileAppender(layout, strFilename, true);
                    BasicConfigurator.Configure(appender);
                    var log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                    log.Info(
                        "</br>----------------------------------------------</br>\r\n" +
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + principalUser.Name + "(" + principalUser.UserId + ")" + "</br>\r\n" + message);
                    log4net.LogManager.Shutdown();
                }
            }
            catch
            {
                throw new Exception("日志记录失败");
            }
        }

        /// <summary>
        ///     日志记录
        /// </summary>
        /// <param name="folderName">文件夹名字</param>
        /// <param name="message">内容</param>
        /// <param name="fileName">文件名（不带后缀）</param>
        /// <param name="path">保存文件地址</param>
        public static void WriteLog(string folderName,
            string message,
            string fileName,
            string path)
        {
            try
            {
                PrincipalUser principalUser = new PrincipalUser()
                {
                    Name = "匿名用户",
                    UserId = Guid.Empty
                };
                var current = HttpContext.Current;
                if (current != null)
                {
                    principalUser = FormAuthenticationExtension.Current(HttpContext.Current.Request);
                }
                if (principalUser == null)
                {
                    principalUser = new PrincipalUser()
                    {
                        Name = "匿名用户",
                        UserId = Guid.Empty
                    };
                }
                var strPath = string.IsNullOrEmpty(path) ? LogPath : path;
                strPath = strPath + folderName + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                lock (Lock)
                {
                    var strFilename = strPath + "\\" + fileName + ".txt";
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    var layout = new PatternLayout("%m%n");
                    var appender = new FileAppender(layout, strFilename, true);
                    BasicConfigurator.Configure(appender);
                    var log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                    log.Info(
                        "</br>----------------------------------------------</br>\r\n" +
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + principalUser.Name + "(" + principalUser.UserId + ")" + "</br>\r\n" + message);
                    log4net.LogManager.Shutdown();
                }
            }
            catch
            {
                throw new Exception("日志记录失败");
            }
        }

        /// <summary>
        ///     记录异常日志
        /// </summary>
        /// <param name="folderName">文件夹名称</param>
        /// <param name="e">异常</param>
        /// <param name="fileName">文件名称</param>
        public static void WriteLog(string folderName,
            Exception e,
            string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                WriteLog(folderName, e.Message + "\r\n" + e.Source + "\r\n" + e.StackTrace);
            }
            else
            {
                WriteLog(folderName, e.Message + "\r\n" + e.Source + "\r\n" + e.StackTrace, fileName);
            }
        }

        #endregion
    }

    /// <summary>
    ///     文件夹名称
    /// </summary>
    public class FolderName
    {
        public const string Error = "错误信息";
        public const string Exception = "异常信息";
        public const string Waring = "警告信息";
        public const string Debug = "调试信息";
        public const string Info = "执行信息";
        public const string ServcieLog = "服务日志";
        public const string DataLog = "数据日志";
        public const string JobLog = "作业日志";
        public const string EIPApi = "接口调用";
        public const string SqlDoLog = "Sql语句";
        public const string Workflow = "工作流";
    }
}