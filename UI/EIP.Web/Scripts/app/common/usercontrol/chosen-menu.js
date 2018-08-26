define([
    'edit',
    'ztree',
    'ztreeExcheck'], function () { initTreeData(); });

var zNodes, //菜单树
    treeObj, //菜单树对象
    setting; //菜单设置

//初始化菜单
function initTree() {
    //配置
    setting = {
        check: {
            enable: true,
            chkStyle: "radio",
            radioType: "all"
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
            onClick: function (e, treeId, treeNode) {
                treeObj.checkNode(treeNode, !treeNode.checked, true);
                if (treeNode.isParent) {
                    treeObj.expandNode(treeNode);
                }
            }
        }
    };
    treeObj = $.fn.zTree.init($("#tree"), setting, zNodes);
}

//初始化树结构
function initTreeData() {
    //判断是否具有筛选菜单条件
    var menuType = UtilGetUrlParam("menuType");
    var postUrl = "/Common/UserControl/GetMenuRemoveChildren";
    //判断类型
    switch (menuType) {
        case Language.menuType.haveMenuPermission.toString():
            break;
        case Language.menuType.haveDataPermission.toString():
            postUrl = "/System/Menu/GetHaveDataPermissionMenu";
            break;
        case Language.menuType.haveFieldPermission.toString():
            postUrl = "/System/Menu/GetHaveFieldPermissionMenu";
            break;
        case Language.menuType.haveFunctionPermission.toString():
            break;
        case Language.menuType.isFreeze.toString():
            break;
        case Language.menuType.isShowMenu.toString():
            break;
        default:
    }
    UtilAjaxPostAsync(postUrl, {
        menuId: UtilGetUrlParam("menuId"),
        isRemove: UtilGetUrlParam("isRemove")
    }, function (data) {
        zNodes = data;
        initTree();
        //绑定选中值
        var pId = UtilGetUrlParam("parentId");
        if (pId != "" && pId != null) {
            ZtreeAssignCheck(treeObj, pId);
        }
    });
}

//表单提交
function formSubmit() {
    var treeData = ZtreeGetCheckedNodes(treeObj);
    //if (treeData.length === 0) {
    //    DialogTipsMsgWarn("请选择需要操作的数据", 1000);
    //    return;
    //}
    //选中行数据
    art.dialog.data("menuObj", treeData);
    var win = artDialog.open.origin; //来源页面
    win.setMenu();
    art.dialog.close();
}

//查询
function search() {
    var p = $.trim($(".w").val());
    if (treeObj) {
        var rootNodes = treeObj.getNodes(),
            allNodes = treeObj.transformToArray(rootNodes),
            checkedNodes = treeObj.getCheckedNodes(true);
        $.each(checkedNodes, function () {
            treeObj.checkNode(this, false);
        });
        treeObj.cancelSelectedNode();
        if (p) {
            treeObj.hideNodes(allNodes);
            var nodes = treeObj.getNodesByParamFuzzy('name', p),
                parentNode,
                o = {};
            $.each(nodes, function (i, node) {
                if (!node.isParent) {
                    parentNode = node.getParentNode();
                    o[node.tId] = node;
                    while (parentNode) {
                        o[parentNode.tId] || (o[parentNode.tId] = parentNode);
                        parentNode = parentNode.getParentNode();
                    }
                }
                return true;
            });
            $.each(o, function (key, value) {
                treeObj.showNode(value);
            });
            $.each(rootNodes, function (i, node) {
                node.isParent && o[node.tId] && treeObj.expandNode(node, true, true);
            });
        } else {
            treeObj.showNodes(allNodes);
            treeObj.expandAll(false);
        }
    }
}