
$(function () {
    $("#tbodysNum tr").each(function () {
        var a = $(this).find("td");
        a.eq(2).css("width","650px")
        a.eq(2).find("p").addClass("Namep")
    })


})
function bigChange(obj) {
    var smObj = document.getElementsByName("commodityId");
    if (obj.checked == false) {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = false;
    } else {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = true;
    }
}
function smallChange(obj) {
    var smObj = document.getElementsByName("commodityId");
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
    var commodityName = $('#commodityName').val();
    var commodityNo = $('#commodityNo').val();
    var commdityKeyword = $('#commdityKeyword').val();
    var commodityState = $("[id=commodityState.Key]").val();

    $.ajax({
        url: "/Shoping/AjaxList",
        data: {
            PageIndex: PageIndex, PageSize: PageSize, commodityName: commodityName, commodityNo: commodityNo,
            commdityKeyword: commdityKeyword, commodityState: commodityState
        },
        type: "post",
        //cache: false,
        dataType: "json",
        async: false,
        success: function (data) {
            var listVal = new Array();
            listVal.push("commodityId");
            listVal.push("commodityNo");
            listVal.push("commodityName");
            listVal.push("commodityPrice");
            listVal.push("commodityFreight");
            listVal.push("commdityKeyword");
            listVal.push("sellQuantity");
            listVal.push("commodityState");
            listVal.push("boor");
            $.GetstrTrs1(data, listVal, "commodityId");
            $("#tbodysNum tr").each(function () {
                var a = $(this).find("td");
                a.eq(2).css("width", "650px")
                a.eq(2).find("p").addClass("Namep")
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
        case "DeleteShoping":
            $(".tip p").text("你确定要删除此商品吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
        case "DeleteShopings":
            var id = "";
            $("#tbodysNum input[name='commodityId']:checked").each(function () {
                id += $(this).val() + ",";
            })
            id = id.substring(0, id.length - 1);
            jsonData = id;
            $(".tip p").text("你确定要删除所选中商品吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
        case "ShoinpsPutaway":
            var id = "";
            $("#tbodysNum input:checked").each(function () {
                id += $(this).val() + ",";
            })
            id = id.substring(0, id.length - 1);
            jsonData = id;
            var boo = false;
            $.ajax({
                url: "/Shoping/CheckShopingSate",
                data: { strIds: id, State: 2},
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

            $(".tip p").text("你确定要将选中的" + $("#tbodysNum input:checked").length + "件商品上架吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
        case "ShoinpSoldout":
            var id = "";
            $("#tbodysNum input:checked").each(function () {
                id += $(this).val() + ",";
            })
            id = id.substring(0, id.length - 1);
            jsonData = id;
            var boo = false;
            $.ajax({
                url: "/Shoping/CheckShopingSate",
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
            $(".tip p").text("你确定要将选中的" + $("#tbodysNum input:checked").length + "件商品下架吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
    }
}
