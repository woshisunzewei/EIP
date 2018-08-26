using System.Linq;
using System.Web.Mvc;
using EIP.Common.Core.Extensions;

namespace EIP.Common.Core.Attributes
{
    /// <summary>
    /// 防止Sql注入特性
    /// </summary>
    public class AntiSqlInjectAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext) { }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionParameters = filterContext.ActionDescriptor.GetParameters();
            foreach (var p in actionParameters.Where(p => p.ParameterType == typeof(string)).Where(p => filterContext.ActionParameters[p.ParameterName] != null))
            {
                //参数过滤
                filterContext.ActionParameters[p.ParameterName] = (filterContext.ActionParameters[p.ParameterName].ToString()).FilterSql();
            }
        }
    }
}