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
        colModel: [
            { name: "ConfigId", hidden: true },
            { label: "代理人", name: "Remark", width: 250, fixed: true },
            { label: "创建人", name: "Code", width: 150, fixed: true },
            { label: "流程类型", name: "OrderNo", width: 100, align: "center", fixed: true },
            { label: "开始时间", name: "OrderNo", width: 100, align: "center", fixed: true },
            { label: "结束时间", name: "OrderNo", width: 100, align: "center", fixed: true },
            { label: "当前状态", name: "OrderNo", width: 100, align: "center", fixed: true }
        ],
        height: $("#uiCenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/System/Config/GetConfig", {}, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//操作:新增
function add() {
    ArtDialogOpen("/System/Config/Edit", "新增参数配置", true, 410, 580);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Config/Edit?id=" + info.ConfigId, "修改参数配置-" + info.Code, true, 410, 580);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                 "/System/Config/DeleteConfig",
                { id: GridGetSingSelectData($grid).ConfigId },
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

//页面初始化执行
$(function () {
    initLayout();
    initGird();
});
