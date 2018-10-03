$.extend({
    jsApiCall: function (data) {
        var jsondata = JSON.parse(data);
        WeixinJSBridge.invoke(
                  'getBrandWCPayRequest',
                   jsondata,//josn串
                   function (res) {
                       if (res.err_msg == "get_brand_wcpay_request:ok") {
                           $.openDialog("支付成功");
                           //$.closeLoading();
                           //$.openSuccessMessage("支付成功,<a href='/home/index'>去看视频</a>");
                           window.location.href = '/PersonalCentre/MyOrder'
                           //window.location.href = "/WeChat/PaySuccess";
                           //$.openSuccessMessage("支付成功<a href='/TicketType/TicketType'>返 回</a>");
                       }
                       else {
                           //alert("支付失败");
                           alert(res.err_code + "|" + res.err_desc + "|" + res.err_msg);
                       }
                       //alert(res.err_code +"|"+ res.err_desc+"|" + res.err_msg);
                   }
          );
    },
    webChatPay: function (data) {
        if (typeof (WeixinJSBridge) == "undefined") {
            if (document.addEventListener) {
                document.addEventListener('WeixinJSBridgeReady', $.jsApiCall, false);
            }
            else if (document.attachEvent) {
                document.attachEvent('WeixinJSBridgeReady', $.jsApiCall);
                document.attachEvent('onWeixinJSBridgeReady', $.jsApiCall);
            }
        }
        else {
            $.jsApiCall(data);
        }
    },
    openLoading: function (msg) {
        var html = '<div id="loadingToast"><div class="weui-mask_transparent"></div>';
        html += ' <div class="weui-toast"><i class="weui-loading weui-icon_toast"></i>';
        if (msg == "undefined" || msg == null || msg == "") {

        } else {
            html += '<p class="weui-toast__content">' + msg + '</p>';
        }
        
        html += '</div>';
        html += '</div>';
        $('body').append(html);
    },
    closeLoading: function () {
        var loadingToast = $("#loadingToast");
        if (loadingToast.length > 0)
            $("#loadingToast").remove();
    },
    openDialog: function (msg) {
        var html = ' <div class="js_dialog" id="iosDialog2">';
        html += '<div class="weui-mask"></div>';
        html += '<div class="weui-dialog">';
        html += ' <div class="weui-dialog__bd">' + msg + '</div>';
        html += '<div class="weui-dialog__ft">';
        html += '<a href="javascript:;" id="closeDialog" onclick="$.closeDialog()" class="weui-dialog__btn weui-dialog__btn_primary">知道了</a>';
        html += '</div>';
        html += '</div>';
        html += '</div>';
        $('body').append(html);
    },
    openDialogUrl: function (msg,url) {
        var html = ' <div class="js_dialog" id="iosDialog2" style="z-index:1000">';
        html += '<div class="weui-mask"></div>';
        html += '<div class="weui-dialog">';
        html += ' <div class="weui-dialog__bd">' + msg + '</div>';
        html += '<div class="weui-dialog__ft">';
        html += '<a href="'+url+'" id="closeDialog" class="weui-dialog__btn weui-dialog__btn_primary">知道了</a>';
        html += '</div>';
        html += '</div>';
        html += '</div>';
        $('body').append(html);
    },
    closeDialog: function () {
        var iosDialog2 = $("#iosDialog2");
        if (iosDialog2.length > 0)
            $("#iosDialog2").remove();
    }

});

var url = "http://120.76.159.57:55364/";
//var url = "http://localhost:5269/";
//var url="http://139.199.107.120:8002/";
//var url="https://www.jazzweng.com/";
//var url = "https://www.jazzweng.com:446/";
//var url = "https://www.jazzweng.com:449/"
var xhr1 = new XMLHttpRequest();

var htmlTableNull = '<li class="listnone"><div class="listnoneimg"><img src="../images/Member/navicon/icon_billing.png" /></div><p>没有信息</p></li>';
var RbIndexInput = '';

function showLoading() {
    var html = document.createElement("div");
    html.setAttribute("class", "loading_wrap block");
    html.setAttribute("id", "load")
    html.innerHTML = '<div class="loading_con"><span class="mui-spinner"></span></div><div class="loading_bg"></div> ';
    document.body.appendChild(html);
}
//返回首页

//	if(plus.webview.getWebviewById("index.html")==null){
//		plus.webview.open("index.html","index.html","","slide-in-right");
//	}else{
//		plus.webview.show("index.html","slide-in-right");
//	}

function closeLoading() {
    var el = document.getElementById("load");
    el.remove();
}

function $post(fun, path) {
    xhr1.onreadystatechange = fun;
    xhr1.open("POST", url + path, true);
    xhr1.send();
}

