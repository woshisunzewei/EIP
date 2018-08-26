using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EIP.Common.Core.Utils
{
    /// <summary>
    /// 控件
    ///     拼接字符串输出到前台
    /// </summary>
    public class ControlUtil
    {
        #region 将服务器控件列表项转换为Radio项
        /// <summary>
        /// 将服务器控件列表项转换为Radio项
        /// </summary>
        /// <param name="items"></param>
        /// <param name="name"></param>
        /// <param name="otherAttr"></param>
        /// <returns></returns>
        public static string GetRadioString(ListItem[] items, 
            string name,
            string otherAttr = "")
        {
            StringBuilder options = new StringBuilder(items.Length * 50);
            foreach (var item in items)
            {
                string tempid = Guid.NewGuid().ToString("N");
                options.AppendFormat("<input type=\"radio\" value=\"{0}\" {1} id=\"{2}\" name=\"{3}\" {4} style=\"vertical-align:middle\" />",
                    item.Value.Replace("\"", "'"),
                    item.Selected ? "checked=\"checked\"" : "",
                    string.Format("{0}_{1}", name, tempid),
                    name,
                    otherAttr
                    );
                options.AppendFormat("<label style=\"vertical-align:middle;margin-right:2px;\" for=\"{0}\">", string.Format("{0}_{1}", name, tempid));
                options.Append(item.Text);
                options.Append("</label>");
            }
            return options.ToString();
        }
        #endregion

        #region 将服务器控件列表项转换为select列表项
        /// <summary>
        /// 将服务器控件列表项转换为select列表项
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string GetOptionsString(ListItem[] items)
        {
            StringBuilder options = new StringBuilder(items.Length * 50);
            foreach (var item in items)
            {
                options.AppendFormat("<option value=\"{0}\" {1}>", item.Value.Replace("\"", "'"), item.Selected ? "selected=\"selected\"" : "");
                options.Append(item.Text);
                options.Append("</option>");
            }
            return options.ToString();
        }
        #endregion

        #region 将服务器控件列表项转换为Checkbox项
        /// <summary>
        /// 将服务器控件列表项转换为Checkbox项
        /// </summary>
        /// <param name="items"></param>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <param name="otherAttr"></param>
        /// <returns></returns>
        public static string GetCheckBoxString(ListItem[] items,
            string name, 
            string[] values, 
            string otherAttr = "")
        {
            StringBuilder options = new StringBuilder(items.Length * 50);
            foreach (var item in items)
            {
                string tempid = Guid.NewGuid().ToString("N");
                options.AppendFormat("<input type=\"checkbox\" value=\"{0}\" {1} id=\"{2}\" name=\"{3}\" {4} style=\"vertical-align:middle\" />",
                    item.Value.Replace("\"", "'"),
                    values != null && values.Contains(item.Value) ? "checked=\"checked\"" : "",
                    string.Format("{0}_{1}", name, tempid),
                    name,
                    otherAttr
                    );
                options.AppendFormat("<label style=\"vertical-align:middle;margin-right:2px;\" for=\"{0}\">", string.Format("{0}_{1}", name, tempid));
                options.Append(item.Text);
                options.Append("</label>");
            }
            return options.ToString();
        }
        #endregion
    }
}