using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EIP.Common.Core.Extensions;
using EIP.Common.Dapper;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Config
{
    /// <summary>
    ///     字典数据访问接口实现
    /// </summary>
    public class SystemDictionaryRepository : DapperAsyncRepository<SystemDictionary>, ISystemDictionaryRepository
    {
        /// <summary>
        ///     根据所有字段信息
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetDictionaryTree()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT DictionaryId id,ParentId pId,name,code FROM System_Dictionary ORDER BY OrderNo");
            return  SqlMapperUtil.SqlWithParams<TreeEntity>(sql.ToString());
        }

        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemDictionary>> GetDictionariesParentId(IdInput input)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT * FROM System_Dictionary WHERE ParentId=@pId ORDER BY OrderNo");
            return  SqlMapperUtil.SqlWithParams<SystemDictionary>(sql.ToString(), new { pId = input.Id });
        }

        /// <summary>
        ///     根据字典代码获取对应下级值
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns></returns>
        public Task<IEnumerable<SystemDictionary>> GetDictionaryByCode(string code)
        {
            var sql = new StringBuilder();
            sql.Append(
                "SELECT DictionaryId,Name FROM dbo.System_Dictionary WHERE ParentId IN (SELECT DictionaryId FROM dbo.System_Dictionary WHERE Code=@code) ORDER BY dbo.System_Dictionary.OrderNo");
            return  SqlMapperUtil.SqlWithParams<SystemDictionary>(sql.ToString(), new { code });
        }

        /// <summary>
        ///     检查字典代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        public Task<bool> CheckDictionaryCode(CheckSameValueInput input)
        {
            var sql = "SELECT DictionaryId FROM System_Dictionary WHERE Code=@param";
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql += " AND DictionaryId!=@dictionaryId";
            }
            return SqlMapperUtil.SqlWithParamsBool<SystemDictionary>(sql, new
            {
                param = input.Param,
                dictionaryId = input.Id
            });
        }

        /// <summary>
        ///     根据字典代码获取字典信息
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns></returns>
        public Task<SystemDictionary> GetThisDictionaryByCode(string code)
        {
            const string sql = "SELECT * FROM System_Dictionary WHERE Code=@param";
            return  SqlMapperUtil.SqlWithParamsSingle<SystemDictionary>(sql, new
            {
                param = code
            });
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetDictionaryTreeByCode(string code)
        {
            var sql = new StringBuilder();
            sql.Append(@"SELECT DictionaryId id,ParentId pId,name,code FROM System_Dictionary WHERE Code like '" + (code + "_").Replace("_", @"\_") + "%" + "' escape '\\' OR Code ='" + code + "' ORDER BY OrderNo");
            return  SqlMapperUtil.SqlWithParams<TreeEntity>(sql.ToString());
        }
    }
}