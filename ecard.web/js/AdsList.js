
$(function () {
    $("#tbodysNum tr").each(function () {
        var a = $(this).find("td");
        a.eq(2).css("width", "650px")
        a.eq(2).find("p").addClass("Namep")
    })


})
function bigChange(obj) {
    var smObj = document.getElementsByName("adId");
    if (obj.checked == false) {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = false;
    } else {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = true;
    }
}
function smallChange(obj) {
    var smObj = document.getElementsByName("adId");
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
    var title = $('#title').val();
    var startTime = $('#startTime').val();
    var endTime = $('#endTime').val();
    var state = $("[id=state.Key]").val();

    $.ajax({
        url: "/Ads/AjaxList",
        data: {
            PageIndex: PageIndex, PageSize: PageSize, title: title, state: state,
            startTime: startTime, endTime: endTime
        },
        type: "post",
        //cache: false,
        dataType: "json",
        async: false,
        success: function (data) {
            var listVal = new Array();
            listVal.push("adId");
            listVal.push("title");
            listVal.push("link");
            listVal.push("state");
            listVal.push("rank");
            listVal.push("submitTime");
            listVal.push("boor");
            $.GetstrTrs1(data, listVal, "adId");
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
        case "Delete":
            $(".tip p").text("你确定要删除此广告吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
        case "Deletes":
            var id = "";
            $("#tbodysNum input[name='adId']:checked").each(function () {
                id += $(this).val() + ",";
            })
            id = id.substring(0, id.length - 1);
            jsonData = id;
            $(".tip p").text("你确定要删除所选中广告吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
        case "Putaway":
            var id = "";
            $("#tbodysNum input:checked").each(function () {
                id += $(this).val() + ",";
            })
            id = id.substring(0, id.length - 1);
            jsonData = id;
            var boo = false;
            $.ajax({
                url: "/Ads/CheckAdsSate",
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

            $(".tip p").text("你确定要将选中的" + $("#tbodysNum input:checked").length + "个广告上架吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
        case "Soldout":
            var id = "";
            $("#tbodysNum input:checked").each(function () {
                id += $(this).val() + ",";
            })
            id = id.substring(0, id.length - 1);
            jsonData = id;
            var boo = false;
            $.ajax({
                url: "/Ads/CheckAdsSate",
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
            $(".tip p").text("你确定要将选中的" + $("#tbodysNum input:checked").length + "个广告下架吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
    }
}