function $get(fun, path) {
    xhr1.onreadystatechange = fun;
    xhr1.open("GET", url + path, true);
    xhr1.send();
}
//获取radio值
function getRadioValue(name) {
    var value = 0;
    var $el = document.getElementsByName(name);
    if ($el.length > 0) {
        for (item in $el) {
            if ($el[item].checked) {
                value = $el[item].value;
            }
        };
    }
    return value;
}
//获取select选中项
function getSelectValue(id) {
    var obj = new Object();
    var $el = document.getElementById(id);
    if ($el.length > 0) {
        var index = $el.selectedIndex;
        obj.value = $el.options[index].value;
        obj.text = $el.options[index].text;
    }
    return obj;
}
function login() {
    var id = getQueryString("id");
    if (id != null && id != null && id != undefined) {
        id = parseInt(id);
        if (id > 0) {
            setId(id);
        }
    }
}
//判断是否登录了
function IsLogin(state) {
    if (getId() == null || getId() == "" || getId() == "0") {
        window.location.href = 'login.html?state=' + state + '&date=' + Date.now
    }
}

function BackIndex() { //返回首页
    closeCurrView();
    //plus.storage.setItem("VideoCategoryId", 0);
    //window.location.href = "index.html";
    //OpenView("indexSubpage.html");
    //plus.webview.currentWebview().hide();
    //window.location.href="index.html";
}

function LoginOff() {
    var str = getMemberInfoId();
    str = crypt.encrypt(str);
    $.openLoading("登出中");
    mui.ajax(url + "/WeChat/LoginOut", {
        data: {
            "str": str,
        },
        type: "post",
        dataType: "json",
        success: function (data) {
            $.closeLoading();
            //mui.toast("退出成功");
        },
        error: function (xhr, type, errorThrown) {
            $.closeLoading();
            //mui.toast("网络错误");
        }
    });
    setMemberInfoId("");
    setIsPay("");
    window.location.href = "login.html";
}

function closeCurrView() {
    plus.webview.currentWebview().close();
}

function OpenPreloadView(obj) {
    obj.show("slide-in-right");
}

function OpenView(url) {
    mui.openWindow({
        url: url,
        id: url,
    });
}

function TwoDecimal(amount) {
    //保留两位小数，不四舍五入
    amount = amount * 100;
    amount = parseInt(amount);
    amount = amount / 100;
    return amount;
}

function escape2Html(str) { //转意符换成普通字符
    var arrEntities = {
        'lt': '<',
        'gt': '>',
        'nbsp': ' ',
        'amp': '&',
        'quot': '"'
    };
    return str.replace(/&(lt|gt|nbsp|amp|quot);/ig, function (all, t) {
        return arrEntities[t];
    });
}

//复制
function copyToClip(text) {
    if (plus.os.name == "Android") {
        var Context = plus.android.importClass("android.content.Context");
        var main = plus.android.runtimeMainActivity();
        var clip = main.getSystemService(Context.CLIPBOARD_SERVICE);
        plus.android.invoke(clip, "setText", text);
        plus.nativeUI.toast("复制成功");
    } else if (plus.os.name == "iOS") {
        var UIPasteboard = plus.ios.importClass("UIPasteboard");
        //这步会有异常因为UIPasteboard是不允许init的，init的问题会在新版中修改 
        var generalPasteboard = UIPasteboard.generalPasteboard();
        // 设置/获取文本内容: www.bcty365.com
        generalPasteboard.setValueforPasteboardType(text, "public.utf8-plain-text");
        //var value = generalPasteboard.valueForPasteboardType("public.utf8-plain-text"); 
        plus.nativeUI.toast("复制成功");
    }
}

//特殊字段
var Id_Key = "Id_Key";
var exchangeRate_Key = "exchangeRate_Key";
var isPay_key = "isPay_key";
var photo_key = "photo_key";
var pay_url_key = "pay_url_key";

function getId() {
    var result = 0;
    if (localStorage.getItem(Id_Key) != null) {
        result = localStorage.getItem(Id_Key);
    }
    return result;
}

function setId(value) {
    localStorage.setItem(Id_Key, value);
}

function getIsPay() {
    var result = 0;
    if (localStorage.getItem(isPay_key) != null) {
        result = localStorage.getItem(isPay_key);
    }
    return result;
}

function setIsPay(value) {
    localStorage.setItem(isPay_key, value);
}

function getPhoto() {
    var result = "";
    if (localStorage.getItem(photo_key) != null) {
        result = localStorage.getItem(photo_key);
    }
    return result;
}

function setPhoto(value) {
    localStorage.setItem(photo_key, value);
}

function getPayUrl() {
    var result = "";
    if (localStorage.getItem(pay_url_key) != null) {
        result = localStorage.getItem(pay_url_key);
    }
    return result;
}

function setPayUrl(value) {
    localStorage.setItem(pay_url_key, value);
}



