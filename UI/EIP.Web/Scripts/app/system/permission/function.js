define([
    "edit",
    "layout",
    "ztree",
    "ztreeExcheck"
], function() {
    initLayout();
    initEvent();
    initFunctionTreeData();
});

var functionZNodes, //模块按钮节点
    functionTreeObj, //模块按钮树对象
    functionSetting; //功能性树设置

//初始化事件
function initEvent() {
    $("#AccessButton li").click(function() {
        if (!$(this).hasClass("selected")) {
            $(this).attr("class", "selected");
        } else {
            $(this).attr("class", "");
        }
    });
}

//初始化菜单
function initFunctionMenuTree() {
    //配置
    functionSetting = {
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
                //隐藏提示
                $(".messagebox").hide();
                $("#AccessButton").show();
                checkAll = true;
                $("#AccessButton li").hide();
                var modelidlength = $("li[modelid='" + treeNode.id + "']").length;
                if (modelidlength > 0) {
                    $("li[modelid='" + treeNode.id + "']").show();
                } else {
                    $(".msg-content").html("<strong style=\"font-size: 14px\">【" + treeNode.name + "】无模块按钮权限</strong>");
                    $(".messagebox").show();
                }

            }
        }
    };
    functionTreeObj = $.fn.zTree.init($("#functionTree"), functionSetting, functionZNodes);
}

//初始化树结构
function initFunctionTreeData() {
    UtilAjaxPost("/System/Permission/GetMenuHavePermissionByPrivilegeMasterValue", {
        privilegeMasterValue: UtilGetUrlParam("privilegeMasterValue"),
        privilegeMaster: UtilGetUrlParam("privilegeMaster"),
        PrivilegeAccess: Language.privilegeAccess.func //模块按钮
    }, function(data) {
        functionZNodes = data;
        initFunctionMenuTree();
    });
}

//初始化功能性权限布局
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

//折叠/展开
var expand = false;

function arrowin() {
    if (expand) {
        functionTreeObj.expandAll(false);
        expand = false;
        $("#arrowin").html("展开").attr("class", "l-icon-arrow-out");
    } else {
        functionTreeObj.expandAll(true);
        expand = true;
        $("#arrowin").html("折叠").attr("class", "l-icon-arrow-in");
    }
}

//全选/重置
var checkAll = true;

function selectall() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("functionTree"));
    if (treeNode.length === 0) {
        DialogTipsMsgWarn("请选择菜单", 1000);
        return;
    }
    var $module = $("li[modelid='" + treeNode[0].id + "']");
    if (checkAll) {
        $module.attr("class", "selected");
        checkAll = false;
    } else {
        $module.attr("class", "");
        checkAll = true;
    }
}

//表单提交
function formSubmit() {
    DialogTipsMsgWait("正在处理中");
    var json = "";
    var $function = $("li[class='selected']").find("a");
    $.each($function, function(i, item) {
        json += "{\"P\":\"" + item.id + "\"},";
    });
    json = json.substring(0, json.length - 1);
    json = "[" + json + "]";
    UtilAjaxPostWait("/System/Permission/SavePermission", {
        privilegeAccess: Language.privilegeAccess.func, //模块按钮
        privilegeMaster: UtilGetUrlParam("privilegeMaster"), //类型
        privilegeMasterValue: UtilGetUrlParam("privilegeMasterValue"), //类型Id
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