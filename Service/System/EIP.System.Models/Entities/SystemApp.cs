using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;

namespace EIP.System.Models.Entities
{
    /// <summary>
    /// BPF_App表实体类
    /// </summary>
    [Serializable]
    [Table(Name = "System_App")]
    public class SystemApp : EntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Id]
        public Guid AppId { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 域名/Ip
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// Dll路径
        /// </summary>
        public string DllPath { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
