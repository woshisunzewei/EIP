namespace EIP.Web.Areas.System.Models
{
    /// <summary>
    /// 系统程序集版本
    /// </summary>
    public class SystemRunningViewModel
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 运行时版本
        /// </summary>
        public string ClrVersion { get; set; }
    }
}