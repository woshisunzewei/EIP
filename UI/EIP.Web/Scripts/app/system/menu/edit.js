define(['edit'], function () { initEvent(); });

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    submitValue["AppId"] = submitValue["App"].split("|")[0];
    submitValue["AppCode"] = submitValue["App"].split("|")[1];
    UtilAjaxPostWait("/System/Menu/SaveMenu",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        var menuObj = { pId: $("#ParentId").val(), menuId: data.Data };
        art.dialog.data("menuObj", menuObj);
        win.loadTreeAndGrid();
        if (UtilEditIsdialogClose()) {
            if ($("#MenuId").val() == Language.common.guidempty) {
                $("#Name,#Action,#OpenUrl,#Remark").val("");
                $("#Code").val($("#HiddenCode").val());
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
    ArtDialogOpen("/Common/UserControl/ChosenMenu?menuId=" + $("#MenuId").val() + "&parentId=" + $("#ParentId").val(), "请选择父级菜单", true, 350, 300);
}

//重新赋予菜单
function setMenu() {
    //判断是新增还是编辑
    var edit = UtilGetUrlParam("id");
    if (art.dialog.data('menuObj').length === 0) {
        $("#ParentName").val("");
        $("#ParentId").val(Language.common.guidempty);
        if (edit == "") {
            $("#Code").val("");
        }
        $("#HiddenCode").val("");
        return;
    }
    var menuObj = art.dialog.data('menuObj')[0];
    $("#ParentName").val(menuObj.name);
    $("#ParentId").val(menuObj.id);
    if (edit == "") {
        $("#Code").val(menuObj.code);
    }
    $("#HiddenCode").val(menuObj.code);
}

//初始化事件
function initEvent() {
    $("#Name").keyup(function () {
        var py = makePy($(this).val());
        var hiddenCode = $("#HiddenCode").val();
        $("#Code").val(hiddenCode === "" ? py[0] : hiddenCode + "_" + py[0]);
    });

    $("#chosen_icon").click(function () {
        initChosenIcon();
    });

    $("#chosen_menu").click(function () {
        initChosenMenu();
    });
}