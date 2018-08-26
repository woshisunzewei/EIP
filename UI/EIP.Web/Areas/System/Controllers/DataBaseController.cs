using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Utils;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.Common.Web;
using EIP.System.Business.Config;
using EIP.System.Models.Dtos.Config;
using EIP.System.Models.Entities;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    /// SQL Server数据库在线管理系统
    /// SQL Server Online Management(缩写SSOM)
    /// http://www.mycodes.net/38/969.htm
    /// http://www.16aspx.com/newui/goods.aspx?id=3647
    /// http://www.cnblogs.com/pcstx/p/4644271.html
    /// SqlWebAdmin
    /// </summary>
    public class DataBaseController : BaseController
    {
        #region 构造函数

        private readonly ISystemDataBaseLogic _dataBaseLogic;
        private readonly ISystemDataBaseBackUpLogic _dataBaseBackUpLogic;

        public DataBaseController(ISystemDataBaseLogic dataBaseLogic,
            ISystemDataBaseBackUpLogic dataBaseBackUpLogic)
        {
            _dataBaseLogic = dataBaseLogic;
            _dataBaseBackUpLogic = dataBaseBackUpLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用数据库-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用数据库-视图-编辑")]
        public async Task<ViewResultBase> Edit(NullableIdInput<Guid> input)
        {
            var database = new SystemDataBase();
            if (!input.Id.IsNullOrEmptyGuid())
            {
                database = await _dataBaseLogic.GetByIdAsync(input.Id);
            }
            return View(database);
        }

        /// <summary>
        ///     数据库表空间
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用数据库-视图-数据库表空间")]
        public ViewResultBase Spaceused()
        {
            return View();
        }

        /// <summary>
        ///     数据库表空间
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("应用数据库-视图-数据库备份")]
        public ViewResultBase BackupOrRestore()
        {
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     获取表空间使用情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-列表-获取表空间使用情况")]
        public async Task<JsonResult> GetDataBaseSpaceused(IdInput input)
        {
            return Json(await _dataBaseLogic.GetDataBaseSpaceused(input));
        }

        /// <summary>
        ///     获取对应数据库表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-列表-获取对应数据库表信息")]
        public async Task<JsonResult> GetDataBaseTables(IdInput input)
        {
            return Json(await _dataBaseLogic.GetDataBaseTables(input));
        }

        /// <summary>
        ///     获取对应表列信息
        /// </summary>
        /// <param name="doubleWayDto"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-列表-获取对应表列信息")]
        public async Task<JsonResult> GetDataBaseColumns(SystemDataBaseTableDoubleWay doubleWayDto)
        {
            return Json(await _dataBaseLogic.GetDataBaseColumns(doubleWayDto));
        }

        /// <summary>
        /// 获取字段树结构
        /// </summary>
        /// <param name="doubleWay"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-列表-获取对应表列信息树结构")]
        public async Task<JsonResult> GetDataBaseColumnsTree(SystemDataBaseTableDoubleWay doubleWay)
        {
            var columns = await _dataBaseLogic.GetDataBaseColumns(doubleWay);
            IList<TreeEntity> treeEntities = new List<TreeEntity>();
            var parentId = CombUtil.NewComb();
            TreeEntity treeEntity = new TreeEntity
            {
                id = parentId,
                name = doubleWay.TableName,
                nocheck = true
            };
            treeEntities.Add(treeEntity);
            foreach (var co in columns)
            {
                treeEntity = new TreeEntity
                {
                    pId = parentId,
                    name = co.ColumnDescription + "(" + co.ColumnName + ")",
                    code = co.ColumnDescription,
                    id = co.ColumnName
                };
                treeEntities.Add(treeEntity);
            }
            return Json(treeEntities);
        }

        /// <summary>
        ///     获取外键信息
        /// </summary>
        /// <param name="doubleWayDto"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-列表-获取外键信息")]
        public async Task<JsonResult> GetdatabsefFkColumn(SystemDataBaseTableDoubleWay doubleWayDto)
        {
            return Json(await _dataBaseLogic.GetdatabsefFkColumn(doubleWayDto));
        }

        /// <summary>
        ///     获取外键信息
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-新增/编辑-保存")]
        public async Task<JsonResult> SaveDataBase(SystemDataBase dataBase)
        {
            return Json(await _dataBaseLogic.SaveDataBase(dataBase));
        }

        /// <summary>
        ///     获取外键信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-列表-删除")]
        public async Task<JsonResult> DeleteDataBase(IdInput input)
        {
            return Json(await _dataBaseLogic.DeleteAsync(input.Id));
        }

        /// <summary>
        ///     获取所有应用数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-列表-获取所有应用数据库")]
        public async Task<JsonResult> GetAllDataBase()
        {
            return Json((await _dataBaseLogic.GetAllEnumerableAsync()).OrderBy(o => o.OrderNo));
        }

        /// <summary>
        ///     数据库备份
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-列表-获取数据库备份文件")]
        public async Task<JsonResult> GetDataBaseBackUpOrRestore(IdInput input)
        {
            return Json(await _dataBaseBackUpLogic.GetDataBaseBackUpOrRestore(input));
        }

        /// <summary>
        ///     数据库备份
        /// </summary>
        /// <param name="doubleWay"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-列表-数据库备份")]
        public async Task<JsonResult> DataBaseBackUp(SystemDataBaseBackUpDoubleWay doubleWay)
        {
            doubleWay.BackUpOrRestorePath = Server.MapPath("~");
            return Json(await _dataBaseBackUpLogic.SystemDataBaseBackUp(doubleWay));
        }

        /// <summary>
        ///     数据库备份
        /// </summary>
        /// <param name="doubleWay"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("应用数据库-方法-列表-数据库还原")]
        public async Task<JsonResult> DataBaseRestore(SystemDataBaseBackUpDoubleWay doubleWay)
        {
            return Json(await _dataBaseBackUpLogic.SystemDataBaseRestore(doubleWay));
        }

        #endregion
    }
}