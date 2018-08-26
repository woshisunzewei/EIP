using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Resource;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.System.DataAccess.Config;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Config
{
    /// <summary>
    ///     系统配置文件接口实现
    /// </summary>
    public class SystemAppLogic : AsyncLogic<SystemApp>, ISystemAppLogic
    {
        #region 构造函数
        private readonly ISystemAppRepository _appRepository;
        public SystemAppLogic(ISystemAppRepository appRepository)
            : base(appRepository)
        {
            _appRepository = appRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     保存数据库配置
        /// </summary>
        /// <param name="app">数据库配置</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveApp(SystemApp app)
        {
            if (!app.AppId.IsEmptyGuid())
                return await UpdateAsync(app);
            app.AppId = CombUtil.NewComb();
            return await InsertAsync(app);
        }

        /// <summary>
        ///     检查代码是否具有重复
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckAppCode(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();
            if (await _appRepository.CheckAppCode(input))
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.HaveCode, input.Param);
            }
            else
            {
                operateStatus.ResultSign = ResultSign.Successful;
                operateStatus.Message = Chs.CheckSuccessful;
            }
            return operateStatus;
        }
        #endregion
    }
}