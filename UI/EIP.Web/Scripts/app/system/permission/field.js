define([
    "edit",
    "layout",
    "ztree",
    "ztreeExcheck", "list"
], function() {
    initLayout();
    initFieldTreeData();
    initFieldGrid();
    $(".messagebox").show();
    $("#div_field").hide();
});

var fieldZNodes, //模块按钮节点
    fieldTreeObj, //模块按钮树对象
    fieldSetting, //功能性树设置
    $grid; //字段权限列表

//初始化菜单
function initFieldMenuTree() {
    //配置
    fieldSetting = {
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
            onClick: function(e, treeId, treeNode) {
                $("#menuname").html(treeNode.name);
                $(".messagebox").hide();
                $("#div_field").show();
                //加载数据
                getFieldGridData(treeNode);

            }
        }
    };
    fieldTreeObj = $.fn.zTree.init($("#fieldTree"),
        fieldSetting, fieldZNodes);
}

//初始化树结构
function initFieldTreeData() {
    UtilAjaxPost("/System/Permission/GetMenuHavePermissionByPrivilegeMasterValue", {
        privilegeMasterValue: UtilGetUrlParam("privilegeMasterValue"),
        privilegeMaster: UtilGetUrlParam("privilegeMaster"),
        PrivilegeAccess: Language.privilegeAccess.data //字段
    }, function(data) {
        fieldZNodes = data;
        initFieldMenuTree();
    });
}

//初始化字段列表
function initFieldGrid() {
    $grid = $("#fieldList").jgridview(
    {
        multiselect: true,
        loadonce: false,
        datatype: "json",
        url: "/System/Field/GetFieldByMenuId",
        colModel: [
            { name: "FieldId", hidden: true },
            { label: "显示名称", name: "Label", width: 130, fixed: true },
            //{ label: "字段名称", name: "Name", index: "field.Name", width: 100, fixed: true },
            //{ label: "排序名称", name: "Index", index: "[Index]", width: 100, fixed: true },
            { label: "显示", name: "Hidden", width: 50, fixed: true, formatter: "YesOrNo" },
            { label: "序号", name: "OrderNo", width: 50, fixed: true },
            { label: "备注", name: "Remark", index: "field.Remark", width: 268, fixed: true }
        ],
        height: 343,
        width: 580,
        postData: { IsShowHidden: false },
        sortname: "menu.Name,field.OrderNo",
        sortorder: "asc",
        pager: "fieldPager",
        rowNum: 200,
        rowList: [200, 500, 1000],
        loadComplete: function() {
            var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("fieldTree"));
            var privilegeMenuId = null;
            if (treeNode.length !== 0) {
                privilegeMenuId = treeNode[0].id;
                //根据角色获取具有的字段权限信息
                UtilAjaxPost("/System/Permission/GetPermissionByPrivilegeMasterValue", {
                    privilegeMasterValue: UtilGetUrlParam("privilegeMasterValue"),
                    privilegeMaster: UtilGetUrlParam("privilegeMaster"),
                    PrivilegeAccess: Language.privilegeAccess.field, //字段
                    PrivilegeMenuId: privilegeMenuId
                }, function(data) {
                    var rowData = $grid.jqGrid("getRowData");
                    for (var i = 0; i < data.length; i++) {
                        if (rowData.length) {
                            for (var j = 1; j <= rowData.length; j++) {
                                var fieldId = $grid.jqGrid("getCell", j, "FieldId");
                                if (fieldId === data[i].PrivilegeAccessValue) {
                                    GridSetSelection($grid, j);
                                    break;
                                }
                            }
                        }
                    }
                });
            }
        }
    });
}

//获取表格数据
function getFieldGridData(treeNode) {
    GridReloadPagingData($grid, {
        postData: {
            MenuId: treeNode.id
        }
    });
}

//初始化布局
function initLayout() {
    $("#layout").layout({
        "west": {
            size: "210",
            closable: true,
            resizable: false,
            sliderTip: "显示/隐藏侧边栏", //在某个Pane隐藏后，当鼠标移到边框上显示的提示语。
            togglerTip_open: "关闭", //pane打开时，当鼠标移动到边框上按钮上，显示的提示语
            togglerTip_closed: "打开", //pane关闭时，当鼠标移动到边框上按钮上，显示的提示语
            resizerTip: "可调整大小", //鼠标移到边框时，提示语
            slidable: false
        },
        "center": {
            onresize_end: function() {

            }
        }
    });
}

//表单提交
function formSubmit() {
    DialogTipsMsgWait("正在处理中");
    //获取所有选中行Id
    var json = "";
    var rowData = $grid.jqGrid("getGridParam", "selarrrow");
    if (rowData.length) {
        for (var i = 0; i < rowData.length; i++) {
            var fieldId = $grid.jqGrid("getCell", rowData[i], "FieldId"); //name是colModel中的一属性
            json += "{\"P\":\"" + fieldId + "\"},";
        }
    }
    json = json.substring(0, json.length - 1);
    json = "[" + json + "]";
    //获取选中树信息
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("fieldTree"));
    var privilegeMenuId = null;
    if (treeNode.length !== 0) {
        privilegeMenuId = treeNode[0].id;
    }
    UtilAjaxPostWait("/System/Permission/SavePermission", {
        privilegeAccess: Language.privilegeAccess.field, //模块按钮
        privilegeMaster: UtilGetUrlParam("privilegeMaster"), //类型
        privilegeMasterValue: UtilGetUrlParam("privilegeMasterValue"), //类型Id
        privilegeMenuId: privilegeMenuId,
        menuPermissions: json //权限json字符串
    }, function(data) {
        if (UtilEditIsdialogClose()) {
            DialogAjaxResult(data);
            DialogCloseTipsMsgWait();
        }
        else {
            art.dialog.close();
        }
    });
}

//折叠/展开
var expand = false;

function arrowin() {
    if (expand) {
        fieldTreeObj.expandAll(false);
        expand = false;
        $("#arrowin").html("展开").attr("class", "l-icon-arrow-out");
    } else {
        fieldTreeObj.expandAll(true);
        expand = true;
        $("#arrowin").html("折叠").attr("class", "l-icon-arrow-in");
    }
}