function loadhtml(pageIndex, orderStatus) {
    $.openLoading("加载中");
    $.ajax({
        url: "/PersonalCentre/MyOrder",
        data: { pageIndex: pageIndex, orderState: orderStatus },
        type: "post",
        dataType: "json",
        success: function (data) {
            $.closeLoading();
            $("#orderState").val(orderStatus);
            if (data.Code == 0) {
                var html = "";
                if (data.List != null && data.List.List != null && data.List.List.length > 0) {
                    $("#pageIndex").val(data.List.NextPage);
                    for (var i = 0; i < data.List.List.length; i++) {
                        var item = data.List.List[i];
                        html += orderHtmlBuilder(item);

                    }
                    $(".ssd_xsd_xdesrx").html('<div style="text-align:center"><a onclick="appendOrder()">点击加载更多...</a></div>');
   
                } else {
                    html = '';
                    $(".ssd_xsd_xdesrx").html('<div style="text-align:center"><a>找不到更多商品了...</a></div>');


                }
                if (data.List.NextPage <= 0) {
                    $(".ssd_xsd_xdesrx").html('<div style="text-align:center"><a>找不到更多商品了...</a></div>');
                }
                $("#MyOrderList").html(html);
                $(".ssd_xsd_xdesrx").css('display', 'block');
                setTimeout(function () {
                    $.getScript('../js/momocha-min.js', function () {
                        fit();
                        mosidebar();
                        spnr();
                        act0();
                    });
                }, 200);

            } else if (data.Code == -2) {
                window.location.href = data.Msg;
            } else {
                $.openDialog(data.Msg);
            }
        },
        error: function () {
            $.closeLoading();
            $.openDialog("网络错误，请稍后再试");
        }
    })



}

function appendOrder() {

    $(".ssd_xsd_xdesrx").css('display', 'none');
    var orderStatus = $("#orderStatus").val();
    var pageIndex = $("#pageIndex").val();
    $.openLoading("加载中");
    setTimeout(function () {
        $.ajax({
            url: "/PersonalCentre/MyOrder",
            data: { pageIndex: pageIndex, orderState: orderStatus },
            type: "post",
            dataType: "json",
            success: function (data) {
                $.closeLoading();
                $("#orderState").val(orderStatus);
                if (data.Code == 0) {
                    var html = "";
                    if (data.List != null && data.List.List != null && data.List.List.length > 0) {
                        $("#pageIndex").val(data.List.NextPage);
                        for (var i = 0; i < data.List.List.length; i++) {
                            var item = data.List.List[i];
                            html += orderHtmlBuilder(item);
                           
                        }
                        $(".ssd_xsd_xdesrx").html('<div style="text-align:center"><a onclick="appendOrder()">点击加载更多...</a></div>');
               
                    } else {
                        html = "";
                        $(".ssd_xsd_xdesrx").html('<div style="text-align:center"><a>找不到更多商品了...</a></div>');
          
                    }
                    if (data.List.NextPage <= 0) {
                        $(".ssd_xsd_xdesrx").html('<div style="text-align:center"><a>找不到更多商品了...</a></div>');
                    }
                    $("#MyOrderList").append(html);
                    $(".ssd_xsd_xdesrx").css('display', 'block');
                    setTimeout(function () {
                        $.getScript('../js/momocha-min.js', function () {
                            fit();
                            mosidebar();
                            spnr();
                            act0();
                        });
                    }, 200);
                } else if (data.Code == -2) {
                    window.location.href = data.Msg;
                } else {
                    $.openDialog(data.Msg);
                }
            },
            error: function () {
                $.closeLoading();
                $.openDialog("网络错误，请稍后再试");
            }
        });
    }, 2000)



}

