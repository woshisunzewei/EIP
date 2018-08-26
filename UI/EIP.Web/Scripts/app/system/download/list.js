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
        url: "/System/Download/GetList",
        colModel: [
            {name:"DownloadId",label:"主键",width:0,align:"center"},
            {name:"Title",label:"标题",width:0,align:"center"},
            {name:"CategoryId",label:"归属类别",width:0,align:"center"},
            {name:"IsFreeze",label:"是否冻结",width:0,align:"center"},
            {name:"Path",label:"文件路径",width:0,align:"center"},
            {name:"FileName",label:"文件名称",width:0,align:"center"},
            {name:"Size",label:"文件大小",width:0,align:"center"},
            {name:"ShowInHome",label:"是否在主页显示",width:0,align:"center"},
            {name:"DownloadNums",label:"下载次数",width:0,align:"center"},
            {name:"OrderNo",label:"排序号",width:0,align:"center"},
            {name:"ContentType",label:"类别",width:0,align:"center"},
            {name:"CreateOrganizationId",label:"发布机构编号",width:0,align:"center"},
            {name:"CreateOrganizationName",label:"发布机构名称",width:0,align:"center"},
            {name:"CreateUserId",label:"发布用户编号",width:0,align:"center"},
            {name:"CreateUserName",label:"发布用户姓名",width:0,align:"center"},
            {name:"CreateTime",label:"发布时间",width:0,align:"center"},
            {name:"UpdateTimeOrganizationId",label:"修改机构编号",width:0,align:"center"},
            {name:"UpdateTimeOrganizationName",label:"修改机构名称",width:0,align:"center"},
            {name:"UpdateUserId",label:"修改用户编号",width:0,align:"center"},
            {name:"UpdateUserName",label:"修改用户名称",width:0,align:"center"},
            {name:"UpdateTime",label:"修改时间",width:0,align:"center"}
        ],
        height: $("#uiCenter").height() - 51
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/System/Download/GetList", {}, function(data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//新增
function add() {
    ArtDialogOpen("/System/Download/Edit", "新增文章下载记录表", true, 100%, 100%);
}

//编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function() {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/Download/Edit?id=" + info.DownloadId, "修改文章下载记录表", true, 100%, 100%);
    });
}

//删除
function del() {
    //查看是否选中
    GridIsSelect($grid, function() {
        ArtDialogConfirm(Language.common.deletemsg, function() {
            UtilAjaxPostWait(
                "/System/Download/Delete",
                { id: GridGetSingSelectData($grid).DownloadId },
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