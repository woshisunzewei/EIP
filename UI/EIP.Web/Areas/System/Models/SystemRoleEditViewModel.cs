using System;

namespace EIP.Web.Areas.System.Models
{
    /// <summary>
    /// 组织机构Id
    /// </summary>
    public class SystemRoleEditViewModel
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid? RoleId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}