var $;
define([
    'jquery',
    'edit',
    'language',
    'solutionFun',
    'layout',
    'ztree',
    'ztreeExcheck',
    'ztreeExhide'], function ($) {
        $ = this.$;
        initTreeData();
    });

var zNodes, //菜单树
    treeObj, //菜单树对象
    setting, //菜单设置
 oNode = null,
 thePlugins = 'formorganization',
 attJSON = parent.formattributeJSON;

//关闭
dialog.oncancel = function () {
    if (UE.plugins[thePlugins].editdom) {
        delete UE.plugins[thePlugins].editdom;
    }
};

//确定
dialog.onok = function () {
    var treeData = ZtreeGetCheckedNodes(treeObj);
    if (treeData.length === 0) {
        DialogTipsMsgWarn("请选择需要操作的数据", 1000);
        return false;
    }
    var value = "";
    $.each(treeData, function (i, item) {
        value += item.id + ",";
    });
    //获取选中值
    value = value.substring(0, value.length - 1);
    //获取有多少个{指定组织}标签
    var $controls = $("[type1^='flow_organization']", editor.document);
    var size = ($controls.size() + 1);
    var html = '<button type="button" type1="flow_organization" value="{指定组织' + size + '}" style="border:0px;background-color:transparent;width:84px" readonly val="' + value + '">';
    html += "{指定组织" + size + "}";
    html += '</button>';
    //var html = '<a class="fieldvaluelink all" type1="flow__organization" onclick=" javascript: void (0); " val="' + value + '" title="{指定组织}">{指定组织}</a>';
    if (oNode) {
        $(oNode).after(html);
        domUtils.remove(oNode, false);
    } else {
        editor.execCommand('insertHtml', html);
    }
    delete UE.plugins[thePlugins].editdom;
}

//初始化菜单
function initTree() {
    //配置
    setting = {
        check: {
            enable: true,
            chkStyle: "checkbox",
            chkboxType: { "Y": "p", "N": "p" }
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
    UtilAjaxPostAsync("/System/Organization/GetOrganizationTree", null, function (data) {
        zNodes = data;
        initTree();
        if (UE.plugins[thePlugins].editdom) {
            oNode = UE.plugins[thePlugins].editdom;
        }
        if (oNode) {
            var $obj = $(oNode);
            selectedOrganization($obj.attr("val"));
        }
    });
}

//表单提交
function formSubmit() {
    var treeData = ZtreeGetCheckedNodes(treeObj);
    if (treeData.length === 0) {
        DialogTipsMsgWarn("请选择需要操作的数据", 1000);
        return;
    }
    var value = "";
    $.each(treeData, function (i, item) {
        value += item.id + ",";
    });
    value = value.substring(0, value.length - 1);
    //选中行数据
    art.dialog.data("organizationObj", value);
    art.dialog.data('editOrganizationObj', art.dialog.data('editOrganizationObj'));
    var win = artDialog.open.origin; //来源页面
    win.setOrganizationValue();
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
                    //if (isRadio) { return false; } // 控制单选的情况下是搜索出来的是第一个还是多个
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

//选中组织机构
function selectedOrganization(organization) {
    var organizationArray = organization.split(',');
    $.each(organizationArray, function (i, item) {
        ZtreeAssignCheck(treeObj, item);
    });
}