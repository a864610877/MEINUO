$(function () {


    /*输入框效果 start*/
    $(".t_val").focus(function () {
        $(this).removeClass("redbd redwords bluewords");
    })/*.blur(function(){
		var txt_value = trim($(this).val());
		if(txt_value==""){
			$(this).addClass("redbd redwords").removeClass('bluewords');
		}

	})*/.keydown(function () {
	    $(this).addClass("bluewords").removeClass('redbd redwords');
	});
    /*输入框效果 end*/

    $(".t_val").each(function (i, inpt) {
        if (!$(inpt).val()) {
            $(inpt).val($(inpt).attr("default-value"));
            $(this).removeClass("colorBlue");
        }
        if ($(inpt).val() && $(inpt).val() != $(inpt).attr("default-value")) {
            $(this).addClass("colorBlue");
        }
        $(inpt).focus(function () {
            if ($(this).val() == $(this).attr("default-value")) {
                $(this).val('');
                $(this).addClass("colorBlue");
            }
        }).blur(function () {

            if (!$(this).val()) {
                $(this).val($(this).attr("default-value"));
                $(this).removeClass("colorBlue");
            }
        });
    });


});

//输入正确信息图标
var rightImg = "<img src='/images/Register/right.png'>";

/*验证姓名 start*/
function checkMemberFormName(obj) {
    //var JquObj = $(obj);
    var JquObj = $('#Name');
    var xname = trim(JquObj.val());
    var defaultValue = trim(JquObj.attr('default-value'));
    var flag = true;

    if (!xname) {
        JquObj.val('请输入您的真实姓名');
        JquObj.attr('default-value', '请输入您的真实姓名');
        flag = false;

    } else if (xname == defaultValue) {
        JquObj.val(defaultValue);
        JquObj.attr('default-value', defaultValue);
        flag = false;
    } else if (xname.length > 50) {
        //$("#xname").attr('placeholder', '姓名长度不能超过50个字');
        JquObj.val('姓名长度不能超过50个字');
        JquObj.attr('default-value', '姓名长度不能超过50个字');
        flag = false;
    } else {
        if (isInteger(xname)) {
            // $("#xname").attr('placeholder', '姓名不能是数字');
            JquObj.attr('default-value', '姓名不能是数字');
            JquObj.val('姓名不能是数字');
            flag = false;
        } else if (checkStr(xname, 1)) {
            flag = false;
        }
    }

    $("#Name").next().remove();
    if (flag) {
        $("#Name").removeClass("redbd redwords").addClass('bluewords');
        $("#Name").after(rightImg);
        //	JquObj.attr('default-value','请输入您的真实姓名');
    } else {
        $("#Name").addClass("redbd redwords").removeClass('bluewords');
        //$("#xname").val('');
    }
    return flag;
}
/*验证姓名 end*/

/*验证手机号 start*/
function checkMemberMobile() {

    //var JquObj = $(obj);
    var JquObj = $('#Mobile');
    var inputVal = trim(JquObj.val());
    var defaultValue = trim(JquObj.attr('default-value'));
    var flag = true;

    if (!inputVal) {
        JquObj.val('请输入您的手机号');
        JquObj.attr('default-value', '请输入您的手机号');
        flag = false;
    } else if (inputVal == defaultValue) {
        JquObj.val(defaultValue);
        JquObj.attr('default-value', defaultValue);
        flag = false;
    } else if (inputVal.length > 11) {
        //$("#Mobile").attr('placeholder', '手机号应为11位');
        JquObj.val('手机号应为11位');
        JquObj.attr('default-value', '手机号应为11位');
        flag = false;
    } else {
        if (!isMobile(inputVal)) {
            //$("#Mobile").attr('placeholder', '手机格式不正确');
            JquObj.val('手机格式不正确');
            JquObj.attr('default-value', '手机格式不正确');
            flag = false;
        } else {
            flag = checkMember(1);
        }
    }

    $("#Mobile").next("img").remove();
    if (flag) {
        $("#Mobile").removeClass("redbd redwords").addClass('bluewords');
        $("#Mobile").after(rightImg);
    } else {
        $("#Mobile").addClass("redbd redwords").removeClass('bluewords');
        //$("#Mobile").val('');
    }

    return flag;
}

