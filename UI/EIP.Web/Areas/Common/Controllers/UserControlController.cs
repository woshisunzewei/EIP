using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.Web.Areas.System.Models;
using EIP.System.Business.Config;
using EIP.System.Business.Identity;
using EIP.System.Business.Permission;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Enums;

namespace EIP.Web.Areas.Common.Controllers
{
    /// <summary>
    ///     用户控件控制器
    /// </summary>
    public class UserControlController : BaseController
    {
        #region 选择所有组织机构

        /// <summary>
        ///     选择所有组织机构
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择所有组织机构")]
        public ViewResultBase ChosenOrganizationAll()
        {
            return View();
        }

        /// <summary>
        ///     选择所有组织机构
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择所有组织机构")]
        public ViewResultBase ChosenOrganizationUeditorAll()
        {
            return View();
        }
        #endregion

        #region 选择所有岗位

        /// <summary>
        ///     选择所有岗位
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择所有岗位")]
        public async Task<ViewResultBase> ChosenGroupAll()
        {
            return View((await _groupLogic.GetGroupByOrganizationId(new NullableIdInput())).ToList());
        }

        #endregion

        #region 选择所有组

        /// <summary>
        ///     选择所有组
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择所有组")]
        public async Task<ViewResultBase> ChosenPostAll()
        {
            return View((await _postLogic.GetPostByOrganizationId(new NullableIdInput())).ToList());
        }

        #endregion

        #region 构造函数

        private readonly ISystemDictionaryLogic _dictionaryLogic;
        private readonly ISystemGroupLogic _groupLogic;
        private readonly ISystemPostLogic _postLogic;
        private readonly ISystemUserInfoLogic _userInfoLogic;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemOrganizationLogic _organizationLogic;
        private readonly ISystemMenuLogic _menuLogic;

        public UserControlController(
            ISystemGroupLogic groupLogic,
            ISystemPostLogic postLogic,
            ISystemUserInfoLogic userInfoLogic,
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemOrganizationLogic organizationLogic,
            ISystemMenuLogic menuLogic,
            ISystemDictionaryLogic dictionaryLogic)
        {
            _groupLogic = groupLogic;
            _postLogic = postLogic;
            _menuLogic = menuLogic;
            _userInfoLogic = userInfoLogic;
            _permissionUserLogic = permissionUserLogic;
            _organizationLogic = organizationLogic;
            _dictionaryLogic = dictionaryLogic;
        }

        #endregion

        #region 选择人员

        /// <summary>
        ///     查看具有特权的人员
        /// </summary>
        /// <param name="privilegeMaster">归属人员类型:组织机构、角色、岗位、组</param>
        /// <param name="privilegeMasterValue">组织机构Id、角色Id、岗位Id、组Id</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-查看具有特权的人员")]
        public async Task<ViewResultBase> ChosenPrivilegeMasterUser(EnumPrivilegeMaster privilegeMaster,
            Guid privilegeMasterValue)
        {
            //获取所有人员信息
            IList<SystemChosenUserOutput> chosenUserDtos = (await _userInfoLogic.GetChosenUser()).ToList();
            //获取所有的用户
            var permissions =
                await
                    _permissionUserLogic.GetPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(privilegeMaster,
                        privilegeMasterValue);
            foreach (var dto in chosenUserDtos)
            {
                var permission = permissions.Where(w => w.PrivilegeMasterUserId == dto.UserId).FirstOrDefault();
                dto.Exist = permission != null;
            }
            return View(chosenUserDtos.OrderByDescending(w => w.Exist).ToList());
        }

        /// <summary>
        ///     用户信息
        /// </summary>
        /// <param name="privilegeMaster">归属人员类型:组织机构、角色、岗位、组</param>
        /// <param name="privilegeMasterValue">对应Id值</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择对应类型下的用户信息")]
        public ViewResultBase PrivilegeMasterUser(EnumPrivilegeMaster privilegeMaster,
            Guid privilegeMasterValue)
        {
            return View(new SystemPermissionUserModel
            {
                PrivilegeMasterValue = privilegeMasterValue,
                PrivilegeMaster = (byte) privilegeMaster
            });
        }

