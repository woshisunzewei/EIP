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
        url: "/System/MenuButton/GetMenuButtonFunctions",
        colModel: [
            { name: "FunctionId", hidden: true },
            { label: "系统代码", name: "AppCode", width: 80, fixed: true },
            { label: "区域", name: "Area", width: 80, fixed: true },
            { label: "控制器", name: "Controller", width: 100, fixed: true },
            { label: "名称", name: "Action", width: 150, fixed: true },
            { label: "界面", name: "IsPage", width: 40, align: "center", fixed: true, formatter: "YesOrNo" },
            { label: "备注", name: "Description", width: 250, fixed: true }
        ],
        postData: {
            id: UtilGetUrlParam("menubuttonId")
        },
        height: $("#uiCenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/System/MenuButton/GetMenuButtonFunctions", { id: UtilGetUrlParam("menubuttonId") }, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//操作:新增
function add() {
    ArtDialogOpen("/System/MenuButton/ChosenFunctions?id=" + UtilGetUrlParam("menubuttonId"), "选择关联模块按钮-" + UtilGetUrlParam("title"), true, 450, 830);
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                "/System/MenuButton/DeleteMenuButtonFunction",
                {
                    functionId: GridGetSingSelectData($grid).FunctionId,
                    menuButtonId: UtilGetUrlParam("menubuttonId")
                },
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
