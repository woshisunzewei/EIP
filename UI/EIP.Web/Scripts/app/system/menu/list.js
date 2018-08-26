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
        initTreeData();
        reloadTreeSpace();
        initTab();
        size = UtilWindowHeightWidth();
    });

var zNodes, treeObj, $grid, size, template;
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

//初始化表格
function initGird() {
    $grid = $("#list").jgridview(
    {
        shrinkToFit: true,
        multiselect: false,
        url: "/System/Menu/GetMeunuByPId",
        colModel: [
            { name: "MenuId", index: "MenuId", hidden: true },
            { label: "代码", name: "Code", width: 200, fixed: true },
            { label: "名称", name: "Name", width: 100, fixed: true },
            { label: "图标", name: "Icon", width: 30, align: "center", fixed: true, formatter: "icon", search: false },
            { label: "地址", name: "OpenUrl", width: 200, fixed: true },
            {
                label: "允许删除",
                name: "CanbeDelete",
                width: 50,
                align: "center",
                fixed: true,
                formatter: "YesOrNo"
            },
            {
                 label: "模块显示",
                 name: "IsShowMenu",
                 width: 50,
                 align: "center",
                 fixed: true,
                 formatter: "YesOrNo"
            },
            {
                label: "模块权限",
                name: "HaveMenuPermission",
                width: 50,
                align: "center",
                fixed: true,
                formatter: "YesOrNo"
            },
             {
                 label: "按钮权限",
                 name: "HaveFunctionPermission",
                 width: 50,
                 align: "center",
                 fixed: true,
                 formatter: "YesOrNo"
             },
            {
                label: "字段权限",
                name: "HaveFieldPermission",
                width: 50,
                align: "center",
                fixed: true,
                formatter: "YesOrNo"
            },
             {
                 label: "数据权限",
                 name: "HaveDataPermission",
                 width: 50,
                 align: "center",
                 fixed: true,
                 formatter: "YesOrNo"
             },
            {
                label: "冻结",
                name: "IsFreeze",
                width: 30,
                align: "center",
                fixed: true,
                formatter: "YesOrNo"
            },
           
            { label: "排序号", align: "center", name: "OrderNo", width: 50, fixed: true, search: false },
            { label: "备注", name: "Remark", width: 150 }
        ],
        height: $("#listcenter").height() - 51,
        postData: { pId: Language.common.guidempty },
        rowList: [200, 500, 1000],
        onSelectRow: function (rowid) {
            var rowDatas = $grid.jqGrid('getRowData', rowid);
            getById(Language.privilegeAccess.menu, rowDatas.MenuId);
        },
        loadComplete: function () {
            //选中第一条
            GridSetSelection($grid, 1);
            var rowDatas = $grid.jqGrid('getRowData', 1);
            getById(Language.privilegeAccess.menu, rowDatas.MenuId);
        }
    });
}

//获取详细信息
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

//获取表格数据
function getGridData() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    var pId;
    if (treeNode.length === 0) {
        pId = Language.common.guidempty;
    } else {
        pId = treeNode[0].id;
    }
    UtilAjaxPost("/System/Menu/GetMeunuByPId", { id: pId }, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//初始化模块
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
    UtilAjaxPost("/System/Menu/GetAllMenu", null, function (data) {
        zNodes = data;
        initTree();
    });
}

//重新加载树高度
function reloadTreeSpace() {
    $("#tree").height($("#uiWest").height() - 38).width($("#uiWest").width() - 10);
}

//树点击触发
function onClickTree(e, treeId, treeNode) {
    $("#MenuId").val(treeNode.id);
    //加载数据
    getGridData();
}

//刷新
function reload() {
    initTreeData();
    getGridData();
}

//操作:新增
function add() {
    var pid = "";
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    if (treeNode.length !== 0) {
        pid = treeNode[0].id;
    }
    ArtDialogOpen("/System/Menu/Edit?parentId=" + pid, "新增模块", true, 440, 600);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Menu/Edit?id=" + info.MenuId, "修改模块-" + info.Name, true, 440, 600);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm("<lable style='color:red;font-size:14px'>删除前请确认该菜单权限使用者已取消该菜单?</lable><br/>删除后不可恢复,确定删除?", function () {
            UtilAjaxPostWait(
                 "/System/Menu/DeleteMenu",
                 { id: GridGetSingSelectData($grid).MenuId },
                 perateStatus
             );
        });
    });
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
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

//请求完成后使用
function loadTreeAndGrid() {
    //重新加载数数据
    initTreeData();
    //获取选中前节点
    var menuObj = art.dialog.data('menuObj');
    var note = treeObj.getNodeByParam("id", menuObj.pId, null);
    //选中加载前节点
    treeObj.selectNode(note);
    treeObj.expandNode(note,true,false);
    //加载列表数据
    getGridData();
}

//初始化选项卡
function initTab() {
    $("#uiSouth").ligerTab({
        showSwitchInTab: true,
        showSwitch: true,
        contextmenu: false
    });
}

//批量生成代码
function generatingCode() {
    ArtDialogConfirm("是否批量生成代码?生成的代码将覆盖以前代码,使用模块代码处可能有误", function () {
        UtilAjaxPostWait(
            "/System/Menu/GeneratingCode",
            {},
            perateStatus
        );
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