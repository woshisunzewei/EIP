using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Web;
using EIP.Workflow.Business.Config;
using EIP.Workflow.Business.Engine;
using EIP.Workflow.Models.Dtos.Engine;

namespace EIP.Web.Areas.Workflow.Controllers
{
    /// <summary>
    /// 未处理/待办流程
    /// </summary>
    public class NeedDoController : BaseController
    {
          #region 构造函数

        private readonly IWorkflowProcessLogic _workflowProcessLogic;
        private readonly IWorkflowEngineLogic _workflowEngineLogic;
        private readonly IWorkflowProcessInstanceLogic _workflowProcessInstanceLogic;
        public NeedDoController(IWorkflowProcessLogic workflowProcessLogic,
            IWorkflowEngineLogic workflowEngineLogic, IWorkflowProcessInstanceLogic workflowProcessInstanceLogic)
        {
            _workflowProcessLogic = workflowProcessLogic;
            _workflowEngineLogic = workflowEngineLogic;
            _workflowProcessInstanceLogic = workflowProcessInstanceLogic;
        }

        #endregion

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        /// 获取待办流程信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetWorkflowEngineNeedDoTaskOutput(WorkflowEngineRunnerInput input)
        {
            input.CurrentUser = CurrentUser;
            return Json(await _workflowEngineLogic.GetWorkflowEngineNeedDoTaskOutput(input));
        }
    }
}
