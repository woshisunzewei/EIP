define(['list', 'layout', 'wdatepicker'],
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
            loadonce: true,
            url: '/System/Running/GgetAllSqlCacheList',
            colModel: [
                { label: "主键名", name: "Key", width: 130 }
            ],
            shrinkToFit: true,
            height: $("#uiCenter").height() - 51,
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
    if (typeof (rowDatas["Key"]) != "undefined") {
        UtilAjaxPost("/System/Running/GetSqlCacheByKey", { id: rowDatas["Key"] },
            function (val) {
                $("#form").find('label').each(function () {
                    var $this = $(this), id = $this.attr('id');
                    (val[id]) && $this.text(val[id]);
                });
                //拼接Table
                var table = '';
                var properties = val.Properties;
                table += '<table class="complex-table" style="margin:4px 0px 4px 0px;width:300px" ><tbody><tr><th style="width:50px;text-align:center"> 序号 </th><th  style="width:250px;text-align:center"> 名称 </th></tr>';
                for (var key in properties) {
                    table += ' <tr><td  style="text-align:center">' + (parseInt(key) + parseInt(1)) + '</td><td>' + properties[key] + '</td></tr>';
                }
                table += '</tbody></table>';
                $("#Properties").html(table);
            }
        );
    } else {
        $("#detailDiv label,#Properties").html("");
    }
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                "/System/Running/DeleteSqlCacheByKey",
                { id: GridGetSingSelectData($grid).Key },
                perateStatus
            );
        });
    });
}

//清空
function delAll() {
    ArtDialogConfirm(Language.common.deleteallmsg, function () {
        UtilAjaxPostWait(
            "/System/Running/DeleteSqlCacheAll",
            {},
            perateStatus
        );
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost('/System/Running/GgetAllSqlCacheList', {},
        function (data) {
            GridReloadLoadOnceData($grid, data);
        });
}


//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        getGridData();
    }
}