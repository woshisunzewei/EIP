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
    public class SystemPostLogic : AsyncLogic<SystemPost>, ISystemPostLogic
    {
        #region 构造函数

        private readonly ISystemPostRepository _postRepository;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemPermissionLogic _permissionLogic;

        public SystemPostLogic(ISystemPostRepository postRepository,
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemPermissionLogic permissionLogic)
            : base(postRepository)
        {
            _permissionUserLogic = permissionUserLogic;
            _permissionLogic = permissionLogic;
            _postRepository = postRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据组织机构获取岗位信息
        /// </summary>
        /// <param name="input">组织机构Id</param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemPostOutput>> GetPostByOrganizationId(NullableIdInput input)
        {
            return (await _postRepository.GetPostByOrganizationId(input)).ToList();
        }

        /// <summary>
        ///     检测配置项代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckPostCode(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();
            if (await _postRepository.CheckPostCode(input))
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
        /// <param name="post">岗位信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> SavePost(SystemPost post)
        {
            if (post.PostId.IsEmptyGuid())
            {
                post.CreateTime = DateTime.Now;
                post.PostId = CombUtil.NewComb();
                return await InsertAsync(post);
            }
            SystemPost systemPost =await GetByIdAsync(post.PostId);
            post.CreateTime = systemPost.CreateTime;
            post.CreateUserId = systemPost.CreateUserId;
            post.CreateUserName = systemPost.CreateUserName;
            post.UpdateTime = DateTime.Now;
            post.UpdateUserId = post.CreateUserId;
            post.UpdateUserName = post.CreateUserName;
            return await UpdateAsync(post);
        }

        /// <summary>
        ///     删除岗位信息
        /// </summary>
        /// <param name="input">岗位信息Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeletePost(IdInput input)
        {
            var operateStatus = new OperateStatus();
            //判断是否具有人员
            var permissionUsers =await  _permissionUserLogic.GetPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.岗位,
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
                new GetPermissionByPrivilegeMasterValueInput()
                {
                    PrivilegeAccess = EnumPrivilegeAccess.菜单按钮,
                    PrivilegeMasterValue = input.Id,
                    PrivilegeMaster = EnumPrivilegeMaster.岗位
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
                new GetPermissionByPrivilegeMasterValueInput()
                {
                    PrivilegeAccess = EnumPrivilegeAccess.菜单,
                    PrivilegeMasterValue = input.Id,
                    PrivilegeMaster = EnumPrivilegeMaster.岗位
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