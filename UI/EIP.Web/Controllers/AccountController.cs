using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Auth;
using EIP.Common.Core.Log;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.System.Business.Config;
using EIP.System.Business.Identity;
using EIP.System.Business.Log;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;
using SystemWeb = System.Web;
namespace EIP.Web.Controllers
{
    /// <summary>
    ///     帐号
    /// </summary>
    public class AccountController : Controller
    {
        #region 构造函数

        private readonly ISystemUserInfoLogic _userInfoLogic;
        private readonly ISystemLoginLogLogic _loginLogLogic;
        private readonly ISystemLoginSlideLogic _systemLoginSlideLogic;
        public AccountController(ISystemUserInfoLogic userInfoLogic, ISystemLoginLogLogic loginLogLogic, ISystemLoginSlideLogic systemLoginSlideLogic)
        {
            _userInfoLogic = userInfoLogic;
            _loginLogLogic = loginLogLogic;
            _systemLoginSlideLogic = systemLoginSlideLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     登录
        /// </summary>
        /// <returns></returns>
        public ViewResultBase Login()
        {
            return View();
        }

        /// <summary>
        ///     生成验证码
        /// </summary>
        /// <returns></returns>
        public FileResult GetValidateCode()
        {
            var bytes = VerifyCodeUtil.GetVerifyCodeImage();
            return File(bytes, @"image/jpeg");
        }
        #endregion

        #region 方法

        /// <summary>
        ///     获取登录幻灯片
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-登录幻灯片-方法-获取登录幻灯片")]
        public async Task<JsonResult> GetLoginSlideList()
        {
            OperateStatus<IList<SystemLoginSlide>> operateStatus = new OperateStatus<IList<SystemLoginSlide>>();
            try
            {
                operateStatus.Data =
                    (await _systemLoginSlideLogic.GetAllEnumerableAsync()).Where(w => w.IsFreeze == false)
                        .OrderBy(o => o.OrderNo)
                        .ToList();
                operateStatus.ResultSign=ResultSign.Successful;
            }
            catch (Exception ex)
            {
                operateStatus.ResultSign=ResultSign.Error;
                operateStatus.Message = ex.Message;
            }
            return Json(operateStatus);
        }

        /// <summary>
        ///     Post提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Submit(UserLoginInput input)
        {
            var operateStatus = new OperateStatus();
            try
            {
                
                //获取生成验证码的结果值
                var verifyCode = VerifyCodeUtil.GetVerifyCode();
                //判断录入验证码和生成的验证码值是否相等
                if (input.Verify != verifyCode)
                {
                    operateStatus.ResultSign = ResultSign.Error;
                    operateStatus.Message = "验证码错误";
                    return Json(operateStatus);
                }
                //验证数据库信息
                var info = await _userInfoLogic.CheckUserByCodeAndPwd(input);
                if (info.Data != null)
                {
                    var principalUser = new PrincipalUser
                    {
                        UserId = info.Data.UserId,
                        Code = info.Data.Code,
                        Name = info.Data.Name,
                        OrganizationId = info.Data.OrganizationId,
                        OrganizationName = info.Data.OrganizationName
                    };
                    principalUser.LoginId = Guid.NewGuid();
                    //写入Cookie信息
                    FormAuthenticationExtension.SetAuthCookie(principalUser.UserId.ToString(), principalUser, input.Remberme);
                    //是否具有返回路径
                    if (Url.IsLocalUrl(input.ReturnUrl) && input.ReturnUrl.Length > 1 && input.ReturnUrl.StartsWith("/")
                        && !input.ReturnUrl.StartsWith("//") && !input.ReturnUrl.StartsWith("/\\"))
                    {
                        info.ResultSign = ResultSign.Successful;
                        info.Message = input.ReturnUrl;
                    }
                    //写入日志
                    WriteLoginLog(principalUser.LoginId);
                }
                return Json(info);
            }
            catch (Exception ex)
            {
                operateStatus.Message = ex.Message;
                return Json(operateStatus);
            }
        }

        /// <summary>
        ///     写入登录日志
        /// </summary>
        /// <param name="loginId">登录日志Id</param>
        private void WriteLoginLog(Guid loginId)
        {
            //获取当前用户信息
            var logHandler = new LoginLogHandler(loginId);
            logHandler.WriteLog();
        }

        /// <summary>
        ///     退出
        /// </summary>
        /// <returns></returns>
        [Description("退出系统")]
        public async Task<RedirectResult> Logout()
        {
            //获取当前用户信息
            var currentUser = FormAuthenticationExtension.Current(SystemWeb.HttpContext.Current.Request);
            if (currentUser != null)
            {
                var loginLog = await _loginLogLogic.GetByIdAsync(currentUser.LoginId);
                if (loginLog != null)
                {
                    loginLog.LoginOutTime = DateTime.Now;
                    var timeSpan = (TimeSpan)(loginLog.LoginOutTime - loginLog.LoginTime);
                    loginLog.StandingTime = timeSpan.TotalHours;
                    _loginLogLogic.UpdateAsync(loginLog);
                }
            }
            FormAuthenticationExtension.SignOut();
            return Redirect("/Account/Login");
        }

        #endregion
    }
}