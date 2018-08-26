using System;

namespace EIP.Web.Areas.System.Models
{
    /// <summary>
    /// 用户编辑
    /// </summary>
    public class SystemUserInfoEditViewModel
    {
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid? OrgId { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid? UserId { get; set; }
    }
}