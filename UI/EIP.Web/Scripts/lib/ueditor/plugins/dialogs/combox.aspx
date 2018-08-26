<%@ Page Language="C#" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript" src="../../dialogs/internal.js"></script>
    <script src="/Scripts/app/common.js"></script>
</head>
<body>
    <div class="wrapper">
        <div id="tabhead" class="tabhead">
            <span class="tab focus" data-content-id="text_attr">&nbsp;&nbsp;属性&nbsp;&nbsp;</span>
            <span class="tab" data-content-id="text_default" onclick="loadOptions();">&nbsp;&nbsp;默认值&nbsp;&nbsp;</span>
            <span class="tab" data-content-id="text_event">&nbsp;&nbsp;事件&nbsp;&nbsp;</span>
        </div>
        <div id="tabbody" class="tabbody" style="height: 400px;">
            <div id="text_attr" class="panel focus">
                <table cellpadding="0" cellspacing="1" border="0" width="100%" class="formtable">
                    <tr>
                        <th style="width: 80px;">绑定字段：</th>
                        <td>
                            <select class="myselect" id="bindfiled" style="width: 227px"></select></td>
                    </tr>
                    <tr>
                        <th>默认值：</th>
                        <td>
                            <input type="text" class="mytext" id="defaultvalue" style="width: 250px; margin-right: 6px;" /><select class="myselect" onchange="setDefaultValue(document.getElementById('defaultvalue'), this.value);" style="width: 100px">
                                <%--<%=workFlowFrom.GetDefaultValueSelectByAspx("") %>--%>
                            </select></td>
                    </tr>
                    <tr>
                        <th>控件宽度：</th>
                        <td>
                            <input type="text" class="mytext" id="width" value="" style="width: 100px;" />&nbsp;&nbsp;列表宽度：<input type="text" class="mytext" id="width1" value="" style="width: 100px;" />&nbsp;&nbsp;列表高度：<input type="text" class="mytext" id="height1" value="" style="width: 100px;" /></td>
                    </tr>
                    <tr>
                        <th>数据源：</th>
                        <td>
                            <%--<%=workFlowFrom.GetDataSourceRadios("datasource","0","onclick='dsChange(this.value);'") %>--%>
                            <input type="radio" value="3" id="datasource_6d24b56a16e44794bcfe265b603fcc31" name="datasource" onclick='dsChange(this.value);' style="vertical-align: middle" /><label style="vertical-align: middle; margin-right: 2px;" for="datasource_6d24b56a16e44794bcfe265b603fcc31">URL</label>
                        </td>
                    </tr>
                    <tr>
                        <th>列表方式：</th>
                        <td>
                            <input type="radio" name="listmode" value="0" id="listmode_0" /><label for="listmode_0">列表项</label>
                            <input type="radio" name="listmode" value="1" id="listmode_1" /><label for="listmode_1">数据表格</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="ismultiple" id="ismultiple" /><label for="ismultiple">是否多选</label>
                        </td>
                    </tr>
                    <tr id="ds_dict">
                        <th>字典项：</th>
                        <td>
                            <input type="text" class="mydict" id="ds_dict_value" more="0" value="" /></td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="1" border="0" width="100%" id="ds_custom" style="display: none; margin-top: 5px;" align="center">
                    <tr>
                        <td colspan="2">
                            <fieldset style="padding: 5px;">
                                <legend style="">自定义选项 </legend>
                                <div style="margin: 0 auto; padding: 0 5px;">
                                    <div style="padding: 3px 0;">
                                        <div>1.格式：选项文本1,选项值1;选项文本2,选项值2</div>
                                        <div>2.如果列表方式为数据表格，则此处为表格html</div>
                                    </div>
                                    <textarea class="mytextarea" id="custom_string" style="height: 150px; width: 100%;"></textarea>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="1" border="0" width="100%" id="ds_sql" style="display: none; margin-top: 5px;" align="center">
                    <tr>
                        <td colspan="2">
                            <fieldset style="padding: 1px;">
                                <legend style="">SQL语句 </legend>
                                <table border="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 80%">
                                            <div>1.SQL应返回两个字段的数据源,第一个字段为值，第二个字段为标题</div>
                                            <div>2.如果只返回一个字段则值和标题一样</div>
                                            <div>3.如果列表方式是数据表格则第一列为值,字段数目不限制</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding-top: 4px;">
                                            <div>
                                                数据连接：<select class="myselect" id="ds_sql_dbconn">
                                                    <%--<%=new RoadFlow.Platform.DBConnection().GetAllOptions() %>--%>
                                                </select>
                                                <input type="button" value="测试SQL" onclick="testSql($('#ds_sql_value').val(), $('#ds_sql_dbconn').val());" class="mybutton" />
                                            </div>
                                            <div style="margin-top: 5px;">
                                                <textarea cols="1" rows="1" id="ds_sql_value" style="width: 99%; height: 105px; font-family: Verdana;" class="mytextarea"></textarea>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>

                <table cellpadding="0" cellspacing="1" border="0" width="100%" id="ds_url" style="display: none; margin-top: 3px;" align="center">
                    <tr>
                        <td colspan="2">
                            <fieldset style="padding: 1px;">
                                <legend style="">URL设置 </legend>
                                <table border="0" style="width: 100%;">
                                    <tr>
                                        <td colspan="2">表头：各列之间用逗号隔开，列1,列2,列3</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding-top: 2px;">
                                            <input type="text" class="mytext" id="ds_url_cols" style="width: 99%; font-family: Verdana;" />
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" style="width: 100%;">
                                    <tr>
                                        <td colspan="2">数据URL：</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding-top: 2px;">
                                            <input type="text" class="mytext" id="ds_url_value" style="width: 99%; font-family: Verdana;" />
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" style="width: 100%;">
                                    <tr>
                                        <td colspan="2">根据值得到标题URL：</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding-top: 2px;">
                                            <input type="text" class="mytext" id="ds_url_gettexts" style="width: 99%; font-family: Verdana;" />
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" style="width: 100%;">
                                    <tr>
                                        <td colspan="2">查询字段：字段1,标题1;字段2,标题2</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding-top: 2px;">
                                            <input type="text" class="mytext" id="ds_url_queryfield" style="width: 99%; font-family: Verdana;" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="text_default" class="panel">
                <div id="text_default_list" style="height: 288px; overflow: auto;"></div>
            </div>
            <div id="text_event" class="panel">
                <%Server.Execute("events.aspx"); %>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var oNode = null, thePlugins = 'formcombox';
        var attJSON = parent.formattributeJSON;
        var parentEvents = parent.formEvents;
        var events = [];
        var eventsid = UtilNewGuid;
        $(function () {
            $("#ds_sql_dbconn").val(attJSON.dbconn || "");
            if (UE.plugins[thePlugins].editdom) {
                oNode = UE.plugins[thePlugins].editdom;
            }
            biddingFileds(attJSON, oNode ? $(oNode).attr("id") : "", $("#bindfiled"));
            if (oNode) {
                $text = $(oNode);
                $("#defaultvalue").val(UtilDecodeUri($text.attr('defaultvalue')));
                $("#width").val($text.attr('width1'));
                $("#width1").val($text.attr('width2'));
                $("#height1").val($text.attr('height1'));
                $("#ismultiple").prop("checked", "1" == $text.attr("ismultiple"));
                $("#hasempty").prop("checked", "1" == $text.attr("hasempty"));
                $("input[name='datasource'][value='" + $text.attr('datasource') + "']").prop('checked', true);
                $("input[name='listmode'][value='" + $text.attr('listmode') + "']").prop('checked', true);
                if ("1" == $text.attr("datasource")) {
                    $("#ds_dict").hide();
                    $("#ds_sql").hide();
                    $("#ds_custom").show();
                    var custionJSONString = $text.attr("customopts");
                    if ($.trim(custionJSONString).length > 0) {
                        var customJSON = JSON.parse(custionJSONString);
                        var customString = [];
                        for (var i = 0; i < customJSON.length; i++) {
                            customString.push(customJSON[i].title + "," + customJSON[i].value);
                        }
                        $("#custom_string").val(customString.join(';'));
                    }
                    new RoadUI.DragSort($("#custom_sort"));
                }
                else if ("0" == $text.attr("datasource")) {
                    $("#ds_dict").show();
                    $("#ds_sql").hide();
                    $("#ds_url").hide();
                    $("#ds_custom").hide();
                    $("#ds_dict_value").val($text.attr('dictid'));
                    new RoadUI.Dict().setValue($("#ds_dict_value"));
                }
                else if ("2" == $text.attr("datasource")) {
                    $("#ds_dict").hide();
                    $("#ds_sql").show();
                    $("#ds_url").hide();
                    $("#ds_custom").hide();
                    $("#ds_sql_dbconn").val($text.attr('dbconn'));
                    $("#ds_sql_value").val($text.attr('sql'));
                }
                else if ("3" == $text.attr("datasource")) {
                    $("#ds_dict").hide();
                    $("#ds_sql").hide();
                    $("#ds_custom").hide();
                    $("#ds_url").show();
                    $("#ds_url_value").val($text.attr('url'));
                    $("#ds_url_cols").val($text.attr("cols"));
                    $("#ds_url_gettexts").val($text.attr("gettextsurl"));
                    $("#ds_url_queryfield").val($text.attr("queryfield"))
                }

                if ($text.attr('eventsid')) {
                    eventsid = $text.attr('eventsid');
                    events = getEvents(eventsid);
                }
            }

            new RoadUI.DragSort($("#custom_sort"));
            initTabs();
        });

        function loadOptions() {
            var $listDiv = $("#text_default_list");
            var datasource = $(":checked[name='datasource']").val();
            var dvalue = $(":checked", $listDiv).val() || ($(oNode).attr("defaultvalue") || "");
            $listDiv.html('');
            if ("1" == datasource) {
                var custom_string = ($("#custom_string").val() || "").split(';');
                for (var i = 0; i < custom_string.length; i++) {
                    var titlevalue = custom_string[i].split(',');
                    if (titlevalue.length != 2) {
                        continue;
                    }
                    var title = titlevalue[0];
                    var value = titlevalue[1];
                    var option = '<div><input type="radio" ' + (value == dvalue ? 'checked="checked"' : '') + ' value="' + value + '" id="defaultvalue_' + value + '" style="vertical-align:middle;" name="defaultvalue"/><label style="vertical-align:middle;" for="defaultvalue_' + value + '">' + title + '(' + value + ')</label></div>';
                    $listDiv.append(option);
                }
            }
            else if ("0" == datasource) {
                var dictid = $("#ds_dict_value").val();
                $.ajax({
                    url: "getdictchilds.aspx?dictid=" + dictid, cache: false, async: false, dataType: "json", success: function (json) {
                        for (var i = 0; i < json.length; i++) {
                            var title = json[i].title;
                            var value = json[i].id;
                            var option = '<div><input type="radio" ' + (value == dvalue ? 'checked="checked"' : '') + ' value="' + value + '" id="defaultvalue_' + value + '" style="vertical-align:middle;" name="defaultvalue"/><label style="vertical-align:middle;" for="defaultvalue_' + value + '">' + title + '(' + value + ')</label></div>';
                            $listDiv.append(option);
                        }
                    }
                });
            }
            else if ("2" == datasource) {
                var sql = $("#ds_sql_value").val();
                $.ajax({
                    url: "getsqljson.aspx", type: "post", data: { sql: sql, conn: attJSON.dbconn }, cache: false, async: false, dataType: "json", success: function (json) {
                        for (var i = 0; i < json.length; i++) {
                            var title = json[i].title;
                            var value = json[i].id;
                            var option = '<div><input type="radio" ' + (value == dvalue ? 'checked="checked"' : '') + ' value="' + value + '" id="defaultvalue_' + value + '" style="vertical-align:middle;" name="defaultvalue"/><label style="vertical-align:middle;" for="defaultvalue_' + value + '">' + title + '(' + value + ')</label></div>';
                            $listDiv.append(option);
                        }
                    }
                });
            }

        }

        function dsChange(value) {
            if (value == 0) {
                $("#ds_dict").show();
                $("#ds_custom").hide();
                $("#ds_sql").hide();
                $("#ds_url").hide();
            }
            else if (value == 1) {
                $("#ds_dict").hide();
                $("#ds_sql").hide();
                $("#ds_url").hide();
                $("#ds_custom").show();
            }
            else if (value == 2) {
                $("#ds_dict").hide();
                $("#ds_custom").hide();
                $("#ds_url").hide();
                $("#ds_sql").show();
            }
            else if (value == 3) {
                $("#ds_dict").hide();
                $("#ds_custom").hide();
                $("#ds_sql").hide();
                $("#ds_url").show();
            }
        }

        dialog.oncancel = function () {
            if (UE.plugins[thePlugins].editdom) {
                delete UE.plugins[thePlugins].editdom;
            }
        };
        dialog.onok = function () {
            var bindfiled = $("#bindfiled").val();
            var id = attJSON.dbconn && attJSON.dbtable && bindfiled ? attJSON.dbtable + '.' + bindfiled : "";
            var datasource = $(":checked[name='datasource']").val();
            var listmode = $(":checked[name='listmode']").val();
            var width = $("#width").val();
            var width1 = $("#width1").val();
            var height1 = $("#height1").val();
            var ismultiple = $("#ismultiple").prop("checked") ? "1" : "0";
            var defaultvalue = $("#defaultvalue").val();
            var hasempty = $("#hasempty").prop("checked") ? "1" : "0";
            var radios = [];
            var dictid = "";
            var dbconn = "";
            var sql = "";
            var url = "";
            var cols = "";
            var gettextsurl = "";
            var queryfield = "";
            var dvalue = $(":checked[name='defaultvalue']", $("#text_default_list")).val() || "";
            if ("1" == datasource) {
                var custom_string = ($("#custom_string").val() || "").split(';');
                for (var i = 0; i < custom_string.length; i++) {
                    var titlevalue = custom_string[i].split(',');
                    if (titlevalue.length != 2) {
                        continue;
                    }
                    var title = titlevalue[0];
                    var value = titlevalue[1];
                    radios.push({ title: title, value: value });
                }
            }
            else if ("0" == datasource) {
                dictid = $("#ds_dict_value").val();
            }
            else if ("2" == datasource) {
                dbconn = $("#ds_sql_dbconn").val();
                sql = $("#ds_sql_value").val();
            }
            else if ("3" == datasource) {
                url = $("#ds_url_value").val();
                cols = $("#ds_url_cols").val();
                gettextsurl = $("#ds_url_gettexts").val();
                queryfield = $("#ds_url_queryfield").val();
            }

            var html = '<input type="text" type1="flow_combox" id="' + id + '" name="' + id + '" datasource="' + datasource + '" dictid="' + dictid + '" value="下拉组合框" defaultvalue="' + dvalue + '" ';
            if (width) {
                html += 'style="width:' + width + '" ';
                html += 'width1="' + width + '" ';
            }
            if (width1) {
                html += 'width2="' + width1 + '" ';
            }
            if (height1) {
                html += 'height1="' + height1 + '" ';
            }
            if (listmode) {
                html += 'listmode="' + listmode + '" ';
            }
            if (ismultiple) {
                html += 'ismultiple="' + ismultiple + '" ';
            }
            if (defaultvalue) {
                html += 'defaultvalue1="' + encodeURI(defaultvalue) + '" ';
            }
            if ("1" == datasource) {
                html += "customopts='" + JSON.stringify(radios) + "' ";
            }
            if ("2" == datasource) {
                html += 'dbconn="' + dbconn + '" ';
                html += 'sql="' + sql.replaceAll('"', '&quot;') + '" ';
            }
            if ("3" == datasource) {
                html += 'url="' + url.replaceAll('"', '&quot;') + '" ';
                html += 'cols="' + cols.replaceAll('"', '&quot;') + '" ';
                html += 'gettextsurl="' + gettextsurl.replaceAll('"', '&quot;') + '" ';
                html += 'queryfield="' + queryfield + '" ';
            }

            if (events.length > 0) {
                html += 'eventsid="' + eventsid + '" ';
                setEvents(eventsid);
            }

            html += ' />';
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
