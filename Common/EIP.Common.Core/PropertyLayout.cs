using log4net.Layout;

namespace EIP.Common.Core
{
    /// <summary>
    /// 支持属性的布局方式
    /// </summary>
    public class PropertyLayout:PatternLayout
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>添加<see cref="PropertyLayoutConverter"/></remarks>
        public PropertyLayout()
        {
            AddConverter("property",typeof(PropertyLayoutConverter));
        }
    }
}
