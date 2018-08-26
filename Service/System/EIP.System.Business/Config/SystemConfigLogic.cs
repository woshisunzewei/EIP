using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Config;
using EIP.Common.Core.Log;
using EIP.Common.Core.Resource;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.System.DataAccess.Config;
using EIP.System.Models.Dtos.Config;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Config
{
    /// <summary>
    ///     系统配置文件接口实现
    /// </summary>
    public class SystemConfigLogic : AsyncLogic<SystemConfig>, ISystemConfigLogic
    {
        #region 构造函数

        private readonly ISystemConfigRepository _configRepository;

        public SystemConfigLogic(ISystemConfigRepository configRepository)
            : base(configRepository)
        {
            _configRepository = configRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     保存系统配置项信息
        /// </summary>
        /// <param name="doubleWays">系统配置项信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveConfig(IEnumerable<SystemConfigDoubleWay> doubleWays)
        {
            OperateStatus operateStatus = new OperateStatus();
            //更新
            try
            {
                foreach (var config in doubleWays)
                {
                    config.V = DEncryptUtil.HttpUtilityUrlEncode(config.V);
                    //更新对应值
                    var c = await GetByIdAsync(config.C);
                    c.Value = config.V;
                    if (await _configRepository.UpdateAsync(c) > 0)
                    {
                        GlobalParams.Set(c.Code, config.V);
                    }
                }
                operateStatus.Message = Chs.Successful;
                operateStatus.ResultSign = ResultSign.Successful;
            }
            catch (Exception e)
            {
                LogWriter.WriteLog(FolderName.Exception, e);
            }
            return operateStatus;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SystemConfigDoubleWay>> GetConfig()
        {
            return _configRepository.GetConfig();
        }
        #endregion
    }
}