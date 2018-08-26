using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using EIP.Common.Core.Config;


namespace EIP.Common.Core.Auth
{
    /// <summary>
    ///     用于登录写入Cookie和取出Cookie信息
    /// </summary>
    public class FormAuthenticationExtension
    {
        //Cookie默认保存时间1天
        private static int _cookieSaveDays = 1;

        /// <summary>
        ///     赋值Cookie并加密
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="user">用户信息</param>
        public static void SetAuthCookie(string userName, PrincipalUser user)
        {
            SetAuthCookie(userName, user, false);
        }

        /// <summary>
        ///     赋值Cookie并加密
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="user">用户信息</param>
        /// <param name="rememberMe">记住我</param>
        public static void SetAuthCookie(string userName, PrincipalUser user, bool rememberMe)
        {
            //如果为记住我
            if (rememberMe)
                //配置文件中读取记住我时间
                _cookieSaveDays = Convert.ToInt32(GlobalParams.Get("rememberMeDay").ToString());
            //赋值Cookie信息
            SetAuthCookie(userName, user, rememberMe, _cookieSaveDays);
        }

        /// <summary>
        ///     赋值Cookie并加密
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="user">用户信息</param>
        /// <param name="rememberMe">记住我</param>
        /// <param name="cookiesSaveDays">cookies失效天数</param>
        public static void SetAuthCookie(string userName, PrincipalUser user, bool rememberMe, int cookiesSaveDays)
        {
            if (cookiesSaveDays != 0)
                _cookieSaveDays = cookiesSaveDays;

            if (user == null)
                throw new ArgumentNullException("user");
            //序列化
            string principalUser = (new JavaScriptSerializer()).Serialize(user);
            //创建票证
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddDays(_cookieSaveDays),
                true, principalUser);
            //将票证加密
            string cookieValue = FormsAuthentication.Encrypt(ticket);
            //创建Cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
            {
                Domain = FormsAuthentication.CookieDomain,
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };
            //如果为"记住我"
            if (rememberMe)
                cookie.Expires = DateTime.Now.AddDays(_cookieSaveDays);
            //写入Cookie
            HttpContext.Current.Response.Cookies.Remove(cookie.Name);
            HttpContext.Current.Response.Cookies.Add(cookie);
            //写入Session及Cache
            HttpContext.Current.Session[user.Code] = user.UserId;
            HttpRuntime.Cache[user.Code] = HttpContext.Current.Session.SessionID;
        }

        /// <summary>
        ///     根据请求获取当前登录人员信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static PrincipalUser Current(HttpRequest request)
        {
            //#region 开发环境解开

            //var prin = new PrincipalUser()
            //{
            //    ContactNumber = "13545454785",
            //    UserId = Guid.Parse("08eea1b5-869b-481c-8135-fe21b673056f"),
            //    Code = "admin",
            //    Name = "开发用户",

            //};
            //return prin;
            //#endregion

            if (request == null)
                throw new ArgumentNullException("request");

            // 1. 读登录Cookie
            HttpCookie cookie = request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) return null;
            try
            {
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {
                    return (new JavaScriptSerializer()).Deserialize<PrincipalUser>(ticket.UserData);
                }
                return null;
            }
            catch
            {
                /* 有异常也不要抛出，防止攻击者试探。 */
                return null;
            }
        }

        /// <summary>
        ///     清除授权Cookie信息
        /// </summary>
        public static void SignOut()
        {
            //清除所有票证
            FormsAuthentication.SignOut();
        }
    }
}