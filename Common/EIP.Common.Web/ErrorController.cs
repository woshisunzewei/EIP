using System.Web.Mvc;
using System.Web.Security;
using EIP.Common.Core.Utils;

namespace EIP.Common.Web
{
    /// <summary>
    ///     错误控制器
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        ///     当请求与此控制器匹配但在此控制器中找不到任何具有指定操作名称
        /// </summary>
        /// <param name="unknownAction">未知方法名称</param>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public ActionResult ActionUnknown(string unknownAction, string url)
        {
            //可自定义跳转界面输出对应错误信息
            ViewData["url"] = url;
            return View("ActionUnknown");
        }

        /// <summary>
        ///     异常触发(开发时直接抛出,发布生产系统后可重写)
        /// </summary>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public ActionResult Exception(string msg)
        {
            ////判断是否Ajax请求(若是Ajax请求则无法进行页面跳转,在ajax中进行错误处理)
            var isAjaxRequest = Request.IsAjaxRequest();
            if (isAjaxRequest)
            {
                Response.StatusCode = 500;
                return new JsonResult
                {
                    Data = new
                    {
                        errorMessage = "错误:【" + msg + "】"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            //可自定义跳转界面
            ViewData["msg"] = msg;
            return View("Exception");
        }

        /// <summary>
        /// 页面没有找到
        /// </summary>
        /// <returns></returns>
        public ActionResult PageNotFind()
        {
            return View("PageNotFind");
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <returns></returns>
        public ActionResult Warn(string msg)
        {
            ViewBag.Warn = msg;
            return View();
        }

        /// <summary>
        /// 跳转到登录
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnToLogin()
        {
            if (HttpContext.Request.Url != null)
            {
                ViewData["url"] = FormsAuthentication.LoginUrl;
            }
            return View();
        }

        /// <summary>
        ///     已登录
        /// </summary>

        /// <returns></returns>
        public ActionResult HaveLogin()
        {
            return View("HaveLogin");
        }

        /// <summary>
        /// 添加跳转界面
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ActionResult Transfer(string t)
        {
            t = DEncryptUtil.Base64Decrypt(t);
            ViewBag.Url = t;
            return View("Transfer");
        }
    }
}