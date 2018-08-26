using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.Workflow.Business.Config;
using EIP.Workflow.Models.Entities;

namespace EIP.Web.Areas.Workflow.Controllers
{
    /// <summary>
    ///     表单控制器
    /// </summary>
    public class FormController : BaseController
    {
        #region 构造函数
        private readonly IWorkflowFormLogic _workflowFormLogic;

        public FormController(IWorkflowFormLogic workflowFormLogic)
        {
            _workflowFormLogic = workflowFormLogic;
        }
        #endregion

        #region 视图
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("表单维护-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("表单维护-视图-编辑")]
        public async Task<ViewResultBase> Edit(NullableIdInput input)
        {
            var form = new WorkflowForm();
            if (input != null)
            {
                if (!input.Id.IsNullOrEmptyGuid())
                {
                    form = await _workflowFormLogic.GetByIdAsync(input.Id);
                }
            }
            return View(form);
        }

        /// <summary>
        /// 设计器
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("表单维护-视图-设计器")]
        public async Task<ViewResultBase> Ueditor(IdInput input)
        {
            return View(await _workflowFormLogic.GetByIdAsync(input.Id));
        }

        /// <summary>
        /// 预览设计
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("表单维护-视图-预览设计")]
        public async Task<ViewResultBase> Preview(IdInput input)
        {
            return View(await _workflowFormLogic.GetByIdAsync(input.Id));
        }

        #endregion

        #region 方法
        /// <summary>
        /// 获取所有流程表单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("表单维护-方法-根据表单类型获取所有表单")]
        public async Task<JsonResult>  GetFormByFormType(NullableIdInput input)
        {
            return Json(await _workflowFormLogic.GetFormByFormType(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程维护-方法-保存表单信息")]
        public async Task<JsonResult>  Save(WorkflowForm form)
        {
            form.UpdateTime = DateTime.Now;
            form.UpdateUserId = CurrentUser.UserId;
            form.UpdateUserName = CurrentUser.Name;
            return Json(await _workflowFormLogic.Save(form));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程维护-方法-保存表单设计信息")]
        [ValidateInput(false)]
        public async Task<JsonResult>  SaveWorkflowFromHtml(WorkflowForm form)
        {
            form.UpdateTime = DateTime.Now;
            form.UpdateUserId = CurrentUser.UserId;
            form.UpdateUserName = CurrentUser.Name;
            return Json(await _workflowFormLogic.SaveWorkflowFromHtml(form));
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程维护-方法-发布表单设计信息")]
        [ValidateInput(false)]
        public async Task<JsonResult>  SaveWorkflowFromPublic(WorkflowForm form)
        {
            string fileName = form.FormId + ".cshtml";
            //生成对应文件
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(
                @"@using EIP.Common.Core.Auth;
                  @using EIP.Common.Core.Utils;
                  @{
                    PrincipalUser principalUser = FormAuthenticationExtension.Current(HttpContext.Current.Request);
                   }");
            stringBuilder.Append(form.Html);
            string formUrl = "~/Areas/Workflow/Views/Form/Designer/" + fileName;
            string file = Server.MapPath(formUrl);
            //写入请求当前人员信息脚本

            FileUtil.WriteFile(file, stringBuilder.ToString());
            form.UpdateTime = DateTime.Now;
            form.UpdateUserId = CurrentUser.UserId;
            form.UpdateUserName = CurrentUser.Name;
            form.Url = formUrl;
            OperateStatus operateStatus =await _workflowFormLogic.SaveWorkflowFromUrl(form);
            if (operateStatus.ResultSign == ResultSign.Successful)
            {
                operateStatus.Message = ResourceWorkflow.表单发布成功;
            }
            return Json(operateStatus);
        }

        /// <summary>
        /// 删除表单设计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程维护-方法-删除表单设计信息")]
        public async Task<JsonResult>  Delete(IdInput input)
        {
            return Json(await _workflowFormLogic.DeleteForm(input));
        }
        #endregion
    }
}