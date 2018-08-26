
//得到一连接的所有表
function getTables(connid)
{
    var tables = [];
    UtilAjaxPost("/System/DataBase/GetDataBaseTables", { id: connid }, function (json) {
        for (var i = 0; i < json.length; i++) {
            tables.push(json[i]);
        }
    });
  
    return tables;
}
//得到一个表所有字段
function getFields(connid, table){
    var fields = [];
    UtilAjaxPost("/System/DataBase/GetDataBaseColumns", { id: connid, tableName: table }, function (json) {
        for (var i = 0; i < json.length; i++) {
            fields.push(json[i]);
        }
    });
    return fields;
}

//得到表下拉选择项
function getTableOps(connid, table)
{
    var options = '<option value=""></option>';
    var tableds = getTables(connid);
    for (var i = 0; i < tableds.length; i++)
    {
        options += '<option value="' + tableds[i].TableName + '" ' + (tableds[i].TableName == table ? 'selected="selected"' : '') + '>' + tableds[i].TableName + (tableds[i].Description ? '(' + tableds[i].Description + ')' : '') + '</option>';
    }
    return options;
}

//得到字段下拉选择项
function getFieldsOps(connid, table, field){
    var options = '';
    var fields = getFields(connid, table);
    for (var i = 0; i < fields.length; i++)
    {
        options += '<option value="' + fields[i].ColumnName + '" ' + (fields[i].ColumnName == field ? 'selected="selected"' : '') + '>' + fields[i].ColumnName + (fields[i].ColumnDescription ? '(' + fields[i].ColumnDescription + ')' : '') + '</option>';
    }
    return options;
}
//测试sql合法性
function testSql(sql)
{
    if ($.trim(sql).length == 0)
    {
        alert("SQL语句为空");
        return;
    }
    var json = parent.formattributeJSON;
    if (!json || !json.dbconn)
    {
        alert("未设置数据连接");
        return;
    }
    $.ajax({
        url:  "/WorkFlowFormDesigner/TestSql", async: false, cache: false, method: "post", data: { sql: sql, dbconn: json.dbconn }, success: function (txt)
        {
            alert(txt);
        }
    });
}
//在文本框光标入插入文本
function insertText(obj, str)
{
    if (document.selection)
    {
        var sel = document.selection.createRange();
        sel.text = str;
    }
    else if (typeof obj.selectionStart === 'number' && typeof obj.selectionEnd === 'number')
    {
        var startPos = obj.selectionStart,
            endPos = obj.selectionEnd,
            cursorPos = startPos,
            tmpStr = obj.value;
        obj.value = tmpStr.substring(0, startPos) + str + tmpStr.substring(endPos, tmpStr.length);
        cursorPos += str.length;
        obj.selectionStart = obj.selectionEnd = cursorPos;
    }
    else
    {
        obj.value += str;
    }
}
//设置默认值
function setDefaultValue(obj,value)
{
    insertText(obj, value);
}

function initTabs()
{
    var tabs = $G('tabhead').children;
    for (var i = 0; i < tabs.length; i++)
    {
        domUtils.on(tabs[i], "click", function (e)
        {
            var target = e.target || e.srcElement;
            setTabFocus(target.getAttribute('data-content-id'));
        });
    }
    if (tabs.length > 0)
    {
        setTabFocus(tabs[0].getAttribute('data-content-id'));
    }
}
function setTabFocus(id)
{
    if (!id) return;
    var i, bodyId, tabs = $G('tabhead').children;
    for (i = 0; i < tabs.length; i++)
    {
        bodyId = tabs[i].getAttribute('data-content-id')
        if (bodyId == id)
        {
            domUtils.addClass(tabs[i], 'focus');
            domUtils.addClass($G(bodyId), 'focus');
        }
        else
        {
            domUtils.removeClasses(tabs[i], 'focus');
            domUtils.removeClasses($G(bodyId), 'focus');
        }
    }

}

function event_change(eventName)
{
    var isIn = false;
    for (var i = 0; i < events.length; i++)
    {
        if (events[i].name == eventName)
        {
            $("#event_script").val(events[i].script);
            isIn = true;
            break;
        }
    }
    if (!isIn)
    {
        $("#event_script").val("");
    }
}
function saveEvent()
{
    var eventName = $("#event_name").val();
    if (!eventName || eventName.length == 0)
    {
        return;
    }
    var isIn = false;
    for (var i = 0; i < events.length; i++)
    {
        if (events[i].name == eventName)
        {
            events[i].script = $("#event_script").val();
            isIn = true;
            break;
        }
    }
    if (!isIn)
    {
        events.push({ 'name': eventName, 'script': $("#event_script").val() });
    }
}
function testScript()
{
    try
    {
        eval($("#event_script").val());
        alert("脚本测试成功!");
    }
    catch (e)
    {
        alert(e);
    }
}

function setEvents(id)
{
    if (!id || $.trim(id).length == 0) return;
    var formEvents = parent.formEvents;
    var isIn = false;
    for (var i = 0; i < formEvents.length; i++)
    {
        if (formEvents[i].id == id)
        {
            formEvents[i].events = events;
            isIn = true;
            break;
        }
    }
    if (!isIn)
    {
        formEvents.push({ "id": id, "events": events });
    }
}

function getEvents(id)
{
    if (!id || $.trim(id).length == 0) return;
    var formEvents = parent.formEvents;
    for (var i = 0; i < formEvents.length; i++)
    {
        if (formEvents[i].id == id)
        {
            if (formEvents[i].events.length > 0)
            {
                $("#event_name").val(formEvents[i].events[0].name);
                $("#event_script").val(formEvents[i].events[0].script);
            }
            return formEvents[i].events;
            break;
        }
    }
    return [];
}

/**
 * 作用:返回一个GUID值
 * 参数:无
 */
function UtilNewGuid() {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "-";
    }
    return guid;
};

//初始化绑定字段选择项
function biddingFileds(attJSON, textid, $bindfiled) {
    if (attJSON.dbconn && attJSON.dbtable) {
        var fieldvalue = "";
        if (textid) {
            var textidarray = textid.split('.');
            if (textidarray.length == 2) {
                fieldvalue = textidarray[1];
            }
        }
        var opts = getFieldsOps(attJSON.dbconn, attJSON.dbtable, fieldvalue);
        $bindfiled.html(opts);
    }
}