        /// <summary>
        ///     保存用户信息
        /// </summary>
        /// <param name="privilegeMasterUser">用户json字符串</param>
        /// <param name="privilegeMasterValue">角色信息</param>
        /// <param name="privilegeMaster">归属人员类型:组织机构、角色、岗位、组</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-方法-保存权限用户信息")]
        public async Task<JsonResult> SavePrivilegeMasterUser(string privilegeMasterUser,
            Guid privilegeMasterValue,
            EnumPrivilegeMaster privilegeMaster)
        {
            IList<SystemPermissionUserModel> models = privilegeMasterUser.JsonStringToList<SystemPermissionUserModel>();
            IList<Guid> users = models.Select(m => m.U).ToList();
            return
                Json(
                    await
                        _permissionUserLogic.SavePermissionUserBeforeDelete(privilegeMaster, privilegeMasterValue, users));
        }

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="privilegeMasterUserId">用户Id</param>
        /// <param name="privilegeMasterValue">归属类型Id:组织机构、角色、岗位、组</param>
        /// <param name="privilegeMaster">归属人员类型:组织机构、角色、岗位、组</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-方法-删除权限用户信息")]
        public async Task<JsonResult> DeletePrivilegeMasterUser(Guid privilegeMasterUserId,
            Guid privilegeMasterValue,
            EnumPrivilegeMaster privilegeMaster)
        {
            return
                Json(await _permissionUserLogic.DeletePrivilegeMasterUser(privilegeMasterUserId, privilegeMasterValue,
                    privilegeMaster));
        }

        /// <summary>
        ///     分页获取所有用户信息
        /// </summary>
        /// <param name="paging">用户信息分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户控件-方法-分页获取所有具有该权限的用户信息")]
        public async Task<JsonResult> GetPagingPrivilegeMasterUser(SystemUserPagingInput paging)
        {
            return JsonForGridPaging(await _userInfoLogic.PagingUserQuery(paging));
        }

        #endregion

        #region 选择图标

        /// <summary>
        ///     筛选所有图片
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择图标界面")]
        public ViewResultBase ChosenIcon()
        {
            return View();
        }

        /// <summary>
        ///     获取所有图标
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-方法-获取所有图标")]
        public JsonResult GetAllIcon()
        {
            var files = Directory.GetFiles(Server.MapPath("/Contents/images/icons"));
            return Json(files.Select(file => new FileInfo(file)).Select(fileInfo => new IconEntity
            {
                Name = fileInfo.Name,
                Length = fileInfo.Length,
                Url = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length)
            }).ToList());
        }

        #endregion

        #region 选择菜单

        /// <summary>
        ///     选择菜单
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择菜单界面")]
        public ViewResultBase ChosenMenu()
        {
            return View();
        }

        /// <summary>
        ///     读取树结构:排除下级
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="isRemove"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户控件-方法-读取菜单树结构:排除下级")]
        public async Task<JsonResult> GetMenuRemoveChildren(Guid? menuId = null,
            bool isRemove = true)
        {
            return isRemove
                ? JsonForZtreeRemoveChildren((await _menuLogic.GetAllMenu()).ToList(), menuId)
                : Json(await _menuLogic.GetAllMenu());
        }

        #endregion

        #region 选择组织机构

        /// <summary>
        ///     选择组织机构
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择组织机构界面")]
        public ViewResultBase ChosenOrganization()
        {
            return View();
        }

        /// <summary>
        ///     读取树结构:排除下级
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户控件-方法-读取组织机构树:排除下级")]
        public async Task<JsonResult> GetOrganizationTreeRemoveChildren(Guid? organizationId = null)
        {
            return JsonForZtreeRemoveChildren((await _organizationLogic.GetOrganizationTree()).ToList(), organizationId);
        }

        #endregion

        #region 选择所有下级字典

        /// <summary>
        ///     根据代码获取字典树
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDictionaryTreeByCode(string code)
        {
            return Json(await _dictionaryLogic.GetDictionaryTreeByCode(code.Replace(" ", "")));
        }

        /// <summary>
        ///     选择所有下级字典
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择所有下级字典")]
        public ViewResultBase ChosenDictionary()
        {
            return View();
        }

        /// <summary>
        ///     选择所有下级字典
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择所有下级字典")]
        public ViewResultBase ChosenDictionaryAll()
        {
            return View();
        }

        /// <summary>
        ///     选择所有下级字典
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("用户控件-视图-选择所有下级字典")]
        public ViewResultBase ChosenDictionaryEditAll()
        {
            return View();
        }

        /// <summary>
        ///     读取树结构:排除下级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("用户控件-方法-选择所有下级字典:排除下级")]
        public async Task<JsonResult> GetDictionaryRemoveChildren(Guid? id = null)
        {
            return JsonForZtreeRemoveChildren((await _dictionaryLogic.GetDictionaryTree()).ToList(), id);
        }

        #endregion
    }
}