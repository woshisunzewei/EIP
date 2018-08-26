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
    ///     省市县管理控制器
    /// </summary>
    public class DistrictController : BaseController
    {
        #region 构造函数

        private readonly ISystemDistrictLogic _districtLogic;

        public DistrictController(ISystemDistrictLogic districtLogic)
        {
            _districtLogic = districtLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("省市县维护-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("省市县维护-视图-编辑")]
        public async Task<ViewResultBase> Edit(string parentId,
            string districtId = null)
        {
            var district = new SystemDistrict();
            if (!string.IsNullOrEmpty(districtId))
            {
                district = await _districtLogic.GetByIdAsync(districtId);
            }
            else
            {
                district.ParentId = parentId;
            }
            return View(district);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据父级查询所有子集
        /// </summary>
        /// <param name="input">父级</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("省市县维护-方法-列表-根据父级查询所有子集")]
        public async Task<JsonResult> GetDistrictTreeByParentId(IdInput<string> input)
        {
            return Json(await _districtLogic.GetDistrictTreeByParentId(input));
        }

        /// <summary>
        ///     根据父级查询所有子集
        /// </summary>
        /// <param name="input">父级</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("省市县维护-方法-列表-根据父级查询所有子集")]
        public async Task<JsonResult> GetDistrictByParentId(IdInput<string> input)
        {
            return Json(await _districtLogic.GetDistrictByParentId(input));
        }

        /// <summary>
        ///     根据县Id获取省市县Id
        /// </summary>
        /// <param name="input">县Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("省市县维护-方法-列表-根据县Id获取省市县Id")]
        public async Task<JsonResult> GetDistrictByCountId(IdInput<string> input)
        {
            return Json(input.Id.IsNullOrEmpty()
                ? new SystemDistrict()
                : await _districtLogic.GetDistrictByCountId(input));
        }

        /// <summary>
        ///     保存省市县信息
        /// </summary>
        /// <param name="district">省市县信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("省市县维护-方法-新增/编辑-保存省市县信息")]
        public async Task<JsonResult> SaveDistrict(SystemDistrict district)
        {
            return Json(await _districtLogic.SaveDistrict(district));
        }

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("省市县维护-方法-新增/编辑-检测代码是否已经具有重复项")]
        public async Task<JsonResult> CheckDistrictId(CheckSameValueInput input)
        {
            return JsonForCheckSameValue(await _districtLogic.CheckDistrictId(input));
        }

        /// <summary>
        ///     删除省市县及下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("省市县维护-方法-列表-删除省市县及下级数据")]
        public async Task<JsonResult> DeleteDistrict(IdInput<string> input)
        {
            return Json(await _districtLogic.DeleteDistrict(input));
        }

        #endregion
    }
}