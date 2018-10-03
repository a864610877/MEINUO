

//$(".forminfo .InputSerach").css("width", s / 100 * 13).css("height", "32px");
$("#file_upload").uploadify({
    swf: '/Scripts/uploadify/uploadify.swf',// 上传使用的 Flash
    uploader: '/Shoping/Upload',    // 服务器端处理地址
    //显示参数
    width: 46,                   // 按钮的宽度
    height: 24,                  // 按钮的高度
    buttonText: "上传",      // 按钮上的文字
    buttonCursor: "hand",        // 按钮的鼠标图标
    buttonClass: "up_button2",
    fileObjName: 'Filedata',     // 上传参数名称
    auto: true,
    //规则参数
    fileSizeLimit: "5000KB",
    fileTypeExts: "*.jpg;*.jpeg;*.png;*.gif",//允许上传的文件扩展名 和下面一起配合使用
    fileTypeDesc: "请选择 jpg、jpeg、png、gif 文件",// 文件说明
    fileSizeLimit: "10MB",         //允许上传的文件大小
    multi: false,                 // 是否支持同时上传多个文件
    removeTimeout: 1,
    onUploadSuccess: function (file, data, respose) {
        var obj = eval("(" + data + ")") //把返回的数据序列化为Obj对象
        if (obj.ret == true) {
            document.getElementById("shopingImg1").src = obj.FilePath;

        } else {
            alert(obj.message);
        }

    }
})
$("#file_upload1").uploadify({
    swf: '/Scripts/uploadify/uploadify.swf',// 上传使用的 Flash
    uploader: '/Shoping/Upload',    // 服务器端处理地址
    //显示参数
    width: 46,                   // 按钮的宽度
    height: 24,                  // 按钮的高度
    buttonText: "上传",      // 按钮上的文字
    buttonCursor: "hand",        // 按钮的鼠标图标
    buttonClass: "up_button2",
    fileObjName: 'Filedata',     // 上传参数名称
    auto: true,
    //规则参数
    fileSizeLimit: "5000KB",
    fileTypeExts: "*.jpg;*.jpeg;*.png;*.gif",//允许上传的文件扩展名 和下面一起配合使用
    fileTypeDesc: "请选择 jpg、jpeg、png、gif 文件",// 文件说明
    fileSizeLimit: "10MB",         //允许上传的文件大小
    multi: false,                 // 是否支持同时上传多个文件
    removeTimeout: 1,
    onUploadSuccess: function (file, data, respose) {
        var obj = eval("(" + data + ")") //把返回的数据序列化为Obj对象
        if (obj.ret == true) {
            document.getElementById("shopingImg2").src = obj.FilePath;


        } else {
            alert(obj.message);
        }

    }
})
$("#file_upload2").uploadify({
    swf: '/Scripts/uploadify/uploadify.swf',// 上传使用的 Flash
    uploader: '/Shoping/Upload',    // 服务器端处理地址
    //显示参数
    width: 46,                   // 按钮的宽度
    height: 24,                  // 按钮的高度
    buttonText: "上传",      // 按钮上的文字
    buttonCursor: "hand",        // 按钮的鼠标图标
    buttonClass: "up_button2",
    fileObjName: 'Filedata',     // 上传参数名称
    auto: true,
    //规则参数
    fileSizeLimit: "5000KB",
    fileTypeExts: "*.jpg;*.jpeg;*.png;*.gif",//允许上传的文件扩展名 和下面一起配合使用
    fileTypeDesc: "请选择 jpg、jpeg、png、gif 文件",// 文件说明
    fileSizeLimit: "10MB",         //允许上传的文件大小
    multi: false,                 // 是否支持同时上传多个文件
    removeTimeout: 1,
    onUploadSuccess: function (file, data, respose) {
        var obj = eval("(" + data + ")") //把返回的数据序列化为Obj对象
        if (obj.ret == true) {
            document.getElementById("shopingImg3").src = obj.FilePath;


        } else {
            alert(obj.message);
        }

    }
})

