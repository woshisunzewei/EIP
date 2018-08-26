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
using EIP.System.Business.Permission;
using EIP.System.DataAccess.Identity;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;

namespace EIP.System.Business.Identity
{
    public class SystemGroupLogic : AsyncLogic<SystemGroup>, ISystemGroupLogic
    {
        #region 构造函数

        private readonly ISystemGroupRepository _groupRepository;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemPermissionLogic _permissionLogic;

        public SystemGroupLogic(ISystemGroupRepository groupRepository,
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemPermissionLogic permissionLogic)
            : base(groupRepository)
        {
            _permissionUserLogic = permissionUserLogic;
            _permissionLogic = permissionLogic;
            _groupRepository = groupRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据组织机构获取组信息
        /// </summary>
        /// <param name="input">组织机构</param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemGroupOutput>> GetGroupByOrganizationId(NullableIdInput input)
        {
            var groups =(await _groupRepository.GetGroupByOrganizationId(input)).ToList();

            //获取组织机构信息
            foreach (var group in groups)
            {
                group.BelongToName = EnumUtil.GetName(typeof (EnumGroupBelongTo), group.BelongTo);
            }
            return groups;
        }

        /// <summary>
        ///     检测配置项代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckGroupCode(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();
            if (await _groupRepository.CheckGroupCode(input))
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

        /// <summary>
        ///     保存组信息
        /// </summary>
        /// <param name="group">岗位信息</param>
        /// <param name="belongTo">归属</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveGroup(SystemGroup group,
            EnumGroupBelongTo belongTo)
        {
            group.BelongTo = (byte) belongTo;
            if (group.GroupId.IsEmptyGuid())
            {
                group.CreateTime = DateTime.Now;
                group.GroupId = CombUtil.NewComb();
                return await InsertAsync(group);
            }
            var systemGroup =await GetByIdAsync(group.GroupId);
            group.CreateTime = systemGroup.CreateTime;
            group.CreateUserId = systemGroup.CreateUserId;
            group.CreateUserName = systemGroup.CreateUserName;
            group.UpdateTime = DateTime.Now;
            group.UpdateUserId = group.CreateUserId;
            group.UpdateUserName = group.CreateUserName;
            return await UpdateAsync(group);
        }

        /// <summary>
        ///     删除组信息
        /// </summary>
        /// <param name="input">组Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteGroup(IdInput input)
        {
            var operateStatus = new OperateStatus();
            //判断是否具有人员
            var permissionUsers =await 
                _permissionUserLogic.GetPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.组,
                    input.Id);
            if (permissionUsers.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format( Chs.Error, ResourceSystem.具有人员);
                return operateStatus;
            }
            //判断是否具有按钮权限
            var functionPermissions =await 
                _permissionLogic.GetPermissionByPrivilegeMasterValue(
                    new GetPermissionByPrivilegeMasterValueInput
                    {
                        PrivilegeAccess = EnumPrivilegeAccess.菜单按钮,
                        PrivilegeMasterValue = input.Id,
                        PrivilegeMaster = EnumPrivilegeMaster.组
                    });
            if (functionPermissions.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format( Chs.Error, ResourceSystem.具有功能项权限);
                return operateStatus;
            }
            //判断是否具有菜单权限
            var menuPermissions =await 
                _permissionLogic.GetPermissionByPrivilegeMasterValue(
                    new GetPermissionByPrivilegeMasterValueInput
                    {
                        PrivilegeAccess = EnumPrivilegeAccess.菜单,
                        PrivilegeMasterValue = input.Id,
                        PrivilegeMaster = EnumPrivilegeMaster.组
                    });

            if (menuPermissions.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format( Chs.Error, ResourceSystem.具有菜单权限);
                return operateStatus;
            }
            return await DeleteAsync(input.Id);
        }

        #endregion
    }
}