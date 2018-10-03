$(function () {
    var setPos = function (script) {
        $("body").append("<script src='" + script + "'></script>");
    };



    $(function () {
        $(".script-selector").each(function () { setPos($(this).val()); });
        $(".ExplainSpan").focus(function () {
            $("[forprop='" + $(this).attr("id") + "']").show("1500");
        });
        $(".create_field select").click(function () {
            $("[forprop='" + $(this).attr("id") + "_Key']").show("1500");
        });
        $(".ExplainSpan").blur(function () {
            $("[forprop='" + $(this).attr("id") + "']").hide("1500");
        });
        $(".create_field select").mouseleave(function () {
                $("[forprop='" + $(this).attr("id") + "_Key']").hide("1500");
            });
    });
    $(".script-selector").live("change", function (evt) {
        evt.preventDefault();
        if ($(this).val() == '')
            return;
        setPos($(this).val());
    });
    $("#btnDisplayAll").live("click", function (evt) {
        evt.preventDefault();
        if ($(this).html() == "全屏显示") {
            $("#header, #sidebar, #footer,#topDiv").hide(0);
            $(this).html("窗口显示");
        } else {
            $("#header, #sidebar, #footer,#topDiv").show(0);
            $(this).html("全屏显示");
        }
    }); //

    $("[data-enter]").live("keydown", function (evt) {
        var $this = $(this);
        if (evt.keyCode == 13) {
            evt.preventDefault();
            $($this.attr("data-enter")).trigger("click");
        }
    });
//    $("[data-val-number]").live("blur", function (evt) {
//        if ($(this).val() == "") {
//            $(this).val("0");
//        }

//    });
    $(".Account [data-command='ChangeAccountPassword']").live("click", function (evt) {
        evt.preventDefault();
        $(this).setCancel();
        var url = $(this).attr("href");
        confirmPassword(function (password, passwordConfirm) {
            $.post(url, { password: password, passwordConfirm: passwordConfirm, challengeData: '' }, function (data) {
                if (data.Success)
                    alert("修改成功");
                else
                    alert(data.Message);
            });
        }, null);
    });
    $(".Account [data-command='Suspend']").live("click", function (evt) {
        evt.preventDefault();
        var $this = $(this);
        $this.setCancel();
        var url = $(this).attr("href");
        if (!confirm("您确认要停用当前卡号？"))
            return false;
        $.post(url, {}, function (data) {
            if (data.Success) {
                if (data.Data1)
                    printTicket(data.Data1);
                else
                    alert("停用成功");
                $this.hide();
            }
            else
                alert(data.Message);
        });
    });
    $(".Account [data-command='Resume']").live("click", function (evt) {
        evt.preventDefault();
        var $this = $(this);
        $this.setCancel();
        var url = $(this).attr("href");
        if (!confirm("您确认要启用当前卡号？"))
            return false;
        $.post(url, {}, function (data) {
            if (data.Success) {
                if (data.Data1)
                    printTicket(data.Data1);
                else
                    alert("启用成功");
                $this.hide();
            }
            else
                alert(data.Message);
        });
    });
    $(".PrePay [data-command='Done']").live("click", function (evt) {
        evt.preventDefault();
        var $this = $(this);
        $this.setCancel();
        var url = $(this).attr("href");
        var v = prompt("请输入要预授权的金额, 不填或者填0 表示 将全部金额预授权完成！");
        if (v == null)
            return false;
        if (!v)
            v = 0;
        else
            v = parseInt(v);

        if (!confirm(String.format("您确认要强制完成当前预授权, 预授权金额 {0}？注意：本操作不可恢复！", v == 0 ? "全部" : v + "元")))
            return false;
        $.post(url, { amount: v }, function (data) {
            if (data.Success) {
                //                if (data.Data1)
                //                    printTicket(data.Data1);
                //                else
                alert("强制完成预授权成功");
                window.location = "/PrePay/list";
            }
            else
                alert(data.Message);
        });
    });
    $(".PrePay [data-command='Cancel']").live("click", function (evt) {
        evt.preventDefault();
        var $this = $(this);
        $this.setCancel();
        var url = $(this).attr("href");

        if (!confirm("您确认要强制取消当前预授权, 预授权金额将被全部取消？注意：本操作不可恢复！"))
            return false;
        $.post(url, {}, function (data) {
            if (data.Success) {
                //                if (data.Data1)
                //                    printTicket(data.Data1);
                //                else
                alert("强制取消预授权成功");
                window.location = "/PrePay/list";
            }
            else
                alert(data.Message);
        });
    });
});
