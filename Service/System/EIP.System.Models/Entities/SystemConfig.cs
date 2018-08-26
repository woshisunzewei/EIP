using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;
namespace EIP.System.Models.Entities
{
    /// <summary>
    /// System_Config表实体类
    /// </summary>
	[Serializable]
	[Table(Name = "System_Config")]
    public  class SystemConfig :  EntityBase
    {
        /// <summary>
        /// 配置主键
        /// </summary>		
		[Id]
        public Guid ConfigId{ get; set; }
       
        /// <summary>
        /// 配置项代码
        /// </summary>		
		public string Code{ get; set; }
       
        /// <summary>
        /// 值
        /// </summary>		
		public string Value{ get; set; }
       
        /// <summary>
        /// 备注
        /// </summary>		
		public string Remark{ get; set; }

   } 
}
