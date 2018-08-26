define(['edit'], function () { initEvent(); });

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    UtilAjaxPostWait("/System/Organization/SaveOrganization",
        submitValue, success);
}

//初始化选择组织
function initChosenOrganization() {
    ArtDialogOpen("/Common/UserControl/ChosenOrganization?organizationId=" + $("#OrganizationId").val() + "&parentId=" + $("#ParentId").val(), "请选择父级菜单", true, 350, 300);
}

//重新赋予菜单
function setOrganization() {
    var edit = UtilGetUrlParam("id");
    var organizationObj = art.dialog.data('organizationObj')[0];
    if (organizationObj.length === 0) {
        $("#ParentName").val("");
        $("#ParentId").val(Language.common.guidempty);
        if (edit == "") {
            $("#Code").val("");
        }
        $("#HiddenCode").val("");
        return;
    }
    $("#ParentName").val(organizationObj.name);
    $("#ParentId").val(organizationObj.id);
    if (edit == "") {
        $("#Code").val(organizationObj.code);
    }
    $("#HiddenCode").val(organizationObj.code);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        var orgObj = { pId: $("#ParentId").val() };
        art.dialog.data("orgObj", orgObj);
        win.loadTreeAndGrid();
        if (UtilEditIsdialogClose()) {
            if ($("#OrganizationId").val() == Language.common.guidempty) {
                $("#Name,#ShortName,#Remark,#MainSupervisor,#MainSupervisorContact").val("");
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

//初始化事件
function initEvent() {
    $("#Name").keyup(function () {
        var py = makePy($(this).val());
        var hiddenCode = $("#HiddenCode").val();
        $("#Code").val(hiddenCode === "" ? py[0] : hiddenCode + "_" + py[0]);
    });

    //选择
    $("#chosen_organization").click(function () {
        initChosenOrganization();
    });
}