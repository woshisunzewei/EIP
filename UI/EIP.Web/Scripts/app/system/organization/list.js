define([
    'list',
    'layout',
    'ztree',
    'ztreeExcheck'
],
    function (list) {
        initLayout();
        initGird();
        initTree();
        initTreeData();
        reloadTreeSpace();
        size = UtilWindowHeightWidth();
    });

var zNodes,
   treeObj,
   setting,
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

//重新计算树高度宽度
function reloadTreeSpace() {
    $("#tree").height($("#uiWest").height() - 56).width($("#uiWest").width() - 10);
}

//初始化表格
function initGird() {
    $grid = $("#list").jgridview(
    {
        multiselect: false,
        url: "/System/Organization/GetOrganizationResultByTreeId",
        colModel: [
            { name: "OrganizationId", index: "OrganizationId", hidden: true },
            { label: "代码", name: "Code", width: 100, fixed: true },
            { label: "名称", name: "Name", width: 150, fixed: true },
            { label: "简称", name: "ShortName", width: 100, fixed: true },
            { label: "性质", name: "NatureName", width: 60, fixed: true },
            { label: "主负责人", name: "MainSupervisor", width: 100, fixed: true },
            { label: "联系方式", name: "MainSupervisorContact", width: 100, fixed: true },
            { label: "冻结", name: "IsFreeze", width: 50, align: "center", fixed: true, formatter: "YesOrNo" },
            { label: "排序", name: "OrderNo", width: 50, align: "center", fixed: true, search: false },
            //{ label: "创建时间", name: "CreateTime", index: "u.CreateTime", width: 120, fixed: true },
            //{ label: "创建人员", name: "CreateUserName", width: 60, align: "center", fixed: true },
            //{ label: "最后修改时间", name: "UpdateTime", width: 120, align: "center", fixed: true },
            //{ label: "最后修改人员", name: "UpdateUserName", width: 60, align: "center", fixed: true },
            { label: "备注", name: "Remark", width: 200, fixed: true }
        ],
        height: $("#listcenter").height() - 51,
        postData: { id: Language.common.guidempty },
        rowList: [200, 500, 1000]
    });
}

//初始化组织机构
function initTree() {
    //配置
    setting = {
        view: {
            dblClickExpand: false,
            showLine: true
        },
        data: {
            simpleData: {
                enable: true
            }
        },
        expandSpeed: "",
        callback: {
            onClick: onClickTree
        }
    };
}

//初始化树结构:同步
function initTreeData() {
    UtilAjaxPost("/System/Organization/GetOrganizationTree", null, function (data) {
        zNodes = data;
        treeObj = $.fn.zTree.init($("#tree"), setting, zNodes);
        treeObj.expandAll(true);
    });
}

//获取表格数据
function getGridData() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    var pId = treeNode.length === 0 ? Language.common.guidempty : treeNode[0].id;
    UtilAjaxPost(
                "/System/Organization/GetOrganizationResultByTreeId",
                 { id: pId },
                function (data) {
                    GridReloadLoadOnceData($grid, data);
                }
            );
}

//操作:新增
function add() {
    var pid = Language.common.guidempty;
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    if (treeNode.length !== 0) {
        pid = treeNode[0].id;
    }
    ArtDialogOpen("/System/Organization/Edit?parentId=" + pid, "新增组织机构", true, 310, 590);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Organization/Edit?id=" + info.OrganizationId, "修改组织机构-" + info.Name, true, 310, 590);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                  "/System/Organization/DeleteOrganization",
                 { id: GridGetSingSelectData($grid).OrganizationId },
                 perateStatus
             );
        });
    });
}

//树点击触发
function onClickTree(e, treeId, treeNode) {
    //加载数据
    getGridData();
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
        }
        getGridData();
    }
}

//刷新
function reload() {
    initTreeData();
    getGridData();
}

//加载页面数据
function loadTreeAndGrid() {
    //重新加载数数据
    initTreeData();
    //获取选中前节点
    var orgObj = art.dialog.data('orgObj');
    var note = treeObj.getNodeByParam("id", orgObj.pId, null);
    //选中加载前节点
    treeObj.selectNode(note);
    //加载列表数据
    getGridData();
}

//模块权限
function menuPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Menu?privilegeMasterValue=" + info.OrganizationId + "&privilegeMaster=" + Language.privilegeMaster.organization, "模块权限授权-" + info.Name, true, 390, 380);
    });
}

//模块按钮权限
function functionPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Function?privilegeMasterValue=" + info.OrganizationId + "&privilegeMaster=" + Language.privilegeMaster.organization, "模块按钮权限授权-" + info.Name, true, 490, 810);
    });
}

//字段权限
function fieldPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Field?privilegeMasterValue=" + info.OrganizationId + "&privilegeMaster=" + Language.privilegeMaster.organization, "字段权限授权-" + info.Name, true, 490, 810);
    });
}

//数据权限
function dataPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Data?privilegeMasterValue=" + info.OrganizationId + "&privilegeMaster=" + Language.privilegeMaster.organization, "数据权限授权-" + info.Name, true, 490, 810);
    });
}

//批量生成代码
function generatingCode() {
    ArtDialogConfirm("是否批量生成代码?生成的代码将覆盖以前代码,已使用机构处可能出现错误", function () {
        UtilAjaxPostWait(
            "/System/Organization/GeneratingCode",
            {},
            perateStatus
        );
    });
}

//折叠/展开
var expand = true;

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