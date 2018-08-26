using System;
using System.Threading;

namespace EIP.Common.Web
{
    /// <summary>
    /// 消息队列
    /// </summary>
    public class MessageQueueConfig
    {
        /// <summary>
        /// 注册登录日志队列
        /// </summary>
        public static void RegisterLoginLogQueue()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    try
                    {
                        //判断登录日志队列是否为空
                        //RedisHashOperator hashOperator = new RedisHashOperator();
                        //var loginLogs = hashOperator.GetAll<LoginLog>("LoginLogToDatabase");
                        ////删除日志缓存
                        //hashOperator.Remove("LoginLogToDatabase");
                        //hashOperator.Dispose();
                        //if (loginLogs.Any())
                        //{
                        //    //写入数据库
                        //    foreach (var loginLog in loginLogs)
                        //    {
                        //        //根据提供的api接口获取登录物理地址:http://whois.pconline.com.cn/
                        //        loginLog.IpAddressName = IpBrowserUtil.GetAddressByApi();
                        //        //读取log4net配置文件信息
                        //        var configStr = (string)GlobalParams.Get("log4net");
                        //        //序列化xml
                        //        var xml = new XmlDocument();
                        //        xml.LoadXml(configStr);
                        //        XmlConfigurator.Configure(xml.DocumentElement);
                        //        var iLog = LogManager.GetLogger("LoginLogToDatabase");
                        //        if (iLog.IsInfoEnabled)
                        //        {
                        //            iLog.Info(loginLog);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    Thread.Sleep(1000);//为避免CUP空转,在队列为空时休息1秒
                        //}
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            });
        }

        /// <summary>
        /// 注册错误日志队列
        /// </summary>
        public static void RegisterExceptionLogQueue()
        {

        }

        /// <summary>
        /// 注册操作日志队列
        /// </summary>
        public static void RegisterOperationLogQueue()
        {

        }
    }
}