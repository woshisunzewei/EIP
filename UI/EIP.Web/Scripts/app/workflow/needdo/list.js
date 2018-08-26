define([
    'list',
    'layout',
    'ztree',
    'ztreeExcheck'
],
    function () {
        initLayout();
        initGird();
        initTreeData();
        reloadTreeSpace();
        size = UtilWindowHeightWidth();
    });

var zNodes,
    treeObj,
    $grid,
    size;

//初始化布局
function initLayout() {
    $("body").layout({
        "west": {
            size: 200,
            closable: true, //是否可伸缩
            resizable: true, //是否可调整大小
            sliderTip: "显示/隐藏侧边栏", //在某个Pane隐藏后，当鼠标移到边框上显示的提示语。
            togglerTip_open: "关闭", //pane打开时，当鼠标移动到边框上按钮上，显示的提示语
            togglerTip_closed: "打开", //pane关闭时，当鼠标移动到边框上按钮上，显示的提示语
            resizerTip: "可调整大小"
        },
        "center": {
            onresize_end: function () {
                reloadTreeSpace();
                GridSetWidthAndHeight($grid, $("#uiCenter").width(), size.WinH - 84);
            }
        }
    });

    $("#layout").layout({
        "north": {
            size: 59,
            closable: true,
            resizable: true,
            sliderTip: "显示/隐藏侧边栏",
            togglerTip_open: "关闭",
            togglerTip_closed: "打开",
            resizerTip: "上下拖动可调整大小"
        },
        "center": {
            onresize_end: function () {
                reloadTreeSpace();
                //获取调整后高度
                $grid.jqGrid("setGridHeight", $("#listcenter").height() - 51).jqGrid("setGridWidth", $("#listcenter").width() - 2);
            }
        }
    });
}

//初始化表格
function initGird() {
    $grid = $("#list").jgridview(
    {
        url: "/Workflow/NeedDo/GetWorkflowEngineNeedDoTaskOutput",
        shrinkToFit: true,
        multiselect: false,
        colModel: [
            { name: "TaskId", hidden: true },
            { name: "CurrentActivityId", hidden: true },
            { name: "ProcessInstanceId", hidden: true },
            { name: "ProcessId", hidden: true },
            { label: "紧急程度", name: "Urgency", width: 50, fixed: true, align: 'center', formatter: 'workflowUrgency' },
            { label: "请求标题", name: "Title", width: 200, fixed: true },
            { label: "流程类型", name: "ProcessName", width: 100, fixed: true },
            { label: "我经办的步骤(流程图)", name: "CurrentActivityName", width: 150, fixed: true },
            { label: "发起人", name: "SendUserName", width: 60, fixed: true },
            { label: "发起时间", name: "SendTime", align: "center", fixed: true },
            { label: "已停留", name: "StayTime", align: "center", fixed: true }
        ],
        height: $("#listcenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    var pId;
    if (treeNode.length === 0) {
        pId = Language.common.guidempty;
    } else {
        pId = treeNode[0].id;
    }
    UtilAjaxPost("/Workflow/NeedDo/GetWorkflowEngineNeedDoTaskOutput", { processId: pId }, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//初始化辅助资料类别
function initTree() {
    //配置
    var setting = {
        view: {
            dblClickExpand: false,
            showLine: true
        },
        data: {
            simpleData: {
                enable: true
            }
        },
        callback: {
            onClick: onClickTree
        }
    };
    treeObj = $.fn.zTree.init($("#tree"), setting, zNodes);
    treeObj.expandAll(true);
}

//初始化树结构
function initTreeData() {
    UtilAjaxPost("/Common/Global/GetAllWorkflow", {}, function (data) {
        zNodes = data;
        initTree();
    });
}

//重新计算树高度宽度
function reloadTreeSpace() {
    $("#tree").height($("#uiWest").height() - 38).width($("#uiWest").width() - 10);
}

//树点击触发
function onClickTree(e, treeId, treeNode) {
    //加载数据
    getGridData();
}

//加载树和列表数据
function reloadTreeAndGrid() {
    initTreeData();
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        loadTreeAndGrid();
    }
}

//请求完成后使用
function loadTreeAndGrid() {
    //重新加载数数据
    initTreeData();
    getGridData();
}

//流程图
function flowTrackForMap() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        //打开流程图界面
        ArtDialogOpen("/Workflow/Run/TrackForMap?processId=" + info.ProcessId + "&processInstanceId=" + info.ProcessInstanceId, "查看流程图", true, 600, 1000);
    });
}

//处理流程
function doWorkflow() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        //打开流程图界面
        ArtDialogOpen("/Workflow/Run/DealWith?processInstanceId=" + info.ProcessInstanceId + "&currentTaskId=" + info.TaskId + "&processId=" + info.ProcessId + "&currentActivityName=" + info.CurrentActivityName + "&currentActivityId=" + info.CurrentActivityId, info.Title + "【" + info.CurrentActivityName + "】", true, "96%", "96%");
    });
}
