define(['edit'], function () { initEvent(); });

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    UtilAjaxPostWait("/Quartz/SaveCalendar",
        submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        //如果是新增则提示是否可以继续添加
        art.dialog.close();
    }
}

//初始化事件
function initEvent() {
    $("#chosen_cron").click(function () {
        initChosenCron();
    });
}

//初始化选择Cron
function initChosenCron() {
    ArtDialogOpen("/Quartz/Cron", "设置排除日历Cron表达式", true, 600, 847);
}

//重新赋予Cron
function setCron() {
    var cronObj = art.dialog.data('cronObj');
    $("#Expression").val(cronObj);
}