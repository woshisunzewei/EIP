using System;

namespace EIP.Common.Entities.CustomAttributes
{
    /// <summary>
    /// 数据库表
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property|AttributeTargets.Class)]
    public class TableAttribute : BaseAttribute
    {
        /// <summary>
        /// 别名，对应数据里面的名字
        /// </summary>
        public string Name { get; set; }
    }
}
