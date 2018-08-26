using System;
using EIP.Common.Entities;
using EIP.Common.Entities.CustomAttributes;
namespace EIP.System.Models.Entities
{
    /// <summary>
    /// System_MenuButton表实体类
    /// </summary>
	[Serializable]
	[Table(Name = "System_MenuButton")]
    public class SystemMenuButton: EntityBase
    {
        /// <summary>
        /// 
        /// </summary>		
		[Id]
        public Guid MenuButtonId { get; set; }
       
        /// <summary>
        /// 菜单id
        /// </summary>		
		public Guid MenuId{ get; set; }
       
        /// <summary>
        /// 若是按钮则可为其添加图标
        /// </summary>		
		public string Icon{ get; set; }
       
        /// <summary>
        /// 名称:新增、删除、编辑、读取数据....
        /// </summary>		
		public string Name{ get; set; }
       
        /// <summary>
        /// 脚本方法
        /// </summary>		
		public string Script{ get; set; }
       
        /// <summary>
        /// 排序
        /// </summary>		
		public int OrderNo{ get; set; }
       
        /// <summary>
        /// 是否有分割线
        /// </summary>		
		public bool Divider{ get; set; }
      
        /// <summary>
        /// 备注
        /// </summary>		
		public string Remark{ get; set; }
   } 
}
