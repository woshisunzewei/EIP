/*
* Author:antianlu
* Date:2012-04-21
* Plugin name:jQuery.Contextmenu
* Address：http://www.oschina.net/code/snippet_153403_9880
* Version:0.2
* Email:atlatl333@126.com
*/
(function(cm){
	jQuery.fn.WinContextMenu=function(options){
		var defaults={
			offsetX:2,//鼠标在X轴偏移量
			offsetY:2,//鼠标在Y轴偏移量
			speed:300,//特效速度
			flash:!1,//特效是否开启，默认不开启
			flashMode:'',//特效模式,与flash为真时使用
			cancel:!1,//排除不出现右键菜单区域
			items:[],//菜单项
			action:$.noop()//自由菜单项回到事件
		};
		var opt=cm.extend(true,defaults,options);
		function create(e){
			var m=cm('<ul class="WincontextMenu"></ul>').appendTo(document.body);
			cm.each(opt.items,function(i,itm){
				if(itm){
					var row=cm('<li><a class="'+(itm.disable?'cmDisable':'')+'" ref="sitem" href="javascript:void(0)"><span></span></a></li>').appendTo(m);
					itm.icon ? cm('<img src="/Contents/images/icons/' + itm.icon + '.png">').insertBefore(row.find('span')) : '';
					itm.text?row.find('span').text(itm.text):'';
					if(itm.action) {
						row.find('a').click(function(){this.className!='cmDisable'?itm.action(e):null;});}
				}
			});
			if(cm('#WincontextMenu').html()!=null){
				cm(cm('#WincontextMenu').html().replace(/#/g,'javascript:void(0)')).appendTo(m);}
			return m;
		}
		if(opt.cancel){//排除不出现右键菜单区域
				cm(opt.cancel).live('contextmenu',function(e){return false});}
		this.live('contextmenu',function(e){
			var m=create(e).show();
			var l = e.pageX + opt.offsetX,
			t = e.pageY+opt.offsetY,
			p={
				wh:cm(window).height(),
				ww:cm(window).width(),
				mh:m.height(),
				mw:m.width()
			}
			t=(t+p.mh)>=p.wh?(t-=p.mh):t;//当菜单超出窗口边界时处理
			l=(l+p.mw)>=p.ww?(l-=p.mw):l;
			m.css({zIndex:1000001, left:l, top:t}).live('contextmenu', function() { return false; });
			m.find('a').click(function(e){//呼叫新从页面增加的菜单项
				var b=$(this).attr('ref');
			    if(b!='sitem'){this.className!='cmDisable'?opt.action(this):null;}
				e.preventDefault();
			});
			cm(document.body).live('contextmenu click', function() {//防止有动态加载的标签失效问题
			  m.remove();
			});
			return false;
		});
		return this;
	}
})(jQuery);