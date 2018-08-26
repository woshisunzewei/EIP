define(['edit'], function () {
    initData();
});

//表单提交
function formSubmit() {
    var json = "";
    $("#edit-content").find(':text, :password, textarea, select:not([multiple]),input[type=hidden],input[type=file]').each(function (i, item) {
        json += "{\"C\":\"" + item.id + "\",\"V\":\"" + encodeURI(encodeURI(item.value)) + "\"},";
    });
    json = json.substring(0, json.length - 1);
    json = "[" + json + "]";
    UtilAjaxPostWait("/System/Config/SaveConfig",
    { value: json }, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) { }
}

//初始化数据
function initData() {
    UtilAjaxPostWait("/System/Config/GetConfig", {}, function (data) {
        $.each(data, function (i, item) {
            if (item.C === "f79a849f-0f49-4483-9b24-5399090188f3" || item.C === "6406657e-6d34-43d9-9593-28b076a94f8b") {
                $("#" + item.C + "imghead").attr("src", item.V);
            }
            else {
                $("#" + item.C).val(item.V);
            }

        });
    });
}