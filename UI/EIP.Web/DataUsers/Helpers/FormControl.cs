using System.Text;
using System.Web.UI.WebControls;
using EIP.Common.Core.Utils;

namespace EIP.Web.Helpers
{
    public class FormControl
    {
        /// <summary>
        ///     得到验证提示方式Radio字符串
        /// </summary>
        /// <returns></returns>
        public static string GetValidatePropTypeRadios(string name, string value, string att = "")
        {
            ListItem[] items =
            {
                new ListItem("弹出(alert)", "0") {Selected = "0" == value},
                new ListItem("图标和提示信息", "1") {Selected = "1" == value},
                new ListItem("图标", "2") {Selected = "2" == value}
            };
            return ControlUtil.GetRadioString(items, name, att);
        }

        /// <summary>
        ///     得到流程文本框输入类型Radio字符串
        /// </summary>
        /// <returns></returns>
        public  static string GetInputTypeRadios(string name, string value, string att = "")
        {
            ListItem[] items =
            {
                new ListItem("明文", "text") {Selected = "0" == value},
                new ListItem("密码", "password") {Selected = "1" == value}
            };
            return ControlUtil.GetRadioString(items, name, att);
        }

        /// <summary>
        ///     得到待选事件选择项
        /// </summary>
        /// <returns></returns>
        public static string GetEventOptions(string name, string value, string att = "")
        {
            ListItem[] items =
            {
                new ListItem("onclick", "onclick") {Selected = "onclick" == value},
                new ListItem("onchange", "onchange") {Selected = "onchange" == value},
                new ListItem("ondblclick", "ondblclick") {Selected = "ondblclick" == value},
                new ListItem("onfocus", "onfocus") {Selected = "onfocus" == value},
                new ListItem("onblur", "onblur") {Selected = "onblur" == value},
                new ListItem("onkeydown", "onkeydown") {Selected = "onkeydown" == value},
                new ListItem("onkeypress", "onkeypress") {Selected = "onkeypress" == value},
                new ListItem("onkeyup", "onkeyup") {Selected = "onkeyup" == value},
                new ListItem("onmousedown", "onmousedown") {Selected = "onmousedown" == value},
                new ListItem("onmouseup", "onmouseup") {Selected = "onmouseup" == value},
                new ListItem("onmouseover", "onmouseover") {Selected = "onmouseover" == value},
                new ListItem("onmouseout", "onmouseout") {Selected = "onmouseout" == value}
            };
            return ControlUtil.GetOptionsString(items);
        }

        /// <summary>
        ///     得到流程值类型选择项字符串
        /// </summary>
        /// <returns></returns>
        public static string GetValueTypeOptions(string value)
        {
            ListItem[] items =
            {
                new ListItem("字符串", "0") {Selected = "0" == value},
                new ListItem("整数", "1") {Selected = "1" == value},
                new ListItem("实数", "2") {Selected = "2" == value},
                new ListItem("正整数", "3") {Selected = "3" == value},
                new ListItem("正实数", "4") {Selected = "4" == value},
                new ListItem("负整数", "5") {Selected = "5" == value},
                new ListItem("负实数", "6") {Selected = "6" == value},
                new ListItem("手机号码", "7") {Selected = "7" == value}
            };
            return ControlUtil.GetOptionsString(items);
        }

