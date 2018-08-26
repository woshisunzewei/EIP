using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.Workflow.Business.Config;
using EIP.Workflow.Models.Entities;

namespace EIP.Web.Areas.Workflow.Controllers
{
    /// <summary>
    ///     意见控制器
    /// </summary>
    public class CommentController : BaseController
    {
        #region 构造函数

        private readonly IWorkflowCommentLogic _commentLogic;

        public CommentController(IWorkflowCommentLogic commentLogic)
        {
            _commentLogic = commentLogic;
        }
        #endregion

        #region 视图
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程意见-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("流程意见-视图-我的列表")]
        public ViewResultBase MyList()
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
            var comment = new WorkflowComment();
            if (input != null)
            {
                if (!input.Id.IsNullOrEmptyGuid())
                {
                    comment = await _commentLogic.GetByIdAsync(input.Id);
                }
            }
            return View(comment);
        }
        #endregion

        #region 方法
        /// <summary>
        ///     读取所有信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程意见-方法-列表-读取所有信息")]
        public async Task<JsonResult> GetComment()
        {
            return Json((await _commentLogic.GetAllEnumerableAsync()).ToList().OrderBy(o => o.OrderNo));
        }

        /// <summary>
        ///     保存配置信息值
        /// </summary>
        /// <param name="comment">配置项信息</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程意见-方法-新增/编辑-保存")]
        public async Task<JsonResult> SaveComment(WorkflowComment comment)
        {
            comment.CreateUserId = CurrentUser.UserId;
            comment.CreateUserName = CurrentUser.Name;
            return Json(await _commentLogic.SaveComment(comment));
        }

        /// <summary>
        ///     根据用户Id查询对应的意见
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程意见-方法-列表-删除")]
        public async Task<JsonResult> GetWorkflowCommentByCreateUserId()
        {
            return Json(await _commentLogic.GetWorkflowCommentByCreateUserId(new IdInput
            {
                Id = CurrentUser.UserId
            }));
        }

        /// <summary>
        ///     删除意见
        /// </summary>
        /// <param name="input">意见Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("流程意见-方法-列表-删除")]
        public async Task<JsonResult> DeleteComment(IdInput input)
        {
            return Json(await _commentLogic.DeleteComment(input));
        }
        #endregion
    }
}