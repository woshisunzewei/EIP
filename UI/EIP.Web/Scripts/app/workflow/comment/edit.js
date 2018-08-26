define(['edit'], function () { });

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    UtilAjaxPostWait("/Workflow/Comment/SaveComment",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        if (UtilEditIsdialogClose()) {
            if ($("#CommentId").val() == Language.common.guidempty) {
                UtilFormReset("form");
                UtilFocus("Content");
            }
        }
        else {
            art.dialog.close();
        }
    }
}
