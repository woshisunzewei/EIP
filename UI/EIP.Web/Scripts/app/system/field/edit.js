define(['edit'], function (edit) {
    $("#chosen_menu").click(function () {
        initChosenMenu();
    });
});

//表单条件
function formSubmit() {
    var submitValue = $("#form").getValue();
    //获取值
    var formatter = UtilGetDropdownListText("Formatter");
    submitValue["Formatter"] = formatter == "=请选择=" ? "" : formatter;

    var sorttype = UtilGetDropdownListText("Sorttype");
    submitValue["Sorttype"] = sorttype == "=请选择=" ? "" : sorttype;

    var align = UtilGetDropdownListText("Align");
    submitValue["Align"] = align == "=请选择=" ? "" : align;
  
    UtilAjaxPostWait("/System/Field/SaveField",
        submitValue, success);
}

//提交成功
function success(data) {
    DialogAjaxResult(data);
    var win = artDialog.open.origin; //来源页面
    win.getGridData();
    if (UtilEditIsdialogClose()) {
        if ($("#FieldId").val() == Language.common.guidempty) {
            UtilFormReset("form");
            UtilFocus("Label");
        }
    }
    else {
        art.dialog.close();
    }
}

//初始化选择菜单
function initChosenMenu() {
    ArtDialogOpen("/Common/UserControl/ChosenMenu?parentId=" + $("#MenuId").val() + "&menuType=" + Language.menuType.haveFieldPermission, "请选择父级菜单", true, 350, 300);
}

//重新赋予菜单
function setMenu() {
    var menuObj = art.dialog.data('menuObj')[0];
    $("#MenuName").val(menuObj.name);
    $("#MenuId").val(menuObj.id);
}
