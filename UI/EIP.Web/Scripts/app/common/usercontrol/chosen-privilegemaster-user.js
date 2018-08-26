define([
    "edit",
    "layout",
    "ztree",
    "ztreeExcheck",
    "ztreeExhide"
], function() {
    initLayout();
    loadTree();
    initEvent();
    size = UtilWindowHeightWidth();
});

var zNodes,
    treeObj,
    setting,
    $grid,
    size;

//表单提交
function formSubmit() {
    var users = $("li[class='selected']", $(".ScrollBar")).find("a");
    var json = "";

    $.each(users, function(i, item) {
        json += "{\"u\":\"" + item.id + "\"},";
    });
    json = json.substring(0, json.length - 1);
    json = "[" + json + "]";

    UtilAjaxPostWait("/Common/UserControl/SavePrivilegeMasterUser",
        {
            privilegeMasterUser: json,
            privilegeMasterValue: UtilGetUrlParam("privilegeMasterValue"),
            privilegeMaster: UtilGetUrlParam("privilegeMaster")
        },
        success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        //是否继续添加
        ArtDialogConfirmYesNo(Language.common.addagain, function() {}, function() {
            art.dialog.close();
        });
    }
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
            onresize_end: function() {}
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
    UtilAjaxPostAsync("/System/Organization/GetOrganizationTree", null, function(data) {
        zNodes = data;
        treeObj = $.fn.zTree.init($("#tree"), setting, zNodes);
    });
}

//树点击触发
function onClickTree(e, treeId, treeNode) {
    searchUserByOrgId(treeNode.id);
    //展开或者关闭该节点
    treeObj.expandNode(treeNode);
}

//根据组织机构id查询对应的人员信息
function searchUserByOrgId(orgId) {
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
    $(".ScrollBar li").click(function() {
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