using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.Workflow.Business.Config;
using EIP.Workflow.Models.Entities;

namespace EIP.Web.Areas.Workflow.Controllers
{
    /// <summary>
    ///     按钮控制器
    /// </summary>
    public class ButtonController : BaseController
    {
        #region 构造函数

        private readonly IWorkflowButtonLogic _workflowButtonLogic;

        public ButtonController(IWorkflowButtonLogic workflowButtonLogic)
        {
            _workflowButtonLogic = workflowButtonLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程按钮-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程按钮-视图-编辑")]
        public async Task<ViewResultBase> Edit(Guid? buttonId = null)
        {
            var button = new WorkflowButton();
            if (buttonId != null)
            {
                button =await _workflowButtonLogic.GetByIdAsync(buttonId);
            }
            return View(button);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     读取所有信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程按钮-方法-列表-读取所有信息")]
        public async Task<JsonResult>  GetButton()
        {
            return Json((await _workflowButtonLogic.GetAllEnumerableAsync()).OrderBy(o => o.OrderNo));
        }

        /// <summary>
        ///     保存配置信息值
        /// </summary>
        /// <param name="button">配置项信息</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程按钮-方法-新增/编辑-保存")]
        public async Task<JsonResult>  SaveButton(WorkflowButton button)
        {
            return Json(await _workflowButtonLogic.SaveButton(button));
        }

        /// <summary>
        ///     删除配置信息
        /// </summary>
        /// <param name="input">配置项主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程按钮-方法-列表-删除")]
        public async Task<JsonResult>  DeleteButton(IdInput input)
        {
            return Json(await _workflowButtonLogic.DeleteAsync(input.Id));
        }

        #endregion
    }
}