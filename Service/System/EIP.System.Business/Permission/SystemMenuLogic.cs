using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.Common.Core.Resource;
using EIP.Common.Core.Utils;
using EIP.System.DataAccess.Permission;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;

namespace EIP.System.Business.Permission
{
    public class SystemMenuLogic : AsyncLogic<SystemMenu>, ISystemMenuLogic
    {
        #region 构造函数

        private readonly ISystemMenuRepository _menuRepository;
        private readonly ISystemPermissionLogic _permissionLogic;

        public SystemMenuLogic(ISystemMenuRepository menuRepository,
            ISystemPermissionLogic permissionLogic)
            : base(menuRepository)
        {
            _permissionLogic = permissionLogic;
            _menuRepository = menuRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据状态为True的菜单信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetAllPortalMenu()
        {
            return await _menuRepository.GetAllPortalMenu();
        }

        /// <summary>
        ///     根据状态为True的菜单信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetAllMenu()
        {
            return await _menuRepository.GetAllMenu();
        }

        /// <summary>
        ///     根据状态为True的菜单信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SystemMenu>> GetMeunuByPId(IdInput input)
        {
            return await _menuRepository.GetMeunuByPId(input);
        }

        /// <summary>
        ///     保存菜单
        /// </summary>
        /// <param name="menu">菜单信息</param>
        /// <returns></returns>
        public async Task<OperateStatus<Guid>> SaveMenu(SystemMenu menu)
        {
            OperateStatus<Guid> operateStatus = new OperateStatus<Guid>();
            OperateStatus result;
            menu.CanbeDelete = true;
            if (menu.MenuId.IsEmptyGuid())
            {
                menu.MenuId = CombUtil.NewComb();
                result = await InsertAsync(menu);
            }
            else
            {
                result = await UpdateAsync(menu);
            }
            if (result.ResultSign != ResultSign.Successful) return operateStatus;
            operateStatus.ResultSign = result.ResultSign;
            operateStatus.Message = result.Message;
            operateStatus.Data = menu.MenuId;
            return operateStatus;
        }

        /// <summary>
        ///     删除菜单及下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteMenu(IdInput input)
        {
            var operateStatus = new OperateStatus();

            //判断该项能否进行删除
            var menu = await GetByIdAsync(input.Id);

            if (menu != null && !menu.CanbeDelete)
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = Chs.CanotDelete;
                return operateStatus;
            }
            //查看是否具有下级
            if ((await GetMeunuByPId(input)).Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.具有下级项);
                return operateStatus;
            }
            //查看该菜单是否已被特性占用
            var permissions = await _permissionLogic.GetSystemPermissionsByPrivilegeAccessAndValue(EnumPrivilegeAccess.菜单, input.Id);
            if (permissions.Any())
            {
                //获取被占用类型及值
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.已被赋予权限);
                return operateStatus;
            }
            return await DeleteAsync(input.Id);
        }

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckMenuCode(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();
            if (await _menuRepository.CheckMenuCode(input))
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
        ///     查询所有具有菜单权限的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetHaveMenuPermissionMenu()
        {
            return await _menuRepository.GetHaveMenuPermissionMenu();
        }

        /// <summary>
        ///     查询所有具有数据权限的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetHaveDataPermissionMenu()
        {
            return await _menuRepository.GetHaveDataPermissionMenu();
        }

        /// <summary>
        ///     查询所有具有字段权限的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetHaveFieldPermissionMenu()
        {
            return await _menuRepository.GetHaveFieldPermissionMenu();
        }

        /// <summary>
        ///     查询所有具有功能项的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetHaveMenuButtonPermissionMenu()
        {
            return await _menuRepository.GetHaveMenuButtonPermissionMenu();
        }

        /// <summary>
        ///     批量生成代码
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> GeneratingCode()
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                //获取所有菜单
                var menus = (await GetAllEnumerableAsync()).ToList();
                var topMenu = menus.Where(w => w.ParentId == Guid.Empty);
                foreach (var menu in topMenu)
                {
                    menu.Code = PinYinUtil.GetFirst(menu.Name);
                    await UpdateAsync(menu);
                    await GeneratingCodeRecursion(menu, menus.ToList(), "");
                }
            }
            catch (Exception ex)
            {
                operateStatus.Message = string.Format(Chs.Error, ex.Message);
                return operateStatus;
            }
            operateStatus.Message = Chs.Successful;
            operateStatus.ResultSign = ResultSign.Successful;
            return operateStatus;
        }

        /// <summary>
        /// 递归获取代码
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="menus"></param>
        /// <param name="generationCode"></param>
        private async Task GeneratingCodeRecursion(SystemMenu menu, IList<SystemMenu> menus, string generationCode)
        {
            string emp = PinYinUtil.GetFirst(menu.Name);
            //获取下级
            var nextMenu = menus.Where(w => w.ParentId == menu.MenuId).ToList();
            if (nextMenu.Any())
            {
                emp = generationCode.IsNullOrEmpty() ? emp : generationCode + "_" + emp;
            }
            foreach (var me in nextMenu)
            {
                me.Code = emp + "_" + PinYinUtil.GetFirst(me.Name);
                await UpdateAsync(me);
                await GeneratingCodeRecursion(me, menus, emp);
            }
        }

        #region 级联删除Demo

        ///// <summary>
        /////     删除菜单及下级数据
        ///// </summary>
        ///// <param name="menuId">父级id</param>
        ///// <returns></returns>
        //public OperateStatus DeleteMenu(Guid menuId)
        //{
        //    var operateStatus = new OperateStatus();

        //    //判断该项能否进行删除
        //    var menu = GetById(menuId);
        //    if (!menu.CanbeDelete)
        //    {
        //        operateStatus.ResultSign = ResultSign.Error;
        //        operateStatus.Message = Chs.CanotDelete;
        //        return operateStatus;
        //    }
        //    MenuDeletGuid.Add(menuId);
        //    GetMenuDeleteGuid(menuId);
        //    foreach (var delete in MenuDeletGuid)
        //    {
        //        operateStatus = Delete(delete);
        //    }
        //    return operateStatus;
        //}

        ///// <summary>
        /////     删除主键集合
        ///// </summary>
        //public IList<Guid> MenuDeletGuid = new List<Guid>();

        ///// <summary>
        /////     获取删除主键信息
        ///// </summary>
        ///// <param name="guid"></param>
        //private void GetMenuDeleteGuid(Guid guid)
        //{
        //    //获取下级
        //    var menus = _menuRepository.GetMeunuByPId(guid);
        //    foreach (var dic in menus)
        //    {
        //        MenuDeletGuid.Add(dic.MenuId);
        //        GetMenuDeleteGuid(dic.MenuId);
        //    }
        //}

        #endregion

        #endregion


    }
}