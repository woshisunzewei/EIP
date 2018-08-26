using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Config;
using EIP.Common.Web;
using EIP.Workflow.Business.Config;
using EIP.Workflow.Business.Engine;
using EIP.Workflow.Models.Dtos.Engine;
using EIP.Workflow.Models.Entities;

namespace EIP.Web.Areas.Workflow.Controllers
{
    /// <summary>
    ///     运行时控制器
    /// </summary>
    public class RunController : BaseController
    {
        #region 构造函数

        private readonly IWorkflowProcessLogic _workflowProcessLogic;
        private readonly IWorkflowEngineLogic _workflowEngineLogic;
        private readonly IWorkflowProcessInstanceLogic _workflowProcessInstanceLogic;
        public RunController(IWorkflowProcessLogic workflowProcessLogic,
            IWorkflowEngineLogic workflowEngineLogic, IWorkflowProcessInstanceLogic workflowProcessInstanceLogic)
        {
            _workflowProcessLogic = workflowProcessLogic;
            _workflowEngineLogic = workflowEngineLogic;
            _workflowProcessInstanceLogic = workflowProcessInstanceLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        /// 流程运行开始
        /// </summary>
        /// <param name="input">表单Id</param>
        /// <returns></returns>
        public async Task<ViewResultBase> Start(WorkflowEngineStartTaskInput input)
        {
            input.Type = ResourceWorkflowEngine.开始节点;
            var output = await _workflowEngineLogic.GetWorkflowEngineStartTaskOutput(input);
            output.ProcessName = output.ProcessName + "-" + CurrentUser.Name + "(" + DateTime.Now.ToString(DateTimeConfig.DateTimeFormatS) + ")";
            return View(output);
        }

        /// <summary>
        /// 处理流程
        /// </summary>
        /// <returns></returns>
        public async Task<ViewResultBase> DealWith(WorkflowEngineRunnerInput input)
        {
            return View(await _workflowEngineLogic.GetWorkflowEngineDealWithTaskOutput(input));
        }

        #endregion

        #region 方法

        /// <summary>
        /// 开始启动流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> StartWorkflowEngineProcess(WorkflowEngineStartTaskInput input)
        {
            input.PrincipalUser = CurrentUser;
            input.Type = ResourceWorkflowEngine.开始节点;
            return Json(await _workflowEngineLogic.StartWorkflowEngineProcess(input));
        }

        /// <summary>
        /// 保存流程到下一步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> SaveWorkflowEngineTaskNext(WorkflowEngineRunnerInput input)
        {
            input.CurrentUser = CurrentUser;
            var processInfo = await _workflowProcessInstanceLogic.GetByIdAsync(input.ProcessInstanceId);
            input.Title = processInfo.Title;
            return Json(await _workflowEngineLogic.SaveWorkflowEngineTaskNext(input));
        }

        /// <summary>
        /// 获取任务历史轨迹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetWorkflowEngineTrackForTable(WorkflowEngineRunnerInput input)
        {
            return Json(await _workflowEngineLogic.GetWorkflowEngineTrackForTable(input));
        }

        #endregion

        #region 流程监控
        /// <summary>
        /// 图形化
        /// </summary>
        /// <returns></returns>
        public async Task<ViewResultBase> TrackForMap(WorkflowEngineRunnerInput input)
        {
            return View(await _workflowEngineLogic.GetWorkflowEngineTrackForMap(input));
        }
        #endregion
    }
}