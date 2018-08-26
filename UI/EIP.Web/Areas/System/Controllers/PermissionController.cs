using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities;
using EIP.Common.Web;
using EIP.Web.Areas.System.Models;
using EIP.System.Business.Permission;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Enums;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     权限控制器
    /// </summary>
    public class PermissionController : BaseController
    {
        #region 构造函数

        private readonly ISystemPermissionLogic _permissionLogic;
        public PermissionController(ISystemPermissionLogic permissionLogic)
        {
            _permissionLogic = permissionLogic;
        }

        #endregion

        #region 模块按钮权限

        /// <summary>
        ///     模块按钮权限
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("模块按钮权限-视图-列表")]
        public async Task<ViewResultBase> Function(Guid privilegeMasterValue,
            EnumPrivilegeMaster privilegeMaster)
        {
            //获取所有模块按钮
            return View((await _permissionLogic.GetFunctionByPrivilegeMaster(privilegeMasterValue, privilegeMaster)).ToList());
        }

        #endregion

        #region 数据权限

        /// <summary>
        ///     数据权限
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("数据权限-视图-列表")]
        public async Task<ViewResultBase> Data(Guid privilegeMasterValue,
            EnumPrivilegeMaster privilegeMaster)
        {
            return View((await _permissionLogic.GetDataByPrivilegeMaster(privilegeMasterValue, privilegeMaster)).ToList());
        }

        #endregion

        #region 字段权限

        /// <summary>
        ///     字段权限
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("字段权限-视图-列表")]
        public ViewResultBase Field()
        {
            return View();
        }

        #endregion

        #region 文件权限

        /// <summary>
        ///     文件权限
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("文件权限-视图-列表")]
        public ViewResultBase File()
        {
            return View();
        }

        #endregion

        #region 模块权限

        /// <summary>
        ///     模块权限
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("模块权限-视图-列表")]
        public ViewResultBase Menu()
        {
            return View();
        }

        /// <summary>
        ///     根据特权Id获取菜单权限信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块权限-方法-列表-根据特权Id获取菜单权限信息")]
        public async Task<JsonResult> GetPermissionByPrivilegeMasterValue(GetPermissionByPrivilegeMasterValueInput input)
        {
            return Json(await _permissionLogic.GetPermissionByPrivilegeMasterValue(input));
        }

        #endregion

        #region 公用

        /// <summary>
        ///     根据角色Id,岗位Id,组Id,人员Id获取具有的菜单信息
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>树形菜单信息</returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("系统权限公用-方法-列表-根据角色Id,岗位Id,组Id,人员Id获取具有的菜单信息")]
        public async Task<JsonResult> GetMenuHavePermissionByPrivilegeMasterValue(GetMenuHavePermissionByPrivilegeMasterValueInput input)
        {
            return JsonForZtree((await _permissionLogic.GetMenuHavePermissionByPrivilegeMasterValue(input)).ToList());
        }

        /// <summary>
        ///     保存权限
        /// </summary>
        /// <param name="input">权限类型:菜单、模块按钮</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("系统权限公用-方法-保存权限")]
        public async Task<JsonResult> SavePermission(SavePermissionInput input)
        {
            input.Permissiones = input.MenuPermissions.JsonStringToList<SystemPermissionViewModel>().Select(m => m.P).ToList();
            return Json(await _permissionLogic.SavePermission(input));
        }

        /// <summary>
        ///     获取菜单功能项信息
        /// </summary>
        /// <param name="mvcRote">权限类型:菜单、模块按钮</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("系统权限公用-方法-获取菜单功能项信息")]
        public async Task<JsonResult> GetFunctionByMenuIdAndUserId(MvcRote mvcRote)
        {
            return Json(await _permissionLogic.GetFunctionByMenuIdAndUserId(mvcRote, CurrentUser.UserId));
        }
        #endregion
    }
}