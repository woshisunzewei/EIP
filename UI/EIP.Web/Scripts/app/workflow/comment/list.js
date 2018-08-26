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
        url: "/Workflow/Comment/GetComment",
        colModel: [
            { name: "CommentId", hidden: true },
            { label: "内容", name: "Content" },
            {
                label: "归属类型", name: "Type", width: 100, fixed: true, formatter: function (cellval) {
                    var title = "";
                    switch (cellval) {
                        case 0:
                            title = "管理员";
                            break;
                        case 1:
                            title = "员工";
                            break;
                    }
                    return ("<span title='" + title + "'>" + title + "</span>");

                }
            },
            { label: "添加人员", name: "CreateUserName", width: 100, fixed: true },
            { label: "添加时间", name: "CreateTime", width: 130, fixed: true, align: "center", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, formatter: 'date' },
            { label: "修改人员", name: "UpdateUserName", width: 100, fixed: true },
            { label: "修改时间", name: "UpdateTime", width: 130, fixed: true, align: "center", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, formatter: 'date' },
            { label: "序号", name: "OrderNo", width: 100, align: "center", fixed: true }
        ],
        height: $("#uiCenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    $.post("/Workflow/Comment/GetComment", function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//操作:新增
function add() {
    ArtDialogOpen("/Workflow/Comment/Edit", "新增意见", true, 250, 580);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Workflow/Comment/Edit?id=" + info.CommentId, "修改意见-" + info.Content, true, 250, 580);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                 "/Workflow/Comment/DeleteComment",
                { id: GridGetSingSelectData($grid).CommentId },
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