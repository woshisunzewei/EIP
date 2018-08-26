using System.Collections.Generic;
using EIP.Common.Entities.Dtos;
using EIP.System.Models.Entities;
using EIP.System.Models.Entities;

namespace EIP.System.Models.Dtos.Permission
{
    public class SystemDataDoubleWayDto : SystemData, IDoubleWayDto
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
    }
}