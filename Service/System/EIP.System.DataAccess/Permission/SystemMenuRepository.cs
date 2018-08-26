using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EIP.Common.Core.Extensions;
using EIP.Common.Dapper;
using EIP.Common.DataAccess;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.System.Models.Entities;

namespace EIP.System.DataAccess.Permission
{
    public class SystemMenuRepository : DapperAsyncRepository<SystemMenu>, ISystemMenuRepository
    {
        /// <summary>
        ///     查询所有导航菜单
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetAllPortalMenu()
        {
            var sql = new StringBuilder();
            sql.Append(
                "SELECT MenuId id,ParentId pId,name,openUrl,icon,code FROM System_Menu WHERE IsFreeze=@isFreeze ORDER BY OrderNo");
            return SqlMapperUtil.SqlWithParams<TreeEntity>(sql.ToString(), new { isFreeze = false });
        }

        /// <summary>
        ///     查询所有菜单
        /// </summary>
        /// <param name="haveUrl">是否具有菜单</param>
        /// <param name="isMenuShow">是否菜单显示</param>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetAllMenu(bool haveUrl = false,
            bool isMenuShow = false)
        {
            var sql = new StringBuilder();
            sql.Append(
                "SELECT menu.MenuId id,menu.ParentId pId,menu.name,menu.icon,menu.code");
            if (haveUrl)
            {
                sql.Append(",((isnull(app.Domain,'')+menu.OpenUrl)) url");
            }
            sql.Append(" FROM System_Menu menu LEFT JOIN System_App app ON app.AppId=menu.AppId");
            if (isMenuShow)
            {
                sql.Append(" WHERE menu.IsShowMenu='true'");
            }
            sql.Append(" ORDER BY menu.OrderNo");
            return SqlMapperUtil.SqlWithParams<TreeEntity>(sql.ToString());
        }

        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SystemMenu>> GetMeunuByPId(IdInput input)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT * FROM System_Menu WHERE ParentId=@pid ORDER BY OrderNo");
            return SqlMapperUtil.SqlWithParams<SystemMenu>(sql.ToString(), new { pid = input.Id });
        }

        /// <summary>
        ///     检查模块代码:唯一性检查
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public Task<bool> CheckMenuCode(CheckSameValueInput input)
        {
            var sql = "SELECT Code FROM System_Menu WHERE Code=@param";
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql += " AND MenuId!=@menuId";
            }
            return SqlMapperUtil.SqlWithParamsBool<SystemMenu>(sql, new
            {
                param = input.Param,
                menuId = input.Id
            });
        }

        /// <summary>
        ///     查询所有具有菜单权限的菜单
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetHaveMenuPermissionMenu()
        {
            var sql = new StringBuilder();
            sql.Append(
                "SELECT MenuId id,ParentId pId,name,icon,code FROM System_Menu WHERE HaveMenuPermission=@haveMenuPermission AND IsFreeze=@isFreeze ORDER BY OrderNo");
            return SqlMapperUtil.SqlWithParams<TreeEntity>(sql.ToString(),
                new { haveMenuPermission = true, isFreeze = false });
        }

        /// <summary>
        ///     查询所有具有数据权限的菜单
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetHaveDataPermissionMenu()
        {
            var sql = new StringBuilder();
            sql.Append(
                "SELECT MenuId id,ParentId pId,name,icon,code FROM System_Menu WHERE HaveDataPermission=@haveDataPermission AND IsFreeze=@isFreeze ORDER BY OrderNo");
            return SqlMapperUtil.SqlWithParams<TreeEntity>(sql.ToString(),
                new { haveDataPermission = true, isFreeze = false });
        }

        /// <summary>
        ///     查询所有具有字段权限的菜单
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetHaveFieldPermissionMenu()
        {
            var sql = new StringBuilder();
            sql.Append(
                "SELECT MenuId id,ParentId pId,name,icon,code FROM System_Menu WHERE HaveFieldPermission=@haveFieldPermission AND IsFreeze=@isFreeze ORDER BY OrderNo");
            return SqlMapperUtil.SqlWithParams<TreeEntity>(sql.ToString(),
                new { haveFieldPermission = true, isFreeze = false });
        }

        /// <summary>
        ///     查询所有具有功能项的菜单
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TreeEntity>> GetHaveMenuButtonPermissionMenu()
        {
            var sql = new StringBuilder();
            sql.Append(
                "SELECT MenuId id,ParentId pId,name,icon,code FROM System_Menu WHERE HaveFunctionPermission=@haveFunctionPermission AND IsFreeze=@isFreeze ORDER BY OrderNo");
            return SqlMapperUtil.SqlWithParams<TreeEntity>(sql.ToString(),
                new { haveFunctionPermission = true, isFreeze = false });
        }
    }
}