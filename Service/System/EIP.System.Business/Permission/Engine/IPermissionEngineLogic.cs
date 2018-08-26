using System;
using System.Threading.Tasks;
using EIP.Common.Entities;

namespace EIP.System.Business.Permission.Engine
{
    /// <summary>
    /// 权限引擎接口
    /// </summary>
    public interface IPermissionEngineLogic
    {
        /// <summary>
        /// 根据Mvc规则及用户Id获取字段权限字符串
        /// </summary>
        /// <param name="rote">Mvc规则</param>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        string GetFiledPermissionStrByMvcRote(MvcRote rote, Guid userId);

        /// <summary>
        /// 根据指定的url路径获取字段权限字符串
        /// </summary>
        /// <param name="url">url地址:区域/控制器/方法</param>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        Task<string> GetFiledPermissionStrByUrl(string url, Guid userId);
    }
}