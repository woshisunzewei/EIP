define([
    'edit',
    'list',   
    'layout'
   ],
    function () {
        initLayout();
        initGird();
    });

var $grid;
//提交
function formSubmit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        //选中行数据
        art.dialog.data("iconObj", GridGetSingSelectData($grid));
        var win = artDialog.open.origin; //来源页面
        win.setIcon();
        art.dialog.close();
    });
}

//初始化布局
function initLayout() {
    $("#layout").layout({
        "north": {
            size: 36,
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
                $grid.jqGrid("setGridHeight", $("#listcenter").height() - 51).jqGrid("setGridWidth", $("#listcenter").width() - 2);
            }
        }
    });
}

//初始化表格
function initGird() {
    $grid = $("#list").jgridview(
    {
        rownumWidth: 40,
        multiselect: false,
        url: "/Common/UserControl/GetAllIcon",
        colModel: [
            { name: "Url", hidden: true },
             {
                 label: "图标",
                 name: "UrlFormatter",
                 width: 35,
                 fixed: true,
                 formatter:
                     function (cellval, opts, rwd, act) {
                         return '<span><div class="l-icon l-icon-' + rwd.Url + '"style="margin: 0 auto;"></div></sapn>';
                     }
             },
                    { label: "名称", name: "Name", width: 250 },
                    { label: "大小", name: "Length", width: 70 }
        ],
        height: $("#listcenter").height() - 51,
        ondblClickRow: function (rowid) {
            formSubmit();
        }
    });
}

//获取表格数据
function getGridData() {
    $.post("/System/DbConnection/GetDbConnection", function (data) {
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
