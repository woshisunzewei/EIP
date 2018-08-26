using System.Web.Mvc;

namespace EIP.Web.Areas.Demo.Controllers
{
    /// <summary>
    /// 下载控制器
    /// </summary>
    public class DownloadController : Controller
    {
        public ViewResultBase FileDownload()
        {
            return View();
        }
    }
}
