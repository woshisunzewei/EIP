using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;
namespace EIP.System.Models.Entities
{
    /// <summary>
    /// System_Dictionary表实体类
    /// </summary>
	[Serializable]
	[Table(Name = "System_Dictionary")]
    public  class SystemDictionary: EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [Id]
		public Guid DictionaryId{ get; set; }
       
        /// <summary>
        /// 父级id
        /// </summary>		
		public Guid ParentId{ get; set; }
        
        /// <summary>
        /// 字典代码
        /// </summary>		
		public string Code{ get; set; }
       
        /// <summary>
        /// 名称
        /// </summary>		
		public string Name{ get; set; }
       
        /// <summary>
        /// 值
        /// </summary>		
		public string Value{ get; set; }
       
        /// <summary>
        /// 是否允许删除(系统默认配置字段不允许删除)
        /// </summary>		
		public bool CanbeDelete{ get; set; }
       
        /// <summary>
        /// 排序
        /// </summary>		
		public int OrderNo{ get; set; }
       
        /// <summary>
        /// 备注
        /// </summary>		
		public string Remark{ get; set; }

       
   } 
}
