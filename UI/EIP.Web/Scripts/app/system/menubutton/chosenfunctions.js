define([
    'list',
    'layout',
    'edit'
],
    function () {
        initLayout();
        initGird();
    });

var zNodes,
    treeObj,
    setting,
    $grid,
    size = UtilWindowHeightWidth();

//表单提交
function formSubmit() {
    //判断是否选中有数据
    var rowData = $grid.jqGrid("getGridParam", "selarrrow");
    if (!rowData.length) {
        ArtDialogContent("请选择关联模块按钮");
        return;
    }
    var json = "";
    for (var i = 0; i < rowData.length; i++) {
        var functionId = $grid.jqGrid('getCell', rowData[i], "FunctionId"); //name是colModel中的一属性
        json += "{\"MenuButtonId\":\"" + UtilGetUrlParam("id") + "\",\"FunctionId\":\"" + functionId + "\"},";
    }
    json = json.substring(0, json.length - 1);
    json = "[" + json + "]";
    UtilAjaxPostWait("/System/MenuButton/SaveMenuButtonFunction",
    {
        menuButtonFunctions: json
    },
    success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        //是否继续添加
        ArtDialogConfirmYesNo(Language.common.addagain, function () {
            getGridData();
        }, function () {
            art.dialog.close();
        });
    }
}

//初始化布局
function initLayout() {
    $("#layout").layout({
        "west": {
            size: 200,
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
        multiselect: true,
        url: "/System/MenuButton/GetAllFunctions",
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
            id: UtilGetUrlParam("id")
        },
        height: $("#uiCenter").height() - 51,
        rowNum: 500,
        rowList: [500, 800, 1000]
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/System/MenuButton/GetAllFunctions", { id: UtilGetUrlParam("id") }, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
};