using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Config;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Report.Excel;
using EIP.Common.Core.Resource;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Dtos.Reports;
using EIP.Common.Entities.Paging;
using EIP.System.Business.Permission;
using EIP.System.DataAccess.Identity;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Entities;
using EIP.System.Models.Enums;
namespace EIP.System.Business.Identity
{
    /// <summary>
    ///     用户业务逻辑实现
    /// </summary>
    public class SystemUserInfoLogic : AsyncLogic<SystemUserInfo>, ISystemUserInfoLogic
    {
        #region 构造函数

        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemUserInfoRepository _userInfoRepository;

        public SystemUserInfoLogic(ISystemUserInfoRepository userInfoRepository,
            ISystemPermissionUserLogic permissionUserLogic)
            : base(userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
            _permissionUserLogic = permissionUserLogic;
        }

        public SystemUserInfoLogic()
        {
            _userInfoRepository = new SystemUserInfoRepository();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据登录代码和密码查询用户信息
        /// </summary>
        /// <param name="input">登录名、密码等</param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemUserOutput>> CheckUserByCodeAndPwd(UserLoginInput input)
        {
            var operateStatus = new OperateStatus<SystemUserOutput>();
            //将传入的密码加密
            var encryptPwd = DEncryptUtil.Encrypt(input.Pwd, GlobalParams.Get("pwdKey").ToString());
            //查询信息
            input.Pwd = encryptPwd;
            var data = await _userInfoRepository.CheckUserByCodeAndPwd(input);
            //是否存在
            if (data == null)
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = ResourceSystem.用户名或密码错误;
                return operateStatus;
            }
            //是否冻结
            if (data.IsFreeze)
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = ResourceSystem.登录用户已冻结;
                return operateStatus;
            }
            //成功
            operateStatus.ResultSign = ResultSign.Successful;
            operateStatus.Message = "/";
            operateStatus.Data = data;
            if (data.FirstVisitTime == null)
            {
                //更新用户最后一次登录时间
                _userInfoRepository.UpdateFirstVisitTime(new IdInput(data.UserId));
            }
            //更新用户最后一次登录时间
            _userInfoRepository.UpdateLastLoginTime(new IdInput(data.UserId));
            return operateStatus;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        public async Task<PagedResults<SystemUserOutput>> PagingUserQuery(SystemUserPagingInput paging)
        {
            return await _userInfoRepository.PagingUserQuery(paging);
        }

        /// <summary>
        ///     Excel导出方式
        /// </summary>
        /// <param name="paging">查询参数</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        public async Task<OperateStatus> ReportExcelUserQuery(SystemUserPagingInput paging,
            ExcelReportDto excelReportDto)
        {
            var operateStatus = new OperateStatus();
            try
            {
                //组装数据
                IList<SystemUserOutput> dtos = (await _userInfoRepository.PagingUserQuery(paging)).Data.ToList();
                var tables = new Dictionary<string, DataTable>(StringComparer.OrdinalIgnoreCase);
                //组装需要导出数据
                var dt = new DataTable("User");
                dt.Columns.Add("Num");
                dt.Columns.Add("Code");
                dt.Columns.Add("Name");
                dt.Columns.Add("OrganizationName");
                dt.Columns.Add("Mobile");
                dt.Columns.Add("IsFreeze");
                dt.Columns.Add("CreatTime");
                dt.Columns.Add("FirstVisitTime");
                dt.Columns.Add("LastVisitTime");
                dt.Columns.Add("Remark");
                var num = 1;
                if (dtos.Any())
                {
                    foreach (var dto in dtos)
                    {
                        var row = dt.NewRow();
                        dt.Rows.Add(row);
                        row[0] = num;
                        row[1] = dto.Code;
                        row[2] = dto.Name;
                        row[3] = dto.OrganizationName;
                        row[4] = dto.Mobile;
                        row[5] = dto.IsFreeze ? "是" : "否";
                        row[7] = dto.FirstVisitTime;
                        row[8] = dto.LastVisitTime;
                        row[9] = dto.Remark;
                        num++;
                    }
                }
                tables.Add(dt.TableName, dt);
                OpenXmlExcel.ExportExcel(excelReportDto.TemplatePath, excelReportDto.DownPath, tables);
                operateStatus.ResultSign = ResultSign.Successful;
            }
            catch (Exception)
            {
                operateStatus.ResultSign = ResultSign.Error;
            }
            return operateStatus;
        }

