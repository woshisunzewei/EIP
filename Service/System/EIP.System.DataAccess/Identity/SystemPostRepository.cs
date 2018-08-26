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
    public class SystemPostRepository : DapperAsyncRepository<SystemPost>, ISystemPostRepository
    {
        /// <summary>
        ///     查询归属某组织下的岗位信息
        /// </summary>
        /// <param name="input">组织机构PostId</param>
        /// <returns>岗位信息</returns>
        public Task<IEnumerable<SystemPostOutput>> GetPostByOrganizationId(NullableIdInput input)
        {
            var sql = new StringBuilder();
            sql.Append(
                @"SELECT post.PostId,post.Name,post.MainSupervisor,post.MainSupervisorContact,post.State,post.OrderNo,post.Remark,post.CreateTime,post.CreateUserId,post.CreateUserName,post.IsFreeze,post.PostId,org.OrganizationId,org.Name OrganizationName
                         FROM System_Post post LEFT JOIN System_Organization org ON org.OrganizationId=post.OrganizationId");
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql.Append(" WHERE post.OrganizationId=@orgId");
            }
            sql.Append(" ORDER BY post.OrganizationId");
            return SqlMapperUtil.SqlWithParams<SystemPostOutput>(sql.ToString(), new
            {
                orgId = input.Id
            });
        }

        /// <summary>
        ///     检查代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        public Task<bool> CheckPostCode(CheckSameValueInput input)
        {
            var sql = "SELECT PostId FROM System_Post WHERE Code=@param";
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql += " AND PostId!=@postId";
            }
            return SqlMapperUtil.SqlWithParamsBool<SystemPost>(sql, new
            {
                param = input.Param,
                postId = input.Id
            });
        }
    }
}