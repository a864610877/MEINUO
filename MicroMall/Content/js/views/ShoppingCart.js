$(function () {
    var list = $(".list ul li").length;
    if (list == 0) {
        $("#cart").hide();
        $("#nullCart").show();
    }
    initialization()
})
//初始化价格，勾选状态，合计总额
function initialization() {
    $(".list ul li").each(function () {
        var number = parseInt($(this).find(".number_btns").find(".number").html());
        var price = parseFloat($(this).find(".r_price").find(".price").find("span").attr("value"));
        $(this).find(".price").find("span").html(changeTwoDecimal_f2(number * price));
    });
    countPrice() //更新购物车总价信息
}
//购物车商品勾选 更换勾选Icon
function check_btn(obj) {
    if ($(obj).attr("src") == "/Content/img/cartchecked1.png") {
        $(obj).attr("src", "/Content/img/cartchecked2.png")  //勾选
        $(obj).parents("li").attr("checked", true);
    } else {
        $(obj).attr("src", "/Content/img/cartchecked1.png") //未勾选
        $(obj).parents("li").attr("checked", false);
    }
    countPrice() //更新购物车总价信息
}
//购物车商品减少事件
function jian(obj) {
    var id = $(obj).parents("li").find(".cartID").val();
    var number = 1;
    if (parseInt($(obj).parent().find(".number").html()) <= 1) {
        $(obj).parent().find(".number").html("1")
    } else {
        $(obj).parent().find(".number").html(parseInt($(obj).parent().find(".number").html()) - 1)
        var price = parseFloat($(obj).parents("li").find(".price").find("span").attr("value")); //获取单价
        number = parseInt($(obj).parent().find(".number").html());
        $(obj).parents("li").find(".price").find("span").html(changeTwoDecimal_f2(number * price)); //设置单件商品总价
    }
    countPrice() //更新购物车总价信息
    UpdateNumber(id, number);
}
//购物车商品增加事件
function jia(obj) {
    var id = $(obj).parents("li").find(".cartID").val();
    var number;
    $(obj).parent().find(".number").html(parseInt($(obj).parent().find(".number").html()) + 1)
    var price = parseFloat($(obj).parents("li").find(".price").find("span").attr("value")); //获取单价
    number = parseInt($(obj).parent().find(".number").html());
    $(obj).parents("li").find(".price").find("span").html(changeTwoDecimal_f2(number * price));  //设置单件商品总价
    countPrice() //更新购物车总价信息
    UpdateNumber(id, number);
}
//删除购物车商品
function deleteCart(obj, id) {
    $(obj).parents("li").remove();
    countPrice() //更新购物车总价信息
    var list = $(".list ul li").length;
    if (list == 0) {
        $("#cart").hide();
        $("#nullCart").show();
    }
    DeleteCart(id)
}

//更新购物车商品总价
function countPrice() {
    var countPirce = 0; //购物车总价
    var countNumber = 0;  //勾选商品的数量
    $(".list ul li[checked='checked']").each(function () {
        var price = parseInt($(this).find(".price").find("span").html()); //获取勾选商品的价格
        countPirce = countPirce + price;
        countNumber = countNumber + 1;
    });
    $("#countPrice").html(changeTwoDecimal_f2(countPirce));
    $("#countNumber").html(countNumber);
}

//全选
function checkedAll(obj) {
    if ($(obj).attr("src") == "/Content/img/allchecked.png") {
        $(obj).attr("src", "/Content/img/cartchecked2.png")  //勾选
        $(".list ul li").each(function () {
            $(this).attr("checked", true);
            $(this).find("img[src='/Content/img/cartchecked1.png']").attr("src", "/Content/img/cartchecked2.png");
        });
    } else {
        $(obj).attr("src", "/Content/img/allchecked.png") //未勾选
        $(".list ul li").each(function () {
            $(this).attr("checked", false);
            $(this).find("img[src='/Content/img/cartchecked2.png']").attr("src", "/Content/img/cartchecked1.png");
        });
    }
    countPrice() //更新购物车总价信息
}
//修改购物车单个商品的购买数量
function UpdateNumber(id, number) {
    $.ajax({ type: "POST", url: "UpdateNumber", data: { Id: id, Number: number } });
}
//删除商品
function DeleteCart(id) {
    $.ajax({ type: "POST", url: "DeleteCart?id=" + id });
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
//去结算按钮事件
function Payment() {
    window.location.href = "/ShoppingCart/ToOrder?ids=" + GetData();
}
//获取选中数据
function GetData() {
    var ids = "";
    $("li[checked='checked']").each(function () {
        if (ids == "") {
            ids = $(this).find(".cartID").val()
        } else {
            ids = ids + "," + $(this).find(".cartID").val()
        }
    });
    return ids;
}