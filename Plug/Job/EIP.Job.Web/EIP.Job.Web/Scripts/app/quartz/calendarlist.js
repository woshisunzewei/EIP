define([
    'list',
    'layout'
],
    function () {
        initLayout();
        initGird();
    });
var $grid;
//初始化布局
function initLayout() {
    $("body").layout({
        "north": {
            size: 59,
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
        shrinkToFit: true,
        multiselect: false,
        url: "/Quartz/GetCalendar",
        colModel: [
            { label: "名称", name: "CalendarName", width: 250, fixed: true },
            { label: "描述", name: "Description", width: 150 }
        ],
        height: $("#uiCenter").height() - 51,
        rowNum: 500
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/Quartz/GetCalendar", {}, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//操作:新增
function add() {
    ArtDialogOpen("/Quartz/CalendarEdit", "新增日历配置", true, 310, 580);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Quartz/CalendarEdit?calendarName=" + info.CalendarName, "修改日历配置-" + info.CalendarName, true, 310, 580);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                 "/Quartz/DeleteCalendar",
                { calendarName: GridGetSingSelectData($grid).CalendarName },
                perateStatus
            );
        });
    });
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        getGridData();
    }
}