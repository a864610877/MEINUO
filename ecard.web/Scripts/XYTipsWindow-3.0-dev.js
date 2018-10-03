/*
*Name:弹出层
*Date:2012年04月25日18:02:00
*/
if (typeof Util == "undefined") var Util = {};
var loadingICO = "style/loading.gif";
Util.getID = function (id){
	return "string" == typeof id ? document.getElementById(id) : id;
};
Util.getTag = function (tag) {
	return document.getElementsByTagName(tag);
};
Util.ce = function (name){
	return document.createElement(name);
};
Util.getStyle = function(element){
	return element.currentStyle || document.defaultView.getComputedStyle(element, null);
};
Util.stopBubble = function(e){
	e.stopPropagation ? e.stopPropagation() : e.cancelBubble = true;
};
Util.stopDefault = function(e){
	e.preventDefault ? e.preventDefault() : e.returnValue = false;
};
Util.bind = function (obj, type, fn) {
	if (obj.attachEvent) {
		obj['e' + type + fn] = fn;
		obj[type + fn] = function(){
			obj['e' + type + fn](window.event);
		}
		obj.attachEvent('on' + type, obj[type + fn]);
	}
	else {
		obj.addEventListener(type, fn, false);
	};
};
Util.hasClass = function(ele, cls) {
	return ele.className.match(new RegExp('(\\s|^)'+cls+'(\\s|$)'));
};
Util.addClass = function(ele, cls) {
	if (!this.hasClass(ele,cls)) ele.className += " "+cls;
};
Util.removeClass = function (ele, cls) {
	if (hasClass(ele, cls)) {
		var reg = new RegExp('(\\s|^)'+cls+'(\\s|$)');
		ele.className = ele.className.replace(reg,' ');
	};
};
Util.getClassName = function(ele, name){
	var ele = Util.getTag(ele);
	var arr = [];
	for (var i=0;i<ele.length;i++){
		if (ele[i].className == name){
			arr.push(ele[i]);
		};
	};
	return arr;
};
Util.Browser = function(a) {
	var b = {};
	b.isStrict = document.compatMode == "CSS1Compat";
	b.isFirefox = a.indexOf("firefox") > -1;
	b.isOpera = a.indexOf("opera") > -1;
	b.isSafari = (/webkit|khtml/).test(a);
	b.isSafari3 = b.isSafari && a.indexOf("webkit/5") != -1;
	b.isIE = !b.isOpera && a.indexOf("msie") > -1;
	b.isIE6 = !b.isOpera && a.indexOf("msie 6") > -1;
	b.isIE7 = !b.isOpera && a.indexOf("msie 7") > -1;
	b.isIE8 = !b.isOpera && a.indexOf("msie 8") > -1;
	b.isGecko = !b.isSafari && a.indexOf("gecko") > -1;
	b.isMozilla = document.all != undefined && document.getElementById != undefined && !window.opera != undefined;
	return b;
}(navigator.userAgent.toLowerCase());
Util.pageSize = {
	get: function() {
		var a = Util.Browser.isStrict ? document.documentElement: document.body;
		var b = ["clientWidth", "clientHeight", "scrollWidth", "scrollHeight"];
		var c = {};
		for (var d in b) c[b[d]] = a[b[d]];
		c.scrollLeft = document.body.scrollLeft || document.documentElement.scrollLeft;
		c.scrollTop = document.body.scrollTop || document.documentElement.scrollTop;
		return c;
	}
};
Util.getPosition = function (obj) {
	if(typeof(obj)== "string") obj = Util.getID(obj);
	var c = 0;
	var d = 0;
	var w = obj.offsetWidth;
	var h = obj.offsetHeight;
	do {
		d += obj.offsetTop || 0;
		c += obj.offsetLeft || 0;
		obj = obj.offsetParent
	
	}
	while (obj)return {
		x: c,
		y: d,
		width: w,
		height: h
	
	};
};
Util.safeRange = function( obj ){
	var b = Util.getID(obj);
	var c, d, e, f, g, h, j, k;
	var s =Util.pageSize.get();
	j = b.offsetWidth;
	k = b.offsetHeight;
	p = Util.pageSize.get();
	c = 0;
	e = p.clientWidth - j;
	g = e/2;
	d = 0;
	f = p.clientHeight - k;
	var hc =  p.clientHeight * 0.382 - k/2;
	h = (k < p.clientHeight / 2) ? hc : f/2;
	if (g < 0) g = 0;
	if (h < 0) h = 0;
	return {
		width: j, height: k,minX: c, minY: d, maxX: e, maxY: f, centerX: g, centerY: h
	};
};
Util.setXY = function(obj, position, referID, fixed){
	var	p = Util.pageSize.get(), o = Util.safeRange(obj), D = Util.getID(obj);
	if (referID) {
		s = Util.safeRange(referID);
		rp = Util.getPosition(referID);
	}
	var _this = position, st = fixed === true ? 0 : p.scrollTop;
	if (referID != undefined && referID!="") {
		var left = !_this.right ? parseInt(_this.left) : p.clientWidth - s.width - parseInt(_this.right);
		var top = !_this.bottom ? parseInt(_this.top) : p.clientHeight - s.height - parseInt(_this.bottom);
		left1  =	rp.x + parseInt(_this.left);//inside
		left2 =	rp.x + parseInt(_this.left) + s.width;//outside
		right1  =	rp.x + s.width - o.width - parseInt(_this.right);//inside
		right2 =	rp.x - o.width - parseInt(_this.right);//outside
		top1 =	rp.y + parseInt(_this.top);//inside
		top2 =	rp.y + parseInt(_this.top) + s.height;//outside
		bottom1 =	rp.y + s.height - o.height - parseInt(_this.bottom);//inside
		bottom2 = rp.y - o.height - parseInt(_this.bottom);//outside
		left = !_this.right ? (_this.lin ? left1 : left2) : (_this.rin ? right1 : right2);
		top = !_this.bottom ? (_this.tin ? top1 : top2) : (_this.bin ? bottom1 : bottom2);
		D.style.left = left + "px";
		D.style.top = top + "px";
	}
	else{
		if (!_this.left&&!_this.right){
			D.style.left = o.centerX + "px";
		}
		else{
			if (!_this.right){
				D.style.left = parseInt(_this.left) + "px";
			}
			else{
				D.style.right = parseInt(_this.right) + "px";
			};
		};
		if (!_this.top&&!_this.bottom){
			D.style.top = o.centerY + st + "px";
		}
		else{
			if (!_this.bottom){
				D.style.top = parseInt(_this.top) + st + "px";
			}
			else{
				D.style.top = p.clientHeight - D.offsetHeight - parseInt(_this.bottom) + "px";
			};
		};
	};
};
Array.prototype.max = function(){
	return Math.max.apply({},this);
};
Array.prototype.indexOf = function(val) {
	for (var i = 0;i < this.length;i++) {
		if (this[i] == val) return i;
	};
	return -1;
};
Array.prototype.remove = function(val) {
	var index = this.indexOf(val);
	if (index > -1) {
		this.splice(index, 1);
	};
};
Util.addCSS = function( val ){
	var b = this.style;
	if(!b){
		b = this.style = document.createElement('style');
		b.setAttribute('type', 'text/css');
		document.getElementsByTagName('head')[0].appendChild(b);
	};
	b.styleSheet && (b.styleSheet.cssText += val) || b.appendChild(document.createTextNode(val));
};
Util.random = function( length, upper, lower, number ){
	if( !upper && !lower && !number ){
		upper = lower = number = true;
	};
	var a = [
	["A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"],
	["a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"],
	["0","1","2","3","4","5","6","7","8","9"]
	];
	var b = [];
	var c = "";
	b = upper ? b.concat(a[0]) : b;
	b = lower ? b.concat(a[1]) : b;
	b = number ? b.concat(a[2]) : b;
	for (var i=0;i<length;i++){
		c += b[ Math.round(Math.random()*(b.length-1)) ];
	};
	return c;
};
Util.fixed = function( obj ){
	var o = Util.getID(obj);
	if (!Util.Browser.isIE6) {
		o.style.position = "fixed";
	}
	else{
		var d = Util.getClassName("div", "ui_dialog_fixed");
		if (Util.getStyle(Util.getID("page"))["backgroundImage"]!="none"){
			Util.addCSS(".ui_dialog_fixed{width:100%; height:1px; position:absolute; z-index: 891201; left:expression(documentElement.scrollLeft+documentElement.clientWidth-this.offsetWidth); top:expression(documentElement.scrollTop)}.body-fixed{background-attachment:fixed;}");
		}
		else{
			Util.addCSS(".ui_dialog_fixed{width:100%; height:1px; position:absolute; z-index: 891201; left:expression(documentElement.scrollLeft+documentElement.clientWidth-this.offsetWidth); top:expression(documentElement.scrollTop)}.body-fixed{background-attachment:fixed;background-image:url(about:blank);}");
		};
		if(d.length == 0){
			var wrap = Util.ce("div");
			wrap.className = 'ui_dialog_fixed';
			wrap.appendChild(o);
			document.body.appendChild(wrap);
			Util.addClass(Util.getTag("html")[0],"body-fixed");
		}
		else{
			d[0].appendChild(o);
		};
	};
};
Util.callBack = {
	ok:function(text){
		return "<div class='ui_box_callback'><span class='ui_box_callback_ok'></span>"+text+"</div>";
	}
	,
	error:function(text){
		return "<div class='ui_box_callback'><span class='ui_box_callback_error'></span>"+text+"</div>";
	}
	,
	tips:function(text){
		return "<div class='ui_box_callback'><span class='ui_box_callback_tips'></span>"+text+"</div>";
	}
};
Util.Imgready = function () {
	var list = [],intervalId = null,
	tick = function () {
		var i = 0;
		for (;i < list.length;i++) {
			list[i].end ? list.splice(i--, 1) : list[i]();
		};
		!list.length && stop();
	}
	,
	stop = function () {
		clearInterval(intervalId);
		intervalId = null;
	};
	return function (url, ready, load, error) {
		var onready, width, height, newWidth, newHeight,
		img = new Image();
		img.src = url;
		if (img.complete) {
			ready.call(img);
			load && load.call(img);
			return;
		};
		width = img.width;
		height = img.height;
		img.onerror = function () {
			error && error.call(img);
			onready.end = true;
			img = img.onload = img.onerror = null;
		};
		onready = function () {
			newWidth = img.width;
			newHeight = img.height;
			if (newWidth !== width || newHeight !== height ||newWidth * newHeight > 1024) {
				ready.call(img);
				onready.end = true;
			};
		};
		onready();
		img.onload = function () {
			!onready.end && onready();
			load && load.call(img);
			img = img.onload = img.onerror = null;
		};
		if (!onready.end) {
			list.push(onready);
			if (intervalId === null) intervalId = setInterval(tick, 40);
		};
	};
}();
(function($){
	Util.drag = function (o) {
		var defaults = {
			obj: "", 
			handle: "", 
			lock: true, 
			lockX: false, 
			lockY: false,
			fixed: false, 
			parent: "", 
			sfns: function () {
			}
			,
			mfns: function () {
			}
			,
			ofns: function () {
			}
		};
		var o = $.extend(defaults, o);
		var drag = false;
		var safe = Util.safeRange(o.obj);
		var s = Util.pageSize.get();
		var $box = $("#" + o.obj);
		var moveX = 0, moveY = 0, _x, _y;
		if (o.fixed) {
			if (o.parent != "") {
				o.parent = "";
			}
		};
		if (o.parent != "") {
			$("#" + o.parent).css("position", "relative");
		};
		if (o.handle != "") {
			$Handle = $(o.handle, $box);
		}
		else {
			$Handle = $box;
		};
		$Handle.css("cursor", "move");
		$Handle.mousedown(function (ev) {
			star(ev);
			this.setCapture();
		});
		var star = function (ev) {
			drag = true;
			if (o.sfns != "" && $.isFunction(o.sfns)) {
				o.sfns(this);
			};
			ev = ev || window.event;
			ev.preventDefault();
			p = Util.getPosition(o.obj);
			ny = o.fixed ? Util.Browser.isIE6 ? s.scrollTop : 0 : 0;
			moveX = ev.clientX - p.x;
			moveY = ev.clientY - p.y + ny;
			$(document).bind("mousemove", function (ev) {
				move(ev) 
			});
			$(document).bind("mouseup", function () {
				stop() 
			});
		};
		var move = function (ev) {
			var parent;
			ev = ev || window.event;
			window.getSelection ? window.getSelection().removeAllRanges() : document.selection.empty();//阻止浏览器默认选取
			_x = ev.clientX - moveX;
			_y = ev.clientY - moveY;
			if (o.parent != "") {
				parent = Util.getPosition(o.parent);
				op = Util.getPosition(o.obj);
				_x = ev.clientX - moveX - parent.x ;
				_y = ev.clientY - moveY - parent.y ;
			};
			maxX = o.parent != "" ? parent.width - op.width : safe.maxX;
			maxY = o.parent != "" ? parent.height - op.height : safe.maxY;
			if (o.lockX) {
				_y = p.y;
			};
			if (o.lockY) {
				_x = p.x;
			};
			if (o.lock) {
				if (_x <= 0) _x = safe.minX;
				if (_x >= maxX) {
					_x = maxX;
				}
				if (o.fixed){
					if (_y <= 0) _y = safe.minY;
					if (_y >= maxY) {
						_y = maxY;
					}
				}
				else{
					if ( _y > maxY+s.scrollTop) {
						_y = maxY+s.scrollTop;
					}
					if ( _y < s.scrollTop){
						_y = s.scrollTop;
					}
				};
			};
			$box.css({
				left: _x + "px",
				top: _y + "px",
				right: "auto",
				bottom: "auto",
				margin: "auto"
			
			});
			if (o.mfns != "" && $.isFunction(o.mfns)) {
				o.mfns(this);
			};
		};
		var stop = function () {
			drag = false;
			$(document).unbind("mousemove");
			if (o.ofns != "" && $.isFunction(o.ofns)) {
				o.ofns(this);
			};
			document.releaseCapture();
		};
	};
	
	Util.Dialog = function(o) {
		defaults = $.extend({
			type: "dialog",//弹窗类型
			title : "",//窗口标题文字;
			boxID : Util.random(10),//弹出层ID;
			referID : "", //相对于这个ID的位置进行定位
			content : "text:内容",//内容(可选内容为){ text | img | grally | swf | url | iframe};
			width : "",//窗口宽度;
			height : "",//窗口高度;
			time : "",//自动关闭等待时间;(单位秒);
			drag: true,//是否启用拖动( 默认为启用);
			lock: true, //是否限制拖动范围；
			fixed: false,//是否开启固定定位;
			showbg : false,//是否显示遮罩层( 默认为false);
			showtitle: true,//是否显示弹出层标题( 默认为显示);
			border: {}, //边框
			arrow: "left",//箭头方向
			arrowset: {
				val: "50%", style: "default", auto: "true"
			},//提示层设置（val => 箭头偏移量 | style => 提示层风格 | auto => 提示层位置自适应）
			closestyle : "white", //关闭按钮风格，五种颜色可选（gray | black | red | white | orange）；
			button : "",//数组，要显示按钮的文字;
			callback : "",//按钮回调函数，默认返回所选按钮显示的文 ;
			position : "",//设定弹出层位置,默认居中;
			ofns : ""//弹出窗打开时后执行的函数;
		}
		,o);
		Util.Dialog.init(defaults);
	};
	$.extend(Util.Dialog,{
		Dialogarr: new Array(),//窗口数组
		zindex: 870618,//初始index值
		init : function(o) {//初始化
			var $box = $("#"+o.boxID);
			$(".ui_close", $box).live("click",function(){
				Util.Dialog.remove(o.boxID);
				return false;
			});
			Util.Dialog.createWindows(o);
			Util.Dialog.loadContent(o);
			if (o.button!=""){
				Util.Dialog.dialog(o);
			};
			if (typeof o.time === 'number'){
				setTimeout(function(){
					Util.Dialog.remove(o.boxID);
				}
				, o.time);
			};
			$(window).resize(function(){
				Util.setXY(o.boxID, o.position, o.referID, o.fixed);
			});
			if (o.fixed) {
				Util.fixed(o.boxID);
			};
			if(o.showbg != "" && o.showbg == true){
				var $boxBgDom = "<div id=\"XYTipsWindowBg\" style=\"position:absolute;background:#000;filter:alpha(opacity=50);opacity:0.5;width:100%;left:0;top:0;z-index:870618;\"><iframe src=\"about:blank\" style=\"width:100%;height:"+$(document).height()+"px;filter:alpha(opacity=50);opacity:0.5;scrolling=no;border:none;z-index:870611;\"></iframe></div>";
				$($boxBgDom).appendTo("body").animate({
					opacity:0.5
				}
				,200);
			};
			Util.Dialog.setDialogIndex(o);
			Util.Dialog.Dialogarr.push([o.boxID, Util.getID(o.boxID).style.zIndex ]);
		}
		,
		createWindows:function(o){
			var boxDom="<div id=\""+o.boxID+"\" class=\"ui_dialog\">";
			boxDom+="<table class=\"ui_table_wrap\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>";
			boxDom+="<tr><td class=\"ui_border ui_td_00\"></td><td class=\"ui_border ui_td_01\"></td><td class=\"ui_border ui_td_02\"></td></tr>";
			boxDom+="<tr><td class=\"ui_border ui_td_10\"></td><td class=\"ui_td_11\"><table class=\"ui_dialog_main\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>";
			boxDom+="<tr><td><div class=\"ui_title_wrap\"><div class=\"ui_title\"><div class=\"ui_title_text\"><span class=\"ui_title_icon\"></span>标题</div><span class=\"ui_close\">x</span></div></div></td></tr>";
			boxDom+="<tr><td class=\"ui_content_wrap\">";
			boxDom+="<div class=\"ui_content\" id=\""+o.boxID+"_content\"></div>";
			boxDom+="</td></tr>";
			boxDom+="<tr><td class=\"ui_button_wrap\"><div class=\"ui_button_box\"><div class=\"ui_btns\"></div><div class=\"ui_resize\"></div></div></td></tr></tbody></table>";
			boxDom+="</td><td class=\"ui_border ui_td_12\"></td></tr>";
			boxDom+="<tr><td class=\"ui_border ui_td_20\"></td><td class=\"ui_border ui_td_21\"></td><td class=\"ui_border ui_td_22\"></td></tr></tbody></table>";
			boxDom+="<iframe src=\"about:blank\" class=\"ui_iframe\" style=\"position:absolute;left:0;top:0; filter:alpha(opacity=0);opacity:0; scrolling=no;border:none;z-index:10714;\"></iframe>";
			boxDom+="</div>";
			if($("#"+o.boxID).length==0){
				$(boxDom).appendTo("body")
			};
			var $box=$("#"+o.boxID);
			$box.find(".ui_title_text").html("<span class=ui_title_icon></span>"+o.title);
			if(o.type=="tips"){
				o.showtitle="remove";
				o.showbg=false;
				o.drag=false;
				o.resize=false;
				o.border={
					opacity:"0"
				}
			};
			if(o.showtitle!=true){
				$(".ui_title_wrap",$box).remove()
			};
			if(o.border!=null){
				var br=o.border;
				br.width=br.width||"1px";
				br.style=br.style||"solid";
				br.color=br.color||"#555";
				br.backgroundColor=br.backgroundColor||"#FFF";
				br.opacity=br.opacity||0.2;
				br.radius=br.radius||0;
				if(br.opacity!=null&&br.opacity==0){
					$box.find(".ui_border").css({
						width:"0px",height:"0px",fontSize:"0",lineHeight:"0",visibility:"hidden",overflow:"hidden"
					});
					$box.find(".ui_resize").css({
						right:"5px",bottom:"5px"
					});
					if(o.type=="dialog"){
						$box.find(".ui_dialog_main").addClass("ui_box_shadow")
					}
				};
				$box.find(".ui_border").css({
					opacity:br.opacity
				});
				if(o.type=="tips"){
					o.arrowset.style=o.arrowset.style||"default";
					$box.find(".ui_content").css({
						borderWidth:br.width
					}).addClass("ui_tips_style_"+o.arrowset.style);
					Util.addCSS(".ui_content{border-radius:"+br.radius+"px;}");
				}
				else{
					$box.find(".ui_dialog_main").css({
						borderWidth:br.width,borderStyle:br.style,borderColor:br.color,backgroundColor:br.backgroundColor
					});
					$box.find(".ui_title_text").css({
						borderWidth:"0 0 1px 0",borderColor:br.color
					})
				}
			};
			Util.Dialog.setPosition(o)
		}
		,
		loadContent: function (o) {
			var $box = $("#"+o.boxID);
			var $contentID = $(".ui_content",$box);
			$contentType = o.content.substring(0,o.content.indexOf(":"));
			$content = o.content.substring(o.content.indexOf(":")+1,o.content.length);
			$.ajaxSetup({
				global: false
			});
			switch($contentType) {
				case "text":
				$box.find(".ui_content").css({
					padding: "0",
					width:o.width,
					height:o.height
				
				});
				$contentID.html($content);
				Util.Dialog.setPosition(o);
				if(o.ofns != "" && $.isFunction(o.ofns)) {
					o.ofns(this);
				};
				break;
				case "img":
				$.ajax({
					beforeSend:function() {
						$contentID.html("<img src='"+loadingICO+"' class='ui_box_loading' alt='加载中...' />");
					}
					,
					error:function(){
						$contentID.html("<p class='ui_box_error'><span class='ui_box_callback_error'></span>加载数据出错！</p>");
						Util.Dialog.setPosition(o);
					}
					,
					success:function(html){
						Util.Imgready($content, function(){
							$box.find(".ui_content").css({
								padding: "0",
								width: this.width,
								height: this.height
							
							});
							$contentID.html("<img src="+$content+" alt='' />");
							Util.Dialog.setPosition(o);
						});
						if(o.ofns != "" && $.isFunction(o.ofns)) {
							o.ofns(this);
						};
					}
				});
				break;
				case "swf":
				$.ajax({
					beforeSend:function() {
						$contentID.html("<img src='"+loadingICO+"' class='ui_box_loading' alt='加载中...' />");
					}
					,
					error:function(){
						$contentID.html("<p class='ui_box_error'><span class='ui_box_callback_error'></span>加载数据出错！</p>");
						Util.Dialog.setPosition(o);
					}
					,
					success:function(html){
						$box.find(".ui_content").css({
							padding: "0",
							width:o.width,
							height:o.height
						
						});
						$contentID.html("<div id='"+o.boxID+"swf'><h1>Alternative content</h1><p><a href=\"http://www.adobe.com/go/getflashplayer\"><img src=\"http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif\" alt=\"Get Adobe Flash player\" /></a></p></div><script type=\"text/javascript\">swfobject.embedSWF('"+$content+"', '"+o.boxID+"swf', '"+o.width+"', '"+o.height+"', '9.0.0', 'expressInstall.swf');</script>");
						$("#"+o.boxID+"swf").css({
							position:"absolute",
							left:"0",
							top:"0",
							textAlign:"center"
						
						});
						Util.Dialog.setPosition(o);
						if(o.ofns != "" && $.isFunction(o.ofns)) {
							o.ofns(this);
						};
					}
				});
				break;
				case "url":
				var contentDate=$content.split("?");
				$.ajax({
					beforeSend:function() {
						$contentID.html("<img src='"+loadingICO+"' class='ui_box_loading' alt='加载中...' />");
					}
					,
					type:contentDate[0],
					url:contentDate[1],
					data:contentDate[2],
					error:function(){
						$contentID.html("<p class='ui_box_error'><span class='ui_box_callback_error'></span>加载数据出错！</p>");
						Util.Dialog.setPosition(o);
					}
					,
					success:function(html){
						$box.find(".ui_content").css({
							padding: "0",
							width:o.width,
							height:o.height
						
						});
						$contentID.html(html);
						Util.Dialog.setPosition(o);
						if(o.ofns != "" && $.isFunction(o.ofns)) {
							o.ofns(this);
						};
					}
				});
				//Util.Dialog.setPosition(o);
				break;
				case"iframe":$contentID.css({
					overflowY:"hidden"
				});
				$.ajax({
					beforeSend:function(){
						$contentID.html("<img src='"+loadingICO+"' class='ui_box_loading' alt='加载中...' />")
					}
					,error:function(){
						$contentID.html("<p class='ui_box_error'><span class='ui_box_callback_error'></span>加载数据出错！</p>");
						Util.Dialog.setPosition(o)
					}
					,success:function(html){
						$box.find(".ui_content").css({
							padding:"0",width:o.width,height:o.height
						});
						$contentID.html("<iframe src=\""+$content+"\" id=\""+o.boxID+"frame\" scrolling=\"auto\" frameborder=\"0\" marginheight=\"0\" marginwidth=\"0\"></iframe>");
						$("#"+o.boxID+"frame").bind("load",function(){
							window.setInterval(fun,100);
							setTimeout(function(){
								Util.Dialog.setPosition(o)
							}
							,100);
							if(o.ofns!=""&&$.isFunction(o.ofns)){
								o.ofns(this)
							}
						});
						var fun=function(){
							var f=document.getElementById(o.boxID+"frame");
							try{
								var frame=f.contentWindow.document,w=Math.max(frame.body.scrollWidth,frame.documentElement.scrollWidth);
								h=Math.max(frame.body.scrollHeight,frame.documentElement.scrollHeight);
								var sw=h!=f.height?17:0;
								if(w!=f.width){
									f.width=w+sw
								};
								if(h!=f.height){
									f.height=h
								};
								$box.css({
									padding:"0",width:w+18,height:h+18
								})
							}
							catch(e){}
						}
					}
				});
			};
			if (o.type=="tips"){
				var tipsarrow = "<div class=\"ui_arrow ui_arrow_mode_"+o.arrow+"\"><em>◆</em><span>◆</span></div>";
				var tipsclose = "<span class=\"ui_close\" style=\"right:5px;top:5px;\">x</span>";
				$contentID.append(tipsarrow).append(tipsclose).css({
					padding: "5px 15px 5px 5px",
					textAlign: "left"
				
				});
				var $arrow = $box.find(".ui_arrow");
				var mode = o.arrow == "left" || o.arrow == "right" ? "top" : "left";
				$arrow.css(mode, o.arrowset.val);
				Util.Dialog.setPosition(o);
			};
			$box.find(".ui_close").addClass("ui_close_ico_"+o.closestyle);
		}
		,
		setPosition : function(o){//设定弹出层大小及位置
			Util.setXY(o.boxID, o.position, o.referID, o.fixed);
			var	safe = Util.safeRange(o.boxID),
			$box = $("#"+o.boxID);
			var $iframe = $(".ui_iframe",$box);
			$iframe.css({
				width:safe.width+"px",
				height:safe.height+"px"
			
			});
			if (o.showbg == true){
				$box.css("zIndex", "870620");
			};
			if (o.type=="tips"){
				var ob = Util.getPosition(o.boxID), rp = Util.getPosition(o.referID),s = Util.safeRange(o.referID);
				var st = document.body.scrollTop || document.documentElement.scrollTop;
				o.arrowset.auto = o.arrowset.auto || "true";
				if (o.arrow=="left"){
					$box.css({
						left: ob.x + 8 +"px",
						top: ob.y +"px"
					
					});
					if (o.arrowset.auto == "true" && p.clientWidth - ob.x < $box.outerWidth()){
						$box.css({
							left: rp.x - $box.outerWidth() - 8
						
						}).find(".ui_arrow").removeClass("ui_arrow_mode_left").addClass("ui_arrow_mode_right");
					};
				};
				if (o.arrow=="right"){
					$box.css({
						left: ob.x - 10 +"px",
						top: ob.y +"px"
					
					});
					if (o.arrowset.auto == "true" && ob.x < 0){
						$box.css({
							left: rp.x + s.width + 8
						
						}).find(".ui_arrow").removeClass("ui_arrow_mode_right").addClass("ui_arrow_mode_left");
					};
				};
				if (o.arrow=="bottom"){
					$box.css({
						left: ob.x +"px",
						top: ob.y - 8 +"px"
					
					});
					if (o.arrowset.auto == "true" && ob.y < 0){
						$box.css({
							top: rp.y + s.height + 8
						
						}).find(".ui_arrow").removeClass("ui_arrow_mode_bottom").addClass("ui_arrow_mode_top");
					};
				};
				if (o.arrow=="top" ){
					$box.css({
						left: ob.x + "px",
						top: ob.y + 8 +"px"
					
					});
					if (o.arrowset.auto == "true" && p.clientHeight - ob.y + st < $box.outerHeight()){
						$box.css({
							top: rp.y - $box.outerHeight() - 8
						
						}).find(".ui_arrow").removeClass("ui_arrow_mode_top").addClass("ui_arrow_mode_bottom");
					};
				};
			};
			if (o.drag) {
				$(".ui_title_wrap",$box).die().live("mouseover",function(){
					Util.drag({
						obj: o.boxID,
						handle:".ui_title_text",
						lock : o.lock,
						fixed: o.fixed
					
					});
				});
			};
		}
		,
		dialog:function(o){//对话框模式
			var $box = $("#"+o.boxID);
			if (o.button!=""){
				var map = {}, answerStrings = [];
				if (o.button instanceof Array) {
					for (var i = 0;i < o.button.length;i++) {
						map[o.button[i]] = o.button[i];
						answerStrings.push(o.button[i]);
					};
				}
				else {
					for (var k in o.button) {
						map[o.button[k]] = k;
						answerStrings.push(o.button[k]);
					};
				};
				var showButton = function(){
					if (answerStrings.length>1) {
						return "<input class='ui_button ui_box_btn' type='button'  value='" + answerStrings[0] + "'  /> <input class='ui_button ui_box_btn2' type='button'  value='" + answerStrings[1] + "'  />";
					}
					else{
						return "<input class='ui_button ui_box_btn' type='button'  value='" + answerStrings[0] + "'  />";
					};
				}
				$(".ui_btns",$box).show().html(showButton());
							$(".ui_box_btn").click(function() {
					var _this = $(this);
					if(o.callback != "" && $.isFunction(o.callback)) {
						o.callback(this);
						if (!closeDialog) return false;
						Util.Dialog.remove(o.boxID);
					};
				});
				$(".ui_box_btn2").click(function() {
					Util.Dialog.remove(o.boxID);
				});
			};
		}
		,
		setDialogIndex: function (o){
			var $box = $("#"+o.boxID);
			var ___event = "mousedown" || "mousemove" || "mouseup" || "click";
			Util.Dialog.zindex += 1;
			$box.css("zIndex", Util.Dialog.zindex);
			$box.live(___event,function(){
				this.style.zIndex = Util.Dialog.zindex += 1;
				for(var i=0;i<Util.Dialog.Dialogarr.length;i++){
					if (Util.Dialog.Dialogarr[i][0]==o.boxID){
						Util.Dialog.Dialogarr.remove(Util.Dialog.Dialogarr[i]);
					};
				};
				Util.Dialog.Dialogarr.push([o.boxID, this.style.zIndex ]);//更新index值
			
			});
		}
		,
		setDialogarr : function(){
			var z = new Array();
			for(var i=0;i<Util.Dialog.Dialogarr.length;i++){
				z.push(Util.Dialog.Dialogarr[i][1]);
			};
			for (var j=0;j<z.length;j++ ){
				if (Util.Dialog.Dialogarr[j][1]==z.max()){
					var o = Util.Dialog.Dialogarr[j][0];
					Util.Dialog.remove(o);
					z.remove(z.max());
					Util.Dialog.Dialogarr.remove(Util.Dialog.Dialogarr[j]);
				};
			};
		}
		,
		remove: function (obj, callback){
			var $box = $("#"+obj);
			var $boxbg = $("#XYTipsWindowBg");
			if($box.length != 0 || $boxbg.length != 0){
				$box.remove();
				$boxbg.animate({
					opacity:"0"
				}
				,100,function(){
					$(this).remove();
				});
				if(callback != "" && $.isFunction(callback)) {
					callback(this);
				};
			};
		}
	});
	
	document.onkeydown = function(e){
		e = e || window.event;
		if (e.keyCode==27){
			Util.Dialog.setDialogarr();
		};
	};
})(jQuery)