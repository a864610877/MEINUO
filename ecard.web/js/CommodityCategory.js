//add
$("#AddCategory").click(function () {
    var name = $("#name").val();
    if (name == null || name == "") {
        alert("请输入名称");
        return;
    }

    $.ajax({
        url: "/CommodityCategory/Create",
        data: {
            name: name
        },
        type: "post",
        //cache: false, 
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.Code == 0) {
                alert("添加成功");
                window.location.href = "/CommodityCategory/List";
            } else {
                alert(data.CodeText);
            }
            //window.location.href = "/Ads/AdsList";
        },
        error: function () {
            window.location.href = "/Home/UserError";
        }
    })
});
//list
$(function () {
    $("#tbodysNum tr").each(function () {
        var a = $(this).find("td");
        a.eq(2).css("width", "650px")
        a.eq(2).find("p").addClass("Namep")
    })


})
function bigChange(obj) {
    var smObj = document.getElementsByName("commodityCategoryId");
    if (obj.checked == false) {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = false;
    } else {
        for (var i = 0; i < smObj.length; i++)
            smObj[i].checked = true;
    }
}
function smallChange(obj) {
    var smObj = document.getElementsByName("commodityCategoryId");
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
    var name = $('#name').val();
    $.ajax({
        url: "/CommodityCategory/AjaxList",
        data: {
            PageIndex: PageIndex, PageSize: PageSize, name: name
        },
        type: "post",
        //cache: false,
        dataType: "json",
        async: false,
        success: function (data) {
            var listVal = new Array();
            listVal.push("commodityCategoryId");
            listVal.push("Name");
            listVal.push("boor");
            $.GetstrTrs1(data, listVal, "commodityCategoryId");
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
            $(".tip p").text("你确定要删除此分类吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
        case "Deletes":
            var id = "";
            $("#tbodysNum input[name='commodityCategoryId']:checked").each(function () {
                id += $(this).val() + ",";
            })
            id = id.substring(0, id.length - 1);
            jsonData = id;
            $(".tip p").text("你确定要删除所选中分类吗？");
            $(".ShowHide").fadeIn(100);
            $(".tip").fadeIn(200);
            break;
    }
}
