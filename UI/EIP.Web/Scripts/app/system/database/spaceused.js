define(['list', 'layout'],
function () {
    initLayout();
    initGird();
    initSearch();
    size = UtilWindowHeightWidth();
});
var $grid,
    $grid_table,
    size,
    rowId = null;

//初始化布局
function initLayout() {
    $("body").layout({
        "west": {
            size: 480,
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
                GridSetWidthAndHeight($grid, $("#uiCenter").width());
                GridSetWidthAndHeight($grid_table, $("#uiCenter").width());
            }
        }
    });
    $("#layout").layout({
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
                $grid.jqGrid("setGridHeight", $("#list_center").height() - 51).jqGrid("setGridWidth", $("#list_center").width() - 2);
                $grid_table.jqGrid("setGridHeight", $("#list_table_center").height() - 51).jqGrid("setGridWidth", $("#list_table_center").width() - 2);
            }
        }
    });
    $("#layout_table").layout({
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
                $grid.jqGrid("setGridHeight", $("#list_center").height() - 51).jqGrid("setGridWidth", $("#list_center").width() - 2);
                $grid_table.jqGrid("setGridHeight", $("#list_table_center").height() - 51).jqGrid("setGridWidth", $("#list_table_center").width() - 2);
            }
        }
    });
}

//初始化表格
function initGird() {
    //应用数据库
    $grid = $("#list").jgridview(
    {
        multiselect: false,
        url: "/System/DataBase/GetAllDataBase",
        colModel: [
            { name: "DataBaseId", hidden: true },
            { label: "名称", name: "Name", width: 150, fixed: true },
            { label: "备注", name: "Remark", width: 200, fixed: true },
            { label: "排序", name: "OrderNo", width: 50, fixed: true }
        ],
        height: $("#list_center").height() - 51,
        loadComplete: function () {
            //选中第一条
            GridSetSelection($grid, 1);
        },
        onSelectRow: function () {
            var info = GridGetSingSelectData($grid);
            initGridTableData(info.DataBaseId);
            tableId = info.DataBaseId;
            getTableGridData();
        }
    });
}

var tableId = null;
//初始化
function initGridTableData(id) {
    tableId = id;
    $grid_table = $("#list_table").jqGrid(
    {
        viewrecords: true,
        loadonce: true,
        mtype: "post",
        datatype: "json",
        pagerpos: "left",
        url: "/System/DataBase/GetDataBaseSpaceused",
        colModel: [
             { label: "表名", name: "Name", width: 180, fixed: true },
             { label: "记录数", name: "Rows", width: 80, fixed: true, sorttype: 'int' },
             { label: "保留空间", name: "Reserved", width: 120, fixed: true, sorttype: 'int' },
             { label: "使用空间", name: "Data", width: 100, fixed: true, sorttype: 'int' },
             { label: "索引使用空间", name: "IndexSize", width: 100, fixed: true, sorttype: 'int' },
             { label: "未用空间", name: "Unused", width: 100, sorttype: 'int' }

        ],
        rowNum: 500,
        rowList: [500, 800, 1000],
        height: $("#list_table_center").height() - 51,
        width: $("#list_table_center").width(),
        pager: '#pager_table',
        sortname: 'BackUpTime',
        sortorder: "asc",
        postData: { id: id },
        multiselect: false
    });
}

//获取表格数据
function getTableGridData() {
    var info = GridGetSingSelectData($grid);
    if (typeof (info.DataBaseId) != "undefined") {
        UtilAjaxPost("/System/DataBase/GetDataBaseSpaceused", { id: info.DataBaseId }, function (data) {
            GridReloadLoadOnceData($grid_table, data);
        });
    }
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/System/DataBase/GetAllDataBase", {}, function (data) {
        GridReloadLoadOnceData($grid, data);
        var info = GridGetSingSelectData($grid);
        tableId = info.DataBaseId;
        getTableGridData();
    });
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        getGridData();
    }
}

//搜索事件
function initSearch() {

    $("#grid_table_search").click(function () {
        var filters = getFilters($(this).parent());
        var postData = $grid_table.getGridParam('postData');
        $.extend(postData, { filters: filters });
        $grid_table.setGridParam({ search: true }).trigger("reloadGrid", [{ page: 1 }]); // must search true
    });

    $("#grid_search").click(function () {
        var filters = getFilters($(this).parent());
        var postData = $grid.getGridParam('postData');
        $.extend(postData, { filters: filters });
        $grid_table.jqGrid("clearGridData");
        $grid.setGridParam({ search: true }).trigger("reloadGrid", [{ page: 1 }]); // must search true
        getTableGridData();
    });
}
