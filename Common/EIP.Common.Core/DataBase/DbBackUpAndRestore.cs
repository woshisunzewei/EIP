using System;
using System.Collections;
using System.Data.SqlClient;

namespace EIP.Common.Core.DataBase
{
    public class DbBackUpAndRestore
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public static string Server
        {
            get;
            set;
        }
        /// <summary>
        /// 登录名
        /// </summary>
        public static string Uid
        {
            get;
            set;
        }
        /// <summary>
        /// 登录密码
        /// </summary>
        public static string Pwd { get; set; }
        /// <summary>
        /// 要操作的数据库
        /// </summary>
        public static string Database
        {
            get;
            set;
        }

        /// <summary>
        /// 还原文件路经
        /// </summary>
        public static string BackUpOrRestorePath { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="isBackup"></param>
        /// <returns></returns>
        public static bool Operate(bool isBackup = true)
        {
            BackUpOrRestorePath = !BackUpOrRestorePath.EndsWith(".bak")
                ? BackUpOrRestorePath += ".bak"
                : BackUpOrRestorePath;
            //备份数据库 
            if (isBackup)
            {
                SqlConnection connection = new SqlConnection("Data Source=" + Server + ";initial catalog=" + Database + ";user id=" + Uid + ";password=" + Pwd + ";");
                SqlCommand command = new SqlCommand("use master;backup database @name to disk=@path;", connection);
                connection.Open();
                command.Parameters.AddWithValue("@name", Database);
                command.Parameters.AddWithValue("@path", BackUpOrRestorePath);
                command.ExecuteNonQuery();
                connection.Close();
            }
            //恢复数据库 
            else
            {
                //master获取数据库连接字符串
                string masterConnection = string.Format("Data Source={0};Initial Catalog=master;User ID={1};pwd={2}", Server, Uid, Pwd);
                //杀掉所有的数据库连接进程，然后再进行恢复，这样就不会存在因为数据库独占性引起的恢复错误
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = masterConnection;
                conn.Open();
                //获取所有与数据库相关进程：spid数据库进程编号，从系统表sysprocesses ,sysdatabases查询
                string sql = string.Format("SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='{0}'", Database);
                SqlCommand cmd1 = new SqlCommand(sql, conn);
                ArrayList list = new ArrayList();
                try
                {
                    var dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        list.Add(dr.GetInt16(0));
                    }
                    dr.Close();
                }
                catch (SqlException)
                {

                }
                finally
                {
                    conn.Close();//关闭数据库连接
                }
                for (int i = 0; i < list.Count; i++)  //循环杀掉进程
                {
                    conn.Open();
                    //执行杀死进程语句:Kill方法（立即停止关联的进程。）
                    cmd1 = new SqlCommand(string.Format("KILL {0}", list[i]), conn);
                    cmd1.ExecuteNonQuery();
                    conn.Close();
                }
                //还原数据库语句:backfile 是传入参数，表示恢复数据库的文件位置
                string backupSql = String.Format("RESTORE  DATABASE {0}  FROM  DISK  ='{1}' WITH REPLACE", Database, BackUpOrRestorePath);
                //这里一定要是master数据库，而不能是要还原的数据库，不然有其它进程又要占用数据库。
                SqlConnection con = new SqlConnection(masterConnection);
                SqlCommand cmd = new SqlCommand(backupSql, con);
                con.Open();
                try
                {
                    //执行备份
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {

                }
                finally
                {
                    con.Close();
                }
            }
            return true;
        }
    }
}