function orderHtmlBuilder(order) {
    if (order == null) {
        return "";
    }
    var html = '<div class="momocha-hentiao ddh " onclick="GoToddxq(\'' + order.orderNo + '\')">订单编号：' + order.orderNo + '</div>';
    html += '<div class="ddxinxi">';
    html += '<ul>';
    if (order.ListCommodityDetail != null) {
        for (var i = 0; i < order.ListCommodityDetail.length; i++) {
            var item = order.ListCommodityDetail[i];
            html += '<li>';
            html += '<div class="qrdd-img"><a onclick="OpenGoodsDtl(' + item.commodityId + ')"><img src="' + item.image + '"></a></div>';
            html += '<div class="qrdd-nr">';
            html += '<div class="biaoti"><a onclick="OpenGoodsDtl(' + item.commodityId + ')">' + item.commodityName + '</a></div>';
            html += '<div class="fubiaoti">' + item.specification + '</div>';
            html += '<div class="quantity">';
            html += '<div class="qrdd-jiage">¥' + item.price + '</div><div class="qrdd-js">X' + item.quantity + '</div>';
            html += '</div>';
            html += '</div>';
            html += '</li>';
        }
    }

    html += '</ul>';
    html += '<div class="ddh-jg momocha-Appinterval">商品合计：<i>¥' + order.Amount + '</i><i>运费：¥' + order.freight + '</i></div>';
    html += '<div class="momocha-Appinterval  ddcz ">';
    html += '<span>';
    if (order.orderState == 1) {
        html += '<a onclick="cancelOrder(' + order.orderId + ')" class=" ahover">取消订单</a>';
        html += '<a onclick="GoToPayConfirm(\'' + order.orderNo + '\')" class="ico-yes ahover ">付款</a>';
    } else if (order.orderState == 2) {
        html += '<a href="javascript:void(0)" class="ico-no ahover">等待发货</a>';
    } else if (order.orderState == 3) {
        html += '<a onclick="confirmReceipt(' + order.orderId + ')" class="ico-yes ahover">确认收货</a>';
    } else if (order.orderState == 4) {
        html += '<a href="javascript:void(0)" class="ico-no ahover">订单已完成</a>';
    } else if (order.orderState == 5) {
        html += '<a href="javascript:void(0)" class="ico-no ahover">退款中</a>';
    }
    else if (order.orderState >= 6) {
        html += '<a href="javascript:void(0)" class="ico-no ahover">已取消</a>';
    }
    html += '</span>';
    html += '</div>';
    html += '</div>';

    return html;
}
function confirmReceipt(orderId) {
    $.openLoading("确认中");
    $.ajax({
        url: "/PersonalCentre/ConfirmReceipt",
        data: { orderId: orderId },
        type: "post",
        dataType: "json",
        success: function (data) {
            $.closeLoading();
            if (data.Code == 0) {
                alert("确认成功");
                var orderStatus = $("#orderStatus").val();
                window.location.href = ("ddzx.html?goodStatus=" + orderStatus);
            } else if (data.Code == -2) {
                window.location.href = data.Msg;
            } else {
                $.openDialog(data.Msg);
            }
        },
        error: function () {
            $.closeLoading();
            $.openDialog("网络错误，请稍后再试");
        }
    });


}

function cancelOrder(orderId) {
    if (!confirm("您确认要取消订单吗？")) {
        return;
    }

    $.openLoading("取消中");
    $.ajax({
        url: "/PersonalCentre/CancelOrder",
        data: { orderId: orderId },
        type: "post",
        dataType: "json",
        success: function (data) {
            $.closeLoading();
            if (data.Code == 0) {

                alert("取消成功");
                var orderStatus = $("#orderStatus").val();
                window.location.href = ("ddzx.html?goodStatus=" + orderStatus);
            } else if (data.Code == -2) {
                window.location.href = data.Msg;
            } else {
                $.openDialog(data.Msg);
            }
        },
        error: function () {
            $.closeLoading();
            $.openDialog("网络错误，请稍后再试");
        }
    });

}


function GoToPayConfirm(FasorderNo) {

    window.location.href = ("qrdd.html?orderNo=" + FasorderNo);

}

function GoToddxq(FasorderNo) {

    window.location.href = ("ddxq.html?orderNo=" + FasorderNo);

}