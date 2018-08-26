define(['edit'], function () { initEvent(); });

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    UtilAjaxPostWait("/System/Dictionary/SaveDictionary",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        var dicObj = { pId: $("#ParentId").val() };
        art.dialog.data("dicObj", dicObj);
        win.loadTreeAndGrid();
        if (UtilEditIsdialogClose()) {
            if ($("#DictionaryId").val() == Language.common.guidempty) {
                $("#Name,#Value,#Remark").val("");
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

//初始化选择菜单
function initChosenDic() {
    ArtDialogOpen("/Common/UserControl/ChosenDictionaryEditAll?id=" + $("#DictionaryId").val() + "&parentId=" + $("#ParentId").val(), "请选择父级字典", true, 350, 300);
}

//重新赋予字典
function setDicEditAllValue() {
    var edit = UtilGetUrlParam("id");
    if (art.dialog.data('dictionaryObj').length === 0) {
        $("#ParentName").val("");
        $("#ParentId").val(Language.common.guidempty);
        if (edit == "") {
            $("#Code").val("");
        }
        $("#HiddenCode").val("");
        return;
    }
    var dicObj = art.dialog.data('dictionaryObj')[0];
    $("#ParentName").val(dicObj.name);
    $("#ParentId").val(dicObj.id);
    if (edit == "") {
        $("#Code").val(dicObj.code);
    }
    $("#HiddenCode").val(dicObj.code);
}

//初始化事件
function initEvent() {
    $("#Name").keyup(function () {
        var py = makePy($(this).val());
        var hiddenCode = $("#HiddenCode").val();
        $("#Code").val(hiddenCode === "" ? py[0] : hiddenCode + "_" + py[0]);
    });
}