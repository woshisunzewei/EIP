define(['list', 'layout', 'wdatepicker'],
    function (listview,layout) {
        initLayout();
        initGird();
        size = UtilWindowHeightWidth();
    });

var $grid,
    size ; //宽高

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
        loadonce: false,
        url: '/System/Log/GetPagingOperationLog',
        colModel: [
              { name: "OperationLogId", hidden: true },
              { label: "操作时间", name: "OperateTime", width: 130, fixed: true, align: "center", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, formatter: 'date' },
              { label: "登录名", name: "Code", width: 100, fixed: true },
              { label: "真实姓名", name: "Name", width: 60, fixed: true },
              { label: "描述", name: "Describe", width: 400 },
              { label: "响应状态", name: "ResponseStatus", align: "center", width: 50, fixed: true },
              { label: "页面展示(毫秒)", name: "ResultExecutionTime", align: "right", width: 120, fixed: true },
              { label: "方法执行(毫秒)", name: "ActionExecutionTime", align: "right", width: 120, fixed: true }
              //{ label: "客户端", name: "ClientHost", width: 150 }
        ],
        sortname: "OperateTime",
        height: $("#uiCenter").height() - 51,
        sortorder: "desc",
         postData: {
            OperateTime: $("#SearchOperateTime").val(),
            Code: $("#SearchCode").val(),
            Name: $("#SearchName").val(),
            Describe: $("#SearchDescribe").val()
        }
    });
}

//重新获取列表数据
function getGridData() {
    GridReloadPagingData($grid, {
        postData: {
            OperateTime: $("#SearchOperateTime").val(),
            Code: $("#SearchCode").val(),
            Name: $("#SearchName").val()
        }
    });
}

//删除匹配项
function del(ids) {
    $.post('/Log/DeleteBatchExceptionLog', { ids: ids }, function (data) {
        getGridData();
    });
}
