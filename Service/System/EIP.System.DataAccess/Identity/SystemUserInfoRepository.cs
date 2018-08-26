using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EIP.Common.Core.Extensions;
using EIP.Common.Dapper;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Paging;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;

namespace EIP.System.DataAccess.Identity
{
    public class SystemUserInfoRepository : DapperAsyncRepository<SystemUserInfo>, ISystemUserInfoRepository
    {
        /// <summary>
        ///     根据用户名和密码查询用户信息
        ///     1:用户登录使用
        /// </summary>
        /// <param name="input">登录名、密码等</param>
        /// <returns></returns>
        public Task<SystemUserOutput> CheckUserByCodeAndPwd(UserLoginInput input)
        {
            var sql = new StringBuilder();
            sql.Append(@"SELECT UserId,userInfo.Code,userInfo.Name,FirstVisitTime,userInfo.IsFreeze,userInfo.State,org.Name OrganizationName,org.OrganizationId FROM [System_UserInfo] userInfo
                         LEFT JOIN System_PermissionUser perUser on userInfo.UserId=perUser.PrivilegeMasterUserId and perUser.PrivilegeMaster=1
                         LEFT JOIN System_Organization org on perUser.PrivilegeMasterValue=org.OrganizationId ");
            sql.Append("WHERE userInfo.Code=@code AND userInfo.Password=@pwd");
            return SqlMapperUtil.SqlWithParamsSingle<SystemUserOutput>(sql.ToString(),
                new
                {
                    code = input.Code,
                    pwd = input.Pwd
                });
        }

        /// <summary>
        ///     更新最后登录时间
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        public Task<int> UpdateLastLoginTime(IdInput input)
        {
            const string sql = @"UPDATE [System_UserInfo] SET LastVisitTime=getdate() WHERE UserId=@userId";
            return SqlMapperUtil.InsertUpdateOrDeleteSql<SystemUserInfo>(sql, new { userId = input.Id });
        }

        /// <summary>
        ///     更新第一次登录时间
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        public Task<int> UpdateFirstVisitTime(IdInput input)
        {
            const string sql = @"UPDATE [System_UserInfo] SET FirstVisitTime=getdate() WHERE UserId=@userId";
            return SqlMapperUtil.InsertUpdateOrDeleteSql<SystemUserInfo>(sql, new { userId = input.Id });
        }

        /// <summary>
        ///     复杂查询分页方式
        /// </summary>
        /// <param name="paging">查询参数</param>
        /// <returns>分页</returns>
        public Task<PagedResults<SystemUserOutput>> PagingUserQuery(SystemUserPagingInput paging)
        {
            StringBuilder sql = new StringBuilder(2000);
            if (paging.PrivilegeMaster == EnumPrivilegeMaster.组织机构)
            {
                sql.Append(
                    string.Format(@"SELECT {1}, @rowNumber, @recordCount 
                                FROM [System_UserInfo] u 
                                LEFT JOIN System_PermissionUser pu ON pu.PrivilegeMasterUserId=u.UserId  
                                @where  AND pu.PrivilegeMaster='{0}' AND u.IsAdmin='false' ", (int)paging.PrivilegeMaster, paging.FiledSql));
            }
            else
            {
                sql.Append(
                    string.Format(@"SELECT u.UserId,
                                       u.Code,
                                       u.Name,
                                       u.Mobile,
                                       u.FirstVisitTime,
                                       u.LastVisitTime,
                                       u.IsFreeze,
                                       u.Remark,
                                       u.State,
                                       u.CreateTime,
                                       u.CreateUserName,
                                       u.UpdateTime,
                                       u.UpdateUserName,

                                --组织机构名称
                                (SELECT Name FROM dbo.System_Organization org WHERE 
                                org.OrganizationId=(SELECT PrivilegeMasterValue FROM dbo.System_PermissionUser 
                                WHERE PrivilegeMaster=1 AND PrivilegeMasterUserId=u.UserId))
                                OrganizationName, 

                                --组织机构Id
                                (SELECT OrganizationId FROM dbo.System_Organization org WHERE 
                                org.OrganizationId=(SELECT PrivilegeMasterValue FROM dbo.System_PermissionUser 
                                WHERE PrivilegeMaster=1 AND PrivilegeMasterUserId=u.UserId)) OrganizationId,
                                @rowNumber, @recordCount 
                                FROM [System_UserInfo] u 
                                LEFT JOIN System_PermissionUser pu ON pu.PrivilegeMasterUserId=u.UserId  
                                @where  AND pu.PrivilegeMaster='{0}' AND u.IsAdmin='false' AND pu.PrivilegeMasterValue='{1}' ", (int)paging.PrivilegeMaster,
                                                                                                                             paging.PrivilegeMasterValue));
            }
            if (!paging.DataSql.IsNullOrEmpty())
            {
                sql.Append(string.Format(" AND {0}", paging.DataSql));
            }
            //获取对应字段信息并进行对应替换
            return PagingQueryAsync<SystemUserOutput>(sql.ToString(), paging);
        }

        /// <summary>
        ///     检查代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        public Task<bool> CheckUserCode(CheckSameValueInput input)
        {
            var sql = "SELECT UserId FROM System_UserInfo WHERE Code=@param";
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql += " AND UserId!=@userId";
            }
            return SqlMapperUtil.SqlWithParamsBool<SystemUserInfo>(sql, new { param = input.Param, userId = input.Id });
        }

        /// <summary>
        ///     获取所有用户
        /// </summary>
        /// <param name="input">是否冻结</param>
        /// <returns></returns>
        public Task<IEnumerable<SystemChosenUserOutput>> GetChosenUser(FreezeInput input = null)
        {
            var sql = new StringBuilder(
                string.Format(
                    @"SELECT u.UserId,u.Code,u.Name,perUser.PrivilegeMasterValue OrganizationId FROM System_UserInfo u
                               INNER JOIN System_PermissionUser perUser ON perUser.PrivilegeMasterUserId=u.UserId AND perUser.PrivilegeMaster={0} AND u.IsAdmin='False'",
                    (int)EnumPrivilegeMaster.组织机构));
            if (input != null)
            {
                sql.Append(string.Format(" u.IsFreeze='{0}'", input.IsFreeze));
            }
            return SqlMapperUtil.SqlWithParams<SystemChosenUserOutput>(sql.ToString());
        }

        /// <summary>
        ///     根据用户Id重置某人密码
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        public Task<bool> ResetPassword(ResetPasswordInput input)
        {
            const string sql = "UPDATE System_UserInfo SET Password=@password WHERE UserId=@userId";
            return SqlMapperUtil.InsertUpdateOrDeleteSqlBool<SystemUserInfo>(sql, new
            {
                userId = input.Id,
                password = input.EncryptPassword
            });
        }

        /// <summary>
        ///     检查代码:唯一性检查
        /// </summary>
        /// <param name="input">代码</param>
        /// <returns></returns>
        public Task<bool> CheckOldPassword(CheckSameValueInput input)
        {
            const string sql = "SELECT UserId FROM System_UserInfo WHERE Password=@param AND UserId=@userId";
            return SqlMapperUtil.SqlWithParamsBool<SystemUserInfo>(sql, new { param = input.Param, userId = input.Id });
        }

        /// <summary>
        ///     获取所有用户
        /// </summary>
        /// <param name="input">是否冻结</param>
        /// <returns></returns>
        public Task<IEnumerable<SystemUserInfo>> GetUser(FreezeInput input = null)
        {
            var sql = new StringBuilder(@"SELECT u.UserId,u.Code,u.Name FROM System_UserInfo u WHERE u.IsAdmin='False'");
            if (input != null)
            {
                sql.Append(string.Format(" AND u.IsFreeze='{0}'", input.IsFreeze));
            }
            return SqlMapperUtil.SqlWithParams<SystemUserInfo>(sql.ToString());
        }
    }
}