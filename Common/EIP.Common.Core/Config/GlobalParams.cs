using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EIP.Common.Core.Config
{
    /// <summary>
    /// 读取数据库中配置信息
    /// </summary>
    public static class GlobalParams
    {
        #region 参数
        private static readonly object ObjectToLock = new object();

        private static bool _loaded;

        /// <summary>
        /// 配置表
        /// </summary>
        private static Dictionary<string, object> _configs;
        #endregion

        #region  根据传入的code获取值
        /// <summary>
        /// 根据传入的code获取值
        ///     数据库中进行维护
        ///     存入缓存
        /// </summary>
        /// <param name="code">配置的键</param>
        /// <returns></returns>
        public static object Get(string code)
        {
            return Get(code, true);
        }

        /// <summary>
        /// 重新赋值
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        public static void Set(string code, string value)
        {
            var config = _configs.FirstOrDefault(w => w.Key == code.ToUpper());
            _configs.Remove(config.Key);
            _configs.Add(code.ToUpper(), value);
        }

        /// <summary>
        /// 根据传入的code获取值
        /// </summary>
        /// <param name="code">配置的键</param>
        /// <param name="throwOnError">键值不存在时是否抛出异常</param>
        /// <returns></returns>
        public static object Get(string code,
            bool throwOnError)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException("code");
            if (!_loaded)
            {
                lock (ObjectToLock)
                {
                    if (!_loaded)
                    {
                        Load();
                    }
                }
            }
            var targetKey = code.ToUpper();
            object result;
            if (_configs.TryGetValue(targetKey, out result))
            {
                var exception = result as Exception;
                if (exception != null)
                {
                    throw exception;
                }
                return result;
            }
            if (throwOnError)
            {
                throw new ConfigurationErrorsException(string.Format("键值【{0}】没有在系统配置中配置。", code));
            }
            return null;
        }
        #endregion

        #region 从数据库加载配置表
        /// <summary>
        /// 从数据库加载配置表
        /// </summary>
        private static void Load()
        {
            _configs = new Dictionary<string, object>();

            //读取数据库
            IEnumerable<string[]> configRecords = ReadConfigRecords();

            //添加配置记录
            foreach (string[] record in configRecords)
            {
                string code = record[0];
                string strValue = record[1];
                _configs.Add(code.ToUpper(), strValue);
            }
            _loaded = true;
        }

        /// <summary>
        /// 从数据库读取配置记录
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string[]> ReadConfigRecords()
        {
            string connectionString = ReadConnectionString();
            string queryString = GetQueryString();
            var data = new List<string[]>();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            data.Add(new[] { reader[0].ToString(), reader[1].ToString() });
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    throw new DataException("打开数据库失败:【连接超时】");
                }

            }
            return data;
        }

        /// <summary>
        /// 获取查询字符串
        /// </summary>
        /// <returns></returns>
        private static string GetQueryString()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT [Code], \n");
            queryBuilder.Append("       [Value] \n");
            queryBuilder.Append("FROM   System_Config \n");
            return queryBuilder.ToString();
        }

        /// <summary>
        /// 获取系统配置数据库连接字符串
        /// </summary>
        /// <returns></returns>
        private static string ReadConnectionString()
        {
            var configSection = ConfigurationManager.ConnectionStrings["EIP"].ConnectionString;
            if (configSection == null)
                throw new ConfigurationErrorsException("没有在配置文件中配置系统配置组件");
            return configSection;
        }
        #endregion
    }
}