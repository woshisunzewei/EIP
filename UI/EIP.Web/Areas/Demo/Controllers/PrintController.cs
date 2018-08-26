using System.Web.Mvc;

namespace EIP.Web.Areas.Demo.Controllers
{
    /// <summary>
    /// 打印控制器
    /// </summary>
    public class PrintController : Controller
    {
        /// <summary>
        /// 浏览器自带打印
        /// </summary>
        /// <returns></returns>
        public ViewResultBase Browser()
        {
            return View();
        }

        /// <summary>
        /// Lodop打印插件
        /// </summary>
        /// <returns></returns>
        public ViewResultBase Lodop()
        {
            return View();
        }
    }
}
