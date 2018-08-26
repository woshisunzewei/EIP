using System;
using System.Diagnostics;
using System.Text;
using EIP.Common.Core.Log;
using Quartz;

namespace EIP.Job.Service.System
{
    /// <summary>
    /// 数据库定时备份作业
    /// </summary>
    public class DatabaseBackupJob : IJob
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            StringBuilder logBuilder = new StringBuilder("开始执行数据库定时备份作业【" + DateTime.Now + "】</br>");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            JobKey jobKey = context.JobDetail.Key;
            // 获取传递过来的参数            
            JobDataMap data = context.JobDetail.JobDataMap;
            string dbConnection = data.GetString("BackupConnection_Solution");
            sw.Stop();
            LogWriter.WriteLog(FolderName.JobLog, logBuilder.Append("结束执行数据库定时备份作业【" + DateTime.Now + "】</br>耗时【" + sw.ElapsedMilliseconds + "毫秒】").ToString());
        }
    }
}