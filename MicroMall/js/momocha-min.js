//获取手机的分辨率
var appwidth = window.screen.width;
var appheight = window.screen.height;
var mbar = $('.momocha-bar'); //bar标签
var msearch = $('.bar-search'); //搜索栏
var mindex = $('.momocha-index');
var msidebar = $('.momocha-sidebar');
var mblack = $('.momocha-black');
//alert(appwidth+','+appheight );

//全局自适应
function fit() {
	var a = (appwidth - 20) + 'px'; //可用容器内容宽度	
	var b = (appwidth - 20 - 120) + 'px'; //搜索栏内容宽度
	var c = (appheight) + 'px';
	var d = (appwidth - 30) * 0.5 + 'px';
	var e = (appwidth - 30) * 0.5;
	var e = e * (150 / 275) + 'px';
	var f = (appwidth - 30) * 0.5;
	var f1 = f + 'px';
	var f2 = f * 1.2 + 'px';
	var f3 = f - 10;
	var f3 = f3 + 'px';
	var g = (f - 10) + 'px';
	var h = appwidth * 2 + 'px';
	var i = appwidth + 'px';
	var j = (appwidth - 20) * 0.5;
	var j = j + 'px';
	var l = appwidth + 'px';
	var aa = appwidth - 20 - 30 - 90;
	var aa = aa + 'px';
	var bb = appwidth - 10 - 50 - 10 - 10 - 80;
	var bb = bb + 'px';
	var cc = appwidth - 20 - 80 - 10;
	var cc = cc + 'px';
	var dd = appwidth - 20 - 40;
	var dd = dd + 'px';

	mbar.css('width', a);
	msearch.css('width', b);
	msidebar.css('height', c);
	$('.momocha-screen').css('height', c);
	$('.momocha-black').css('height', c);
	$('.ddzf-black').css('height', c);
	
	$('.momocha-Appinterval').css('width', a);
	$('.momocha-shop').css('width', a);
	$('.momocha-shop ul li').css('width', d)
	$('.momocha-shop ul li img').css('width', d)
	$('.momocha-shop ul li img').css('height', d)
	$('.momocha-shop ul li a p').css('width', d)
	$('.bar-litle').css('width', b);
	$('.momocha-spnr .mleft').css('width', a);
	$('.momocha-spnr .mleft a').css('width', f1);
	$('.momocha-spnr .mleft a img').css('width', f1);
	$('.momocha-spnr .mleft a img').css('height', f1);
	$('.momocha-spnr .mleft a span').css('width', g);
	$('.momocha-spnr').css('width', h);
	var mleft = $('.mleft').height();
	var mleft = mleft + 'px';
	$('.momocha-spnr').css('height', mleft);
	$('.momocha-spnr .mright').css('width', i);
	$('.momocha-spnr .mright span').css('width', a);
	$('.momocha-spnr .mright span img').css('height', a);
	$('.shousuo .shousuo-nr').css('width', i);
	$('.shousuo .shousuo-nr span').css('width', a);
	$('.shousuo .shousuo-nr span img').css('height', a);
	$('.momocha-banner img').css('height', l);
	$('.momocha-banner-sp img').css('height', l);
	$('.detail').css('width', a);
	$('.detail li').css('width', a);
	$('.momocha-hentiao').css('width', a);
	$('.momocha-hentiao2').css('width', a);
	$('.momocha-size ul li').css('width', a);
	$('.momocha-wenben').css('width', a);
	$('.select-nr').css('width', aa)
	$('.gwc-nr ul li').css('width', a);
	$('.ddxinxi ul li').css('width', a);
	$('.jiesuan .heji').css('width', bb);
	$('.qrdd-nr').css('width', cc);
	$('.sh-li li').css('width', a);
	$('.guest input').css('width', dd)
	var add_input = (appwidth - 90) + 'px';
	$('.add-input input').css('width', add_input);
	var i_input = (appwidth - 170) + 'px';
	$('.i-tel input').css('width', i_input);
	//var aa = $('.momocha-banner img').height()
	//alert(aa)

};

