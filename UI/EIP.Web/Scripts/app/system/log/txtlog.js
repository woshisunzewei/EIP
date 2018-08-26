define([
     'solutionFun',
     'layout',
    'ztree',
    'ztreeExcheck'],
    function () {
        initLayout();
        loadTree();
        reloadTreeSpace();
    });

var zNodes, treeObj, setting;

//初始化布局
function initLayout() {
    $("body").layout({
        "west": {
            size: 260,
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
            }
        }
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
        callback: {
            onClick: onClickTree
        }
    };
}

//初始化树结构:同步
function initTreeData() {
    UtilAjaxPost("/System/Log/GetLogZTree", {}, function (data) {
        zNodes = data;
        treeObj = $.fn.zTree.init($("#tree"), setting, zNodes);
    });
}

//树点击触发
function onClickTree(e, treeId, treeNode) {
    if (treeNode.isParent) {
        treeObj.expandNode(treeNode);
        return;
    }
    //判断模块类型:不同模块调整不同地址
    var url = "/System/Log/TxtLogContent?filePath=" + treeNode.url;
    $("#main").attr("src", url);
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

//加载页面数据
function loadTree() {
    initTree();
    initTreeData();
    $("#main").attr("src", "");
}

//重新计算树高度宽度
function reloadTreeSpace() {
    $("#tree").height($("#uiWest").height() - 38).width($("#uiWest").width() - 10);
}
