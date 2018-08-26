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
        shrinkToFit: true,
        multiselect: false,
        url: "/System/Running/GetAssemblyByFullName",
        colModel: [
            { label: "名称", name: "Name", width: 350, fixed: true },
            { label: "版本号", name: "Version", width: 150, fixed: true },
            { label: "CLR版本", name: "ClrVersion", width: 100, align: "center", fixed: true }
        ],
        height: $("#uiCenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/System/Running/GetAssemblyByFullName", {}, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//重新加载
function reload() {
    getGridData();
}