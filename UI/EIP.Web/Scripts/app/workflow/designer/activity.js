define(["edit", "artTemplate"], function (edit, artTemplate) {
    template = artTemplate;
    activityObj = art.dialog.data("activityObj");
    var buttonHtml = template('buttons-template', activityObj);
    $("#button_have").html(buttonHtml);
    $("#form").setValue(activityObj);
    showHideActivityTimeoutRemind();
    initEvent();
});

var $currentButton,
    submitValue, //提交数据
    activityObj,
    template;

//初始化事件
function initEvent() {
    //超期提醒改变
    $("#ActivityTimeoutRemind").change(function () {
        showHideActivityTimeoutRemind();
    });
}

//显示或者隐藏超期提醒
function showHideActivityTimeoutRemind() {
    var $this = $("#ActivityTimeoutRemind");
    switch ($this.val()) {
        case "0":
            $("#ShowActivityTimeoutRemind").show();
            break;
        case "1":
            $("#ShowActivityTimeoutRemind").hide();
            break;
    }
}

//表单条件
function formSubmit() {
    //声明连线对象
    var submitValue = $("#form").getValue();
    //获取按钮
    var activityButtons = [];
    $("ul", $("#button_Select")).each(function (item) {
        var $this = $(this),
            iconStyle = $("label", $this).attr("style");
        activityButtons.push({
            ButtonId: $this.attr("val"),
            Remarks: $this.attr("title"),
            Script: $this.attr("script"),
            Icon: iconStyle.substring(iconStyle.lastIndexOf("/") + 1, iconStyle.lastIndexOf(".")),
            Title: $("label", $this).text()
        });
    });
    var activityObj =
    {
        ActivityId: submitValue.ActivityId,
        ActivityName: submitValue.ActivityName,
        ActivityOpinion: submitValue.ActivityOpinion,
        ActivityCommentsBelow: submitValue.ActivityCommentsBelow,
        ActivityTimeoutRemind: submitValue.ActivityTimeoutRemind,
        ActivityArchive: submitValue.ActivityArchive,
        ActivityWorkTime: submitValue.ActivityWorkTime,
        ActivityTimeoutRemindTypeEmail: submitValue.ActivityTimeoutRemindTypeEmail,
        ActivityTimeoutRemindTypeNote: submitValue.ActivityTimeoutRemindTypeNote,
        ActivityTimeoutRemindTypeWx: submitValue.ActivityTimeoutRemindTypeWx,
        ActivityRemark: submitValue.ActivityRemark,
        ActivityForm: submitValue.ActivityForm,
        //策略
        ActivityProcessorType: submitValue.ActivityProcessorType,
        ActivityProcessorHandler: submitValue.ActivityProcessorHandler,
        ActivityHandlingStrategy: submitValue.ActivityHandlingStrategy,
        ActivityHandlingStrategyPercentage: submitValue.ActivityHandlingStrategyPercentage,
        //按钮
        ActivityButtons: activityButtons,
        //数据

        //事件
        ActivityEventSubmitBefore: submitValue.ActivityEventSubmitBefore,
        ActivityEventSubmitAfter: submitValue.ActivityEventSubmitAfter,
        ActivityEventBackBefore: submitValue.ActivityEventBackBefore,
        ActivityEventBackAfter: submitValue.ActivityEventBackAfter
    };
    //判断提醒类型
    art.dialog.data("activityObj", activityObj);
    var win = artDialog.open.origin; //来源页面
    win.resetActivity();
    art.dialog.close();
}

//双击
function button_dblclick(ul) {
    button_click(ul);
    button_add();
}

//单击
function button_click(ul) {
    $currentButton = $(ul);
    var $buttons = null;
    if ($currentButton.parent().parent().attr("id") == "button_List") {
        $buttons = $("#button_List div ul");
    } else if ($currentButton.parent().parent().attr("id") == "button_Select") {
        $buttons = $("#button_Select div ul");
    }
    $buttons.each(function () {
        $(this).removeClass().addClass("listulli");
    });
    $(ul).removeClass().addClass("listulli1");
    $("#button_Note1").text($(ul).attr("note"));
}

//添加按钮点击
function button_add() {

    if ($currentButton == null) {
        alert("请选择要添加的按钮!");
        return;
    }
    if ($currentButton.parent().parent().attr("id") == "button_List") {
        if ($("#button_Select div ul[val='" + $currentButton.attr("val") + "']").size() > 0) {
            alert("当前按钮已经选择了!");
            return;
        }
        $("#button_Select div").append($currentButton.clone());
    } else if ($currentButton.parent().parent().attr("id") == "button_Select") {
        $currentButton.remove();
    }
    $currentButton = null;
}

//点击删除按钮
function button_remove() {
    if ($currentButton == null) {
        alert("请选择要删除的按钮!");
        return;
    }
    $currentButton.remove();
}