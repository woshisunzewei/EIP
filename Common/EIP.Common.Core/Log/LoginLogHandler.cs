using System;
using System.Web;
using EIP.Common.Core.Auth;
using EIP.Common.Core.Utils;

namespace EIP.Common.Core.Log
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class LoginLogHandler : BaseHandler<LoginLog>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loginLogId">登录Id</param>
        public LoginLogHandler(Guid loginLogId)
            : base("LoginLogToDatabase")
        {
            PrincipalUser principalUser = new PrincipalUser
            {
                Name = "匿名用户",
                UserId = Guid.Empty
            };
            var current = HttpContext.Current;
            if (current != null)
            {
                principalUser = FormAuthenticationExtension.Current(HttpContext.Current.Request);
            }
            if (principalUser == null)
            {
                principalUser = new PrincipalUser()
                {
                    Name = "匿名用户",
                    UserId = Guid.Empty
                };
            }
            var request = HttpContext.Current.Request;
            log = new LoginLog
            {
                LoginLogId = loginLogId,
                CreateUserId = principalUser.UserId,
                CreateUserCode = principalUser.Code ?? "",
                CreateUserName = principalUser.Name,
                ServerHost = String.Format("{0}【{1}】", IpBrowserUtil.GetServerHost(), IpBrowserUtil.GetServerHostIp()),
                ClientHost = String.Format("{0}", IpBrowserUtil.GetClientIp()),
                UserAgent = request.Browser.Browser + "【" + request.Browser.Version + "】",
                OsVersion = IpBrowserUtil.GetOsVersion(),
                LoginTime = DateTime.Now,
                IpAddressName = IpBrowserUtil.GetAddressByApi()
            };
            //根据提供的api接口获取登录物理地址:http://whois.pconline.com.cn/
        }
    }
}