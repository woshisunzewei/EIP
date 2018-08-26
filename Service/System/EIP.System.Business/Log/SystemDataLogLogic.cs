using EIP.Common.Business;
using EIP.System.DataAccess.Log;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Log
{
    public class SystemDataLogLogic : AsyncLogic<SystemDataLog>, ISystemDataLogLogic
    {
        #region ¹¹Ôìº¯Êý

        private readonly ISystemDataLogRepository _DataLogRepository;

        public SystemDataLogLogic(ISystemDataLogRepository DataLogRepository)
            : base(DataLogRepository)
        {
            _DataLogRepository = DataLogRepository;
        }

        #endregion
    }
}