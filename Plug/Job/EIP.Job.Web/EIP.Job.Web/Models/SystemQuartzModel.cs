using System;
using EIP.Common.Core.Quartz.Enums;

namespace EIP.Job.Web.Models
{
    public class SystemQuartzJobModel
    {

        /// <summary>
        /// 组名称
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 作业描述
        /// </summary>
        public string JobDescription { get; set; }

        /// <summary>
        /// 触发器名称
        /// </summary>
        public string TriggerName { get; set; }

        /// <summary>
        /// 触发器组名称
        /// </summary>
        public string TriggerGroupName { get; set; }

        /// <summary>
        /// 触发器类别
        /// </summary>
        public string TriggerType { get; set; }

        /// <summary>
        /// 触发器状态
        /// </summary>
        public string TriggerState { get; set; }

        /// <summary>
        /// 下一次运行时间
        /// </summary>
        public DateTime? NextFireTime { get; set; }

        /// <summary>
        /// 上一次运行时间
        /// </summary>
        public DateTime? PreviousFireTime { get; set; }

        /// <summary>
        /// 程序集
        /// </summary>
        public string ClassName { get; set; }
    }

    /// <summary>
    /// 调度日历
    /// </summary>
    public class SystemQuartzCalendarModel
    {
        /// <summary>
        /// 日历名称
        /// </summary>
        public string CalendarName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否替换现有日历
        /// </summary>
        public bool ReplaceExists { get; set; }

        /// <summary>
        /// 更新相关作业
        /// </summary>
        public bool UpdateTriggers { get; set; }

        /// <summary>
        /// 当为Cron时条件表达式
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 日历类型枚举
        /// </summary>
        public EnumCalendar EnumCalendar { get; set; }
    }
}