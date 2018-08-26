var processId = UtilGetUrlParam("processId");

function checkUrlParam() {
    //获取参数
    if (processId === "") {
        ArtDialogContent("参数错误");
        return;
    }
}

//流程图
function flowDesinger() {
    checkUrlParam();
    var processInstanceId = UtilGetUrlParam("processInstanceId");
    if (processInstanceId === "") {
        //打开流程图界面
        ArtDialogOpen("/Workflow/Designer/GooflowPreview?id=" + processId, "查看流程图", true, 600, 1000);
    } else {
        ArtDialogOpen("/Workflow/Run/TrackForMap?processId=" + processId + "&processInstanceId=" + processInstanceId, "查看流程图", true, 600, 1000);
    }
}

//保存数据
function flowSave() {
    checkUrlParam();
    ArtDialogContent("工程师正在努力开发中...");
    return;
}

//启动
function flowBegin() {
    checkUrlParam();
    //判断是否输入标题
    var title = $("#Title").val();
    if (title === "") {
        ArtDialogContent("流程标题不能为空...");
        return;
    }
    ArtDialogConfirm("确定开始启动流程?", function () {
        UtilAjaxPostWait(
            "/Workflow/Run/StartWorkflowEngineProcess",
            {
                processId: processId,
                title: title,
                urgency: $("#Urgency").val()
            },
            perateStatus
        );
    });
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        UtilReloadPage();
    }
}

//打印
function formPrint() {
    ArtDialogContent("工程师正在努力开发中...");
    return;
}

//退回
function flowBack() {
    ArtDialogContent("工程师正在努力开发中...");
    return;
}

//下一步
function flowNext() {
    checkUrlParam();
    var processInstanceId = UtilGetUrlParam("processInstanceId"),
        currentTaskId = UtilGetUrlParam("currentTaskId"),
        currentActivityId = UtilGetUrlParam("currentActivityId"),
        currentActivityName = UtilGetUrlParam("currentActivityName"),
        comment = commentKe.html();
    if (processInstanceId === "" || currentTaskId === "" || currentActivityId === "" || currentActivityName === "") {
        ArtDialogContent("参数不正确");
        return;
    }
    if (comment === "") {
        ArtDialogContent("请填写流程处理意见");
        return;
    }
    UtilAjaxPostWait("/Workflow/Run/SaveWorkflowEngineTaskNext",
        {
            processId: processId,
            processInstanceId: processInstanceId,
            currentTaskId: currentTaskId,
            currentActivityId: currentActivityId,
            currentActivityName: currentActivityName,
            comment: comment
        },
        function (data) {
            DialogAjaxResult(data);
            if (data.ResultSign === 0) {
                var win = artDialog.open.origin; //来源页面
                win.getGridData();
                art.dialog.close();
            }
        });
}

//转交
function flowOtherDo() {
    ArtDialogContent("工程师正在努力开发中...");
    return;
}

//加签
function flowAdd() {
    ArtDialogContent("工程师正在努力开发中...");
    return;
}

//刷新
function flowReload() {
    ArtDialogContent("工程师正在努力开发中...");
    return;
}

//获取历史信息
function getTaskHistory() {
    var processInstanceId = UtilGetUrlParam("processInstanceId");
    if (processInstanceId === "") {
        ArtDialogContent("参数不正确");
    }
    UtilAjaxPostAsync("/Workflow/Run/GetWorkflowEngineTrackForTable",
        {
            processInstanceId: processInstanceId
        },
        function (data) {
            var historyData = { history: data }
            template.config("escape", false);
            var templateHtml = template('taskhistory-template', historyData);
            $("#form_commentlist_div").html(templateHtml);
        });
}