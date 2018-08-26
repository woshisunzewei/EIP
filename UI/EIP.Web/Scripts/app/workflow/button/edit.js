define(['edit'], function () {
    $("#chosen_icon").click(function () {
        initChosenIcon();
    });
});

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    UtilAjaxPostWait("/Workflow/Button/SaveButton",
        submitValue, success);
}

//初始化选择图标库
function initChosenIcon() {
    var name = $("#Title").val();
    var title = name == "" ? "请选择图标" : "请选择图标-" + name;
    ArtDialogOpen("/Common/UserControl/ChosenIcon", title, true, 350, 500);
}

//重新赋予Icon
function setIcon() {
    var iconObj = art.dialog.data('iconObj');
    $("#Icon").val(iconObj.Url);
    $("#chosen_icon_show").attr("class", "l-icon l-icon-" + iconObj.Url);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        if (UtilEditIsdialogClose()) {
            if ($("#ButtonId").val() == Language.common.guidempty) {
                UtilFormReset("form");
                UtilFocus("Title");
            }
        }
        else {
            art.dialog.close();
        }
    }
}
