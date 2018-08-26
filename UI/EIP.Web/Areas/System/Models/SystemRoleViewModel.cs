using System;

namespace EIP.Web.Areas.System.Models
{
    /// <summary>
    /// 角色用户
    /// </summary>
    public class SystemRoleUserModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid U { get; set; }

        /// <summary>
        /// 角色Id:获取角色用户使用
        /// </summary>
        public Guid RoleId { get; set; }
    }
}