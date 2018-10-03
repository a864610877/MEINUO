$(function () {
    $(".content ul li:last").css("margin-bottom", "6rem");
})
function delete_btn(id) {
    $('#type-dialogBox').dialogBox({
        type: 'correct',
        autoSize: true,
        hasMask: true,
        hasBtn: true,
        effect: 'fall',
        title: '操作确认',
        content: '是否要删除地址？',
        confirm: function () {
            $.ajax({
                type: "POST",
                url: "DeleteAddress",
                data: { id: id },
                success: function (result) {
                    result = $.parseJSON(result);
                    if (result.state == "success") {
                        popup({ type: 'success', msg: result.message, delay: 2000, bg: true, clickDomCancel: true });
                        $("#li_" + id).remove();
                    } else {
                        popup({ type: 'error', msg: result.message, delay: 2000, bg: true, clickDomCancel: true });
                    }
                }
            });
        }
    });
}
function setDefault(id) {
    $('#type-dialogBox').dialogBox({
        type: 'correct',
        autoSize: true,
        hasMask: true,
        hasBtn: true,
        effect: 'fall',
        title: '操作确认',
        content: '是否设为默认？',
        confirm: function () {
            $.ajax({
                type: "POST",
                url: "SetDefault",
                data: { id: id },
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
    });
}