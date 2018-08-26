using System;

namespace EIP.Common.Core.Utils
{
    /// <summary>
    /// 特性帮助类
    /// </summary>
    public class AttributeUtil
    {
        #region 根据特性名称获取特性
        /// <summary>
        /// 根据特性名称获取特性
        /// </summary>
        /// <typeparam name="T">自定义特性</typeparam>
        /// <param name="methodname">特性名称</param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object[] GetAttribute<T>(string methodname, Type t)
        {
            return t.GetMethod(methodname).GetCustomAttributes(typeof(T), true);
        }
        #endregion
    }
}