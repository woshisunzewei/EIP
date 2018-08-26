using System.Web.Mvc;
using System.Web.Routing;

namespace EIP.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                 "Default", // 路由名称
                 "{controller}/{action}/{id}", // 带有参数的 URL
                 new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // 参数默认值
                 new[] { "Home.Controllers" } //默认控制器的命名空间
             ).DataTokens.Add("area", "Console"); //默认area 的控制器名称
        }
    }
}
