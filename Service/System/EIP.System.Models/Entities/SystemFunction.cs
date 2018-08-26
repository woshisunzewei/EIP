using System;
using EIP.Common.Entities.CustomAttributes;
using EIP.Common.Entities;

namespace EIP.System.Models.Entities
{
    /// <summary>
    /// System_Group表实体类
    /// </summary>
    [Serializable]
    [Table(Name = "System_Function")]
    public class SystemFunction : EntityBase
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Id]
        public Guid FunctionId { get; set; }

        /// <summary>
        /// 系统代码
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 区块
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 是否为界面
        /// </summary>
        public bool IsPage { get; set; }

        /// <summary>
        /// 添加者
        /// </summary>
        public string ByDeveloperCode { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public string ByDeveloperTime { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}