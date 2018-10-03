
var pageS = 10;
var index = 0;
var ActionName = "";
var ActionUrl = "";
var jsonData = "";
$(function () {
    //返回
    $("#OrderReturn").click(function () {
        window.location.href = "/Order/OrderList"

    })
    //保存按钮
    $("#orderEdit").click(function () {
        ActionName = "EditOrder";
        ActionUrl = "/Order/EditOrder/";
        if (confirm("你确定要保存此次修改吗？")) {
            var orderId = $("#orderId").val();
            var orderState = $("#orderState").val();
            var payType = $("#payType").val();
            var point = $("#point").val();
            var payState = $("#payState").val();
            var distributionstate = $("#distributionstate").val();
            var distributionType = $("#distributionType").val();
            var ExpressCompany = $("#ExpressCompany").val();
            var province = $("#province").val();
            var city = $("#city").val();
            var recipients = $("#recipients").val();
            var detailedAddress = $("#detailedAddress").val();
            var zipCode = $("#zipCode").val();
            var moblie = $("#moblie").val();
            var phone = $("#phone").val();
            jsonData = {
                orderId: orderId, orderState: orderState, payType: payType, payState: payState, distributionstate: distributionstate,
                ExpressCompany: ExpressCompany, provinceId: province, recipients: recipients, detailedAddress: detailedAddress, zipCode: zipCode, moblie: moblie, phone: phone
                , cityId: city, distributionType: distributionType,point:point
            };
            $.ajax({
                url: ActionUrl,
                data: jsonData ,
                type: "post",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.Code == 1) {
                        alert(data.CodeText)
                    }
                }
            })
        }
    })

    //省份下拉框改变事件
    $("#province").change(function () {
        var provinceid = $(this).val();
        $.ajax({
            url: "/Order/GetCityByProvinceId",
            data: { id: provinceid },
            type: "post",
            //cache: false, 
            dataType: "json",
            async: false,
            success: function (data) {
                $("#city").empty();
                $("#city").append("<option value=''>--请选择城市--</option>");
                for (var i = 0; i < data.length; i++) {
                    $("#city").append("<option value=" + data[i].CityId + ">" + data[i].Name + "</option>");
                }
            },
            error: function () {
                window.location.href = "/Home/UserError";
            }
        })
    })

    $("#orderAgree").click(function () {
        var amount = $("#payAmount").val();
        if (!confirm("您确定要同意退款吗,本次退款将退"+amount+"元！")) {
            return;
        }
        var orderId = $("#orderId").val();
        $("#orderEdit").attr("disabled", "disabled");
        $("#orderAgree").attr("disabled", "disabled");
        $("#orderAgree").text("处理中");
        $.ajax({
            url: "/order/AgreeRefund",
            data: { orderId: orderId },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.Code == 0) {
                    alert("处理成功，已退款");
                    $("#orderAgree").text("处理成功，已退款");
                }
                else {
                    alert(data.CodeText);
                    $("#orderEdit").removeAttr("disabled");
                    $("#orderAgree").removeAttr("disabled");
                    $("#orderAgree").text("同意");
                }
            }
        });
    })


    $("#orderShip").click(function () {
        ActionName = "EditOrder";
        ActionUrl = "/Order/Ship/";
        if (confirm("你确定要发货吗？")) {
            var orderId = $("#orderId").val();
            var ExpressCompany = $("#ExpressCompany").val();
            var ExpressNumber = $("#ExpressNumber").val();
            if (ExpressCompany == null || ExpressNumber == "") {
                alert("请输入快递公司");
                return;
            }
            if (ExpressNumber == null || ExpressNumber == "") {
                alert("请输入快递单号");
                return;
            }
            jsonData = {
                orderId: orderId, expressCompany: ExpressCompany, expressNumber: ExpressNumber
            };
            $.ajax({
                url: ActionUrl,
                data: jsonData,
                type: "post",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.Code == 0) {
                        alert("发货成功");
                        $("#orderShip").attr("disabled", true);
                    } else {
                        alert(data.CodeText);
                    }
                }
            })
        }
    });
    $("#orderCancel").click(function () {
        ActionName = "EditOrder";
        ActionUrl = "/Order/cancelOrder/";
        if (confirm("你确定要取消订单吗？")) {
            var orderId = $("#orderId").val();
           
            jsonData = {
                orderId: orderId
            };
            $.ajax({
                url: ActionUrl,
                data: jsonData,
                type: "post",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.Code == 0) {
                        alert("取消成功");
                        $("#orderCancel").attr("disabled", true);
                    } else {
                        alert(data.CodeText);
                    }
                }
            })
        }
    });
    
})

function selectInput(choose) {
    pageS = choose.value;
    submitClicks(choose);
}
//获取数据
function AjaxGetData(PageIndex, PageSize) {
    var orderId = $("#orderId").val();
    $.ajax({
        url: "/Order/AjaxList1",
        data: { PageIndex: PageIndex, PageSize: PageSize, orderId: orderId },
        type: "post",
        //cache: false,
        dataType: "json",
        async: false,
        success: function (data) {
            var listVal = new Array();
            listVal.push("commodityId");
            listVal.push("commodityName");
            listVal.push("price");
            listVal.push("quantity");
            listVal.push("boor");
            $.GetstrTrs(data, listVal);
        },
        error: function () {
            window.location.href = "/Home/Error";
        }
    })
}

//操作
function OperatorThis(RName, RUrl) {
    ActionName = RName;
    ActionUrl = RUrl;
    switch (RName) {
        case "Delete":
            $(".tip p").text("你确定要删除此商品信息吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
    }
}

