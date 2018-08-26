using System;

namespace EIP.Web.Areas.System.Models
{
    /// <summary>
    ///岗位
    /// </summary>
    public class SystemPostEditViewModel
    {
        /// <summary>
        /// 岗位Id
        /// </summary>
        public Guid? PostId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}