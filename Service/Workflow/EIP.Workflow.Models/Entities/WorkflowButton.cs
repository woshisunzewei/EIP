using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;

namespace EIP.Workflow.Models.Entities
{
    /// <summary>
    /// Workflow_Button表实体类
    /// </summary>
	[Serializable]
	[Table(Name = "Workflow_Button")]
    public class WorkflowButton: EntityBase
    {
       
        /// <summary>
        /// 按钮Id
        /// </summary>		
		[Id]
        public Guid ButtonId{ get; set; }
       
        /// <summary>
        /// 名称
        /// </summary>		
		public string Title{ get; set; }
       
        /// <summary>
        /// 图标
        /// </summary>		
		public string Icon{ get; set; }
       
        /// <summary>
        /// 脚本
        /// </summary>		
		public string Script{ get; set; }
       
        /// <summary>
        /// 备注
        /// </summary>		
		public string Remark{ get; set; }
       
        /// <summary>
        /// 排序字段
        /// </summary>		
		public int OrderNo{ get; set; }
   } 
}
