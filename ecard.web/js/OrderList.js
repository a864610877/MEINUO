function bigChange(obj) {
    var smObj = document.getElementsByName("orderId");
    if (obj.checked == false) {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = false;
    } else {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = true;
    }
}
function smallChange(obj) {
    var smObj = document.getElementsByName("orderId");
    var bigObj = document.getElementById("goodslist_checkbox_all");
    if (obj.checked == true)
        bigObj.checked = true;
    else {
        b = true;
        for (var i = 0; i < smObj.length; i++) {
            if (smObj[i].checked == true)
                b = false;
        }
        if (b == true)
            bigObj.checked = false;
    }
}
var pageS = 10;
var index = 0;
var ActionName = "";
var ActionUrl = "";
var jsonData = "";
function selectInput(choose) {
    pageS = choose.value;
    submitClicks(choose);
}
//获取数据
function AjaxGetData(PageIndex, PageSize) {

    var orderNo = $('#orderNo').val();
    var name = $('#name').val();
    var startTime = $('#startTime').val();
    var endTime = $('#endTime').val();
    var orderState = $("[id=orderState.Key]").val();
    var payState = $("[id=payState.Key]").val();
    var distributionstate = $("[id=distributionstate.Key]").val();
    $.ajax({
        url: "/Order/AjaxList",
        data: {
            PageIndex: PageIndex, PageSize: PageSize, orderNo: orderNo, name: name, startTime: startTime,
            endTime: endTime, orderState: orderState, payState: payState, distributionstate: distributionstate
        },
        type: "post",
        //cache: false,
        dataType: "json",
        async: false,
        success: function (data) {
            var listVal = new Array();
            listVal.push("orderId");
            listVal.push("orderNo");
            listVal.push("userName");
            listVal.push("amount");
            listVal.push("orderState");
            listVal.push("payState");
            listVal.push("payType");
            listVal.push("distributionType");
            listVal.push("distributionstate");
            listVal.push("submitTime");
            listVal.push("boor");
            $.GetstrTrs(data, listVal, "orderId");
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
        case "OrderDetail":
            window.location.href = RUrl;
            break;
    }
}