/*验证开户行地址 start*/
function checkMemberCardAdress() {

    //var JquObj = $(obj);
    var JquObj = $('#BankAdress');
    var inputVal = trim(JquObj.val());
    var defaultValue = trim(JquObj.attr('default-value'));
    var flag = true;

    if (!inputVal) {
        JquObj.val('请输入您的开户银行地址');
        JquObj.attr('default-value', '请输入您的开户银行地址');
        flag = false;
    } else if (inputVal == defaultValue) {
        JquObj.val(defaultValue);
        JquObj.attr('default-value', defaultValue);
        flag = false;
    } else if (isInteger(inputVal)) {
        JquObj.attr('default-value', '请输入您的开户银行地址');
        JquObj.val('请输入您的开户银行地址');
        flag = false;
    }

    $("#BankAdress").next("img").remove();
    if (flag) {
        $("#BankAdress").removeClass("redbd redwords").addClass('bluewords');
        $("#BankAdress").after(rightImg);
    } else {
        $("#BankAdress").addClass("redbd redwords").removeClass('bluewords');
    }

    return flag;
}

/*验证身份证号 start*/
function checkIdentity() {

    //var JquObj = $(obj);
    var JquObj = $('#Identity');
    var inputVal = trim(JquObj.val());
    var defaultValue = trim(JquObj.attr('default-value'));
    var flag = true;

    if (!inputVal) {
        JquObj.val('请输入您的身份证号');
        JquObj.attr('default-value', '请输入您的身份证号');
        flag = false;
    } else if (inputVal == defaultValue) {
        JquObj.val(defaultValue);
        JquObj.attr('default-value', defaultValue);
        flag = false;
    } else if (inputVal.length > 18) {
        //$("#Mobile").attr('placeholder', '手机号应为11位');
        JquObj.val('身份证号应为15位或18位');
        JquObj.attr('default-value', '身份证号应为15位或18位');
        flag = false;
    } else {
        if (!isIdentity(inputVal)) {
            //$("#Mobile").attr('placeholder', '手机格式不正确');
            JquObj.val('身份证号格式不正确');
            JquObj.attr('default-value', '身份证号格式不正确');
            flag = false;
        } else {
            flag = checkMember(2);
        }
    }

    $("#Identity").next("img").remove();
    if (flag) {
        $("#Identity").removeClass("redbd redwords").addClass('bluewords');
        $("#Identity").after(rightImg);
    } else {
        $("#Identity").addClass("redbd redwords").removeClass('bluewords');
        //$("#Mobile").val('');
    }
    phoneflag = flag;
    return flag;
}


//检测短信验证码
function CheckPhoneCode() {

    var JquObj = $('#txtpcode');
    var inputVal = trim(JquObj.val());
    var defaultValue = trim(JquObj.attr('default-value'));
    var flag = true;

    if (!inputVal) {
        JquObj.val('请输入您的手机验证码');
        JquObj.attr('default-value', '请输入您的手机验证码');
        flag = false;
    } else if (inputVal == defaultValue) {
        JquObj.val(defaultValue);
        JquObj.attr('default-value', defaultValue);
        flag = false;
    } else if (inputVal.length > 6) {
        //$("#Mobile").attr('placeholder', '手机号应为11位');
        JquObj.val('手机验证码应为6位');
        JquObj.attr('default-value', '手机验证码应为6位');
        flag = false;

    } else {

        if (!isCode(inputVal)) {
            //$("#Mobile").attr('placeholder', '手机格式不正确');
            JquObj.val('验证码格式不正确');
            JquObj.attr('default-value', '验证码格式不正确');
            flag = false;
        } else {

            $.ajax({
                type: "post",
                url: "/User/CheckCode",
                async: false,
                dataType: "json",
                data: { "Code": inputVal },

                success: function (data) {

                    if (data.isOk == "no") {
                        var mobileFn = checkMemberMobile();

                        if (mobileFn == false) {

                            return;
                        }
                        var JquObj = $('#txtpcode');
                        JquObj.val(data.msg);
                        JquObj.attr('default-value', data.msg);
                        $("#txtpcode").addClass("redbd redwords").removeClass('bluewords');

                    } else {

                    }
                }
            });
        }

    }



    if (flag) {
        $("#txtpcode").removeClass("redbd redwords").addClass('bluewords');

    } else {
        $("#txtpcode").addClass("redbd redwords").removeClass('bluewords');

    }

    return flag;

}


