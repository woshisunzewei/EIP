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
            size: 29,
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
        url: "/System/Dictionary/GetDictionariesByParentId",
        colModel: [
            { name: "DictionaryId", index: "DictionaryId", hidden: true },
            { label: "代码", name: "Code", width: 300, fixed: true },
            { label: "名称", name: "Name", width: 150, fixed: true },
            { label: "值", name: "Value", width: 200, fixed: true },
            { label: "允许删除", formatter: "YesOrNo", name: "CanbeDelete", width: 60, align: "center", fixed: true },
            { label: "排序", align: "center", name: "OrderNo", width: 50, fixed: true, search: false, sorttype: "int" },
            { label: "备注", name: "Remark", width: 150 }
        ],
        height: $("#listcenter").height() - 51,
        rowNum: 200,
        postData: { id: Language.common.guidempty },
        rowList: [200, 500, 1000]
    });
}

//获取表格数据
function getGridData() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    var pId = treeNode.length === 0 ? Language.common.guidempty : treeNode[0].id;
    UtilAjaxPost("/System/Dictionary/GetDictionariesByParentId", { id: pId }, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//初始化树
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
}

//初始化树结构
function initTreeData() {
    UtilAjaxPost("/Dictionary/GetDictionaryTree", {}, function (data) {
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
function loadTreeAndGrid() {
    initTreeData();
    //获取选中前节点
    var dicObj = art.dialog.data('dicObj');
    var note = treeObj.getNodeByParam("id", dicObj.pId, null);
    //选中加载前节点
    treeObj.selectNode(note);
    treeObj.expandNode(note);
    getGridData();
}

//操作:新增
function add() {
    var pid = Language.common.guidempty;
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    if (treeNode.length !== 0) {
        pid = treeNode[0].id;
    }
    ArtDialogOpen("/System/Dictionary/Edit?parentId=" + pid, "新增字典", true, 330, 610);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Dictionary/Edit?dictionaryId=" + info.DictionaryId, "编辑字典-" + info.Name, true, 330, 610);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                "/System/Dictionary/DeleteDictionary",
                { id: GridGetSingSelectData($grid).DictionaryId },
                perateStatus
            );
        });
    });
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        //获取重新加载前选中树
        var breforeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
        initTreeData();
        //重新获取该选中树
        if (breforeNode.length !== 0) {
            //获取选中前节点
            var note = treeObj.getNodeByParam("id", breforeNode[0].id, null);
            //获取选中前节点
            treeObj.selectNode(note);
            treeObj.expandNode(note);
        }
        getGridData();
    }
}

//批量生成代码
function generatingCode() {
    ArtDialogConfirm("是否批量生成代码?生成的代码将覆盖以前代码,使用字典代码处可能有误", function () {
        UtilAjaxPostWait(
            "/System/Dictionary/GeneratingCode",
            {},
            perateStatus
        );
    });
}

//重新加载
function reload() {
    initTreeData();
    getGridData();
}

//折叠/展开
var expand = false;

function arrowin() {
    if (expand) {
        treeObj.expandAll(false);
        expand = false;
        $("#arrowin").html("展开").attr("class", "l-icon-arrow-out");
    } else {
        treeObj.expandAll(true);
        expand = true;
        $("#arrowin").html("折叠").attr("class", "l-icon-arrow-in");
    }
}