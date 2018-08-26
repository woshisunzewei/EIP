using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Permission
{
    public interface ISystemMenuLogic : IAsyncLogic<SystemMenu>
    {
        #region 菜单

        /// <summary>
        ///     根据状态为True的菜单信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetAllPortalMenu();

        /// <summary>
        ///     根据状态为True的菜单信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetAllMenu();

        /// <summary>
        ///     查询所有具有菜单权限的菜单
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetHaveMenuPermissionMenu();

        /// <summary>
        ///     查询所有具有数据权限的菜单
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetHaveDataPermissionMenu();

        /// <summary>
        ///     查询所有具有字段权限的菜单
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetHaveFieldPermissionMenu();

        /// <summary>
        ///     查询所有具有功能项的菜单
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TreeEntity>> GetHaveMenuButtonPermissionMenu();

        /// <summary>
        ///     根据状态为True的菜单信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemMenu>> GetMeunuByPId(IdInput input);

        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="menu">父级id</param>
        /// <returns></returns>
        Task<OperateStatus<Guid>> SaveMenu(SystemMenu menu);

        /// <summary>
        ///     删除菜单及下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        Task<OperateStatus> DeleteMenu(IdInput input);

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        Task<OperateStatus> CheckMenuCode(CheckSameValueInput input);

        /// <summary>
        ///     批量生成代码
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> GeneratingCode();
        #endregion
    }
}