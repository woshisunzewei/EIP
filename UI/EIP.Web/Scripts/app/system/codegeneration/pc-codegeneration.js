var listDataJson = [],
    editDataJson = [],
    fileDataObj,
    zNodes,
    treeObj,
    setting,
    $grid;

$(document).ready(function () {
    initSmartWizard();
    initData();
    initEvent();
    initTab();
});

//初始化事件
function initEvent() {
    //按钮勾选
    $("#AccessButton li").click(function () {
        if (!$(this).hasClass("selected")) {
            $(this).attr("class", "selected");
        } else {
            $(this).attr("class", "");
        }
    });

    //悬浮效果
    $(".editview").hover(function () {
        $(this).find('.editviewtitle').show();
    }, function (e) {
        $(this).find('.editviewtitle').hide();
    });

    //预览表格
    $("#viewgrid").click(function () {
        ArtDialogContent("预览表格");
    });

    //点击切换表单类型（一排、二排）
    $("#FormshowType").find('a').click(function () {
        $("#FormshowType").find('a').removeClass('active');
        if ($(this).attr('id') === "FormType1") {
            $(this).addClass('active');
            $(".app_layout .item_row").css({ width: "100%", "float": "left" });
        } else if ($(this).attr('id') === "FormType2") {
            $(this).addClass('active');
            $(".app_layout .item_row").css({ width: "50%", "float": "left" });
        }
    });

    //表单控件可以拖动
    $("#Form_layout_list").sortable({
        handle: '.item_field_label',
        placeholder: "ui-state-highlight"
    });

}

//初始化选项卡
function initTab() {
    $("#step-4").ligerTab({
        showSwitchInTab: true,
        showSwitch: true,
        contextmenu: false
    });
}

//初始化数据
function initData() {
    var tableName = UtilGetUrlParam("tableName");

    var replaceTableName = tableName.replace(/_/g, "");
    var tableNameSplit = tableName.split("_");
    var tableNameSplitBefore = tableNameSplit[0];
    var tableNameSplitAfter = tableNameSplit[1].replace(/_/g, "");

    setEntityName(replaceTableName);
    setDataAccessName(replaceTableName);
    setBusinessName(replaceTableName);
    setControllerName(tableNameSplitAfter);
    setOutputFilePath(tableNameSplitBefore, tableNameSplitAfter);

    initColumnsTree();
}

//初始化步骤:http://www.techlaboratory.net/smartwizard/documentation
function initSmartWizard() {
    //var btnFinish = $('<button></button>').text('完成')
    //    .addClass('btn btn-info')
    //    .on('click', function () {
    //        //判断是否已生成代码
    //        if (fileDataObj == null) {

    //        } else {
    //            createCode();
    //        }

    //    });
    var btnCancel = $('<button></button>').text('完成')
        .addClass('btn btn-danger')
        .on('click', function () {
            art.dialog.close();
        });

    // Smart Wizard
    $('#smartwizard').smartWizard({
        selected: 0,
        theme: 'arrows',
        transitionEffect: 'fade',
        toolbarSettings: {
            toolbarPosition: 'bottom',
            toolbarExtraButtons: [btnCancel]
        },
        lang: {
            next: '下一步',
            previous: '上一步'
        }
    });

    //上一步
    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {
        //隐藏下一步

    });

    //下一步
    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection) {
        switch (stepNumber) {
            case 2://列表界面
                setlistDataJson();
                break;
            case 3://生成代码
                createCode();
                break;
            case 4://自动创建
                createFile();
                break;
        }

    });
}

//创建文件
function createFile() {
    //判断是否已生成代码
    if (fileDataObj != null) {
        //为后台提供数据并生成文件
        UtilAjaxPostWait("/System/CodeGeneration/CreateFile", fileDataObj, function (data) {
            if (DialogAjaxResult(data)) {
                $("#createFileMessage").html("创建成功");
                $("#createFileAddMenu").show();
            }
        });
    } else {
        createCode();
    }
}

