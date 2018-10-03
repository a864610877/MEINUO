$(function () {
    var type = $("input[name='Type']:checked").val();
    if (type == 1) {
        $("#ggsxTxt").show();
        $("#ggsxImg").hide();
    } else {
        $("#ggsxTxt").hide();
        $("#ggsxImg").show();
    }

    $('.shuxinfenlei').click(function () {
        $('.mask').css({'display': 'block'});
        //center($('.alertBox'));
        $('.alertBox').css({ 'display': 'block' });

    });
    // 居中
	
	function leftTop(obj){
			var screenWidth = $(window).width();
            var screenHeight = $(window).height();
            var scrolltop = $(document).scrollTop();
			var scrollleft = $(document).scrollLeft();
            var objLeft = (screenWidth - obj.width())/2 + scrollleft  ;
            var objTop = (screenHeight - obj.height()) / 2 ;
			obj.css({left: objLeft + 'px', top: objTop + 'px'});
	}
    function center(obj) {
        leftTop(obj);
	
		
        //浏览器窗口大小改变时
        $(window).resize(function() {
             leftTop(obj);
        });
        //浏览器有滚动条时的操作、
        $(window).scroll(function() {
            leftTop(obj);
        });
       
    }
    //确定取消的操作
	$('.btn1,.btn2').click(function(){
		$('.mask,.alertBox').hide();
	})
   
	$("#tiaojiaoImg").click(function () {
	    var imgsrc = $(".dizhi_suxing_i").val();
	    if (imgsrc=="") {
	        alert("图片地址不能为空！");
	        return;
	    }
	    var describeimg = $("#describeimg").val();
	    if (describeimg == "") {
	        alert("属性描述不能为空！");
	        return;
	    }
	    $("#tupiantype").fadeOut();
	    $(".dizhi_suxing_i").val("");
	    $("#describeimg").val("");
	    $(".shuxingzhi_img").append("<li><img width='33px' height='33px' src='" + imgsrc + "'><input type='hidden' value=" + describeimg + "></li>");
	});

	$(".shuxin_close").click(function () {
	    $(".tianjia_shuxingzhi_p").fadeOut();
	})

	$("#tiaojiaoTxt").click(function () {
	    var txt = $(".shuxingzhi_jiashang").val();
	    if (txt == "") {
	        alert("属性值不能为空！");
	        return;
	    }
	    var describetxt = $("#describetxt").val();
	    if (describetxt == "") {
	        alert("属性描述不能为空！");
	        return;
	    }
	    $("#describetxt").val("");
	    $(".shuxingzhi_jiashang").val("");
	    $("#wenzitype").fadeOut();
	    $(".shuxingzhi_txt").append("<li>" + txt + "<input type='hidden' value=" + describetxt + "></li>");
	});

	$("#tag_class_edit1").click(function () {

	    var type = $("input[name='Type']:checked").val();
	    if (type == 1) {
	        $("#wenzitype").show();
	        $("#tupiantype").hide();
	    } else {
	        $("#tupiantype").show();
	        $("#wenzitype").hide();
	    }

	})

});
function docheck(obj) {
    var type = $(obj).val();
    if (type == 1) {
        $("#ggsxTxt").show();
        $("#ggsxImg").hide();
    } else {
        $("#ggsxTxt").hide();
        $("#ggsxImg").show();
    }
}