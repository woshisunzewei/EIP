namespace EIP.Common.Entities.Paging
{
    /// <summary>
    ///     说  明:分页查询参数
    ///     备  注:当分页需要传入查询参数时,使用该类
    ///     编写人:孙泽伟-2015/03/27
    /// </summary>
    public class QuerySearch
    {
        /// <summary>
        ///     参数键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     参数值
        /// </summary>
        public string Value { get; set; }
    }
}