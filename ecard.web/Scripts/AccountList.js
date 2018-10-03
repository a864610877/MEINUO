$(function () {
    $(".Detail a").live("click", function () {
        var url = $(this).attr("href");
        $("body").eq(0).addClass("wait");
        $.post(url, "", function (data, success) {
            try {
                $("#AccountName").val(data.AccountName);
                $("#AccountId").val(data.AccountId);
                $("#AccountLevel").val(data.AccountLevel);
                $("#Amount").val(data.Amount);
                $("#Accountpoint").val(data.Point);
                $("#AccountPrePay").val(data.PrePay);
                $("#ExpiredDate").val(data.ExpiredDate);
                $("#RealName").val(data.DisplayName);
                $("#IdentityCard").val(data.IdentityCard);
                $("#Gender").val(data.Gender); //select
                $("#saveAccount").attr("disabled", false);
                $("#Birthday").val(data.BirthDate);
                $("#Address").val(data.Address);
                $("#Mobile").val(data.Mobile);
                $("#Phone").val(data.Phone);
                $("#Email").val(data.Email);
                $(".readonlyItem").attr("disabled", true);
            }
            catch (ex) { alert("无法读取会员信息"); }
            $("body").eq(0).removeClass("wait");
        }, null);
        return false;
    });
});
$(function () {
    $("#Birthday").datepicker();
    if ($("#AccountId").val().length < 1)
    { $("#saveAccount").attr("disabled", true); }
    $("#saveAccount").live("click", function () {
        if ($("#Gender").val() == 0)
        { $("#GenderSpan").show(); }
        else {
            var item = new Object();
            item.AccountName = $("#AccountName").val();
            item.AccountId = $("#AccountId").val();
            item.AccountLevel = $("#AccountLevel").val();
            item.Amount = $("#Amount").val();
            item.Accountpoint = $("#Accountpoint").val();
            item.AccountPrePay = $("#AccountPrePay").val();
            item.ExpiredDate = $("#ExpiredDate").val();
            item.DisplayName = $("#RealName").val();
            item.IdentityCard = $("#IdentityCard").val();
            item.Gender = $("#Gender").val();
            item.BirthDate = $("#Birthday").val();
            item.Address = $("#Address").val();
            item.Mobile = $("#Mobile").val();
            item.Phone = $("#Phone").val();
            item.Email = $("#Email").val();
            item.NewPassWord = $("#passWord").val();
            $.post("../Account/SaveAccount", item, function (data, status) {
                if (status == "success") {
                    alert(data);
                }
                else
                { alert("系统忙，请稍后再试"); }
            }, null);
        }
    });
});
$("#ChangeShopBut").live("click", function () {
    //如果商户列表为空，则加载
    var j = 0;
    var ids = new Array();
    $(".ckitem[type=checkbox]").each(function () {
        if ($(this).attr("checked")) {
            ids[j] = $(this).attr("datakey");
            j++;
        }
    });
    if (j < 1)
        return false;
    $.post("../Account/GetShops/1", { "ids": ids }, function (data, status) {
        if (status == "success") {
            try {
                $("#shopList").html("");
                for (var i = 0; i < data.length; i++) {
                    $("#shopList").append($("<option value='" + data[i].Id + "'>" + data[i].DisplayName + "</option>"));
                }
                Util.Dialog({
                    boxID: "changeshopBox",
                    title: "卡调拨",
                    width: 220,
                    height: 60,
                    content: "text:" + $("#ChangeShopBoxDiv").html(),
                    showbg: true,
                    closestyle: "orange"
                });
                return false;

            } catch (e) {
                alert(e.Message);
            }
        }
    }, null);
});
function submitshopchange() {
    var i = 0; var arr = new Array;
    $(".ckitem[type=checkbox]").each(function () {
        if ($(this).attr("checked")) {
            arr[i] = $(this).attr("datakey");
            i++;
        }
    });
    if (i > 0) {
        var shopId = $("#changeshopBox [id='shopList']").val();
        $.post("../Account/ChangeShop", { "ids": arr, "shopId": shopId }, function (data, status) {
            if (status == "success") {
                alert(data);
                location.reload();
            }
        }, null);
    }
}
$(function () {
    $("#RechargeBut").live("click", function () {
        $(".checkRechargeAmount").hide();
        var j = 0;
        var ids = new Array();
        $(".ckitem[type=checkbox]").each(function () {
            if ($(this).attr("checked")) {
                ids[j] = $(this).attr("datakey");
                j++;
            }
        });
        if (j < 1)
            return false;
        Util.Dialog({
            boxID: "rechargeBox",
            title: "卡充值",
            width: 220,
            height: 60,
            content: "text:" + $("#rechargeBoxDiv").html(),
            showbg: true,
            closestyle: "orange"
        });
        return false;
    });
});
function CheckValue(but) {
    var r = "^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
    var reg = new RegExp(r);
    if (reg.test($(but).val())) {
        $(".checkRechargeAmount").text("√").show();
    }
    else { $(".checkRechargeAmount").text("×").show(); }
}
function submitRecharge() {
    $("body").eq(0).addClass("wait");
    var r = "^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
    var reg = new RegExp(r);
    if (reg.test($("#rechargeBox [id='amountValue']").val())) {
        $(".checkRechargeAmount").text("√").show();
        var i = 0; var arr = new Array;
        $(".ckitem[type=checkbox]").each(function () {
            if ($(this).attr("checked")) {
                arr[i] = $(this).attr("datakey");
                i++;
            }
        });
        if (i > 0) {
            var form = $("<form></form>")
            form.attr('action', "/Recharge/Recharge")
            form.attr('method', 'post')
            input1 = $("<input type='hidden' name='ids' />")
            input1.attr('value', arr)
            input2 = $("<input type='text' name='rechargeAmount' value='" + $("#rechargeBox [id='amountValue']").val() + "' />")
            form.append(input1)
            form.append(input2)
            form.appendTo("body")
            form.css('display', 'none')
            form.submit()
        }
    }
    else { $(".checkRechargeAmount").text("×").show(); }

}



