<%@ Page Language="C#" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../style.css" rel="stylesheet" />
    <script type="text/javascript" src="../../dialogs/internal.js"></script>
    <script src="/Scripts/app/common.js"></script>
    <script src="/Scripts/lib/jquery-1.7.2.js"></script>
    <script src="/Scripts/app/function.js"></script>
</head>
<body>
    <br />
    <table cellpadding="0" cellspacing="1" border="0" width="95%" class="formtable">
        <tr>
            <th style="width: 80px;">绑定字段：</th>
            <td>
                <select class="myselect" id="bindfiled" style="width: 227px"></select></td>
        </tr>
        <tr>
            <th>宽度：</th>
            <td>
                <input type="text" id="width" class="mytext" style="width: 150px" />
                <span style="margin-left: 6px;">选择框标题：<input type="text" class="mytext" id="dialogtitle" style="width: 150px;" /></span>
            </td>
        </tr>
        <tr>
            <th>选择控制：</th>
            <td>
                <span style="margin-left: 0px;">
                    <input type="checkbox" id="ismore" value="1" style="vertical-align: middle;" />
                    <label for="ismore" style="vertical-align: middle;">是否允许多选</label>
                </span>
                <span style="margin-left: 10px;">
                    <input type="checkbox" id="isroot" value="1" style="vertical-align: middle;" />
                    <label for="isroot" style="vertical-align: middle;">是否允许选择根节点</label>
                </span>
                <span style="margin-left: 10px;">
                    <input type="checkbox" id="isparent" value="1" style="vertical-align: middle;" />
                    <label for="isparent" style="vertical-align: middle;">是否允许选择父节点</label>
                </span>
            </td>
        </tr>
        <!--
    <tr>
        <th>列表类型：</th>
        <td>
            <input type="radio" name="listtype" value="0" id="listtype_0" checked="checked" style="vertical-align:middle;" /><label style="vertical-align:middle;" for="listtype_0">树型</label>
            <input type="radio" name="listtype" value="1" id="listtype_1" style="vertical-align:middle;" /><label style="vertical-align:middle;" for="listtype_1">列表</label>
        </td>
    </tr>
    -->
        <tr>
            <th>数据来源：</th>
            <td>
                <input type="radio" onclick="dsChange(this.value);" checked="checked" name="datasource" value="0" id="datasource_0" style="vertical-align: middle;" /><label style="vertical-align: middle;" for="datasource_0">数据字典</label>
                <input type="radio" onclick="dsChange(this.value);" name="datasource" value="1" id="datasource_1" style="vertical-align: middle;" /><label style="vertical-align: middle;" for="datasource_1">SQL</label>
                <input type="radio" onclick="dsChange(this.value);" name="datasource" value="2" id="datasource_2" style="vertical-align: middle;" /><label style="vertical-align: middle;" for="datasource_2">URL</label>
                <input type="radio" onclick="dsChange(this.value);" name="datasource" value="3" id="datasource_3" style="vertical-align: middle;" /><label style="vertical-align: middle;" for="datasource_3">数据表</label>
            </td>
        </tr>
        <tr id="ds_datasource_0">
            <td colspan="2">
                <div style="margin: 5px 0;">
                    <input type="checkbox" id="ds_dict_allchild" value="1" style="vertical-align: middle;" />
                    <label for="ds_dict_allchild" style="vertical-align: middle;">是否加载所有下级节点</label>
                </div>
                <div style="padding: 5px 0;">字典根：<input type="text" id="ds_dict_value" class="mydict" more="0" style="width: 260px;" /></div>
                <!--
            <div style="padding:5px 0;">
                值字段：
                <input type="radio" name="ds_dict_valuefield" id="ds_dict_valuefield_id" value="id" style="vertical-align:middle;" />
                <label for="ds_dict_valuefield_id" style="vertical-align:middle;">ID</label>

                <input type="radio" name="ds_dict_valuefield" id="ds_dict_valuefield_code" value="code" style="vertical-align:middle;" />
                <label for="ds_dict_valuefield_code" style="vertical-align:middle;">唯一代码</label>

                <input type="radio" name="ds_dict_valuefield" id="ds_dict_valuefield_title" value="title" style="vertical-align:middle;" />
                <label for="ds_dict_valuefield_title" style="vertical-align:middle;">标题</label>

                <input type="radio" name="ds_dict_valuefield" id="ds_dict_valuefield_value" value="value" style="vertical-align:middle;" />
                <label for="ds_dict_valuefield_value" style="vertical-align:middle;">值</label>

                <input type="radio" name="ds_dict_valuefield" id="ds_dict_valuefield_note" value="note" style="vertical-align:middle;" />
                <label for="ds_dict_valuefield_note" style="vertical-align:middle;">备注</label>

                <input type="radio" name="ds_dict_valuefield" id="ds_dict_valuefield_other" value="other" style="vertical-align:middle;" />
                <label for="ds_dict_valuefield_other" style="vertical-align:middle;">其它</label>
            </div>
            -->
            </td>
        </tr>
        <tr id="ds_datasource_1" style="display: none;">
            <td colspan="2">
                <div style="margin: 5px 0;">
                    SQL语句应该返回两列，第一列为值，第二列为标题。
                </div>
                <div style="margin: 5px 0;">
                    <div>
                        数据连接：<select class="myselect" id="ds_sql_dbconn">
                            <%--<%=dbconnOptionString %>--%>
                        </select>
                        <input type="button" value="测试SQL" onclick="testSql($('#ds_sql_value').val(), $('#ds_sql_dbconn').val());" class="mybutton" />
                    </div>
                </div>
                <textarea class="mytextarea" id="ds_sql_value" style="height: 60px; width: 99%;" cols="50" rows="5"></textarea>
            </td>
        </tr>
        <tr id="ds_datasource_2" style="display: none;">
            <td colspan="2">
                <div>
                    URL返回树型要求的json格式字符串
                </div>
                <div>
                    初始化URL：
                </div>
                <textarea class="mytextarea" id="ds_url_value0" style="height: 35px; width: 99%;" cols="30" rows="5"></textarea>
                <div>
                    二次加载URL：
                </div>
                <textarea class="mytextarea" id="ds_url_value1" style="height: 35px; width: 99%;" cols="30" rows="5"></textarea>
                <div>
                    根据值得到标题URL：
                </div>
                <textarea class="mytextarea" id="ds_url_gettitle" style="height: 35px; width: 99%;" cols="30" rows="5"></textarea>
            </td>
        </tr>
        <tr id="ds_datasource_3" style="display: none;">
            <td colspan="2">
                <div style="margin: 5px 0;">
                    <table cellpadding="0" cellspacing="1" border="0" width="98%" class="formtable">
                        <tr>
                            <th style="width: 100px;">数据：</th>
                            <td>连接：<select class="myselect" id="ds_table_conn" onchange="db_change()">
                                <%--<%=dbconnOptionString %>--%>
                            </select>
                                表：<select class="myselect" id="ds_table_table" onchange="table_change()" style="width: 227px"></select>
                            </td>
                        </tr>
                        <tr>
                            <th>值字段：</th>
                            <td>
                                <select class="myselect" id="ds_table_valuefield" style="width: 227px"></select></td>
                        </tr>
                        <tr>
                            <th>标题字段：</th>
                            <td>
                                <select class="myselect" id="ds_table_titlefield" style="width: 227px"></select></td>
                        </tr>
                        <tr>
                            <th>父子关系字段：</th>
                            <td>
                                <select class="myselect" id="ds_table_parentfield" style="width: 227px"></select></td>
                        </tr>
                        <tr>
                            <th>过滤条件：</th>
                            <td>
                                <input type="text" class="mytext" id="ds_table_where" style="width: 95%;" /></td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        var oNode = null, thePlugins = 'formdictionary';
        var attJSON = parent.formattributeJSON;
        $(function () {
            if (UE.plugins[thePlugins].editdom) {
                oNode = UE.plugins[thePlugins].editdom;
            }
            biddingFileds(attJSON, oNode ? $(oNode).attr("id") : "", $("#bindfiled"));
            db_change();
            if (oNode) {
                $text = $(oNode);
                $("#defaultvalue").val(UtilDecodeUri($text.attr('defaultvalue')));
                if ($text.attr('width1')) $("#width").val($text.attr('width1'));
                $("#dialogtitle").val($text.attr("dialogtitle") || "");
                $("#rang").val($text.attr('rootid'));
                new RoadUI.Dict().setValue($("#rang"));
                $("#ismore").prop('checked', "1" == $text.attr('ismore'));
                $("#isroot").prop('checked', "1" == $text.attr('isroot'));
                $("#isparent").prop('checked', "1" == $text.attr('isparent'));

                $("input[type='radio'][name='listtype'][value='" + ($text.attr('listtype') || "") + "']").prop("checked", true);
                $("input[type='radio'][name='datasource'][value='" + ($text.attr('datasource') || "") + "']").prop("checked", true);

                $("#ds_dict_allchild").prop('checked', "1" == $text.attr('ds_dict_allchild'));
                $("#ds_dict_value").val($text.attr("ds_dict_value") || "");

                $("#ds_sql_dbconn").val($text.attr("ds_sql_dbconn") || "");
                $("#ds_sql_value").val(decodeURIComponent($text.attr("ds_sql_value") || ""));

                $("#ds_url_value0").val($text.attr("ds_url_value0") || "");
                $("#ds_url_value1").val($text.attr("ds_url_value1") || "");
                $("#ds_url_gettitle").val($text.attr("ds_url_gettitle") || "");

                $("#ds_table_conn").val($text.attr("ds_table_conn") || "");
                $("#ds_table_table").val($text.attr("ds_table_table") || "").change();
                $("#ds_table_valuefield").val($text.attr("ds_table_valuefield") || "");
                $("#ds_table_titlefield").val($text.attr("ds_table_titlefield") || "");
                $("#ds_table_parentfield").val($text.attr("ds_table_parentfield") || "");
                $("#ds_table_where").val($text.attr("ds_table_where") || "");

                dsChange($text.attr('datasource') || "");
            }

        });
        function dsChange(value) {
            $("[id^='ds_datasource']").hide();
            $("#ds_datasource_" + value).show();
        }
        function db_change() {
            $("#ds_table_table").html(getTableOps($("#ds_table_conn").val(), $("#ds_table_table").val()));
            table_change();
        }
        function table_change() {

            var conn = $("#ds_table_conn").val();
            var table = $("#ds_table_table").val();
            if (!conn || !table) return;
            var opts0 = getFieldsOps(conn, table, $("#ds_table_valuefield").val());
            $("#ds_table_valuefield").html(opts0);
            var opts1 = getFieldsOps(conn, table, $("#ds_table_titlefield").val());
            $("#ds_table_titlefield").html(opts1);
            var opts2 = getFieldsOps(conn, table, $("#ds_table_parentfield").val());
            $("#ds_table_parentfield").html(opts2);
        }
        dialog.oncancel = function () {
            if (UE.plugins[thePlugins].editdom) {
                delete UE.plugins[thePlugins].editdom;
            }
        };
        dialog.onok = function () {
            var bindfiled = $("#bindfiled").val();
            var id = attJSON.dbconn && attJSON.dbtable && bindfiled ? attJSON.dbtable + '.' + bindfiled : "";
            var width = $("#width").val();
            var dialogtitle = $("#dialogtitle").val() || "";
            var ismore = $("#ismore").prop('checked') ? "1" : "0";
            var isroot = $("#isroot").prop('checked') ? "1" : "0";
            var isparent = $("#isparent").prop('checked') ? "1" : "0";
            var listtype = $(":checked[type='radio'][name='listtype']").val();
            var datasource = $(":checked[type='radio'][name='datasource']").val();
            var ds_dict_allchild = $("#ds_dict_allchild").prop('checked') ? "1" : "0";
            var ds_dict_value = $("#ds_dict_value").val() || "";
            var ds_sql_dbconn = $("#ds_sql_dbconn").val() || "";
            var ds_sql_value = $("#ds_sql_value").val() || "";
            var ds_url_value0 = $("#ds_url_value0").val() || "";
            var ds_url_value1 = $("#ds_url_value1").val() || "";
            var ds_url_gettitle = $("#ds_url_gettitle").val() || "";
            var ds_table_conn = $("#ds_table_conn").val() || "";
            var ds_table_table = $("#ds_table_table").val() || "";
            var ds_table_valuefield = $("#ds_table_valuefield").val() || "";
            var ds_table_titlefield = $("#ds_table_titlefield").val() || "";
            var ds_table_parentfield = $("#ds_table_parentfield").val() || "";
            var ds_table_where = $("#ds_table_where").val() || "";

            var html = '<input type="text" type1="flow_dict" id="' + id + '" name="' + id + '" value="左右选择框" ';
            if (width) {
                html += 'style="width:' + width + '" ';
                html += 'width1="' + width + '" ';
            }
            html += 'dialogtitle="' + dialogtitle + '" ';
            html += 'ismore="' + ismore + '" ';
            html += 'isroot="' + isroot + '" ';
            html += 'isparent="' + isparent + '" ';
            html += 'listtype="' + listtype + '" ';
            html += 'datasource="' + datasource + '" ';
            html += 'ds_dict_allchild="' + ds_dict_allchild + '" ';
            html += 'ds_dict_value="' + ds_dict_value + '" ';
            html += 'ds_sql_dbconn="' + ds_sql_dbconn + '" ';
            html += 'ds_sql_value="' + encodeURIComponent(ds_sql_value) + '" ';
            html += 'ds_url_value0="' + ds_url_value0 + '" ';
            html += 'ds_url_value1="' + ds_url_value1 + '" ';
            html += 'ds_url_gettitle="' + ds_url_gettitle + '" ';
            html += 'ds_table_conn="' + ds_table_conn + '" ';
            html += 'ds_table_table="' + ds_table_table + '" ';
            html += 'ds_table_valuefield="' + ds_table_valuefield + '" ';
            html += 'ds_table_titlefield="' + ds_table_titlefield + '" ';
            html += 'ds_table_parentfield="' + ds_table_parentfield + '" ';
            html += 'ds_table_where="' + ds_table_where + '" ';
            html += '/>';
            if (oNode) {
                $(oNode).after(html);
                domUtils.remove(oNode, false);
            }
            else {
                editor.execCommand('insertHtml', html);
            }
            delete UE.plugins[thePlugins].editdom;
        }
    </script>
</body>
</html>
