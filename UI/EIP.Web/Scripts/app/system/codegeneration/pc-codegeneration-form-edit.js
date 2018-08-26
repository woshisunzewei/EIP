define(['edit'], function () { });

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    art.dialog.data("editDataJsonObj", submitValue);
    var win = artDialog.open.origin; //来源页面
    win.resetFormEditDataJson();
    art.dialog.close();
}