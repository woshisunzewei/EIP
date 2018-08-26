using System;

namespace EIP.Common.Entities.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class DbAttribute : Attribute
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Name { get; set; }
    }
}