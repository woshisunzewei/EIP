define([
    'list',
    'artTemplate',
    'layout',
    'ztree',
    'ztreeExcheck',
    'ligerMenu',
    'ligerTab', 'wdatepicker'],
    function (list, artTemplate) {
        template = artTemplate;
        initLayout();
        initGird();
        initTree();
        initTreeData();
        reloadTreeSpace();
        initTab();
        size = UtilWindowHeightWidth();
    });

var zNodes,
    treeObj,
    setting,
    $grid,
    size,
    template;

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
    $("#tree").height($("#uiWest").height() - 38).width($("#uiWest").width() - 10);
}

//初始化选项卡
function initTab() {
    $("#uiSouth").ligerTab({
        showSwitchInTab: true,
        showSwitch: true,
        contextmenu: false
    });
}

//初始化表格
function initGird() {
    $grid = $("#list").jgridview(
    {
        loadonce: false,
        multiselect: false,
        url: "/System/User/GetPagingUser",
        colModel: colModel,
        height: $("#listcenter").height() - 51,
        sortname: "u.CreateTime",
        rowList: [200, 500, 1000],
        onSelectRow: function (rowid) {
            getById(rowid);
        },
        loadComplete: function () {
            //选中第一条
            GridSetSelection($grid, 1);
            getById(1);
        }
    });
}

//获取详细信息
function getById(rowid) {
    var rowDatas = $grid.jqGrid('getRowData', rowid);
    if (typeof (rowDatas["UserId"]) != "undefined") {
        UtilAjaxPost("/System/User/GetDetailByUserId", { id: rowDatas["UserId"] },
            function (val) {
                $(".tableView").find('label').each(function () {
                    var $this = $(this), id = $this.attr('id');
                    (val[id] && typeof (val[id]) === 'string') && $this.text(val[id]);
                });
                //角色
                var roleHtml = template('role-template', val);
                $("#tabs-role").html(roleHtml);

                //岗位
                var postHtml = template('post-template', val);
                $("#tabs-post").html(postHtml);

                //组
                var groupHtml = template('group-template', val);
                $("#tabs-group").html(groupHtml);
            }
        );
    } else {
        $(".tableView label").html("");
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
    }
}

//获取表格数据
function getGridData() {
    //重新查询
    search();
}

//导出Excel
function reportExcel() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    var organizationId;
    if (treeNode.length === 0) {
        organizationId = Language.common.guidempty;
    } else {
        organizationId = treeNode[0].id;
    }
    var isFreeze = $("#SearchIsFreeze").val();
    var $form = $("<form method='post' action='/System/User/ExportExcel?" +
        "PrivilegeMasterValue=" + organizationId + "&CreateTime=" + $("#u.CreateTime").val() + "&Code=" + $("#u.Code").val() + "&Name=" + $("#u.Name").val() + "&IsFreeze=" + (isFreeze == "" ? null : (isFreeze == "0" ? false : true)) +
        "' enctype='multipart/form-data'></form>");
    $form.appendTo($("body")).submit().remove();
    // UtilAjaxPostWaitHiddenOverlay(
    //    "/System/User/ExportExcel",
    //    {
    //        PrivilegeMasterValue: organizationId,
    //        CreateTime: $("#u.CreateTime").val(),
    //        Code: $("#u.Code").val(),
    //        Name: $("#u.Name").val(),
    //        IsFreeze: isFreeze == "" ? null : (isFreeze == "0" ? false : true)
    //    },
    //    function (data) {

    //    }
    //);
}

//操作:新增
function add() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree")),orgId="",orgName="";
    if (treeNode.length!== 0) {
        orgId = treeNode[0].id;
        orgName = treeNode[0].name;
    }
    ArtDialogOpen("/System/User/Edit?orgId=" + orgId + "&orgName=" + orgName, "新增用户信息", true, 380, 580);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/User/Edit?userId=" + info.UserId + "&orgId=" + info.OrganizationId + "&orgName=" + info.OrganizationName, "修改用户信息-" + info.Name, true, 380, 580);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                "/System/User/DeleteUser",
                { id: GridGetSingSelectData($grid).UserId },
                perateStatus
            );
        });
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

//树点击触发
function onClickTree(e, treeId, treeNode) {
    resetOrgId(treeNode.id);
    getGridData();
}

//重置组织机构
function resetOrgId(id) {
    $("#PrivilegeMasterValue").val(id);
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        getGridData();
    }
}

//加载页面数据
function reloadTreeAndGrid() {
    resetOrgId(null);
    initTreeData();
    getGridData();
}

//模块权限
function menuPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Menu?privilegeMasterValue=" + info.UserId + "&privilegeMaster=" + Language.privilegeMaster.user, "模块权限授权-" + info.Name, true, 390, 380);
    });
}

//模块按钮权限
function functionPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Function?privilegeMasterValue=" + info.UserId + "&privilegeMaster=" + Language.privilegeMaster.user, "模块按钮权限授权-" + info.Name, true, 490, 810);
    });
}

//字段权限
function fieldPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Field?privilegeMasterValue=" + info.UserId + "&privilegeMaster=" + Language.privilegeMaster.user, "字段权限授权-" + info.Name, true, 490, 810);
    });
}

//数据权限
function dataPermission() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Permission/Data?privilegeMasterValue=" + info.UserId + "&privilegeMaster=" + Language.privilegeMaster.user, "数据权限授权-" + info.Name, true, 490, 810);
    });
}

//用户角色
function userRole() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Role/Chosen?userId=" + info.UserId, "维护用户角色-" + info.Name, true, 450, 870);
    });
}

//重置密码
function resetPassword() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.system.resetPassword, function () {
            UtilAjaxPostWait(
                "/System/User/ResetPassword",
                { id: GridGetSingSelectData($grid).UserId },
                perateStatus
            );
        });
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