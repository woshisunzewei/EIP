define(['edit'], function () { });

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    UtilAjaxPostWait("/System/District/SaveDistrict",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.loadTreeAndGrid();
        art.dialog.close();
    }
}