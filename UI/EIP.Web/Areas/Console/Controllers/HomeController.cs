using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Web;
using EIP.System.Business.Permission;

namespace EIP.Web.Areas.Console.Controllers
{
    /// <summary>
    ///     首页
    /// </summary>
    public class HomeController : BaseController
    {
        #region 视图

        /// <summary>
        ///     首页
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("首页-界面")]
        [Ignore]
        public ViewResultBase Index()
        {
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     加载模块权限
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("首页-方法-获取菜单模块权限")]
        [Ignore]
        public async Task<JsonResult>  LoadMenuPermission()
        {
            return JsonForWdTree((await _permissionLogic.GetSystemPermissionMenuByUserId(CurrentUser.UserId)).ToList());
        }

        /// <summary>
        ///     替换模板中的字段值
        /// </summary>
        public string ReplaceText(string userName, string name, string myName)
        {
            var path = Server.MapPath("\\Templates\\Email\\TestTemplate.html");

            if (path == string.Empty)
            {
                return string.Empty;
            }
            var sr = new StreamReader(path);
            var str = sr.ReadToEnd();
            str = str.Replace("$USER_NAME$", userName);
            str = str.Replace("$NAME$", name);
            str = str.Replace("$MY_NAME$", myName);

            return str;
        }

        #endregion

        #region 构造函数

        private readonly ISystemPermissionLogic _permissionLogic;

        public HomeController(ISystemPermissionLogic permissionLogic)
        {
            _permissionLogic = permissionLogic;
        }

        #endregion
    }
}