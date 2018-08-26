using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Config;
using EIP.System.Models.Dtos.Config;
using EIP.System.Models.Entities;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     字典控制器
    /// </summary>
    public class DictionaryController : BaseController
    {
        #region 构造函数

        private readonly ISystemDictionaryLogic _dictionaryLogic;

        public DictionaryController(ISystemDictionaryLogic dictionaryLogic)
        {
            _dictionaryLogic = dictionaryLogic;
        }

        #endregion

        #region 视图

            /// <summary>
            ///     列表
            /// </summary>
            /// <returns></returns>
            [CreateBy("孙泽伟")]
            [Description("字典信息维护-视图-列表")]
            public ViewResultBase List()
            {
                return View();
            }

            /// <summary>
            ///     编辑
            /// </summary>
            /// <returns></returns>
            [CreateBy("孙泽伟")]
            [Description("字典信息维护-视图-编辑")]
            public async Task<ViewResultBase> Edit(SystemDictionaryEditInput input)
            {
                SystemDictionaryEditOutput output = new SystemDictionaryEditOutput();
                //如果为编辑
                if (!input.DictionaryId.IsEmptyGuid())
                {
                    output = (await _dictionaryLogic.GetByIdAsync(input.DictionaryId)).MapTo<SystemDictionaryEditOutput>();
                    //获取父级信息
                    var parentInfo = await _dictionaryLogic.GetByIdAsync(output.ParentId);
                    if (parentInfo != null)
                    {
                        output.ParentName = parentInfo.Name;
                        output.ParentCode = parentInfo.Code;
                    }
                }
                //新增
                else
                {
                    if (!input.ParentId.IsEmptyGuid())
                    {
                        var parentInfo = await _dictionaryLogic.GetByIdAsync(input.ParentId);
                        output.Code = parentInfo.Code;
                        output.ParentId = input.ParentId;
                        output.ParentName = parentInfo.Name;
                        output.ParentCode = parentInfo.Code;
                    }
                }
                return View(output);
            }

        #endregion

        #region 方法

        /// <summary>
        ///     查询所有字典信息:Ztree格式
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("字典信息维护-方法-列表-查询所有字典信息")]
        public async Task<JsonResult> GetDictionaryTree()
        {
            return Json(await _dictionaryLogic.GetDictionaryTree());
        }

        /// <summary>
        ///     根据父级Id读取子字典列表信息
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("字典信息维护-方法-列表-根据父级Id读取子字典列表信息")]
        public async Task<JsonResult> GetDictionariesByParentId(IdInput input)
        {
            return Json(await _dictionaryLogic.GetDictionariesParentId(input));
        }

        /// <summary>
        /// 根据代码获取字典树
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDictionaryTreeByCode(string code)
        {
            return JsonForZtree((await _dictionaryLogic.GetDictionaryTreeByCode(code)).ToList());
        }

        /// <summary>
        ///     保存字典数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("字典信息维护-方法-新增/编辑-保存字典数据")]
        public async Task<JsonResult> SaveDictionary(SystemDictionary dictionary)
        {
            return Json(await _dictionaryLogic.SaveDictionary(dictionary));
        }

        /// <summary>
        ///     检测字典代码是否已经具有重复项
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("字典信息维护-方法-新增/编辑-检测字典代码是否已经具有重复项")]
        public async Task<JsonResult> CheckDictionaryCode(CheckSameValueInput input)
        {
            return JsonForCheckSameValue(await _dictionaryLogic.CheckDictionaryCode(input));
        }


        /// <summary>
        ///     删除字典数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("字典信息维护-方法-列表-删除")]
        public async Task<JsonResult> DeleteDictionary(IdInput input)
        {
            return Json(await _dictionaryLogic.DeleteDictionary(input));
        }

        /// <summary>
        ///     批量生成代码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("字典信息维护-方法-列表-批量生成代码")]
        public async Task<JsonResult> GeneratingCode()
        {
            return Json(await _dictionaryLogic.GeneratingCode());
        }

        #endregion
    }
}