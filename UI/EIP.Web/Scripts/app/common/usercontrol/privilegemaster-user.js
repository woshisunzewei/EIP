define([
    'list',
    'layout'
    ], function () {
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
        loadonce: false,
        shrinkToFit: true,
        multiselect: false,
        url: "/Common/UserControl/GetPagingPrivilegeMasterUser",
        colModel: [
            { name: "UserId", hidden: true },
            { label: "登录名", name: "Code", index: "u.Code", width: 120, fixed: true },
            { label: "姓名", name: "Name", index: "u.Name", width: 120, fixed: true },
            { label: "归属组织", name: "OrganizationName", index: "OrganizationId", width: 290, fixed: true },
            { label: "电话号码", name: "Mobile", width: 80 },
            { label: "冻结", name: "IsFreeze", index: "u.IsFreeze", width: 50, align: "center", fixed: true, formatter: "YesOrNo" }
        ],
        postData: {
            filters: getFilters("select_box"),
            privilegeMaster: $("#PrivilegeMaster").val(),
            privilegeMasterValue: $("#PrivilegeMasterValue").val()
        },
        height: $("#uiCenter").height() - 51,
        sortname: "u.CreateTime"
    });
}

//获取表格数据
function getGridData() {
    //重新查询
    search();
}

//操作:新增
function add() {
    ArtDialogOpen("/Common/UserControl/ChosenPrivilegeMasterUser?" +
        "privilegeMaster=" + $("#PrivilegeMaster").val() + "&" +
        "privilegeMasterValue=" + $("#PrivilegeMasterValue").val(), "选择人员", true, 450,630);
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                "/Common/UserControl/DeletePrivilegeMasterUser",
                {
                    privilegeMasterUserId: GridGetSingSelectData($grid).UserId,
                    privilegeMasterValue: $("#PrivilegeMasterValue").val(),
                    privilegeMaster: $("#PrivilegeMaster").val()
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
