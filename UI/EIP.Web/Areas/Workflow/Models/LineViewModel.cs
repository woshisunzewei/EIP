using System;

namespace EIP.Web.Areas.Workflow.Models
{
    /// <summary>
    /// 连线
    /// </summary>
    public class LineViewModel
    {
        /// <summary>
        /// 连线Id
        /// </summary>
        public Guid LineId { get; set; }

        /// <summary>
        /// 连线名称
        /// </summary>
        public string LineName { get; set; }

        /// <summary>
        /// 连线类型
        /// </summary>
        public byte LineType { get; set; }

        /// <summary>
        /// 退回连线-策略-退回策略
        /// </summary>
        public byte LineReturnPolicy { get; set; }

        /// <summary>
        /// 条件连线-表达式
        /// </summary>
        public string LineConditions { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string LineRemark { get; set; }
    }
}