$("#file_upload4").uploadify({
    'onInit': function () {                        //载入时触发，将flash设置到最小
        $("#uploadify-queue").hide();
    },
    swf: '/Scripts/uploadify/uploadify.swf',// 上传使用的 Flash
    uploader: '/Shoping/Upload',    // 服务器端处理地址
    //显示参数
    width: 46,                   // 按钮的宽度
    height: 24,                  // 按钮的高度
    buttonText: "上传",      // 按钮上的文字
    buttonCursor: "hand",        // 按钮的鼠标图标
    buttonClass: "up_button2",
    fileObjName: 'Filedata',     // 上传参数名称
    auto: true,
    //规则参数
    fileSizeLimit: "5000KB",
    fileTypeExts: "*.jpg;*.jpeg;*.png;*.gif",//允许上传的文件扩展名 和下面一起配合使用
    fileTypeDesc: "请选择 jpg、jpeg、png、gif 文件",// 文件说明
    fileSizeLimit: "10MB",         //允许上传的文件大小
    multi: false,                 // 是否支持同时上传多个文件
    removeTimeout: 1,
    onUploadSuccess: function (file, data, respose) {
        var obj = eval("(" + data + ")") //把返回的数据序列化为Obj对象
        if (obj.ret == true) {
            document.getElementById("shopingImg4").src = obj.FilePath;


        } else {
            alert(obj.message);
        }

    }
})

$("#file_upload5").uploadify({
    'onInit': function () {                        //载入时触发，将flash设置到最小
        $("#uploadify-queue").hide();
    },
    swf: '/Scripts/uploadify/uploadify.swf',// 上传使用的 Flash
    uploader: '/Shoping/Upload',    // 服务器端处理地址
    //显示参数
    width: 46,                   // 按钮的宽度
    height: 24,                  // 按钮的高度
    buttonText: "上传",      // 按钮上的文字
    buttonCursor: "hand",        // 按钮的鼠标图标
    buttonClass: "up_button2",
    fileObjName: 'Filedata',     // 上传参数名称
    auto: true,
    //规则参数
    fileSizeLimit: "5000KB",
    fileTypeExts: "*.jpg;*.jpeg;*.png;*.gif",//允许上传的文件扩展名 和下面一起配合使用
    fileTypeDesc: "请选择 jpg、jpeg、png、gif 文件",// 文件说明
    fileSizeLimit: "10MB",         //允许上传的文件大小
    multi: false,                 // 是否支持同时上传多个文件
    removeTimeout: 1,
    onUploadSuccess: function (file, data, respose) {
        var obj = eval("(" + data + ")") //把返回的数据序列化为Obj对象
        if (obj.ret == true) {
            document.getElementById("shopingImg5").src = obj.FilePath;


        } else {
            alert(obj.message);
        }

    }
});

$("#file_upload6").uploadify({
    'onInit': function () {                        //载入时触发，将flash设置到最小
        $("#uploadify-queue").hide();
    },
    swf: '/Scripts/uploadify/uploadify.swf',// 上传使用的 Flash
    uploader: '/Shoping/Upload',    // 服务器端处理地址
    //显示参数
    width: 46,                   // 按钮的宽度
    height: 24,                  // 按钮的高度
    buttonText: "上传",      // 按钮上的文字
    buttonCursor: "hand",        // 按钮的鼠标图标
    buttonClass: "up_button2",
    fileObjName: 'Filedata',     // 上传参数名称
    auto: true,
    //规则参数
    fileSizeLimit: "5000KB",
    fileTypeExts: "*.jpg;*.jpeg;*.png;*.gif",//允许上传的文件扩展名 和下面一起配合使用
    fileTypeDesc: "请选择 jpg、jpeg、png、gif 文件",// 文件说明
    fileSizeLimit: "10MB",         //允许上传的文件大小
    multi: false,                 // 是否支持同时上传多个文件
    removeTimeout: 1,
    onUploadSuccess: function (file, data, respose) {
        var obj = eval("(" + data + ")") //把返回的数据序列化为Obj对象
        if (obj.ret == true) {
            document.getElementById("shopingImg6").src = obj.FilePath;


        } else {
            alert(obj.message);
        }

    }
});


