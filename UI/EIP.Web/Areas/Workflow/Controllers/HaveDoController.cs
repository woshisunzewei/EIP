using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Web;
using EIP.Workflow.Business.Engine;
using EIP.Workflow.Models.Dtos.Engine;

namespace EIP.Web.Areas.Workflow.Controllers
{
    /// <summary>
    /// 已处理流程
    /// </summary>
    public class HaveDoController : BaseController
    {
        #region 构造函数

        private readonly IWorkflowEngineLogic _workflowEngineLogic;

        public HaveDoController(IWorkflowEngineLogic workflowEngineLogic)
        {
            _workflowEngineLogic = workflowEngineLogic;
        }

        #endregion

        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     已处理的任务(已处理事项)
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetWorkflowEngineHaveDoTaskOutput(WorkflowEngineRunnerInput input)
        {
            input.CurrentUser = CurrentUser;
            return JsonForGridPaging(await _workflowEngineLogic.GetWorkflowEngineHaveDoTaskOutput(input));
        }
    }
}
