/************************************************************************
*************************************************************************
@Name :       	jNotify - jQuery Plugin
@Revison :    	2.0
@Date : 		01/2011
@Author:     		Surrel Mickael (www.myjqueryplugins.com - www.msconcept.fr)  -  Croissance Net (www.croissance-net.com)
@Support:    	FF, IE7, IE8, MAC Firefox, MAC Safari
@License :		Open Source - MIT License : http://www.opensource.org/licenses/mit-license.php

**************************************************************************
*************************************************************************/
(function ($) {

    $.jNotify = {
        defaults: {
            /** VARS - OPTIONS **/
            autoHide: true,				// 是否自动隐藏提示条 
            clickOverlay: false,			// 是否单击遮罩层才关闭提示条
            MinWidth: 100,					// 最小宽度 
            TimeShown: 1500, 				// 显示时间：毫秒 
            ShowTimeEffect: 200, 			// 显示到页面上所需时间：毫秒
            HideTimeEffect: 200, 			// 从页面上消失所需时间：毫秒
            LongTrip: 15,					// 从页面上消失所需时间：毫秒
            HorizontalPosition: 'right', 	// 水平位置:left, center, right 
            VerticalPosition: 'bottom',	 // 垂直位置：top, center, bottom 
            ShowOverlay: true,				// 是否显示遮罩层
            ColorOverlay: '#000',			//设置遮罩层的颜色 
            OpacityOverlay: 0.3,			// 设置遮罩层的透明度

            /** METHODS - OPTIONS **/
            onClosed: null,
            onCompleted: null
        },

        /*****************/
        /** Init Method **/
        /*****************/
        init: function (msg, options, id) {
            opts = $.extend({}, $.jNotify.defaults, options);

            /** Box **/
            if ($("#" + id).length == 0)
                $Div = $.jNotify._construct(id, msg);

            // Width of the Brower
            WidthDoc = parseInt($(window).width());
            HeightDoc = parseInt($(window).height());

            // Scroll Position
            ScrollTop = parseInt($(window).scrollTop());
            ScrollLeft = parseInt($(window).scrollLeft());

            // Position of the jNotify Box
            //posTop = $.jNotify.vPos(opts.VerticalPosition);
            posTop = 0;
            posLeft = $.jNotify.hPos(opts.HorizontalPosition);

            // Show the jNotify Box
            if (opts.ShowOverlay && $("#jOverlay").length == 0)
                $.jNotify._showOverlay($Div);

            $.jNotify._show(msg);
        },

        /*******************/
        /** Construct DOM **/
        /*******************/
        _construct: function (id, msg) {
            $Div =
			$('<div id="' + id + '"/>')
			.css({ opacity: 0, minWidth: opts.MinWidth })
			.html(msg)
			.appendTo('body');
            return $Div;
        },

        /**********************/
        /** Postions Methods **/
        /**********************/
        vPos: function (pos) {
            switch (pos) {
                case 'top':
                    var vPos = ScrollTop + parseInt($Div.outerHeight(true) / 2);
                    break;
                case 'center':
                    var vPos = ScrollTop + (HeightDoc / 2) - (parseInt($Div.outerHeight(true)) / 2);
                    break;
                case 'bottom':
                    var vPos = ScrollTop + HeightDoc - parseInt($Div.outerHeight(true));
                    break;
            }
            return vPos;
        },

        hPos: function (pos) {
            switch (pos) {
                case 'left':
                    var hPos = ScrollLeft;
                    break;
                case 'center':
                    var hPos = ScrollLeft + (WidthDoc / 2) - (parseInt($Div.outerWidth(true)) / 2);
                    break;
                case 'right':
                    var hPos = ScrollLeft + WidthDoc - parseInt($Div.outerWidth(true));
                    break;
            }
            return hPos;
        },

        /*********************/
        /** Show Div Method **/
        /*********************/
        _show: function (msg) {
            $Div
			.css({
			    top: posTop,
			    left: posLeft
			});
            switch (opts.VerticalPosition) {
                case 'top':
                    $Div.animate({
                        top: 5,
                        opacity: 1
                    }, opts.ShowTimeEffect, function () {
                        if (opts.onCompleted) opts.onCompleted();
                    });
                    if (opts.autoHide)
                        $.jNotify._close();
                    else
                        $Div.css('cursor', 'pointer').click(function (e) {
                            $.jNotify._close();
                        });
                    break;
                case 'center':
                    $Div.animate({
                        opacity: 1
                    }, opts.ShowTimeEffect, function () {
                        if (opts.onCompleted) opts.onCompleted();
                    });
                    if (opts.autoHide)
                        $.jNotify._close();
                    else
                        $Div.css('cursor', 'pointer').click(function (e) {
                            $.jNotify._close();
                        });
                    break;
                case 'bottom':
                    $Div.animate({
                        top: posTop - opts.LongTrip,
                        opacity: 1
                    }, opts.ShowTimeEffect, function () {
                        if (opts.onCompleted) opts.onCompleted();
                    });
                    if (opts.autoHide)
                        $.jNotify._close();
                    else
                        $Div.css('cursor', 'pointer').click(function (e) {
                            $.jNotify._close();
                        });
                    break;
            }
        },

        _showOverlay: function (el) {
            var overlay =
			$('<div id="jOverlay" />')
			.css({
			    backgroundColor: opts.ColorOverlay,
			    opacity: opts.OpacityOverlay
			})
			.appendTo('body')
			.show();

            if (opts.clickOverlay)
                overlay.click(function (e) {
                    e.preventDefault();
                    $.jNotify._close();
                });
        },

        _close: function () {
            switch (opts.VerticalPosition) {
                case 'top':
                    if (!opts.autoHide)
                        opts.TimeShown = 0;
                    $Div.delay(opts.TimeShown).animate({
                        top: posTop - opts.LongTrip,
                        opacity: 0
                    }, opts.HideTimeEffect, function () {
                        $(this).remove();
                        if (opts.ShowOverlay && $("#jOverlay").length > 0)
                            $("#jOverlay").remove();
                        if (opts.onClosed) opts.onClosed();
                    });
                    break;
                case 'center':
                    if (!opts.autoHide)
                        opts.TimeShown = 0;
                    $Div.delay(opts.TimeShown).animate({
                        opacity: 0
                    }, opts.HideTimeEffect, function () {
                        $(this).remove();
                        if (opts.ShowOverlay && $("#jOverlay").length > 0)
                            $("#jOverlay").remove();
                        if (opts.onClosed) opts.onClosed();
                    });
                    break;
                case 'bottom':
                    if (!opts.autoHide)
                        opts.TimeShown = 0;
                    $Div.delay(opts.TimeShown).animate({
                        top: posTop + opts.LongTrip,
                        opacity: 0
                    }, opts.HideTimeEffect, function () {
                        $(this).remove();
                        if (opts.ShowOverlay && $("#jOverlay").length > 0)
                            $("#jOverlay").remove();
                        if (opts.onClosed) opts.onClosed();
                    });
                    break;
            }
        },

        _isReadable: function (id) {
            if ($('#' + id).length > 0)
                return false;
            else
                return true;
        }
    };

    /** Init method **/
    jNotify = function (msg, options) {
        if ($.jNotify._isReadable('jNotify'))
            $.jNotify.init(msg, options, 'jNotify');
    };

    jSuccess = function (msg, options) {
        if ($.jNotify._isReadable('jSuccess'))
            $.jNotify.init(msg, options, 'jSuccess');
    };

    jError = function (msg, options) {
        if ($.jNotify._isReadable('jError'))
            $.jNotify.init(msg, options, 'jError');
    };

    jWait = function (msg, options) {
        if ($.jNotify._isReadable('jWait'))
            $.jNotify.init(msg, options, 'jWait');
    };

})(jQuery);