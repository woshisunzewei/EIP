define(['edit'], function () {});

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    submitValue["RdbmsType"] = UtilGetDropdownListText("RdbmsType");
    UtilAjaxPostWait("/System/DataBase/SaveDataBase",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        if (UtilEditIsdialogClose()) {
            if ($("#AppId").val() == Language.common.guidempty) {
                UtilFormReset("form");
                UtilFocus("Name");
            }
        }
        else {
            art.dialog.close();
        }
    }
}