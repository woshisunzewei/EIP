require(['require.config','page'], function(cfg,page) {
    // 默认的页面映射
    var pageMaps = {};

    /**
     * 页面映射
     * @param maps 初始化的页面映射
     * @constructor
     */
    var PageMap = function (maps) {
        pageMaps = maps || pageMaps;
    };

    /**
     * 添加或更新一个页面映射关系
     * @param moduleName 模块名（页面名）
     * @param path 模块（页面）的路径，相对于 BaseUrl
     */
    PageMap.prototype.add = function (moduleName, path) {
        pageMaps[moduleName] = path;
    };

    /**
     * 移除相应的模块
     * @param moduleName 模块名
     */
    PageMap.prototype.remove = function (moduleName) {
        if (!(typeof moduleName === 'string') || !moduleName.length) {
            return;
        }
        delete pageMaps[moduleName];
    };

    /**
     * 查找相关页面映射
     * @param moduleName 模块名
     * @returns {*} 查找到返回模块映射对象，没有查找到返回 null
     */
    PageMap.prototype.find = function (moduleName) {
        if (!(typeof moduleName === 'string') || !moduleName.length) {
            return null;
        }
        var path = pageMaps[moduleName];

        return typeof path !== 'string' ?
            null :
            (
                path === '' ?
                // 如果没有 path , 则以 moduleName 作为 path
                { moduleName: moduleName, path: moduleName } :
                { moduleName: moduleName, path: path }
            );
    };

    var pageMap = new PageMap(page); //页面映射

    /**
     * 引入与页面相关的主模块
     */
    var requireMainModule = function (moduleName) {
        var m = moduleName || window.moduleName ||
            (function () {
                // 从 html 节点中查找 data-module 属性
                var body = document.getElementsByTagName('html')[0],
                    attribute = body.attributes['data-require'],
                    value = attribute.value;
                return value;
            })();

        var o, path = (typeof m === 'string' && (o = pageMap.find(m), o && o.path || '') || '');
        path && require([path]);
    };

    requireMainModule();
})