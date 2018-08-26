define(["list", "layout"],
    function() {
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
            onresize_end: function() {
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
        url: "/System/EmailAccount/GetList",
        colModel: [
            { name: "EmailAccountId", hidden: true },
            { label: "账号", name: "Name", width: 250, fixed: true },
            { label: "Smtp", name: "Smtp", width: 150, fixed: true },
            { label: "类型", name: "TypeName", width: 150, fixed: true },
            { label: "冻结", name: "IsFreeze", width: 50, align: "center", fixed: true, formatter: "YesOrNo" },
            { label: "备注", name: "Remark", width: 200 }
        ],
        height: $("#uiCenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/System/EmailAccount/GetList", {}, function(data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//操作:新增
function add() {
    ArtDialogOpen("/System/EmailAccount/Edit", "新增邮件账号", true, 410, 580);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function() {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/EmailAccount/Edit?id=" + info.EmailAccountId, "修改邮件账号-" + info.Name, true, 410, 580);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function() {
        ArtDialogConfirm(Language.common.deletemsg, function() {
            UtilAjaxPostWait(
                "/System/EmailAccount/Delete",
                { id: GridGetSingSelectData($grid).EmailAccountId },
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