//關閉所有打開的窗口
function RbIndex() {
    var wvs = plus.webview.all(); //所有窗口对象
    //var launch = plus.webview.getLaunchWebview(); //首页窗口对象
    var self = plus.webview.currentWebview(); //当前窗口对象
    //plus.nativeUI.alert(wvs[i].id); 
    //plus.nativeUI.alert(self.id);
    for (var i = 0, len = wvs.length; i < len; i++) {
        //plus.nativeUI.alert(wvs[i].id);
        //plus.nativeUI.alert(self.id);
        // 首页以及当前窗口对象，不关闭；
        if (wvs[i].id === self.id) { //wvs[i].id === launch.id || 

            continue;
        } else {
            wvs[i].close('none'); //关闭中间的窗口对象，为防止闪屏，不使用动画效果；
        }
    }
    var page = plus.webview.getWebviewById("login.html");
    if (page == null) {
        page = plus.webview.create("login.html");
    }
    mui.fire(page, 'refresh', {});
    page.show();

    // 此时，窗口对象只剩下首页以及当前窗口，直接关闭当前窗口即可；
    self.close('slide-out-right');
}
////身份证号合法性验证 
//支持15位和18位身份证号
//支持地址编码、出生日期、校验位验证
function IdentityCodeValid(code) {
    var city = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江 ", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北 ", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏 ", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外 " };
    var tip = "";
    var pass = true;

    if (!code || !/^\d{6}(18|19|20)?\d{2}(0[1-9]|1[12])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/i.test(code)) {
        tip = "身份证号格式错误";
        pass = false;
    }

    else if (!city[code.substr(0, 2)]) {
        tip = "地址编码错误";
        pass = false;
    }
    else {
        //18位身份证需要验证最后一位校验位
        if (code.length == 18) {
            code = code.split('');
            //∑(ai×Wi)(mod 11)
            //加权因子
            var factor = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2];
            //校验位
            var parity = [1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2];
            var sum = 0;
            var ai = 0;
            var wi = 0;
            for (var i = 0; i < 17; i++) {
                ai = code[i];
                wi = factor[i];
                sum += ai * wi;
            }
            var last = parity[sum % 11];
            if (parity[sum % 11] != code[17]) {
                tip = "校验位错误";
                pass = false;
            }
        }
    }
    // if(!pass) return false;
    return pass;
}
//获取url参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function getOpenId(url) {
    var rui = encodeURIComponent(url);
    var url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx1eaebf07824ac84c&redirect_uri=" + rui + "&response_type=code&scope=snsapi_base&state=123#wechat_redirect ";
    window.location.href = url;
}
function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
            + " " + date.getHours() + seperator2 + date.getMinutes()
            + seperator2 + date.getSeconds();
    return currentdate;
}

function toast() {
    mui.toast("敬请期待");
}

function OpenGoodsDetail(commodityId, hrefPage, parameter) {
    window.location.href = ("GoodsDetail.html?commodityId=" + commodityId + "&hrefPage=" + hrefPage + "&parameter=" + parameter);

}

function getMoblieType() {
    var u = navigator.userAgent;
    if (u.indexOf('Android') > -1 || u.indexOf('Linux') > -1) {//安卓手机
        return "Android";
        // window.location.href = "mobile/index.html";
    } else if (u.indexOf('iPhone') > -1) {//苹果手机
        return "ios";
    } else if (u.indexOf('Windows Phone') > -1) {//winphone手机
        alert("不支持");
        // window.location.href = "mobile/index.html";
    }
}
//验证手机号
function VerifiMobile(mobile) {
    var reg = /(1[3-9]\d{9}$)/;
    if (!reg.test(mobile)) {
        return false;
    }
    return true;
}
//验证邮箱
function VerifiEmail(email) {
    var regemail = /\w+[@]{1}\w+[.]\w+/;
    if (!regemail.test(email)) {
        return false;
    }
    return true;
}

//function GetWxUserInfo(state) {
//    $.openLoading("加载中...");
//    $.ajax({
//        type: 'post',
//        data: {
//            state: state
//        },
//        url: url + '/Base/RediToLogin',
//        success: function (data) {
//            $.closeLoading();
//            if (data.Code == 0) {
//                window.location.href = data.Msg;
//            } else {
//                $.closeLoading();
//            }
//        }, error: function () {
//            $.closeLoading();
//            $.openDialog("网络错误，请稍后再试");
//        }
//    })


//}

function refresh(){
    window.location.reload();//强制刷新
}

$(function () {
    $(document).on("click", ".lessNum", function () {
        var num = $(".valueNum").val();
        if (num==""||parseInt(num) <= 0) {
            $(".valueNum").val(0);
            return;
        } else {
            $(".valueNum").val(parseInt(num)-1);
            return;
        }
    });
    $(document).on("click", ".plusNum", function () {
        var num = $(".valueNum").val();
        if (num==""||parseInt(num) <= 0) {
            $(".valueNum").val(1);
            return;
        } else {
            $(".valueNum").val(parseInt(num) + 1);
            return;
        }
    });

})

function OpenGoodsDtl(commodityId) {
    window.location.href = ("goodsDetail.html?id=" + commodityId);

}
//加Num
function plusNum(el) {
    var num = $("#" + el).val();
    if (parseInt(num) <= 1)
        num = 1;
    $("#" + el).val(parseInt(num) + 1);
}

function reduceNum(el) {
    var num = $("#" + el).val();
    if (parseInt(num) <= 1)
    {
        $("#" + el).val(1);
        num = 1;
        return;
    }
    $("#" + el).val(parseInt(num) -1);
}

