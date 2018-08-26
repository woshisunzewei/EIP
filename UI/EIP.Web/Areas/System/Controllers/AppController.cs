using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Config;
using EIP.System.Business.Permission;
using EIP.System.Models.Entities;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     应用系统
    /// </summary>
    public class AppController : BaseController
    {
        #region 构造函数

        private readonly ISystemFunctionLogic _functionLogic;
        private readonly ISystemAppLogic _appLogic;

        public AppController(ISystemAppLogic appLogic, ISystemFunctionLogic functionLogic)
        {
            _appLogic = appLogic;
            _functionLogic = functionLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-视图-编辑")]
        public async Task<ViewResultBase> Edit(NullableIdInput input)
        {
            var app = new SystemApp();
            if (input != null)
            {
                if (!input.Id.IsNullOrEmptyGuid())
                {
                    app = await _appLogic.GetByIdAsync(input.Id);
                }
            }
            return View(app);
        }

        /// <summary>
        ///     系统规范检查
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-视图-列表")]
        public async Task<ViewResultBase> CodeCheckList()
        {
            //获取所有模块
            Dictionary<string, string> apps = (await _appLogic.GetAllEnumerableAsync()).Where(app => !app.DllPath.IsNullOrEmpty()).ToDictionary(app => app.Code, app => app.DllPath);
            //拉取模块按钮
            await _functionLogic.SaveFunction(FunctionListImport.Import(apps));
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     读取所有信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用系统-方法-列表-获取列表数据")]
        public async Task<JsonResult> GetApp()
        {
            return Json((await _appLogic.GetAllEnumerableAsync()).OrderBy(o => o.OrderNo));
        }

        /// <summary>
        ///     保存配置信息值
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用系统-方法-新增/编辑-保存配置信息值")]
        public async Task<JsonResult> SaveApp(SystemApp app)
        {
            return Json(await _appLogic.SaveApp(app));
        }

        /// <summary>
        ///     删除配置信息
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用系统-方法-列表-删除")]
        public async Task<JsonResult> DeleteApp(IdInput input)
        {
            return Json(await _appLogic.DeleteAsync(input.Id));
        }

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用系统-方法-新增/编辑-检测代码是否已经具有重复项")]
        public async Task<JsonResult> CheckAppCode(CheckSameValueInput input)
        {
            return JsonForCheckSameValue(await _appLogic.CheckAppCode(input));
        }

        /// <summary>
        ///     根据代码获取模块按钮信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用系统-方法-列表-根据系统代码获取模块按钮")]
        public async Task<JsonResult> GetFunctionsByAppCode(IdInput<string> input)
        {
            return Json(await _functionLogic.GetFunctionsByAppCode(input));
        }

        #endregion
    }
}