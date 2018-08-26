using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;
namespace EIP.System.Models.Entities
{
    /// <summary>
    /// System_Organization表实体类
    /// </summary>
    [Serializable]
    [Table(Name = "System_Organization")]
    public  class SystemOrganization : EntityBase
    {
        /// <summary>
        /// 组织机构id
        /// </summary>		
        [Id]
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构父id
        /// </summary>		
        public Guid ParentId { get; set; }

        /// <summary>
        /// 代码
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>		
        public string ShortName { get; set; }

        /// <summary>
        /// 主负责人
        /// </summary>		
        public string MainSupervisor { get; set; }

        /// <summary>
        /// 主负责人联系方式
        /// </summary>		
        public string MainSupervisorContact { get; set; }

        /// <summary>
        /// 状态:0外部、2临时
        /// </summary>		
        public short? State { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 性质:0集团,1公司,2分公司,3子公司,4部门
        /// </summary>
        public int? Nature { get; set; }

    }
}
