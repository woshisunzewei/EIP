using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Identity;
using EIP.System.Models.Entities;
using EIP.Web.Areas.System.Models;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     岗位控制器
    /// </summary>
    public class PostController : BaseController
    {
        #region 构造函数
        private readonly ISystemPostLogic _postLogic;
        private readonly ISystemOrganizationLogic _organizationLogic;
        public PostController(ISystemPostLogic postLogic, ISystemOrganizationLogic organizationLogic)
        {
            _postLogic = postLogic;
            _organizationLogic = organizationLogic;
        }

        #endregion

        #region 视图
        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("岗位维护-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        /// 岗位基础信息编辑
        /// </summary>
        /// <param name="viewModel">岗位Id</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("岗位维护-视图-编辑")]
        public async Task<ViewResultBase> Edit(SystemPostEditViewModel viewModel)
        {
            SystemPost post = new SystemPost();
            //如果为编辑
            if (!viewModel.PostId.IsNullOrEmptyGuid())
            {
                post = await _postLogic.GetByIdAsync(viewModel.PostId);
                ViewData["OrganizationName"] = (await _organizationLogic.GetByIdAsync(post.OrganizationId)).Name;
            }
            //新增
            else
            {
                post.CreateTime = DateTime.Now;
                if (!viewModel.OrganizationId.IsNullOrEmptyGuid())
                {
                    post.OrganizationId = (Guid)viewModel.OrganizationId;
                    ViewData["OrganizationName"] = (await _organizationLogic.GetByIdAsync(viewModel.OrganizationId)).Name;
                }

            }
            return View(post);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 根据组织机构获取岗位信息
        /// </summary>
        /// <param name="input">组织机构Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("岗位维护-方法-列表-根据组织机构获取岗位信息")]
        public async Task<JsonResult> GetPostByOrganizationId(NullableIdInput input)
        {
            return Json(await _postLogic.GetPostByOrganizationId(input));
        }

        /// <summary>
        /// 检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("岗位维护-方法-新增/编辑-检测代码是否已经具有重复项")]
        public async Task<JsonResult> CheckPostCode(CheckSameValueInput input)
        {
            return JsonForCheckSameValue(await _postLogic.CheckPostCode(input));
        }

        /// <summary>
        ///     保存岗位数据
        /// </summary>
        /// <param name="post">岗位信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("岗位维护-方法-新增/编辑-保存")]
        public async Task<JsonResult> SavePost(SystemPost post)
        {
            post.CreateUserId = CurrentUser.UserId;
            post.CreateUserName = CurrentUser.Name;
            return Json(await _postLogic.SavePost(post));
        }

        /// <summary>
        ///    删除岗位数据
        /// </summary>
        /// <param name="input">岗位Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("岗位维护-方法-列表-删除")]
        public async Task<JsonResult> DeletePost(IdInput input)
        {
            return Json(await _postLogic.DeletePost(input));
        }
        #endregion
    }
}