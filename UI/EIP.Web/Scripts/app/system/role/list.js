define([
    'list',
    'artTemplate',
    'layout',
    'ztree',
    'ztreeExcheck',
    'ligerMenu',
    'ligerTab'],
    function (list, artTemplate) {
        template = artTemplate;
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
                url: "/System/Role/GetRolesByOrganization",
                multiselect: false,
                colModel: [
                    { name: "RoleId", hidden: true },
                    { label: "名称", name: "Name", width: 100, fixed: true },
                    { label: "归属组织", name: "OrganizationName", width: 150, fixed: true },
                    { label: "冻结", name: "IsFreeze", width: 50, fixed: true, formatter: 'YesOrNo' },
                    { label: "允许删除", name: "CanbeDelete", width: 50, fixed: true, formatter: 'YesOrNo' },
                    //{ label: "创建时间", name: "CreateTime", index: "u.CreateTime", width: 120, fixed: true },
                    //{ label: "创建人员", name: "CreateUserName", width: 60, align: "center", fixed: true },
                    //{ label: "最后修改时间", name: "UpdateTime", width: 120, align: "center", fixed: true },
                    //{ label: "最后修改人员", name: "UpdateUserName", width: 60, align: "center", fixed: true },
                    { label: "排序", name: "OrderNo", width: 50, fixed: true, align: "center" },
                    { label: "备注", name: "Remark", width: 200, fixed: true }
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
    UtilAjaxPost("/System/Role/GetRolesByOrganization", { Id: pId },
                function (data) {
                    GridReloadLoadOnceData($grid, data);
                });
}

//操作:新增
function add() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree")), organizationId = "", organizationName = "";
    if (treeNode.length !== 0) {
        organizationId = treeNode[0].id;
        organizationName = treeNode[0].name;
    }
    ArtDialogOpen("/System/Role/Edit?organizationId=" + organizationId + "&organizationId=" + organizationName, "新增角色", true, 300, 590);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Role/Edit?roleId=" + info.RoleId, "修改角色-" + info.Name, true, 300, 590);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                 "/System/Role/DeleteRole",
                { id: GridGetSingSelectData($grid).RoleId },
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

//模块权限
function menuPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Menu?privilegeMasterValue=" + info.RoleId + "&privilegeMaster=" + Language.privilegeMaster.role, "模块权限授权-" + info.Name, true, 390, 380);
    });
}

//模块按钮权限
function functionPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Function?privilegeMasterValue=" + info.RoleId + "&privilegeMaster=" + Language.privilegeMaster.role, "模块按钮权限授权-" + info.Name, true, 490, 810);
    });
}

//字段权限
function fieldPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Field?privilegeMasterValue=" + info.RoleId + "&privilegeMaster=" + Language.privilegeMaster.role, "字段权限授权-" + info.Name, true, 490, 810);
    });
}

//数据权限
function dataPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Data?privilegeMasterValue=" + info.RoleId + "&privilegeMaster=" + Language.privilegeMaster.role, "数据权限授权-" + info.Name, true, 490, 810);
    });
}

//角色用户
function roleUser() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Common/UserControl/PrivilegeMasterUser?" +
            "privilegeMaster=" + Language.privilegeMaster.role + "&" +
            "privilegeMasterValue=" + info.RoleId,
            "维护角色用户-" + info.Name, true, 450, 830);
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