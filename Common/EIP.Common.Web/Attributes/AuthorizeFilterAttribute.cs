using System.Configuration;
using System.Web;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Auth;
using EIP.Common.Core.Config;

namespace EIP.Common.Web.Attributes
{
    /// <summary> 
    /// 执行方法前进入该特定方法
    /// </summary>
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 执行方法前进入该重置方法
        ///     1、一个帐号只能在一个地方登录
        ///     2、权限验证
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //获取当前登录人员信息
            PrincipalUser currentUser = FormAuthenticationExtension.Current(HttpContext.Current.Request);

            #region 是否具有忽略验证特性
            //是否具有忽略特性:若有忽略特性则不进行其他的验证
            if (filterContext.ActionDescriptor.IsDefined(typeof(IgnoreAttribute), false))
            {
                return;
            }
            #endregion

            #region 一个帐号只能在一个浏览器登录
            if (GlobalParams.Get("loginOnce").ToString() == "是")
            {
                //如果当前登录人员信息不为空
                if (currentUser != null)
                {
                    //检查对应登录状态缓存是否为空
                    if (HttpRuntime.Cache[currentUser.Code] != null)
                    {
                        if (filterContext.HttpContext.Session != null &&
                            HttpRuntime.Cache[currentUser.Code].ToString() !=
                            filterContext.HttpContext.Session.SessionID)
                        {
                            //清空Session
                            filterContext.HttpContext.Session.Remove(currentUser.Code);
                            //清空Cookie
                            FormAuthenticationExtension.SignOut();
                            //跳转强制下线界面
                            ErrorRedirect(filterContext, "/Error/HaveLogin");
                        }
                    }
                    //否则重新赋值Cache
                    else
                    {
                        if (filterContext.HttpContext.Session != null)
                        {
                            filterContext.HttpContext.Session[currentUser.Code] = currentUser.UserId;
                            HttpRuntime.Cache[currentUser.Code] = filterContext.HttpContext.Session.SessionID;
                        }
                    }
                }
            }
            #endregion

            #region 用户是否登录
            PrincipalUser principalUser = FormAuthenticationExtension.Current(HttpContext.Current.Request);
            if (principalUser == null)
            {
                ErrorRedirect(filterContext, "/Error/ReturnToLogin");
                return;
            }
            #endregion

            #region 是否具有HttpPost/HttpGet请求验证
            var isAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
            #endregion

            #region 验证该方法是否需要进行权限验证
            //todo:1、获取用户信息。2、从缓存中获取该用户权限,若没有数据则从数据库中重新拉取(有可能缓存失效),再将权限数据填充到缓存中。
            //配置的当前系统代码
            string appCode = ConfigurationManager.AppSettings["AppCode"];
            //区域
            string area = string.Empty;
            //控制器
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //方法
            string action = filterContext.ActionDescriptor.ActionName;
            var routeData = filterContext.RequestContext.RouteData;
            if (routeData.DataTokens["area"] != null)
            {
                area = routeData.DataTokens["area"].ToString();
            }
            //调用Api接口查看是否具有该权限
            string apiUrl = ConfigurationManager.AppSettings["SolutionApiUrl"];

            //是否为Ajax请求,若是Ajax请求则不进行界面验证(此处只验证视图)
            if (!isAjaxRequest)
            {
                if (currentUser != null)
                {
                    //string request = RequestUtil.SendPostRequest(apiUrl + "api/System/Permission/GetSystemPermissionsMvcRote",
                    //"UserId=" + currentUser.UserId + "&AppCode=" + appCode + "&Area=" + area + "&Controller=" + controller + "&Action=" + action);
                    //OperateStatus operateStatus = request.Deserialize<OperateStatus>();
                    //if (operateStatus.ResultSign == ResultSign.Error)
                    //{
                    //     //ErrorRedirect(filterContext, "/Error/Warn");
                    //}
                }
            }

            #endregion
        }

        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="context"></param>
        /// <param name="url">地址</param>
        private void ErrorRedirect(ActionExecutingContext context, string url)
        {
            context.Result = new RedirectResult(url);
        }

    }
}