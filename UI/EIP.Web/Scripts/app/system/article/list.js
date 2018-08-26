define(["list", "layout"],
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
        loadonce: false,
        url: "/System/Article/GetList",
        colModel: [
            { name: "ArticleId", hidden: true },
            {
                label: "标题", name: "Title", width: 300,
                formatter: function (cellvalue, options, rowObject) {
                    return '<a style="text-decoration: underline;" href="/System/Article/Detail?id=' + rowObject.ArticleId + '" target="_blank">' + rowObject.Title + '</a>';
                }
            },
            { label: "栏目", name: "CategoryName", index: "CategoryId", width: 100, align: "center", fixed: true },
            { label: "发布单位", name: "CreateOrganizationName", index: "CreateOrganizationName", width: 150, fixed: true },
            { label: "发布人", name: "CreateUserName", index: "CreateUserName", width: 80, fixed: true },
            { label: "发布时间", name: "CreateTime", index: "CreateTime", width: 120, align: "center", fixed: true },
            { label: "更新人", name: "UpdateUserName", index: "UpdateUserName", width: 80, fixed: true },
            { label: "更新时间", name: "UpdateTime", index: "UpdateTime", width: 120, align: "center", fixed: true },
            { label: "查看次数", name: "ViewNums", width: 50, align: "center", fixed: true },
            { label: "序号", name: "OrderNo", index: "OrderNo", width: 50, align: "center", fixed: true },
            { label: "首页显示", name: "ShowInHome", index: "ShowInHome", width: 50, align: "center", fixed: true, formatter: "YesOrNo" },
            { label: "冻结", name: "IsFreeze", index: "IsFreeze", width: 50, align: "center", fixed: true, formatter: "YesOrNo" }
        ],
        sortname: "CreateTime",
        height: $("#uiCenter").height() - 51
    });
}


//新增
function add() {
    ArtDialogOpen("/System/Article/Edit", "新增文章新闻表", true, 600, 1060);
}

//编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Article/Edit?id=" + info.ArticleId, "修改文章新闻表", true, 600, 1060);
    });
}

//删除
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                "/System/Article/Delete",
                { id: GridGetSingSelectData($grid).ArticleId },
                perateStatus
            );
        });
    });
}

//冻结/解冻
function freeze() {

}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        search();
    }
}