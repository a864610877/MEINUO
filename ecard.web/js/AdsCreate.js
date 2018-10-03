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

$("#adsReturn").click(function () {
    window.location.href = "/Ads/AdsList";
})

$("#AddAds").click(function () {
    var title = $("#title").val();  
    var link = $("#link").val();
    var state = $('input[name="state"]:checked').val();
    var rank = $('#rank').val();
    var img1 = $("#shopingImg1").attr("src").substring(28);

    var defaultimg = "/MicroMalls/CommodityImages/shopdefault.jpg";

    if (img1 == defaultimg) {
        defaultimg = "";
    }
    $.ajax({
        url: "/Ads/Create",
        data: {
            title: title, link: link, state: state,
            rank: rank, ImageUrl: img1
        },
        type: "post",
        //cache: false, 
        dataType: "json",
        async: false,
        success: function (data) {
            window.location.href = "/Ads/AdsList";
        },
        error: function () {
            window.location.href = "/Home/UserError";
        }
    })
})