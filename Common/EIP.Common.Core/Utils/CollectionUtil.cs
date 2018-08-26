using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;


namespace EIP.Common.Core.Utils
{
    /// <summary>
    /// 集合转换
    /// </summary>
    public static class CollectionUtil
    {
        #region List转DataTable
        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="list">集合数据</param>
        /// <returns></returns>
        public static DataTable ConvertTo<T>(IList<T> list)
        {
            var table = CreateTable<T>();
            var entityType = typeof(T);
            var properties = TypeDescriptor.GetProperties(entityType);
            foreach (T item in list)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// 创建data table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataTable CreateTable<T>()
        {
            var entityType = typeof(T);
            var table = new DataTable(entityType.Name);
            var properties = TypeDescriptor.GetProperties(entityType);
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            return table;
        }
        #endregion

        #region datarow转list
        /// <summary>
        /// datarow转list
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            return rows == null ? null : rows.Select(CreateItem<T>).ToList();
        }

        /// <summary>
        /// 创建list item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row == null) return obj;
            obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in row.Table.Columns)
            {
                var prop = obj.GetType().GetProperty(column.ColumnName);
                object value = row[column.ColumnName];
                prop.SetValue(obj, value, null);
            }

            return obj;
        }
        #endregion

        #region datatable转list
        /// <summary>
        /// datatable转list
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="table">table</param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            var rows = table.Rows.Cast<DataRow>().ToList();
            return ConvertTo<T>(rows);
        }
        #endregion
    }
}