using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EIP.Common.Core.Utils
{
    /// <summary>
    /// 程序集帮助类
    /// </summary>
    public class AssemblyUtil
    {
        /// <summary>
        /// 根据关键名称获取程序集信息
        /// </summary>
        /// <param name="fullName">关键字</param>
        /// <returns></returns>
        public static IList<Assembly> GetAssemblyByFullName(string fullName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                   .Where(a => a.FullName.StartsWith(fullName, StringComparison.OrdinalIgnoreCase))
                   .OrderBy(a => a.FullName).ToList();
        }

        /// <summary>
        /// 根据名称反射出值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);
            if (property == null) return string.Empty;
            object o = property.GetValue(t, null);
            if (o == null) return string.Empty;
            return o.ToString();
        }
    }
}