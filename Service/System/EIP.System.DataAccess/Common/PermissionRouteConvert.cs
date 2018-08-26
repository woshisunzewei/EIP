using EIP.Common.Entities;
using EIP.System.Models.Enums;

namespace EIP.System.DataAccess.Common
{
    /// <summary>
    /// 权限路由转换
    /// </summary>
    public class PermissionRouteConvert
    {
        /// <summary>
        /// 路由转换
        /// </summary>
        /// <param name="roteConvert">转换类型枚举</param>
        public static MvcRote RoteConvert(EnumPermissionRoteConvert roteConvert)
        {
            MvcRote mvcRote = new MvcRote();
            switch (roteConvert)
            {
                case EnumPermissionRoteConvert.用户字段数据权限:
                    mvcRote.AppCode = "Solution";
                    mvcRote.Area = "System";
                    mvcRote.Controller = "User";
                    mvcRote.Action = "List";
                    break;
            }
            return mvcRote;
        }
    }
}