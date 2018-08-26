using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using EIP.Common.Core.Attributes;
using EIP.Common.Core.Extensions;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Web;
using EIP.System.Business.Identity;
using EIP.System.Models.Dtos.Identity;
using EIP.System.Models.Entities;

namespace EIP.Web.Areas.System.Controllers
{
    /// <summary>
    ///     组织机构
    /// </summary>
    public class OrganizationController : BaseController
    {
        #region 构造函数

        private readonly ISystemOrganizationLogic _organizationLogic;

        public OrganizationController(ISystemOrganizationLogic organizationLogic)
        {
            _organizationLogic = organizationLogic;
        }

        #endregion

        #region 视图

        /// <summary>
        ///     列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("组织机构维护-视图-列表")]
        public ViewResultBase List()
        {
            return View();
        }

        /// <summary>
        ///     组织机构基础信息
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Description("组织机构维护-视图-编辑")]
        public async Task<ViewResultBase> Edit(SystemOrganizationInput input)
        {
            var orgOutput = new SystemOrganizationOutput();

            //如果为编辑
            if (!input.Id.IsEmptyGuid())
            {
                var menu = await _organizationLogic.GetByIdAsync(input.Id);
                orgOutput = menu.MapTo<SystemOrganizationOutput>();

                //获取父级信息
                var parentInfo = await _organizationLogic.GetByIdAsync(orgOutput.ParentId);
                if (parentInfo != null)
                {
                    orgOutput.ParentName = parentInfo.Name;
                    orgOutput.ParentCode = parentInfo.Code;
                }
            }
            //新增
            else
            {
                if (!input.ParentId.IsEmptyGuid())
                {
                    var parentInfo = await _organizationLogic.GetByIdAsync(input.ParentId);
                    orgOutput.Code = parentInfo.Code;
                    orgOutput.ParentId = input.ParentId;
                    orgOutput.ParentName = parentInfo.Name;
                    orgOutput.ParentCode = parentInfo.Code;
                }
            }
            return View(orgOutput);
        }

        #endregion

        #region 方法

        ///// <summary>
        /////     读取组织机构树
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[CreateBy("孙泽伟")]
        //[Description("组织机构维护-方法-列表-读取组织机构树")]
        //public async Task<JsonResult> GetOrganizationTreeAsync(IdInput input)
        //{
        //    return Json(await _organizationLogic.GetOrganizationTreeAsync(input));
        //}

        /// <summary>
        ///     读取组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("组织机构维护-方法-列表-读取组织机构树")]
        public async Task<JsonResult> GetOrganizationTree()
        {
            return Json(await _organizationLogic.GetOrganizationTree());
        }

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("组织机构维护-方法-新增/编辑-检测代码是否已经具有重复项")]
        public async Task<JsonResult> CheckOrganizationCode(CheckSameValueInput input)
        {
            var operateStatus = await _organizationLogic.CheckOrganizationCode(input);
            return
                Json(
                    new
                    {
                        info = operateStatus.Message,
                        status = operateStatus.ResultSign == ResultSign.Successful ? "y" : "n"
                    });
        }

        /// <summary>
        ///     读取组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("组织机构维护-方法-新增/编辑-读取组织机构树")]
        public async Task<JsonResult> GetOrganizationResultByTreeId(IdInput input)
        {
            return Json(await _organizationLogic.GetOrganizationResultByTreeId(input));
        }

        /// <summary>
        ///     保存组织机构数据
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("组织机构维护-方法-新增/编辑-保存")]
        public async Task<JsonResult> SaveOrganization(SystemOrganization organization)
        {
            organization.CreateUserId = CurrentUser.UserId;
            organization.CreateUserName = CurrentUser.Name;
            return Json(await _organizationLogic.SaveOrganization(organization));
        }

        /// <summary>
        ///     删除组织机构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("组织机构维护-方法-列表-删除")]
        public async Task<JsonResult> DeleteOrganization(IdInput input)
        {
            return Json(await _organizationLogic.DeleteOrganization(input));
        }

        /// <summary>
        ///     批量生成代码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("组织机构维护-方法-列表-批量生成代码")]
        public async Task<JsonResult> GeneratingCode()
        {
            return Json(await _organizationLogic.GeneratingCode());
        }

        ///// <summary>
        ///// 读取Ladp信息
        ///// </summary>
        ///// <param name="adHelper">Ladp基础信息</param>
        ///// <returns></returns>
        //public async Task<JsonResult>  GetLadp(ADHelper adHelper, Guid companyId)
        //{
        //    OperationResult or = new OperationResult();
        //    DirectoryEntry root = null;
        //    root = new DirectoryEntry(adHelper.LdapPath, adHelper.AdminName, adHelper.AdminPassword);
        //    DirectoryContext context = new DirectoryContext(root, SearchScope.OneLevel);
        //    List<DirectoryOrganizationalUnit> ous = context.GetEntities<DirectoryOrganizationalUnit>();


        //    foreach (DirectoryOrganizationalUnit ou in ous)
        //    {

        //        Organization sStruct = new Organization();

        //        //获取对应组织下人员信息
        //        //var list = ou.GetUsers();
        //        sStruct.Name = ou.Name;
        //        sStruct.Id = Guid.NewGuid();
        //        sStruct.Pid = new Guid();
        //        sStruct.CompanyId = companyId;

        //        sStruct.Sort = 0;
        //        sStruct.CreatePerson = "admin";
        //        sStruct.CreateTime = DateTime.Now;
        //        sStruct.IsFreeze = false;
        //        //GetUsersInfo(ou, sStruct.Id);
        //        orgList.Add(sStruct);

        //        //获取子组织机构
        //        GetChildDepartment(ou, sStruct.Id.ToString(), companyId);

        //    }
        //    //获取所有组织机构信息
        //    orgList.RemoveAt(0);
        //    GetInstance<IOrganizationLogic>().InsertList(orgList, out or);
        //    return Json(or);
        //}
        ///// <summary>
        ///// 递归遍历组织机构树
        ///// </summary>
        ///// <param name="ou"></param>
        ///// <param name="ParentId"></param>
        ///// <param name="companyId"></param>
        //private void GetChildDepartment(DirectoryOrganizationalUnit ou, string ParentId, Guid companyId)
        //{
        //    var ous = ou.GetEntities<DirectoryOrganizationalUnit>();

        //    foreach (var item in ous)
        //    {
        //        Organization structs = new Organization();
        //        //var list = item.GetUsers();
        //        structs.Name = item.Name;
        //        structs.Id = Guid.NewGuid();
        //        structs.Pid = new Guid(ParentId);
        //        structs.CompanyId = companyId;
        //        structs.Sort = 0;
        //        structs.CreatePerson = "admin";
        //        structs.CreateTime = DateTime.Now;
        //        structs.IsFreeze = false;
        //        orgList.Add(structs);
        //        GetChildDepartment(item, structs.Id.ToString(), companyId);

        //    }
        //}

        #endregion
    }
}