        /// <summary>
        ///     检测配置项代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckUserCode(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();
            if (await _userInfoRepository.CheckUserCode(input))
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.HaveCode, input.Id);
            }
            else
            {
                operateStatus.ResultSign = ResultSign.Successful;
                operateStatus.Message = Chs.CheckSuccessful;
            }
            return operateStatus;
        }

        /// <summary>
        ///     保存人员信息
        /// </summary>
        /// <param name="user">人员信息</param>
        /// <param name="orgId">业务表Id：如组织机构Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveUser(SystemUserInfo user,
            Guid orgId)
        {
            OperateStatus operateStatus;
            if (user.UserId.IsEmptyGuid())
            {
                //新增
                user.CreateTime = DateTime.Now;
                user.UserId = Guid.NewGuid();
                user.Password = DEncryptUtil.Encrypt(GlobalParams.Get("defaultPwd").ToString(),
                    GlobalParams.Get("pwdKey").ToString());
                 operateStatus = await InsertAsync(user);
                if (operateStatus.ResultSign == ResultSign.Successful)
                {
                    //添加用户到组织机构
                    operateStatus =
                        await
                            _permissionUserLogic.SavePermissionUser(EnumPrivilegeMaster.组织机构, orgId,
                                new List<Guid> { user.UserId });
                    if (operateStatus.ResultSign == ResultSign.Successful)
                    {
                        return operateStatus;
                    }
                }
                else
                {
                    return operateStatus;
                }
            }
            else
            {
                //删除对应组织机构
                 operateStatus = await _permissionUserLogic.DeletePrivilegeMasterUser(user.UserId, EnumPrivilegeMaster.组织机构);
                if (operateStatus.ResultSign == ResultSign.Successful)
                {
                    //添加用户到组织机构
                    operateStatus = await _permissionUserLogic.SavePermissionUser(EnumPrivilegeMaster.组织机构, orgId, new List<Guid> { user.UserId });
                    if (operateStatus.ResultSign == ResultSign.Successful)
                    {
                        var userInfo = await GetByIdAsync(user.UserId);
                        user.CreateTime = userInfo.CreateTime;
                        user.Password = userInfo.Password;
                        user.UpdateTime = DateTime.Now;
                        user.UpdateUserId = userInfo.CreateUserId;
                        user.UpdateUserName = user.CreateUserName;
                        return await UpdateAsync(user);
                    }
                }
            }
            return operateStatus;
        }

        /// <summary>
        ///     获取所有用户
        /// </summary>
        /// <param name="input">是否冻结</param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemChosenUserOutput>> GetChosenUser(FreezeInput input = null)
        {
            return await _userInfoRepository.GetChosenUser(input);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemUserInfo>> GetUser(FreezeInput input = null)
        {
            return await _userInfoRepository.GetUser(input);
        }

        /// <summary>
        ///     删除用户信息
        /// </summary>
        /// <param name="input">用户id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteUser(IdInput input)
        {
            await _permissionUserLogic.DeletePermissionUser(input);
            return await DeleteAsync(input.Id);
        }

        /// <summary>
        ///     根据用户Id获取该用户信息
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        public async Task<SystemUserDetailOutput> GetDetailByUserId(IdInput input)
        {
            //获取用户基本信息
            var userDto = (await _userInfoRepository.GetByIdAsync(input.Id)).MapTo<SystemUserOutput>();
            //转换
            var userDetailDto = userDto.MapTo<SystemUserDetailOutput>();
            //获取角色、组、岗位数据
            IList<SystemPrivilegeDetailListOutput> privilegeDetailDtos = (await
                _permissionUserLogic.GetSystemPrivilegeDetailOutputsByUserId(input)).ToList();
            //角色
            userDetailDto.Role = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.角色).ToList();
            //组
            userDetailDto.Group = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组).ToList();
            //岗位
            userDetailDto.Post = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.岗位).ToList();
            return userDetailDto;
        }

        /// <summary>
        ///     根据用户Id重置某人密码
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> ResetPassword(IdInput input)
        {
            var operateStatus = new OperateStatus();
            //获取系统默认配置重置密码
            var password = GlobalParams.Get("resetPassword").ToString();
            //加密密码
            //将传入的密码加密
            var encryptPwd = DEncryptUtil.Encrypt(password, GlobalParams.Get("pwdKey").ToString());
            if (await _userInfoRepository.ResetPassword(new ResetPasswordInput
            {
                EncryptPassword = encryptPwd,
                Id = input.Id
            }))
            {
                operateStatus.ResultSign = ResultSign.Successful;
                operateStatus.Message = string.Format(ResourceSystem.重置密码成功, password);
            }
            return operateStatus;
        }

        /// <summary>
        /// 保存修改后密码信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveChangePassword(ChangePasswordInput input)
        {
            var operateStatus = new OperateStatus();
            //后台再次验证是否一致
            if (!input.NewPassword.Equals(input.ConfirmNewPassword))
            {
                operateStatus.Message = string.Format(Chs.Error, "录入的新密码和确认密码不一致。");
                return operateStatus;
            }
            //旧密码是否正确
            operateStatus = await CheckOldPassword(new CheckSameValueInput() { Id = input.Id, Param = input.OldPassword });
            if (operateStatus.ResultSign == ResultSign.Error)
            {
                return operateStatus;
            }
            //将传入的密码加密
            var encryptPwd = DEncryptUtil.Encrypt(input.NewPassword, GlobalParams.Get("pwdKey").ToString());
            if (await _userInfoRepository.ResetPassword(new ResetPasswordInput
            {
                EncryptPassword = encryptPwd,
                Id = input.Id
            }))
            {
                operateStatus.ResultSign = ResultSign.Successful;
                operateStatus.Message = string.Format(ResourceSystem.重置密码成功, input.NewPassword);
            }
            return operateStatus;
        }

        /// <summary>
        ///     验证旧密码是否输入正确
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckOldPassword(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();
            input.Param = DEncryptUtil.Encrypt(input.Param, GlobalParams.Get("pwdKey").ToString());
            if (!await _userInfoRepository.CheckOldPassword(input))
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format("旧密码不正确");
            }
            else
            {
                operateStatus.ResultSign = ResultSign.Successful;
            }
            return operateStatus;
        }

        #endregion
    }
}