(function (doc, win) {
	var docEl = doc.documentElement,
		resizeEvt = 'orientationchange' in window ? 'orientationchange' : 'resize',
		recalc = function () {
			var clientWidth = docEl.clientWidth;
			if (!clientWidth) return;
			if(clientWidth>=414){
				docEl.style.fontSize = '100px';
			}else{

			}
			docEl.style.fontSize = ((clientWidth / 414) * 16) + 'px';
		};

	if (!doc.addEventListener) return;
	win.addEventListener(resizeEvt, recalc, false);
	doc.addEventListener('DOMContentLoaded', recalc, false);
})(document, window);
function imghost() {
    return "http://imgmeinuo.leipengcar.com/";
}

function checkIsLogin(url) {
    $("#dialog_btn_no").click(function () {
        $(this).parents('.js_dialog').fadeOut(200);
    })
    $("#dialog_btn_yes").click(function () {
        $(this).parents('.js_dialog').fadeOut(200);
        url = url.replace(/\&/g, "_");
        window.location.href = "/User/Login?url=" + url;
    })
    $.ajax({
        type: "get",
        url: "/User/IsLogin",
        success: function (result) {
            result = $.parseJSON(result);
            if (result.state == "true") {
                window.location.href = url;
            } else {
                var dialog = '<div class="js_dialog" id="iosDialog1" style="display: none;"><div class="weui-mask"></div><div class="weui-dialog"><div class="weui-dialog__hd"><strong class="weui-dialog__title">登录验证</strong></div><div class="weui-dialog__bd">您还没登录,是否现在登录!</div><div class="weui-dialog__ft"><a href="javascript:;" class="weui-dialog__btn weui-dialog__btn_default" onclick="dialog_btn_no(this)">否</a><a href="javascript:;" class="weui-dialog__btn weui-dialog__btn_primary" onclick="dialog_btn_yes(this)">是</a></div></div></div>';
                if ($("#iosDialog1") == undefined || $("#iosDialog1") == "undefined") {
                    $("body").append(dialog);
                }
                $('#iosDialog1').fadeIn(200);
            }
        }
    });
}
function sendAjax(url, data) {
    $.ajax({
        type: "post",
        url: url,
        data:data,
        success: function (result) {
            result = $.parseJSON(result);
            if (result.state == "success") {
                popup({ type: 'success', msg: result.message, delay: 2000, bg: true, clickDomCancel: true });
            } else {
                popup({ type: 'error', msg: result.message, delay: 2000, bg: true, clickDomCancel: true });
            }
        }
    });
}