/*验证银行卡号 start*/
function checkMemberOriginCode() {


    var JquObj = $('#CardNumber');
    var inputVal = trim(JquObj.val());
    var defaultValue = trim(JquObj.attr('default-value'));
    var flag = true;

    if (!inputVal) {
        JquObj.val('请输入您的银行卡号');
        JquObj.attr('default-value', '请输入您的银行卡号');
        flag = false;
    } else if (inputVal == defaultValue) {
        JquObj.val(defaultValue);
        JquObj.attr('default-value', defaultValue);
        flag = false;
    } else if (inputVal.length > 19) {
        //$("#Mobile").attr('placeholder', '手机号应为11位');
        JquObj.val('银行卡号应为12位或15位或19位');
        JquObj.attr('default-value', '银行卡号应为12位或15位或19位');
        flag = false;
    } else {
        if (!isCardNumber(inputVal)) {
            //$("#Mobile").attr('placeholder', '手机格式不正确');
            JquObj.val('银行卡号格式不正确');
            JquObj.attr('default-value', '银行卡号格式不正确');
            flag = false;
        } else {
            flag = (checkMember(4));
        }
    }

    $("#CardNumber").next("img").remove();
    if (flag) {
        $("#CardNumber").removeClass("redbd redwords").addClass('bluewords');
        $("#CardNumber").after(rightImg);
    } else {
        $("#CardNumber").addClass("redbd redwords").removeClass('bluewords');
        //$("#Mobile").val('');
    }
    phoneflag = flag;
    return flag;
}



var _time = 300; //设置验证码倒计时时间
//验证码倒计时
function PhoneCodeTime() {
    //将手机设置为不可点击
    $("#aphonecode").attr("disabled", "true");
    _time = parseInt(_time, 10);
    _time--;
    if (_time < 1) {
        $("#aphonecode").removeAttr("disabled").val("获取手机验证码");
    }
    else {
        $("#aphonecode").val("请在" + _time + "秒后重新获取");
    }
}









/*验证手机号 end*/

/*验证密码 start*/
function checkMemberFormPassword() {

    var JquObj = $('#Password');
    var inputVal = trim(JquObj.val());
    var defaultValue = trim(JquObj.attr('default-value'));

    var JquObj1 = $('#Password1');
    var inputVal1 = trim(JquObj1.val());
    var defaultValue1 = trim(JquObj1.attr('default-value'));


    var flag = true;

    if (!inputVal) {
        JquObj.val('密码为6-18位数字或字母');
        JquObj.attr('default-value', '密码为6-18位数字或字母');
        JquObj1.val('密码为6-18位数字或字母');
        JquObj1.attr('default-value', '密码为6-18位数字或字母');
        flag = false;
    } else if (inputVal == defaultValue) {
        JquObj.val(defaultValue1);
        JquObj.attr('default-value', defaultValue1);
        JquObj1.val(defaultValue1);
        JquObj1.attr('default-value', defaultValue1);
        flag = false;
    } else if (inputVal.length > 18 || (inputVal.length < 6 && inputVal.length >= 0)) {
        JquObj.attr('default-value', '密码为6-18位数字或字母');
        JquObj.val('密码为6-18位数字或字母');
        JquObj1.attr('default-value', '密码为6-18位数字或字母');
        JquObj1.val('密码为6-18位数字或字母');
        flag = false;
    } else if (/[^\x00-\xff]/g.test(trim(inputVal))) {
        // $("#Password").attr('placeholder', '密码不能含有汉字');
        JquObj.attr('default-value', '密码不能含有汉字');
        JquObj.val('密码不能含有汉字');
        JquObj1.attr('default-value', '密码不能含有汉字');
        JquObj1.val('密码不能含有汉字');
        flag = false;
    } else if (checkStr(inputVal, 2)) {
        flag = false;
    }

    $("#Password").next().remove();
    if (flag) {

        JquObj.removeClass("redbd redwords").addClass('bluewords');
        JquObj.after(rightImg);

    } else {

        JquObj1.addClass("redbd redwords").removeClass('bluewords');
        //JquObj1.val('');

        JquObj.hide();
        JquObj1.show();
        //JquObj1.focus();
    }
    return flag;
}
/*验证密码 end*/

