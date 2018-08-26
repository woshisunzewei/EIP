using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EIP.Common.Core.Extensions;
using EIP.Common.Dapper;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Identity
{
    public class SystemGroupRepository : DapperAsyncRepository<SystemGroup>, ISystemGroupRepository
    {
        /// <summary>
        ///     查询归属某组织下的组信息
        /// </summary>
        /// <param name="input">组织机构Id</param>
        /// <returns>组信息</returns>
        public Task<IEnumerable<SystemGroupOutput>> GetGroupByOrganizationId(NullableIdInput input)
        {
            var sql = new StringBuilder();
            sql.Append(
                @"SELECT gr.GroupId,gr.Name,gr.BelongTo,gr.BelongToUserId,gr.[State],gr.IsFreeze,gr.OrderNo,gr.Remark,gr.CreateTime,gr.CreateUserName,gr.UpdateTime,gr.UpdateUserName,org.OrganizationId,org.Name OrganizationName
                         FROM System_Group gr LEFT JOIN System_Organization org ON org.OrganizationId=gr.OrganizationId");
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql.Append(" WHERE gr.OrganizationId=@orgId");
            }
            sql.Append(" ORDER BY gr.OrganizationId");
            return SqlMapperUtil.SqlWithParams<SystemGroupOutput>(sql.ToString(), new {orgId = input.Id});
        }

        /// <summary>
        ///     检查代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        public Task<bool> CheckGroupCode(CheckSameValueInput input)
        {
            var sql = "SELECT GroupId FROM System_Group WHERE Code=@param";
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql += " AND GroupId!=@groupId";
            }
            return SqlMapperUtil.SqlWithParamsBool<SystemGroup>(sql, new { param = input.Param, groupId = input.Id });
        }
    }
}