using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using EIP.Common.Core.Quartz;
using EIP.Common.Core.Utils;
using EIP.Common.Entities.Dtos;
using Quartz;

namespace EIP.Job.Web.DataUser.Helpers
{
    /// <summary>
    ///     扩展Html帮助类
    /// </summary>
    public static class ExtensionHtmlHelper
    {
        #region 提醒信息

        /// <summary>
        ///     消息提示框
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MvcHtmlString MessageBox(this HtmlHelper htmlHelper, MessageBoxInput input)
        {
            var html =
                string.Format(@"<table class='messagebox' align='center' {0} cellpadding='0' cellspacing='0'>
                <tbody>
                    <tr class='head-no-title'>
                        <td class='left'></td>
                        <td class='center'></td>
                        <td class='right'></td>
                    </tr>
                    <tr class='msg'>
                        <td class='left'></td>
                        <td class='center {1}'>
                            <div class='msg-content'>
                                <strong style='font-size: 14px'>{2}</strong>
                            </div>
                        </td>
                        <td class='right'></td>
                    </tr>
                    <tr class='foot'>
                        <td class='left'></td>
                        <td class='center'><b></b></td>
                        <td class='right'></td>
                    </tr>
                </tbody>
            </table>", input.HtmlAttributes, input.MessageBoxType, input.Msg);
            return new MvcHtmlString(html);
        }

        #endregion

        #region 枚举控件

        /// <summary>
        ///     枚举控件下拉框
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListEnum(this HtmlHelper htmlHelper,
            DropDownListEnumInput input)
        {
            var stringBuilder =
                new StringBuilder("<select id='" + input.Id + "' name='" + input.Name + "'" + input.HtmlAttributes + ">");
            if (input.NeedDefault)
            {
                stringBuilder.Append("<option value=''>===请选择===</option>");
            }
            var dictionarys = EnumUtil.EnumToDictionary((Type)input.EnumType, e => e.ToString());

            foreach (var dic in dictionarys)
            {
                if (input.SelectedVal != null)
                {
                    switch (input.CompareType)
                    {
                        case EnumCompareType.Text:
                            if (dic.Value == input.SelectedVal)
                            {
                                stringBuilder.Append(" <option value='" + dic.Key + "' selected='selected'>" + dic.Value +
                                                     "</option> ");
                            }
                            else
                            {
                                stringBuilder.Append(" <option value='" + dic.Key + "' >" + dic.Value + "</option> ");
                            }
                            break;
                        case EnumCompareType.Value:
                            if (dic.Key == input.SelectedVal)
                            {
                                stringBuilder.Append(" <option value='" + dic.Key + "' selected='selected'>" + dic.Value +
                                                     "</option> ");
                            }
                            else
                            {
                                stringBuilder.Append(" <option value='" + dic.Key + "' >" + dic.Value + "</option> ");
                            }
                            break;
                        default:
                            stringBuilder.Append(" <option value='" + dic.Key + "' >" + dic.Value + "</option> ");
                            break;
                    }
                }
                else
                {
                    stringBuilder.Append(" <option value='" + dic.Key + "' >" + dic.Value + "</option> ");
                }
            }
            stringBuilder.Append("</select>");
            return new MvcHtmlString(stringBuilder.ToString());
        }

        /// <summary>
        ///     编辑界面头部备注
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input">Model</param>
        /// <returns></returns>
        public static MvcHtmlString EditTopRemark(this HtmlHelper htmlHelper,
            EditTopRemarkInput input)
        {
            var builder = new StringBuilder(2000);
            builder.Append(
                string.Format(
                    "<div class=\"edit-top\"><div class=\"content\"><div class=\"left\"></div><div class=\"center\">{0}</div><div class=\"right\"></div></div><div class=\"right\"></div><div class=\"edit-top-center\"><span style=\"color: red\">{1}</span></div>{2}</div>",
                    input.Title, input.Remark,input.HtmlAttributes));
            return new MvcHtmlString(builder.ToString());
        }

        /// <summary>
        ///     复选框Html扩展
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input">控件信息</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxHtmlString(this HtmlHelper htmlHelper,
            CheckBoxInput input)
        {
            var builder = new StringBuilder(2000);
            var checkboxStr = input.IsCheck
                ? string.Format(
                    "<input type=\"checkbox\" style=\"cursor: pointer\" name=\"{0}\" id=\"{0}\" checked=\"checked\" " + input.HtmlAttributes + "><label style=\"cursor: pointer\" for=\"{0}\">{1}</label>",
                    input.Name, input.Title)
                : string.Format(
                    "<input type=\"checkbox\" style=\"cursor: pointer\" name=\"{0}\" id=\"{0}\" " + input.HtmlAttributes + "><label style=\"cursor: pointer\" for=\"{0}\">{1}</label>",
                    input.Name, input.Title);
            builder.Append(checkboxStr);
            return new MvcHtmlString(builder.ToString());
        }
        
       
        #endregion

