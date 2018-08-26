//文本框
UE.plugins['formtext'] = function () {
    var me = this, thePlugins = 'formtext';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/text.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '文本框',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>文本框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//文本域
UE.plugins['formtextarea'] = function () {
    var me = this, thePlugins = 'formtextarea';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/textarea.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '文本域',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/textarea/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>文本域: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//html编辑器
UE.plugins['formhtml'] = function () {
    var me = this, thePlugins = 'formhtml';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/html.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: 'HTML编辑器',
                cssRules: "width:500px;height:260px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>HTML编辑器: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//单选按钮组
UE.plugins['formradio'] = function () {
    var me = this, thePlugins = 'formradio';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/radio.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '单选按钮组',
                cssRules: "width:600px;height:360px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>单选按钮组: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//复选按钮组
UE.plugins['formcheckbox'] = function () {
    var me = this, thePlugins = 'formcheckbox';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/checkbox.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '复选按钮组',
                cssRules: "width:600px;height:360px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>复选按钮组: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//组织机构选择框
UE.plugins['formorg'] = function () {
    var me = this, thePlugins = 'formorg';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/org.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '组织机构选择框',
                cssRules: "width:500px;height:280px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>组织机构选择框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//数据字典选择框
UE.plugins['formdictionary'] = function () {
    var me = this, thePlugins = 'formdictionary';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/dictionary.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '数据字典选择框',
                cssRules: "width:600px;height:280px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_dict") {
            var html = popup.formatHtml('<nobr>数据字典选择框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//日期时间选择
UE.plugins['formdatetime'] = function () {
    var me = this, thePlugins = 'formdatetime';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/datetime.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '日期时间选择',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>日期时间选择: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//隐藏域
UE.plugins['formhidden'] = function () {
    var me = this, thePlugins = 'formhidden';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/hidden.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '隐藏域',
                cssRules: "width:500px;height:280px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>隐藏域: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//下拉列表框
UE.plugins['formselect'] = function () {
    var me = this, thePlugins = 'formselect';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/select.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '下拉列表框',
                cssRules: "width:600px;height:360px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>下拉列表框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//附件上传
UE.plugins['formfiles'] = function () {
    var me = this, thePlugins = 'formfiles';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/files.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '附件上传',
                cssRules: "width:500px;height:280px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>附件上传: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//子表
UE.plugins['formsubtable'] = function () {
    var me = this, thePlugins = 'formsubtable';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/subtable.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '子表',
                cssRules: "width:980px;height:500px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>子表: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//Label标签
UE.plugins['formlabel'] = function () {
    var me = this, thePlugins = 'formlabel';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/label.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: 'Label标签',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>Label标签: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};
//按钮
UE.plugins['formbutton'] = function () {
    var me = this, thePlugins = 'formbutton';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/button.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '按钮',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>按钮: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};

//grid数据表格
UE.plugins['formgrid'] = function () {
    var me = this, thePlugins = 'formgrid';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/grid.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '数据表格',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>数据表格: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};

//组织机构选择
UE.plugins['formorganization'] = function () {
    var me = this, thePlugins = 'formorganization';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: '/Common/UserControl/ChosenOrganizationUeditorAll',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '请选择组织机构',
                cssRules: "width:300px;height:350px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        //指定组织
        if (/button/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('form', '')) {
            var html = popup.formatHtml('<nobr>指定组织: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
        //当前用户
        if (/button/ig.test(el.tagName) && type1 == "flow_current_user_id") {
            var html = popup.formatHtml('<nobr>当前用户: <span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }

        //所有
        if (/button/ig.test(el.tagName) && type1 == "flow_all") {
            var html = popup.formatHtml('<nobr>所有: <span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }

        //所在组织
        if (/button/ig.test(el.tagName) && type1 == "flow_current_organization_id") {
            var html = popup.formatHtml('<nobr>所在组织: <span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }

        //所在组织代码
        if (/button/ig.test(el.tagName) && type1 == "flow_current_organization_code") {
            var html = popup.formatHtml('<nobr>所在组织代码: <span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }

        //所在岗位
        if (/button/ig.test(el.tagName) && type1 == "flow_current_post_id") {
            var html = popup.formatHtml('<nobr>所在岗位: <span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }

        //所在工作组
        if (/button/ig.test(el.tagName) && type1 == "flow_current_group_id") {
            var html = popup.formatHtml('<nobr>所在工作组: <span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};

//工作组选择
UE.plugins['formgroup'] = function () {
    var me = this, thePlugins = 'formgroup';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: '/Common/UserControl/ChosenGroupAll',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '请选择工作组',
                cssRules: "width:650px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        //指定工作组
        if (/button/ig.test(el.tagName) && type1 == "flow_group") {
            var html = popup.formatHtml('<nobr>指定工作组: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }

     
    });
};

//岗位选择
UE.plugins['formpost'] = function () {
    var me = this, thePlugins = 'formpost';
    me.commands[thePlugins] = {
        execCommand: function () {
            var dialog = new UE.ui.Dialog({
                iframeUrl: '/Common/UserControl/ChosenPostAll',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '请选择岗位',
                cssRules: "width:650px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function () {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function () {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
       
        //指定岗位
        if (/button/ig.test(el.tagName) && type1 == "flow_post") {
            var html = popup.formatHtml('<nobr>指定岗位: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};