
$(function () {
    //分页
    $(".selectSize").change(function () {
        var Size = $(this).children('option:selected').val();
        $("#PageSize").val(Size);
        $("#PageIndex").val(1);
        $("form").submit();
    });
    $(".paginItem a").click(function () {
        var pageIndex = $(this).attr("value");
        if (pageIndex > 0) {
            $("#PageIndex").val(pageIndex);
            $("form").submit();
        }

    });

    $(".pagepre").click(function () {
        var pageIndex = $("#PageIndex").val();
        if (pageIndex > 1) {
            pageIndex = pageIndex - 1;
        }
        else {
            return;
        }
        $("#PageIndex").val(pageIndex);
        $("form").submit();
    });
    $(".pagenxt").click(function () {
        var pageIndex = $("#PageIndex").val();

        pageIndex = parseInt(pageIndex) + 1;
        $("#PageIndex").val(pageIndex);
        $("form").submit();
    });

    //提示框
    $(".click").click(function () {
        $(".ShowHide").fadeIn(100);
        $(".tip").fadeIn(200);
    });

    $(".tiptop a").click(function () {
        $(".ShowHide").fadeOut(100);
        $(".tip").fadeOut(200);
    });

    //    $(".sure").click(function () {
    //        $(".ShowHide").fadeOut(100);
    //        $(".tip").fadeOut(100);
    //    });

    $(".cancel").click(function () {
        $(".ShowHide").fadeOut(100);
        $(".tip").fadeOut(100);
    });
    $(".tiptop2 a").click(function () {
        $(".ShowHide").fadeOut(100);
        $(".tip2").fadeOut(200);
    })



    $(".click").click(function () {
        $(".ShowHide").fadeIn(100);
        $(".tip").fadeIn(200);
    });

    $(".tiptopaa a").click(function () {
        $(".ShowHide").fadeOut(100);
        $(".tipaa").fadeOut(200);
    });

    //    $(".sure").click(function () {
    //        $(".ShowHide").fadeOut(100);
    //        $(".tip").fadeOut(100);
    //    });

    $(".cancel").click(function () {
        $(".ShowHide").fadeOut(100);
        $(".tipaa").fadeOut(100);
    });
    $(".tipaa a").click(function () {
        $(".ShowHide").fadeOut(100);
        $(".tipaa").fadeOut(200);
    })


    //表格全选
    $("#ckAll").click(function () {
        var flag = $(this).attr("checked");
        if (!flag)
            $("[name = list]:checkbox").attr("checked", false);
        else
            $("[name = list]:checkbox").attr("checked", true);
    });
    $("[name=Deletes]").click(function () {
        var path = $(this).attr("url");
        $('#form1').attr("action", path).submit();
    });

    //城市下拉框触发
    $("#select2").change(function () {
        $("#select3").text("");
        $("#select4").text("");
        $("#selectCitys").text("全部");
        $("#selectDistrict").text("全部");
        $("#select4").append("<option value='0'>全部</option>");
        var value = $(this).val();
        if (value == "0") {
            $("#select4").append("<option value='0'>全部</option>");
            $("#select3").append("<option value='0'>全部</option>");

        }
        else {
            $.post('/User/GetCity', { id: value }, function (data, status) {
                if (status == "success") {
                    if (data != null) {
                        // var html1 = "<select  style='width:150;z-index: 1;position: relative;padding-right: 20px;font-size: 12px;text-indent: 5px;background: url('../images/inputbg.gif') repeat-x scroll 0% 0% transparent;'  id='select3'> <option value='0'>全部</option>";
                        var html1 = "<option value='0'>全部</option>";
                        for (var i = 0; i < data.length; i++) {
                            html1 += "<option value=" + data[i].CityID + ">" + data[i].CityName + "</option>"
                        }
                        // html1 += "</select>";
                        $("#select3").append(html1);

                    }
                }
            });
        }
    });

    $("#select3").change(function () {
        $("#select4").text("");
        $("#selectDistrict").text("全部");
        var value = $(this).val();
        if (value == "0") {
            $("#select4").append("<option value='0'>全部</option>");
        }
        else {
            $.post('/User/GetDistrict', { id: value }, function (data, status) {
                if (status == "success") {
                    if (data != null) {
                        // var html = "<select class='select2' id='select4'><option value='0'>全部</option>";
                        var html = "<option value='0'>全部</option>";
                        for (var i = 0; i < data.length; i++) {
                            html += "<option value=" + data[i].DistrictID + ">" + data[i].DistrictName + "</option>"
                        }
                        // html += "</select>";
                        $("#select4").append(html);
                    }
                }
            });
        }
    });
    //修改密码
    $("#AdminUpdatePwd").click(function () {
        var OldPassword = $("#OldPassword").val();
        var NewPassword = $("#NewPassword").val();
        var NewPassword1 = $("#NewPassword1").val();
        if (OldPassword == null || OldPassword == "") {
            $("#OldText").text("请输入旧密码");
            return;
        }
        if (NewPassword == null || NewPassword == "") {
            $("#NewText").text("请输入新密码");
            return;
        }
        //        var reg = new RegExp("^[a-zA-Z]\w{5,17}$");
        //        if (!reg.test(NewPassword)) {
        //            $("#NewText").text("以字母开头，长度在6-18之间，只能包含字符、数字和下划线")
        //            return;
        //                }
        if (NewPassword != NewPassword1) {
            $("#NewText1").text("两次输入密码不一致");
            return;
        }
        $.post('/Information/AdminUpdatePwd', { OldPassword: OldPassword, NewPassword: NewPassword }, function (data, status) {
            if (status == "success") {
                if (data != null) {
                    if (data.Code == 0) {
                        $("#OldPassword").val("");
                        $("#NewPassword").val("");
                        $("#NewPassword1").val("");
                        $(".tipinfo2 img").attr("src", "../../images/Succeed01.png");
                        $(".tipright2 P").text(data.CodeText);
                        $(".ShowHide").fadeIn(100);
                        $(".tip2").fadeIn(200);
                    }
                    else {
                        $(".tipinfo2 img").attr("src", "../../images/error02.png");
                        $(".tipright2 P").text(data.CodeText);
                        $(".ShowHide").fadeIn(100);
                        $(".tip2").fadeIn(200);
                    }
                }
            }
        });
    });
    //修改密码
    $("#UpdatePwd").click(function () {
        var Code = $("#Code").val();
        var NewPassword = $("#NewPassword").val();
        var NewPassword1 = $("#NewPassword1").val();
        if (NewPassword == null || NewPassword == "") {
            $("#text1").text("请输入新密码");
            return;
        }
        //        var reg = new RegExp("^[a-zA-Z]\w{5,17}$");
        //        if (!reg.test(NewPassword)) {
        //            $("#NewText").text("以字母开头，长度在6-18之间，只能包含字符、数字和下划线")
        //            return;
        //                }
        if (NewPassword != NewPassword1) {
            $("#text2").text("两次输入密码不一致");
            return;
        }
        if (Code == null || Code == "") {
            $("#text3").text("请输入验证码");
            return;
        }

        $.post('/Information/UpdatePwd', { Code: Code, NewPassword: NewPassword }, function (data, status) {
            if (status == "success") {
                if (data != null) {
                    if (data.Code == 0) {
                        $("#Code").val("");
                        $("#NewPassword").val("");
                        $("#NewPassword1").val("");
                        $(".tipright2 P").text(data.CodeText);
                        $(".tipinfo2 img").attr("src", "../../images/Succeed01.png");
                        $(".ShowHide").fadeIn(100);
                        $(".tip2").fadeIn(200);
                    }
                    else {
                        $(".tipinfo2 img").attr("src", "../../images/error02.png");
                        $(".tipright2 P").text(data.CodeText);
                        $(".ShowHide").fadeIn(100);
                        $(".tip2").fadeIn(200);
                    }
                }
            }
        });

    });


    //更换手机号
    $("#UpdateMoblie").click(function () {
        var Code = $("#MobileCode").val();
        var NewMobile = $("#NewMobile").val();
        var NewMobile1 = $("#NewMobile1").val();
        if (isNaN(NewMobile) || NewMobile == null || NewMobile == "") {
            $("#NewMobileText").text("请输入正确手机号");
            return;
        }
        if (NewMobile != NewMobile1) {
            $("#NewMobileText1").text("两次输入手机号不一致");
            return;
        }
        if (Code == null || Code == "") {
            $("#MobileCodeText").text("请输入验证码");
            return;
        }

        $.post('/Information/UpdateMoblie', { Code: Code, NewMobile: NewMobile }, function (data, status) {
            if (status == "success") {
                if (data != null) {
                    if (data.Code == 0) {
                        $("#MobileCode").val("");
                        $("#NewMobile").val("");
                        $("#NewMobile1").val("");
                        $(".tipinfo2 img").attr("src", "../../images/Succeed01.png");
                        $(".tipright2 P").text(data.CodeText);
                        $(".ShowHide").fadeIn(100);
                        $(".tip2").fadeIn(200);
                    }
                    else {
                        $(".tipinfo2 img").attr("src", "../../images/error02.png");
                        $(".tipright2 P").text(data.CodeText);
                        $(".ShowHide").fadeIn(100);
                        $(".tip2").fadeIn(200);
                    }
                }
            }
        });

    });

    //验证银行卡号
    function isCardNumber(val) {
        var myreg = /^(\d{12})|(\d{15})|(\d{19})$/;
        return (myreg.test(val));
    }
    //添加银行卡
    $("#InsertBank").click(function () {
        var UserName = $("#UserName").val();
        var CardNumber = $("#CardNumber").val();
        var BankAddress = $("#BankAdress").val();

        if (CardNumber == null || CardNumber == "") {
            $("#CardNumberText").text("请输入卡号");
            return;
        }
        if (!isCardNumber(CardNumber)) {
            $("#CardNumberText").text("卡号填写有误,验证失败");
            return;
        }
        if (UserName == null || UserName == "") {
            $("#UserNameText").text("请输入开户人");
            return;
        }
        if (BankAddress == null || BankAddress == "") {
            $("#BankAdressText").text("请输入银行地址");
            return;
        }
        $.post('/Information/InsertBank', { UserName: UserName, CardNumber: CardNumber, BankAddress: BankAddress }, function (data, status) {
            if (status == "success") {
                if (data != null) {
                    if (data.Code > 0) {
                        $(".tipinfo2 img").attr("src", "../../images/Succeed01.png");
                        $(".tipright2 P").text("添加成功");
                        $(".ShowHide").fadeIn(100);
                        $(".tip2").fadeIn(200);
                        $("#BankName").val("");
                        $("#UserName").val("");
                        $("#CardNumber").val("");
                        $("#BankAdress").val("");
                        var html = "<tr><td>" + data.CodeText + "</td><td>" + CardNumber + "</td><td>" + UserName + "</td><td>" + BankAddress + "</td><td><a name='BankDelete' bankid=" + data.Code + "  onclick='deleRow(this)'  href='#' class='tablelink'>删除</a></td></tr>";
                        $("#banks").append(html);
                    }
                    else {
                        $(".tipinfo2 img").attr("src", "../../images/error02.png");
                        $(".ShowHide").fadeIn(100);
                        $(".tipright2 P").text(data.CodeText);
                        $(".tip2").fadeIn(200);
                    }
                }
            }
        });


    });

    //修改支付密码
    $("#PayAdminUpdatePwd").click(function () {
        var PayNewPassword = $("#PayNewPassword").val();
        var PayNewPassword1 = $("#PayNewPassword1").val();
        var PayCode = $("#PayCode").val();
        if (PayNewPassword == "" || PayNewPassword == null) {
            $("#PayNewText").text("请输入新密码");
            return;
        }
        if (PayNewPassword1 == "" || PayNewPassword1 == null) {
            $("#PayNewText1").text("请输入确认密码");
            return;
        }
        if (PayNewPassword != PayNewPassword1) {
            $("#PayNewText1").text("两次输入密码不一致");
            return;
        }
        if (PayCode == "" || PayCode == null) {
            $("#Paytext3").text("请输入验证码");
            return;
        }
        $.post('/Information/UpdatePayPwd', { PayNewPassword: PayNewPassword, PayNewPassword1: PayNewPassword1, PayCode: PayCode }, function (data, status) {
            if (status == "success") {
                $("#PayNewPassword").val("");
                $("#PayNewPassword1").val("");
                $("#PayCode").val("");
                $(".tipinfo2 img").attr("src", "../../images/Succeed01.png");
                $(".tipright2 P").text(data.CodeText);
                $(".ShowHide").fadeIn(100);
                $(".tip2").fadeIn(200);
            }
            else {
                $(".tipinfo2 img").attr("src", "../../images/error02.png");
                $(".tipright2 P").text(data.CodeText);
                $(".ShowHide").fadeIn(100);
                $(".tip2").fadeIn(200);
            }
        });
    });

    $('[name="Category"]').change(function () {
        var ShopCategoryId = $(this).val();
        $.post('/ShopCategory/GetCategorys', { ShopCategoryId: ShopCategoryId }, function (data, status) {
            if (status == "success") {
                if (data != null) {
                    if (data.length > 0) {
                        var html = "<select name='Category'><option value='0'> </option>";
                        for (var i = 0; i < data.length; i++) {
                            html += "<option value=" + data[i].ShopCategoryId + ">" + data[i].CategoryName + "</option>"
                        }
                        html += "</select>";
                        $("#Categorys").append(html);
                    }
                }
            }
        });
    });


});
//表格操作
//删除行
function deleRow(filed) {
    if (confirm('您确定要删除吗，删除后不可恢复?')) {
        var tableObj = document.getElementById("banks");
        var Id = filed.getAttribute("bankid");
        $.post('/Information/DeleteBank', { Id: Id }, function (data, status) {
            if (status == "success") {
                if (data != null) {
                    if (data.Code == 0) {
                        alert(data.CodeText);
                        var num = getElementOrder(filed);
                        tableObj.deleteRow(num);

                    }
                    else {
                        alert(data.CodeText);
                    }
                }
            }
        });
    }
}
//查询要删除行的索引
function getElementOrder(field) {

    var i = 0;

    var order = 0;

    var elements = document.getElementsByName(field.name);

    for (i = 0; i < elements.length; i++) {

        order++;

        if (elements[i] == field) {

            break;

        }

    }

    return order;

}

