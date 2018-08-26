var $;
define([
    'jquery',
    'language',
    'solutionFun',
    'formval',
    'validform',
    'qtip',
    'artdialog',
    'artdialogIframeTools'],
    function ($) {
        $ = this.$;
        $(function () {
            //验证
            method.initValidform();
            //渲染必填项方法
            method.render();
        });
    });


//声明方法
var method = {
    //初始化
    render: function () {
        ValidformNeed();
        //第一个元素焦点
        EditFocus();
    },

    //调整控件大小
    adjustControlsSize: function (container) {
        container || (container = this.wrapper);

        var $container = !(container instanceof jQuery) ? $(container) : container;
        $(':text, :password', $container).each(function () {
            var $this = $(this), $parent = $this.parents();
            $this.css({ width: $parent.width() - 10, height: $parent.height() });
        });
        $('textarea', $container).each(function () {
            var $this = $(this), $parent = $this.parents();
            $this.css({ width: $parent.width() - 10, height: $parent.height() - 6 });
        });
        $('select', $container).each(function () {
            var $this = $(this), $parent = $this.parents();
            $this.css({ width: $parent.width(), height: $parent.height() });
        });
    },

    //初始化验证:若是使用edit.js则必须要有formSubmit方法
    initValidform: function () {
        Validform('form', "btnSubmit", formSubmit, true);
    }
}