/*验证确认密码 start*/
function checkPasswordT() {

    var Password = trim($("#Password").val());
    var PasswordT = trim($("#PasswordT").val());
    var PasswordT1 = trim($("#PasswordT1").val()); //显示区
    var defaultValueT = trim($("#PasswordT").attr('default-value'));
    var defaultValueT1 = trim($("#PasswordT1").attr('default-value'));
    var flag = true;

    if (!PasswordT) {
        $("#PasswordT").val('再次输入密码');
        $("#PasswordT").attr('default-value', '再次输入密码');
        $("#PasswordT1").val('再次输入密码');
        $("#PasswordT1").attr('default-value', '再次输入密码');
        flag = false;
    } else if (PasswordT == defaultValueT) {
        $("#PasswordT").val(defaultValueT);
        $("#PasswordT").attr('default-value', defaultValueT);
        $("#PasswordT1").val(defaultValueT1);
        $("#PasswordT1").attr('default-value', defaultValueT1);
        flag = false;
    } else if (Password != PasswordT) {
        //$("#PasswordT").attr('placeholder', '两次输入密码不一致');
        $("#PasswordT").val('两次输入密码不一致');
        $("#PasswordT").attr('default-value', '两次输入密码不一致');
        $("#PasswordT1").val('两次输入密码不一致');
        $("#PasswordT1").attr('default-value', '两次输入密码不一致');
        flag = false;
    }

    $("#PasswordT").next().remove();
    if (flag) {
        $("#PasswordT").removeClass("redbd redwords").addClass('bluewords');
        $("#PasswordT").after(rightImg);
    } else {
        $("#PasswordT1").addClass("redbd redwords").removeClass('bluewords');
        //$("#PasswordT").val('');

        $("#PasswordT").hide();
        $("#PasswordT1").show();
    }
    return flag;
}
/*验证确认密码 end*/

/*验证邮箱 start*/
function checkMemberEmail(obj) {


    var JquObj = $('#Email');
    var inputVal = trim(JquObj.val());
    var defaultValue = trim(JquObj.attr('default-value'));
    var flag = true;

    if (inputVal == '请输入您的邮箱地址' || !inputVal) {
        JquObj.val('请输入您的邮箱地址');
        JquObj.attr('default-value', '请输入您的邮箱地址');
        flag = true;
    } else if (inputVal != '请输入您的邮箱地址' && inputVal == defaultValue) {
        JquObj.val(defaultValue);
        JquObj.attr('default-value', defaultValue);
        flag = false;
    } else if (inputVal.length > 50) {
        //	$("#Email").attr('placeholder', '邮箱地址过长');
        JquObj.val('邮箱地址过长');
        JquObj.attr('default-value', '邮箱地址过长');
        flag = false;
    } else if (inputVal.length > 0 && inputVal.length <= 50) {
        if (!isEmail(inputVal)) {
            // $("#Email").attr('placeholder', '邮箱地址格式不正确');
            JquObj.val('邮箱地址格式不正确');
            JquObj.attr('default-value', '邮箱地址格式不正确');
            flag = false;
        } else {
            flag = checkMember(3);
        }
    }


    $("#Email").next().remove();
    if (flag) {

        if (inputVal == '' || inputVal == '请输入您的邮箱地址') {
            JquObj.removeClass("redbd redwords bluewords");
        } else {
            JquObj.removeClass("redbd redwords").addClass('bluewords');
            JquObj.after(rightImg);
        }
    } else {
        JquObj.addClass("redbd redwords").removeClass('bluewords');
    }

    if (inputVal.length == 0) {
        JquObj.next().remove();
    }
    return flag;
}

