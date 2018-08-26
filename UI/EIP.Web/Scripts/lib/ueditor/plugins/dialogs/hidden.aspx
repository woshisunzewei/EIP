<%@ Page Language="C#" %>
<%@ Import Namespace="EIP.Web.Helpers" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script type="text/javascript" src="../../dialogs/internal.js"></script>
        <script src="/Scripts/app/common.js"></script>
     <link href="../style.css" rel="stylesheet" />
    <script src="../../../jquery-1.7.2.js"></script>
    <script src="../../../../app/function.js"></script>
</head>
<body>

<br />
<table cellpadding="0" cellspacing="1" border="0" width="95%" class="formtable">
    <tr>
        <th style="width:80px;">绑定字段：</th>
        <td><select class="myselect" id="bindfiled" style="width:227px"></select></td>
    </tr>
    <tr>
        <th>默认值：</th>
        <td><input type="text" class="mytext" id="defaultvalue" style="width:250px; margin-right:6px;"/><select class="myselect" onchange="setDefaultValue(document.getElementById('defaultvalue'), this.value);" style="width:100px">
            <%=FormControl.GetDefaultValueSelectByAspx("") %>
        </select></td>
    </tr>
</table>
<script type="text/javascript">
    var oNode = null, thePlugins = 'formhidden';
    var attJSON = parent.formattributeJSON;
    $(function ()
    {
        if (UE.plugins[thePlugins].editdom)
        {
            oNode = UE.plugins[thePlugins].editdom;
        }
        biddingFileds(attJSON, oNode ? $(oNode).attr("id") : "", $("#bindfiled"));
        if (oNode)
        {
            $text = $(oNode);
            $("#defaultvalue").val(UtilDecodeUri($text.attr('defaultvalue')));
        }
    });
    dialog.oncancel = function ()
    {
        if (UE.plugins[thePlugins].editdom)
        {
            delete UE.plugins[thePlugins].editdom;
        }
    };
    dialog.onok = function ()
    {
        var bindfiled = $("#bindfiled").val();
        var id = attJSON.dbconn && attJSON.dbtable && bindfiled ? attJSON.dbtable + '.' + bindfiled : "";
        var defaultvalue = $("#defaultvalue").val();

        var html = '<input type="text" style="width:80px;" readonly="readonly" type1="flow_hidden" id="' + id + '" name="' + id + '" value="隐藏域" ';
        html += 'defaultvalue="' + encodeURI(defaultvalue) + '" ';
        html += '/>';
        if (oNode)
        {
            $(oNode).after(html);
            domUtils.remove(oNode, false);
        }
        else
        {
            editor.execCommand('insertHtml', html);
        }
        delete UE.plugins[thePlugins].editdom;
    }
</script>
</body>
</html>