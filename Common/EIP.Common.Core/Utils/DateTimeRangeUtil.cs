using System;
using EIP.Common.Core.Config;

namespace EIP.Common.Core.Utils
{
    /// <summary>
    /// 时间帮助工具类
    /// </summary>
    public  class DateTimeRangeUtil
    {
        #region 根据年月日计算星期几
        /// <summary>
        /// 根据年月日计算星期几(Label2.Text=CaculateWeekDay(2004,12,9);)
        /// </summary>
        /// <param name="dateTime">日</param>
        /// <returns></returns>
        public static string CaculateWeekDay(DateTime dateTime)
        {
            string result = "";

            if (dateTime.DayOfWeek == DayOfWeek.Monday)
            {
                result = "一";
            }
            else if (dateTime.DayOfWeek == DayOfWeek.Tuesday)
            {
                result = "二";
            }
            else if (dateTime.DayOfWeek == DayOfWeek.Wednesday)
            {
                result = "三";
            }
            else if (dateTime.DayOfWeek == DayOfWeek.Thursday)
            {
                result = "四";
            }
            else if (dateTime.DayOfWeek == DayOfWeek.Friday)
            {
                result = "五";
            }
            else if (dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                result = "六";
            }
            else if (dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                result = "日";
            }
            return result;
        }
        #endregion

        #region 将时间转换为:16:32   2015/03/17 周二 格式
        /// <summary>
        /// 将时间转换为:16:32   2015/03/17 周二 格式
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatTimeWeekDay(DateTime dateTime)
        {
            return dateTime.Hour + ":" + dateTime.Minute + " " + dateTime.Year + "/" + dateTime.Month +
                   "/" + dateTime.Day + " " + "周" + CaculateWeekDay(dateTime);
        }

        /// <summary>
        /// 时间差(格式 y:年 M:月 d:天	h:小时 m:分钟 s:秒 msec:毫秒 microsecond:微妙)
        /// </summary>
        /// <param name="start">日期1</param>
        /// <param name="end">日期2</param>
        /// <param name="dateformat">
        /// 格式 y:年 M:月 d:天	h:小时 m:分钟 s:秒 msec:毫秒 microsecond:微妙
        /// </param>
        /// <returns>差数</returns>
        public static long TimeDiff(DateTime start,
            DateTime end, 
            string dateformat)
        {
            try
            {
                var interval = start.Ticks - end.Ticks;
                DateTime tmpStart;
                DateTime tmpEnd;

                switch (dateformat)
                {
                    case "microsecond": //微妙
                        interval /= 10;
                        break;
                    case "msec": //毫秒
                        interval /= 10000;
                        break;
                    case "s": //秒
                        interval /= 10000000;
                        break;
                    case "m": //分鐘
                        interval /= 600000000;
                        break;
                    case "h": //小時
                        interval /= 36000000000;
                        break;
                    case "d": //天
                        interval /= 864000000000;
                        break;
                    case "M": //月
                        tmpStart = (start.CompareTo(end) >= 0) ? end : start;
                        tmpEnd = (start.CompareTo(end) >= 0) ? start : end;
                        interval = -1;
                        while (tmpEnd.CompareTo(tmpStart) >= 0)
                        {
                            interval++;
                            tmpStart = tmpStart.AddMonths(1);
                        }
                        break;
                    case "y": //年
                        tmpStart = (start.CompareTo(end) >= 0) ? end : start;
                        tmpEnd = (start.CompareTo(end) >= 0) ? start : end;
                        interval = -1;
                        while (tmpEnd.CompareTo(tmpStart) >= 0)
                        {
                            interval++;
                            tmpStart = tmpStart.AddMonths(1);
                        }
                        interval /= 12;
                        break;
                }
                return Math.Abs(interval);
            }
            catch
            {
                return 0;
            }
        }
        #endregion
        
        /// <summary>
        /// 初始化一个<see cref="DateTimeRangeUtil"/>类型的新实例
        /// </summary>
        public DateTimeRangeUtil()
            : this(DateTime.MinValue, DateTime.MaxValue)
        { }