        /// <summary>
        ///     得到默认值下拉选项字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDefaultValueSelect(string value)
        {
            var options = new StringBuilder(1000);
            options.Append("<option value=\"\" selected=\"selected\">===请选择===</option>");
            options.Append("<optgroup label=\"组织机构相关选项\"></optgroup>");
            options.AppendFormat(
                "<option value=\"u_@RoadFlow.Platform.Users.CurrentUserID.ToString()\" {0}>当前步骤用户ID</option>",
                "10" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(RoadFlow.Platform.Users.CurrentUserName)\" {0}>当前步骤用户姓名</option>",
                "11" == value ? "selected=\"selected\"" : "");
            options.AppendFormat("<option value=\"@(RoadFlow.Platform.Users.CurrentDeptID)\" {0}>当前步骤用户部门ID</option>",
                "12" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@(RoadFlow.Platform.Users.CurrentDeptName)\" {0}>当前步骤用户部门名称</option>",
                "13" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"u_@(new RoadFlow.Platform.WorkFlowTask().GetFirstSnderID(FlowID.ToGuid(), GroupID.ToGuid(), true))\" {0}>流程发起者ID</option>",
                "14" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@(new RoadFlow.Platform.Users().GetName(new RoadFlow.Platform.WorkFlowTask().GetFirstSnderID(FlowID.ToGuid(), GroupID.ToGuid(), true)))\" {0}>流程发起者姓名</option>",
                "15" == value ? "selected=\"selected\"" : "");
            options.Append("<optgroup label=\"日期时间相关选项\"></optgroup>");
            options.AppendFormat(
                "<option value=\"@(RoadFlow.Utility.DateTimeNew.ShortDate)\" {0}>短日期格式(2014/4/15)</option>",
                "20" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@(RoadFlow.Utility.DateTimeNew.LongDate)\" {0}>长日期格式(2014年4月15日)</option>",
                "21" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@(RoadFlow.Utility.DateTimeNew.ShortTime)\" {0}>短时间格式(23:59)</option>",
                "22" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@(RoadFlow.Utility.DateTimeNew.LongTime)\" {0}>长时间格式(23时59分)</option>",
                "23" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@(RoadFlow.Utility.DateTimeNew.ShortDateTime)\" {0}>短日期时间格式(2014/4/15 22:31)</option>",
                "24" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@(RoadFlow.Utility.DateTimeNew.LongDateTime)\" {0}>长日期时间格式(2014年4月15日 22时31分)</option>",
                "25" == value ? "selected=\"selected\"" : "");
            options.Append("<optgroup label=\"流程实例相关选项\"></optgroup>");
            options.AppendFormat(
                "<option value=\"@Html.Raw(BWorkFlow.GetFlowName(FlowID.ToGuid()))\" {0}>当前流程名称</option>",
                "30" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@Html.Raw(BWorkFlow.GetStepName(StepID.ToGuid(), FlowID.ToGuid(), true))\" {0}>当前步骤名称</option>",
                "31" == value ? "selected=\"selected\"" : "");
            return options.ToString();
        }

        /// <summary>
        ///     得到默认值下拉选项字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDefaultValueSelectByAspx(string value)
        {
            var options = new StringBuilder(1000);
            options.Append("<option value=\"\"></option>");
            options.Append("<optgroup label=\"组织机构相关选项\"></optgroup>");
            options.AppendFormat(
                "<option value=\"@principalUser.UserId\" {0}>当前步骤用户ID</option>",
                "10" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@principalUser.Name\" {0}>当前步骤用户姓名</option>",
                "11" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@principalUser.OrganizationId\" {0}>当前步骤用户部门ID</option>",
                "12" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@principalUser.OrganizationName\" {0}>当前步骤用户部门名称</option>",
                "13" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@principalUser.UserId\" {0}>流程发起者ID</option>",
                "14" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@principalUser.Name\" {0}>流程发起者姓名</option>",
                "15" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
               "<option value=\"@principalUser.OrganizationId\" {0}>流程发起者部门ID</option>",
               "16" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@principalUser.OrganizationName\" {0}>流程发起者部门名称</option>",
                "17" == value ? "selected=\"selected\"" : "");
            options.Append("<optgroup label=\"日期时间相关选项\"></optgroup>");
            options.AppendFormat(
                "<option value=\"@DateTimeNewUtil.ShortDate\" {0}>短日期格式(2014/4/15)</option>",
                "20" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@DateTimeNewUtil.LongDate\" {0}>长日期格式(2014年4月15日)</option>",
                "21" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@DateTimeNewUtil.ShortTime\" {0}>短时间格式(23:59)</option>",
                "22" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@DateTimeNewUtil.LongTime\" {0}>长时间格式(23时59分)</option>",
                "23" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@DateTimeNewUtil.ShortDateTime\" {0}>短日期时间格式(2014/4/15 22:31)</option>",
                "24" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"@DateTimeNewUtil.LongDateTime\" {0}>长日期时间格式(2014年4月15日 22时31分)</option>",
                "25" == value ? "selected=\"selected\"" : "");
            options.Append("<optgroup label=\"流程实例相关选项\"></optgroup>");
            options.AppendFormat("<option value=\"<%=BWorkFlow.GetFlowName(FlowID.ToGuid())%>\" {0}>当前流程名称</option>",
                "30" == value ? "selected=\"selected\"" : "");
            options.AppendFormat(
                "<option value=\"<%=BWorkFlow.GetStepName(StepID.ToGuid(), FlowID.ToGuid(), true)%>\" {0}>当前步骤名称</option>",
                "31" == value ? "selected=\"selected\"" : "");
            return options.ToString();
        }

