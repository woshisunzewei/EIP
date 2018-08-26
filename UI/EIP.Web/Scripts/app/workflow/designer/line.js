define(["edit"], function () {
    lineObj = art.dialog.data("lineObj");
    $("#form").setValue(lineObj);
    initVal();
    initEvent();
    
});

var submitValue, //提交数据
    lineObj;

//表单条件
function formSubmit() {
    //声明连线对象
    var submitValue = $("#form").getValue();
    var lineObj = {
        "LineId": submitValue.LineId,
        "LineName": submitValue.LineName,
        "LineType": submitValue.LineType
    };
    //判断连线选择类型
    var lineType = $("#LineType").val();
    switch (lineType) {
        case "4": //条件连线
            lineObj.LineConditions = submitValue.LineConditions;
            lineObj.LineReturnPolicy = null;
            break;
        case "6": //退回连线
            lineObj.LineConditions = null;
            lineObj.LineReturnPolicy = submitValue.LineReturnPolicy;
            break;
        default:
            lineObj.LineReturnPolicy = null;
            lineObj.LineConditions = null;
            break;
    }
    lineObj.LineRemark = submitValue.LineRemark;
    art.dialog.data("lineObj", lineObj);
    var win = artDialog.open.origin; //来源页面
    win.resetLine();
    art.dialog.close();
}

//初始化值
function initVal() {
    hidden();
    switch ($("#LineType").val()) {
        case "4"://条件连线
            $("#ShowLineConditions").show();
            break;
        case "6"://退回连线
            $("#ShowLineReturnPolicy").show();
            break;
    }
}

//显示或者隐藏
function hidden() {
    $("#ShowLineConditions,#ShowLineReturnPolicy").hide();
};

//初始化事件
function initEvent() {
    //连线类型
    $("#LineType").change(function () {
        initVal();
    });
}