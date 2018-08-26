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
        loadonce: true,
        shrinkToFit: true,
        multiselect: false,
        url: "/Workflow/Button/GetButton",
        colModel: [
            { name: "ButtonId", hidden: true },
            { label: "标题", name: "Title", width: 150, fixed: true },
            { label: "图标", name: "Icon", width: 50, formatter: "icon", fixed: true },
            { label: "脚本", name: "Script", width: 350, fixed: true },
            { label: "备注", name: "Remark", width: 350 },
            { label: "序号", name: "OrderNo", width: 100, align: "center", fixed: true }
        ],
        height: $("#uiCenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    $.post("/Workflow/Button/GetButton", function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//操作:新增
function add() {
    ArtDialogOpen("/Workflow/Button/Edit", "新增按钮", true, 410, 580);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Workflow/Button/Edit?buttonId=" + info.ButtonId, "修改按钮-" + info.Title, true, 410, 580);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                 "/Workflow/Button/DeleteButton",
                { id: GridGetSingSelectData($grid).ButtonId },
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