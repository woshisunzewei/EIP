define(['edit'], function () {
    formId = $("#FormId").val();
    initToolbar();
    ueresize();
    formattributeJSON = {
        formId: formId,
        hasEditor: "0",
        dbconn: $("#DataBaseId").val(),
        dbtable: $("#DataBaseTable").val()
    }
});

//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    submitValue["Html"] = SolutionUE.getContent();
    UtilAjaxPostWait("/Workflow/Form/SaveWorkflowFromHtml", submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        if (!UtilEditIsdialogClose()) { art.dialog.close(); }        
    }
}

var formattributeJSON = {}, //表单属性:新建/打开流程后将赋予给对象值,其他组件可直接使用属性值
            formsubtabs = [],
            formEvents = [],
            SolutionUE = null, //表单设计器对象
formId;
//初始化工具栏
function initToolbar() {
    var controlmenu = {
        width: 120,
        items:
        [
            {
                text: "文本框",
                click: function () {
                    execCommend('formtext');
                    return false;
                },
                icon: "ui-text-field"
            },
            {
                text: "文本域",
                click: function () {
                    execCommend('formtextarea');
                    return false;
                },
                icon: "ui-scroll-pane"
            },
            {
                text: "Html编辑器",
                click: function () {
                    execCommend('formhtml');
                    return false;
                },
                icon: "document-attribute-h"
            },
            {
                text: "单选按钮组",
                click: function () {
                    execCommend('formradio');
                    return false;
                },
                icon: "ui-radio-button"
            },
            {
                text: "复选按钮组",
                click: function () {
                    execCommend('formcheckbox');
                    return false;
                },
                icon: "ui-check-boxes-series"
            },
            {
                text: "隐藏域",
                click: function () {
                    execCommend('formhidden');
                    return false;
                },
                icon: "ui-text-field-hidden"
            },
            {
                text: "下拉列表框",
                click: function () {
                    execCommend('formselect');
                    return false;
                },
                icon: "ui-combo-box"
            },
            {
                text: "Lable标签",
                click: function () {
                    execCommend('formlabel');
                    return false;
                },
                icon: "ui-label"
            },
            {
                text: "按钮",
                click: function () {
                    execCommend('formbutton');
                    return false;
                },
                icon: "ui-button"
            },
            {
                text: "组织机构选择框",
                click: function () {
                    execCommend('formorg');
                    return false;
                },
                icon: "sitemap"
            },
            {
                text: "数据字典选择框",
                click: function () {
                    execCommend('formdictionary');
                    return false;
                },
                icon: "ui-scroll-pane-blog"
            },
            {
                text: "日期时间选择",
                click: function () {
                    execCommend('formdatetime');
                    return false;
                },
                icon: "calendar-day"
            },
            {
                text: "附件上传",
                click: function () {
                    execCommend('formfiles');
                    return false;
                },
                icon: "paper-clip"
            },
            {
                text: "子表",
                click: function () {
                    execCommend('formsubtable');
                    return false;
                },
                icon: "table"
            },
            {
                text: "数据表格",
                click: function () {
                    execCommend('formgrid');
                    return false;
                },
                icon: "table-select-column"
            }
        ]
    };
    $("#formtoolbar").ligerToolBar({
        items: [
            { text: "表单控件", menu: controlmenu, icon: "ui-label-link" },
            { line: true },
            {
                text: "发布",
                click: function () {
                    ArtDialogConfirm("确认发布该表单", function () {
                        save();
                        pub();
                    });
                },
                icon: "drive-globe"
            },
             { line: true },
            {
                text: "另存为",
                click: function () { },
                icon: "save"
            }
        ]
    });
}

//发布
function pub() {
    var formid = "";
    if (!formattributeJSON || !formattributeJSON.formId || $.trim(formattributeJSON.formId).length == 0) {
        alert('您未设置表单相关属性!');
        dialog.close();
        return;
    }
    else {
        formattributeJSON.hasEditor = "0";
        formid = formattributeJSON.formId;
        var html = SolutionUE.getContent();
        var $controls = $("[type1^='flow_']", SolutionUE.document);
        for (var i = 0; i < $controls.size() ; i++) {
            var $control = $controls.eq(i);
            var type1Arr = $control.attr('type1').split('_');
            var controlType = type1Arr.length > 1 ? type1Arr[1] : "";
            switch (controlType) {
                case 'text':
                    UE.compule.getTextHtml($control);
                    break;
                case 'sumtext':
                    UE.compule.getSumTextHtml($control);
                    break;
                case 'password':
                    UE.compule.getTextHtml($control);
                    break;
                case 'textarea':
                    UE.compule.getTextareaHtml($control);
                    break;
                case 'radio':
                    UE.compule.getRadioOrCheckboxHtml($control, 'radio');
                    break;
                case 'checkbox':
                    UE.compule.getRadioOrCheckboxHtml($control, 'checkbox');
                    break;
                case 'select':
                    UE.compule.getSelectHtml($control);
                    break;
                case 'combox':
                    UE.compule.getComboxHtml($control);
                    break;
                case 'org':
                    UE.compule.getOrgHtml($control);
                    break;
                case 'dict':
                    UE.compule.getDictHtml($control);
                    break;
                case 'datetime':
                    UE.compule.getDateTimeHtml($control);
                    break;
                case 'files':
                    UE.compule.getFilesHtml($control);
                    break;
                case 'hidden':
                    UE.compule.getHiddenHtml($control);
                    break;
                case 'html':
                    UE.compule.getHtmlHtml($control, formattributeJSON);
                    break;
                case "subtable":
                    UE.compule.getSubTableHtml($control);
                    break;
                case "label":
                    UE.compule.getLabelHtml($control);
                    break;
                case "grid":
                    UE.compule.getGridHtml($control);
                    break;
                case "button":
                    UE.compule.getButtonHtml($control);
                    break;
            }
        }
        UtilAjaxPostWait("/Workflow/Form/SaveWorkflowFromPublic", { formId: formattributeJSON.formId, html: SolutionUE.getContent() }, function (data) {
            if (DialogAjaxResult(data)) { }
            SolutionUE.setContent(html);
        });

    }
}

//保存
function save() {
    if (!formattributeJSON || !formattributeJSON.formId || $.trim(formattributeJSON.formId).length == 0) {
        alert('您未设置表单相关属性!');
        return;
    }
    var submitValue = {
        formId: formattributeJSON.formId,
        html: SolutionUE.getContent()
    };
    UtilAjaxPostWait("/Workflow/Form/SaveWorkflowFromHtml", submitValue, function (data) { });
}

//调整表单设计器大小
function ueresize() {
    SolutionUE = UE.getEditor("editor", {
        wordCount: false,
        maximumWords: 1000000000,
        autoHeightEnabled: false
    });

    SolutionUE.ready(function () {
        SolutionUE.setContent($("#Html").val());
    });
}

//执行方法
function execCommend(method) {
    SolutionUE.execCommand(method);
}
