define([
    'list',
    'layout'
],
    function () {
        initEvent();
        initLayout();
        initGird();
    });

var $grid, $mvcAssembly;

//初始化布局
function initLayout() {
    $("body").layout({
        "west": {
            size: 380,
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

    $("#layout_app").layout({
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
                $grid.jqGrid("setGridHeight", $("#uiCenter").height() - 116).jqGrid("setGridWidth", $("#uiWest").width() - 2);
            }
        }
    });

    $("#layout_mvcAssembly").layout({
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
                $mvcAssembly.jqGrid("setGridHeight", $("#uiCenter").height() - 116).jqGrid("setGridWidth", $("#uiCenter").width() - 2);
            }
        }
    });
}

//初始化表格
function initGird() {
    $grid = $("#list").jgridview(
    {
        multiselect: false,
        url: "/System/App/GetApp",
        colModel: [
            { name: "AppId", hidden: true },
            { label: "代码", name: "Code", width: 100, fixed: true },
            { label: "名称", name: "Name", width: 200, fixed: true }
        ],
        height: $("#uiWest").height() - 116,
        ondblClickRow: function () {
            var info = GridGetSingSelectData($grid);
            mvcAssemblyCode = info.Code;
            getMvcAssembly();
        },
        loadComplete: function () {
            //选中第一条
            GridSetSelection($grid, 1);
            var rowDatas = $grid.jqGrid('getRowData', 1);
            initMvcAssemblyCode(rowDatas.Code);
        }
    });
}

//初始化
var mvcAssemblyCode;
function initMvcAssemblyCode(code) {
    mvcAssemblyCode = code;
    $mvcAssembly = $("#mvcAssembly_list").jgridview(
    {
        shrinkToFit: true,
        multiselect: false,
        url: "/System/App/GetFunctionsByAppCode",
        colModel: [
            { label: "区域", name: "Area", width: 60, fixed: true },
            { label: "控制器", name: "Controller", width: 100, fixed: true },
            { label: "名称", name: "Action", width: 170, fixed: true },
            { label: "开发者", name: "ByDeveloperCode", width: 50, fixed: true },
            { label: "开发时间", name: "ByDeveloperTime", width: 80, align: "center", fixed: true },
            { label: "界面", name: "IsPage", width: 40, align: "center", fixed: true, formatter: "YesOrNo" },
            { label: "备注", name: "Description", width: 250, fixed: true }
        ],
        postData: { id: code },
        pager: "mvcAssembly_pager",
        height: $("#uiWest").height() - 116
    });
}

//获取详细信息
function getMvcAssembly() {
    var info = GridGetSingSelectData($grid);
    if (typeof (info.Code) != "undefined") {
        UtilAjaxPost("/System/App/GetFunctionsByAppCode", { id: info.Code }, function (data) {
            GridReloadLoadOnceData($mvcAssembly, data);
        });
    }
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/System/App/GetApp", {}, function (data) { GridReloadLoadOnceData($grid, data); });
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        getGridData();
    }
}

//初始化事件
function initEvent() {
    $("#btn_mvcassmebly_select_box").click(function () {
        var filters = getFilters("mvc_select_box");
        var postData = $mvcAssembly.getGridParam('postData');
        $.extend(postData, { filters: filters });
        $mvcAssembly.setGridParam({ search: true }).trigger("reloadGrid", [{ page: 1 }]);
    });
}
