// 验证手机号
function isPhoneNo(phone) {
    var pattern = /^1[34578]\d{9}$/;
    return pattern.test(phone);
}
//下一步按钮点击事件
function next_btn() {
    var phone = $("#phone").val();
    //数据验证
    if (phone == "") {
        popup({ type: 'error', msg: "请输入手机号码", delay: 200000000, bg: true, clickDomCancel: true });
        return;
    }
    if (!isPhoneNo(phone)) {
        popup({ type: 'error', msg: "手机号码不正确", delay: 2000, bg: true, clickDomCancel: true });
        return;
    }
    var code = $("#code").val();
    if (code == "") {
        popup({ type: 'error', msg: "请输入验证码", delay: 2000, bg: true, clickDomCancel: true });
        return;
    }
    if (!code_state) {
        popup({ type: 'error', msg: "请获取验证码", delay: 2000, bg: true, clickDomCancel: true });
        return;
    }
    if (phone_result) {   //确认手机号码是否已注册
        $("#Register1").hide();
        $("#Register2").show();  //切换到设置密码界面
    } else {
        popup({ type: 'error', msg: "手机号码已注册,请换个手机号码注册!", delay: 2000, bg: true, clickDomCancel: true });
    }
    return;
}
var phone_result = false;
var code_state = false;
//获取验证码
function getCode() {
    var phone = $("#phone").val();
    if (phone == "") {
        popup({ type: 'error', msg: "请输入手机号码", delay: 2000, bg: true, clickDomCancel: true });
        return;
    }
    code_state = true;
    $.ajax({
        type: "POST",
        url: "CheckPhone",
        data: { phone: phone },
        success: function (result) {
            result = $.parseJSON(result);
            if (result.state == "success") {
                phone_result = true;
                showBtn();
            } else {
                phone_result = false
                popup({ type: 'error', msg: result.message, delay: 2000, bg: true, clickDomCancel: true });
            }
        }
    });
}
//显示下一步按钮
function showBtn() {
    $("#next_btn").removeClass("btn_null");
    $("#next_btn").addClass("btn_ok");
}
//提交到服务器
function submit() {
    //验证数据
    var password = $("#password").val();
    if (password == "") {
        popup({ type: 'error', msg: "请输入密码", delay: 2000, bg: true, clickDomCancel: true });
        return;
    }
    var phone = $("#phone").val();
    var code = $("#code").val();
    $.ajax({
        type: "POST",
        url: "SaveRegister",
        data: { Phone: phone, PassWord: $.md5($.trim(password)) },
        success: function (result) {
            result = $.parseJSON(result);
            if (result.state == "success") {
                popup({
                    type: 'load', msg: result.message, delay: 1500, callBack: function () {
                        window.location.href = "/Home/Index";
                    }
                });
            } else {
                popup({ type: 'error', msg: result.message, delay: 2000, bg: true, clickDomCancel: true });
            }
        }
    });
}