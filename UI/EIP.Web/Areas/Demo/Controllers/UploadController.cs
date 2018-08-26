using System.Web.Mvc;

namespace EIP.Web.Areas.Demo.Controllers
{
    /// <summary>
    /// 上传下载
    /// </summary>
    public class UploadController : Controller
    {
        /// <summary>
        /// 图片带预览
        /// </summary>
        /// <returns></returns>
        public ViewResultBase ImagePreview()
        {
            return View();
        }

    }
}
