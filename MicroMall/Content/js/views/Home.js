$(document).ready(function (e) {
    $('.banner').unslider({
        dots: true,
        delay: 5000,
        fluid: true
    });
});
$(function () {
    getProductList()
});

//获取热销产品
function getProductList() {

    $.ajax({
        type: "POST",
        url: "/Product/GetRandomList",
        data: { number: 6 },  //获取6个热销产品
        success: function (result) {
            $.each($.parseJSON(result), function (i, v) {
                var li = '<li  onclick="window.location.href = \'/Product/Details?productId=' + v.Id + '\'">';
                li = li + '<div class="img_div"><div><img src="' + imghost() +'/'+ v.ProductImg + '" style="width: 11rem;height: 11rem;" /></div></div>';
                li = li + '<div class="title_div">' + v.ProductName + '</div>';
                //li = li + '<div class="price_div"><span class="price_span"><span style="font-size: 0.6rem;">￥</span>' + changeTwoDecimal_f(v.ProductPrice) + '</span><span class="vip_price" style="background-color:#FF69B4">会员价￥' + changeTwoDecimal_f(v.VipPrice) + '</span></div>';
                li = li + '<div class="price_div"><span class="price_span"><span style="font-size: 0.6rem;">￥</span>' + changeTwoDecimal_f(v.ProductPrice) + '</span><span class="vip_price" style="background-color:#FF69B4">积分' + v.BuyIntegral + '</span></div>';
                li = li + '</li>';
                $(".product_list").find("ul").append(li);
            });
            $(".product_list ul li:odd").css("margin-left", "0.088rem");
            $(".product_list ul li:even").css("margin-right", "0.088rem");
        }
    });
}
function getColor(name) {
    var color = "#fff";
    $(".Classification ul li").each(function () {
        if ($(this).find("div").html() == name) {
            color = $(this).find("div").attr("color");
            return;
        }
    });
    return color;
}
//强制保留两位小数
function changeTwoDecimal_f(x) {
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