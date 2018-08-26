define(['edit'], function () { });

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    submitValue["DesignJson"] = '{"title":"{0}","nodes":{"{1}":{"name":"开始","left":163,"top":220,"type":"start round","width":24,"height":24,"alt":true},"{2}":{"name":"结束","left":803,"top":220,"type":"end round","width":24,"height":24,"alt":true}},"lines":{},"areas":{},"initNum":1}';
    UtilAjaxPostWait("/Workflow/Designer/Save",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        if (UtilEditIsdialogClose()) {
            if ($("#是否继续添加").val() == Language.common.guidempty) {
                $("#Name,#Code,#PageUrl,#Remark").val("");
                $("#OrderNo").val((parseFloat($("#OrderNo").val()) + 1));
            }
        }
        else {
            art.dialog.close();
        }
    }
}