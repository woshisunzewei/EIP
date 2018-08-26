using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Permission;
using EIP.System.Models.Dtos.Permission;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     数据权限控制器
    /// </summary>
    public class DataController : BaseController
    {
        #region 构造函数

        private readonly ISystemDataLogic _dataLogic;
        private readonly ISystemMenuLogic _menuLogic;

        public DataController(ISystemDataLogic dataLogic, ISystemMenuLogic menuLogic)
        {
            _dataLogic = dataLogic;
            _menuLogic = menuLogic;
        }

        #endregion

        #region 视图
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("配置信息-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="menuId">菜单id</param>
        /// <param name="menuName">菜单名称</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("配置信息-视图-编辑")]
        public async Task<ViewResultBase> Edit(Guid? id,
            Guid? menuId = null,
            string menuName = null)
        {
            var data = new SystemDataDoubleWayDto();
            if (menuId != null)
                data.MenuId = Guid.Parse(menuId.ToString());

            if (string.IsNullOrEmpty(menuName))
                data.MenuName = menuName;

            if (id != null)
            {
                data =(await _dataLogic.GetByIdAsync(id)).MapTo<SystemDataDoubleWayDto>();
                //获取菜单信息
                data.MenuName =(await _menuLogic.GetByIdAsync(data.MenuId)).Name;
            }
            return View(data);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据菜单Id获取数据权限规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("配置信息-方法-列表-根据菜单Id获取数据权限规则")]
        public async Task<JsonResult>  GetDataByMenuId(NullableIdInput input = null)
        {
            return Json(await _dataLogic.GetDataByMenuId(input));
        }

        /// <summary>
        ///     保存数据权限规则
        /// </summary>
        /// <param name="doubleWayDto">数据权限规则</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("配置信息-方法-新增/编辑-保存数据权限规则")]
        [ValidateInput(false)]
        public async Task<JsonResult>  SaveData(SystemDataDoubleWayDto doubleWayDto)
        {
            return Json(await _dataLogic.SaveData(doubleWayDto));
        }

        /// <summary>
        ///     根据字段Id删除数据权限规则
        /// </summary>
        /// <param name="input">数据权限规则Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("配置信息-方法-列表-删除")]
        public async Task<JsonResult>  DeleteByDataId(IdInput input)
        {
            return Json(await _dataLogic.DeleteByDataId(input));
        }

        #endregion
    }
}