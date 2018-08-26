define(['list', 'layout', 'wdatepicker'],
    function (listview, layout) {
        initLayout();
        initGird();
        size = UtilWindowHeightWidth();
    });

var $grid,
    size; //宽高

//初始化布局
function initLayout() {
    $("body").layout({
        "north": {
            size: 29,
            closable: true,
            resizable: false,
            sliderTip: "显示/隐藏侧边栏",
            togglerTip_open: "关闭",
            togglerTip_closed: "打开",
            resizerTip: "上下拖动可调整大小", //鼠标移到边框时，提示语
            slidable: true
        },
        "center": {
            onresize_end: function () {
                //获取调整后高度
                $grid.jqGrid("setGridHeight", $("#uiCenter").height() - 50).jqGrid("setGridWidth", $("#uiCenter").width() - 2);
            }
        }
    });
}

//初始化表格
function initGird() {
    $grid = $("#list").jgridview(
        {
            multiselect: true,
            loadonce: false,
            shrinkToFit: true,
            url: '/System/Log/GetPagingLoginLog',
            colModel: [
                { name: "LoginLogId", hidden: true },
                { label: "登录名", name: "CreateUserCode", width: 130 },
                { label: "真实姓名", name: "CreateUserName", width: 100 },
                { label: "登录时间", name: "LoginTime", align: "center", fixed: true, width: 120, formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, formatter: 'date' },
                { label: "退出时间", name: "LoginOutTime", align: "center", width: 120, fixed: true, formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, formatter: 'date' },
                { label: "停留时间(小时)", name: "StandingTime", width: 100 },
                { label: "客户端IP", name: "ClientHost", width: 150 },
                { label: "参考地址", name: "IpAddressName", width: 150 },
                //{ label: "服务器", name: "ServerHost", width: 150 },
                { label: "浏览器信息", name: "UserAgent", width: 150 },
                { label: "操作系统", name: "OsVersion", width: 100 }
            ],
            sortname: "CreateTime",
            height: $("#uiCenter").height() - 51,
            sortorder: "desc"
        });
}

//获取数据
function getGridData() {
    var select = $("[name='btn_select_box']");
    if (select.length > 0) {
        search($(select[0]).parent());
    }
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            $.post('/System/Log/DeleteLoginLogById', { id: GridSelectIds($grid, "LoginLogId") }, function (data) {
                getGridData();
            });
        });
    });
}

//删除匹配项
function delAll() {
    //查看是否选中
    ArtDialogConfirm(Language.common.deleteallmsg, function () {
        $.post('/System/Log/DeleteLoginLogAll', {}, function (data) {
            getGridData();
        });
    });
}
