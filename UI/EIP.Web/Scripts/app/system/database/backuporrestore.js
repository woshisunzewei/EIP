define(['list', 'layout', 'wdatepicker'],
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
            size: 440,
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
            size:29,
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
        url: "/System/DataBase/GetDataBaseBackUpOrRestore",
        colModel: [
             { name: "BackUpId", hidden: true },
             {
                 label: "名称", name: "Name", width: 180, fixed: true, formatter: function (cellvalue, options, rowObject) {
                     return '<a style="text-decoration: underline;" href="javascript:void(0);" target="_bank">' + rowObject.Name + '</a>';
                 }
             },
             { label: "备份时间", name: "BackUpTime", width: 120, fixed: true, formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, formatter: 'date' },
             { label: "还原时间", name: "RestoreTime", width: 120, fixed: true },
             { label: "来源", name: "DataBaseBackFrom", width: 50, fixed: true, align: "center" },
             { label: "大小", name: "Size", width: 80, fixed: true, sorttype: 'int' },
             { label: "文件本地路径", name: "Path", width: 100 }

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
        UtilAjaxPost("/System/DataBase/GetDataBaseBackUpOrRestore", { id: info.DataBaseId }, function (data) {
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

//备份
function backup() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm("确认备份该数据库?", function () {
            UtilAjaxPostWait(
                "/System/DataBase/DataBaseBackUp",
                { dataBaseId: GridGetSingSelectData($grid).DataBaseId },
                perateStatus
            );
        });
    });
}

//还原
function restore() {
    //查看是否选中
    GridIsSelect($grid_table, function () {
        var data = GridGetSingSelectData($grid_table);
        ArtDialogConfirm("确认还原该数据库到<span style='color:red;font-size:18px'>【" + data.BackUpTime + "】</span>", function () {
            UtilAjaxPostWait(
                "/System/DataBase/DataBaseRestore",
                { id: data.BackUpId },
                perateStatus
            );
        });
    });
}