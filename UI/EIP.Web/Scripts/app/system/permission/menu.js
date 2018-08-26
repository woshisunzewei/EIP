define([
    "edit",
    "ztree",
    "ztreeExcheck"
], function() {
    initMenuTreeData();
});

var menuZNodes, //菜单树
    menuTreeObj, //菜单树对象
    menuSetting; //菜单设置

//初始化菜单
function initMenuTree() {
    //配置
    menuSetting = {
        check: {
            enable: true
        },
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
                if (treeNode.isParent) {
                    var zTree = $.fn.zTree.getZTreeObj("menuTree");
                    zTree.expandNode(treeNode);
                }
            }
        }
    };
    menuTreeObj = $.fn.zTree.init($("#menuTree"), menuSetting, menuZNodes);
}

//初始化树结构
function initMenuTreeData() {
    UtilAjaxPost("/System/Menu/GetHaveMenuPermissionMenu", null, function(data) {
        menuZNodes = data;
        initMenuTree();
        initRoleMenuTreeData();

    });
}

//根据Id绑定已有权限信息
function initRoleMenuTreeData() {
    var treeObj = $.fn.zTree.getZTreeObj("menuTree");
    //取消选中父节点之后选中子节点
    treeObj.setting.check.chkboxType = { "Y": "p", "N": "ps" };
    UtilAjaxPost("/System/Permission/GetPermissionByPrivilegeMasterValue", {
        privilegeMasterValue: UtilGetUrlParam("privilegeMasterValue"),
        privilegeMaster: UtilGetUrlParam("privilegeMaster"),
        PrivilegeAccess: Language.privilegeAccess.menu //模块按钮
    }, function(data) {
        var zTree = $.fn.zTree.getZTreeObj("menuTree");
        $.each(data, function(i, item) {
            ZtreeAssignCheck(zTree, item.PrivilegeAccessValue);
        });
        //勾选父节点后子节点也选中
        treeObj.setting.check.chkboxType = { "Y": "ps", "N": "ps" };
    });

}

//折叠/展开
var expand = false;

function arrowin() {
    if (expand) {
        menuTreeObj.expandAll(false);
        expand = false;
        $("#arrowin").html("展开").attr("class", "l-icon-arrow-out");
    } else {
        menuTreeObj.expandAll(true);
        expand = true;
        $("#arrowin").html("折叠").attr("class", "l-icon-arrow-in");
    }
}

//全选/重置
var checkAll = true;

function selectall() {
    if (checkAll) {
        menuTreeObj.checkAllNodes(true);
        checkAll = false;
        $("#select").html("反选").attr("class", "l-icon-ui-check-box-uncheck");
    } else {
        menuTreeObj.checkAllNodes(false);
        checkAll = true;
        $("#select").html("全选").attr("class", "l-icon-ui-check-box");
    }
}

//表单提交
function formSubmit() {
    DialogTipsMsgWait("正在处理中");
    var json = "";
    var nodes = menuTreeObj.getCheckedNodes(true);
    for (var i = 0; i < nodes.length; i++) {
        var info = nodes[i].id;
        json += "{\"P\":\"" + info + "\"},";
    };
    json = json.substring(0, json.length - 1);
    json = "[" + json + "]";

    UtilAjaxPostWait("/System/Permission/SavePermission",
    {
        privilegeAccess: Language.privilegeAccess.menu, //菜单
        menuPermissions: json,
        privilegeMasterValue: UtilGetUrlParam("privilegeMasterValue"),
        privilegeMaster: UtilGetUrlParam("privilegeMaster")
    }, function (data) {
        if (UtilEditIsdialogClose()) {
            DialogAjaxResult(data);
            DialogCloseTipsMsgWait();
        }
        else {
            art.dialog.close();
        }
    });
}