        /// <summary>
        ///     得到流程文本域模式Radio字符串
        /// </summary>
        /// <returns></returns>
        public static string GetTextareaModelRadios(string name, string value, string att = "")
        {
            ListItem[] items =
            {
                new ListItem("普通", "default") {Selected = "default" == value},
                new ListItem("HTML", "html") {Selected = "html" == value}
            };
            return ControlUtil.GetRadioString(items, name, att);
        }

        /// <summary>
        ///     得到数据源Radio字符串
        /// </summary>
        /// <returns></returns>
        public static string GetDataSourceRadios(string name, string value, string att = "")
        {
            ListItem[] items =
            {
                new ListItem("数据字典", "0") {Selected = "0" == value},
                new ListItem("自定义", "1") {Selected = "1" == value},
                new ListItem("SQL语句", "2") {Selected = "2" == value}
            };

            return ControlUtil.GetRadioString(items, name, att);
        }

        /// <summary>
        ///     得到组织机构选择范围Radio字符串
        /// </summary>
        /// <returns></returns>
        public static string GetOrgRangeRadios(string name, string value, string att = "")
        {
            ListItem[] items =
            {
                new ListItem("发起者部门", "0") {Selected = "0" == value},
                new ListItem("处理者部门", "1") {Selected = "1" == value}
            };
            return ControlUtil.GetRadioString(items, name, att);
        }

        /// <summary>
        ///     得到组织机构选择类型Radio字符串
        /// </summary>
        /// <returns></returns>
        public static string GetOrgSelectTypeCheckboxs(string name, string value, string att = "")
        {
            ListItem[] items =
            {
                new ListItem("部门", "0") {Selected = "0" == value},
                new ListItem("岗位", "1") {Selected = "1" == value},
                new ListItem("人员", "2") {Selected = "2" == value},
                new ListItem("工作组", "3") {Selected = "3" == value},
                new ListItem("单位", "4") {Selected = "4" == value}
            };
            return ControlUtil.GetCheckBoxString(items, name, value.Split(','), att);
        }

        /// <summary>
        ///     得到从表编辑模式选择
        /// </summary>
        /// <returns></returns>
        public static string GetEditmodeOptions(string value)
        {
            ListItem[] items =
            {
                new ListItem("无", "") {Selected = "" == value},
                new ListItem("文本框", "text") {Selected = "text" == value},
                new ListItem("文本域", "textarea") {Selected = "textarea" == value},
                new ListItem("下拉列表", "select") {Selected = "select" == value},
                new ListItem("复选框", "checkbox") {Selected = "checkbox" == value},
                new ListItem("单选框", "radio") {Selected = "radio" == value},
                new ListItem("日期时间", "datetime") {Selected = "datetime" == value},
                new ListItem("组织机构选择", "org") {Selected = "org" == value},
                new ListItem("数据字典选择", "dict") {Selected = "dict" == value},
                new ListItem("附件", "files") {Selected = "files" == value}
            };
            return ControlUtil.GetOptionsString(items);
        }
    }
}