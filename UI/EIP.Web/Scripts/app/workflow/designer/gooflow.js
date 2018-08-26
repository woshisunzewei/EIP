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
            "task",
            "arrowretweet"
        ],
        haveHead: true,
        headBtns: [
            "open",
            "attibutes",
            "save",
            "undo",
            "redo",
            "reload"
        ], //如果haveHead=true，则定义HEAD区的按钮
        haveTool: true,
        haveGroup: true,
        useOperStack: true
    },
    remark = {
        cursor: "选择指针",
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
        group: "泳道",
        arrowretweet: "子流程",
        complex: "聚合结点"
    },
    workflow,
    globalLineType = { Conditions: "4", Return: "6" };

//初始化流程
function initFlow() {
    //初始化流程图
    workflow = $.createGooFlow($("#workflow"), property);
    workflow.setNodeRemarks(remark);
    $(".ico_open").attr("title", "流程Json字符串");
    $(".ico_save").attr("title", "保存流程图");
    $(".ico_undo").attr("title", "撤销最近一次操作");
    $(".ico_redo").attr("title", "重做最近一次被撤销的操作");
    $(".ico_reload").attr("title", "刷新重载流程图");
    $(".ico_attibutes").attr("title", "复制");
    $(".ico_up").attr("title", "发布流程");
    //初始化提示
    $("[title]").each(function () {
        $(this).qtip({
            content: {
                attr: "title"
            },
            position: {
                target: "mouse", // 跟随鼠标指针
                effect: false, // 关闭效果
                viewport: $(window),
                adjust: { x: 17, y: 10 } // tip位置偏移，防止遮住鼠标指针
            }
        });
    });
};

