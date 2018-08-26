<%@ Page Language="C#" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script type="text/javascript" src="../../dialogs/internal.js"></script>
        <script src="/Scripts/app/common.js"></script>
    
</head>
<body>


<div class="wrapper">
    <div id="tabhead" class="tabhead">
        <span class="tab focus" data-content-id="text_attr">&nbsp;&nbsp;属性&nbsp;&nbsp;</span>
        <span class="tab" data-content-id="text_expression">&nbsp;&nbsp;表达式&nbsp;&nbsp;</span>
        <span class="tab" data-content-id="text_event">&nbsp;&nbsp;事件&nbsp;&nbsp;</span>
    </div>
    <div id="tabbody" class="tabbody">
        <div id="text_attr" class="panel focus">
            <table cellpadding="0" cellspacing="1" border="0" width="100%" class="formtable">
                <tr>
                    <th style="width:80px;">绑定字段:</th>
                    <td><select class="myselect" id="bindfiled" style="width:255px"></select></td>
                </tr>
                <tr>
                    <th>默认值:</th>
                    <td><input type="text" class="mytext" id="defaultvalue" style="width:290px; margin-right:6px;"/><select class="myselect" onchange="setDefaultValue(document.getElementById('defaultvalue'), this.value);" style="width:150px">
                        <%--<%=workFlowFrom.GetDefaultValueSelectByAspx("") %>--%>
                    </select></td>
                </tr>
                <tr>
                    <th>宽度:</th>
                    <td><input type="text" id="width" class="mytext" style="width:150px" /></td>
                </tr>
                <tr>
                    <th>小数位数:</th>
                    <td><input type="text" id="tofixed"  class="mytext" style="width:150px" /></td>
                </tr>
                <tr>
                    <th>只读:</th>
                    <td><select class="myselect" id="isreadonly" ><option value="1">是</option><option value="0">否</option></select></td>
                </tr>
            </table>    
        </div>

        <div id="text_expression" class="panel">
            <table cellpadding="0" cellspacing="1" border="0" width="98%" class="listtable" id="expressiontable">
                <thead>
                    <tr>
                        <th style="width:10%;">左括号</th>
                        <th style="width:50%;">字段</th>
                        <th style="width:10%;">右括号</th>
                        <th style="width:10%;">计算</th>                        
                        <th style="width:20%;"></th>
                    </tr>
                </thead>
                <tbody>
                    
                </tbody>
            </table>
        </div>

        <div id="text_event" class="panel">
          <%Server.Execute("events.aspx"); %>
        </div>
    </div>
</div>

