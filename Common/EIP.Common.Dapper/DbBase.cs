using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace EIP.Common.Dapper
{
    public class DbBase : IDisposable
    {
        private string _paramPrefix = "@";
        private readonly string _providerName = "System.Data.SqlClient";
        private readonly IDbConnection _dbConnecttion;
        private readonly DbProviderFactory _dbFactory;
        private DBType _dbType = DBType.SqlServer;
        public IDbConnection DbConnecttion
        {
            get
            {
                return _dbConnecttion;
            }
        }
        public IDbTransaction DbTransaction
        {
            get
            {
                return _dbConnecttion.BeginTransaction();
            }
        }
        public string ParamPrefix
        {
            get
            {
                return _paramPrefix;
            }
        }
        public string ProviderName
        {
            get
            {
                return _providerName;
            }
        }
        public DBType DbType
        {
            get
            {
                return _dbType;
            }
        }
        public DbBase(string connectionStringName)
        {
            var connStr = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName))
                _providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
            else
                throw new Exception("ConnectionStrings中没有配置提供程序ProviderName！");
            _dbFactory = DbProviderFactories.GetFactory(_providerName);
            _dbConnecttion = _dbFactory.CreateConnection();
            _dbConnecttion.ConnectionString = connStr;
            _dbConnecttion.Open();
            SetParamPrefix();
        }


        private void SetParamPrefix()
        {
            string dbtype = (_dbFactory == null ? _dbConnecttion.GetType() : _dbFactory.GetType()).Name;

            // 使用类型名判断
            if (dbtype.StartsWith("MySql")) _dbType = DBType.MySql;
            else if (dbtype.StartsWith("SqlCe")) _dbType = DBType.SqlServerCE;
            else if (dbtype.StartsWith("Npgsql")) _dbType = DBType.PostgreSQL;
            else if (dbtype.StartsWith("Oracle")) _dbType = DBType.Oracle;
            else if (dbtype.StartsWith("SQLite")) _dbType = DBType.SQLite;
            else if (dbtype.StartsWith("System.Data.SqlClient.")) _dbType = DBType.SqlServer;
            // else try with provider name
            else if (_providerName.IndexOf("MySql", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.MySql;
            else if (_providerName.IndexOf("SqlServerCe", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.SqlServerCE;
            else if (_providerName.IndexOf("Npgsql", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.PostgreSQL;
            else if (_providerName.IndexOf("Oracle", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.Oracle;
            else if (_providerName.IndexOf("SQLite", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.SQLite;

            if (_dbType == DBType.MySql && _dbConnecttion != null && _dbConnecttion.ConnectionString != null && _dbConnecttion.ConnectionString.IndexOf("Allow User Variables=true") >= 0)
                _paramPrefix = "?";
            if (_dbType == DBType.Oracle)
                _paramPrefix = ":";
        }

        public void Dispose()
        {
            if (_dbConnecttion != null)
            {
                try
                {
                    _dbConnecttion.Dispose();
                }
                catch { }
            }
        }
    }
    public enum DBType
    {
        SqlServer,
        SqlServerCE,
        MySql,
        PostgreSQL,
        Oracle,
        SQLite
    }
}