//冻结解冻。
$(function () {
    $("#FreezeBut").live("click", function () {
        var j = 0;
        var ids = new Array();
        $(".ckitem[type=checkbox]").each(function () {
            if ($(this).attr("checked")) {
                ids[j] = $(this).attr("datakey");
                j++;
            }
        });
        if (j < 1)
            return false;
        var form = $("<form></form>")
        form.attr('action', $(this).attr("href"));
        form.attr('method', 'post');
        input1 = $("<input type='hidden' name='ids' />");
        input1.attr('value', ids);
        form.append(input1);
        form.appendTo("body");
        form.css('display', 'none');
        form.submit();
    });
    $("#UnFreezeBut").live("click", function () {
        var j = 0;
        var ids = new Array();
        $(".ckitem[type=checkbox]").each(function () {
            if ($(this).attr("checked")) {
                ids[j] = $(this).attr("datakey");
                j++;
            }
        });
        if (j < 1)
            return false;
        var form = $("<form></form>")
        form.attr('action', $(this).attr("href"));
        form.attr('method', 'post');
        input1 = $("<input type='hidden' name='ids' />");
        input1.attr('value', ids);
        form.append(input1);
        form.appendTo("body");
        form.css('display', 'none');
        form.submit();
    });
});

$(function () {
    $("#DelayBut").live("click", function () {
        var j = 0;
        var ids = new Array();
        $(".ckitem[type=checkbox]").each(function () {
            if ($(this).attr("checked")) {
                ids[j] = $(this).attr("datakey");
                j++;
            }
        });
        if (j < 1)
            return false;
        var form = $("<form></form>")
        form.attr('action', $(this).attr("href"));
        form.attr('method', 'post');
        input1 = $("<input type='hidden' name='ids' />");
        input1.attr('value', ids);
        form.append(input1);
        form.appendTo("body");
        form.css('display', 'none');
        form.submit();
    });
});

$(function () {
    $("#DeadCardBut").live("click", function () {
        var j = 0;
        var ids = new Array();
        $(".ckitem[type=checkbox]").each(function () {
            if ($(this).attr("checked")) {
                ids[j] = $(this).attr("datakey");
                j++;
            }
        });
        if (j < 1)
            return false;
        var form = $("<form></form>")
        form.attr('action', $(this).attr("href"));
        form.attr('method', 'post');
        input1 = $("<input type='hidden' name='ids' />");
        input1.attr('value', ids);
        form.append(input1);
        form.appendTo("body");
        form.css('display', 'none');
        form.submit();
    });
});

$(function () {
    $("#ChangeAccountNameBut").live("click", function () {
        var j = 0;
        var id = "";
        $(".ckitem[type=checkbox]").each(function () {
            if ($(this).attr("checked")) {
                id = $(this).parent().parent().find("a").text().replace(" ", "");
                j++;
            }
        });
        if (j < 1)
            return false;
        if (j != 1) {
            alert("换卡时不能多选");
            return false;
        }
        $("body").eq(0).addClass("wait");
        $.post("/ChangeName/CheckOldName", { "accountName": id }, function (data, status) {
            $("body").eq(0).removeClass("wait");
            if (status == "success") {
                if (data == "true") {
                    $("#oldNameText").text(id);
                    Util.Dialog({
                        boxID: "ChangeNameBox",
                        title: "换卡",
                        width: 220,
                        height: 80,
                        content: "text:" + $("#ChangeNameDiv").html(),
                        showbg: true,
                        closestyle: "orange"
                    });
                }
                else {
                    alert("选择的会员卡不符合换卡条件。");
                }
            }
        }, null);
        return false;
    });
});

function submitAccountNameChange() {
    var oldName = $("#ChangeNameBox [id='oldNameText']").text().replace(" ", "");
    var newName = $("#ChangeNameBox [id='newNameText']").val().replace(" ", "");
    if (oldName != "" && newName != "") {
        $.post("/ChangeName/ChangeName", { "oldName": oldName, "newName": newName }, function (data, status) {
            if (status == "success") {
                alert(data);
                location.Reload();
            }
        }, null);
    }
}