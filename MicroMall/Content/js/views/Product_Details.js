var mode = 0; //1、加入购物车 2、立即购买(用于提交确定是的判断)
var options = "";
$(function () {
    var mySwiper = new Swiper('.swiper-container', {
        loop: true,
        grabCursor: true,
        paginationClickable: true
    })
})
$(function () {
    //初始化价格强制保留2位小数
    var price = changeTwoDecimal_f2($("#price").html());
    var vipPrice = changeTwoDecimal_f2($("#vip_price").html());
    $("#price").html(price);  //格式化原价 保留2位小数
    //$("#vip_price").html(vipPrice); //格式化会员价 保留2位小数
    //$("#cart_price").html($("#Grade").val() == 1 ? price : vipPrice); //设置购物车价格，如果用户不是会员使用原价，如果是会员按照等级计算折扣后的价格显示
    //$("#cart_price").attr("price", $("#Grade").val() == 1 ? price : vipPrice);    //设置属性存值 方便读取数据

    //var imgurl = imghost();

    $("#ProductDetail img").each(function () {
        $(this).attr("src", "http://imgmeinuo.leipengcar.com/" + $(this).attr("src"))
    });


    //getVipPrice();  //获取会员价格和折扣列表

    //加载产品选项参数
    $(".spec_list ul li").click(function () {
        if ($(this).parents(".spec_list").attr("Method") == 1) {
            $(this).parents(".spec_list").find("ul").find("li").removeClass("specSelectColor");
            $(this).addClass("specSelectColor");
        } else {
            if (!$(this).hasClass("specSelectColor")) {
                $(this).addClass("specSelectColor");
            } else {
                $(this).removeClass("specSelectColor");
            }
        }
    });
});
function getFileName(o){
    var pos = o.lastIndexOf("/");
    return o.substring(pos+1,o.length);  
}
//加入购物车
function addCart() {
    mode = 1;
    $("#cart").show();
}
//立即购买
function submitOrder() {
    mode = 2;
    $("#cart").show();
}

//减少购买数量
function jian() {
    if (parseInt($(".number").html()) <= 1) {
        $(".number").html("1");
    } else {
        $(".number").html(parseInt($(".number").html()) - 1);
    }
    count();
}
//增加购买数量
function jia() {
    $(".number").html(parseInt($(".number").html()) + 1);
    count();
}
//统计总额
function count() {
    var price = parseInt($("#cart_price").attr("price"));
    var number = parseInt($(".number").html());
    $("#cart_price").html(changeTwoDecimal_f2(price * number));
}
function submit() {
    getOptions()//获取用户选择的规格参数

    if (mode == 1) {  //加入购物车
        $.ajax({
            type: "POST",
            url: "/ShoppingCart/AddCart",
            data: {
                ProductId: $("#ProductId").val(),
                UnitPrice: $("#cart_price").attr("price"),
                Number: parseInt($("#number").html()),
                Options: options
            },
            success: function (result) {
                result = $.parseJSON(result);
                if (result.state == "success") {
                    popup({ type: 'success', msg: result.message, delay: 2000, bg: true, clickDomCancel: true });
                    $("#cart").hide();
                } else {
                    var dialog = '<div class="js_dialog" id="iosDialog1" style="display: none;"><div class="weui-mask"></div><div class="weui-dialog"><div class="weui-dialog__hd"><strong class="weui-dialog__title">登录验证</strong></div><div class="weui-dialog__bd">您还没登录,是否现在登录!</div><div class="weui-dialog__ft"><a href="javascript:;" class="weui-dialog__btn weui-dialog__btn_default" onclick="dialog_btn_no(this)">否</a><a href="javascript:;" class="weui-dialog__btn weui-dialog__btn_primary" onclick="dialog_btn_yes(this)">是</a></div></div></div>';
                    if ($("#iosDialog1") == undefined || $("#iosDialog1") == "undefined") {
                        $("body").append(dialog);
                    }
                    $("#dialog_btn_no").click(function () {
                        $(this).parents('.js_dialog').fadeOut(200);
                    })
                    $("#dialog_btn_yes").click(function () {
                        $(this).parents('.js_dialog').fadeOut(200);
                        window.location.href = "/User/Login?url=/Product/Details?productId=" + $("#ProductId").val();
                    })
                    $('#iosDialog1').fadeIn(200);
                }
            }
        });
    } else if (mode == 2) {   //立即购买
        var ProductId = $("#ProductId").val();
        var UnitPrice = $("#cart_price").attr("price");
        var Number = parseInt($("#number").html());
        var Options = options;
        checkIsLogin("/Order/AddOrder?ProductId=" + ProductId + "&UnitPrice=" + UnitPrice + "&Number=" + Number + "&Options=" + Options);
    }
}
//获取已选择的规格参数
function getOptions() {
    $(".spec_list ul li[class='specSelectColor']").each(function () {
        if (options == "") {
            options = options + $(this).html();
        } else {
            options = options + "," + $(this).html();
        }
    })
}

//强制保留1位小数
function changeTwoDecimal_f2(x) {
    var f_x = parseFloat(x);
    if (isNaN(f_x)) {
        return false;
    }
    f_x = Math.round(f_x * 100) / 100;
    var s_x = f_x.toString();
    var pos_decimal = s_x.indexOf('.');
    if (pos_decimal < 0) {
        pos_decimal = s_x.length;
        s_x += '.';
    }
    while (s_x.length <= pos_decimal + 2) {
        s_x += '0';
    }
    return s_x;
}