//生成代码
function createCode() {
    //等待提示
    //DialogTipsMsgWait("代码生成中,请等待...");
    var base = $(".base").getValue();
    base["IsPaging"] = UtilCheckboxIsCheckByObj($("#IsPaging"));
    base["id"] = UtilGetUrlParam("id");
    base["tableName"] = UtilGetUrlParam("tableName");
    base["description"] = UtilGetUrlParam("description");
    base["editWidth"] = $("#EditWidth").val() === "" ? 0 : parseInt($("#EditWidth").val());
    base["editHeight"] = $("#EditHeight").val() === "" ? 0 : parseInt($("#EditHeight").val());

    //获取数据并发送到后台
    UtilAjaxPostWait("/System/CodeGeneration/CreateCode",
    {
        Base: JSON.stringify(base),
        List: JSON.stringify(listDataJson),
        Edit: JSON.stringify(editDataJson)
    }, function (data) {
        var dataJons = data;
        $('#codeForEntities').html('<textarea name="SyntaxHighlighter" class="brush: c-sharp;">' + dataJons["Entities"] + '</textarea>');
        $('#codeForDataAccessInterface').html('<textarea name="SyntaxHighlighter" class="brush: c-sharp;">' + dataJons["DataAccessInterface"] + '</textarea>');
        $('#codeForDataAccess').html('<textarea name="SyntaxHighlighter" class="brush: c-sharp;">' + dataJons["DataAccess"] + '</textarea>');
        $('#codeForBusinessInterface').html('<textarea name="SyntaxHighlighter" class="brush: c-sharp;">' + dataJons["BusinessInterface"] + '</textarea>');
        $('#codeForBusiness').html('<textarea name="SyntaxHighlighter" class="brush: c-sharp;">' + dataJons["Business"] + '</textarea>');
        $('#codeForController').html('<textarea name="SyntaxHighlighter" class="brush: c-sharp;">' + dataJons["Controller"] + '</textarea>');
        $('#codeForList').html('<textarea name="SyntaxHighlighter" class="brush: c-sharp;">' + dataJons["List"] + '</textarea>');
        $('#codeForEdit').html('<textarea name="SyntaxHighlighter" class="brush: c-sharp;">' + dataJons["Edit"] + '</textarea>');
        $('#codeForListJs').html('<textarea name="SyntaxHighlighter" class="brush: js;">' + dataJons["ListJs"] + '</textarea>');
        $('#codeForEditJs').html('<textarea name="SyntaxHighlighter" class="brush: js;">' + dataJons["EditJs"] + '</textarea>');
        SyntaxHighlighter.config.tagName = 'textarea';
        SyntaxHighlighter.highlight();
        fileDataObj = data;
        fileDataObj["Base"] = JSON.stringify(base);
    });
}

//替换
function setEntityName(tableName) {
    $("#entity").val(tableName);
}

//替换DataAccess
function setDataAccessName(tableName) {
    $("#dataAccessInterface").val("I" + tableName + "Repository");
    $("#dataAccess").val(tableName + "Repository");
}

//替换Business
function setBusinessName(tableName) {
    $("#businessInterface").val("I" + tableName + "Logic");
    $("#business").val(tableName + "Logic");
}

//替换控制器
function setControllerName(tableName) {
    $("#controller").val(tableName + "Controller");
    $("#list").val("List");
    $("#edit").val("Edit");
    $("#detail").val("Detail");
    $("#listjs").val("list");
    $("#editjs").val("edit");
}

