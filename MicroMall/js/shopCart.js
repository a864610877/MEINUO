


//加数量
var addNumFun = function (el) {
    plusNum(el);
    getTotal();
}

var jianNumFun = function (el) {
    reduceNum(el);
    getTotal();
}



var checkboxIsCheck = function () {
    getTotal();
}

var allCheck = function (el) {
    //第一种方法
    //alert(el.prop("checked"));
    if (el.prop("checked")) {
        $("input[type='checkbox'][name='xz']").prop("checked", "checked");
    }
    else {
        $("input[type='checkbox'][name='xz']").removeAttr("checked");
    }
    getTotal();

    /*第二种*/
    //if (el.prop("checked")) {
    //    var allCheckBox = $("input[type='checkbox'][name='xz']").not("input:checked");
    //    allCheckBox.each(function () {
    //        $(this).attr("checked", "checked");
    //        $(this).click(function () {
    //            getTotal();
    //        })
    //    });
    //}
    //else {
    //    var allCheckBox = $("input[type='checkbox'][name='xz']:checked");
    //    allCheckBox.each(function () {
    //        $(this).removeAttr("checked");
    //        $(this).click(function () {
    //            getTotal();
    //        })
    //    });
    //}
}

var getTotal = function () {
    var allCheckBox = $("input[type='checkbox'][name='xz']:checked");
    var total = 0.00; //商品总额
    var totalF = 0.00;//总邮费
    ///alert(allCheckBox.length);
    if (allCheckBox.length > 0) {
        allCheckBox.each(function () {
            var price = parseFloat($(this).parent().parent().find(".price").val());
            var freight = parseFloat($(this).parent().parent().find(".freight").val())
            //alert("price:" + price + " ;freight:" + freight);
            var num = parseInt($(this).parent().next().find("input[name='goodnum']").val());
            //alert("num:" + num );
            total += price * num;
            totalF += freight * num;
            setTotalValue(total, totalF);
        });
    }
    else {
        setTotalValue(total, totalF);
    }

}

var setTotalValue = function (total, totalF) {
    $("#total").text("￥" + total);
    $("#totalF").text("运费：" + totalF);
}

//删除购物车

var deleteShopCart = function (productid) {
    var item = $("#buycartList_" + productid);
    //alert(productid);
    $.ajax({
        type: 'POST',
        url: "/JuMeiMall/DelShoppingCartById",
        data:  { "shoppingCartIdList":productid },
        success: function (data) {
            if (data.Code == 110) {
                //$.openDialogUrl('未登陆', data.Msg);
                //$.openDialog("添加成功！");
                alert('未登陆');
                location.href = data.Msg;
            }
            else if (data.Code == 0) {
                $.openDialog("删除成功！");
                item.remove();
                //检查购物车是否为空
                checkShopCartIsNull();
                //$.openDialog("添加成功！");
            } else {
                alert(data.Msg);
            }
            

            
            
            //setTimeout(function () { $.closeDialog(); }, 1000)
        },
        error: function () {
            alert("系统异常，请检查网络！");
        }
    });
}

var checkShopCartIsNull = function () {
    if ($(".buycartList").length == 0) {
        var html = '<div class="buycart_k am-text-center am-padding-lg"><h2><img src="/images/buyCart.png" alt=""></h2><h3>您的购物车空空如也<p>快去买买买吧！</p></h3></div>';
        $("#list").html(html);
    }
}

