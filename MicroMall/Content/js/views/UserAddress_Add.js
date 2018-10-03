$(function () {
    $("#province1").val("");
    $("#district1").val("");
    $("#city1").val("");
});
// 验证手机号
function isPhoneNo(phone) {
    var pattern = /^1[34578]\d{9}$/;
    return pattern.test(phone);
}
//提交新增
function submit() {
    //数据验证
    var Consignee = $("#Consignee").val();
    if (Consignee == "") {
        popup({ type: 'error', msg: "请输入收货人姓名", delay: 2000, bg: true, clickDomCancel: true });
        return false;
    }
    var ConsigneePhone = $("#ConsigneePhone").val();
    if (ConsigneePhone == "") {
        popup({ type: 'error', msg: "请输入收货人手机号码", delay: 2000, bg: true, clickDomCancel: true });
        return false;
    }
    if (!isPhoneNo(ConsigneePhone)) {
        popup({ type: 'error', msg: "请输入正确的手机号码格式", delay: 2000, bg: true, clickDomCancel: true });
        return false;
    }
    var p = $("#province1").val();
    var d = $("#district1").val();
    var c = $("#city1").val();
    if (p == "" || d == "" || c == "") {
        popup({ type: 'error', msg: "请选择省市区！", delay: 2000, bg: true, clickDomCancel: true });
        return false;
    }
    var Address = $("#Address").val();
    if (Address == "") {
        popup({ type: 'error', msg: "请输入收货地址", delay: 2000, bg: true, clickDomCancel: true });
        return false;
    }
    //验证通过拼接收货地址
    Address = p + "-" + d + "-" + c + "-" + Address;
    //提交数据
    $.ajax({
        type: "POST",
        url: "SaveAdd",
        data: { Consignee: Consignee, ConsigneePhone: ConsigneePhone, Address: Address, IsDefault: 1 },
        success: function (result) {
            result = $.parseJSON(result);
            if (result.state == "success") {
                popup({
                    type: 'load', msg: result.message, delay: 1000, callBack: function () {
                        window.location.href = "/UserAddress/Index";
                    }
                });
            } else {
                popup({ type: 'error', msg: result.message, delay: 2000, bg: true, clickDomCancel: true });
            }
        }
    });
}