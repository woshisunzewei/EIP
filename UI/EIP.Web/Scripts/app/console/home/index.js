var $;
define([
    'jquery',
    'beyond',
    'wdtree',
    'ligerui',
    'ligerMenu',
    'ligerTab',
    'ligerLayout',
    'ligerTree',
    'ligerAccordion',
    'signalR',
    'jnotify',
    'artdialog',
    'artdialogIframeTools',
    'language',
    'solutionFun'],
    function ($) {
        $ = this.$;
        $("[data-toggle='tooltip']").tooltip();
        //布局
        $("#layout").ligerLayout({
            leftWidth: 180,
            height: '100%',
            heightDiff: -24,
            space: 4,
            onHeightChanged: heightChanged
        });

        var height = $(".l-layout-center").height();

        //Tab
        $("#framecenter").ligerTab({
            height: height,
            showSwitchInTab: true,
            showSwitch: true,
            onAfterAddTabItem: function (tabdata) {
                tabItems.push(tabdata);
                //saveTabStatus();
            },
            onAfterRemoveTabItem: function (tabid) {
                for (var i = 0; i < tabItems.length; i++) {
                    var o = tabItems[i];
                    if (o.tabid == tabid) {
                        tabItems.splice(i, 1);
                        //saveTabStatus();
                        break;
                    }
                }
            }
        });

        initMenu();

        tab = liger.get("framecenter");
        loadTree();
        $("#pageloading_bg,#pageloading").hide();

        //注册事件
        initEvent();
    });
var tab = null,
    accordion = null,
    tabItems = [],
    menu = null;

//初始化Accordion
function initAccordion() {
    var height = $(".l-layout-center").height();
    //面板
    $("#accordion").ligerAccordion({
        height: height - 24,
        speed: null
    });
    accordion = liger.get("accordion");
}

//加载树形菜单信息
function loadTree() {
    $("#position-left").html("<div id=\"accordion\"></div>");
    //便利具有多少个模块
    UtilAjaxPostAsync("/Console/Home/LoadMenuPermission", {}, function (data) {
        //加载accordion
        var accordionHtml = "";
        $.each(data, function (i, item) {
            accordionHtml += '' +
                '<div title="' + item.name + '" data-icon="' + item.icon + '">' +
                '<ul id="' + item.id + '" style="margin-top: 3px;"></ul>' +
                '</div>';
        });
        $("#accordion").html(accordionHtml);
        //初始化Accordion
        initAccordion();
        //添加树结构
        $.each(data, function (i, dataItem) {
            if (dataItem.tree.length !== 0) {
                var o = {
                    onnodeclick: function (item) {
                        //点击节点打开下级
                        var nid = item.id.replace(/[^\w]/gi, "_");
                        var img = $("#" + dataItem.id + "_" + nid + " img.bbit-tree-ec-icon");
                        if (img.length > 0) {
                            img.click();
                        }
                        if (item.url !== "") {
                            addTab(item.id, item.text, item.url, item.icon);
                        }
                    }
                };
                o.data = dataItem.tree;
                $("#" + dataItem.id + "").treeview(o);
            }
        });
    });
}

//重新加载树
function reloadTree() {
    UtilAjaxPostAsync("/Console/Home/LoadMenuPermission", {}, function (data) {
        //添加树结构
        $.each(data, function (i, dataItem) {
            if (dataItem.tree.length !== 0) {
                var o = {
                    onnodeclick: function (item) {
                        //点击节点打开下级
                        var nid = item.id.replace(/[^\w]/gi, "_");
                        var img = $("#" + dataItem.id + "_" + nid + " img.bbit-tree-ec-icon");
                        if (img.length > 0) {
                            img.click();
                        }
                        if (item.url !== "") {
                            addTab(item.id, item.text, item.url, item.icon);
                        }
                    }
                };
                o.data = dataItem.tree;
                $("#" + dataItem.id + "").treeview(o);
            }
        });
    });
}

//快捷菜单回调函数
function itemclick(item) {
    switch (item.text) {
        case "管理首页":
            addTab('home', '管理中心', '/Console/Index', 'home');
            break;
        case "关闭菜单":
            var tabid = $(".l-selected").attr("tabid");
            if (tabid !== "home") {
                tab.removeTabItem(tabid);
            }
            //调用函数
            break;
    }
}

//高度改变触发
function heightChanged(options) {
    if (tab)
        tab.addHeight(options.diff);
    if (accordion && options.middleHeight - 24 > 0)
        accordion.setHeight(options.middleHeight - 24);
}

//添加tab选项卡
function addTab(tabid, text, url, icon) {
    if (!icon)
        icon = "tree-leaf";
    tab.addTabItem({
        tabid: tabid,
        text: text,
        url: url,
        icon: icon,
        callback: function () { }
    });
}

//退出登录
function loginOut() {
    ArtDialogConfirm(Language.console.loginout, function () {
        window.location.href = "/Account/Logout";
    });
}

//初始化事件
function initEvent() {
    $("#reload-menu").click(function () {
        loadTree();
    });

    $("#tab-tools-nav").bind("click", function () {
        var offset = $(this).offset(); //取得事件对象的位置
        menu.show({ top: offset.top + 27, left: offset.left - 120 });
        return false;
    });

    $(".l-link").hover(function () {
        $(this).addClass("l-link-over");
    }, function () {
        $(this).removeClass("l-link-over");
    });

    //修改密码
    $("#changePwd").click(function () {
        ArtDialogOpen("/Console/Main/ChangePassword", "修改密码", true, 250, 420);
    });
    
    //下载文件
    $("#dowloadFile").click(function () {
        addTab('download', '常用下载', '/Common/Download/Index', 'inbox-download');
    });
}

//初始化下拉菜单
function initMenu() {
    //快捷菜单
    menu = $.ligerMenu({
        width: 120,
        items:
        [
            { text: '管理首页', click: itemclick, icon: "home" },
            { line: true },
            { text: '关闭菜单', click: itemclick, icon: "tab-close-left" }
        ]
    });
}