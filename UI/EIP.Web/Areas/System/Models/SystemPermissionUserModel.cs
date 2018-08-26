using System;

namespace EIP.Web.Areas.System.Models
{
    public class SystemPermissionUserModel
    {
        /// <summary>
        /// 用户
        /// </summary>
        public Guid U { get; set; }

        /// <summary>
        /// 特性Id
        /// </summary>
        public Guid PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 特性Id
        /// </summary>
        public int PrivilegeMaster { get; set; } 
    }
}