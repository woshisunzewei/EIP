using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Utils;
using EIP.Common.Entities.Tree;
using EIP.Common.Web;
using EIP.System.Business.Config;
using EIP.System.Business.Permission;
using EIP.System.Models.Common.Dtos;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Entities;
using EIP.Workflow.Business.Config;

namespace EIP.Web.Areas.Common.Controllers
{
    /// <summary>
    ///     公用控制器
    /// </summary>
    public class GlobalController : BaseController
    {
        #region 构造函数

        private readonly ISystemDictionaryLogic _dictionaryLogic;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly IWorkflowProcessLogic _workflowProcessLogic;
        public GlobalController(ISystemPermissionUserLogic permissionUserLogic,
            ISystemDictionaryLogic dictionaryLogic,
            IWorkflowProcessLogic workflowProcessLogic)
        {
            _permissionUserLogic = permissionUserLogic;
            _dictionaryLogic = dictionaryLogic;
            _workflowProcessLogic = workflowProcessLogic;
        }

        #endregion

        #region 下载文件

        /// <summary>
        ///     下载文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="filePath">文件路径</param>
        [CreateBy("孙泽伟")]
        [Description("公用-下载文件")]
        public void ResponseFile(string fileName,
            string filePath)
        {
            UploadUtil.ResponsOutFile(fileName, filePath);
        }

        #endregion

        #region 获取权限用户信息

        /// <summary>
        ///     获取菜单、字段对应拥有者信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("公用-获取菜单、字段对应拥有者信息")]
        public async Task<JsonResult> GetSystemPrivilegeDetailOutputsByAccessAndValue(SystemPrivilegeDetailInput input)
        {
            return Json(await _permissionUserLogic.GetSystemPrivilegeDetailOutputsByAccessAndValue(input));
        }

        #endregion

        #region 获取指定字典数据

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("公用-根据代码获取字典数据")]
        public async Task<JsonResult> GetDictionaryByCode(GlobalGetDictionaryInput input)
        {
            var dics = await _dictionaryLogic.GetDictionaryByCode(input.Code);
            IList<TreeEntity> treeEntities = new List<TreeEntity>();
            treeEntities.Add(new TreeEntity() { id = Guid.Empty, name = input.ParentName, isParent = true, open = true });
            foreach (var dic in dics)
            {
                treeEntities.Add(new TreeEntity() { id = dic.DictionaryId, pId = Guid.Empty, name = dic.Name });
            }
            return Json(treeEntities);
        }
        #endregion

        #region 获取所有流程
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("公用-获取所有流程")]
        public async Task<JsonResult> GetAllWorkflow()
        {
            //所有流程信息
            IList<TreeEntity> treeEntities = (await _dictionaryLogic.GetDictionaryByCode(ResourceDictionary.流程类别)).Select(dic => new TreeEntity { id = dic.DictionaryId, pId = Guid.Empty, name = dic.Name }).ToList();
            foreach (var workflow in await _workflowProcessLogic.GetAllEnumerableAsync())
            {
                treeEntities.Add(new TreeEntity { id = workflow.ProcessId, pId = workflow.ProcessType, name = workflow.Name });
            }
            return Json(treeEntities);
        }
        #endregion
    }
}