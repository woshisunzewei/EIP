using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DapperEx;
using EIP.Common.Entities.CustomAttributes;

namespace EIP.Common.Dapper
{
    public static class DapperCacheCommon
    {
        private static object objLock = new object();
        /// <summary>
        /// 用于缓存对象转换实体
        /// </summary>
        public static Dictionary<string, ModelDes> _ModelDesCache = new Dictionary<string, ModelDes>();
        /// <summary>
        /// 确定是否已经存在缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static ModelDes ExistModelDesCache(string key)
        {
            ModelDes value;
            _ModelDesCache.TryGetValue(key, out value);
            return value;
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="des"></param>
        private static void Add(string key, ModelDes des)
        {
            lock (objLock)
            {
                if ((!_ModelDesCache.ContainsKey(key)) && des != null)
                {
                    _ModelDesCache.Add(key, des);
                }
            }
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static ModelDes GetModelDesCache(string key)
        {
            ModelDes value;
            _ModelDesCache.TryGetValue(key, out value);
            if (value != null)
                return value;
            throw new Exception("缓存中没存在此数据");
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static ModelDes UpdateModelDesCache<T>()
        {
            var type = typeof(T);
            var cacheValue=ExistModelDesCache(type.FullName);
            if (cacheValue==null)
            {
                var model = new ModelDes();
                #region 表描述
                model.ClassType = type;
                model.ClassName = type.Name;
                var tbAttrObj = type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
                if (tbAttrObj != null)
                {
                    var tbAttr = tbAttrObj as TableAttribute;
                    if (!string.IsNullOrEmpty(tbAttr.Name))
                        model.TableName = tbAttr.Name;
                    else
                        model.TableName = model.ClassName;
                }
                else
                    model.TableName = model.ClassName;
                #endregion
                #region 属性描述
                foreach (var propertyInfo in type.GetProperties())
                {
                    var pty = new PropertyDes();
                    pty.Field = propertyInfo.Name;
                    var arri = propertyInfo.GetCustomAttributes(typeof(BaseAttribute), true).FirstOrDefault();
                    if (arri is IgnoreColumnAttribute)
                    {
                        continue;
                    }
                    if (arri is IdAttribute)
                    {
                        pty.CusAttribute = arri as IdAttribute;
                    }
                    else if (arri is ColumnAttribute)
                    {
                        pty.CusAttribute = arri as ColumnAttribute;
                    }
                    model.Properties.Add(pty);
                }
                #endregion
                Add(type.FullName, model);
                cacheValue = model;
            }
            return cacheValue;
        }
        /// <summary>
        /// 获取转换实体对象描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static ModelDes GetModelDes<T>()
        {
            return UpdateModelDesCache<T>();
        }
        /// <summary>
        /// 获取要执行SQL时的列,添加和修改数据时
       /// </summary>
       /// <param name="des"></param>
       /// <param name="add">是否是添加</param>
       /// <returns></returns>
        internal static IList<ParamColumnModel> GetExecColumns(ModelDes des,bool add=true)
        {
            var columns = new List<ParamColumnModel>();
            if (des != null && des.Properties != null)
            {
                foreach (var item in des.Properties)
                {
                    if ((!add)&&item.CusAttribute is IdAttribute)
                    {
                        continue;
                    }
                    else if ((item.CusAttribute is IdAttribute) && ((item.CusAttribute as IdAttribute).CheckAutoId))
                    {
                        continue;
                    }
                    else if ((item.CusAttribute is ColumnAttribute) && ((item.CusAttribute as ColumnAttribute).AutoIncrement))
                    {
                        continue;
                    }
                    columns.Add(new ParamColumnModel { ColumnName = item.Column, FieldName = item.Field });
                }
            }
            return columns;
        }
        /// <summary>
        /// 获取对象的主键标识列和属性
        /// </summary>
        /// <param name="des"></param>
        /// <returns></returns>
        internal static PropertyDes GetPrimary(ModelDes des)
        {
            var p = des.Properties.FirstOrDefault(m => m.CusAttribute is IdAttribute);
            if (p == null)
            {
                throw new Exception("没有任何列标记为主键特性");
            }
            return p;
        }

        /// <summary>
        /// 通过表达式树获取属性名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="des"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        internal static PropertyDes GetPropertyByExpress<T>(ModelDes des, Expression<Func<T, object>> expr) where T : class
        {
            var pname = "";
            if (expr.Body is UnaryExpression)
            {
                var uy = expr.Body as UnaryExpression;
                pname = (uy.Operand as MemberExpression).Member.Name;
            }
            else
            {
                pname = (expr.Body as MemberExpression).Member.Name;
            }
            var p = des.Properties.FirstOrDefault(m => m.Column == pname);
            if (p == null)
            {
                throw new Exception(string.Format("{0}不是映射列，不能进行SQL处理",pname));
            }
            return p;
        }
    }
}
