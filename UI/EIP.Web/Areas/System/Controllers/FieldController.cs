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
using EIP.System.Models.Entities;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     字段管理控制器
    /// </summary>
    public class FieldController : BaseController
    {
        #region 构造函数

        private readonly ISystemFieldLogic _fieldLogic;
        private readonly ISystemMenuLogic _menuLogic;

        public FieldController(ISystemFieldLogic fieldLogic, ISystemMenuLogic menuLogic)
        {
            _fieldLogic = fieldLogic;
            _menuLogic = menuLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("字段维护-视图-列表")]
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
        [Description("字段维护-视图-编辑")]
        public async Task<ViewResultBase> Edit(Guid? id,
            Guid? menuId = null,
            string menuName = null)
        {
            var field = new SystemFieldOutput();
            field.Hidden = true;
            if (menuId != null)
                field.MenuId = Guid.Parse(menuId.ToString());

            if (string.IsNullOrEmpty(menuName))
                field.MenuName = menuName;

            if (id != null)
            {
                field = (await _fieldLogic.GetByIdAsync(id)).MapTo<SystemFieldOutput>();
                //获取菜单信息
                field.MenuName = (await _menuLogic.GetByIdAsync(field.MenuId)).Name;
            }
            return View(field);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据菜单Id获取字段数据
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("字段维护-方法-列表-根据菜单Id获取字段数据")]
        public async Task<JsonResult> GetFieldByMenuId(SystemFieldPagingInput paging)
        {
            return JsonForGridPaging(await _fieldLogic.GetFieldByMenuId(paging));
        }

        /// <summary>
        ///     保存字段信息
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("字段维护-方法-新增/编辑-保存字段信息")]
        public async Task<JsonResult> SaveField(SystemField field)
        {
            return Json(await _fieldLogic.SaveField(field));
        }

        /// <summary>
        ///     根据字段Id删除字段信息
        /// </summary>
        /// <param name="input">字段Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("字段维护-方法-列表-删除字段信息")]
        public async Task<JsonResult> DeleteField(IdInput input)
        {
            return Json(await _fieldLogic.DeleteField(input));
        }

        #endregion
    }
}