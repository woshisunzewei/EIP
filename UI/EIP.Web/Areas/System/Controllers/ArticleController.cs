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
    /// 文章新闻表控制器
    /// </summary>
    public class ArticleController : BaseController
    {
        #region 构造函数
        private readonly ISystemArticleLogic _systemArticleLogic;
        public ArticleController(ISystemArticleLogic systemArticleLogic)
        {
            _systemArticleLogic = systemArticleLogic;
        }
        #endregion

        #region 视图
        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-文章新闻表-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-文章新闻表-编辑")]
        public async Task<ViewResultBase> Edit(NullableIdInput input)
        {
            SystemArticle model = new SystemArticle();
            if (!input.Id.IsNullOrEmptyGuid())
            {
                model = await _systemArticleLogic.GetByIdAsync(input.Id);
            }
            return View(model);
        }

        /// <summary>
        ///     查看详情
        /// </summary>
        /// <returns></returns>
        public async Task<ViewResultBase> Detail(IdInput input)
        {
            await _systemArticleLogic.SaveViewNums(input);
            return View(await _systemArticleLogic.GetByIdAsync(input.Id));
        }
        #endregion

        #region 方法

        /// <summary>
        ///     获取文章新闻表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-文章新闻表-方法-获取文章新闻表")]
        public async Task<JsonResult> GetList(QueryParam queryParam)
        {
            return JsonForGridPaging(await _systemArticleLogic.PagingQueryProcAsync(queryParam));
        }

        /// <summary>
        ///     保存文章新闻表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-文章新闻表-方法-保存文章新闻表")]
        [ValidateInput(false)]
        public async Task<JsonResult> Save(SystemArticle model)
        {
            model.CreateOrganizationId = CurrentUser.OrganizationId;
            model.CreateOrganizationName = CurrentUser.OrganizationName;
            model.CreateUserId = CurrentUser.UserId;
            model.CreateUserName = CurrentUser.Name;
            return Json(await _systemArticleLogic.Save(model));
        }

        /// <summary>
        ///     删除文章新闻表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用系统-文章新闻表-方法-删除文章新闻表")]
        public async Task<JsonResult> Delete(IdInput input)
        {
            return Json(await _systemArticleLogic.DeleteAsync(input.Id));
        }
        #endregion
    }
}
