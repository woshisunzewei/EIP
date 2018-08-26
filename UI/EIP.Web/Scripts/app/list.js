var $;
define([
    'jquery',
    'jqgrid',
    'jgridview',
    'listview',
    'language',
    'solutionFun',
    'artdialog',
    'artdialogIframeTools'],
    function ($) {
        $ = this.$;
        $(function () {
            var toolbar = "";
            $.ajax({
                url: "/System/Permission/GetFunctionByMenuIdAndUserId", // 跳转到地址
                data: { Area: $("#area").val(), Controller: $("#aontroller").val(), Action: $("#action").val() },
                type: "post",
                async: true, //开启异步模式
                dataType: "json",
                success: function (data) {
                    $.each(data, function (i, item) {
                        toolbar += '<a href="javascript:void(0);" title="' + item.Name + '" onclick="' + item.Script + '();return false; "><span class="l-icon-' + item.Icon + '">' + item.Name + '</span></a><span class="toolbarsplit">&nbsp;</span>';
                    });
                    $("#partial_button_toolbar").prepend(toolbar);
                },
                error: function () {
                    DialogTipsMsgError(Language.common.ajaxposterror);
                }
            });
        });
    });
