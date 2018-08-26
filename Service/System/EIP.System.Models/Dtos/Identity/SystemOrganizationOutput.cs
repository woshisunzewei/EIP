using EIP.Common.Entities.Dtos;
using EIP.System.Models.Entities;

namespace EIP.System.Models.Dtos.Identity
{
    public class SystemOrganizationOutput : SystemOrganization, IOutputDto
    {
        /// <summary>
        /// 性质名称
        /// </summary>
        public string NatureName { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 父级代码
        /// </summary>
        public string ParentCode { get; set; }
    }
}