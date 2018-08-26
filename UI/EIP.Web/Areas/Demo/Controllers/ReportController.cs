using System.Web.Mvc;

namespace EIP.Web.Areas.Demo.Controllers
{
    /// <summary>
    /// 报表
    /// </summary>
    public class ReportController : Controller
    {
        /// <summary>
        /// tableexport:本地
        /// </summary>
        /// <returns></returns>
        public ViewResultBase TableExport()
        {
            return View();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        public ViewResultBase Paging()
        {
            return View();
        }
    }
}
