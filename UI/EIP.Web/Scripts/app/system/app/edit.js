define(['edit'], function () { });

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    UtilAjaxPostWait("/System/App/SaveApp",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        //如果是新增则提示是否可以继续添加
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