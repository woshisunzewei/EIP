using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.Web.Areas.Workflow.Models;
using EIP.System.Business.Config;
using EIP.Workflow.Business.Config;
using EIP.Workflow.Models.Dtos;
using EIP.Workflow.Models.Entities;

namespace EIP.Web.Areas.Workflow.Controllers
{
    /// <summary>
    ///     工作流设计器
    /// </summary>
    public class DesignerController : BaseController
    {
        #region 构造函数

        private readonly IWorkflowProcessLogic _workflowProcessLogic;
        private readonly ISystemDictionaryLogic _systemDictionaryLogic;
        public DesignerController(IWorkflowProcessLogic workflowProcessLogic,
            ISystemDictionaryLogic systemDictionaryLogic)
        {
            _systemDictionaryLogic = systemDictionaryLogic;
            _workflowProcessLogic = workflowProcessLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程维护-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程维护-视图-编辑")]
        public async Task<ViewResultBase> Edit(NullableIdInput input)
        {
            var process = new DesignerEditModel();
            process.WorkflowProcess = new WorkflowProcess();
            if (input != null)
            {
                if (!input.Id.IsNullOrEmptyGuid())
                {
                    process.WorkflowProcess = await _workflowProcessLogic.GetByIdAsync(input.Id);
                    process.ProcessTypeStr = (await _systemDictionaryLogic.GetByIdAsync(process.WorkflowProcess.ProcessType)).Name;
                }
            }
            return View(process);
        }

        /// <summary>
        /// 设计器
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程维护-视图-设计器")]
        public async Task<ViewResultBase> Gooflow(IdInput input)
        {
            //获取流程信息
            return View(await _workflowProcessLogic.GetByIdAsync(input.Id));
        }

        /// <summary>
        /// 设计器
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程维护-视图-设计器预览")]
        public async Task<ViewResultBase> GooflowPreview(IdInput input)
        {
            //获取流程信息
            return View(await _workflowProcessLogic.GetByIdAsync(input.Id));
        }

        /// <summary>
        /// 审批活动
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程维护-视图-审批活动")]
        public ViewResultBase ApproveActivity()
        {
            return View();
        }

        /// <summary>
        /// 条件活动
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程维护-视图-连线活动")]
        public ViewResultBase LineActivity()
        {
            return View();
        }

        public ViewResultBase Test()
        {
            return View();
        }
        #endregion

        #region 方法

        /// <summary>
        /// 获取所有流程
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程维护-方法-根据流程类型获取所有流程")]
        public async Task<JsonResult> GetWorkflowByProcessType(NullableIdInput input)
        {
            return Json(await _workflowProcessLogic.GetWorkflowByProcessType(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程维护-方法-保存流程信息")]
        public async Task<JsonResult> Save(WorkflowProcess process)
        {
            process.UpdateTime = DateTime.Now;
            process.UpdateUserId = CurrentUser.UserId;
            process.UpdateUserName = CurrentUser.Name;
            return Json(await _workflowProcessLogic.Save(process));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程维护-方法-保存流程设计Json")]
        public async Task<JsonResult> SaveWorkflowProcessJson(WorkflowSaveProcessInput process)
        {
            process.UpdateTime = DateTime.Now;
            process.UpdateUserId = CurrentUser.UserId;
            process.UpdateUserName = CurrentUser.Name;
            return Json(await _workflowProcessLogic.SaveWorkflowProcessJson(process));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程维护-方法-删除流程信息")]
        public async Task<JsonResult> Delete(IdInput input)
        {
            return Json(await _workflowProcessLogic.DeleteAsync(input.Id));
        }

        #endregion
    }
}