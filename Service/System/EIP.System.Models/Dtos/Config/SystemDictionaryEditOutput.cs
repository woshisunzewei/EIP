using EIP.System.Models.Entities;

namespace EIP.System.Models.Dtos.Config
{
    public class SystemDictionaryEditOutput:SystemDictionary
    {
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