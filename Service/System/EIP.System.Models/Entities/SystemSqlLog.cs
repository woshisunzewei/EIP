using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;

namespace EIP.System.Models.Entities
{
    /// <summary>
    /// System_DataLog表实体类
    /// </summary>
    [Db(Name = "EIP_Log")]
    [Table(Name = "System_SqlLog")]
    public class SystemSqlLog : EntityBase
    {
        /// <summary>
        /// 主键Id
        /// </summary>		
        [Id]
        public Guid SqlLogId { get; set; }

        /// <summary>
        ///     操作sql
        /// </summary>
        public string OperateSql { get; set; }

        /// <summary>
        ///     结束时间
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        ///     耗时
        /// </summary>
        public double ElapsedTime { get; set; }

        /// <summary>
        ///     参数
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        ///     创建人员
        /// </summary>
        public Guid CreateUserId { get; set; }

        /// <summary>
        ///     创建人员登录代码
        /// </summary>
        public string CreateUserCode { get; set; }

        /// <summary>
        ///     创建人员名字
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}