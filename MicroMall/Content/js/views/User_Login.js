// 验证手机号
function isPhoneNo(phone) {
    var pattern = /^1[34578]\d{9}$/;
    return pattern.test(phone);
}
//登录按钮点击事件
function login_btn() {
    var phone = $("#phone").val();
    var password = $("#password").val();
    if (phone == "") {
        popup({ type: 'error', msg: "请输入手机号码", delay: 2000, bg: true, clickDomCancel: true });
        return;
    }
    if (!isPhoneNo(phone)) {
        popup({ type: 'error', msg: "手机号码格式不正确", delay: 2000, bg: true, clickDomCancel: true });
        return;
    }
    if (password == "") {
        popup({ type: 'error', msg: "请输入密码", delay: 2000, bg: true, clickDomCancel: true });
        return;
    }
    //数据验证通过 提交登录
    $.ajax({
        type: "GET",
        url: "CheckLogin",
        data: { Phone: phone, PassWord: $.md5($.trim(password)),url:$("#H_URL").val() },
        success: function (result) {
            result = $.parseJSON(result);
            if (result.state == "success") {
                if (result.url != "" && result.url != null ) {
                    window.location.href = result.url;
                }
                else {
                    window.location.href = "/Home/Index";
                }
            } else {
                popup({ type: 'error', msg: result.message, delay: 2000, bg: true, clickDomCancel: true });
            }
        }
    });
}