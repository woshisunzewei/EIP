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
        "west": {size: 200},
        "center": {
            onresize_end: function () {
                reloadTreeSpace();
                GridSetWidthAndHeight($grid, $("#uiCenter").width(), size.WinH - 84);
            }
        }
    });

    $("#layout").layout({
        "north": {size: 59},
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
        url: "/Workflow/HaveSend/GetWorkflowEngineHaveSendOutput",
        shrinkToFit: true,
        multiselect: false,
        colModel: [
            { name: "ProcessInstanceId", hidden: true },
            { name: "ProcessId", hidden: true },
            { label: "紧急程度", name: "Urgency", width: 50, fixed: true, align: 'center', formatter: 'workflowUrgency' },
            { label: "请求标题", name: "Title", width: 200, fixed: true },
            { label: "流程类型", name: "Name", width: 250, fixed: true },
            { label: "发起时间", name: "CreateTime", width: 100, fixed: true },
            { label: "结束时间", name: "EndTime", width: 100, fixed: true },
            { label: "结束者", name: "EndUserName", width: 100, fixed: true },
            { label: "结束者归属机构", name: "EndUserOrganization", width: 100, fixed: true }
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
    UtilAjaxPost("/Workflow/HaveSend/GetWorkflowEngineHaveSendOutput", { id: pId }, function (data) {
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
