using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using EIP.Common.Core.Utils;
using EIP.Common.Entities.Dtos;
using EIP.System.Business.Config;
using EIP.System.DataAccess.Config;
using EIP.System.Models.Entities;
using EIP.Workflow.DataAccess.Config;

namespace EIP.Web.DataUsers.Helpers
{
    /// <summary>
    ///     扩展Html帮助类
    /// </summary>
    public static class ExtensionHtmlHelper
    {
        #region 获取字典下拉数据

        /// <summary>
        ///     字典下拉框
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input">下拉实体</param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListDictionary(this HtmlHelper htmlHelper,
            DropDownListDictionaryInput input)
        {
            var dictionaryBusiness = new SystemDictionaryLogic();
            var dictionaries = Task.Run(async () => await dictionaryBusiness.GetDictionaryByCode(input.Code)).Result;
            var list = new List<SelectListItem>();
            if (input.NeedDefault)
            {
                list.Add(new SelectListItem
                {
                    Value = "",
                    Text = @"===请选择==="
                });
            }
            list.AddRange(dictionaries.Select(o => new SelectListItem { Text = o.Name, Value = o.DictionaryId.ToString() }));
            if (input.SelectedVal != null)
            {
                var item = list.Find(o => o.Value == input.SelectedVal.ToString());
                if (item != null)
                    item.Selected = true;
            }
            return htmlHelper.DropDownList(input.Name, list, input.HtmlAttributes);
        }

