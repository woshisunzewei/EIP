using System;
using System.Web;
using System.Web.Mvc;
using EIP.Common.Core.Auth;
using EIP.Common.Core.Utils;

namespace EIP.Common.Web.Attributes
{
    /// <summary>
    ///  登录拦截器
    ///     需要登录验证后才能访问,在类或方法上添加此特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class LoginFilterAttribute : AuthorizeAttribute
    {
        /// <summary>
        ///     重写验证
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>是否验证成功</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            PrincipalUser principalUser = FormAuthenticationExtension.Current(HttpContext.Current.Request);
            if (principalUser != null)
            {
                HttpContext.Current.User = principalUser;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     当重写验证返回为false时进入该重写方法
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            //验证不通过,直接跳转到相应页面，注意：如果不使用以下跳转，则会继续执行Action方法
            if (filterContext.HttpContext.Request.Url != null)
            {
                filterContext.Result = new RedirectResult("/Error/Transfer?t=" + DEncryptUtil.Base64Encrypt("/Error/ReturnToLogin"));
            }
        }
    }
}