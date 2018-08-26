define(['edit'], function () {
    initEvent();
});

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    UtilAjaxPostWait("/System/User/SaveUser",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        if (UtilEditIsdialogClose()) {
            if ($("#UserId").val() == Language.common.guidempty) {
                UtilFormReset("form");
                UtilFocus("Code");
            }
        }
        else {
            art.dialog.close();
        }
    }
}

//初始化事件
function initEvent() {
    $("#Name").keyup(function () {
        var py = makePy($(this).val());
        $("#Code").val(py[0].toLowerCase());
    });
}
