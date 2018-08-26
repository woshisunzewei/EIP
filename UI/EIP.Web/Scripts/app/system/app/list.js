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
        multiselect: false,
        shrinkToFit:true,
        url: "/System/App/GetApp",
        colModel: [
            { name: "AppId", hidden: true },
            { label: "代码", name: "Code", width: 100, fixed: true },
            { label: "名称", name: "Name", width: 200 },
            { label: "简称", name: "ShortName", width: 100, fixed: true },
            { label: "域名/Ip", name: "Domain", width: 200, fixed: true },
            { label: "程序Dll文件", name: "DllPath", width: 200},
            { label: "备注", name: "Remark", width: 100, fixed: true },
            { label: "排序", name: "OrderNo", width: 50, fixed: true }
        ],
        height: $("#uiCenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/System/App/GetApp", {}, function (data) { GridReloadLoadOnceData($grid, data); });
}

//操作:新增
function add() {
    ArtDialogOpen("/System/App/Edit", "新增应用系统配置", true, 380, 580);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/App/Edit?id=" + info.AppId, "修改应用系统配置-" + info.Name, true, 380, 580);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                "/System/App/DeleteApp",
                { id: GridGetSingSelectData($grid).AppId },
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
