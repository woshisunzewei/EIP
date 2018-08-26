using System;
using EIP.Common.Entities.Dtos;

namespace EIP.System.Models.Dtos.Permission
{
    public class SystemMenuEditInput : IdInput
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid ParentId { get; set; }
    }
}