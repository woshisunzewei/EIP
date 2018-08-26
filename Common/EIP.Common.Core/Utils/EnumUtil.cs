using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace EIP.Common.Core.Utils
{
    /// <summary>
    ///     枚举工具
    /// </summary>
    public static class EnumUtil
    {
        #region 获取枚举类型对象集合
        /// <summary>
        ///     获取枚举类型对象集合
        /// </summary>
        public static List<NameValueObject<int>> GetObjectsFromEnum(string typeName)
        {
            var enumType = Type.GetType(typeName);
            return GetObjectsFromEnum(enumType);
        }

        /// <summary>
        ///     获取枚举类型对象集合
        /// </summary>
        public static List<NameValueObject<int>> GetObjectsFromEnum(Type enumType)
        {
            if (enumType == null || enumType == (Type)Type.Missing || !enumType.IsEnum)
            {
                return null;
            }
            var objs = new List<NameValueObject<int>>();
            var typeDescription = typeof(DescriptionAttribute);
            var fields = enumType.GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    var obj = new NameValueObject<int>();
                    var arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        var aa = (DescriptionAttribute)arr[0];
                        obj.Name = aa.Description;
                    }
                    else
                    {
                        obj.Name = field.Name;
                    }
                    var enumName = enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null).ToString();
                    obj.Value = Convert.ToInt32(Enum.Parse(enumType, enumName));
                    objs.Add(obj);
                }
            }
            return objs;
        }
        #endregion

        #region 获取枚举类型上Description特性
        /// <summary>
        ///     获取枚举类型上Description特性
        /// </summary>
        /// <param name="enumObj1"></param>
        /// <returns></returns>
        public static string GetDescription(Object enumObj1)
        {
            var enumObj = Convert.ToInt32(enumObj1);
            var enumType = enumObj1.GetType();
            // 获得特性Description的类型信息
            var typeDescription = typeof(DescriptionAttribute);
            // 获得枚举的字段信息（因为枚举的值实际上是一个static的字段的值）
            var fields = enumType.GetFields();
            // 检索所有字段
            foreach (var field in fields)
            {
                // 过滤掉一个不是枚举值的，记录的是枚举的源类型
                if (field.FieldType.IsEnum == false)
                    continue;
                // 通过字段的名字得到枚举的值
                var value = (int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                if (value == enumObj)
                {
                    var arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        // 因为Description自定义特性不允许重复，所以只取第一个
                        var aa = (DescriptionAttribute)arr[0];
                        // 获得特性的描述值
                        return aa.Description;
                    }
                }
            }
            return enumObj.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     返回指定枚举类型的指定值的描述
        /// </summary>
        /// <param name="t">枚举类型</param>
        /// <param name="v">枚举值</param>
        /// <returns></returns>
        public static string GetDescription(Type t,
            object v)
        {
            try
            {
                var fi = t.GetField(GetName(t, v));
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 根据下标值获取枚举名称
        /// </summary>
        /// <param name="t"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string GetName(Type t,
            object v)
        {
            try
            {
                return Enum.GetName(t, v);
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 获取枚举所有成员名称

        /// <summary>
        ///     获取枚举所有成员名称
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        public static string[] GetMemberNames<T>()
        {
            return Enum.GetNames(typeof(T));
        }

        #endregion

        #region 获取枚举所有成员值

        /// <summary>
        ///     获取枚举所有成员值
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        public static Array GetMemberValues<T>()
        {
            return Enum.GetValues(typeof(T));
        }

        #endregion

        #region 获取枚举的基础类型

        /// <summary>
        ///     获取枚举的基础类型
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        public static Type GetUnderlyingType(Type enumType)
        {
            //获取基础类型
            return Enum.GetUnderlyingType(enumType);
        }

        #endregion

        #region 检测枚举是否包含指定成员

        /// <summary>
        ///     检测枚举是否包含指定成员
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        /// <param name="member">枚举成员名或成员值</param>
        public static bool IsDefined<T>(string member)
        {
            return Enum.IsDefined(typeof(T), member);
        }

        #endregion

        #region 获取状态
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static SortedList GetStatus(Type t)
        {
            var list = new SortedList();
            var a = Enum.GetValues(t);
            for (var i = 0; i < a.Length; i++)
            {
                var enumName = a.GetValue(i).ToString();
                var enumKey = (int)Enum.Parse(t, enumName);
                var enumDescription = GetDescription(t, enumKey);
                list.Add(enumKey, enumDescription);
            }
            return list;
        }
        #endregion

        #region 根据下标值获取枚举名称
        /// <summary>
        /// 根据下标获取枚举名称
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetEnumNameByIndex<T>(int index)
        {
            return Enum.GetName(typeof(T), index);
        }
        #endregion

        #region 转换Enum到SelectListItem
        /// <summary>
        ///     转换Enum到SelectListItem (int类型)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="selectList"></param>
        public static void ToListItemForInt<T>(ref List<SelectListItem> selectList) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型");
            selectList.AddRange(from int s in Enum.GetValues(typeof(T))
                                select new SelectListItem
                                {
                                    Value = s.ToString(),
                                    Text = Enum.GetName(typeof(T), s)
                                });
        }

        /// <summary>
        ///     转换Enum到SelectListItem (byte类型)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="selectList"></param>
        public static void ToListItemForByte<T>(ref List<SelectListItem> selectList) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型");
            selectList.AddRange(from byte s in Enum.GetValues(typeof(T))
                                select new SelectListItem
                                {
                                    Value = s.ToString(),
                                    Text = Enum.GetName(typeof(T), s)
                                });
        }
        #endregion

        #region 把枚举转换为键值对集合
        /// <summary>
        /// 把枚举转换为键值对集合
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="getText">获得值得文本</param>
        /// <returns>以枚举值为key，枚举文本为value的键值对集合</returns>
        public static Dictionary<Int32, String> EnumToDictionary(Type enumType, Func<Enum, String> getText)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("传入的参数必须是枚举类型！", "enumType");
            }
            Dictionary<Int32, String> enumDic = new Dictionary<int, string>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (Enum enumValue in enumValues)
            {
                Int32 key = Convert.ToInt32(enumValue);
                String value = getText(enumValue);
                enumDic.Add(key, value);
            }
            return enumDic;
        }
        #endregion
    }

    public class NameValueObject<T>
    {
        /// <summary>
        ///     名称
        /// </summary>
        private string _name;

        /// <summary>
        ///     值
        /// </summary>
        private T _value;

        public NameValueObject()
        {
        }

        public NameValueObject(string name, T value)
        {
            _name = name;
            _value = value;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}