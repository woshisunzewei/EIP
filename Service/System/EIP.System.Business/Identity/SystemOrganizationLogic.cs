using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.Common.Core.Resource;
using EIP.System.Business.Permission;
using EIP.System.DataAccess.Identity;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;
namespace EIP.System.Business.Identity
{
    public class SystemOrganizationLogic : AsyncLogic<SystemOrganization>, ISystemOrganizationLogic
    {
        #region 构造函数

        private readonly ISystemOrganizationRepository _organizationRepository;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemGroupLogic _groupLogic;
        private readonly ISystemRoleLogic _roleLogic;
        private readonly ISystemPostLogic _postLogic;
        public SystemOrganizationLogic(ISystemOrganizationRepository organizationRepository,
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemPermissionLogic permissionLogic, 
            ISystemGroupLogic groupLogic, 
            ISystemRoleLogic roleLogic,
            ISystemPostLogic postLogic)
            : base(organizationRepository)
        {
            _permissionUserLogic = permissionUserLogic;
            _permissionLogic = permissionLogic;
            _groupLogic = groupLogic;
            _roleLogic = roleLogic;
            _postLogic = postLogic;
            _organizationRepository = organizationRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     异步读取树数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetOrganizationTreeAsync(IdInput input)
        {
            var lists = (await _organizationRepository.GetSystemOrganizationByPid(input)).ToList();
            foreach (var list in lists)
            {
                var info = (await _organizationRepository.GetSystemOrganizationByPid(input)).ToList();
                if (info.Count > 0)
                {
                    list.isParent = true;
                }
            }
            return lists;
        }

        /// <summary>
        ///     同步读取所有树数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetOrganizationTree()
        {
            return await _organizationRepository.GetSystemOrganization();
        }

        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemOrganizationOutput>> GetOrganizationResultByTreeId(IdInput input)
        {
            IList<SystemOrganizationOutput> organizations = (await _organizationRepository.GetOrganizationResultByTreeId(input)).ToList();
            foreach (var organization in organizations)
            {
                organization.NatureName = EnumUtil.GetName(typeof(EnumOrgNature), organization.Nature);
            }
            return organizations;
        }

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckOrganizationCode(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();
            if (await _organizationRepository.CheckOrganizationCode(input))
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
        ///     保存组织机构
        /// </summary>
        /// <param name="organization">组织机构</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveOrganization(SystemOrganization organization)
        {
            if (organization.OrganizationId.IsEmptyGuid())
            {
                organization.CreateTime = DateTime.Now;
                organization.OrganizationId = Guid.NewGuid();
                return await InsertAsync(organization);
            }
            organization.UpdateTime = DateTime.Now;
            organization.UpdateUserId = organization.CreateUserId;
            organization.UpdateUserName = organization.CreateUserName;
            SystemOrganization systemOrganization = await GetByIdAsync(organization.OrganizationId);
            organization.CreateTime = systemOrganization.CreateTime;
            organization.CreateUserId = systemOrganization.CreateUserId;
            organization.CreateUserName = systemOrganization.CreateUserName;
            return await UpdateAsync(organization);
        }

        /// <summary>
        ///     删除组织机构下级数据
        ///     删除条件:
        ///     1:没有下级菜单
        ///     2:没有权限数据
        ///     3:没有人员
        /// </summary>
        /// <param name="input">组织机构id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteOrganization(IdInput input)
        {
            var operateStatus = new OperateStatus();
            //判断下级菜单
            IList<TreeEntity> orgs = (await _organizationRepository.GetSystemOrganizationByPid(input)).ToList();
            if (orgs.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.具有下级项);
                return operateStatus;
            }

            //判断是否具有人员
            var permissionUsers =await 
                _permissionUserLogic.GetPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(
                    EnumPrivilegeMaster.组织机构,
                    input.Id);
            if (permissionUsers.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.具有人员);
                return operateStatus;
            }

            //判断是否有角色
            var orgRole = await
               _roleLogic.GetRolesByOrganizationId(new NullableIdInput(input.Id));
            if (orgRole.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.具有角色);
                return operateStatus;
            }

            //判断是否有组
            var orgGroup = await
                _groupLogic.GetGroupByOrganizationId(new NullableIdInput(input.Id));
            if (orgGroup.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.具有组);
                return operateStatus;
            }

            //判断是否有岗位
            var orgPost = await
               _postLogic.GetPostByOrganizationId(new NullableIdInput(input.Id));
            if (orgPost.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.具有岗位);
                return operateStatus;
            }

            //判断是否具有按钮权限
            var functionPermissions =await 
                _permissionLogic.GetPermissionByPrivilegeMasterValue(
                  new GetPermissionByPrivilegeMasterValueInput()
                {
                    PrivilegeAccess = EnumPrivilegeAccess.菜单按钮,
                    PrivilegeMasterValue = input.Id,
                    PrivilegeMaster = EnumPrivilegeMaster.组织机构
                });
            if (functionPermissions.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.具有功能项权限);
                return operateStatus;
            }
            //判断是否具有菜单权限
            var menuPermissions =await 
                _permissionLogic.GetPermissionByPrivilegeMasterValue(
                 new GetPermissionByPrivilegeMasterValueInput()
                {
                    PrivilegeAccess = EnumPrivilegeAccess.菜单,
                    PrivilegeMasterValue = input.Id,
                    PrivilegeMaster = EnumPrivilegeMaster.组织机构
                });
            if (menuPermissions.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.具有菜单权限);
                return operateStatus;
            }
            //进行删除操作
            return await DeleteAsync(input.Id);
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
                //获取所有字典树
                var dics = (await GetAllEnumerableAsync()).ToList();
                var topOrgs = dics.Where(w => w.ParentId == Guid.Empty);
                foreach (var org in topOrgs)
                {
                    org.Code = PinYinUtil.GetFirst(org.Name);
                    await UpdateAsync(org);
                    await GeneratingCodeRecursion(org, dics.ToList(), "");
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
        /// <param name="organization"></param>
        /// <param name="dictionaries"></param>
        /// <param name="generationCode"></param>
        private async Task GeneratingCodeRecursion(SystemOrganization organization, IList<SystemOrganization> dictionaries, string generationCode)
        {
            string emp = PinYinUtil.GetFirst(organization.Name);
            //获取下级
            var nextOrg = dictionaries.Where(w => w.ParentId == organization.OrganizationId).ToList();
            if (nextOrg.Any())
            {
                emp = generationCode.IsNullOrEmpty() ? emp : generationCode + "_" + emp;
            }
            foreach (var org in nextOrg)
            {
                org.Code = emp + "_" + PinYinUtil.GetFirst(org.Name);
                await UpdateAsync(org);
                await GeneratingCodeRecursion(org, dictionaries, emp);
            }
        }

        /// <summary>
        /// 数据权限组织机构树
        /// </summary>
        ///  <param name="input"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetOrganizationResultByDataPermission(IdInput<string> input)
        {
            return await _organizationRepository.GetOrganizationResultByDataPermission(input);
        }
        #endregion
    }
}