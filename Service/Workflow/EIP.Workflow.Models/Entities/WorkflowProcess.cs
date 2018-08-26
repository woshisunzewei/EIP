using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;

namespace EIP.Workflow.Models.Entities
{
    /// <summary>
    /// Workflow_Process表实体类
    /// </summary>
    [Serializable]
    [Table(Name = "Workflow_Process")]
    public class WorkflowProcess : EntityBase
    {
        /// <summary>
        /// 实例Id
        /// </summary>		
        [Id]
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 流程代码
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>		
        public byte Status { get; set; }

        /// <summary>
        /// 流程类型
        /// </summary>		
        public Guid ProcessType { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>		
        public string PageUrl { get; set; }

        /// <summary>
        /// 设计Json
        /// </summary>		
        public string DesignJson { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人员Id
        /// </summary>		
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>		
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新人员id
        /// </summary>		
        public Guid UpdateUserId { get; set; }

        /// <summary>
        /// 更新人员名称
        /// </summary>		
        public string UpdateUserName { get; set; }
    }
}
