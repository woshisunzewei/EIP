using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Web;
using EIP.Workflow.Business.Engine;
using EIP.Workflow.Models.Dtos.Engine;

namespace EIP.Web.Areas.Workflow.Controllers
{
    /// <summary>
    /// 已发起流程
    /// </summary>
    public class HaveSendController : BaseController
    {
        #region 构造函数

        private readonly IWorkflowEngineLogic _workflowEngineLogic;

        public HaveSendController(IWorkflowEngineLogic workflowEngineLogic)
        {
            _workflowEngineLogic = workflowEngineLogic;
        }

        #endregion
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     所有发起过的流程(我的请求)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetWorkflowEngineHaveSendOutput(WorkflowEngineRunnerInput input)
        {
            input.CurrentUser = CurrentUser;
            return Json(await _workflowEngineLogic.GetWorkflowEngineHaveSendOutput(input));
        }
    }
}
