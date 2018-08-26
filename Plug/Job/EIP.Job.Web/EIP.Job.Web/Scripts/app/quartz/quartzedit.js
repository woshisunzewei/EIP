define([
    'list',
    'edit'
],
 function () {
     initGrid();
     initEvent();
     showByTriggerType();
     setJobInterval();
 });

var $grid, editRow;
//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    submitValue["AssemblyName"] = submitValue["JobType"];
    submitValue["FullName"] = UtilGetDropdownListText("JobType");
    submitValue["TriggerType"] = UtilGetDropdownListText("TriggerType");
    submitValue["EditBeforeJobName"] = UtilGetUrlParam("jobName");
    submitValue["EditBeforeJobGroup"] = UtilGetUrlParam("jobGroup");
    submitValue["Interval"] = $("#Interval").timespinner('getValue');
    submitValue["ChoicedCalendar"] = $("#ChoicedCalendar").val() == "" ? "" : UtilGetDropdownListText("ChoicedCalendar");
    var $ts = $("#list"), ids = $ts.getDataIDs(), json = "";;
    $.each(ids, function (index, rowid) {
        var $row = $ts.getGridRowById(rowid),
            key = $.trim($(":text[name='Key']", $row).val()),
            value = $.trim($(":text[name='Value']", $row).val());
        if (key != "" && value != "") {
            json += "{\"Key\":\"" + key + "\",\"Value\":\"" + value + "\"},";
        }
    });
    json = json.substring(0, json.length - 1);
    json = "[" + json + "]";
    submitValue["ParametersJson"] = json;

    UtilAjaxPostWait("/Quartz/ScheduleJob",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        if (UtilEditIsdialogClose()) {
            if ($("#ConfigId").val() == Language.common.guidempty) {
                UtilFormReset("form");
                UtilFocus("Code");
            }
        }
        else {
            art.dialog.close();
        }
    }
}

//初始化表格
function initGrid() {
    $grid = $("#list").jgridview(
    {
        shrinkToFit: true,
        url: "/Quartz/GetDetailJobDataMap",
        postData: {
            JobName: $("#JobName").val(),
            JobGroup: $("#JobGroup").val()
        },
        multiselect: false,
        colModel: [
           { label: "键", name: "Key", width: 260, fixed: true, sortable: false, editable: true, classes: "input_text" },
           { label: "值", name: "Value", width: 290, sortable: false, fixed: true, editable: true, classes: "input_text" },
           {
               label: "<span class='l-icon-add' title='新增' style='display:inline-block;height:16px;width:16px'></span>", sortable: false, fixed: true, width: 60, name: "Action", align: "center", formatter: function () {
                   return "<span class='l-icon-edit' title='编辑' style='display:inline-block;height:16px;width:16px;margin-right:8px'></span><span class='l-icon-delete' title='删除' style='display:inline-block;height:16px;width:16px'></span>";
               }
           }
        ],
        ondblClickRow: function (rowid, iRow, iCol, e) {
            $("#" + rowid).find(".l-icon-edit").trigger("click");
        },
        height: 150,
        width: 677,
        rowNum: 500,
        loadComplete: function () {
            // 添加一条
            $grid.addRow({
                position: "last"
            });
            //获取调整后高度
            $grid.jqGrid("setGridWidth", 675);
        }
    });

    $(".l-icon-add").bind("click", function () {
        $grid.addRow({ position: "last" });
    });

    $('#list').delegate('.l-icon-delete', 'click', function () { // 删除
        delRow($(this));
    });
    $('#list').delegate('.l-icon-edit', 'click', function () { // 编辑
        var str = $(this).parents(".ui-jqgrid-btable").attr("id");
        var element = $(this).parent().parent();
        if (element.find("input").length > 1) {
            cancelEditTable(); // 取消
        } else {
            editTable(str, element); // 编辑
        }
    });
}

// 删除行
function delRow(element) {
    var rowId = element.parents("tr").attr("id");
    element.parents("table").delRowData(rowId);
}

//取消编辑
function cancelEditTable() {
    if (editRow != null) {
        editRow.find(".input_date,.input_text").each(function () {
            $(this).text($(this).find("input").eq(0).val());
        });
        editRow.find(".input_select").each(function () {
            if ($(this).find("select").eq(0).find("option:selected").index() == 0) {
                $(this).html("");
            } else {
                $(this).html($(this).find("select").eq(0).find("option:selected").text());
            }
        });
        editRow = null;
    }
}

//编辑
function editTable(tableId, element) {
    // 如果当前处于编辑状态直接返回
    if (element == editRow) return;
    cancelEditTable(); // 取消编辑行的编辑状态

    //设置element为编辑列
    element.find(".input_text").each(function () {
        var value = $(this).text();
        $(this).html("<input type='text' value='" + value + "' style='width: 99%;height: 90%;'/>");
    });
    editRow = element;
};

//初始化事件
function initEvent() {
    $("#chosen_cron").click(function () {
        initChosenCron();
    });

    $("#TriggerType").click(function () {
        showByTriggerType();
    });
}

//根据类型显示隐藏
function showByTriggerType() {
    $("#tr_simple").hide();
    $("#tr_cron").hide();
    if (UtilGetDropdownListText("TriggerType") == "SimpleTriggerImpl") {
        $("#tr_simple").show();
    } else {
        $("#tr_cron").show();
    }
}

//初始化选择Cron
function initChosenCron() {
    ArtDialogOpen("/Quartz/Cron", "设置Cron表达式", true, 600, 847);
}

//重复间隔赋值
function setJobInterval() {
    $('#Interval').timespinner('setValue', $("#hiddenInterval").val());
}

//重新赋予Cron
function setCron() {
    var cronObj = art.dialog.data('cronObj');
    $("#Cron").val(cronObj);
}
