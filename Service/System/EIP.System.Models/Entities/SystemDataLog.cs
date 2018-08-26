using System;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;

namespace EIP.System.Models.Entities
{
    /// <summary>
    /// System_DataLog表实体类
    /// </summary>
    [Db(Name = "EIP_Log")]
    [Table(Name = "System_DataLog")]
   public class SystemDataLog : EntityBase
    {
        /// <summary>
        /// 主键Id
        /// </summary>		
        [Id]
        public Guid DataLogId { get; set; }

        /// <summary>
        ///     操作类型:0新增/2编辑/3删除
        /// </summary>
        public byte OperateType { get; set; }

        /// <summary>
        ///     操作表
        /// </summary>
        public string OperateTable { get; set; }

        /// <summary>
        ///     操作前数据:若为新增，删除等数据
        /// </summary>
        public string OperateData { get; set; }

        /// <summary>
        ///     操作后数据:编辑操作
        /// </summary>
        public string OperateAfterData { get; set; }

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

        #region 扩展字段
        [IgnoreColumn]
        public string OperateTypeName
        {
            get
            {
                return EnumUtil.GetEnumNameByIndex<OperateType>(OperateType);
            }
        }
        #endregion
    }
}