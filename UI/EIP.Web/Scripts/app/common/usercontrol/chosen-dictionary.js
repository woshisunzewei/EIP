define([
    'edit',
    'ztree',
    'ztreeExcheck'], function () { initTreeData(); });

var zNodes, //字典树
    treeObj, //字典树对象
    setting; //字典设置

//初始化字典
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
    treeObj.expandAll(true);
}

//初始化树结构
function initTreeData() {
    //判断是否具有筛选字典条件
    var postUrl = "/Common/UserControl/GetDictionaryTreeByCode";
    UtilAjaxPostAsync(postUrl, { code: UtilGetUrlParam("code").replace(" ", "") }, function (data) {
        zNodes = data;
        initTree();
        //绑定选中值
        var pId = UtilGetUrlParam("dicId").replace(" ", "");
        if (pId != Language.common.guidempty && pId != null) {
            ZtreeAssignCheck(treeObj, pId);
        }
    });
}

//表单提交
function formSubmit() {
    var treeData = ZtreeGetCheckedNodes(treeObj), dicObj = [];
    dicObj.push({
        code: UtilGetUrlParam("code").replace(" ", ""),
        tree: treeData,
        id: UtilGetUrlParam("id").replace(" ", "")
    });
    art.dialog.data("dictionaryObj", dicObj);
    var win = artDialog.open.origin; //来源页面
    win.setDicValue();
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