//初始化事件
function initEvent() {
    //获取移动位置
    $(document).mousemove(function (event) {
        workflowX = event.clientX;
        workflowY = event.clientY;
    });

    var size = UtilWindowHeightWidth();
    property.width = size.WinW;
    property.height = size.WinH;

    //打开
    workflow.onBtnOpenClick = function () {
        art.dialog.prompt("", function (val) {
            workflow.setTitle(val);
        }, formatJson(JSON.stringify(workflow.exportData())));
    };

    //保存
    workflow.onBtnSaveClick = function () {
        art.dialog.confirm("是否保存流程信息？", function () {
            //节点
            var activity = "", exportObj = workflow.exportData();

            $.each(exportObj.nodes, function (i, node) {
                activity +=
                    "{\"ActivityId\": \"" + i + "\""
                    + ",\"Name\": \"" + node.name + "\""
                    + ",\"Left\":  " + node.left + ""
                    + ",\"Top\":  " + node.top + ""
                    + ",\"Type\":  \"" + node.type + "\""
                    + ",\"Width\":  " + node.width + ""
                    + ",\"Height\":  " + node.height + ""
                    + ",\"Alt\":  " + node.alt + ""
                    + ",\"Opinion\":  " + checkActivity(node.activityOpinion) + ""
                    + ",\"CommentsBelow\":  " + checkActivity(node.activityCommentsBelow) + ""
                    + ",\"TimeoutRemind\":  " + checkActivity(node.activityTimeoutRemind) + ""
                    + ",\"Archive\":  " + checkActivity(node.activityArchive) + ""
                    + ",\"WorkTime\":  " + checkActivity(node.activityWorkTime) + ""
                    + ",\"TimeoutRemindTypeEmail\":  " + checkActivity(node.activityTimeoutRemindTypeEmail) + ""
                    + ",\"TimeoutRemindTypeNote\":  " + checkActivity(node.activityTimeoutRemindTypeNote) + ""
                    + ",\"TimeoutRemindTypeWx\":  " + checkActivity(node.activityTimeoutRemindTypeWx) + ""
                    + ",\"Remark\":  " + checkActivity(node.activityRemark) + ""
                    + ",\"ProcessorType\":  " + checkActivity(node.activityProcessorType) + ""
                    + ",\"ProcessorHandler\":  " + checkActivity(node.activityProcessorHandler) + ""
                    + ",\"HandlingStrategy\":  " + checkActivity(node.activityHandlingStrategy) + ""
                    + ",\"HandlingStrategyPercentage\":  " + checkActivity(node.activityHandlingStrategyPercentage) + ""
                    + ",\"ButtonList\":  " + (((node.activityButtons === "") || typeof (node.activityButtons) === "undefined") ? "null" : JSON.stringify(node.activityButtons)) + ""
                    + ",\"EventSubmitBefore\":  " + checkActivity(node.activityEventSubmitBefore) + ""
                    + ",\"EventSubmitAfter\":  " + checkActivity(node.activityEventSubmitAfter) + ""
                    + ",\"EventBackBefore\":  " + checkActivity(node.activityEventBackBefore) + ""
                    + ",\"EventBackAfter\":  " + checkActivity(node.activityEventBackAfter) + ""
                    + ",\"FormId\":  " + checkActivity(node.activityForm) + "},";
            });

            if (activity !== "") {
                activity = "[" + activity.substring(0, activity.length - 1) + "]";
            }
            //连接线
            var lineData = "";

            $.each(exportObj.lines, function (i, line) {
                lineData +=
                    "{\"LineId\": \"" + i + "\""
                    + ",\"Name\": \"" + line.name + "\""
                    + ",\"LineType\": " + (checkActivity(line.lineType) == null ? 2 : line.lineType) + ""
                    + ",\"ReturnPolicy\": " + checkActivity(line.lineReturnPolicy) + ""
                    + ",\"Conditions\": " + checkActivity(line.lineConditions) + ""
                    + ",\"Type\":  \"" + line.type + "\""
                    + ",\"M\":  " + checkActivity(line.M) + ""
                    + ",\"From\":  \"" + line.from + "\""
                    + ",\"To\":  \"" + line.to + "\"},";
            });

            if (lineData !== "") {
                lineData = "[" + lineData.substring(0, lineData.length - 1) + "]";
            }

            //区域
            var areasData = "";

            $.each(exportObj.areas, function (i, areas) {
                areasData +=
                    "{\"AreasId\": \"" + i + "\""
                    + ",\"Name\": \"" + areas.name + "\""
                    + ",\"Left\":  " + areas.left + ""
                    + ",\"Top\":  " + areas.top + ""
                    + ",\"Color\":  \"" + areas.color + "\""
                    + ",\"Width\":  " + areas.width + ""
                    + ",\"Height\":  " + areas.height + ""
                    + ",\"Alt\":  \"" + areas.alt + "\"},";
            });

            if (areasData !== "") {
                areasData = "[" + areasData.substring(0, areasData.length - 1) + "]";
            }
            //获取流程数据
            UtilAjaxPostWait("/Workflow/Designer/SaveWorkflowProcessJson", {
                activity: activity,
                line: lineData,
                areas: areasData,
                designJson: JSON.stringify(exportObj),
                processId: $("#ProcessId").val()
            }, function (data) {
                DialogAjaxResult(data);
            });

        });
    };

    //刷新
    workflow.onFreshClick = function () {
        getTemplateData();
    };

    //单元由不选中变为选中触发
    workflow.onItemFocus = function (id, type) {
        //开始和结束节点无删除图标
        if (id === startId || id === endId) {
            $(".rs_close", $("#" + id)).remove();
        }
        return true;
    };

    //单元连接线双击事件
    var tmpClk = "PolyLine";
    if (GooFlow.prototype.useSVG !== "")
        tmpClk = "g";
    $(workflow.$draw).delegate(tmpClk, "dblclick", { inthis: workflow }, function (e) {
        var $this = $(this),
            //连线Id
            lineId = $this.attr("id"),
            //连线对象
            line = workflow.getItemInfo(lineId, "line"),
            //传递给弹出框对象
            lineObj = {
                LineId: lineId,
                LineName: line.name,
                LineType: (typeof (line.lineType) == "undefined") ? "2" : line.lineType,
                LineConditions: (typeof (line.lineConditions) == "undefined") ? "" : line.lineConditions,
                LineReturnPolicy: (typeof (line.lineReturnPolicy) == "undefined") ? "2" : line.lineReturnPolicy,
                LineRemark: (typeof (line.lineRemark) == "undefined") ? "" : line.lineRemark
            };
        //如果是条件
        art.dialog.data("lineObj", lineObj); // 存储数据
        art.dialog.open("/Workflow/Designer/LineActivity", {
            title: "连线节点配置",
            height: 340,
            padding: 1,
            width: 420,
            resize: false,
            lock: true,
            opacity: 0.3,
            close: function () { }
        }, false);

    });
}