//输出路径
function setOutputFilePath(tableNameSplitBefore, tableNameSplitAfter) {
    var path = Language.common.codegenerationpath;
    $("#entityPath").val(path + "\\Service\\" + tableNameSplitBefore + "\\EIP." + tableNameSplitBefore + ".Models\\Entities");
    $("#dataAccessInterfacePath").val(path + "\\Service\\" + tableNameSplitBefore + "\\EIP." + tableNameSplitBefore + ".DataAccess");
    $("#dataAccessPath").val(path + "\\Service\\" + tableNameSplitBefore + "\\EIP." + tableNameSplitBefore + ".DataAccess");
    $("#businessInterfacePath").val(path + "\\Service\\" + tableNameSplitBefore + "\\EIP." + tableNameSplitBefore + ".Business");
    $("#businessPath").val(path + "\\Service\\" + tableNameSplitBefore + "\\EIP." + tableNameSplitBefore + ".Business");
    $("#controllerPath").val(path + "\\UI\\EIP.Web\\Areas\\" + tableNameSplitBefore + "\\Controllers");
    $("#listjspath").val(path + "\\UI\\EIP.Web\\Scripts\\app\\" + tableNameSplitBefore.toLocaleLowerCase() + "\\" + tableNameSplitAfter.toLocaleLowerCase());
    $("#editjspath").val(path + "\\UI\\EIP.Web\\Scripts\\app\\" + tableNameSplitBefore.toLocaleLowerCase() + "\\" + tableNameSplitAfter.toLocaleLowerCase());
}

//字段树
function initColumnsTree() {
    var id = UtilGetUrlParam("id");
    var tableName = UtilGetUrlParam("tableName");
    //配置
    setting = {
        view: {
            dblClickExpand: false,
            showLine: true
        },
        data: {
            simpleData: {
                enable: true
            }
        },
        check: {
            enable: true,
            chkStyle: "checkbox"
        },
        expandSpeed: "",
        callback: {
            beforeCheck: zTreeBeforeCheck
        }
    };

    UtilAjaxPost("/System/DataBase/GetDataBaseColumnsTree", { id: id, tableName: tableName }, function (data) {
        zNodes = data;
        treeObj = $.fn.zTree.init($("#tree"), setting, zNodes);
        treeObj.expandAll(true);
    });
}

//点击
function zTreeBeforeCheck(treeId, treeNode) {
    if (!treeNode.checked) {
        editDataJson.push({
            ControlName: treeNode.code,
            ControlId: treeNode.id,
            ControlValidator: 0, //'@EnumControlValidator.不能为空',
            ControlType: 0 // '@EnumControlType.文本框'
        });
        //判断是选择的几排
        var formshowType = $(".active", $("#FormshowType")).attr("id"), itemStyle = "";
        switch (formshowType) {
            case "FormType1":
                itemStyle = "style='width: 100%; float: left;'";
                break;
            case "FormType2":
                itemStyle = "style='width: 50%; float: left;'";
                break;
            default:
        }
        //添加字段
        var $item = $('<div class="item_row" data-value="' + treeNode.id + '"' + itemStyle + '><div class="item_field_label"><span>' + treeNode.code + '</span></div><div class="item_field_value"></div><div class="editviewtitle">编辑控件</div></div>');
        $("#Form_layout .item_table").append($item);
        $item.find('.editviewtitle').click(function () {
            var value = $item.attr('data-value'), editvalue;
            $.each(editDataJson, function (i) {
                if (editDataJson[i].ControlId === value) {
                    editvalue = editDataJson[i];
                }
            });
            //打开编辑框
            editcontrol(editvalue, treeNode.name);
        });
        $item.hover(function () {
            $(this).find('.editviewtitle').show();
        }, function (e) {
            $(this).find('.editviewtitle').hide();
        });
    } else {
        $.each(editDataJson, function (i) {
            if (editDataJson[i].ControlId === treeNode.id) {
                editDataJson.splice(i, 1);
                return false;
            }
        });
        //删除字段
        $("#Form_layout .item_table").find('[data-value=' + treeNode.id + ']').remove();
    }
    return true;
};

//编辑控件
function editcontrol(value, title) {
    value = JSON.stringify(value);
    ArtDialogOpen("/System/CodeGeneration/PcCodeGenerationFormEdit?editJson=" + value, "编辑控件-" + title, true, 300, 590);
}