//侧栏动作
function mosidebar() {
	$('.bar-nav').click(function() {
		var rel = $(this).attr('rel');
		var left = '-280px'
		mblack.css('display', 'block')
		$('.momocha-screen').animate({
			'left': left
		}, 300);
		mbar.animate({
			'left': left
		}, 300);
		mindex.animate({
			'left': left
		}, 300);
		$('.momocha-xuanze').animate({
			'left': left
		}, 300);
		$('#push-button').animate({
			'left': left
		}, 300);
		mblack.animate({
			'left': left
		}, 300);

	});

	mblack.click(function() {
		mblack.css('display', 'none')
		$('.momocha-screen').animate({
			'left': 0
		}, 300);
		mbar.animate({
			'left': 0
		}, 300);
		mindex.animate({
			'left': 0
		}, 300);
		$('.momocha-xuanze').animate({
			'left': 0
		}, 300);
		$('#push-button').animate({
			'left': 0
		}, 300);
		mblack.animate({
			'left': 0
		}, 300);
		$('.bar-nav').attr('rel', 'off');

	});
}

//商城选择动作
function spnr() {
	$('.momocha-xuanze a').click(function() {
		var mi = $(this).index();
		var s_left = '-' + appwidth;
		var ml = $('.mleft').height();
		var ml = ml + 'px';
		var mr = $('.mright').height();
		var mr = mr + 'px';
		if(mi == '0') {
			$('.momocha-spnr').animate({
				'margin-left': 0
			}, 300);
			$('body,html').scrollTop(0);
			$('.momocha-xuanze a ').eq(1).removeClass('ahover');
			$('.momocha-xuanze a ').eq(0).addClass('ahover');
			$('.momocha-spnr').css('height', ml);
		}
		if(mi == '1') {
			$('.momocha-spnr').animate({
				'margin-left': s_left
			}, 300);
			$('body,html').scrollTop(0);
			$('.momocha-xuanze a ').eq(0).removeClass('ahover');
			$('.momocha-xuanze a ').eq(1).addClass('ahover');
			$('.momocha-spnr').css('height', mr);
		}
	});

}

//点击下拉显示 两个DIV
function act0() {
	$('.act0').click(function() {
		var rel = $(this).attr('rel');
		var act_f = $(this).parent('.act0-father')
		var act_c = act_f.children('.act0-children');
		if(rel == 'off') {
			act_c.css('display', 'block');
			$(this).attr('rel', 'on')
		};
		if(rel == 'on') {
			act_c.css('display', 'none');
			$(this).attr('rel', 'off')
		};
	})

}

//点击下拉显示 两个DIV
function act1() {
	$('.act1').click(function() {
		var rel = $(this).attr('rel');
		var act_f = $(this).parent('.act1-father')
		var act_c = act_f.children('.act1-children');
		if(rel == 'off') {
			act_c.css('display', 'block');
			$(this).attr('rel', 'on')
		};
		if(rel == 'on') {
			act_c.css('display', 'none');
			$(this).attr('rel', 'off')
		};

	});

	$('.act1-children a').click(function() {
		var act_f = $(this).parents('.act1-father')
		var act_c = act_f.children('.act1-children');
		var act_c2 = act_f.children('.act1-children2');
		var act_me = act_f.children('.act1');
		act_me.css('display', 'none');
		act_c.css('display', 'none');
		act_c2.css('display', 'block');
		act_c2.attr('rel', 'off')

	});

	$('.act1-children2').click(function() {
		var rel = $(this).attr('rel');
		var act_f = $(this).parents('.act1-father')
		var act_c = act_f.children('.act1-children');
		if(rel == 'off') {
			act_c.css('display', 'block');
			$(this).attr('rel', 'on')
		};
		if(rel == 'on') {
			act_c.css('display', 'none');
			$(this).attr('rel', 'off')
		};

	});

}

function zf(){
	$('.ddzf').click(function(){
		$('.ddzf-play').css('display','block');
	});
	
	
}