//格式化json
var formatJson = function (json, options) {
    var reg = null,
        formatted = "",
        pad = 0,
        PADDING = "    "; // one can also use '\t' or a different number of spaces

    // optional settings
    options = options || {};
    // remove newline where '{' or '[' follows ':'
    options.newlineAfterColonIfBeforeBraceOrBracket = (options.newlineAfterColonIfBeforeBraceOrBracket === true) ? true : false;
    // use a space after a colon
    options.spaceAfterColon = (options.spaceAfterColon === false) ? false : true;

    // begin formatting...

    // make sure we start with the JSON as a string
    if (typeof json !== "string") {
        json = JSON.stringify(json);
    }
    // parse and stringify in order to remove extra whitespace
    json = JSON.parse(json);
    json = JSON.stringify(json);

    // add newline before and after curly braces
    reg = /([\{\}])/g;
    json = json.replace(reg, "\r\n$1\r\n");

    // add newline before and after square brackets
    reg = /([\[\]])/g;
    json = json.replace(reg, "\r\n$1\r\n");

    // add newline after comma
    reg = /(\,)/g;
    json = json.replace(reg, "$1\r\n");

    // remove multiple newlines
    reg = /(\r\n\r\n)/g;
    json = json.replace(reg, "\r\n");

    // remove newlines before commas
    reg = /\r\n\,/g;
    json = json.replace(reg, ",");

    // optional formatting...
    if (!options.newlineAfterColonIfBeforeBraceOrBracket) {
        reg = /\:\r\n\{/g;
        json = json.replace(reg, ":{");
        reg = /\:\r\n\[/g;
        json = json.replace(reg, ":[");
    }
    if (options.spaceAfterColon) {
        reg = /\:/g;
        json = json.replace(reg, ": ");
    }

    $.each(json.split("\r\n"), function (index, node) {
        var i = 0,
            indent = 0,
            padding = "";

        if (node.match(/\{$/) || node.match(/\[$/)) {
            indent = 1;
        } else if (node.match(/\}/) || node.match(/\]/)) {
            if (pad !== 0) {
                pad -= 1;
            }
        } else {
            indent = 0;
        }

        for (i = 0; i < pad; i++) {
            padding += PADDING;
        }

        formatted += padding + node + "\r\n";
        pad += indent;
    });

    return formatted;
};

//初始化右键功能
function initContextMenu() {
    var baseitem = [
        {
            id: "addtask",
            text: "新增审批",
            icon: "users",
            action: function () {
                workflow.addNode(UtilNewGuid(),
                {
                    "name": "审批",
                    "left": workflowX - 30,
                    "top": workflowY - 30,
                    "type": "task",
                    "width": 24,
                    "height": 24,
                    "alt": true
                });
            }
        },
        {
            id: "addcondition",
            text: "新增决策",
            icon: "arrow-branch-270",
            action: function () {
                workflow.addNode(UtilNewGuid(),
                {
                    "name": "决策",
                    "left": workflowX - 30,
                    "top": workflowY - 30,
                    "type": "condition",
                    "width": 24,
                    "height": 24,
                    "alt": true
                });
            }
        }
    ];

    //整个流程界面设计区均可以右键点击
    $(".GooFlow_work").WinContextMenu({
        cancel: ".cancel",
        items: baseitem,
        action: function (e) { alert(e.id); } //自由设计项事件回调
    });
};

//检查活动
function checkActivity(value) {
    var compareValue = null;
    if (value !== "" && typeof (value) !== "undefined") {
        compareValue = "\"" + value + "\"";
    }
    return compareValue;
}

//打开节点
function openNode() {
    var activityId = $(".item_focus").attr("id"),
        focusNode = workflow.getItemInfo(activityId, "node"),
        activityObj = {
            ActivityId: activityId,
            ActivityName: focusNode.name,
            ActivityOpinion: (typeof (focusNode.activityOpinion) == "undefined") ? "0" : focusNode.activityOpinion,
            ActivityCommentsBelow: (typeof (focusNode.activityCommentsBelow) == "undefined") ? "0" : focusNode.activityCommentsBelow,
            ActivityTimeoutRemind: (typeof (focusNode.activityTimeoutRemind) == "undefined") ? "0" : focusNode.activityTimeoutRemind,
            ActivityArchive: (typeof (focusNode.activityArchive) == "undefined") ? "0" : focusNode.activityArchive,
            ActivityWorkTime: (typeof (focusNode.activityWorkTime) == "undefined") ? "0" : focusNode.activityWorkTime,
            ActivityForm: (typeof (focusNode.activityForm) == "undefined") ? "" : focusNode.activityForm,
            ActivityTimeoutRemindTypeEmail: (typeof (focusNode.activityTimeoutRemindTypeEmail) == "undefined") ? "false" : focusNode.activityTimeoutRemindTypeEmail,
            ActivityTimeoutRemindTypeNote: (typeof (focusNode.activityTimeoutRemindTypeNote) == "undefined") ? "false" : focusNode.activityTimeoutRemindTypeNote,
            ActivityTimeoutRemindTypeWx: (typeof (focusNode.activityTimeoutRemindTypeWx) == "undefined") ? "false" : focusNode.activityTimeoutRemindTypeWx,
            ActivityRemark: (typeof (focusNode.activityRemark) == "undefined") ? "" : focusNode.activityRemark,
            //策略
            ActivityProcessorType: (typeof (focusNode.activityProcessorType) == "undefined") ? "0" : focusNode.activityProcessorType,
            ActivityProcessorHandler: (typeof (focusNode.activityProcessorHandler) == "undefined") ? "" : focusNode.activityProcessorHandler,
            ActivityHandlingStrategy: (typeof (focusNode.activityHandlingStrategy) == "undefined") ? "0" : focusNode.activityHandlingStrategy,
            ActivityHandlingStrategyPercentage: (typeof (focusNode.activityHandlingStrategyPercentage) == "undefined") ? "100" : focusNode.activityHandlingStrategyPercentage,
            //按钮
            ActivityButtons: (typeof (focusNode.activityButtons) == "undefined") ? [] : focusNode.activityButtons,
            //数据

            //事件
            ActivityEventSubmitBefore: (typeof (focusNode.activityEventSubmitBefore) == "undefined") ? "" : focusNode.activityEventSubmitBefore,
            ActivityEventSubmitAfter: (typeof (focusNode.activityEventSubmitAfter) == "undefined") ? "" : focusNode.activityEventSubmitAfter,
            ActivityEventBackBefore: (typeof (focusNode.activityEventBackBefore) == "undefined") ? "" : focusNode.activityEventBackBefore,
            ActivityEventBackAfter: (typeof (focusNode.activityEventBackAfter) == "undefined") ? "" : focusNode.activityEventBackAfter

        };
    art.dialog.data("activityObj", activityObj); // 存储数据
    art.dialog.open("/Workflow/Designer/ApproveActivity", {
        title: focusNode.name + "节点配置",
        height: 440,
        padding: 1,
        width: 740,
        resize: false,
        lock: true,
        opacity: 0.3
    }, false);
};

//重新设置Node信息
function resetActivity() {
    var activityObj = art.dialog.data("activityObj"),
        focusNode = workflow.getItemInfo(activityObj.ActivityId, "node");
    workflow.setName(
        activityObj.ActivityId,
        activityObj.ActivityName, "node");
    //设置数据
    focusNode.activityOpinion = activityObj.ActivityOpinion;
    focusNode.activityCommentsBelow = activityObj.ActivityCommentsBelow;
    focusNode.activityTimeoutRemind = activityObj.ActivityTimeoutRemind;
    focusNode.activityArchive = activityObj.ActivityArchive;
    focusNode.activityWorkTime = activityObj.ActivityWorkTime;
    focusNode.activityTimeoutRemindTypeEmail = activityObj.ActivityTimeoutRemindTypeEmail;
    focusNode.activityTimeoutRemindTypeNote = activityObj.ActivityTimeoutRemindTypeNote;
    focusNode.activityTimeoutRemindTypeWx = activityObj.ActivityTimeoutRemindTypeWx;
    focusNode.activityTimeoutRemindType = activityObj.ActivityTimeoutRemindType;
    focusNode.activityRemark = activityObj.ActivityRemark;
    focusNode.activityProcessorType = activityObj.ActivityProcessorType;
    focusNode.activityProcessorHandler = activityObj.ActivityProcessorHandler;
    focusNode.activityHandlingStrategy = activityObj.ActivityHandlingStrategy;
    focusNode.activityHandlingStrategyPercentage = activityObj.ActivityHandlingStrategyPercentage;
    focusNode.activityForm = activityObj.ActivityForm;
    //按钮
    focusNode.activityButtons = activityObj.ActivityButtons;
    //事件
    focusNode.activityEventSubmitBefore = activityObj.ActivityEventSubmitBefore;
    focusNode.activityEventSubmitAfter = activityObj.ActivityEventSubmitAfter;
    focusNode.activityEventBackBefore = activityObj.ActivityEventBackBefore;
    focusNode.activityEventBackAfter = activityObj.ActivityEventBackAfter;
};

//重新这是Line信息
function resetLine() {
    var lineObj = art.dialog.data("lineObj"),
        line = workflow.getItemInfo(lineObj.LineId, "line");
    workflow.setName(lineObj.LineId, lineObj.LineName, "line");
    line.lineType = lineObj.LineType;
    line.lineRemark = lineObj.LineRemark;
    //如果是没有类型
    switch (lineObj.LineType) {
        case globalLineType.Conditions: //条件连线
            line.lineConditions = lineObj.LineConditions;
            lineObj.lineReturnPolicy = null;
            break;
        case globalLineType.Return: //退回连线
            line.lineReturnPolicy = lineObj.LineReturnPolicy;
            lineObj.lineConditions = null;
            break;
        default:
            lineObj.lineReturnPolicy = null;
            lineObj.lineConditions = null;
            break;
    }
}

//根据模版Id获取模版Json数据
function getTemplateData() {
    var json = JSON.parse($("#designJson").text());
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
    //从新赋予标题
    workflow.setTitle(UtilGetUrlParam("name"));
}

//初始化
$(document).ready(function () {
    //初始化流程
    initFlow();
    //初始化事件
    initEvent();
    //初始化右键
    initContextMenu();
    //获取流程信息
    getTemplateData();
});