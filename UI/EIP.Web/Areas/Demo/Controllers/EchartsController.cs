using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Web;
using EIP.System.Business.Log;

namespace EIP.Web.Areas.Demo.Controllers
{
    /// <summary>
    /// Echarts插件
    /// </summary>
    public class EchartsController : BaseController
    {
        #region 构造函数

        private readonly ISystemExceptionLogLogic _exceptionLogLogic;
        private readonly ISystemLoginLogLogic _loginLogLogic;
        private readonly ISystemOperationLogLogic _operationLogLogic;

        public EchartsController(ISystemExceptionLogLogic exceptionLogLogic,
            ISystemLoginLogLogic loginLogLogic,
            ISystemOperationLogLogic operationLogLogic)
        {
            _operationLogLogic = operationLogLogic;
            _exceptionLogLogic = exceptionLogLogic;
            _loginLogLogic = loginLogLogic;
        }

        #endregion

        #region 浏览器分析
        /// <summary>
        /// 浏览器分析
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("日志分析-视图-浏览器")]
        public ViewResultBase BrowserAnalysis()
        {
            return View();
        }

        /// <summary>
        /// 浏览器分析
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("日志分析-方法-获取浏览器分析数据")]
        public async Task<JsonResult>  GetBrowserAnalysis()
        {
            return Json(await _loginLogLogic.GetBrowserAnalysis());
        }
        #endregion

        #region 日志
        /// <summary>
        /// 登录分布图
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("日志分析-视图-登录分布图")]
        public ViewResultBase LogMapAnalysis()
        {
            return View();
        }
        #endregion
    }
}
