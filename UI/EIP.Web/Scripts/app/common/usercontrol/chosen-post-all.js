var $;
define([
    'jquery',
    'solutionFun',
    'layout',
    'ztree',
    'ztreeExcheck',
    'ztreeExhide'], function ($) {
        $ = this.$;
        initLayout();
        loadTree();
        initEvent();
        initSelected();
        size = UtilWindowHeightWidth();
    });

var zNodes,
    treeObj,
    setting,
    $grid,
    size,
    oNode = null,
    thePlugins = 'formpost',
    attJSON = parent.formattributeJSON;

//关闭
dialog.oncancel = function () {
    if (UE.plugins[thePlugins].editdom) {
        delete UE.plugins[thePlugins].editdom;
    }
};

//确定
dialog.onok = function () {
    var users = $("li[class='selected']", $(".ScrollBar")).find("a");
    if (users.length === 0) {
        DialogTipsMsgWarn("请选择需要操作的数据", 1000);
        return false;
    }
    var value = "";

    $.each(users, function (i, item) {
        value += item.id + ",";
    });
    //获取选中值
    value = value.substring(0, value.length - 1);
    //获取有多少个{指定组织}标签
    var $controls = $("[type1^='flow_post']", editor.document);
    var size = ($controls.size() + 1);
    var html = '<button type="button" type1="flow_post" value="{指定岗位' + size + '}" style="border:0px;background-color:transparent;width:84px" readonly val="' + value + '">';
    html += "{指定岗位" + size + "}";
    html += '</button>';
    if (oNode) {
        $(oNode).after(html);
        domUtils.remove(oNode, false);
    } else {
        editor.execCommand('insertHtml', html);
    }
    delete UE.plugins[thePlugins].editdom;
}


//表单提交
function formSubmit() {
    var users = $("li[class='selected']", $(".ScrollBar")).find("a");
    var value = "";

    $.each(users, function (i, item) {
        value += item.id + ",";
    });
    value = value.substring(0, value.length - 1);

    art.dialog.data('groupObj', value);
    art.dialog.data('editGroupObj', art.dialog.data('editGroupObj'));
    var win = artDialog.open.origin; //来源页面
    win.setPostValue();
    art.dialog.close();
}

//初始化布局
function initLayout() {
    $("#layout").layout({
        "west": {
            size: 200,
            closable: true,
            resizable: false,
            sliderTip: "显示/隐藏侧边栏",
            togglerTip_open: "关闭",
            togglerTip_closed: "打开",
            resizerTip: "上下拖动可调整大小", //鼠标移到边框时，提示语
            slidable: true
        },
        "center": {
            onresize_end: function () { }
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
        expandSpeed: "",
        callback: {
            onClick: onClickTree
        }
    };
}

//初始化树结构:同步
function initTreeData() {
    UtilAjaxPostAsync("/System/Organization/GetOrganizationTree", null, function (data) {
        zNodes = data;
        treeObj = $.fn.zTree.init($("#tree"), setting, zNodes);
    });
}

//树点击触发
function onClickTree(e, treeId, treeNode) {
    searchPostByOrgId(treeNode.id);
    //展开或者关闭该节点
    treeObj.expandNode(treeNode);
}

//根据组织机构id查询对应的岗位信息
function searchPostByOrgId(orgId) {
    $(".ScrollBar li").hide();
    $("[name='" + orgId + "']").show();
}

var isSelectAll = false;
//选择所有
function selectAll() {
    //选择
    if (!isSelectAll) {
        $("li[style!='display: none;']", $(".ScrollBar")).attr("class", "selected");
        isSelectAll = true;
        $("#select").html("反选").attr("class", "l-icon-ui-check-box-uncheck");
    } else {
        $("li[style!='display: none;']", $(".ScrollBar")).attr("class", "");
        isSelectAll = false;
        $("#select").html("全选").attr("class", "l-icon-ui-check-box");
    }
}

//加载页面数据
function loadTree() {
    initTree();
    initTreeData();
    $(".ScrollBar li").show();
}

//初始化事件
function initEvent() {
    $(".ScrollBar li").click(function () {
        if (!$(this).hasClass("selected")) {
            $(this).attr("class", "selected");
        } else {
            $(this).attr("class", "");
        }
    });
}

//查询
function search() {
    var key = $("#txtKeywords").val().toLocaleLowerCase();
    if (key != "") {
        $(".ScrollBar li").hide();
    } else {
        $(".ScrollBar li").show();
    }
    $("[searchcode*='" + key + "']", $(".ScrollBar")).show();
    $("[searchname*='" + key + "']", $(".ScrollBar")).show();
}

//选中岗位
function selectedPost(posts) {
    if (posts != "") {
        var postArray = posts.split(',');
        $.each(postArray, function (item, i) {
            $("#" + i).parent().attr("class", "selected");
        });
    }
}

//初始化选中
function initSelected() {
    if (UE.plugins[thePlugins].editdom) {
        oNode = UE.plugins[thePlugins].editdom;
    }
    if (oNode) {
        var $obj = $(oNode);
        selectedPost($obj.attr("val"));
    }
}
