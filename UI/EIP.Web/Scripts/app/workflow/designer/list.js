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
            resizerTip: "可调整大小", //鼠标移到边框时，提示语
            slidable: true
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
            resizable: false,
            sliderTip: "显示/隐藏侧边栏",
            togglerTip_open: "关闭",
            togglerTip_closed: "打开",
            resizerTip: "上下拖动可调整大小", //鼠标移到边框时，提示语
            slidable: true
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
        shrinkToFit: true,
        multiselect: false,
        url: "/Workflow/Designer/GetWorkflowByProcessType",
        colModel: [
            { name: "ProcessId", hidden: true },
            { label: "代码", name: "Code", width: 150, fixed: true },
            { label: "名称", name: "Name", width: 250 },
            { label: "版本号", name: "Version", width: 60, fixed: true },
            //{ label: "状态", name: "CanbeDelete", width: 60, align: "center", fixed: true },
            { label: "冻结", name: "IsFreeze", formatter: "YesOrNo", width: 50, align: "center", fixed: true },
            { label: "添加人员", name: "CreateUserName", width: 60, fixed: true },
            { label: "添加时间", name: "CreateTime", width: 120, fixed: true, formatter: 'date', align: "center", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i' } },
            { label: "修改人员", name: "UpdateUserName", width: 60, fixed: true },
            { label: "修改时间", name: "UpdateTime", width: 120, fixed: true, formatter: 'date', align: "center", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i' } },
            { label: "排序", name: "OrderNo", align: "center", width: 50, fixed: true, sorttype: "int" }
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
    UtilAjaxPost("/Workflow/Designer/GetWorkflowByProcessType", { id: pId }, function (data) {
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
    UtilAjaxPostAsync("/Common/UserControl/GetDictionaryTreeByCode", { Code: Language.dictionary.流程类别 }, function (data) {
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

//操作:新增
function add() {
    ArtDialogOpen("/Workflow/Designer/Edit", "新增流程", true, 400, 600);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Workflow/Designer/Edit?id=" + info.ProcessId, "编辑流程-" + info.Name, true, 400, 600);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                 "/Workflow/Designer/Delete",
                 { id: GridGetSingSelectData($grid).ProcessId },
                 perateStatus
             );
        });
    });
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

//是否选中
function design() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Workflow/Designer/Gooflow?id=" + info.ProcessId + "&name=" + info.Name, "流程设计-" + info.Name, true, 600, 1000);
    });
}

//预览流程
function preview() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Workflow/Designer/GooflowPreview?id=" + info.ProcessId, "流程预览-" + info.Name, true, 600, 1000);
    });
}
