using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Resource;
using EIP.Common.Core.Utils;
using EIP.Common.Dapper;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;

using EIP.Web.Areas.System.Models;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    /// 运行时控制器
    /// </summary>
    public class RunningController : BaseController
    {
        #region 程序集
        /// <summary>
        /// 程序集
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("程序集-视图-程序集列表")]
        public ViewResultBase AssemblyList()
        {
            return View();
        }

        /// <summary>
        /// 根据关键名称获取程序集信息
        /// </summary>
        /// <param name="fullName">关键字</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("程序集-方法-根据关键名称获取程序集信息")]
        public JsonResult GetAssemblyByFullName(string fullName = "")
        {
            IList<SystemRunningViewModel> assemblies = AssemblyUtil.GetAssemblyByFullName(fullName).Select(assembly => new SystemRunningViewModel
            {
                Name = assembly.GetName().Name,
                ClrVersion = assembly.ImageRuntimeVersion,
                Version = assembly.GetName().Version.ToString()
            }).ToList();
            return Json(assemblies.OrderBy(o => o.Name));
        }

        #endregion

        #region 缓存

        /// <summary>
        /// 缓存
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("缓存-视图-列表")]
        public ViewResultBase SqlCacheList()
        {
            return View();
        }

        /// <summary>
        /// 获取Sql缓存列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("缓存-方法-获取Sql缓存列表")]
        public JsonResult GgetAllSqlCacheList()
        {
            IList<SystemSqlCacheViewModel> viewModels = new List<SystemSqlCacheViewModel>();
            foreach (var result in DapperCacheCommon._ModelDesCache)
            {
                viewModels.Add(new SystemSqlCacheViewModel()
                {
                    Key = result.Key
                });
            }
            return Json(viewModels);
        }

        /// <summary>
        /// 根据关键字查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("缓存-方法-获取主键获取Sql缓存")]
        public JsonResult GetSqlCacheByKey(IdInput<string> input)
        {
            var result = DapperCacheCommon._ModelDesCache.Where(w => w.Key == input.Id).FirstOrDefault().Value;
            return Json(new
            {
                result.TableName,//表名
                result.ClassName,//类名称
                Properties = result.Properties.Select(s => s.Column)//缓存字段
            });
        }

        /// <summary>
        /// 根据关键字查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("缓存-方法-获取主键删除Sql缓存")]
        public JsonResult DeleteSqlCacheByKey(IdInput<string> input)
        {
            OperateStatus operateStatus=new OperateStatus();
            try
            {
                if (DapperCacheCommon._ModelDesCache.Remove(input.Id))
                {
                    operateStatus.ResultSign = ResultSign.Successful;
                    operateStatus.Message = Chs.Successful;
                }
            }
            catch (Exception ex)
            {
                operateStatus.Message = ex.Message;
            }
            return Json(operateStatus);
        }

        /// <summary>
        /// 根据关键字查询
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("缓存-方法-清空Sql缓存")]
        public JsonResult DeleteSqlCacheAll()
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                DapperCacheCommon._ModelDesCache = new Dictionary<string, ModelDes>();
                operateStatus.ResultSign = ResultSign.Successful;
                operateStatus.Message = Chs.Successful;
            }
            catch (Exception ex)
            {
                operateStatus.Message = ex.Message;
            }
            return Json(operateStatus);
        }
        #endregion
    }
}
