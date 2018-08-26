﻿define(['list', 'layout', 'wdatepicker'],
    function (listview, layout) {
        initLayout();
        initGird();
        resetForm();
        size = UtilWindowHeightWidth();
    });

var $grid,
    size; //宽高
//重置表单高度
function resetForm() {
    $("#detailDiv").css({ "height": $("#uiSouth").height() });
}

//布局
function initLayout() {
    $("body").layout({
        "south": {
            size: 245,
            closable: true,
            resizable: true,
            sliderTip: "显示/隐藏侧边栏", //在某个Pane隐藏后，当鼠标移到边框上显示的提示语。
            togglerTip_open: "关闭", //pane打开时，当鼠标移动到边框上按钮上，显示的提示语
            togglerTip_closed: "打开", //pane关闭时，当鼠标移动到边框上按钮上，显示的提示语
            resizerTip: "上下拖动可调整大小", //鼠标移到边框时，提示语
            slidable: true,
            onresize_end: function () {
                resetForm();
            }
        },
        "center": {
            onresize_end: function () {
                //获取调整后高度
                $("#list").jqGrid("setGridHeight", $("#uiCenter").height() - 98);
            }
        }
    });
    $("#layout").layout({
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
                $grid.jqGrid("setGridHeight", $("#uiCenter").height() - 51).jqGrid("setGridWidth", $("#uiCenter").width() - 2);
            }
        }
    });
}

//初始化列表
function initGird() {
    $grid = $("#list").jgridview(
        {
            multiselect: true,
            loadonce: false,
            url: '/System/Log/GetPagingExceptionLog',
            colModel: [
                { name: "ExceptionLogId", hidden: true },
                { label: "登录名", name: "CreateUserCode", width: 100 },
                { label: "真实姓名", name: "CreateUserName", width: 100 },
                { label: "异常时间", name: "CreateTime", align: "center", width: 130, formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, formatter: 'date' },
                { label: "错误信息", name: "Message", width: 350 },
                { label: "请求地址", name: "RequestUrl", width: 350 },
                { label: "客户端", name: "ClientHost", width: 140 }
            ],
            shrinkToFit: true,
            sortname: "CreateTime",
            height: $("#uiCenter").height() - 51,
            sortorder: "desc",
            onSelectRow: function (rowid) {
                getById(rowid);
            },
            loadComplete: function () {
                //选中第一条
                GridSetSelection($grid, 1);
                getById(1);
            }
        });
}

//获取详细信息
function getById(rowid) {
    var rowDatas = $grid.jqGrid('getRowData', rowid);
    if (typeof (rowDatas["ExceptionLogId"]) != "undefined") {
        UtilAjaxPost("/System/Log/GetExceptionLogById", { id: rowDatas["ExceptionLogId"] },
            function (val) {
                $("#form").find('label').each(function () {
                    var $this = $(this), id = $this.attr('id');
                    (val[id] && typeof (val[id]) === 'string') && $this.text(val[id]);
                });
            }
        );
    } else {
        $("#detailDiv label").html("");
    }
}

//获取数据
function getGridData() {
    var select = $("[name='btn_select_box']");
    if (select.length > 0) {
        search($(select[0]).parent());
    }
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            $.post('/System/Log/DeleteExceptionLogById', { id: GridSelectIds($grid, "ExceptionLogId") }, function (data) {
                getGridData();
            });
        });
    });
}

//删除匹配项
function delAll() {
    //查看是否选中
    ArtDialogConfirm(Language.common.deleteallmsg, function () {
        $.post('/System/Log/DeleteExceptionLogAll', {}, function (data) {
            getGridData();
        });
    });
}