        #endregion

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
                stringBuilder.Append("<option value=''>=请选择=</option>");
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
                    input.Title, input.Remark, input.HtmlAttributes));
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

        /// <summary>
        /// 应用系统下拉框
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListApp(this HtmlHelper htmlHelper, BaseDropDownListInput input)
        {
            StringBuilder stringBuilder = new StringBuilder(2000);
            SystemAppRepository repository = new SystemAppRepository();

            IList<SystemApp> apps = Task.Run(async () => await repository.GetAllEnumerableAsync()).Result.OrderBy(o => o.OrderNo).ToList();
            stringBuilder.Append("<select id='" + input.Id + "' name='" + input.Name + "'" + input.HtmlAttributes + ">");
            if (input.NeedDefault)
            {
                stringBuilder.Append("<option value=''>===请选择===</option>");
            }

            foreach (var app in apps)
            {
                if (input.SelectedVal != null)
                {
                    if ((app.AppId.ToString()) == input.SelectedVal)
                    {
                        stringBuilder.Append(" <option value='" + app.AppId + "' selected='selected'>" + app.Name + "</option> ");
                    }
                    else
                    {
                        stringBuilder.Append(" <option value='" + app.AppId + "' >" + app.Name + "</option> ");
                    }
                }
                else
                {
                    stringBuilder.Append(" <option value='" + app.AppId + "' >" + app.Name + "</option> ");
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
        /// <param name="name">显示的名称</param>
        /// <returns></returns>
        public static MvcHtmlString SearchTextBox(this HtmlHelper htmlHelper, string id, string name)
        {
            var builder = new StringBuilder(2000);
            builder.Append(
                string.Format(
                    //"{1}：<select name=\"op\"><option value=\"cn\">包含 </option><option value=\"eq\">等于 </option></select><input placeholder=\"请输入{1}查询\" id=\"{0}\" name=\"{0}\" type=\"text\" class=\"filter\" />",
                     "{1}：<input type=\"hidden\" value=\"cn\" /><input placeholder=\"请输入{1}查询\" id=\"{0}\" name=\"{0}\" type=\"text\" class=\"filter\" />",
                    id, name));
            return new MvcHtmlString(builder.ToString());
        }

        /// <summary>
        ///     时间查询:等于、小于、小于等于、大于、大于等于
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id">控件id</param>
        /// <param name="loadonce">是否为一次性加载:默认为true</param>
        /// <param name="condition">时间条件:eg dateFmt:'yyyy-MM-dd HH:mm:ss'</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString SearchDateTime(this HtmlHelper htmlHelper,
            string id,
            string name = "",
            bool loadonce = true,
            string condition = "dateFmt:'yyyy-MM-dd'"
           )
        {
            var builder = new StringBuilder(2000);
            builder.Append("" + name + "：<select name=\"op\">");
            builder.Append(loadonce ? " <option value=\"eq\">等于 </option>" : "<option value=\"time\">等于 </option>");
            builder.Append(string.Format(
                    "<option value=\"lt\">小于 </option>" +
                    "<option value=\"le\">小于等于 </option>" +
                    "<option value=\"gt\">大于 </option>" +
                    "<option value=\"ge\">大于等于</option>" +
                    "</select>" +
                    "<input placeholder=\"请输入{0}查询\" id=\"" + id + "\" type=\"text\" name=\"" + id + "\" class=\"Wdate filter\" onclick=\" WdatePicker() \" onFocus=\"WdatePicker({1})\" />", name, "{" + condition + "}"));
            return new MvcHtmlString(builder.ToString());
        }

        #endregion

        #region 流程
        /// <summary>
        /// 流程按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString LoadWorkflowButton(this HtmlHelper htmlHelper)
        {
            //获取所有流程按钮
            WorkflowButtonRepository workflowButtonRepository = new WorkflowButtonRepository();
            StringBuilder stringBuilder = new StringBuilder(2000);
            foreach (var item in Task.Run(async () => await workflowButtonRepository.GetAllEnumerableAsync()).Result)
            {
                stringBuilder.Append("<ul class=\"listulli\" script=\"" + item.Script + "\" note=\"" + item.Remark + "\" title=\"" + item.Remark + "\" val=\"" + item.ButtonId + "\" onmouseover=\"$(this).removeClass().addClass('listulli1'); \" onmouseout=\" if ($currentButton == null || $currentButton.get(0) !== this) {$(this).removeClass().addClass('listulli');} \" onclick=\" button_click(this); \" ondblclick=\" button_dblclick(this) \"><label style=\"background: url(/Contents/images/icons/" + item.Icon + ".png) no-repeat left; padding-left: 20px;\">" + item.Title + "</label></ul>");
            }
            return new MvcHtmlString(stringBuilder.ToString());
        }

        /// <summary>
        /// 流程表单
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MvcHtmlString LoadWorkflowForm(this HtmlHelper htmlHelper, BaseDropDownListInput input)
        {
            //获取所有流程按钮
            WorkflowFormRepository workflowFormRepository = new WorkflowFormRepository();
            var stringBuilder =
                 new StringBuilder("<select id='" + input.Id + "' name='" + input.Name + "'" + input.HtmlAttributes + ">");
            if (input.NeedDefault)
            {
                stringBuilder.Append("<option value=''>===请选择===</option>");
            }
            var dictionarys = Task.Run(async () => await workflowFormRepository.GetAllEnumerableAsync()).Result;
            foreach (var dic in dictionarys)
            {
                if (input.SelectedVal != null)
                {
                    if (dic.FormId == input.SelectedVal)
                    {
                        stringBuilder.Append(" <option value='" + dic.FormId + "' selected='selected'>" + dic.Name +
                                             "</option> ");
                    }
                    else
                    {
                        stringBuilder.Append(" <option value='" + dic.FormId + "' >" + dic.Name + "</option> ");
                    }
                }
                else
                {
                    stringBuilder.Append(" <option value='" + dic.FormId + "' >" + dic.Name + "</option> ");
                }
            }
            stringBuilder.Append("</select>");
            return new MvcHtmlString(stringBuilder.ToString());
        }
        #endregion

        #region 表字段

        /// <summary>
        /// 流程表单
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MvcHtmlString LoadDataBase(this HtmlHelper htmlHelper, BaseDropDownListInput input)
        {
            //获取所有数据库连接
            SystemDataBaseRepository dataBaseLogic = new SystemDataBaseRepository();
            var stringBuilder =
                 new StringBuilder("<select id='" + input.Id + "' name='" + input.Name + "'" + input.HtmlAttributes + ">");
            if (input.NeedDefault)
            {
                stringBuilder.Append("<option value=''>===请选择===</option>");
            }
            var dictionarys = Task.Run(async () => await dataBaseLogic.GetAllEnumerableAsync()).Result;
            foreach (var dic in dictionarys)
            {
                if (input.SelectedVal != null)
                {
                    if (dic.DataBaseId == input.SelectedVal)
                    {
                        stringBuilder.Append(" <option value='" + dic.DataBaseId + "' selected='selected'>" + dic.Name + "</option> ");
                    }
                    else
                    {
                        stringBuilder.Append(" <option value='" + dic.DataBaseId + "' >" + dic.Name + "</option> ");
                    }
                }
                else
                {
                    stringBuilder.Append(" <option value='" + dic.DataBaseId + "' >" + dic.Name + "</option> ");
                }
            }
            stringBuilder.Append("</select>");
            return new MvcHtmlString(stringBuilder.ToString());
        }
        #endregion

        #region Smtp/Pop3地址

        //            腾讯QQ邮箱
        //接收服务器：pop.qq.com
        //发送服务器：smtp.qq.com 

        //网易126邮箱
        //接收服务器：pop3.126.com
        //发送服务器：smtp.126.com 

        //网易163免费邮
        //接收服务器：pop.163.com
        //发送服务器：smtp.163.com

        //网易163VIP邮箱
        //接收服务器：pop.vip.163.com
        //发送服务器：smtp.vip.163.com

        //网易188财富邮
        //接收服务器：pop.188.com
        //发送服务器：smtp.188.com

        //网易yeah.net邮箱
        //接收服务器：pop.yeah.net
        //发送服务器：smtp.yeah.net

        //网易netease.com邮箱
        //接收服务器：pop.netease.com
        //发送服务器：smtp.netease.com

        //新浪收费邮箱
        //接收服务器：pop3.vip.sina.com
        //发送服务器：smtp.vip.sina.com

        //新浪免费邮箱
        //接收服务器：pop3.sina.com.cn
        //发送服务器：smtp.sina.com.cn

        //搜狐邮箱
        //接收服务器：pop3.sohu.com
        //发送服务器：smtp.sohu.com

        //21cn快感邮
        //接收服务器：vip.21cn.com
        //发送服务器：vip.21cn.com

        //21cn经济邮
        //接收服务器：pop.21cn.com
        //发送服务器：smtp.21cn.com

        //tom邮箱
        //接收服务器：pop.tom.com
        //发送服务器：smtp.tom.com

        //263邮箱
        //接收服务器：263.net
        //发送服务器：smtp.263.net

        //网易163.com邮箱
        //接收服务器：rwypop.china.com
        //发送服务器：rwypop.china.com

        //雅虎邮箱
        //接收服务器：pop.mail.yahoo.com
        //发送服务器：smtp.mail.yahoo.com

        //Gmail邮箱
        //接收服务器：pop.gmail.com
        //发送服务器：smtp.gmail.com
        /// <summary>
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListEmailServer(this HtmlHelper htmlHelper,
          DropDownListDictionaryInput input)
        {
            var list = new List<SelectListItem>();
            if (input.NeedDefault)
            {
                list.Add(new SelectListItem
                {
                    Value = "",
                    Text = @"===请选择==="
                });
            }
            list.Add(new SelectListItem
            {
                Value = "smtp.qq.com",
                Text = @"腾讯QQ邮箱(smtp.qq.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.126.com",
                Text = @"网易126邮箱(smtp.126.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.163.com",
                Text = @"网易163免费邮(smtp.163.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.vip.163.com",
                Text = @"网易163VIP邮箱(smtp.vip.163.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.vip.163.com",
                Text = @"网易188财富邮(smtp.188.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.yeah.net",
                Text = @"网易yeah.net邮箱(smtp.yeah.net)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.netease.com",
                Text = @"网易netease.com邮箱(smtp.netease.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.vip.sina.com",
                Text = @"新浪收费邮箱(smtp.vip.sina.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.sina.com.cn",
                Text = @"新浪免费邮箱(smtp.vip.sina.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.sohu.com",
                Text = @"搜狐邮箱(smtp.sohu.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "vip.21cn.com",
                Text = @"21cn快感邮(vip.21cn.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.21cn.com",
                Text = @"21cn经济邮(smtp.21cn.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.tom.com",
                Text = @"tom邮箱(smtp.tom.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.263.net",
                Text = @"263邮箱(smtp.263.net)"
            });
            list.Add(new SelectListItem
            {
                Value = "rwypop.china.com",
                Text = @"网易163.com邮箱(rwypop.china.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.mail.yahoo.com",
                Text = @"雅虎邮箱(smtp.mail.yahoo.com)"
            });
            list.Add(new SelectListItem
            {
                Value = "smtp.gmail.com",
                Text = @"Gmail邮箱(smtp.gmail.com)"
            });
            if (input.SelectedVal != null)
            {
                var item = list.Find(o => o.Value == input.SelectedVal.ToString());
                if (item != null)
                    item.Selected = true;
            }
            return htmlHelper.DropDownList(input.Name, list, input.HtmlAttributes);
        }
        #endregion
    }
}