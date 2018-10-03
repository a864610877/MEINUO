var LevelName2;
var LevelName3;
var LevelName4;
var Discount2;
var Discount3;
var Discount4;
var typeid = $("#TypeId").val()
var sort = "CreateTime desc"  //默认排序
$(function () {
    $(".title").html($(".type ul li[typeid='" + typeid + "'] span").html());
    GetProductList()  //加载数据

    //选择分类点击事件
    $(".type ul li").click(function () {
        $(".title").html($(this).find("span").html());
        typeid = $(this).attr("typeid");
        GetProductList();
    });
});
//加载数据
function GetProductList() {
    $.ajax({
        type: "POST",
        url: "GetListProduct",
        data: { typeId: typeid, sort: sort },  //分类ID
        success: function (result) {
            $(".product_ul").find("li").remove();
            $.each($.parseJSON(result), function (i, v) {
                var li = '<li class="li" onclick="window.location.href = \'/Product/Details?productId=' + v.Id + '\'">';
                li = li + '<div class="f_l"><img src="' + imghost() + '/' + v.ProductImg + '" /></div>';
                li = li + '<div class="f_l"><h3 class="product_title">' + v.ProductName + '</h3></div>';
                li = li + '<div class="product_bq">免邮费</div>';
                li = li + '<div class="price"><span class="price_i">￥</span><span class="price_p">' + changeTwoDecimal_f(v.ProductPrice) + '</span></div>';
                li = li + '</li>';
                $(".product_ul").append(li);

                $(".product_ul li").each(function () {
                    if ($(this).find(".product_title").height() <= 20) {
                        $(this).find(".product_title").css("margin-top","1.7rem")
                    }
                });
            });
            $(".product_ul li .product_title").each(function () {
                if ($(this).height() == 20) {
                    $(this).css("margin-top", "1.3rem");
                    $(this).css("margin-bottom", "0.5rem");
                }
            });
        }
    });
    
    hideType()
}
//点击显示分类选项
function showType() {
    $(".type_div").show();
}
//隐藏分类层
function hideType() {
    $(".type_div").hide();
}
function Sort(obj) {
    $(".sort_date,.sort_price").removeClass("sort_color");
    $(obj).addClass("sort_color");
    $(obj).attr("sort") == "1" ? $(obj).attr("sort", "2") : $(obj).attr("sort", "1");
    sort = $(obj).attr("column") + " " + ($(obj).attr("sort") == "1" ? "desc" : "asc")
    GetProductList();
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