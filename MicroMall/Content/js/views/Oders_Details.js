$(function () {
    countPrice()
})
//商品减少事件
function jian(obj) {
    if (parseInt($(obj).parent().find(".number").html()) <= 1) {
        $(obj).parent().find(".number").html("1")
    } else {
        $(obj).parent().find(".number").html(parseInt($(obj).parent().find(".number").html()) - 1)
        var number = parseInt($(obj).parent().find(".number").html());
        var price = parseFloat($(obj).parents("li").find(".productPrice").find("span").attr("value")); //获取单价
        var totle_p = changeTwoDecimal_f2(number * price);
        $(obj).parents("li").find(".productPrice").find("span").html(totle_p); //设置单件商品总价
    }
    //获取修改后的 数量 总价 
    var id = parseInt($(obj).parents("li").find(".id").val());
    var num = parseInt($(obj).parents("li").find(".number").html());
    var orderPrice = parseFloat(parseFloat($(obj).parents("li").find(".productPrice").find("span").attr("value")) * num);
    UpdateNumber(id, num, orderPrice); //提交到后台修改数据
    countPrice();
}
//商品增加事件
function jia(obj) {
    $(obj).parent().find(".number").html(parseInt($(obj).parent().find(".number").html()) + 1)
    var price = parseFloat($(obj).parents("li").find(".productPrice").find("span").attr("value")); //获取单价
    $(obj).parents("li").find(".productPrice").find("span").html(changeTwoDecimal_f2(parseInt($(obj).parent().find(".number").html()) * price));  //设置单件商品总价
    //获取修改后的 数量 总价 
    var id = parseInt($(obj).parents("li").find(".id").val());
    var num = parseInt($(obj).parents("li").find(".number").html());
    var orderPrice = parseFloat(parseFloat($(obj).parents("li").find(".productPrice").find("span").attr("value")) * num);
    UpdateNumber(id, num, orderPrice); //提交到后台修改数据
    countPrice();
}
//更新商品总价
function countPrice() {
    var countPirce = 0; //总价
    $(".list ul li").each(function () {
        var price = parseFloat($(this).find(".productPrice").find("span").html()); //获取勾选商品的价格
        $(this).find(".productPrice").find("span").html(changeTwoDecimal_f2($(this).find(".productPrice").find("span").html()));
        countPirce = countPirce + price;
    });
    $("#totlePrice").html(changeTwoDecimal_f2(countPirce));
}

//修改单个商品的购买数量
function UpdateNumber(id, number,orderPrice) {
    $.ajax({ type: "POST", url: "UpdateNumber", data: { id: id, number: number, orderPrice: orderPrice } });
}

//强制保留2位小数
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


function Payment() {
    //判断是否有收货地址
    var AddressId = $("#AddressId").val();
    if (AddressId == "") {
        popup({ type: 'error', msg: "请选择收货地址！", delay: 2000, bg: true, clickDomCancel: true });
        return false;
    }
    //提交支付
    $.ajax({
        type: "POST",
        url: "Payment",
        data: { ids: $("#Order_Ids").val() },
        success: function (result) {
            result = $.parseJSON(result);
            if (result.state == "success") {
                popup({
                    type: 'load', msg: result.message, delay: 1500, callBack: function () {
                        window.location.href = "/Order/Index";
                    }
                });
            } else {
                popup({ type: 'error', msg: result.message, delay: 2000, bg: true, clickDomCancel: true });
            }
        }
    });
}