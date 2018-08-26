(function ($) {
    var columnResized = false; //标识表格表格宽度是否已加载了。

    function saveColumnWidth() {
        //2016-01-15 孙泽伟修改:将当前界面地址当作默认主键
        var gridTitle = window.location.pathname;

        if (typeof gridTitle === 'undefined')
            return;

        var widthModels = [];

        //1 遍历每一列，取出列名称及宽度。 
        $.each($grid.getGridParam("colModel"), function (index, model) {
            if (!model.hidden) {
                var codeModel = $grid.jqGrid('getColProp', model.name);
                var m = {};
                m.name = model.name;
                m.width = parseInt(codeModel.width);
                widthModels.push(m);
            }
        });
        localStorage[gridTitle] = JSON.stringify(widthModels);
    }

    function loadColumnWidth() {
        //2016-01-15 孙泽伟修改:将当前界面地址当作默认主键
        var gridTitle = window.location.pathname;

        if (typeof gridTitle === 'undefined')
            return;

        if (columnResized)
            return;

        var storedWidth = localStorage[gridTitle];
        if (storedWidth === undefined)
            return;

        var widthModels = JSON.parse(storedWidth);
        $.each(widthModels, function (index, model) {
            $grid.jqGrid('setColWidth', model.name, model.width, false);
        });

        columnResized = true;
    }

    $.fn.jgridview = function (options) {
        var $this = $(this); //控件本身
        //定义默认的样式
        var $grid,
            size = UtilWindowHeightWidth(),
            defaults = {
                caption: "",
                shrinkToFit: false,
                autowidth: true,
                url: '',
                multiselect: false, //是否需要复选框
                rowNum: 50,
                rownumbers: true,
                sortname: "",
                sortorder: "desc",
                searchOperators: false,
                loadonce: true, //一次性加载
                postData: {},
                colModel: [],
                rownumWidth: 25,
                pager: "pager",
                height: size.WinH - 97,
                rowList: [50, 80, 100],
                onSelectRow: function () { },
                ondblClickRow: function () { },
                loadComplete: function () {
                    loadColumnWidth();
                },
                resizeStop: function (newwidth, index) {
                    saveColumnWidth();
                }
            };
        var opts = $.extend(defaults, options); //扩展

        $grid = $this.jqGrid(
        {
            altRows: true, //隔行变色
            caption: opts.caption,
            multiselect: opts.multiselect,
            datatype: "json",
            url: opts.url,
            colModel: opts.colModel,
            rowNum: opts.rowNum,
            postData: opts.postData,
            height: opts.height,
            emptyrecords: "没有查询到相关数据",
            loadonce: opts.loadonce,
            autowidth: opts.autowidth,
            rownumbers: opts.rownumbers,
            shrinkToFit: opts.shrinkToFit,
            rowList: opts.rowList,
            mtype: "post",
            sortname: opts.sortname,
            pager: jQuery("#" + opts.pager),
            pagerpos: "left",
            viewrecords: true,
            sortorder: opts.sortorder,
            rownumWidth: opts.rownumWidth,
            ondblClickRow: opts.ondblClickRow,
            onSelectRow: opts.onSelectRow,
            loadComplete: opts.loadComplete,
            resizeStop: opts.resizeStop //宽度改变后事件
        }).navGrid("#" + opts.pager, {
            edit: false,
            add: false,
            del: false,
            search: false,
            refresh: false
        });
        if (opts.searchOperators) {
            $grid.jqGrid('filterToolbar', { searchOperators: true });
        }
        return $grid;
    }
})(jQuery);

//扩展:自动记住拖拽后表格宽度
$.jgrid.extend({
    setColWidth: function (iCol, newWidth, adjustGridWidth) {
        return this.each(function () {
            var $self = $(this), grid = this.grid, p = this.p, colName, colModel = p.colModel, i, nCol;
            if (typeof iCol === "string") {
                // the first parametrer is column name instead of index
                colName = iCol;
                for (i = 0, nCol = colModel.length; i < nCol; i++) {
                    if (colModel[i].name === colName) {
                        iCol = i;
                        break;
                    }
                }
                if (i >= nCol) {
                    return; // error: non-existing column name specified as the first parameter
                }
            } else if (typeof iCol !== "number") {
                return; // error: wrong parameters
            }
            grid.resizing = { idx: iCol };
            grid.headers[iCol].newWidth = newWidth;
            grid.newWidth = p.tblwidth + newWidth - grid.headers[iCol].width;
            grid.dragEnd(); // adjust column width
            if (adjustGridWidth !== false) {
                $self.jqGrid("setGridWidth", grid.newWidth, false); // adjust grid width too
            }
        });
    }
});
