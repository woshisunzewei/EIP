using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Identity;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;
using EIP.Web.Areas.System.Models;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     组管理控制器
    /// </summary>
    public class GroupController : BaseController
    {
        #region 构造函数

        private readonly ISystemGroupLogic _groupLogic;
        private readonly ISystemUserInfoLogic _userInfoLogic;
        private readonly ISystemOrganizationLogic _organizationLogic;
        public GroupController(ISystemGroupLogic groupLogic,
            ISystemUserInfoLogic userInfoLogic, ISystemOrganizationLogic organizationLogic)
        {
            _groupLogic = groupLogic;
            _userInfoLogic = userInfoLogic;
            _organizationLogic = organizationLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("组维护-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("组维护-视图-编辑")]
        public async Task<ViewResultBase> Edit(SystemGroupEditViewModel viewModel)
        {
            var group = new SystemGroup();
            //如果为编辑
            if (!viewModel.GroupId.IsNullOrEmptyGuid())
            {
                group = await _groupLogic.GetByIdAsync(viewModel.GroupId);
                ViewData["OrganizationName"] = (await _organizationLogic.GetByIdAsync(group.OrganizationId)).Name;
            }
            //新增
            else
            {
                group.CreateTime = DateTime.Now;
                if (!viewModel.OrganizationId.IsNullOrEmptyGuid())
                {
                    viewModel.OrganizationId = (Guid)viewModel.OrganizationId;
                    ViewData["OrganizationName"] = (await _organizationLogic.GetByIdAsync(viewModel.OrganizationId)).Name;
                }
            }
            return View(group);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据组织机构Id获取对应下的组信息:id为空查询所有
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("组维护-方法-列表-根据组织机构Id获取对应下的组信息:id为空查询所有")]
        public async Task<JsonResult> GetGroupByOrganizationId(NullableIdInput input)
        {
            return Json(await _groupLogic.GetGroupByOrganizationId(input));
        }

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("组维护-方法-新增/编辑-检测代码是否已经具有重复项")]
        public async Task<JsonResult> CheckGroupCode(CheckSameValueInput input)
        {
            return JsonForCheckSameValue(await _groupLogic.CheckGroupCode(input));
        }

        /// <summary>
        ///     保存组数据
        /// </summary>
        /// <param name="group">组信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("组维护-方法-新增/编辑-保存")]
        public async Task<JsonResult> SaveGroup(SystemGroup group)
        {
            group.CreateUserId = CurrentUser.UserId;
            group.CreateUserName = CurrentUser.Name;
            return Json(await _groupLogic.SaveGroup(group, EnumGroupBelongTo.系统));
        }

        /// <summary>
        ///     删除组数据
        /// </summary>
        /// <param name="input">组Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("组维护-方法-新增/编辑-删除")]
        public async Task<JsonResult> DeleteGroup(IdInput input)
        {
            return Json(await _groupLogic.DeleteGroup(input));
        }

        /// <summary>
        ///     分页获取所有用户信息
        /// </summary>
        /// <param name="paging">用户信息分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("组维护-方法-列表-分页获取所有用户信息")]
        public async Task<JsonResult> GetPagingUser(SystemUserPagingInput paging)
        {
            paging.PrivilegeMaster = EnumPrivilegeMaster.岗位;
            return JsonForGridPaging(await _userInfoLogic.PagingUserQuery(paging));
        }

        #endregion
    }
}