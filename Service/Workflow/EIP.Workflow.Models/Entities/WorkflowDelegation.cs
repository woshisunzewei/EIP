using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;

namespace EIP.Workflow.Models.Entities
{
    /// <summary>
    /// Workflow_Delegation表实体类
    /// </summary>
	[Serializable]
	[Table(Name = "Workflow_Delegation")]
    public class WorkflowDelegation: EntityBase
    {
        /// <summary>
        /// 委托Id
        /// </summary>		
		[Id]
        public Guid DelegationId{ get; set; }
       
        /// <summary>
        /// 流程实例Id
        /// </summary>		
		public Guid ProcessInstanceId{ get; set; }
       
        /// <summary>
        /// 委托方Id
        /// </summary>		
		public Guid UserId{ get; set; }
       
        /// <summary>
        /// 被委托方Id
        /// </summary>		
		public Guid ToUserId{ get; set; }
       
        /// <summary>
        /// 开始时间
        /// </summary>		
		public DateTime StartTime{ get; set; }
       
        /// <summary>
        /// 结束时间
        /// </summary>		
		public DateTime EndTime{ get; set; }
       
        /// <summary>
        /// 创建时间
        /// </summary>		
		public DateTime CreateTime{ get; set; }
       
        /// <summary>
        /// 备注
        /// </summary>		
		public string Remark{ get; set; }
		
   } 
}
