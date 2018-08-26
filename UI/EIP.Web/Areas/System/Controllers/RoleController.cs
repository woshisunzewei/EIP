using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.Web.Areas.System.Models;
using EIP.System.Business.Identity;
using EIP.System.Business.Permission;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     角色控制器
    /// </summary>
    public class RoleController : BaseController
    {
        #region 构造函数

        private readonly ISystemOrganizationLogic _organizationLogic;
        private readonly ISystemRoleLogic _roleLogic;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;

        public RoleController(ISystemRoleLogic roleLogic,
            ISystemPermissionUserLogic permissionUserLogic, ISystemOrganizationLogic organizationLogic)
        {
            _roleLogic = roleLogic;
            _permissionUserLogic = permissionUserLogic;
            _organizationLogic = organizationLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     角色列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("角色维护-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     选择角色:获取某个人对应的角色信息
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("角色维护-视图-选择角色:获取某个人对应的角色信息")]
        public async Task<ViewResultBase> Chosen(Guid userId)
        {
            //获取所有角色
            IList<SystemRoleOutput> roleDtos = (await _roleLogic.GetRolesByOrganizationId()).ToList();
            //获取当前人员具有的角色
            IList<SystemRole> roles = (await _roleLogic.GetHaveUserRole(EnumPrivilegeMaster.角色, userId)).ToList();
            IList<SystemUserRoleOutput> userRoleDtos = roleDtos.Select(role => new SystemUserRoleOutput
            {
                Exist = roles.Where(w => w.RoleId == role.RoleId).Any(),
                Name = role.Name,
                OrganizationId = role.OrganizationId,
                RoleId = role.RoleId
            }).ToList();
            return View(userRoleDtos);
        }

        /// <summary>
        ///     角色基础信息编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("角色维护-视图-编辑")]
        public async Task<ViewResultBase> Edit(SystemRoleEditViewModel viewModel)
        {
            var role = new SystemRole();
            //如果为编辑
            if (!viewModel.RoleId.IsNullOrEmptyGuid())
            {
                role = await _roleLogic.GetByIdAsync(viewModel.RoleId);
                ViewData["OrganizationName"] = (await _organizationLogic.GetByIdAsync(role.OrganizationId)).Name;
            }
            //新增
            else
            {
                role.CreateTime = DateTime.Now;
                if (!viewModel.OrganizationId.IsNullOrEmptyGuid()) { 
                    role.OrganizationId = (Guid) viewModel.OrganizationId;
                    ViewData["OrganizationName"] = (await _organizationLogic.GetByIdAsync(role.OrganizationId)).Name;
                }
            }
            return View(role);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     保存用户角色信息
        /// </summary>
        /// <param name="userRole">角色json字符串</param>
        /// <param name="userId">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("角色维护-方法-新增/编辑-保存用户角色信息")]
        public async Task<JsonResult>  SaveUserRole(string userRole,
            Guid userId)
        {
            IList<SystemUserRoleViewModel> models = userRole.JsonStringToList<SystemUserRoleViewModel>();
            IList<Guid> roles = models.Select(m => m.R).ToList();
            return Json(await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.角色, userId, roles));
        }

        /// <summary>
        ///     根据组织机构获取角色信息
        /// </summary>
        /// <param name="input">组织机构主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("角色维护-方法-列表-根据组织机构获取角色信息")]
        public async Task<JsonResult>  GetRolesByOrganization(NullableIdInput input = null)
        {
            return Json(await _roleLogic.GetRolesByOrganizationId(input));
        }

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("角色维护-方法-新增/编辑-检测代码是否已经具有重复项")]
        public async Task<JsonResult>  CheckRoleCode(CheckSameValueInput input)
        {
            return JsonForCheckSameValue(await _roleLogic.CheckRoleCode(input));
        }

        /// <summary>
        ///     保存角色数据
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("角色维护-方法-新增/编辑-保存")]
        public async Task<JsonResult>  SaveRole(SystemRole role)
        {
            role.CreateUserId = CurrentUser.UserId;
            role.CreateUserName = CurrentUser.Name;
            return Json(await _roleLogic.SaveRole(role));
        }

        /// <summary>
        ///     删除角色数据
        /// </summary>
        /// <param name="input">角色Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("角色维护-方法-列表-删除")]
        public async Task<JsonResult>  DeleteRole(IdInput input)
        {
            return Json(await _roleLogic.DeleteRole(input));
        }

        #endregion
    }
}