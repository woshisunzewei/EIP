using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Tree;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;

namespace EIP.System.DataAccess.Permission
{
    public interface ISystemPermissionRepository : IAsyncRepository<SystemPermission>
    {
        /// <summary>
        ///     根据权限归属Id查询菜单权限信息
        /// </summary>
        /// <param name="input">权限类型:菜单、功能项、数据、字段、文件</param>
        /// <returns></returns>
        Task<IEnumerable<SystemPermission>> GetPermissionByPrivilegeMasterValue(GetPermissionByPrivilegeMasterValueInput input);

        /// <summary>
        ///     根据权限归属Id删除菜单权限信息
        /// </summary>
        /// <param name="privilegeAccess">权限类型:菜单、功能项、数据、字段、文件</param>
        /// <param name="privilegeMasterValue"></param>
        /// <param name="privilegeMenuId"></param>
        /// <returns></returns>
        Task<bool> DeletePermissionByPrivilegeMasterValue(EnumPrivilegeAccess? privilegeAccess,
            Guid privilegeMasterValue,
            Guid? privilegeMenuId);

        /// <summary>
        ///     根据用户Id获取用户具有的菜单权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetSystemPermissionMenuByUserId(Guid userId);

        /// <summary>
        ///     根据角色Id,岗位Id,组Id,人员Id获取具有的菜单信息
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>树形菜单信息</returns>
        Task<IEnumerable<TreeEntity>> GetMenuHavePermissionByPrivilegeMasterValue(GetMenuHavePermissionByPrivilegeMasterValueInput input);

        /// <summary>
        /// 获取菜单、功能项等被使用的权限信息
        /// </summary>
        /// <param name="privilegeAccess">类型:菜单、功能项</param>
        /// <param name="privilegeAccessValue">对应值</param>
        /// <returns></returns>
        Task<IEnumerable<SystemPermission>> GetSystemPermissionsByPrivilegeAccessAndValue(EnumPrivilegeAccess privilegeAccess,
            Guid? privilegeAccessValue = null);

        /// <summary>
        /// 根据功能项获取权限信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemPermission>> GetSystemPermissionsMvcRote(GetSystemPermissionsMvcRoteInput input);

        /// <summary>
        /// 获取字段权限Sql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemField>> GetFieldPermission(SystemPermissionSqlInput input);

        /// <summary>
        /// 获取数据权限Sql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemData>> GetDataPermission(SystemPermissionSqlInput input);
    }
}