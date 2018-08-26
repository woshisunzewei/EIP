/**
 * 将表单中的输入控件的值转换成键值对对象的格式，以下是转换规则：
 * 1. 文本框(type=text, type=password)、文本区域、不包含multiple属性的下拉选择框、name相同的一组单选框转换为字符串
 *    注意：一组单选框如果都没有点选，则为null
 * 2. 数值类型的文本框(type=number)转换为数字(HTML5 Only)
 * 3. 不包含value属性的复选框转换为布尔值
 *    注意：不应该出现多个name相同而又不包含value属性的复选框，否则结果不可预料
 * 4. name相同且都包含value属性的复选框、包含multiple属性的多重选择框(按ctrl键多选)的所选项转换为字符串数组
 *    注意：当前只能转成字符串数组
 * @return {*}
 */
$.fn.getValue = function () {
    var ret = {};

    this.find(':text, :password, textarea, select:not([multiple]),input[type=hidden]').each(function () {
        var $this = $(this);
        ret[$this.attr('name')] = $this.val();
    });

    this.find('input[type=number]').each(function () {
        var $this = $(this);
        ret[$this.attr('name')] = Number($this.val());
    });

    this.find(':radio').each(function () {
        var $this = $(this), name = $this.attr('name');
        $.type(ret[name]) === 'undefined' && (ret[name] = null);
        $this.is(':checked') && (ret[name] = $this.val());
    });

    this.find(':checkbox').each(function () {
        var $this = $(this), name = $this.attr('name');
        $.type(ret[name]) === 'undefined' && (ret[name] = null);
        ret[name] = $this.is(':checked');
    });
    return ret;
};

/**
* getValue的逆操作，设置符合类型的控件的值
* @param val
*/
$.fn.setValue = function (val) {

    this.find(':text, :password, textarea, select:not([multiple]),input[type=hidden]').each(function () {
        var $this = $(this), name = $this.attr('name');
        //(val[name] && typeof (val[name]) === 'string' || typeof (val[name]) === 'number') &&
        $this.val(val[name]);
    });

    this.find('input[type=number]').each(function () {
        var $this = $(this), name = $this.attr('name');
        //(val[name] && typeof (val[name]) === 'number') &&
        $this.val(val[name]);
    });

    this.find(':radio').each(function () {
        var $this = $(this), name = $this.attr('name');
        ($.type(val[name]) !== 'undefined' && $this.val() === String(val[name])) && $this.attr('checked', true);
    });

    this.find(':checkbox').each(function () {
        var $this = $(this), name = $this.attr('name');
        (val[name] && typeof (val[name]) == 'boolean') && $(this).attr('checked', val[name]);
    });
};
