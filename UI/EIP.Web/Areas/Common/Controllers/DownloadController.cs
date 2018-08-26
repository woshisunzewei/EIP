using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using System.ComponentModel;

namespace EIP.Web.Areas.Common.Controllers
{
    /// <summary>
    /// 公用下载
    /// </summary>
    public class DownloadController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("公用-下载")]
        public ViewResultBase Index()
        {
            return View();
        }

    }
}
