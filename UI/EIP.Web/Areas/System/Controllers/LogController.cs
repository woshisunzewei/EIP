using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Config;
using EIP.Common.Core.Utils;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Dtos.Reports;
using EIP.Common.Entities.Paging;
using EIP.Common.Entities.Tree;
using EIP.Common.Web;
using EIP.System.Business.Log;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     日志管理控制器
    /// </summary>
    public class LogController : BaseController
    {
        #region 构造函数

        private readonly ISystemExceptionLogLogic _exceptionLogLogic;
        private readonly ISystemLoginLogLogic _loginLogLogic;
        private readonly ISystemOperationLogLogic _operationLogLogic;
        private readonly ISystemDataLogLogic _dataLogLogic;
        private readonly ISystemSqlLogLogic _sqlLogLogic;
        public LogController(ISystemExceptionLogLogic exceptionLogLogic,
            ISystemLoginLogLogic loginLogLogic,
            ISystemOperationLogLogic operationLogLogic,
            ISystemDataLogLogic dataLogLogic, ISystemSqlLogLogic sqlLogLogic)
        {
            _operationLogLogic = operationLogLogic;
            _dataLogLogic = dataLogLogic;
            _sqlLogLogic = sqlLogLogic;
            _exceptionLogLogic = exceptionLogLogic;
            _loginLogLogic = loginLogLogic;
        }

        #endregion

        #region 数据日志

        /// <summary>
        ///     数据日志
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("数据日志-列表")]
        public ViewResultBase DataLog()
        {
            return View();
        }
        /// <summary>
        ///     获取所有数据日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("数据日志-方法-列表-获取所有数据日志")]
        public async Task<JsonResult> GetPagingDataLog(QueryParam paging)
        {
            return JsonForGridPaging(await _dataLogLogic.PagingQueryProcAsync(paging));
        }

        /// <summary>
        ///     根据主键获取数据日志
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("数据日志-方法-列表-根据主键获取数据日志")]
        public async Task<JsonResult> GetDataLogById(IdInput input)
        {
            return Json(await _dataLogLogic.GetByIdAsync(input.Id));
        }
        #endregion

        #region 文本日志

        /// <summary>
        ///     文本日志
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("文本日志-视图")]
        public ViewResultBase TxtLog()
        {
            return View();
        }

        #region 获取日志文件树ZTree

        /// <summary>
        ///     获取目录树
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("公用-获取目录树")]
        public JsonResult GetLogZTree()
        {
            var tree = new List<TreeEntity>();
            var path = GlobalParams.Get("logPath").ToString();
            GetFiles(path, ref tree);
            return Json(tree.OrderByDescending(o => o.name).ToList());
        }

        private TreeEntity _treeEntity;

        /// <summary>
        ///     递归获取文件信息并返回树型结构
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="tree">树集合</param>
        private void GetFiles(string path,
            ref List<TreeEntity> tree)
        {
            var strFileNames = Directory.GetFiles(path);
            var strDirectories = Directory.GetDirectories(path);
            foreach (var filename in strFileNames)
            {
                _treeEntity = new TreeEntity
                {
                    icon = "blue-document-text",
                    id = filename,
                    pId = path,
                    name = Path.GetFileName(filename) + "(" + FileUtil.GetFileSize(filename) + ")",
                    url = filename
                };
                tree.Add(_treeEntity);
            }
            foreach (var dir in strDirectories)
            {
                var directoryName = dir.Substring(dir.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
                _treeEntity = new TreeEntity
                {
                    isParent = true,
                    id = dir,
                    pId = path,
                    name = directoryName
                };
                tree.Add(_treeEntity);
                GetFiles(dir, ref tree);
            }
        }

        /// <summary>
        ///     显示内容
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("公用-获取对应路径下文件内容")]
        public ViewResultBase TxtLogContent(string filePath)
        {
            ViewBag.FileContent = FileUtil.ReadFile(filePath);
            return View();
        }

        #endregion

        #endregion

        #region 异常日志

        /// <summary>
        ///     异常日志
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("异常日志-列表")]
        public ViewResultBase ExceptionLog()
        {
            return View();
        }

        /// <summary>
        ///     获取所有异常信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-获取所有异常信息")]
        public async Task<JsonResult> GetPagingExceptionLog(QueryParam paging)
        {
            return JsonForGridPaging(await _exceptionLogLogic.PagingQueryProcAsync(paging));
        }

        /// <summary>
        ///     根据主键获取异常明细
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-根据主键获取异常明细")]
        public async Task<JsonResult> GetExceptionLogById(IdInput<int> input)
        {
            return Json(await _exceptionLogLogic.GetByIdAsync(input.Id));
        }

        /// <summary>
        ///     根据主键删除异常信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-根据主键删除异常信息")]
        public async Task<JsonResult> DeleteExceptionLogById(IdInput<string> input)
        {
            return Json(await _exceptionLogLogic.DeleteByIdsAsync(input.Id));
        }

        /// <summary>
        ///     根据主键删除异常信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-根据主键删除异常信息")]
        public async Task<JsonResult> DeleteExceptionLogAll()
        {
            return Json(await _exceptionLogLogic.DeleteAllAsync());
        }

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-导出到Excel")]
        public async Task<FileResult> ExportExcelToExceptionLog(QueryParam paging)
        {
            ExcelReportDto excelReportDto = new ExcelReportDto()
            {
                TemplatePath = Server.MapPath("/") + "DataUser/Templates/System/Log/异常日志.xlsx",
                DownTemplatePath = "异常日志" + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".xlsx",
                Title = "异常日志.xlsx"
            };
            await _exceptionLogLogic.ReportExcelExceptionLogQuery(paging, excelReportDto);
            return File(new FileStream(excelReportDto.DownPath, FileMode.Open), "application/octet-stream", Server.UrlEncode(excelReportDto.Title));
        }

        #endregion

        #region 登录日志

        /// <summary>
        ///     登录日志
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("登录日志-列表")]
        public ViewResultBase LoginLog()
        {
            return View();
        }

        /// <summary>
        ///     获取所有登录日志信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("登录日志-方法-列表-获取所有登录日志信息")]
        public async Task<JsonResult> GetPagingLoginLog(QueryParam paging)
        {
            return JsonForGridPaging(await _loginLogLogic.PagingQueryProcAsync(paging));
        }

        /// <summary>
        ///     根据主键删除登录日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-根据主键删除登录日志")]
        public async Task<JsonResult> DeleteLoginLogById(IdInput<string> input)
        {
            return Json(await _loginLogLogic.DeleteByIdsAsync(input.Id));
        }

        /// <summary>
        ///     根据主键删除登录日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-根据主键删除登录日志")]
        public async Task<JsonResult> DeleteLoginLogAll()
        {
            return Json(await _loginLogLogic.DeleteAllAsync());
        }

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("登录日志-方法-列表-导出到Excel")]
        public async Task<FileResult> ExportExcelToLoginLog(QueryParam paging)
        {
            ExcelReportDto excelReportDto = new ExcelReportDto()
            {
                TemplatePath = Server.MapPath("/") + "DataUser/Templates/System/Log/登录日志.xlsx",
                DownTemplatePath = "登录日志" + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".xlsx",
                Title = "登录日志.xlsx"
            };
            await _loginLogLogic.ReportExcelLoginLogQuery(paging, excelReportDto);
            return File(new FileStream(excelReportDto.DownPath, FileMode.Open), "application/octet-stream", Server.UrlEncode(excelReportDto.Title));

        }
        #endregion

        #region 操作日志

        /// <summary>
        ///     操作日志
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("操作日志-列表")]
        public ViewResultBase OperationLog()
        {
            return View();
        }

        /// <summary>
        ///     获取所有操作日志信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("操作日志-方法-列表-获取所有操作日志信息")]
        public async Task<JsonResult> GetPagingOperationLog(QueryParam paging)
        {
            return JsonForGridPaging(await _operationLogLogic.PagingQueryProcAsync(paging));
        }

        /// <summary>
        ///     根据主键获取操作日志信息明细
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("操作日志-方法-列表-根据主键获取操作日志信息明细")]
        public async Task<JsonResult> GetOperationLogById(IdInput<int> input)
        {
            return Json(await _operationLogLogic.GetByIdAsync(input.Id));
        }

        /// <summary>
        ///     根据主键删除操作日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-根据主键删除操作日志")]
        public async Task<JsonResult> DeleteOperationLogById(IdInput<string> input)
        {
            return Json(await _operationLogLogic.DeleteByIdsAsync(input.Id));
        }

        /// <summary>
        ///     根据主键删除操作日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-根据主键删除登录日志")]
        public async Task<JsonResult> DeleteOperationLogAll()
        {
            return Json(await _operationLogLogic.DeleteAllAsync());
        }

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("操作日志-方法-列表-导出到Excel")]
        public async Task<FileResult> ExportExcelToOperationLog(QueryParam paging)
        {
            ExcelReportDto excelReportDto = new ExcelReportDto()
            {
                TemplatePath = Server.MapPath("/") + "DataUser/Templates/System/Log/操作日志.xlsx",
                DownTemplatePath = "操作日志" + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".xlsx",
                Title = "操作日志.xlsx"
            };
            await _operationLogLogic.ReportExcelOperationLogQuery(paging, excelReportDto);
            return File(new FileStream(excelReportDto.DownPath, FileMode.Open), "application/octet-stream", Server.UrlEncode(excelReportDto.Title));

        }
        #endregion

        #region Sql日志

        /// <summary>
        ///     Sql日志
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("Sql日志-列表")]
        public ViewResultBase SqlLog()
        {
            return View();
        }
        /// <summary>
        ///     获取所有数据日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("Sql日志-方法-列表-获取所有Sql日志")]
        public async Task<JsonResult> GetPagingSqlLog(QueryParam paging)
        {
            return JsonForGridPaging(await _sqlLogLogic.PagingQueryProcAsync(paging));
        }

        /// <summary>
        ///     根据主键获取数据日志
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("Sql日志-方法-列表-根据主键获取Sql日志")]
        public async Task<JsonResult> GetSqlLogById(IdInput input)
        {
            return Json(await _sqlLogLogic.GetByIdAsync(input.Id));
        }

        /// <summary>
        ///     根据主键删除Sql日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-根据主键删除Sql日志")]
        public async Task<JsonResult> DeleteSqlLogById(IdInput<string> input)
        {
            return Json(await _sqlLogLogic.DeleteByIdsAsync(input.Id));
        }

        /// <summary>
        ///     根据主键删除Sql日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("异常日志-方法-列表-根据主键删除Sql日志")]
        public async Task<JsonResult> DeleteSqlLogAll()
        {
            return Json(await _sqlLogLogic.DeleteAllAsync());
        }
        #endregion

        #region 日志分析

        #region 浏览器分析
        /// <summary>
        /// 浏览器分析
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("日志分析-视图-浏览器")]
        public ViewResultBase AnalysisForBrowser()
        {
            return View();
        }

        /// <summary>
        /// 浏览器分析
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("日志分析-方法-获取浏览器分析数据")]
        public async Task<JsonResult> GetAnalysisForBrowser()
        {
            return Json(await _loginLogLogic.GetBrowserAnalysis());
        }
        #endregion

        #endregion
    }
}