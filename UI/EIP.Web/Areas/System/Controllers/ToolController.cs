using System.Web.Mvc;
using EIP.Common.Web;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     系统工具:(序列号、计算器等)
    /// </summary>
    public class ToolController : BaseController
    {
        #region 科学计算器

        /// <summary>
        /// 科学计算器
        /// </summary>
        /// <returns></returns>
        public ViewResultBase Calculator()
        {
            return View();
        }

        #endregion

        #region 万年历

        /// <summary>
        /// 万年历
        /// </summary>
        /// <returns></returns>
        public ViewResultBase PerpetualCalendar()
        {
            return View();
        }

        #endregion
    }
}