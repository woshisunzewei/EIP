using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Extensions;
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
    public class SystemRoleLogic : AsyncLogic<SystemRole>, ISystemRoleLogic
    {
        #region 构造函数

        private readonly ISystemRoleRepository _roleRepository;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemPermissionLogic _permissionLogic;

        public SystemRoleLogic(ISystemRoleRepository roleRepository,
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemPermissionLogic permissionLogic)
            : base(roleRepository)
        {
            _permissionUserLogic = permissionUserLogic;
            _permissionLogic = permissionLogic;
            _roleRepository = roleRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据组织机构Id查询角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isFreeze">是否冻结</param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemRoleOutput>> GetRolesByOrganizationId(NullableIdInput input = null,
            bool? isFreeze = null)
        {
            return await _roleRepository.GetRolesByOrganizationId(input, isFreeze);
        }

        /// <summary>
        ///     检测配置项代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckRoleCode(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();
            if ( await _roleRepository.CheckPostCode(input))
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
        ///     保存岗位信息
        /// </summary>
        /// <param name="role">岗位信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveRole(SystemRole role)
        {
            role.CanbeDelete = true;
            if (role.RoleId.IsEmptyGuid())
            {
                role.CreateTime = DateTime.Now;
                role.RoleId = Guid.NewGuid();
                return await InsertAsync(role);
            }
            var systemRole =await GetByIdAsync(role.RoleId);
            role.CreateTime = systemRole.CreateTime;
            role.CreateUserId = systemRole.CreateUserId;
            role.CreateUserName = systemRole.CreateUserName;
            role.UpdateTime = DateTime.Now;
            role.UpdateUserId = role.CreateUserId;
            role.UpdateUserName = role.CreateUserName;
            return await UpdateAsync(role);
        }

        /// <summary>
        ///     获取该用户已经具有的角色信息
        /// </summary>
        /// <param name="privilegeMaster"></param>
        /// <param name="userId">需要查询的用户id</param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemRole>> GetHaveUserRole(EnumPrivilegeMaster privilegeMaster,
            Guid userId)
        {
            return await _roleRepository.GetHaveUserRole(privilegeMaster, userId);
        }

        /// <summary>
        ///     删除角色信息
        /// </summary>
        /// <param name="input">角色Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteRole(IdInput input)
        {
            var operateStatus = new OperateStatus();
            //判断是否具有人员
            var permissionUsers =await 
                _permissionUserLogic.GetPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.角色,
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
                        PrivilegeMaster = EnumPrivilegeMaster.角色
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
                        PrivilegeMaster = EnumPrivilegeMaster.角色
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