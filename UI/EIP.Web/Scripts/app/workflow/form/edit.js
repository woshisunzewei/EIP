define(['edit'], function () {
    dbChange($("#DataBaseId").val(), $("#HiddenDataBaseTable").val() || "");
    //改变事件
    $("#DataBaseId").change(function () {
        dbChange($("#DataBaseId").val(), "");
    });
});

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    UtilAjaxPostWait("/Workflow/Form/Save",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        if (UtilEditIsdialogClose()) {
            if ($("#FormId").val() == Language.common.guidempty) {
                UtilFormReset("form");
                UtilFocus("Name");
            }
        }
        else {
            art.dialog.close();
        }
    }
}

//数据库连接切换
function dbChange(value, table) {
    if (!value) return;
    $("#DataBaseTable").html(getTableOps(value, table));
    var dbtablepk = $("#HiddenDataBasePrimaryKey").val() || "";
    tableChange($("#DataBaseTable").val(), dbtablepk);
}

//删除
function tableChangeThis(opt) {
    var dbtablepk = $("#HiddenDataBasePrimaryKey").val() || "";
    tableChange($(opt).val(), dbtablepk);
}

//表切换
function tableChange(value, fields) {
    if (!value) return;
    var edit = "1" == UtilGetUrlParam("edit");
    var dbtabletitle = $("#HiddenDataBaseTitle").val() || "";
    var conn = $("#DataBaseId").val();
    var opts = getFieldsOps(conn, value, fields);
    $("#DataBasePrimaryKey").html(opts);
    if (edit) {
        var opts1 = getFieldsOps(conn, value, dbtabletitle);
        $("#DataBaseTitle").html(opts1);
    }
    else {
        $("#DataBaseTitle").html(opts);
    }
}