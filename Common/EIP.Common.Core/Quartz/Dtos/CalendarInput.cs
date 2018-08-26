using System;
using System.Collections.Generic;
using EIP.Common.Core.Quartz.Enums;

namespace EIP.Common.Core.Quartz.Dtos
{
    /// <summary>
    /// 日历input
    /// </summary>
    public class CalendarInput
    {
        /// <summary>
        /// 日期
        /// </summary>
        public IList<DateTime> Dates { get; set; }

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
        /// 枚举类型
        /// </summary>
        public EnumCalendar EnumCalendar { get; set; }

        /// <summary>
        /// 当为Cron时条件表达式
        /// </summary>
        public string Expression { get; set; }
    }
}