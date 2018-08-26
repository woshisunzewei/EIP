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
        shrinkToFit: true,
        multiselect: false,
        url: "/Quartz/GetAllJobs",
        colModel: [
           {
               name: "TriggerStateInfo", hidden: true, formatter: function (cellvalue, options, rowObject) {
                   return rowObject.TriggerState;
               }
           },
           { label: "触发器名称", name: "TriggerName", hidden: true },
           { label: "触发器组名", name: "TriggerGroupName", hidden: true },
           { label: "作业组名称", name: "JobGroup", width: 100, fixed: true },
           { label: "作业名称", name: "JobName", width: 170, fixed: true },
           //{ label: "触发器类型", name: "TriggerType", width: 100, fixed: true },
           { label: "方法", name: "ClassName", width: 230, fixed: true },
           {
               label: "状态", name: "TriggerState", width: 60, fixed: true, align: "center", formatter: function (cellval) {
                   var icon = null, title = "";
                   switch (cellval) {
                       case "Paused":
                           icon = "control-pause";
                           title = "暂停中";
                           break;
                       case "Normal":
                           icon = "control";
                           title = "运行中";
                           break;
                       case "Blocked":
                           icon = "control-pause-record";
                           title = "堵塞中";
                           break;
                       case "Complete":
                           icon = "ok";
                           title = "已完成";
                           break;
                       case "Error":
                           icon = "control-record";
                           title = "错误";
                           break;
                       case "None":
                           title = "";
                           break;
                   }
                   return ("<span class='l-icon l-icon-" + icon + "'  title='" + title + "'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "</span>" +
                        "<span>" + title + "</span>"
                          );

               }
           },
           { label: "下一次运行时间", name: "NextFireTime", stype: "date", formatter: 'date', formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' }, width: 120, fixed: true },
           { label: "上一次运行时间", name: "PreviousFireTime", width: 120, fixed: true },
           { label: "作业描述", name: "JobDescription", width: 100 }
        ],
        sortname: "NextFireTime",
        sortorder: "asc",
        height: $("#uiCenter").height() - 51,
        rowNum: 500
    });
}

//获取表格数据
function getGridData() {
    UtilAjaxPost("/Quartz/GetAllJobs", {}, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//操作:新增
function add() {
    ArtDialogOpen("/Quartz/JobEdit", "新增调度作业配置", true, 580, 680);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/Quartz/JobEdit?edit=1&"
            + "jobGroup=" + info.JobGroup
            + "&jobName=" + info.JobName
             + "&triggerName=" + info.TriggerName
              + "&triggerGroup=" + info.TriggerGroupName, "修改调度作业配置-" + info.JobName, true, 580, 680);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm(Language.common.deletemsg, function () {
            UtilAjaxPostWait(
                 "/Quartz/DeleteJob",
                {
                    JobName: GridGetSingSelectData($grid).JobName,
                    JobGroup: GridGetSingSelectData($grid).JobGroup
                },
                perateStatus
            );
        });
    });
}

//开启调度作业
function resumeJob() {
    GridIsSelect($grid, function () {
        if (!(GridGetSingSelectData($grid).TriggerStateInfo == "Normal")) {
            ArtDialogConfirm("确定开启调度作业【" + GridGetSingSelectData($grid).JobName + "】", function () {
                UtilAjaxPostWait(
                    "/Quartz/ResumeJob",
                    {
                        JobName: GridGetSingSelectData($grid).JobName,
                        JobGroup: GridGetSingSelectData($grid).JobGroup
                    },
                    perateStatus
                );
            });
        } else {
            DialogTipsMsgWarn("调度作业【" + GridGetSingSelectData($grid).JobName + "】已启动", 1000);
        }
    });
}


//开启所有调度作业
function resumeAll() {
    ArtDialogConfirm("确定开启所有调度作业", function () {
        UtilAjaxPostWait(
            "/Quartz/ResumeAll",
            {},
            perateStatus
        );
    });
}

//暂停调度作业
function pauseJob() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm("确定调度作业【" + GridGetSingSelectData($grid).JobName + "】", function () {
            UtilAjaxPostWait(
                 "/Quartz/PauseJob",
                {
                    JobName: GridGetSingSelectData($grid).JobName,
                    JobGroup: GridGetSingSelectData($grid).JobGroup
                },
                perateStatus
            );
        });
    });
}

//暂停所有调度作业
function pauseAll() {
    //查看是否选中
    ArtDialogConfirm("是否暂停所有调度作业", function () {
        UtilAjaxPostWait(
             "/Quartz/PauseAll", {},
             perateStatus
        );
    });
}

//导出Excel
function reportExcel() {
    $('#list').tableExport({ fileName: '调度作业Excel', worksheetName: '调度作业列表' });
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        getGridData();
    }
}