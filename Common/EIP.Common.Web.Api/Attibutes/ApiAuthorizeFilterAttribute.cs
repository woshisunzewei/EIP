using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EIP.Common.Web.Api.Attibutes
{
    /// <summary>
    /// Api权限控制特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthorizeFilterAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// 执行方法前进入该重置方法
        ///     1、一个帐号只能在一个地方登录
        ///     2、权限验证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }
    }
}