/*验证是否有非法字符 start*/
function checkStr(str, tf) {
    var flag = false;
    var checkString = "`~!@#$%^&*()+-=[]{}\\|;':\",./<>?";
    for (var j = 0; j < checkString.length; j++) {
        if (str.indexOf(checkString.substring(j, j + 1)) != -1) {
            var jqueryVar = null;
            if (tf == 1) {
                jqueryVar = $("#Name");

            }
            if (tf == 2) {
                jqueryVar = $("#Password1");
                $("#Password").val("不能有非法字符:\"" + checkString.substring(j, j + 1) + "\"");
                $("#Password").attr('default-value', "不能有非法字符:\"" + checkString.substring(j, j + 1) + "\"");
            }
            if (tf == 3) {
                jqueryVar = $("#tempCardNO");
            }
            // jqueryVar.attr('placeholder', "不能有非法字符:\"" + checkString.substring(j, j + 1) + "\"");

            jqueryVar.val("不能有非法字符:\"" + checkString.substring(j, j + 1) + "\"");
            jqueryVar.attr('default-value', "不能有非法字符:\"" + checkString.substring(j, j + 1) + "\"");
            jqueryVar.addClass("redbd redwords").removeClass('bluewords');
            flag = true;
            break;
        }
    }
    return flag;
}










/*检查 注册 会员  或者  修改会员信息时 检测 手机号 ，邮箱，身份证号,银行卡号  是否重复 start*/
function checkMember(tx) {

    var Mobile = trim($('#Mobile').val());
    var Email = trim(($('#Email').val()));
    var Identity = $('#Identity').val();
    var CardNumber = $('#CardNumber').val();
    var flag = true;
    var jqueryVar = null;

    if (Mobile.length == 0) {
        Mobile = null;
    }
    if (Email.length == 0) {
        Email = null;
    }

    if (trim(Identity).length == 0 || trim(Identity) == "请输入您的证件号码") {
        Identity = null;
    }
    if (trim(CardNumber).length == 0 || trim(CardNumber) == "请输入您的银行卡号") {
        CardNumber = null;
    }
    if (Mobile == '' && Email == '' && Identity == '' && CardNumber == '') {
        flag = false;
    } else {

        $.ajax({
            type: "post",
            url: "/User/CheckMemberInfo",
            async: false,
            dataType: "json",
            data: { Mobile: Mobile, Email: Email, xflag: tx, Identity: Identity, CardNumber: CardNumber },

            success: function (data) {
                if (data.msg) {
                    alert(data.msg);
                }
                var xflag = data.xflag;
                if (xflag == 1) {

                    if (data.Mobile == true) {
                        $("#Mobile").val("手机号码已经注册");
                        $("#Mobile").attr('default-value', '手机号码已经注册');
                        $("#Mobile").addClass("redbd redwords").removeClass('bluewords');
                        flag = false;
                    }
                    jqueryVar = $("#Mobile");
                }
                if (xflag == 3) {
                    if (data.Email == true) {
                        $("#Email").val("邮箱已经注册");
                        $("#Email").attr('default-value', '邮箱已经注册');
                        $("#Email").addClass("redbd redwords").removeClass('bluewords');
                        flag = false;
                    }
                    jqueryVar = $("#Email");
                }
                if (xflag == 2) {
                    if (data.xidentitycard == true) {
                        $("#Identity").val("证件号码已经注册");
                        $("#Identity").attr('default-value', '证件号码已经注册');
                        $("#Identity").addClass("redbd redwords").removeClass('bluewords');
                        flag = false;
                    }
                    jqueryVar = $("#Identity");
                }
                if (xflag == 4) {
                    if (data.Card == true) {
                        $("#CardNumber").val("银行卡已注册或者银行卡无效");
                        $("#CardNumber").attr('default-value', '银行卡已注册或者银行卡无效');
                        $("#CardNumber").addClass("redbd redwords").removeClass('bluewords');
                        flag = false;
                    }
                    jqueryVar = $("#CardNumber");
                }
            }
        });
    }
    return flag;


}
/*检查 注册 会员  或者  修改会员信息时 检测 手机号 ，邮箱，身份证号，银行卡号  是否重复 （） end*/


