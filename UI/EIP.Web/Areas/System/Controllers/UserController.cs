using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Dtos.Reports;
using EIP.Common.Web;
using EIP.System.Business.Identity;
using EIP.System.Business.Permission;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;
using EIP.Web.Areas.System.Models;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     用户管理:此用户为系统使用人员,系统的人员管理在其他模块进行管理(如:人事管理HR)
    ///     此模块只维护基础信息
    /// </summary>
    public class UserController : BaseController
    {
        #region 构造函数

        private readonly ISystemUserInfoLogic _userInfoLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        public UserController(ISystemUserInfoLogic userInfoLogic,
            ISystemPermissionLogic permissionLogic)
        {
            _permissionLogic = permissionLogic;
            _userInfoLogic = userInfoLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户维护-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户维护-视图-编辑")]
        public async Task<ViewResultBase> Edit(
          SystemUserInfoEditViewModel viewModel)
        {
            var user = new SystemUserInfo();
            //如果为编辑
            if (viewModel.UserId != null)
            {
                user = await _userInfoLogic.GetByIdAsync(viewModel.UserId);
            }
            //新增
            else
            {
                user.CreateTime = DateTime.Now;
            }
            ViewData["OrgId"] = viewModel.OrgId;
            ViewData["OrgName"] = viewModel.OrgName;
            return View(user);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     分页获取所有用户信息
        /// </summary>
        /// <param name="paging">用户信息分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户维护-方法-列表-分页获取所有用户信息")]
        public async Task<JsonResult> GetPagingUser(SystemUserPagingInput paging)
        {
            #region 获取权限控制器信息
            SystemPermissionSqlInput input = new SystemPermissionSqlInput()
            {
                PrincipalUser = CurrentUser,
                EnumPermissionRoteConvert = EnumPermissionRoteConvert.用户字段数据权限
            };
            paging.FiledSql = await _permissionLogic.GetFieldPermissionSql(input);
            paging.DataSql = await _permissionLogic.GetDataPermissionSql(input);
            #endregion
            paging.PrivilegeMaster = EnumPrivilegeMaster.组织机构;
            var users = await _userInfoLogic.PagingUserQuery(paging);
            return JsonForGridPaging(users);
        }

        /// <summary>
        ///     根据主键获取用户信息
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户维护-方法-列表-根据主键获取用户信息")]
        public async Task<JsonResult> GetUserByUserId(IdInput input)
        {
            return Json(await _userInfoLogic.GetByIdAsync(input.Id));
        }

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户维护-方法-新增/编辑-检测代码是否已经具有重复项")]
        public async Task<JsonResult> CheckUserCode(CheckSameValueInput input)
        {
            return JsonForCheckSameValue(await _userInfoLogic.CheckUserCode(input));
        }

        /// <summary>
        ///     保存人员数据
        /// </summary>
        /// <param name="user">人员信息</param>
        /// <param name="orgId">组织机构Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户维护-方法-新增/编辑-保存")]
        public async Task<JsonResult> SaveUser(SystemUserInfo user,
            Guid orgId)
        {
            user.CreateUserId = CurrentUser.UserId;
            user.CreateUserName = CurrentUser.Name;
            return Json(await _userInfoLogic.SaveUser(user, orgId));
        }

        /// <summary>
        ///     删除人员数据
        /// </summary>
        /// <param name="input">人员Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户维护-方法-列表-删除")]
        public async Task<JsonResult> DeleteUser(IdInput input)
        {
            return Json(await _userInfoLogic.DeleteUser(input));
        }

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户维护-方法-列表-导出到Excel")]
        public FileResult ExportExcel(SystemUserPagingInput paging)
        {
            ExcelReportDto excelReportDto = new ExcelReportDto()
            {
                TemplatePath = Server.MapPath("/") + "Templates/System/User/用户导出模版.xlsx",
                DownTemplatePath = "用户信息" + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".xlsx",
                Title = "用户信息.xlsx"
            };
            _userInfoLogic.ReportExcelUserQuery(paging, excelReportDto);
            return File(new FileStream(excelReportDto.DownPath, FileMode.Open), "application/octet-stream", Server.UrlEncode(excelReportDto.Title));

        }

        /// <summary>
        ///     根据用户Id获取用户详细情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户维护-方法-列表-根据用户Id获取用户详细情况")]
        public async Task<JsonResult> GetDetailByUserId(IdInput input)
        {
            return Json(await _userInfoLogic.GetDetailByUserId(input));
        }

        /// <summary>
        ///     根据用户Id重置某人密码
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户维护-方法-列表-重置密码")]
        public async Task<JsonResult> ResetPassword(IdInput input)
        {
            return Json(await _userInfoLogic.ResetPassword(input));
        }

        #endregion
    }
}