//验证码
function f_timeout(type) {
    if (type == "PayUpdatePwdCode") {
        var typeStr = type;
        $('#Paydivphone').html('  <lable id="timePay"> 300 </lable>秒后重新获取 ');
        timer = self.setInterval(function addsec(type) {
            var t = $('#timePay').html();
            if (t > 0) {
                $('#timePay').html(t - 1);
            } else {
                window.clearInterval(timer);
                $('#Paydivphone').html("<span><a onclick=sends('" + typeStr + "')  href='#'>点击获取验证码</a></span>");
                $("#Mobile").html("");
            }
        }, 1000); 
    }

    else if (type == "UpdateMoblieCode") {
        var typeStr = type;
        $('#changedivphone').html('  <lable id="timeChange"> 300  </lable>秒后重新获取 ');
        timer = self.setInterval(function addsec(type) {
            var t = $('#timeChange').html();
            if (t > 0) {
                $('#timeChange').html(t - 1);
            } else {
                window.clearInterval(timer);
                $('#changedivphone').html("<span><a onclick=sends('" + typeStr + "')  href='#'>点击获取验证码</a></span>");
                $("#Mobile").html("");
            }
        }, 1000); 
    }
    else {
        $('#divphone').html('  <lable id="timeb2"> 300 </lable>秒后重新获取 ');
        timer = self.setInterval(function () {
            addsec(type);
        }, 1000);
    }
}
function addsec(type) {
    var t = $('#timeb2').html();
    if (t > 0) {
        $('#timeb2').html(t - 1);
    } else {
        window.clearInterval(timer);
        $('#divphone').html("<span><a onclick=sends('" + type + "')  href='#'>点击获取验证码</a></span>");
        $("#Mobile").html("");
    }
}
function sends(type) {
    if (type == "UpdateWithdraw_Code") {
        var PointItem = $("#Point");
        if (isNaN(PointItem.val()) || PointItem.val() <= 0) {
            $(PointItem).next('i').text("积分填写有误");
            return;
        }
    } else if (type == "Loan_Code") {
        var AmountItem = $("#Amount");
        if (isNaN(AmountItem.val()) || AmountItem.val() <= 0) {
            $(AmountItem).next('i').text("贷款金额填写有误");
            return;
        }
    } else if (type == "UpdatePwdCode") {
        var NewPassword = $("#NewPassword").val();
        var NewPassword1 = $("#NewPassword1").val();
        if (NewPassword == null || NewPassword == "") {
            $("#text1").text("请输入新密码");
            return;
        } 
        if (NewPassword != NewPassword1) {
            $("#text2").text("两次输入密码不一致");
            return;
        }
    } else if (type == "UpdateMoblieCode") {
        var NewMobile = $("#NewMobile").val();
        var NewMobile1 = $("#NewMobile1").val();
        if (isNaN(NewMobile)|| NewMobile == null || NewMobile == "") {
            $("#NewMobileText").text("请输入正确手机号");
            return;
        }
        if (NewMobile != NewMobile1) {
            $("#NewMobileText1").text("两次输入手机号不一致");
            return;
        }
    } 

    //发短信
    $.post('/User/SendCode', {type:type}, function (data, status) {
        if (status == "success") {
            if (data.Code == 0) {
                $(".tipright2 P").text(data.CodeText);
                $(".ShowHide").fadeIn(100);
                $(".tip2").fadeIn(200);
            }
            else if (data.Code == -1) {
                f_timeout(type);
            }
            else {
                f_timeout(type); //倒计时
            }
        }

    });

}


//原系统
$(".ckall").click(function () {
    var table = $(this).parents("table")[0];

    $(".ckitem", table).attr("checked", $(this).attr("checked"));
});


