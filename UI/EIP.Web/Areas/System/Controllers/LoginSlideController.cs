using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Config;
using EIP.System.Models.Entities;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    /// 登录幻灯片控制器
    /// </summary>
    public class LoginSlideController : BaseController
    {
        #region 构造函数
        private readonly ISystemLoginSlideLogic _systemLoginSlideLogic;

        public LoginSlideController(ISystemLoginSlideLogic systemLoginSlideLogic)
        {
            _systemLoginSlideLogic = systemLoginSlideLogic;
        }
        #endregion

        #region 视图
        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-登录幻灯片-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-登录幻灯片-编辑")]
        public async Task<ViewResultBase> Edit(NullableIdInput input)
        {
            SystemLoginSlide model = new SystemLoginSlide();
            if (!input.Id.IsNullOrEmptyGuid())
            {
                model = await _systemLoginSlideLogic.GetByIdAsync(input.Id);
            }
            return View(model);
        }
        #endregion

        #region 方法

        /// <summary>
        ///     获取登录幻灯片
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-登录幻灯片-方法-获取登录幻灯片")]
        public async Task<JsonResult> GetList()
        {
            return Json((await _systemLoginSlideLogic.GetAllEnumerableAsync()).OrderBy(o=>o.OrderNo));
        }

        /// <summary>
        ///     保存登录幻灯片
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-登录幻灯片-方法-保存登录幻灯片")]
        public async Task<JsonResult> Save(SystemLoginSlide model)
        {
            return Json(await _systemLoginSlideLogic.Save(model));
        }

        /// <summary>
        ///     删除登录幻灯片
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-登录幻灯片-方法-删除登录幻灯片")]
        public async Task<JsonResult> Delete(IdInput input)
        {
            return Json(await _systemLoginSlideLogic.DeleteAsync(input.Id));
        }
        #endregion
    }
}
