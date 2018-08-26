define([
    'list',
    'layout',
    'ztree',
    'ztreeExcheck'],
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
        url: "/System/Post/GetPostByOrganizationId",
        multiselect: false,
        colModel: [
            { name: "PostId", hidden: true },
            { label: "名称", name: "Name", index: "PostName", width: 100, fixed: true },
            { label: "归属组织", name: "OrganizationName", width: 150, fixed: true },
            { label: "主联系人", name: "MainSupervisor", width: 50, fixed: true },
            { label: "联系方式", name: "MainSupervisorContact", width: 150, fixed: true },
            { label: "冻结", name: "IsFreeze", width: 50, align: "center", fixed: true, formatter: "YesOrNo" },
            { label: "排序", name: "OrderNo", index: "OrderNo", width: 50, align: "center", fixed: true, search: false },
            //{ label: "创建时间", name: "CreateTime", index: "u.CreateTime", width: 120, fixed: true },
            //{ label: "创建人员", name: "CreateUserName", width: 60, align: "center", fixed: true },
            //{ label: "最后修改时间", name: "UpdateTime", width: 120, align: "center", fixed: true },
            //{ label: "最后修改人员", name: "UpdateUserName", width: 60, align: "center", fixed: true },
            { label: "备注", name: "Remark", index: "Remark", width: 200, align: "center", fixed: true }
        ],
        postData: { Id: Language.common.guidempty },
        height: $("#listcenter").height() - 51
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
    UtilAjaxPostAsync("/System/Organization/GetOrganizationTree", null, function (data) {
        zNodes = data;
        treeObj = $.fn.zTree.init($("#tree"), setting, zNodes);
        treeObj.expandAll(true);
    });
}

//获取表格数据
function getGridData() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    var pId = Language.common.guidempty;
    if (treeNode.length !== 0) {
        pId = treeNode[0].id;
    }
    UtilAjaxPost("/System/Post/GetPostByOrganizationId", { Id: pId },
        function (data) {
            GridReloadLoadOnceData($grid, data);
        }
    );
}

//操作:新增
function add() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree")), organizationId = "", organizationName = "";
    if (treeNode.length !== 0) {
        organizationId = treeNode[0].id;
        organizationName = treeNode[0].name;
    }
    ArtDialogOpen("/System/Post/Edit?organizationId=" + organizationId + "&organizationId=" + organizationName, "新增岗位", true, 370, 590);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Post/Edit?postId=" + info.PostId, "编辑岗位-" + info.Name, true, 370, 590);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                "/System/Post/DeletePost",
                { id: GridGetSingSelectData($grid).PostId },
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
        getGridData();
    }
}

//加载页面数据
function loadTreeAndGrid() {
    initTree();
    initTreeData();
    getGridData();
}

//岗位用户
function postUser() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Common/UserControl/PrivilegeMasterUser?" +
            "privilegeMaster=" + Language.privilegeMaster.post + "&" +
            "privilegeMasterValue=" + info.PostId,
            "维护岗位用户-" + info.Name, true, 450, 830);
    });
}

//模块权限
function menuPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Menu?privilegeMasterValue=" + info.PostId + "&privilegeMaster=" + Language.privilegeMaster.post, "模块权限授权-" + info.Name, true, 390, 380);
    });
}

//模块按钮权限
function functionPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Function?privilegeMasterValue=" + info.PostId + "&privilegeMaster=" + Language.privilegeMaster.post, "模块按钮权限授权-" + info.Name, true, 490, 810);
    });
}

//字段权限
function fieldPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Field?privilegeMasterValue=" + info.PostId + "&privilegeMaster=" + Language.privilegeMaster.post, "字段权限授权-" + info.Name, true, 490, 810);
    });
}

//数据权限
function dataPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Data?privilegeMasterValue=" + info.PostId + "&privilegeMaster=" + Language.privilegeMaster.post, "数据权限授权-" + info.Name, true, 490, 810);
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