<script type="text/javascript">
    var oNode = null, thePlugins = 'formsumtext';
    var attJSON = parent.formattributeJSON;

    var parentEvents = parent.formEvents;
    var events = [];
    var eventsid = UtilNewGuid;
    var fieldsArray = getFields(attJSON.dbconn, attJSON.dbtable);
    var elements = $();
    $(function ()
    {
        if (UE.plugins[thePlugins].editdom)
        {
            oNode = UE.plugins[thePlugins].editdom;
        }
        elements = $("[type1]", dialog.editor.document);
        biddingFileds(attJSON, oNode ? $(oNode).attr("id") : "", $("#bindfiled"));
        if (oNode)
        {
            $text = $(oNode);
            $("#defaultvalue").val(UtilDecodeUri($text.attr('defaultvalue')));
            if ($text.attr('width1')) $("#width").val($text.attr('width1'));
            $("#tofixed").val($text.attr('tofixed'));
            $("#isreadonly").val($text.attr('isreadonly') || '1');

            if ($text.attr('eventsid'))
            {
                eventsid = $text.attr('eventsid');
                events = getEvents(eventsid);
            }

            var sumstr = $text.attr("sumstr") && $text.attr("sumstr").length > 0 ? JSON.parse($text.attr("sumstr")) : [];
            if (sumstr.length > 0)
            {
                for (var i = 0; i < sumstr.length; i++)
                {
                    addSumRow(sumstr[i].field, sumstr[i].sum, sumstr[i].leftkh, sumstr[i].rightkh);
                }
            }
            else
            {
                addSumRow("", "");
            }
        }
        else
        {
            addSumRow("", "");
        }

        initTabs();
    });

    function addSumRow(field, sum, leftkh, rightkh)
    {
        var $listtable = $("#expressiontable tbody");
        var fielsOptions = '<option value=""></option>';
        var sumArray = ['+', '-', '*', '/'];
        if (elements.size() > 0)
        {
            for (var i = 0; i < elements.size() ; i++)
            {
                if (!elements.eq(i) || !elements.eq(i).attr("id")) continue;
                var valuetype = elements.eq(i).attr("valuetype") || "";
                if ("1" != valuetype && "2" != valuetype && "3" != valuetype && "4" != valuetype && "5" != valuetype && "6" != valuetype)
                {
                    continue;
                }
                var note1 = '';
                for (var j = 0; j < fieldsArray.length; j++)
                {
                    if (attJSON.dbtable + '.' + fieldsArray[j].name == elements.eq(i).attr("id") && fieldsArray[j].note && fieldsArray[j].note.length > 0)
                    {
                        note1 = '(' + fieldsArray[j].note + ')';
                        break;
                    }
                }
                var txt = elements.eq(i).attr("id").indexOf('.') >= 0
                    ? elements.eq(i).attr("id").split('.')[1] + note1 + '--' + '' + elements.eq(i).attr("id").split('.')[0] + ''
                    : elements.eq(i).attr("id");

                fielsOptions += '<option' + (elements.eq(i).attr("id") == field ? ' selected="selected"' : '') + ' value="' + elements.eq(i).attr("id") + '">' + txt + '</option>';
            }
            var tr = '<tr style="height:30px;">';
            tr += '<td><select class="myselect"><option value=""></option><option value="("' + ("(" == leftkh ? ' selected="selected"' : '') + '>(</option></select></td>';
            tr += '<td><select class="myselect">' + fielsOptions + '</select></td>';
            tr += '<td><select class="myselect"><option value=""></option><option value=")"' + (")" == rightkh ? ' selected="selected"' : '') + '>)</option></select></td>';
            tr += '<td><select class="myselect"><option value=""></option>';
            for (var i = 0; i < sumArray.length; i++)
            {
                tr += '<option' + (sumArray[i] == sum ? ' selected="selected"' : '') + ' value="' + sumArray[i] + '">' + sumArray[i] + '</option>';
            }
            tr += '</select></td>';

            tr += '<td><input type="button" class="mybutton" onclick="addSumRow(\'\',\'\')" value="添加" style="margin-right:4px;"/><input type="button" onclick="if($(\'#listtable tbody tr\').size()>1){$(this).parent().parent().remove();}" class="mybutton" value="删除"/></td>';
            tr += '</tr>';
            var $tr = $(tr);

            $listtable.append($tr);
            new RoadUI.Select().init($(".myselect"), $tr);
            new RoadUI.Button().init($(".mybutton"), $tr);
        }
    }

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
        var width = $("#width").val();
        var defaultvalue = $("#defaultvalue").val();
        var tofixed = $("#tofixed").val();
        var isreadonly = $("#isreadonly").val();
        var $listtabletr = $("#expressiontable tbody tr");
        var sumstr = [];

        for (var i = 0; i < $listtabletr.size() ; i++)
        {
            var $tds = $("td", $listtabletr.eq(i));
            var $leftKh = $("select", $tds.eq(0));
            var $field = $("select", $tds.eq(1));
            var $rightKh = $("select", $tds.eq(2));
            var $sum = $("select", $tds.eq(3));

            if ($field.size() == 0 || $field.val().length == 0)
            {
                continue;
            }
            sumstr.push('{"leftkh":"' + $leftKh.val() + '", "field": "' + $field.val() + '", "rightkh":"' + $rightKh.val() + '", "sum": "' + $sum.val() + '"}');
        }
        var html = '<input type="text" id="' + id + '" type1="flow_sumtext" name="' + id + '" value="计算字段" ';
        if (width)
        {
            html += 'style="width:' + width + '" ';
            html += 'width1="' + width + '" ';
        }
        html += 'defaultvalue="' + encodeURI(defaultvalue) + '" valuetype="2" ';
        if (tofixed)
        {
            html += 'tofixed="' + tofixed + '" ';
        }
        if (events.length > 0)
        {
            html += 'eventsid="' + eventsid + '" ';
            setEvents(eventsid);
        }
        if (sumstr.length > 0)
        {
            html += 'sumstr=\'[' + sumstr.join(',') + ']\' ';
        }
        html += 'isreadonly="' + isreadonly + '" ';
        if ("1" == isreadonly)
        {
            html += 'readonly="readonly" ';
        }
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