function CheckClick() {



    var nameFn = checkMemberFormName();
    if (nameFn == false) {
        return;
    }

    var mobileFn = checkMemberMobile();

    if (mobileFn == false) {

        return;
    }

    var type = $("#AccountType option:selected").val();
    if (type != "PAgentUser" && type != "Attract") {
        if (trim($("#txtnativecity").val()) == "全部") {
            $("#txtnativecity").val("请选择市");
        }
    }
    if (type != "CAgentUser" && type != "PAgentUser" && type != "Attract") {
        if (trim($("#txtnativePAgentUser").val()) == "全部") {
            $("#txtnativePAgentUser").val("请选择区县");
        }
    }
    if (trim($("#txtnativeprovince").val()) == "请选择省") {
        $("#txtnativeprovince").addClass("redbd redwords");
        return;
    }
    if (trim($("#txtnativecity").val()) == "请选择市") {
        $("#txtnativecity").addClass("redbd redwords");
        return;
    }
    if (trim($("#txtnativePAgentUser").val()) == "请选择区县") {
        $("#txtnativePAgentUser").addClass("redbd redwords");
        return;
    }

    var passwordFn = checkMemberFormPassword();
    if (passwordFn == false) {
        return;
    }

    var passwordTFn = checkPasswordT();
    if (passwordTFn == false) {
        return;
    }

    var emaileFn = checkMemberEmail();
    if (emaileFn == false) {
        return;
    }
    var IdentityFn = checkIdentity()
    if (IdentityFn == false) {
        return;
    }
    var CardAdress = checkMemberCardAdress()
    if (CardAdress == false) {
        return;
    }
    var CardNumberFn = checkMemberOriginCode();
    if (CardNumberFn == false) {

        return;
    }

    if (!isOkClick) {
        GetPhoneCode();


    }
}


//获取手机验证码
function GetPhoneCode() {

    $.post("/User/RegisterSendCode", { "phone": $('#Mobile').val() }, function (data) {

        if (data.isOk == "no") {
            var mobileFn = checkMemberMobile();

            if (mobileFn == false) {

                return;
            }


            alert(data.msg);

        } else {
            $("#mobileInfo").text($("#Mobile").val().substring(0, 3) + "****" + $("#Mobile").val().substring(7, 11));
            _time = 300;
            setInterval(PhoneCodeTime, 1000);
            $("#sz_registerBox1").css("display", "none");
            $("#sz_registerBox2").css("display", "");
            $("#chengeSrc").attr("src", "/images/Register/problue.png");
        }

    })

}


var stime = 10;
function SucessTime() {

    stime = parseInt(stime, 10);
    stime--;
    if (stime < 1) {
        window.location = "/User/LogOn";
    }
    else {
        $("#modifyEmailBtn").text("..." + stime + "秒后自动跳到登陆页");
        $("#modifyEmailBtn").append("<a href='/User/LogOn'>立即登录</a>");
    }
}
function showDiv() {
    document.getElementById('popDiv').style.display = 'block';
    document.getElementById('popIframe').style.display = 'block';
    document.getElementById('bg').style.display = 'block';
}

var isOkClick = false;

