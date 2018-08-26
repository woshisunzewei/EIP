using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Resource;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.System.DataAccess.Permission;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;

namespace EIP.System.Business.Permission
{
    /// <summary>
    ///     功能项业务逻辑
    /// </summary>
    public class SystemMenuButtonLogic : AsyncLogic<SystemMenuButton>, ISystemMenuButtonLogic
    {
        #region 构造函数

        private readonly ISystemMenuButtonRepository _functionRepository;
        private readonly ISystemPermissionLogic _permissionLogic;

        public SystemMenuButtonLogic(ISystemMenuButtonRepository functionRepository,
            ISystemPermissionLogic permissionLogic)
            : base(functionRepository)
        {
            _permissionLogic = permissionLogic;
            _functionRepository = functionRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据菜单获取功能项信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemMenuButtonOutput>> GetMenuButtonByMenuId(NullableIdInput input)
        {
            var functions = await _functionRepository.GetMenuButtonByMenuId(input);
            return functions.OrderBy(o => o.MenuName).ThenBy(b => b.OrderNo).ToList();
        }

        /// <summary>
        ///     保存功能项信息
        /// </summary>
        /// <param name="function">功能项信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveMenuButton(SystemMenuButton function)
        {
            if (function.MenuButtonId.IsEmptyGuid())
            {
                function.MenuButtonId = CombUtil.NewComb();
                return await InsertAsync(function);
            }
            return await UpdateAsync(function);
        }

        /// <summary>
        ///     删除功能项
        /// </summary>
        /// <param name="input">功能项信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteMenuButton(IdInput input)
        {
            var operateStatus = new OperateStatus();
            //查看该功能项是否已被特性占用
            var permissions = await _permissionLogic.GetSystemPermissionsByPrivilegeAccessAndValue(EnumPrivilegeAccess.菜单按钮, input.Id);
            if (permissions.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.已被赋予权限);
                return operateStatus;
            }
            return await DeleteAsync(input.Id);
        }

        /// <summary>
        ///     获取登录人员对应菜单下的功能项
        /// </summary>
        /// <param name="mvcRote">路由信息</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task< IEnumerable<SystemMenuButton>> GetMenuButtonByMenuIdAndUserId(MvcRote mvcRote,
            Guid userId)
        {
            return (await _functionRepository.GetMenuButtonByMenuIdAndUserId(mvcRote, userId)).ToList();
        }

        #endregion
    }
}