define(['list', 'layout', 'wdatepicker'],
    function () {
        initLayout();
        initGird();
        size = UtilWindowHeightWidth();
    });

var $grid,
    size; //宽高
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

//初始化表格
function initGird() {
    $grid = $("#list").jgridview(
        {
            multiselect: true,
            loadonce: false,
            shrinkToFit: true,
            url: '/System/Log/GetPagingDataLog',
            colModel: [
                { name: "DataLogId", hidden: true },
                { label: "登录名", name: "CreateUserCode", width: 130, fixed: true },
                { label: "真实姓名", name: "CreateUserName", width: 130, fixed: true },
                { label: "操作时间", name: "CreateTime", align: "center", fixed: true, width: 130, formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, formatter: 'date' },
                { label: "操作类型", name: "OperateTypeName", index: "OperateType", width: 100, fixed: true, align: "center" },
                { label: "操作表名", name: "OperateTable", width: 150 }
            ],
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
    if (typeof (rowDatas["DataLogId"]) != "undefined") {
        UtilAjaxPost("/System/Log/GetDataLogById", { id: rowDatas["DataLogId"] },
            function (val) {
                $("#form").find('label').each(function () {
                    var $this = $(this), id = $this.attr('id');
                    (val[id]) && $this.text(val[id]);
                });
                //拼接Table
                var table = '';
                var operateData = eval('(' + val.OperateData + ')');
                switch (val.OperateType) {
                    case 0://新增
                    case 2:
                        if (typeof (operateData.length) === "number") {
                            for (var key in operateData) {
                                table += '<table class="complex-table" style="margin:4px 0px 4px 0px"><tbody><tr><th style="width:150px"> 字段名： </th><td>值</td></tr>';
                                for (var dataKey in operateData[key]) {
                                    table += ' <tr><th>' + dataKey + '：</th><td>' + operateData[key][dataKey] + '</td></tr>';
                                }
                                table += '</tbody></table>';
                            }
                        } else {
                            table += '<table class="complex-table" style="margin-top:10px"><tbody><tr><th style="width:150px"> 字段名： </th><td>值</td></tr>';
                            for (var key in operateData) {
                                table += ' <tr><th>' + key + '：</th><td>' + operateData[key] + '</td></tr>';
                            }
                            table += '</tbody></table>';
                        }
                        break;
                    case 1:
                        var operateAfterData = eval('(' + val.OperateAfterData + ')');

                        if (typeof (operateData.length) === "number") {
                            for (var key in operateData) {
                                table += '<table class="complex-table" style="margin-top:10px"><tbody><tr><th style="width:150px"> 字段名： </th><td>修改前</td><td>修改后</td></tr>';
                                for (var dataKey in operateData[key]) {
                                    table += ' <tr><th>' + dataKey + '：</th><td>' + ((operateData[key][dataKey] != operateAfterData[key][dataKey]) ? '<lable style="color:blue">' + operateData[key][dataKey] + '</lable>' : operateData[key][dataKey]) + '</td><td>' + ((operateData[key][dataKey] != operateAfterData[key][dataKey]) ? '<lable style="color:red">' + operateAfterData[key][dataKey] + '</lable>' : operateAfterData[key][dataKey]) + '</td></tr>';
                                }
                                table += '</tbody></table>';
                            }
                        } else {
                            table += '<table class="complex-table" style="margin-top:10px"><tbody><tr><th style="width:150px"> 字段名： </th><td>修改前</td><td>修改后</td></tr>';
                            for (var key in operateData) {
                                table += ' <tr><th>' + key + '：</th><td>' + ((operateData[key] != operateAfterData[key]) ? '<lable style="color:blue">' + operateData[key] + '</lable>' : operateData[key]) + '</td><td>' + ((operateData[key] != operateAfterData[key]) ? '<lable style="color:red">' + operateAfterData[key] + '</lable>' : operateAfterData[key]) + '</td></tr>';
                            }
                            table += '</tbody></table>';
                        }
                        break;
                    default:
                }
                $("#OperateData").html(table);
            }
        );
    } else {
        $("#detailDiv label,#OperateData").html("");
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
            $.post('/System/Log/DeleteDataLogLogById', { id: GridSelectIds($grid, "DataLogId") }, function (data) {
                getGridData();
            });
        });
    });
}

//删除匹配项
function delAll() {
    //查看是否选中
    ArtDialogConfirm(Language.common.deleteallmsg, function () {
        $.post('/System/Log/DeleteDataLogLogAll', {}, function (data) {
            getGridData();
        });
    });
}
