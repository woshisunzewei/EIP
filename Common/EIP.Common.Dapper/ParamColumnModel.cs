using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperEx
{
    /// <summary>
    /// 生成SQL时参数里面的列名和对应值名称
    /// </summary>
    internal class ParamColumnModel
    {
        /// <summary>
        /// 数据库列名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 对应类属性名
        /// </summary>
        public string FieldName { get; set; }
    }
}