        /// <summary>
        /// 初始化一个<see cref="DateTimeRangeUtil"/>类型的新实例
        /// </summary>
        public DateTimeRangeUtil(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// 获取或设置 起始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 获取或设置 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 获取 昨天的时间范围
        /// </summary>
        public static DateTimeRangeUtil Yesterday
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRangeUtil(now.Date.AddDays(-1), now.Date.AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 今天的时间范围
        /// </summary>
        public static DateTimeRangeUtil Today
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRangeUtil(now.Date.Date, now.Date.AddDays(1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 明天的时间范围
        /// </summary>
        public static DateTimeRangeUtil Tomorrow
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRangeUtil(now.Date.AddDays(1), now.Date.AddDays(2).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 上周的时间范围
        /// </summary>
        public static DateTimeRangeUtil LastWeek
        {
            get
            {
                DateTime now = DateTime.Now;
                DayOfWeek[] weeks =
                {
                    DayOfWeek.Sunday,
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday
                };
                int index = Array.IndexOf(weeks, now.DayOfWeek);
                return new DateTimeRangeUtil(now.Date.AddDays(-index - 7), now.Date.AddDays(-index).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 本周的时间范围
        /// </summary>
        public static DateTimeRangeUtil ThisWeek
        {
            get
            {
                DateTime now = DateTime.Now;
                DayOfWeek[] weeks =
                {
                    DayOfWeek.Sunday,
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday
                };
                int index = Array.IndexOf(weeks, now.DayOfWeek);
                return new DateTimeRangeUtil(now.Date.AddDays(-index), now.Date.AddDays(7 - index).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 下周的时间范围
        /// </summary>
        public static DateTimeRangeUtil NextWeek
        {
            get
            {
                DateTime now = DateTime.Now;
                DayOfWeek[] weeks =
                {
                    DayOfWeek.Sunday,
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday
                };
                int index = Array.IndexOf(weeks, now.DayOfWeek);
                return new DateTimeRangeUtil(now.Date.AddDays(-index + 7), now.Date.AddDays(14 - index).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 上个月的时间范围
        /// </summary>
        public static DateTimeRangeUtil LastMonth
        {
            get
            {
                DateTime now = DateTime.Now;
                DateTime startTime = now.Date.AddDays(-now.Day + 1).AddMonths(-1);
                DateTime endTime = startTime.AddMonths(1).AddMilliseconds(-1);
                return new DateTimeRangeUtil(startTime, endTime);
            }
        }

        /// <summary>
        /// 获取 本月的时间范围
        /// </summary>
        public static DateTimeRangeUtil ThisMonth
        {
            get
            {
                DateTime now = DateTime.Now;
                DateTime startTime = now.Date.AddDays(-now.Day + 1);
                DateTime endTime = startTime.AddMonths(1).AddMilliseconds(-1);
                return new DateTimeRangeUtil(startTime, endTime);
            }
        }

        /// <summary>
        /// 获取 下个月的时间范围
        /// </summary>
        public static DateTimeRangeUtil NextMonth
        {
            get
            {
                DateTime now = DateTime.Now;
                DateTime startTime = now.Date.AddDays(-now.Day + 1).AddMonths(1);
                DateTime endTime = startTime.AddMonths(1).AddMilliseconds(-1);
                return new DateTimeRangeUtil(startTime, endTime);
            }
        }

        /// <summary>
        /// 获取 上一年的时间范围
        /// </summary>
        public static DateTimeRangeUtil LastYear
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRangeUtil(new DateTime(now.Year - 1, 1, 1), new DateTime(now.Year, 1, 1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 本年的时间范围
        /// </summary>
        public static DateTimeRangeUtil ThisYear
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRangeUtil(new DateTime(now.Year, 1, 1), new DateTime(now.Year + 1, 1, 1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 下一年的时间范围
        /// </summary>
        public static DateTimeRangeUtil NextYear
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRangeUtil(new DateTime(now.Year + 1, 1, 1), new DateTime(now.Year + 2, 1, 1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 过去30天的时间范围
        /// </summary>
        public static DateTimeRangeUtil Last30Days
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRangeUtil(now.AddDays(-30), now);
            }
        }

        /// <summary>
        /// 获取 过去7天的时间范围
        /// </summary>
        public static DateTimeRangeUtil Last7Days
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRangeUtil(now.AddDays(-7), now);
            }
        }

        /// <summary>
        /// 返回表示当前 <see cref="T:System.Object"/> 的 <see cref="T:System.String"/>。
        /// </summary>
        /// <returns>
        /// <see cref="T:System.String"/>，表示当前的 <see cref="T:System.Object"/>。
        /// </returns>
        public override string ToString()
        {
            return string.Format("[{0} - {1}]", StartTime, EndTime);
        }
    }

    public class DateTimeNewUtil
    {

        public static DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 短日期格式(yyyy/MM/dd)
        /// </summary>
        public static string ShortDate
        {
            get
            {
                return Now.ToString(DateTimeConfig.DateFormat);
            }
        }
        /// <summary>
        /// 长日期格式(yyyy月MM日dd日)
        /// </summary>
        public static string LongDate
        {
            get
            {
                return Now.ToString("yyyy月MM日dd日");
            }
        }
        /// <summary>
        /// 日期时间(yyyy/MM/dd HH:mm)
        /// </summary>
        public static string ShortDateTime
        {
            get
            {
                return Now.ToString(DateTimeConfig.DateTimeFormat);
            }
        }
        /// <summary>
        /// 日期时间(yyyy/MM/dd HH:mm:ss)
        /// </summary>
        public static string ShortDateTimeS
        {
            get
            {
                return Now.ToString(DateTimeConfig.DateTimeFormatS);
            }
        }
        /// <summary>
        /// 日期时间(yyyy年MM月dd日 HH时mm分)
        /// </summary>
        public static string LongDateTime
        {
            get
            {
                return Now.ToString("yyyy年MM月dd日 HH时mm分");
            }
        }
        /// <summary>
        /// 日期时间(yyyy年MM月dd日 HH时mm分ss秒)
        /// </summary>
        public static string LongDateTimeS
        {
            get
            {
                return Now.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            }
        }
        /// <summary>
        /// 日期时间(yyyy年MM月dd日 HH时mm分)
        /// </summary>
        public static string LongTime
        {
            get
            {
                return Now.ToString("HH时mm分");
            }
        }

        /// <summary>
        /// 日期时间(yyyy年MM月dd日 HH时mm分)
        /// </summary>
        public static string ShortTime
        {
            get
            {
                return Now.ToString("HH:mm");
            }
        }
    }
}