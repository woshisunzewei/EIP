using System;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;
using EIP.System.Models.Enums;

namespace EIP.System.Models.Entities
{
    /// <summary>
    /// System_EmailAccount表实体类
    /// </summary>
    [Serializable]
    [Table(Name = "System_EmailAccount")]
    public class SystemEmailAccount : EntityBase
    {
        /// <summary>
        /// 邮件账号Id
        /// </summary>		
        [Id]
        public Guid EmailAccountId { get; set; }

        /// <summary>
        /// 邮件用户名
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 邮件密码
        /// </summary>		
        public string Password { get; set; }

        /// <summary>
        /// 类型:0错误使用,1其他使用
        /// </summary>		
        public short Type { get; set; }

        /// <summary>
        /// 启用Ssl
        /// </summary>		
        public bool Ssl { get; set; }

        /// <summary>
        /// 端口
        /// </summary>		
        public short? Port { get; set; }

        /// <summary>
        /// Smtp服务器地址:smtp.qq.com
        /// </summary>		
        public string Smtp { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        #region 扩展

        /// <summary>
        /// 类别名称
        /// </summary>
        [IgnoreColumn]
        public string TypeName
        {
            get
            {
                return EnumUtil.GetEnumNameByIndex<EnumEmailAccountType>(Type);
            }
        }

        #endregion
    }
}
