using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Paging;
using EIP.Common.Web;
using EIP.System.Business.Config;
using EIP.System.Models.Entities;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    /// 文章下载记录表控制器
    /// </summary>
    public class DownloadController : BaseController
    {
        #region 构造函数
        private readonly ISystemDownloadLogic _systemDownloadLogic;

        public DownloadController(ISystemDownloadLogic systemDownloadLogic)
        {
            _systemDownloadLogic = systemDownloadLogic;
        }
        #endregion

        #region 视图
        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-文章下载记录表-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-文章下载记录表-编辑")]
        public async Task<ViewResultBase> Edit(NullableIdInput input)
        {
            SystemDownload model = new SystemDownload();
            if (!input.Id.IsNullOrEmptyGuid())
            {
                model = await _systemDownloadLogic.GetByIdAsync(input.Id);
            }
            return View(model);
        }
        #endregion

        #region 方法

        /// <summary>
        ///     获取文章下载记录表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-文章下载记录表-方法-获取文章下载记录表")]
        public async Task<JsonResult> GetList(QueryParam param)
        {
            return Json(await _systemDownloadLogic.PagingQueryProcAsync(param));
        }

        /// <summary>
        ///     保存文章下载记录表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-文章下载记录表-方法-保存文章下载记录表")]
        public async Task<JsonResult> Save(SystemDownload model)
        {
            return Json(await _systemDownloadLogic.Save(model));
        }

        /// <summary>
        ///     删除文章下载记录表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-文章下载记录表-方法-删除文章下载记录表")]
        public async Task<JsonResult> Delete(IdInput input)
        {
            return Json(await _systemDownloadLogic.DeleteAsync(input.Id));
        }
        #endregion
    }
}
