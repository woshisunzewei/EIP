using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace EIP.Common.Core.Extensions
{
    /// <summary>
    /// Json扩展
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// 字符串序列化为集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<T> JsonStringToList<T>(this string jsonStr)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<T> objs = serializer.Deserialize<List<T>>(jsonStr);
            return objs;
        }

        /// <summary>
        /// 字符串序列化为集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T JsonStringToObject<T>(this string jsonStr)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            T objs = serializer.Deserialize<T>(jsonStr);
            return objs;
        }

        /// <summary>
        /// 字符串序列化为集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ListToJsonString<T>(IEnumerable<T> t)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(t);
        }
    }
}