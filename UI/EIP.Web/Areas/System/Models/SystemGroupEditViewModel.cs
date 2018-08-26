using System;

namespace EIP.Web.Areas.System.Models
{
    /// <summary>
    /// 组
    /// </summary>
    public class SystemGroupEditViewModel
    {
        /// <summary>
        /// 组Id
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}