//重置表单编辑的Json
function resetFormEditDataJson() {
    var data = art.dialog.data("editDataJsonObj");
    if (data.ControlColspan === "1") {
        $("#Form_layout .item_table").find('[data-value=' + data.ControlId + ']').css({ width: "100%", "float": "left" });
    } else {
        $("#Form_layout .item_table").find('[data-value=' + data.ControlId + ']').css({ width: "50%", "float": "left" });
    }
    $("#Form_layout .item_table").find('[data-value=' + data.ControlId + ']').find('.item_field_label span').html(data.ControlName);
    $.each(editDataJson, function (i) {
        if (editDataJson[i].ControlId === data.ControlId) {
            editDataJson[i] = data;
            return false;
        }
    });
}

//给listDataJson赋值
function setlistDataJson() {
    var allList = $("tr", $("#codeGenerationList")), isPaging = UtilCheckboxIsCheckByObj($("#IsPaging"));
    listDataJson = [];
    $.each(allList, function (i, item) {
        if (i > 0) {

            var $item = $(item);
            var need = UtilCheckboxIsCheckByObj($("[name='need']", $item));
            if (!need) {
                return;
            }
            var data = {
                name: $("[name='name']", $item).val()
            };
            //是否隐藏
            var hidden = UtilCheckboxIsCheckByObj($("[name='hidden']", $item));
            if (hidden) {
                data["label"] = $("[name='label']", $item).val();
            } else {
                data["hidden"] = hidden;
            }

            //是否分页
            if (isPaging) {
                data["index"] = $("[name='index']", $item).val();
            }

            //自适应
            var fixed = UtilCheckboxIsCheckByObj($("[name='fixed']", $item));
            if (fixed) {
                data["fixed"] = fixed;
            } else {
                data["width"] = $("[name='width']", $item).val() === "" ? 0 : parseInt($("[name='width']", $item).val());
            }

            //排序
            data["sortable"] = UtilCheckboxIsCheckByObj($("[name='sortable']", $item));
            //是否有排序类型
            if ($("[name='sorttype']", $item).val() !== "") {
                data["sorttype"] = $("[name='sorttype'] option:selected", $item).text();
            }

            //是否居中
            if ($("[name='align']", $item).val() !== "") {
                data["align"] = $("[name='align'] option:selected", $item).text();
            }

            //是否格式化
            if ($("[name='formatter']", $item).val() !== "") {
                data["formatter"] = $("[name='formatter'] option:selected", $item).text();
            }

            data["orderno"] = $("[name='orderno']", $item).val();

            listDataJson.push(data);
        }
    });
}

//新增菜单
function addMenu() {
    ArtDialogOpen("/System/Menu/Edit?parentId=", "新增模块", true, 440, 600);
}

//创建菜单成功
function loadTreeAndGrid() {
    var menuId = art.dialog.data("menuObj").menuId;
    //添加菜单按钮
    var buttonDataJson = "";
    var buttons = $("li[class='selected']").find("a");
    $.each(buttons, function (i, item) {
        buttonDataJson += "{\"MenuButtonId\":\"" + UtilNewGuid() + "\",\"MenuId\":\"" + menuId + "\",\"Icon\":\"" + $(item).attr("icon") + "\",\"Name\":\"" + $(item).attr("title") + "\",\"Script\":\"" + $(item).attr("script") + "\",\"OrderNo\":" + $(item).attr("orderNo") + ",\"Remark\":\"" + $(item).attr("remark") + "\"},";
    });
    buttonDataJson = buttonDataJson.substring(0, buttonDataJson.length - 1);
    buttonDataJson = "[" + buttonDataJson + "]";
    UtilAjaxPostWait("/System/CodeGeneration/SaveMenuButton", { buttons: buttonDataJson }, function (data) {

    });
}