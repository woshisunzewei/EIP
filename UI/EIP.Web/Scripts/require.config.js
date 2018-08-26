//require配置项:配置所有基层及插件的js及依赖css

//配置项
var requireConfig = {
    urlArgs: 'v=1.2',//版本号
    //加载需要在框架使用的文件
    paths: {
        jquery: 'lib/jquery-1.7.2.min',//引入jquery
        language: 'resource/language_zh',//语言包
        solutionFun: 'app/function', //基础function
        artdialog: 'lib/artdialog/artDialog.source',//弹出框组件
        artdialogIframeTools: 'lib/artdialog/plugins/iframeTools.source',
        wdatepicker: 'lib/datepicker/WdatePicker', //日期
        ztree: 'lib/ztree/js/jquery.ztree.core-3.5',//树形结构
        ztreeExcheck: 'lib/ztree/js/jquery.ztree.excheck-3.5',
        ztreeExhide: 'lib/ztree/js/jquery.ztree.exhide-3.5.min',
        jqgrid: 'lib/jqgrid/js/jquery.jqGrid.src',//jqgrid
        jqgridLocale: 'lib/jqgrid/js/i18n/grid.locale-cn',
        jgridview: 'lib/jgridview',
        listview: 'lib/listview',
        layout: 'lib/layout/jquery.layout-latest',
        layoutUiLatest: 'lib/layout/jquery-ui-latest',
        tableexport: 'lib/tableexport', //导出
        formval: 'lib/jquery.formval',//获取数据
        validform: 'lib/validform/Validform_v5.3.2_min',//验证
        qtip: 'lib/qtip/jquery.qtip.min',//提示
        list: 'app/list', //列表
        edit: 'app/edit',//编辑
        ligerui: 'lib/ligerui/js/core/base',//ligerui
        ligerMenu: 'lib/ligerui/js/plugins/ligerMenu',
        ligerTab: 'lib/ligerui/js/plugins/ligerTab',
        ligerLayout: 'lib/ligerui/js/plugins/ligerLayout',
        ligerTree: 'lib/ligerui/js/plugins/ligerTree',
        ligerAccordion: 'lib/ligerui/js/plugins/ligerAccordion',
        artTemplate: 'lib/template',
        beyond: 'lib/beyond/js/bootstrap.min',
        wdtree: 'lib/wdtree/src/Plugins/jquery.tree',
        signalR: 'lib/jquery.signalR-2.2.0',
        jnotify: 'lib/jnotify/jNotify.jquery',
        easyui: 'lib/easyui/jquery.easyui.min',
        kindeditor: 'lib/kindeditor/lang/zh-CN',
        kindeditorch: 'lib/kindeditor/kindeditor-all',
        chosenImage: 'app/common/usercontrol/chosen-image'
    },
    map: {
        '*': { 'css': 'lib/css' }
    },
    //设置依赖关系
    shim: {
        //弹出框
        artdialog: {
            deps: ['css!lib/artdialog/skins/chrome.css', 'jquery']
        },
        artdialogIframeTools: {
            deps: ['artdialog']
        },
        //树
        ztree: {
            deps: ['css!lib/ztree/css/metroStyle/metroStyle.css', 'jquery'] //加载前依赖的css及jquery
        },
        ztreeExcheck: {
            deps: ['jquery', 'ztree']
        },
        ztreeExhide: {
            deps: ['jquery', 'ztree']
        },
        //jqgrid
        jqgrid: {
            deps: [
                'css!lib/jqgrid/css/ui.jqgrid.css',
                'css!lib/jqueryui/default/jquery-ui-base.css',
                'css!lib/jqgrid/css/ui-jqgrid-extend.css',
                'jquery',
                'jqgridLocale'
            ] //加载前依赖的css及jquery
        },
        jqgridLocale: {
            deps: ['jquery']
        },
        jgridview: {
            deps: ['jqgrid']
        },
        listview: {
            deps: ['jgridview']
        },
        //layout
        layout: {
            deps: ['css!lib/layout/layout-default-latest.css', 'layoutUiLatest', 'jquery'] //加载前依赖的css及jquery
        },
        layoutUiLatest: {
            deps: ['jquery'] //加载前依赖的css及jquery
        },
        solutionFun: {
            deps: ['jquery']
        },
        formval: {
            deps: ['jquery']
        },
        validform: {
            deps: ['jquery']
        },
        qtip: {
            deps: ['css!lib/qtip/jquery.qtip.min.css', 'jquery']
        },
        tableexport: {
            deps: ['jquery']
        },
        ligerMenu: {
            deps: ['jquery', 'ligerui']
        },
        ligerTab: {
            deps: ['jquery', 'ligerui']
        },
        ligerLayout: {
            deps: ['jquery', 'ligerui']
        },
        ligerTree: {
            deps: ['jquery', 'ligerui']
        },
        ligerAccordion: {
            deps: ['jquery', 'ligerui']
        },
        ligerui: {
            deps: ['css!lib/ligerui/skins/Aqua/css/ligerui-tab.css', 'jquery']
        },
        artTemplate: {
            deps: ['jquery']
        },
        beyond: {
            deps: ['css!lib/beyond/css/beyond.min.css', 'css!lib/beyond/css/bootstrap.min.css', 'css!lib/beyond/css/font-awesome.min.css', 'jquery']
        },
        wdtree: {
            deps: ['css!lib/wdtree/css/tree.css', 'jquery']
        },
        signalR: {
            deps: ['jquery']
        },
        jnotify: {
            deps: ['css!lib/jnotify/jNotify.jquery.css', 'jquery']
        },
        easyui: {
            deps: ['css!lib/easyui/themes/default/easyui.css', 'css!lib/easyui/themes/icon.css', 'jquery'] //加载前依赖的css及jquery
        },
        kindeditor: {
            deps: ['css!lib/kindeditor/themes/default/default.css', 'jquery', 'kindeditorch']
        },
        chosenImage: {
            deps: ['jquery']
        }
    }
}

//加入配置
require.config(requireConfig);