using EIP.Common.Entities.Dtos;


namespace EIP.System.Models.Common.Dtos
{
    public class GlobalGetDictionaryInput:IInputDto
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentName { get; set; }
    }
}