$("#shopingReturn").click(function () {
    window.location.href = "/Shoping/ShopingList";
})

$(".shanchu").click(function () {
    var s = $(this).siblings('div').find("img");
    var url = s.attr("src");
    if (url == "/MicroMalls/CommodityImages/shopdefault.jpg") {
        return;
    }
    $.ajax({
        url: "/Shoping/DeleteUpload",
        data: { url: url },
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

$("#tag_class_edit").click(function () {
    $("#goods_edit_tag_class").fadeIn();
})

$("#finish_tag_item").click(function () {
    $("#goods_edit_tag_class").fadeOut();
})

//商品规格属性添加按钮点击
$("#guigeAdd").click(function () {


    var Name = $("input[name='Name']").val();
    var describetxt = $("#describetxt").val();
    var describeimg = $("#describeimg").val();
    var type = $("input[name='Type']:checked").val();
    if (Name == "" || Name == null) {
        alert("规格名称不能未空！");
        return;
    }
    //var a = true;
    //$.ajax({
    //    url: "/Specification/CheckName",
    //    data: { Name: Name },
    //    type: "post",
    //    //cache: false, 
    //    dataType: "json",
    //    async: false,
    //    success: function (data) {
    //        if (data == 2) {
    //            alert("规格名称不能重复！");
    //            a = false;
    //        }
    //    },
    //    error: function () {
    //        window.location.href = "/Home/UserError";
    //    }
    //})
    //if (!a) {
    //    return;
    //}

    if (type == "" || type == null) {
        alert("规格类型必选！");
        return;
    }

    var showType = $("input[name='showType']").val();


    var value = "";
    if ($(".shuxingzhi_txt").css("display") == "block") {
        $(".shuxingzhi_txt li").each(function () {
            value += $(this).text() + "/" + $(this).find("input").val() + ",";
        })

    }
    if ($(".shuxingzhi_img").css("display") == "block") {
        $(".shuxingzhi_img li img").each(function () {
            var imgname = $(this).attr("src").substring(28);
            value += imgname + "/" + $(this).next("input").val() + ",";
        })
    }
    value = value.substring(0, value.length - 1);
    $.ajax({
        url: "/Specification/Create",
        data: { Name: Name, Type: type, showType: showType, value: value},
        type: "post",
        //cache: false, 
        dataType: "json",
        async: false,
        success: function (data) {
            $("#Name").val("");
            $(".shuxingzhi_jiashang").val("");
            $(".dizhi_suxing_i").val("");
            $(".shuxingzhi_img  li").remove();
            $(".shuxingzhi_txt  li").remove();

            $(".alertBox").fadeOut();
            var ddl = $("#specification");
            //删除节点
            $("#specification option").remove();
            //方法1：添加默认节点 
            ddl.append("<option value='-1'>--请选择--</option>");
            //转成Json对象
            var result = eval(data);
            //循环遍历 下拉框绑定
            $(result).each(function (key) {
                ddl.append("<option value='" + result[key].Key + "'>" + result[key].Name + "</option>");

            });
        },
        error: function () {
            window.location.href = "/Home/UserError";
        }
    })
})

$("#confirm_tag_item").click(function () {
    var text = $('#specification option:selected').text();//选中的文本
    var value = $('#specification option:selected').val();//选中的值

    if (value == -1 || value == "") {
        return;
    }
    var bo = false;
    $(".txet_suxing").each(function () {
        var setext = $(this).find('dt').text();

        if (text == setext) {
            alert("商品已存在此规格");
            bo = true;
        }
    })
    if (bo) {
        return;
    }
    $(".shuxingzhi_wrap").append("<dl class='txet_suxing'><dt>" + text + "</dt><dd><input type=hidden value=" + value + "><ul id='" + text + "'></ul></dd><dd class='shanchushuxing'><a href='javascript:void(0)' onclick='DeleteSp(this)' title='删除属性'></a></dd></dl>");


    $.ajax({
        url: "/Specification/GetSpecificationDetails",
        data: { id: value },
        type: "post",
        //cache: false, 
        dataType: "json",
        async: false,
        success: function (data) {
            //转成Json对象
            var result = eval(data.sp);
            $(result).each(function (key) {
                if (data.type == 1) {
                    $("#" + text + "").append("<li>" + result[key].value + "</li>");
                } else {
                    $("#" + text + "").append("<li><img width='33px' height='33px' src='/MicroMalls/CommodityImages/" + result[key].value + "'></li>");
                }

            });

        },
        error: function () {
            window.location.href = "/Home/UserError";
        }
    })
});

//保存按钮点击保存商品信息
$("#Addshoping").click(function () {
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
    var le = $(".txet_suxing").length;
    //if (le < 1) {
    //    alert("商品属性是必须的");
    //    return;
    //}
    var bo = false;
    $.ajax({
        url: "/Shoping/CheckcommodityNo",
        data: { commodityNo: commodityNo },
        type: "post",
        //cache: false, 
        dataType: "json",
        async: false,
        success: function (data) {
            if (data == 2) {

                alert("商品编码已存在,请重新输入");
            } else {
                bo = true;
            }
        },
        error: function () {
            window.location.href = "/Home/UserError";
        }
    })


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
    if (!bo) {
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
        url: "/Shoping/Create",
        data: {
            commodityCategoryId: commodityCategoryId, commodityJifen: commodityJifen,
            commodityName: commodityName, commodityRemark: commodityRemark, commdityKeyword: commdityKeyword,
            commodityPrice: commodityPrice, commodityInventory: commodityInventory, commodityNo: commodityNo, commodityFreight: commodityFreight
            ,commodityRank: commodityRank, commodityDetails: commodityDetails, images: images, specificationId: specificationId, IsPinkage: isPinkage
        },
        type: "post",
        //cache: false, 
        dataType: "json",
        async: false,
        success: function (data) {
            document.getElementById("shopingImg1").src = "/CommodityImages/shopdefault.jpg";
            document.getElementById("shopingImg2").src = "/CommodityImages/shopdefault.jpg";
            document.getElementById("shopingImg3").src = "/CommodityImages/shopdefault.jpg";
            alert(data);
            window.location.href = "/Shoping/ShopingList";
        },
        error: function () {
            window.location.href = "/Home/UserError";
        }
    })


})



//限制输入的字符数
//var chackTextarea = function (obj, num, objTip, name) {
//    setInterval(function () {
//        var newvalue = obj.value.replace(/[^\x00-\xff]/g, "**");
//        if (newvalue.length >= 0) {
//            if (newvalue.length > num) {
//                objTip.innerHTML = name + "已超出<em>" + parseInt((newvalue.length - num) / 2) + "</em>个字!";
//                objTip.style.color = "#F00";
//                return;
//            } else {
//                objTip.innerHTML = name + "还能输入<em>" + parseInt((num - newvalue.length) / 2) + "</em>个字!";
//                objTip.style.color = "#588905";
//            }
//        }
//    }, 100)

//}
//chackTextarea(document.getElementById("commodityName"), 100, document.getElementById("tip"), "商品名称");
//chackTextarea(document.getElementById("commodityRemark"), 100, document.getElementById("tip1"), "商品备注");
//chackTextarea(document.getElementById("commdityKeyword"), 100, document.getElementById("tip2"), "搜索文字");

//删除属性值
function deleteValue(obj) {
    var $td = $(obj).parent().prev();
    $td.find("ul li").remove();
}
