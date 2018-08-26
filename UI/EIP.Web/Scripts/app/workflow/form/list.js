define([
    'list',
    'layout'
],
    function () {
        initLayout();
        initGird();
        size = UtilWindowHeightWidth();
    });

var $grid,
    size;

//初始化布局
function initLayout() {
    $("body").layout({
        "north": {
            size: 59,
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
        url: "/Workflow/Form/GetFormByFormType",
        colModel: [
            { name: "FormId", index: "FormId", hidden: true },
            { label: "名称", name: "Name", width: 250, fixed: true },
            { label: "冻结", formatter: "YesOrNo", name: "IsFreeze", width: 50, align: "center", fixed: true },
            { label: "添加人员", name: "CreateUserName", width: 80, fixed: true },
            { label: "添加时间", name: "CreateTime", width: 130, fixed: true, align: "center", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, formatter: 'date' },
            { label: "修改人员", name: "UpdateUserName", width: 80, fixed: true },
            { label: "修改时间", name: "UpdateTime", width: 130, fixed: true, align: "center", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, formatter: 'date' },
            { label: "排序", align: "center", name: "OrderNo", width: 50, fixed: true, search: false, sorttype: "int" },
            { label: "备注", name: "Remark" }
        ],
        height: $("#uiCenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/Workflow/Form/GetFormByFormType", {}, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//操作:新增
function add() {
    ArtDialogOpen("/Workflow/Form/Edit?edit=0", "新增表单", true, 450, 590);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Workflow/Form/Edit?id=" + info.FormId + "&edit=1", "编辑表单-" + info.Name, true, 450, 590);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                "/Workflow/Form/Delete",
                { id: GridGetSingSelectData($grid).FormId },
                perateStatus
            );
        });
    });
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        loadTreeAndGrid();
    }
}

//设计
function ueditor() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Workflow/Form/Ueditor?id=" + info.FormId, "表单设计-" + info.Name, true, 590, 1090);
    });
}

//预览
function preview() {
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        top.addTab(info.FormId, "预览表单-" + info.Name, "/Workflow/Form/Preview?id=" + info.FormId, "projection-screen-presentation");
    });
}