var workflowX,
           workflowY,
           $templateId = $("#templateId"),
           size = UtilWindowHeightWidth(),
           startId = UtilNewGuid(),
           endId = UtilNewGuid(),
           property = {
               width: 990,
               height: 590,
               headLabel: true,
               toolBtns: [
                   //"start round",
                   //"end round",
                   "task",
                   "node",
                   "condition",
                   "state",
                   "plug",
                   "join",
                   "fork",
                   "arrowretweet"
               ],
               haveHead: false,
               headBtns: [
                   "open",
                   "attibutes",
                   "up",
                   "save",
                   "undo",
                   "redo",
                   "reload"
               ], //如果haveHead=true，则定义HEAD区的按钮
               haveTool: false,
               haveGroup: true,
               useOperStack: true
           },
           remark = {
               cursor: "选择指针",
               mutiselect: "多选",
               direct: "结点连线",
               start: "开始结点",
               "end": "结束结点",
               "task": "审批结点",
               node: "自动结点",
               condition: "决策结点",
               state: "定时节点",
               plug: "附加插件",
               fork: "分支结点",
               "join": "联合结点",
               group: "组织划分框编辑开关",
               arrowretweet: "循环结点",
               complex: "聚合结点"
           },
           workflow;

//初始化流程
function initFlow() {
    //初始化流程图
    workflow = $.createGooFlow($("#workflow"), property);
    workflow.setNodeRemarks(remark);
};

//根据模版Id获取模版Json数据
function getTemplateData() {
    var json = JSON.parse($("#designJson").html());
    //重新赋予开始结束值
    $.each(json.nodes, function (i, node) {
        if (node.type === "start round") {
            startId = i;
        }
        if (node.type === "end round") {
            endId = i;
        }
    });
    //清楚界面信息
    workflow.clearData();
    //重绘窗口
    workflow.loadData(json);
}

$(document).ready(function () {
    //初始化流程
    initFlow();
    //获取流程信息
    getTemplateData();
});