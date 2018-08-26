namespace EIP.Common.Core.Quartz.Dtos
{
    /// <summary>
    /// 作业类型
    /// </summary>
    public class JobTypeInput
    {
        /// <summary>
        /// 全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FullName;
        } 
    }
}