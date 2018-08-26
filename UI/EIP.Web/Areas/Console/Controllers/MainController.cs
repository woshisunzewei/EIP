using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Identity;
using EIP.System.Models.Dtos.Identity;

namespace EIP.Web.Areas.Console.Controllers
{
    public class MainController : BaseController
    {
        #region 构造函数

        private readonly ISystemUserInfoLogic _userInfoLogic;

        public MainController(ISystemUserInfoLogic userInfoLogic)
        {
            _userInfoLogic = userInfoLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     主界面
        /// </summary>
        /// <returns></returns>
        public ViewResultBase Index()
        {
            return View();
        }

        public ViewResultBase Portal()
        {
            return View();
        }

        /// <summary>
        ///     修改密码
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("主界面-视图-修改密码")]
        public ViewResultBase ChangePassword()
        {
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     保存修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("主界面-方法-保存修改后密码信息")]
        public async Task<JsonResult> SaveChangePassword(ChangePasswordInput input)
        {
            input.Id = CurrentUser.UserId;
            return Json(await _userInfoLogic.SaveChangePassword(input));
        }

        /// <summary>
        ///     验证旧密码是否输入正确
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("主界面-方法-验证旧密码是否输入正确")]
        public async Task<JsonResult> CheckOldPassword(CheckSameValueInput input)
        {
            input.Id = CurrentUser.UserId;
            return JsonForCheckSameValue(await _userInfoLogic.CheckOldPassword(input));
        }

        /// <summary>
        ///     获取天气
        /// </summary>
        /// <returns></returns>
        public JsonResult GetWeather()
        {
            string strDate = "", strValue = "";
            try
            {
                const string strUrl = "http://apis.baidu.com/heweather/weather/free?city=chengdu";
                var request = (HttpWebRequest) WebRequest.Create(strUrl);
                request.Method = "GET";
                // 添加header
                request.Headers.Add("apikey", "2faa619d5ce2b02e8475eb0812552d73");
                var response = (HttpWebResponse) request.GetResponse();
                var s = response.GetResponseStream();

                var reader = new StreamReader(s, Encoding.UTF8);
                while ((strDate = reader.ReadLine()) != null)
                {
                    strValue += strDate + "\r\n";
                }
                return Json(strValue);
            }
            catch (Exception ex)
            {
                return Json(strValue);
            }
        }

        #endregion
    }
}