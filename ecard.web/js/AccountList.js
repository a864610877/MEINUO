function bigChange(obj) {
    var smObj = document.getElementsByName("accountId");
    if (obj.checked == false) {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = false;
    } else {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = true;
    }
}
function smallChange(obj) {
    var smObj = document.getElementsByName("accountId");
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

    var DisplayName = $('#DisplayName').val();
    var Mobile = $('#Mobile').val();
    var startTime = $('#startTime').val();
    var endTime = $('#endTime').val();
    $.ajax({
        url: "/Account/AjaxList",
        data: {
            PageIndex: PageIndex, PageSize: PageSize, DisplayName: DisplayName, Mobile: Mobile, startTime: startTime,
            endTime: endTime
        },
        type: "post",
        //cache: false,
        dataType: "json",
        async: false,
        success: function (data) {
            $("#goodslist_checkbox_all").attr("checked", "");
            var listVal = new Array();
            listVal.push("accountId");
            listVal.push("salerName");
            listVal.push("DisplayName");
            listVal.push("Mobile");
            listVal.push("Gender");
            listVal.push("grade");
            //listVal.push("State");
            listVal.push("presentExp");
            listVal.push("activatePoint");
            listVal.push("submitTime");
            //listVal.push("Address");
            
           // listVal.push("orangeKey");
            //listVal.push("qrCodeUrl");
            //listVal.push("ticket");
            listVal.push("boor");
            $.GetstrTrs1(data, listVal, "accountId");
            $("#tbodysNum tr").each(function () {
                var a = $(this).find("td");
                a.eq(8).css("width", "230px")
                a.eq(9).css("width", "230px")
                a.eq(8).find("p").addClass("nap")
                a.eq(9).find("p").addClass("nap")
            })
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
            $(".tip p").text("你确定要删除此会员吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
        case "GetSaleAmount":
            $("#saleAmount").html("0")
            var strs = RUrl.split("/");
            if (strs.length < 3) {
                alert("错误");
                return;
            }
            var accountId = strs[3];
            $.ajax({
                url: "/Account/GetSaleAmount",
                data: { Id: accountId, State: 2 },
                type: "post",
                dataType: "json",
                async: false,
                success: function (data) {
                    if(data.Code == 0) {
                      $("#saleAmount").html(data.CodeText);
                      $(".ShowHide").fadeIn(100);
                      $(".saleAmount").fadeIn(200);
                    }
                },
                error: function () {
                    window.location.href = "/Home/Error";
                }
            })
            
            break;
        case "DeleteAccount":
                var id = "";
                $("#tbodysNum input:checked").each(function () {
                    id += $(this).val() + ",";
                })
                id = id.substring(0, id.length - 1);
                jsonData = id;
                $(".tip p").text("你确定要删除所选中会员吗？");
                $(".ShowHide").fadeIn(100);
                $(".tip").fadeIn(200);
                break;
        case "BatchOutage":
            var id = "";
            $("#tbodysNum input:checked").each(function () {
                id += $(this).val() + ",";
            })
            id = id.substring(0, id.length - 1);
            jsonData = id;
            var boo = false;
            $.ajax({
                url: "/Account/CheckAccountSate",
                data: { strIds: id, State: 2 },
                type: "post",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.Code == 1) {
                        $(".tipinfo2 img").attr("src", "../../images/error02.png");
                        $(".tipright2 P").text(data.CodeText);
                        $(".ShowHide").fadeIn(100);
                        $(".tip2").fadeIn(200);
                        boo = true;
                    }

                },
                error: function () {
                    window.location.href = "/Home/Error";
                }
            })
            if (boo) {
                return;
            }

            $(".tip p").text("你确定要将选中的" + $("#tbodysNum input:checked").length + "个会员停用吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
        case "BatchEnabled":
            var id = "";
            $("#tbodysNum input:checked").each(function () {
                id += $(this).val() + ",";
            })
            id = id.substring(0, id.length - 1);
            jsonData = id;
            var boo = false;
            $.ajax({
                url: "/Account/CheckAccountSate",
                data: { strIds: id, State: 1 },
                type: "post",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.Code == 1) {
                        $(".tipinfo2 img").attr("src", "../../images/error02.png");
                        $(".tipright2 P").text(data.CodeText);
                        $(".ShowHide").fadeIn(100);
                        $(".tip2").fadeIn(200);
                        boo = true;
                    }

                },
                error: function () {
                    window.location.href = "/Home/Error";
                }
            })
            if (boo) {
                return;
            }

            $(".tip p").text("你确定要将选中的" + $("#tbodysNum input:checked").length + "个会员启用吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
    }
}
