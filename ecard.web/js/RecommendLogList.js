function bigChange(obj) {
    var smObj = document.getElementsByName("recommendLogId");
    if (obj.checked == false) {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = false;
    } else {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = true;
    }
}
function smallChange(obj) {
    var smObj = document.getElementsByName("recommendLogId");
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

    var salerName = $('#salerName').val();
    var userName = $('#userName').val();
    var startTime = $('#startTime').val();
    var endTime = $('#endTime').val();
    $.ajax({
        url: "/RecommendLog/AjaxList",
        data: {
            PageIndex: PageIndex, PageSize: PageSize, salerName: salerName, userName: userName, startTime: startTime,
            endTime: endTime
        },
        type: "post",
        //cache: false,
        dataType: "json",
        async: false,
        success: function (data) {
            $("#goodslist_checkbox_all").attr("checked", "");
            var listVal = new Array();
            listVal.push("recommendLogId");
            listVal.push("salerName");
            listVal.push("saler");
            listVal.push("salerphone");
            listVal.push("userName");
            listVal.push("user");
            listVal.push("userphone");
            listVal.push("remark");
            listVal.push("submitTime");
            listVal.push("boor");
            $.GetstrTrs(data, listVal, "recommendLogId");

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
