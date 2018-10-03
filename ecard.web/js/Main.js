$(function () {
    $(".tiptop a").click(function () {
        $(".tip").fadeOut(200);
        $(".ShowHide").fadeOut(100);
    });
    $(".tipCheckSure a").click(function () {
        $(".tipCheckSure").fadeOut(200);
        $(".ShowHide").fadeOut(100);
    });

    $(".sure").click(function () {
        $(".tip").fadeOut(100);
        $(".ShowHide").fadeOut(100);
    });

    $(".cancel").click(function () {
        $(".tip").fadeOut(100);
        $(".ShowHide").fadeOut(100);
    })
    $(".tipDivTop a").click(function () {
        $(".tipDiv").fadeOut(200);
        $(".ShowHide").fadeOut(100);
    });
    if ($("#AllCheck").length > 0) {
        $("#AllCheck").live("click", function () {
            var flag = $(this).attr("checked");
            if (flag) {
                $("#tbodysNum [type='checkbox']").attr("checked", true);
            } else {
                $("#tbodysNum [type='checkbox']").attr("checked", false);
            }
        })
    }
    if ($("#tbodysNum [type='checkbox']").length > 0) {
        $("#tbodysNum [type='checkbox']").live("click", function () {

            if ($("#tbodysNum [type='checkbox']:checked").length == $("#tbodysNum [type='checkbox']").length) {
                $("#AllCheck").attr("checked", true);
            } else {
                $("#AllCheck").attr("checked", false);
            }
        })
    }
    

    $("#divnativeprovince").addClass("hinen");
    $("#divnativecity").addClass("hinen");
    $("#divnativePAgent").addClass("hinen");
    //    $("#divstate").addClass("hinen");
    $("#divIsWebSite").addClass("hinen");
    //    $("#divstate ul li").click(function () {
    //        var node = $(this).children("a");
    //        $("#divstate").removeClass("active").addClass("hinen");
    //        $("#state1").val(node.text());
    //        $("#state1").attr("k", node.attr("value"));
    //        //                $("#state").val(node.attr("value"));

    //        if (node.attr("value") != -1) {
    //            $("#state1").addClass("check");

    //        } else {
    //            $("#state1").removeClass("check");
    //        }
    //    })

    $("#divIsWebSite ul li").click(function () {


        var node = $(this).children("a");


        $("#AccountTypes").val(node.text());
        $("#AccountTypes").attr("k", node.attr("value"));
        $("#divIsWebSite").removeClass("active").addClass("hinen");
        if (node.attr("value") != "all") {
            $("#AccountTypes").addClass("check");
            // $("#AccountType").val(node.attr("value"));
        } else {
            $("#AccountTypes").removeClass("check");
        }
    })

    $("#selectTd i,.num i").click(function () {
        var curNode = $(this).siblings("div");
        if (curNode.hasClass("active")) {

            $("#selectTd i").each(function () {

                var Node = $(this).siblings("div");
                Node.removeClass("active").addClass("hinen");
            })
            $("#divIsWebSite").removeClass("active").addClass("hinen");
            $("#divstate").removeClass("active").addClass("hinen");
        } else {

            $("#selectTd i").each(function () {

                var Node = $(this).siblings("div");
                Node.removeClass("active").addClass("hinen");

            })
            $("#divIsWebSite").removeClass("active").addClass("hinen");
            $("#divstate").removeClass("active").addClass("hinen");
            if (curNode.children("ul").length > 0) {
                curNode.removeClass("hinen").addClass("active");
            }
        }
    })

    $(".mCity li,.proCity li").click(function () {

        var node = $(this).children("a");
        var pid = node.attr("value");

        $("#txtnativeprovince").val(node.text());
        //                $("#ProvinceId").val(node.attr("value"));
        $("#txtnativeprovince").attr("k", node.attr("value"));
        $("#txtnativeprovince").addClass("check");

        var curNode = $("#divnativeprovince");
        curNode.removeClass("active").addClass("hinen");
        var url = "/User/GetCity/" + pid;
        var str = "";
        $.post(url, function (data) {
            str += "<ul>";
            for (var i = 0; i < data.length; i++) {
                str += "<li class=''><a href='javascript:void(0);' pid='" + pid + "' name='" + data[i].CityName + "' value='" + data[i].CityID + "'>" + data[i].CityName + "</a></li>";

            }
            if ($("#divnativecity ul").length > 0) {
                $("#divnativecity ul").remove();

            }
            $("#txtnativecity").val("请选择市").removeClass("colorBlue");
            $("#txtnativePAgentUser").val("请选择区县").removeClass("colorBlue");
            $("#txtnativecity").removeClass("check");
            $("#txtnativePAgentUser").removeClass("check");
            $("#txtnativecity").attr("k", "-1");
            $("#txtnativePAgentUser").attr("k", "-1");
            if (str.length > 0) {
                $("#divnativecity").append(str);
                $("#divnativecity").removeClass("hinen");
                $("#divnativecity").addClass("active");
            }
        })
    })
    if ($("#divnativecity ul li").length > 0) {
        $("#divnativecity ul li").live("click", function () {

            var node = $(this).children("a");
            var pid = node.attr("value");
            $("#txtnativecity").val(node.text());

            $("#txtnativecity").attr("k", node.attr("value"));


            var curNode = $("#divnativecity");
            curNode.removeClass("active").addClass("hinen");
            $("#txtnativecity").addClass("check");
            var str = "";
            var url = "/User/GetDistrict/" + pid;

            $.post(url, function (data) {
                if (data.length > 0) {
                    str += "<ul>";
                }


                for (var i = 0; i < data.length; i++) {
                    str += "<li class=''><a href='javascript:void(0);' pid='" + pid + "' name='" + data[i].DistrictName + "' value='" + data[i].DistrictID + "'>" + data[i].DistrictName + "</a></li>";

                }

                if ($("#divnativePAgent ul").length > 0) {
                    $("#divnativePAgent ul").remove();
                }
                $("#txtnativePAgentUser").val("请选择区县");
                $("#txtnativePAgentUser").attr("k", "-1");
                $("#txtnativePAgentUser").removeClass("check");

                if (str.length > 0) {

                    $("#divnativePAgent").append(str);
                    $("#divnativePAgent").removeClass("hinen").addClass("active");

                }
            })



        })

    }
    if ($("#divnativePAgent ul li").length > 0) {
        $("#divnativePAgent ul li").live("click", function () {
            var node = $(this).children("a");

            $("#txtnativePAgentUser").val(node.text());

            $("#txtnativePAgentUser").attr("k", node.attr("value"));


            $("#txtnativePAgentUser").addClass("check");

            var curNode = $("#divnativePAgent");
            curNode.removeClass("active").addClass("hinen");
        })
    }
    


    var startleft = $('.leftmenu').height();
    $(".menuson li").click(function () {
        $(".menuson li.active").removeClass("active")
        $(this).addClass("active");
    });

    $('.title').click(function () {
        var $ul = $(this).next('ul');
        $('dd').find('ul').slideUp();
        if ($ul.is(':visible')) {
            $(this).next('ul').slideUp("normal", leftPaddingHeight);

        } else {
            $(this).next('ul').slideDown("normal", leftPaddingHeight);
        }

    });

    function setLiActive(link) {
        var locations = window.location.href;

        var hrefs = $(link).eq(0).find("a").attr("name");

        var _hostname = window.location.hostname  //获得主机名或ip地址
        var _port = window.location.port   //获得端口号

        if (_port != null && _port != "")
            hrefs = "http://" + _hostname + ":" + _port + hrefs;
        else
            hrefs = "http://" + _hostname + hrefs;

        if (locations.toLowerCase().indexOf(hrefs.toLowerCase()) >= 0) {
            $(link).parent("ul").css("display", "block");
            $(".menuson li.active").removeClass("active")
            $(link).addClass("active");

        }

    }
    window.onload = $(function () {
        $(".menuson li").each(function () {

            setLiActive(this);
        });
        $(".nav li a.selected").removeClass("selected");
        leftPaddingHeight();
        var s = (window.screen.width) / 100 * 8;
        $("#selectTd .InputSerach").css("width", s);
        $("#selectTd .uew-select").css("width", s);
        $("#selectTd .uew-select-value").css("width", s-27);
        $("#selectTd .select1").css("width", s); 
    })
    function leftPaddingHeight() {
        var PaddingHeight = $(document).height() - $('#top').height() - $('.lefttop').height() - $('.leftmenu').height();

        $('.leftmenu').css("padding-bottom", PaddingHeight + "px");
    }


    $(".excel").click(function () {
        var pageIndex, pageSize, UserName, url, paramt;
        pageIndex = parseInt($(".current a").attr("value"));
        pageSize = $(".selectSize").val();
        url = $(this).attr("urlhref");
        if ($('#UserName').val().length != 0) {

            paramt = "&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&UserName=" + $('#UserName').val();
        } else {
            paramt = "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        }
        window.location = url + paramt;
    })

});

//select选项
//if ($(".select1").length > 0) {
    $(".select1").live("change", function () {
        $(this).siblings("div").find("em").eq(0).text($(this).find("option:selected").text());
    });
//}

//清空验证信息
function clearI(obj) {
    $(obj).next('i').text("");
    return;
}
function fideOutDiv() {
    $(".tipDiv").fadeOut(200);
    $(".ShowHide").fadeOut(100); 
}