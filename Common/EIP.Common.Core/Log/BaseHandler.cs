using System.Xml;
using EIP.Common.Core.Config;
using log4net;
using log4net.Config;

namespace EIP.Common.Core.Log
{
    /// <summary>
    ///     说  明:日志记录基类
    ///     备  注:
    ///     编写人:孙泽伟-2015/04/01
    /// </summary>
    /// <typeparam name="TLog">记录日志实体</typeparam>
    public abstract class BaseHandler<TLog>
    {
        #region 构造函数

        /// <summary>
        ///     构造函数初始化
        /// </summary>
        /// <param name="loggerConfig"></param>
        protected BaseHandler(string loggerConfig)
        {
            LoadConfig();
            LoggerConfig = loggerConfig;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     写入日志,虚函数.可进行重写
        /// </summary>
        public virtual void WriteLog()
        {
            if (string.IsNullOrEmpty(LoggerConfig))
            {
                return;
            }
            var iLog = LogManager.GetLogger(LoggerConfig);
            if (iLog.IsInfoEnabled)
            {
                iLog.Info(log);
            }
        }

        #endregion

        #region 属性

        /// <summary>
        ///     需要启动的日志模式名称
        /// </summary>
        private string LoggerConfig { get; set; }

        public TLog log { get; set; }

        #endregion

        #region 单例模式

        private static bool hasLoad;

        private static readonly object obj = new object();

        /// <summary>
        ///     从数据库中读取配置信息
        /// </summary>
        private static void LoadConfig()
        {
            //如果没有加载则进行加载
            if (!hasLoad)
            {
                lock (obj)
                {
                    if (!hasLoad)
                    {
                        //读取log4net配置文件信息
                        var configStr = (string)GlobalParams.Get("log4net");
                        //序列化xml
                        var xml = new XmlDocument();
                        xml.LoadXml(configStr);
                        XmlConfigurator.Configure(xml.DocumentElement);
                        hasLoad = true;
                    }
                }
            }
        }
        #endregion
    }
}