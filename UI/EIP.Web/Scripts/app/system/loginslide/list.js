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
        url: "/System/LoginSlide/GetList",
        colModel: [
            { name: "LoginSlideId", hidden: true },
            { name: "Src", label: "图片", width: 400, fixed: true, align: "center", formatter: "image" },
            { name: "Title",label:"标题",width:200,fixed:true},
            { name: "IsFreeze", label: "冻结", width: 80, fixed: true, align: "center", formatter: "YesOrNo" },
            { name: "OrderNo", label: "排序号", width: 80, fixed: true, sorttype: "int", align: "center" },
            { name: "Remark",label:"备注"}
        ],
        height: $("#uiCenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/System/LoginSlide/GetList", {}, function(data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//新增
function add() {
    ArtDialogOpen("/System/LoginSlide/Edit", "新增登录幻灯片", true, '90%', '90%');
}

//编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function() {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/LoginSlide/Edit?id=" + info.LoginSlideId, "修改登录幻灯片", true, '90%', '90%');
    });
}

//删除
function del() {
    //查看是否选中
    GridIsSelect($grid, function() {
        ArtDialogConfirm(Language.common.deletemsg, function() {
            UtilAjaxPostWait(
                "/System/LoginSlide/Delete",
                { id: GridGetSingSelectData($grid).LoginSlideId },
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