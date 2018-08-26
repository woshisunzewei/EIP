using EIP.Workflow.Models.Entities;

namespace EIP.Web.Areas.Workflow.Models
{
    public class DesignerEditModel
    {
        public WorkflowProcess WorkflowProcess { get; set; }
        /// <summary>
        /// 流程类型字符串
        /// </summary>
        public string ProcessTypeStr { get; set; }
    }
}