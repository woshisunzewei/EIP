using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Config;
using EIP.System.Models.Entities;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    /// 邮件账号
    /// </summary>
    public class EmailAccountController : BaseController
    {
        #region 构造函数
        private readonly ISystemEmailAccountLogic _systemEmailAccountLogic;

        public EmailAccountController(ISystemEmailAccountLogic systemEmailAccountLogic)
        {
            _systemEmailAccountLogic = systemEmailAccountLogic;
        }
        #endregion

        #region 视图
        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-邮件账号-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-邮件账号-编辑")]
        public async Task<ViewResultBase> Edit(NullableIdInput input)
        {
            SystemEmailAccount emailAccount = new SystemEmailAccount();
            if (!input.Id.IsNullOrEmptyGuid())
            {
                emailAccount = await _systemEmailAccountLogic.GetByIdAsync(input.Id);
            }
            return View(emailAccount);
        }
        #endregion

        #region 方法

        /// <summary>
        ///     获取邮件账号
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-邮件账号-方法-获取邮件账号")]
        public async Task<JsonResult> GetList()
        {
            return Json(await _systemEmailAccountLogic.GetAllEnumerableAsync());
        }

        /// <summary>
        ///     保存邮件账号
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-邮件账号-方法-保存邮件账号")]
        public async Task<JsonResult> SaveEmailAccount(SystemEmailAccount emailAccount)
        {
            return Json(await _systemEmailAccountLogic.SaveEmailAccount(emailAccount));
        }

        /// <summary>
        ///     删除邮件账号
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-邮件账号-方法-删除邮件账号")]
        public async Task<JsonResult> Delete(IdInput input)
        {
            return Json(await _systemEmailAccountLogic.DeleteAsync(input.Id));
        }
        #endregion
    }
}
