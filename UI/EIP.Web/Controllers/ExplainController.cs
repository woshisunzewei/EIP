using System.Web.Mvc;
using EIP.Common.Core.Utils;

namespace EIP.Web.Controllers
{
    public class ExplainController : Controller
    {

        /// <summary>
        /// 网站升级
        /// </summary>
        /// <returns></returns>
        public ViewResultBase Maintain()
        {
            return View();
        }
        /// <summary>
        /// 浏览器升级
        /// </summary>
        /// <returns></returns>
        public ViewResultBase BrowserUpdate()
        {
            return View();
        }

        public JsonResult Test()
        {
            EmailUtil emailUtil = new EmailUtil();
            emailUtil._mAddress = "pop.qq.com";
            emailUtil._mPort = 110;
            var info = emailUtil.GetMailTable("1039318332@qq.com", "wvoxwnxydvntbcbh");
            return null;
        }
    }
}
