using System;
using System.IO;
using System.Web;

namespace EIP.Common.Pay.WeiXin.lib
{
    public class Log
    {
        //在网站根目录下创建日志目录
        public static string path = HttpContext.Current.Request.PhysicalApplicationPath + "logs";

        /**
         * 向日志文件写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content, string payType)
        {
            if(WxPayConfig.LOG_LEVENL >= 3)
            {
                WriteLog("DEBUG", className, content, payType);
            }
        }

        /**
        * 向日志文件写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content,string payType)
        {
            if (WxPayConfig.LOG_LEVENL >= 2)
            {
                WriteLog("INFO", className, content, payType);
            }
        }

        /**
        * 向日志文件写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content, string payType)
        {
            if(WxPayConfig.LOG_LEVENL >= 1)
            {
                WriteLog("ERROR", className, content, payType);
            }
        }

        /**
        * 实际的写日志操作
        * @param type 日志记录类型
        * @param className 类名
        * @param content 写入内容
        */
        protected static void WriteLog(string type, string className, string content, string payType)
        {
            if(!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path +"/"+ payType + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter mySw = File.AppendText(filename); 

            //向日志文件写入内容
            string write_content ="创建日志时间："+ time + " " + type + " " + className + " " + content;
            mySw.WriteLine(write_content);

            //关闭日志文件
            mySw.Close();
        }
    }
}