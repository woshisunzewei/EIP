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
        $menuId = $("#menuId");
        initTreeData();
        initLayout();
        initGird();
        initTab();
        reloadTreeSpace();
        size = UtilWindowHeightWidth();
    });

var zNodes,
          treeObj,
          setting,
          $grid,
          $menuId,
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
        "south": {
            size: 225,
            closable: true,
            resizable: false,
            sliderTip: "显示/隐藏侧边栏", //在某个Pane隐藏后，当鼠标移到边框上显示的提示语。
            togglerTip_open: "关闭", //pane打开时，当鼠标移动到边框上按钮上，显示的提示语
            togglerTip_closed: "打开", //pane关闭时，当鼠标移动到边框上按钮上，显示的提示语
            resizerTip: "上下拖动可调整大小", //鼠标移到边框时，提示语
            slidable: false
        },
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

//重新加载树高度
function reloadTreeSpace() {
    $("#tree").height($("#uiWest").height() - 38).width($("#uiWest").width() - 10);
}

//初始化表格
function initGird() {
    $grid = $("#list").jgridview(
    {
        datatype: "json",
        url: "/System/Data/GetDataByMenuId",
        colModel: [
            { name: "DataId", hidden: true },
            { label: "名称", name: "Name", width: 200, fixed: true },
            { label: "归属", name: "MenuName", index: "menu.Name", width: 100, fixed: true },
            { label: "排序", name: "OrderNo", align: "center", width: 50, fixed: true },
            { label: "冻结", name: "IsFreeze", formatter: "YesOrNo", width: 50, align: "center", fixed: true },
            { label: "备注", name: "Remark", width: 350, fixed: true }
        ],
        height: $("#listcenter").height() - 51,
        sortname: "OrderNo",
        sortorder: "asc",
        onSelectRow: function (rowid) {
            var rowDatas = $grid.jqGrid('getRowData', rowid);
            getById(Language.privilegeAccess.data, rowDatas.DataId);
        },
        loadComplete: function () {
            //选中第一条
            GridSetSelection($grid, 1);
            var rowDatas = $grid.jqGrid('getRowData', 1);
            getById(Language.privilegeAccess.data, rowDatas.DataId);
        }
    });
}

function getById(access, id) {
    if (id != null && id != "") {
        UtilAjaxPost("/Common/Global/GetSystemPrivilegeDetailOutputsByAccessAndValue", { id: id, access: access },
            function (val) {
                //角色
                var roleHtml = template('role-template', val);
                $("#tabs-role").html(roleHtml);

                //岗位
                var postHtml = template('post-template', val);
                $("#tabs-post").html(postHtml);

                //组
                var groupHtml = template('group-template', val);
                $("#tabs-group").html(groupHtml);

                //组织机构
                var organizationHtml = template('organization-template', val);
                $("#tabs-organization").html(organizationHtml);

                //人员
                var userHtml = template('user-template', val);
                $("#tabs-user").html(userHtml);
            }
        );
    } else {

        var val = [];
        //角色
        var roleHtml = template('role-template', val);
        $("#tabs-role").html(roleHtml);

        //岗位
        var postHtml = template('post-template', val);
        $("#tabs-post").html(postHtml);

        //组
        var groupHtml = template('group-template', val);
        $("#tabs-group").html(groupHtml);

        //组织机构
        var organizationHtml = template('organization-template', val);
        $("#tabs-organization").html(organizationHtml);

        //人员
        var userHtml = template('user-template', val);
        $("#tabs-user").html(userHtml);
    }
}

//初始化菜单
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
        callback: {
            onClick: onClickTree
        }
    };
    treeObj = $.fn.zTree.init($("#tree"), setting, zNodes);
}

//初始化树结构
function initTreeData() {
    UtilAjaxPost("/System/Menu/GetHaveDataPermissionMenu", null, function (data) {
        zNodes = data;
        initTree();
    });
}

//获取表格数据
function getGridData() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    var pId = null;
    if (treeNode.length !== 0) {
        pId = treeNode[0].id;
    }
    UtilAjaxPost("/System/Data/GetDataByMenuId", { id: pId }, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//重新加载
function reloadTreeAndGrid() {
    initTreeData();
    getGridData();
}

//操作:新增
function add() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    var addUrl = "/System/Data/Edit";
    if (treeNode.length !== 0) {
        addUrl += "?menuId=" + treeNode[0].id + "&menuName=" + treeNode[0].name;
    }
    ArtDialogOpen(addUrl, "新增数据权限规则", true, 460, 780);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Data/Edit?id=" + info.DataId, "修改数据权限规则-" + info.Name, true, 460, 780);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                "/System/Data/DeleteByDataId",
                { id: GridGetSingSelectData($grid).DataId },
                perateStatus
            );
        });
    });
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        getGridData();
    }
}

//树点击触发
function onClickTree(e, treeId, treeNode) {
    //加载数据
    getGridData();
    //赋值
    $menuId.val(treeNode.id);
}

//初始化选项卡
function initTab() {
    $("#uiSouth").ligerTab({
        showSwitchInTab: true,
        showSwitch: true,
        contextmenu: false
    });
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