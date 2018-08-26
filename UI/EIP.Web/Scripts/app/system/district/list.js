define([
    'list',
    'layout',
    'ztree',
    'ztreeExcheck'
],
function () {
        initLayout();
        initGird();
        initTree();
        initTreeData();
        reloadTreeSpace();
        size = UtilWindowHeightWidth();
    });

var zNodes,
    treeObj,
    setting,
    $grid,
    size;

//初始化布局
function initLayout() {
    $("body").layout({
        "west": {
            size: 200,
            closable: true, //是否可伸缩
            resizable: true, //是否可调整大小
            sliderTip: "显示/隐藏侧边栏", //在某个Pane隐藏后，当鼠标移到边框上显示的提示语。
            togglerTip_open: "关闭", //pane打开时，当鼠标移动到边框上按钮上，显示的提示语
            togglerTip_closed: "打开", //pane关闭时，当鼠标移动到边框上按钮上，显示的提示语
            resizerTip: "可调整大小", //鼠标移到边框时，提示语
            slidable: true
        },
        "center": {
            onresize_end: function () {
                reloadTreeSpace();
                GridSetWidthAndHeight($grid, $("#uiCenter").width(), size.WinH - 84);
            }
        }
    });

    $("#layout").layout({
        "north": {
            size: 29,
            closable: true,
            resizable: false,
            sliderTip: "显示/隐藏侧边栏",
            togglerTip_open: "关闭",
            togglerTip_closed: "打开",
            resizerTip: "上下拖动可调整大小", //鼠标移到边框时，提示语
            slidable: true
        },
        "center": {
            onresize_end: function () {
                reloadTreeSpace();
                //获取调整后高度
                $grid.jqGrid("setGridHeight", $("#listcenter").height() - 51).jqGrid("setGridWidth", $("#listcenter").width() - 2);
            }
        }
    });
}

//初始化表格
function initGird() {
    $grid = $("#list").jgridview(
    {
        multiselect: false,
        url: "/System/District/GetDistrictByParentId",
        colModel: [
            { label: "代码", name: "DistrictId", width: 90 },
            { label: "名称", name: "Name", width: 100, fixed: true },
            { label: "简称", name: "ShortName", width: 80, fixed: true },
            { label: "城市代码", name: "CityCode", width: 80, fixed: true },
            { label: "邮编", name: "ZipCode", width: 60, fixed: true },
            { label: "经度", name: "Lng", width: 100, fixed: true },
            { label: "纬度", name: "Lat", width: 100, fixed: true },
            { label: "拼音", name: "PinYin", width: 100, fixed: true },
            {
                label: "冻结",
                name: "IsFreeze",
                width: 50,
                align: "center",
                fixed: true,
                formatter: "YesOrNo"
            },
             { label: "排序号", name: "OrderNo", width: 50, fixed: true, sorttype: "int" }
        ],
        height: $("#listcenter").height() - 51,
        postData: { id: '0' },
        rowList: [200, 500, 1000]
    });
}

//初始化组织机构
function initTree() {
    //配置
    setting = {
        view: {
            dblClickExpand: false,
            showLine: true
        },
        async: {
            enable: true, //开启异步加载
            url: "/System/District/GetDistrictTreeByParentId", //读取数据地址
            autoParam: ["id"], //后台传递的参数
            otherParam: {}, //额外参数
            type: "post", //请求类型
            dataType: "json"
        },
        data: {
            simpleData: {
                enable: true
            }
        },
        expandSpeed: "",
        callback: {
            onClick: onClickTree
        }
    };
}

//树点击触发
function onClickTree() {
    setHiddenPidValue();
    getGridData();
}

//初始化树结构
function initTreeData() {
    UtilAjaxPostAsync("/System/District/GetDistrictTreeByParentId", { id: '0' }, function (data) {
        zNodes = data;
        treeObj = $.fn.zTree.init($("#tree"), setting, zNodes);
    });
}

//获取表格数据
function getGridData() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    var pId;
    if (treeNode.length === 0) {
        pId = '0';
    } else {
        pId = treeNode[0].id;
    }
    UtilAjaxPost("/System/District/GetDistrictByParentId", { id: pId }, function (data) {
        GridReloadLoadOnceData($grid, data);
    });
}

//赋予隐藏值数据
function setHiddenPidValue() {
    var treeNode = ZtreeGetSelectedNodes($.fn.zTree.getZTreeObj("tree"));
    var pId;
    if (treeNode.length === 0) {
        pId = 0;
    } else {
        pId = treeNode[0].id;
    }
    $("#hiddenPid").val(pId);
    return pId;
}

//操作:新增
function add() {
    ArtDialogOpen("/System/District/Edit?parentId=" + setHiddenPidValue(), "新增省市县", true, 450, 590);
}

//操作:编辑
function edit() {
    //查看是否选中
    GridIsSelect($grid, function () {
        var info = GridGetSingSelectData($grid);
        ArtDialogOpen("/System/District/Edit?parentId=" + setHiddenPidValue() + "&districtId=" + info.DistrictId, "编辑省市县-" + info.Name, true, 450, 590);
    });
}

//删除匹配项
function del() {
    //查看是否选中
    GridIsSelect($grid, function () {
        ArtDialogConfirm("此操作为级联删除,将删除对应的子项!删除后不可恢复,确认要删除?", function () {
            UtilAjaxPostWait(
                "/System/District/DeleteDistrict",
                { districtId: GridGetSingSelectData($grid).DistrictId },
                perateStatus
            );
        });
    });
}

//请求完成
function perateStatus(data) {
    DialogAjaxResult(data);
    if (data.ResultSign === 0) {
        loadTreeAndGrid();
    }
}

//加载页面数据
function loadTreeAndGrid() {
    initTree();
    initTreeData();
    getGridData();
}

//重新计算树高度宽度
function reloadTreeSpace() {
    $("#tree").height($("#uiWest").height() - 38).width($("#uiWest").width() - 10);
}