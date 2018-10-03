function bigChange(obj) {
    var smObj = document.getElementsByName("operationPointLogId");
    if (obj.checked == false) {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = false;
    } else {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = true;
    }
}
function smallChange(obj) {
    var smObj = document.getElementsByName("operationPointLogId");
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

    var startTime = $('#startTime').val();
    var endTime = $('#endTime').val();
    $.ajax({
        url: "/OperationPointLog/AjaxList",
        data: {
            PageIndex: PageIndex, PageSize: PageSize,  startTime: startTime,endTime: endTime
        },
        type: "post",
        //cache: false,
        dataType: "json",
        async: false,
        success: function (data) {
            $("#goodslist_checkbox_all").attr("checked", "");
            var listVal = new Array();
            listVal.push("operationPointLogId");
            listVal.push("point");
            listVal.push("account");
            listVal.push("DisplayName");
            listVal.push("Mobile");
            listVal.push("Gender");
            listVal.push("Email");
            listVal.push("remark");
            listVal.push("submitTime");
            listVal.push("boor");
            $.GetstrTrs(data, listVal, "operationPointLogId");

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
        case "Edit":
            window.location.href = RUrl;
            break;
        case "Delete":
            $(".tip p").text("你确定要删除此记录吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
        case "Deletes":
            var id = "";
            $("#tbodysNum input:checked").each(function () {
                id += $(this).val() + ",";
            })
            id = id.substring(0, id.length - 1);
            jsonData = id;
            $(".tip p").text("你确定要删除所选中记录吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
    }
}
