using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Permission;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Entities;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     模块维护
    /// </summary>
    public class MenuController : BaseController
    {
        #region 构造函数

        private readonly ISystemMenuLogic _menuLogic;

        public MenuController(ISystemMenuLogic menuLogic)
        {
            _menuLogic = menuLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("模块维护-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="input">菜单Id</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("模块维护-视图-编辑")]
        public async Task<ViewResultBase> Edit(SystemMenuEditInput input)
        {
            var menuDto = new SystemMenuEditOutput();
            //如果为编辑
            if (!input.Id.IsEmptyGuid())
            {
                var menu = await _menuLogic.GetByIdAsync(input.Id);
                menuDto = menu.MapTo<SystemMenuEditOutput>();

                //获取父级信息
                var parentInfo = await _menuLogic.GetByIdAsync(menuDto.ParentId);
                if (parentInfo != null)
                {
                    menuDto.ParentName = parentInfo.Name;
                    menuDto.ParentCode = parentInfo.Code;
                }
            }
            //新增
            else
            {
                if (!input.ParentId.IsEmptyGuid())
                {
                    var parentInfo = await _menuLogic.GetByIdAsync(input.ParentId);
                    menuDto.Code = parentInfo.Code;
                    menuDto.ParentId = input.ParentId;
                    menuDto.ParentName = parentInfo.Name;
                    menuDto.ParentCode = parentInfo.Code;
                }
            }
            return View(menuDto);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     获取所有菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块维护-方法-列表-获取所有菜单信息")]
        public async Task<JsonResult> GetAllMenu()
        {
            return Json(await _menuLogic.GetAllMenu());
        }

        /// <summary>
        ///     根据父级Id获取下级菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块维护-方法-列表-根据父级Id获取下级菜单")]
        public async Task<JsonResult> GetMeunuByPId(IdInput input)
        {
            return Json(await _menuLogic.GetMeunuByPId(input));
        }

        /// <summary>
        ///     保存
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块维护-方法-新增/编辑-保存")]
        public async Task<JsonResult> SaveMenu(SystemMenu menu)
        {
            return Json(await _menuLogic.SaveMenu(menu));
        }

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块维护-方法-新增/编辑-检测代码是否已经具有重复项")]
        public async Task<JsonResult> CheckMenuCode(CheckSameValueInput input)
        {
            return JsonForCheckSameValue(await _menuLogic.CheckMenuCode(input));
        }

        /// <summary>
        ///     删除菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块维护-方法-列表-删除")]
        public async Task<JsonResult> DeleteMenu(IdInput input)
        {
            return Json(await _menuLogic.DeleteMenu(input));
        }

        /// <summary>
        ///     查询所有具有菜单权限的菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块维护-方法-列表-查询所有具有菜单权限的菜单")]
        public async Task<JsonResult> GetHaveMenuPermissionMenu()
        {
            return JsonForZtree((await _menuLogic.GetHaveMenuPermissionMenu()).ToList());
        }

        /// <summary>
        ///     查询所有具有数据权限的菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块维护-方法-列表-查询所有具有数据权限的菜单")]
        public async Task<JsonResult> GetHaveDataPermissionMenu()
        {
            return JsonForZtree((await _menuLogic.GetHaveDataPermissionMenu()).ToList());
        }

        /// <summary>
        ///     查询所有具有字段权限的菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块维护-方法-列表-查询所有具有字段权限的菜单")]
        public async Task<JsonResult> GetHaveFieldPermissionMenu()
        {
            return JsonForZtree((await _menuLogic.GetHaveFieldPermissionMenu()).ToList());
        }

        /// <summary>
        ///     查询所有具有模块按钮的菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块维护-方法-列表-查询所有具有模块按钮的菜单")]
        public async Task<JsonResult> GetHaveMenuButtonPermissionMenu()
        {
            return JsonForZtree((await _menuLogic.GetHaveMenuButtonPermissionMenu()).ToList());
        }

        /// <summary>
        ///     批量生成代码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("模块维护-方法-列表-批量生成代码")]
        public async Task<JsonResult> GeneratingCode()
        {
            return Json(await _menuLogic.GeneratingCode());
        }
        #endregion
    }
}