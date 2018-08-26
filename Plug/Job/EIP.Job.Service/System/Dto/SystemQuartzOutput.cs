using System;
using EIP.Common.Entities.Dtos;

namespace EIP.Job.Service.System.Dto
{
    /// <summary>
    /// 输出参数
    /// </summary>
    public class SystemQuartzOutput : IOutputDto
    {
        /// <summary>
        /// JobType
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Job组名称
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// Job描述
        /// </summary>
        public string JobDescription { get; set; }

        /// <summary>
        /// 触发器类型
        /// </summary>
        public string TriggerType { get; set; }

        /// <summary>
        /// 触发器组:必须和Job组名称一样
        /// </summary>
        public string TriggerGroup { get; set; }

        /// <summary>
        /// 触发器组名称
        /// </summary>
        public string TriggerName { get; set; }

        /// <summary>
        /// 触发器组描述
        /// </summary>
        public string TriggerDescription { get; set; }

        /// <summary>
        /// 是否重复执行
        /// </summary>
        public bool Repeat { get; set; }

        /// <summary>
        /// 时间轴
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 选择好的日历
        /// </summary>
        public string ChoicedCalendar { get; set; }

        /// <summary>
        /// 根据JobKey/TriggerKey替换现有任务
        /// </summary>
        public bool ReplaceExists { get; set; }
    }
}