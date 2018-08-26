using System;
using System.Web.Http.Filters;

namespace EIP.Common.Web.Api.Attibutes
{
    /// <summary>
    ///  异常拦截器
    /// 对操作使用该特性类或者是方法,不能在同一位置多次使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 接口异常处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {

        }
    }
}