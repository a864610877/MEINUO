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
    // ����
	
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
	
		
        //��������ڴ�С�ı�ʱ
        $(window).resize(function() {
             leftTop(obj);
        });
        //������й�����ʱ�Ĳ�����
        $(window).scroll(function() {
            leftTop(obj);
        });
       
    }
    //ȷ��ȡ���Ĳ���
	$('.btn1,.btn2').click(function(){
		$('.mask,.alertBox').hide();
	})
   
	$("#tiaojiaoImg").click(function () {
	    var imgsrc = $(".dizhi_suxing_i").val();
	    if (imgsrc=="") {
	        alert("ͼƬ��ַ����Ϊ�գ�");
	        return;
	    }
	    var describeimg = $("#describeimg").val();
	    if (describeimg == "") {
	        alert("������������Ϊ�գ�");
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
	        alert("����ֵ����Ϊ�գ�");
	        return;
	    }
	    var describetxt = $("#describetxt").val();
	    if (describetxt == "") {
	        alert("������������Ϊ�գ�");
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