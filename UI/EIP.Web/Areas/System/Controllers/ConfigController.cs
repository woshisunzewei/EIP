using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Config;
using EIP.System.Models.Dtos.Config;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     系统配置文件类
    /// </summary>
    public class ConfigController : BaseController
    {
        #region 构造函数

        private readonly ISystemConfigLogic _configLogic;

        public ConfigController(ISystemConfigLogic configLogic)
        {
            _configLogic = configLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("配置信息-视图-编辑")]
        public ViewResultBase BaseConfigEdit()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("配置信息-视图-编辑")]
        public ViewResultBase LogConfigEdit()
        {
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
        [Description("配置信息-方法-列表-获取配置信息")]
        public async Task<JsonResult> GetConfig()
        {
            return Json(await _configLogic.GetConfig());
        }

        /// <summary>
        ///     保存配置信息值
        /// </summary>
        /// <param name="input">配置项信息</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("配置信息-方法-新增/编辑-保存配置信息值")]
        public async Task<JsonResult> SaveConfig(Input input)
        {
            return Json(await _configLogic.SaveConfig(input.Value.JsonStringToList<SystemConfigDoubleWay>()));
        }
        #endregion
    }
}