        #region 调度作业
        /// <summary>
        ///     消息提示框
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MvcHtmlString LoadJobAssemblies(this HtmlHelper htmlHelper, BaseDropDownListInput input)
        {
            IList<Assembly> assemblies = AssemblyUtil.GetAssemblyByFullName("EIP.Job.Service");
            SortedList<string, string> jobTypes = new SortedList<string, string>();
            foreach (var assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IJob).IsAssignableFrom(type) && type.IsClass)
                    {
                        jobTypes.Add(type.FullName, assembly.GetName().Name);
                    }
                }
            }

            StringBuilder stringBuilder = new StringBuilder(2000);
            stringBuilder.Append("<select id='" + input.Id + "' name='" + input.Name + "'" + input.HtmlAttributes + ">");
            if (input.NeedDefault)
            {
                stringBuilder.Append("<option value=''>===请选择===</option>");
            }

            foreach (var item in jobTypes)
            {
                if (input.SelectedVal != null)
                {
                    if (item.Key == input.SelectedVal)
                    {
                        stringBuilder.Append(" <option value='" + item.Value + "' selected='selected'>" + item.Key + "</option> ");
                    }
                    else
                    {
                        stringBuilder.Append(" <option value='" + item.Value + "' >" + item.Key + "</option> ");
                    }
                }
                else
                {
                    stringBuilder.Append(" <option value='" + item.Value + "' >" + item.Key + "</option> ");
                }
            }
            stringBuilder.Append("</select>");
            return new MvcHtmlString(stringBuilder.ToString());
        }

        /// <summary>
        ///     日历
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MvcHtmlString LoadQuartzCalendar(this HtmlHelper htmlHelper, BaseDropDownListInput input)
        {
            StringBuilder stringBuilder = new StringBuilder(2000);
            stringBuilder.Append("<select id='" + input.Id + "' name='" + input.Name + "'" + input.HtmlAttributes + ">");
            if (input.NeedDefault)
            {
                stringBuilder.Append("<option value=''>===请选择===</option>");
            }
            var calendars = StdSchedulerManager.GetCalendarNames().ToList().Select(n => new
            {
                Name = n,
                Calendar = StdSchedulerManager.GetCalendar(n)
            }).ToDictionary(n => n.Name, n => n.Calendar);
            foreach (var item in calendars)
            {
                if (input.SelectedVal != null)
                {
                    if (item.Key == input.SelectedVal)
                    {
                        stringBuilder.Append(" <option value='" + item.Key + "' selected='selected'>" + item.Key + "</option> ");
                    }
                    else
                    {
                        stringBuilder.Append(" <option value='" + item.Key + "' >" + item.Key + "</option> ");
                    }
                }
                else
                {
                    stringBuilder.Append(" <option value='" + item.Key + "' >" + item.Key + "</option> ");
                }
            }
            stringBuilder.Append("</select>");
            return new MvcHtmlString(stringBuilder.ToString());
        }
        #endregion

        #region 查询

        /// <summary>
        ///     文本查询:等于、包含
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id">控件id</param>
        /// <returns></returns>
        public static MvcHtmlString SearchTextBox(this HtmlHelper htmlHelper, string id)
        {
            var builder = new StringBuilder(2000);
            builder.Append(
                string.Format(
                    " <select name=\"op\"><option value=\"cn\">包含 </option><option value=\"eq\">等于 </option></select><input id=\"{0}\" name=\"{0}\" type=\"text\" class=\"filter\" />",
                    id));
            return new MvcHtmlString(builder.ToString());
        }

        /// <summary>
        ///     时间查询:等于、小于、小于等于、大于、大于等于
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id">控件id</param>
        /// <param name="loadonce">是否为一次性加载:默认为true</param>
        /// <param name="condition">时间条件:eg dateFmt:'yyyy-MM-dd HH:mm:ss'</param>
        /// <returns></returns>
        public static MvcHtmlString SearchDateTime(this HtmlHelper htmlHelper,
            string id,
            bool loadonce = true,
            string condition = "dateFmt:'yyyy-MM-dd'")
        {
            var builder = new StringBuilder(2000);
            builder.Append("<select name=\"op\">");
            builder.Append(loadonce ? " <option value=\"eq\">等于 </option>" : "<option value=\"time\">等于 </option>");
            builder.Append(
                    "<option value=\"lt\">小于 </option>" +
                    "<option value=\"le\">小于等于 </option>" +
                    "<option value=\"gt\">大于 </option>" +
                    "<option value=\"ge\">大于等于</option>" +
                    "</select>" +
                    "<input id=\"" + id + "\" type=\"text\" name=\"" + id + "\" class=\"Wdate filter\" onclick=\" WdatePicker() \" onFocus=\"WdatePicker({" + condition + "})\" />");
            return new MvcHtmlString(builder.ToString());
        }

        #endregion

    }
}