var img = $("#imghidde").val();
var imgstr = img.split(',')

document.getElementById("shopingImg1").src = "/MicroMalls/CommodityImages/" + imgstr[0];
document.getElementById("shopingImg2").src = "/MicroMalls/CommodityImages/" + imgstr[1];
document.getElementById("shopingImg3").src = "/MicroMalls/CommodityImages/" + imgstr[2];
document.getElementById("shopingImg4").src = "/MicroMalls/CommodityImages/" + imgstr[3];
document.getElementById("shopingImg5").src = "/MicroMalls/CommodityImages/" + imgstr[4];
document.getElementById("shopingImg6").src = "/MicroMalls/CommodityImages/" + imgstr[5];

$("#Editshoping").click(function () {
    if (confirm("确定要修改商品吗")) {
        var commodityName = $("#commodityName").val();  //商品名称
        var commodityRemark =$("#commodityRemark").val();//商品备注
        var commdityKeyword = ""; //$("#commdityKeyword").val();//搜索文字
        var commodityFreight = $("#commodityFreight").val();//商品运费
        var commodityPrice = $("#commodityPrice").val();//商品价格
        var commodityInventory = $("#commodityInventory").val();//商品数量
        var commodityNo = $("#commodityNo").val();//商品编码
        var commodityRank = $("#commodityRank").val();//商品排序
        var commodityDetails = editor.html();  //商品详情
        var commodityCategoryId = $('#CommodityCategorysList option:selected').val();//选中的值
        var commodityJifen = $("#commodityJifen").val();//商品积分
        var isPinkage = false;
        if ($('#IsPinkage').attr("checked") == true) {
            isPinkage = true;
        }
        var commodityId = $("#CommIdhidde").val();
        if (commodityName == "" || commodityName == null) {
            alert("商品名称不能够为空！");
            return;
        }

        var img1 = $("#shopingImg1").attr("src");
        var img2 = $("#shopingImg2").attr("src");
        var img3 = $("#shopingImg3").attr("src");
        var img4 = $("#shopingImg4").attr("src");
        var img5 = $("#shopingImg5").attr("src");
        var img6 = $("#shopingImg6").attr("src");
        var defaultimg = "/MicroMalls/CommodityImages/shopdefault.jpg";
        var images = "";
        if (img1 == defaultimg && img2 == defaultimg && img3 == defaultimg) {
            alert("商品图片必须上传");
            return;
        }
        if (img4 == defaultimg && img5 == defaultimg && img6 == defaultimg) {
            alert("请上传至少一张商品详情图");
            return;
        }

        images = img1.substring(28) + "," + img2.substring(28) + "," + img3.substring(28);
        if (img4 != defaultimg)
            images += "," + img4.substring(28);
        if (img5 != defaultimg)
            images += "," + img5.substring(28);
        if (img6 != defaultimg)
            images += "," + img6.substring(28);

        var specificationId = "";
        $(".txet_suxing input").each(function () {
            specificationId += $(this).val() + ",";
        })
        specificationId = specificationId.substring(0, specificationId.length - 1);

        $.ajax({
            url: "/Shoping/Edit",
            data: {
                commodityId: commodityId, commodityName: commodityName, commodityRemark: commodityRemark, commdityKeyword: commdityKeyword,
                commodityPrice: commodityPrice, commodityInventory: commodityInventory, commodityNo: commodityNo, commodityFreight: commodityFreight
                , commodityRank: commodityRank, commodityDetails: commodityDetails, images: images, specificationId: specificationId, IsPinkage: isPinkage,
                commodityCategoryId: commodityCategoryId, commodityJifen: commodityJifen
            },
            type: "post",
            //cache: false, 
            dataType: "json",
            async: false,
            success: function (data) {
                alert(data.CodeText);
                window.location.href = "/Shoping/ShopingList";
                //$(".tipinfo2 img").attr("src", "../../images/Succeed01.png");
                //$(".tipright2 P").text(data.CodeText);
                //$(".ShowHide").fadeIn(100);
                //$(".tip2").fadeIn(200);
            },
            error: function () {
                window.location.href = "/Home/UserError";
            }
        })
    }


})
//返回
$("#shopingReturn").click(function () {
    window.location.href = "/Shoping/ShopingList"

})

//删除图片
$(".shanchuEdit").click(function () {
    var s = $(this).siblings('div').find("img");
    var url = s.attr("src");
    if (url == "/MicroMalls/CommodityImages/shopdefault.jpg") {
        return;
    }
    var commodityId = $("#CommIdhidde").val();

    $.ajax({
        url: "/Shoping/DeleteImg",
        data: { url: url, id: commodityId },
        type: "post",
        //cache: false, 
        dataType: "json",
        async: false,
        success: function (data) {
            if (data == 1) {
                s.attr("src", "/MicroMalls/CommodityImages/shopdefault.jpg");
                alert("删除成功！");
            } else {
                alert("删除失败！");
            }
        },
        error: function () {
            window.location.href = "/Home/UserError";
        }
    })
})


//删除属性
function DeleteSp(obj) {
    var le = $(".txet_suxing").length;
    if (le < 2) {
        alert("规格是必须的");
        return;
    }
    if (confirm("确定要删除此规格吗？")) {
        $(obj).parent().parent().remove();

        //var spid = $(obj).parent().prev().find("input").val();
        //var id = $("#CommIdhidde").val();
        //$.ajax({
        //    url: "/Shoping/DeleteSp",
        //    data: { spid: spid, id: id },
        //    type: "post",
        //    //cache: false, 
        //    dataType: "json",
        //    async: false,
        //    success: function (data) {
        //        $(obj).parent().parent().remove();
        //    },
        //    error: function () {
        //        window.location.href = "/Home/UserError";
        //    }
        //})
    }

}