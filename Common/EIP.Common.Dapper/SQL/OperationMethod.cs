namespace EIP.Common.Dapper.SQL
{
    /// <summary>
    /// SQL操作符
    /// </summary>
    public enum OperationMethod
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 1,
        /// <summary>
        /// 小于
        /// </summary>
        Less = 2,
        /// <summary>
        /// 大于
        /// </summary>
        Greater = 3,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessOrEqual = 4,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterOrEqual = 5,
        /// <summary>
        /// 包含%-%
        /// </summary>
        Contains = 6,
        /// <summary>
        /// 开始包含-%
        /// </summary>
        StartsWith = 7,
        /// <summary>
        /// 结束包含%-
        /// </summary>
        EndsWith = 8,
        /// <summary>
        /// IN
        /// </summary>
        In = 9,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 10,
    }
}
