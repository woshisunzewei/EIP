define(['edit'], function (edit) {
    resize();
    initEvent();
});

var formattributeJSON = [],
    formsubtabs = [],
    formEvents = [],
    myUE = null;

//改变大小
function resize() {
    myUE = UE.getEditor('editor', {
        toolbars: [['FullScreen', 'Source', 'Undo', 'Redo']],
        wordCount: false,
        maximumWords: 1000000000,
        autoHeightEnabled: false
    });
    myUE.ready(function () {
        myUE.setContent($("#RuleHtml").val());
    });
}

//执行
function execCommend(method) {
    myUE.execCommand(method);
}

//表单条件
function formSubmit() {
    var submitValue = $("#form").getValue();
    var $controls = $("[type1^='flow_']", myUE.document);
    for (var i = 0; i < $controls.size() ; i++) {
        var $control = $controls.eq(i);
        var type1Arr = $control.attr('type1').split('_');
        var controlType = type1Arr.length > 1 ? type1Arr[1] : "";
        switch (controlType) {
            case 'organization'://组织机构
                formattributeJSON.push({ Field: $control.val(), Value: $control.attr("val") });
                break;
        }
    }
    submitValue["RuleSql"] = myUE.getContentTxt();
    submitValue["RuleHtml"] = myUE.getContent();
    submitValue["RuleJson"] = JSON.stringify(formattributeJSON);
    UtilAjaxPostWait("/System/Data/SaveData", submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.getGridData();
        if (UtilEditIsdialogClose()) {
            if ($("#DataId").val() == Language.common.guidempty) {
                UtilFormReset("form");
                UtilFocus("Name");
            }
        }
        else {
            art.dialog.close();
        }
    }
}

//初始化选择菜单
function initChosenMenu() {
    ArtDialogOpen("/Common/UserControl/ChosenMenu?parentId=" + $("#MenuId").val() + "&menuType=" + Language.menuType.haveDataPermission, "请选择父级菜单", true, 350, 300);
}

//重新赋予菜单
function setMenu() {
    var menuObj = art.dialog.data('menuObj')[0];
    $("#MenuName").val(menuObj.name);
    $("#MenuId").val(menuObj.id);
}

//初始化事件
function initEvent() {
    $("#chosen_menu").click(function () {
        initChosenMenu();
    });

    //所有
    $(".all").click(function () {
        var html = '<button type1="flow_all" value="{所有}" style="border:0px;background-color:transparent;width:84px" readonly>';
        html += "{所有}";
        html += '</button>';
        myUE.execCommand('insertHtml', html);
    });

    //当前用户
    $(".current_user_id").click(function () {
        var html = '<button type1="flow_current_user_id" value="{当前用户}" style="border:0px;background-color:transparent;width:84px" readonly>';
        html += "{当前用户}";
        html += '</button>';
        myUE.execCommand('insertHtml', html);
    });

    //所在组织
    $(".current_organization_id").click(function () {
        var html = '<button type1="flow_current_organization_id" value="{所在组织}" style="border:0px;background-color:transparent;width:84px" readonly>';
        html += "{所在组织}";
        html += '</button>';
        myUE.execCommand('insertHtml', html);
    });

    //所在岗位
    $(".current_post_id").click(function () {
        var html = '<button type1="flow_current_post_id" value="{所在岗位}" style="border:0px;background-color:transparent;width:84px" readonly>';
        html += "{所在岗位}";
        html += '</button>';
        myUE.execCommand('insertHtml', html);
    });

    //所在工作组
    $(".current_group_id").click(function () {
        var html = '<button type1="flow_current_group_id" value="{所在工作组}" style="border:0px;background-color:transparent;width:90px" readonly>';
        html += "{所在工作组}";
        html += '</button>';
        myUE.execCommand('insertHtml', html);
    });

    //指定组织
    $(".chosen_organization_id").click(function () {
        execCommend('formorganization');
        return false;
    });

    //指定岗位
    $(".chosen_post_id").click(function () {
        execCommend('formpost');
        return false;
    });

    //指定工作组
    $(".chosen_group_id").click(function () {
        execCommend('formgroup');
        return false;
    });

}

//组织机构赋值
function setOrganizationValue() {
    //获取传递回来的值
    var organization = art.dialog.data('organizationObj');
    var editorganization = art.dialog.data('editOrganizationObj');

    var ruleJsonDiv = $("#ruleJsonDiv");
    //若不为空则表示修改
    if (editorganization != null) {
        $("#" + editorganization, ruleJsonDiv).val(organization);
    } else {
        //添加到input标签
        //获取共有多少个org标签input
        var inputsLength = (($("input[id^='org']", ruleJsonDiv).length) + 1);
        var html = '<input id="org' + inputsLength + '" name="org' + inputsLength + '" type="hidden" value="' + organization + '">';
        ruleJsonDiv.append(html);
    }
}

//组赋值
function setGroupValue() {
    //获取传递回来的值
    var group = art.dialog.data('groupObj');
    var editgroup = art.dialog.data('editGroupObj');

    var ruleJsonDiv = $("#ruleJsonDiv");
    //若不为空则表示修改
    if (editgroup != null) {
        $("#" + editgroup, ruleJsonDiv).val(group);
    } else {
        //获取共有多少个group标签input
        var inputsLength = ($("input[id^='group']", ruleJsonDiv).length + 1);
        var html = '<input id="group' + inputsLength + '" name="group' + inputsLength + '" type="hidden" value="' + group + '">';
        ruleJsonDiv.append(html);
    }
}

//岗位赋值
function setPostValue() {
    //获取传递回来的值
    var post = art.dialog.data('postObj');
    var editpost = art.dialog.data('editPostObj');

    var ruleJsonDiv = $("#ruleJsonDiv");
    //添加到input标签
    if (editpost != null) {
        $("#" + editpost, ruleJsonDiv).val(post);
    } else {
        //获取共有多少个post标签input
        var inputsLength = ($("input[id^='post']", ruleJsonDiv).length + 1);
        var html = '<input id="post' + inputsLength + '" name="post' + inputsLength + '" type="hidden" value="' + post + '">';
        ruleJsonDiv.append(html);
    }
}

//修改组织机构值
function editOrganizationValue(organization) {
    var ruleJsonDiv = $("#ruleJsonDiv");
    art.dialog.data('editOrganizationObj', organization);
    art.dialog.data('organizationObj', $("#" + organization, ruleJsonDiv).val());
    ArtDialogOpen("/Common/UserControl/ChosenOrganizationUeditorAll", "请选择组织机构菜单", true, 350, 300);
}

//修改组值
function editGroupValue(group) {
    var ruleJsonDiv = $("#ruleJsonDiv");
    art.dialog.data('editGroupObj', group);
    art.dialog.data('groupObj', $("#" + group, ruleJsonDiv).val());
    ArtDialogOpen("/Common/UserControl/ChosenPostAll", "请选择岗位", true, 450, 600);
}

//修改岗位值
function editPostValue(post) {
    var ruleJsonDiv = $("#ruleJsonDiv");
    art.dialog.data('editPostObj', post);
    art.dialog.data('postObj', $("#" + post, ruleJsonDiv).val());
    ArtDialogOpen("/Common/UserControl/ChosenGroupAll", "请选择组", true, 450, 600);
}
