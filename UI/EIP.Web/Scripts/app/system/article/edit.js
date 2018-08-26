define([
    'edit',
    'kindeditor',
    'kindeditorch'], function () {
        $(function () {
            kedit();
        });
    });


//表单提交
function formSubmit() {
    var submitValue = $("#form").getValue();
    submitValue["Contents"] = contentsKe.html();
    UtilAjaxPostWait("/System/Article/Save", submitValue, success);
}

//提交成功
function success(data) {
    if (DialogAjaxResult(data)) {
        var win = artDialog.open.origin; //来源页面
        win.search();
        if (UtilEditIsdialogClose()) {
            if ($("#ArticleId").val() === Language.common.guidempty) {
                UtilFormReset("form");
            }
        } else {
            art.dialog.close();
        }
    }
}
var contentsKe;

function kedit() {
    contentsKe = KindEditor.create(
    'textarea[name="results_ke"]',
    {
        width: "1045px", //编辑器的宽度为70%
        height: "400px", //编辑器的高度为100px 
        filterMode: false, //不会过滤HTML代码
        resizeMode: 1,//编辑器只能调整高度 
        uploadJson: '/Common/Upload/KindEditorUpload',
        allowUpload: true,
        allowFileManager: true,
        afterCreate: function () {
            var self = this;
            KindEditor.ctrl(document, 13, function () {
                self.sync();
                document.forms['example'].submit();
            });

            KindEditor.ctrl(self.edit.doc, 13, function () {
                self.sync();
                document.forms['example'].submit();
            });
        },
        items: [
        'source', '|', 'undo', 'redo', '|', 'preview', 'print', 'code', 'cut', 'copy', 'paste',
        'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
        'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
        'superscript', 'clearhtml', 'quickformat', 'selectall', '|', 'fullscreen', '/',
        'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
        'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|',
        'table', 'hr', 'pagebreak',
        'anchor', 'link', 'unlink', '|', 'image', 'multiimage', 'flash', 'media', 'insertfile', 'editImage'
        ],
        afterBlur: function () { this.sync(); },//和DWZ 的 Ajax onsubmit 冲突,提交表单时 编辑器失去焦点执行填充内容
        newlineTag: "br"
    });
}