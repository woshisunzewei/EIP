using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Config
{
    /// <summary>
    /// 字典数据访问接口
    /// </summary>
    public interface ISystemDictionaryRepository : IAsyncRepository<SystemDictionary>
    {
        /// <summary>
        ///     根据所有字段信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetDictionaryTree();

        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemDictionary>> GetDictionariesParentId(IdInput input);

        /// <summary>
        ///     根据字典代码获取对应下级值
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns></returns>
        Task<IEnumerable<SystemDictionary>> GetDictionaryByCode(string code);

        /// <summary>
        ///     根据字典代码获取字典信息
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns></returns>
        Task<SystemDictionary> GetThisDictionaryByCode(string code);

        /// <summary>
        ///     根据代码获取值
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetDictionaryTreeByCode(string code);

        /// <summary>
        ///     检查字典代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        Task<bool> CheckDictionaryCode(CheckSameValueInput input);
    }
}