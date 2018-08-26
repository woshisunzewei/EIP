using System.IO;
using System.Reflection;
using log4net.Core;
using log4net.Layout.Pattern;

namespace EIP.Common.Core
{
    /// <summary>
    /// 支持属性的布局转换器
    /// </summary>
    public class PropertyLayoutConverter : PatternLayoutConverter
    {
        #region Overrides of PatternLayoutConverter

        /// <summary>
        /// 重写基类的转换器，根据配置的property key 获取消息对象上的相应属性
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="loggingEvent"></param>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (Option != null)
            {
                // Write the value for the specified key
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                // Write all the key value pairs
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }

        #endregion

        /// <summary>
        /// 通过反射获取传入的日志对象的某个属性的值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="loggingEvent"></param>
        /// <returns></returns>
        private static object LookupProperty(string property, LoggingEvent loggingEvent)
        {
            object messageObject = loggingEvent.MessageObject;
            PropertyInfo propertyInfo = messageObject.GetType().GetProperty(property);

            object propertyValue = propertyInfo != null ? propertyInfo.GetValue(messageObject, null) : string.Empty;
            return propertyValue;
        }
    }
}