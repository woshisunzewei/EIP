using EIP.Common.Business;
using EIP.System.DataAccess.Log;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Log
{
    public class SystemSqlLogLogic : AsyncLogic<SystemSqlLog>, ISystemSqlLogLogic
    {
        #region ¹¹Ôìº¯Êý

        private readonly ISystemSqlLogRepository _SqlLogRepository;

        public SystemSqlLogLogic(ISystemSqlLogRepository SqlLogRepository)
            : base(SqlLogRepository)
        {
            _SqlLogRepository = SqlLogRepository;
        }

        #endregion
    }
}