/*下一步，验证 start*/
function okClick() {
    isOkClick = true;
    CheckClick();

    var AccountType = $('#AccountType').val();
    var xname = $('#Name').val();
    var Mobile = $('#Mobile').val();
    var Email = $('#Email').val();
    var Password = $('#Password').val();
    var tempCardNO = $('#tempCardNO').val();
    // var BankName = $('#BankName').val();
    var BankAdress = $('#BankAdress').val();
    var Gender = $("#Gender").val();
    var ProvinceId = $("#txtnativeprovince").attr("k");
    var CityId = $("#txtnativecity").attr("k");
    var DistrictID = $("#txtnativePAgentUser").attr("k");
    var CardNumber = $("#CardNumber").val();
    var Identity = $("#Identity").val();
    if (Email == '请输入您的邮箱地址') {
        Email = '';
    }
    var RecommendName = "";

    if ($("#UserName").length > 0) {
        RecommendName = $("#UserName").val();
    }

    var codeFn = CheckPhoneCode();
    if (codeFn == false) {

        return;
    }

    //提交表单到服务器
    $.ajax({
        url: "/User/MemberRegister",
        data: {UserName:"",BankName:"",RecommendType:"",openId:"", AccountType: AccountType, Gender: Gender, Name: xname, Password: Password, Mobile: Mobile, Email: Email, BankAdress: BankAdress, ProvinceId: ProvinceId, CityId: CityId, DistrictID: DistrictID, Identity: Identity, CardNumber: CardNumber, RecommendName: RecommendName },
        type: "post",
        //cache: false, 
        dataType: "json",
        async: false,
        success: function (data) {
            var flag = data.flag;
            if (flag == 0) {

                showDiv();


                setInterval(SucessTime, 1000);

            } else if (flag == 1) {
                alert("类型不能空,注册失败");
                return;
            } else if (flag == 2) {
                alert("身份类型与所填地址不符");
                return;
            } else if (flag == 3) {
                $("#Mobile").val('手机格式不正确');
                $("#Mobile").attr('default-value', '手机格式不正确');
                $("#Mobile").addClass("redbd redwords").removeClass('bluewords');
                return;
            } else if (flag == 4) {
                $("#CardNumber").val('银行卡有误,或者无效');
                $("#CardNumber").attr('default-value', '银行卡有误,或者无效');
                $("#CardNumber").addClass("redbd redwords").removeClass('bluewords');
                return;
            }
            else if (flag == 5) {
                $("#Password").val('密码格式不正确');
                $("#Password").attr('default-value', '密码格式不正确');
                $("#Password").addClass("redbd redwords").removeClass('bluewords');
                return;
            }
            else if (flag == 6) {
                $("#Identity").val('身份证格式不正确');
                $("#Identity").attr('default-value', '身份证格式不正确');
                $("#Identity").addClass("redbd redwords").removeClass('bluewords');
                return;
            }
            else if (flag == 7) {
                alert('银行卡注册失败,或者系统错误');
            }
            else {
                alert('请重新填写会员信息');
            }
            return;
        }
    });

}


//身份证校验

function checkMemberIdentitycard() {
    var flag = true;
    var xidentitycard = $("#xidentitycard").val();

    if ($("#xcardtype").val() == "10" || $("#xcardtype").val() == "身份证") {
        var now = new Date();
        var date = now.getFullYear() + "-" + (now.getMonth() + 1) + "-" + now.getDate();
        if (trim(xidentitycard).length > 0 && trim(xidentitycard) != "请输入您的证件号码" && CheckIDCard(trim(xidentitycard)) == 1) {
            $("#identitycardInfo").css("display", "block").text("您的身份证号码不正确！");
            flag = false;
        }
        var converted = new Date(parseInt(date.split("-")[0]) - 18, parseInt(date.split("-")[1]) - 1, parseInt(date.split("-")[2]));
        var birthday = new Date(parseInt(xidentitycard.substring(6, 10)), parseInt(xidentitycard.substring(10, 12)) - 1, parseInt(xidentitycard.substring(12, 14)));

        if (xidentitycard.length == 18) {
            if (birthday.getTime() - converted.getTime() > 0) {
                $("#identitycardInfo").css("display", "block").text("（含）以上！");
                flag = false;
            }
        } else if (xidentitycard.length == 15) {
            xidentitycard = C15ToC18(xidentitycard);
            birthday = new Date(parseInt(xidentitycard.substring(6, 10)), parseInt(xidentitycard.substring(10, 12)) - 1, parseInt(xidentitycard.substring(12, 14)));

            if (birthday.getTime() - converted.getTime() > 0) {
                $("#identitycardInfo").css("display", "block").text("（含）以上！");
                flag = false;
            }
        }
    } else {
        if (xidentitycard.length > 20) {
            $("#identitycardInfo").css("display", "block").text("证件号码输入过长！");
            flag = false;
        }
    }
    //证件号为空，特殊字符校验
    if (flag == true) {
        for (var j = 0; j < xidentitycard.length; j++) {
            if (xidentitycard.substring(j, j + 1) == " ") {
                $("#identitycardInfo").css("display", "block").text("证件号不能包含空格！");
                xidentitycard = "";
                flag = false;
                break;
            } else {
                $("#identitycardInfo").css("display", "none");
            }
        }
        if (flag == true) {
            if (checkStr(xidentitycard, 5)) {
                flag = false;
            }
        }
    }
    if (flag && trim(xidentitycard).length > 0 && trim(xidentitycard) != "请输入您的证件号码") {
        flag = checkMember(2);
    }

    return flag;
}
