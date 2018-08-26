using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Core.Resource;
using EIP.System.DataAccess.Permission;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;

namespace EIP.System.Business.Permission
{
    /// <summary>
    ///     数据权限规则业务逻辑
    /// </summary>
    public class SystemDataLogic : AsyncLogic<SystemData>, ISystemDataLogic
    {
        #region 构造函数

        private readonly ISystemDataRepository _dataRepository;
        private readonly ISystemPermissionRepository _permissionRepository;

        public SystemDataLogic(ISystemDataRepository dataRepository, ISystemPermissionRepository permissionRepository)
            : base(dataRepository)
        {
            _dataRepository = dataRepository;
            _permissionRepository = permissionRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据菜单Id获取数据权限规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async  Task<IEnumerable<SystemDataDoubleWayDto>> GetDataByMenuId(NullableIdInput<Guid> input)
        {
            return await _dataRepository.GetDataByMenuId(input);
        }

        /// <summary>
        ///     保存数据权限规则
        /// </summary>
        /// <param name="data">数据权限规则</param>
        /// <returns></returns>
        public async  Task<OperateStatus> SaveData(SystemDataDoubleWayDto data)
        {
            SystemData systemData = data.MapTo<SystemData>();
            if (data.DataId.IsEmptyGuid())
            {
                systemData.DataId = CombUtil.NewComb();
                return await InsertAsync(systemData);
            }
            return await UpdateAsync(systemData);
        }

        /// <summary>
        ///     删除数据权限规则信息
        /// </summary>
        /// <param name="input">数据权限规则Id</param>
        /// <returns></returns>
        public async  Task<OperateStatus> DeleteByDataId(IdInput input)
        {
            var operateStatus = new OperateStatus();
            //查看该功能项是否已被特性占用
            var permissions =await _permissionRepository.GetSystemPermissionsByPrivilegeAccessAndValue(EnumPrivilegeAccess.数据权限, input.Id);
            if (permissions.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.已被赋予权限);
                return operateStatus;
            }
            return await DeleteAsync(input.Id);
        }

        #endregion
    }
}