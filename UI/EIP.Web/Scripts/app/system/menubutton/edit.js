define(['edit'], function () {
    initEvent();
});

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    UtilAjaxPostWait("/System/MenuButton/SaveMenuButton",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        var menuObj = { id: $("#MenuId").val() };
        art.dialog.data("menuObj", menuObj);
        win.loadTreeAndGrid();
        if (UtilEditIsdialogClose()) {
            if ($("#MenuButtonId").val() == Language.common.guidempty) {
                $("#Name,#Script,#Remark").val("");
                $("#OrderNo").val((parseFloat($("#OrderNo").val()) + 1));
                UtilFocus("Name");
            }
        }
        else {
            art.dialog.close();
        }
    }
}

//初始化选择图标库
function initChosenIcon() {
    var name = $("#Name").val();
    var title = name == "" ? "请选择图标" : "请选择图标-" + name;
    ArtDialogOpen("/Common/UserControl/ChosenIcon", title, true, 350, 500);
}

//重新赋予Icon
function setIcon() {
    var iconObj = art.dialog.data('iconObj');
    $("#Icon").val(iconObj.Url);
    $("#chosen_icon_show").attr("class", "l-icon l-icon-" + iconObj.Url);
}

//初始化选择菜单
function initChosenMenu() {
    ArtDialogOpen("/Common/UserControl/ChosenMenu?parentId=" + $("#MenuId").val() + "&isRemove=false", "请选择父级菜单", true, 350, 300);
}

//重新赋予菜单
function setMenu() {
    if (art.dialog.data('menuObj').length === 0) {
        $("#MenuName").val("");
        $("#MenuId").val(UtilNewGuid());
        return;
    }
    var menuObj = art.dialog.data('menuObj')[0];
    $("#MenuName").val(menuObj.name);
    $("#MenuId").val(menuObj.id);
}

//初始化事件
function initEvent() {
    $("#chosen_icon").click(function () {
        initChosenIcon();
    });

    $("#chosen_menu").click(function () {
        initChosenMenu();
    });
}
