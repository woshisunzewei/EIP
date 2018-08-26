using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Config
{
    /// <summary>
    ///     系统字典业务逻辑接口
    /// </summary>
    public interface ISystemDictionaryLogic : IAsyncLogic<SystemDictionary>
    {
        /// <summary>
        ///     查询所有字典信息:Ztree格式
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetDictionaryTree();

        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        Task<IEnumerable<SystemDictionary>> GetDictionariesParentId(IdInput input);

        /// <summary>
        /// 根据代码获取字典树
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetDictionaryTreeByCode(string code);

            /// <summary>
        ///     保存字典信息
        /// </summary>
        /// <param name="dictionary">字典信息</param>
        /// <returns></returns>
        Task<OperateStatus> SaveDictionary(SystemDictionary dictionary);

        /// <summary>
        ///     删除字典及下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        Task<OperateStatus> DeleteDictionary(IdInput input);

        /// <summary>
        ///     根据字典代码获取对应下级值
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns></returns>
        Task<IEnumerable<SystemDictionary>> GetDictionaryByCode(string code);

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        Task<OperateStatus> CheckDictionaryCode(CheckSameValueInput input);

        /// <summary>
        ///     批量生成代码
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> GeneratingCode();

    }
}