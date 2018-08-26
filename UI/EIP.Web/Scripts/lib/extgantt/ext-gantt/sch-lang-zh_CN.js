/**
 * English translations for the Scheduler component
 *
 * NOTE: To change locale for month/day names you have to use the Ext JS language pack.
 */

Localize = function() {
    if (Sch.plugin) {
        if (Sch.plugin.SummaryColumn) {
            Ext.override(Sch.plugin.SummaryColumn, {
                dayText : 'd',
                hourText : 'h',
                minuteText : 'min'
            });
        }

        if (Sch.plugin.CurrentTimeLine) {
            Sch.plugin.CurrentTimeLine.prototype.tooltipText = '当前时间';
        }
    }
    
    var M = Sch.preset.Manager,
        vp = M.getPreset("hourAndDay");

    if (vp) {
        vp.displayDateFormat = 'g:i A';
        vp.headerConfig.middle.dateFormat = 'g A';
        vp.headerConfig.top.dateFormat = 'Y年m月d日';
    } 
    
    vp = M.getPreset("dayAndWeek");
    if (vp) {
        vp.displayDateFormat = 'm/d h:i A';
        vp.headerConfig.middle.dateFormat = 'Y年m月d日';
        vp.headerConfig.top.renderer = function(start, end, cfg) {
            var w = start.getWeekOfYear();
            return 'w.' + ((w < 10) ? '0' : '') + w + ' ' + Sch.util.Date.getShortMonthName(start.getMonth()) + ' ' + start.getFullYear();
        };
    } 

    vp = M.getPreset("weekAndDay");
    if (vp) {
        vp.displayDateFormat = 'm月d日';
        vp.headerConfig.bottom.dateFormat = 'm月d日';
        vp.headerConfig.middle.dateFormat = 'Y年m月d日';
    }
    
    vp = M.getPreset("weekAndDayLetter");
    if (vp) {
        vp.displayDateFormat = 'm月d日';
        vp.headerConfig.bottom.dateFormat = 'M月d日';
        vp.headerConfig.middle.dateFormat = 'Y年m月d日';
    }

    vp = M.getPreset("weekAndMonth");
    if (vp) {
        vp.displayDateFormat = 'Y年m月d日';
        vp.headerConfig.middle.dateFormat = 'm月d日';
        vp.headerConfig.top.dateFormat = 'Y年m月d日';
    } 

    vp = M.getPreset("monthAndYear");
    if (vp) {
        vp.displayDateFormat = 'Y年m月d日';
        vp.headerConfig.middle.dateFormat = 'Y年m月';
        vp.headerConfig.top.dateFormat = 'Y年';
    } 

    vp = M.getPreset("year");
    if (vp.year) {
        vp.displayDateFormat = 'Y年m月d日';
        vp.headerConfig.bottom.renderer = function(start, end, cfg) {
            return Ext.String.format('{0}季度', Math.floor(start.getMonth() / 3) + 1);
        };
        vp.headerConfig.middle.dateFormat = 'Y年';
    } 
};