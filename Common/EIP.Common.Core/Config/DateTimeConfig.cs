namespace EIP.Common.Core.Config
{
    /// <summary>
    /// 时间格式
    /// </summary>
    public class DateTimeConfig
    {
        /// <summary>
        /// 日期格式
        /// </summary>
        public static string DateFormat
        {
            get
            {
                return "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// 日期时间格式
        /// </summary>
        public static string DateTimeFormat
        {
            get
            {
                return "yyyy-MM-dd HH:mm";
            }
        }

        /// <summary>
        /// 日期时间格式(带秒)
        /// </summary>
        public static string DateTimeFormatS
        {
            get
            {
                return "yyyy-MM-dd HH:mm:ss";
            }
        }

        /// <summary>
        /// 日期时间格式中文
        /// </summary>
        public static string DateTimeFormatCh
        {
            get
            {
                return "yyyy年MM月dd日 HH时mm分ss秒";
            }
        } 
    }
}