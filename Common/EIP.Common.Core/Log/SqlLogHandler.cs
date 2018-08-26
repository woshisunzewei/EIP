using System;
using System.Web;
using EIP.Common.Core.Auth;
using EIP.Common.Core.Utils;

namespace EIP.Common.Core.Log
{
    public class SqlLogHandler : BaseHandler<SqlLog>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlLogHandler(string operateSql,
            DateTime endDateTime,
            double elapsedTime,
            string parameter
            )
            : base("SqlLogToDatabase")
        {
            PrincipalUser principalUser = new PrincipalUser
            {
                Name = "匿名用户",
                UserId = Guid.Empty
            };
            var current = HttpContext.Current;
            if (current != null)
            {
                principalUser = FormAuthenticationExtension.Current(HttpContext.Current.Request);
            }
            if (principalUser == null)
            {
                principalUser = new PrincipalUser()
                {
                    Name = "匿名用户",
                    UserId = Guid.Empty
                };
            }
            log = new SqlLog
            {
                SqlLogId = CombUtil.NewComb(),
                CreateTime = DateTime.Now,
                CreateUserId = principalUser.UserId,
                CreateUserCode = principalUser.Code,
                CreateUserName = principalUser.Name,
                OperateSql = operateSql,
                ElapsedTime = elapsedTime,
                EndDateTime = endDateTime,
                Parameter = parameter
            };
        }
    }
}