(function ($) {
    var refresh = function() {
        $.get("/ShopLiquidate/ShopDealLogs", function(view) {
            $("#deallogs-panel").html(view);
        });
        $.get("/ShopLiquidate/ShopLiquidates", function(view) {
            $("#liquidates-panel").html(view);
        });
        $.get("/ShopLiquidate/ShopRollbacks", function(view) {
            $("#rollbacks-panel").html(view);
        });
    };
    $(".ckall").live("click", function () {
        var checked = $(this).attr("checked");
        $(".deallog :checkbox").attr("checked", checked);
    });
    $("#btnCreateLiquidate").live("click", function (evt) {
        evt.preventDefault();
        if ($(".deallog :checkbox[checked]").length == 0) {
            alert("请先选择要清算的记录！");
            return;
        }
        if (!confirm("确定清算当前交易记录？"))
            return;
        var ids = [];
        $(".deallog :checkbox[checked]").each(function () {
            ids.push(parseInt($(this).val()));
        });
        $.post("/ShopLiquidate/addLiquidate", { ids: $.toJSON(ids) }, function (data) {
            if (!data.Success) {
                alert(data.Message);
            } else {
                refresh();
            }
        });
    });
    $("#btnCreateRollback").live("click", function (evt) {
        evt.preventDefault();
        if ($(".deallog :checkbox[checked]").length == 0) {
            alert("请先选择要取消的记录！");
            return;
        }
        if (!confirm("确定取消当前交易记录？"))
            return;
        var ids = [];
        $(".deallog :checkbox[checked]").each(function () {
            ids.push(parseInt($(this).val()));
        });
        $.post("/ShopLiquidate/addRollback", { ids: $.toJSON(ids) }, function (data) {
            if (!data.Success) {
                alert(data.Message);
            } else {
                refresh();
            }
        });
    });
    $("#shopLiquidates .deleteLiquidate").live("click", function (evt) {
        evt.preventDefault();
        if (!confirm("确定删除当前交易记录？"))
            return;

        var href = $(this).attr("href");

        $.post(href, function (data) {
            if (!data.Success) {
                alert(data.Message);
            } else {
                refresh();
            }
        });
    });
    $("#shopLiquidates .deleteRollback").live("click", function (evt) {
        evt.preventDefault();
        if (!confirm("确定删除当前冲正申请？"))
            return;

        var href = $(this).attr("href");

        $.post(href, function (data) {
            if (!data.Success) {
                alert(data.Message);
            } else {
                refresh();
            }
        });
    });
})(jQuery);
