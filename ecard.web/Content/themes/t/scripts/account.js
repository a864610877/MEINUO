(function ($) {
    $(function () {
        $.accountEditor(
                $("#changeLimitAmount #accountName"),
                $("#changeLimitAmount #accountToken"),
                $("#changeLimitAmount #btnResetAccount"));
        $("#limitAmount").keydown(function (evt) {
            if (evt.keyCode == 13) {
                evt.preventDefault();
                $("#changeLimitAmount #save").trigger("click");
            }
        });
        $("#changeLimitAmount #accountName").bind("accountLoaded", function (evt) {
            evt.preventDefault();
            ecardSvc.loadAccount($("#changeLimitAmount #accountName").val(),
                    $("#changeLimitAmount #accountToken").val(),
                    function (rsp) {
                        $("#changeLimitAmount #accountInfo").displayContent("drop", 200, rsp.view);
                        $("#changeLimitAmount #save").disabled(!rsp.success);
                        if (rsp.success){
                            ecardSvc.getAccountLimit($("#changeLimitAmount #accountName").val(), $("#changeLimitAmount #accountToken").val(), function (rsp) {
                                $("#limitAmount").val(rsp.data);
                                $("#limitAmount").select().focus();
                            });
                        }
                    });
        });
        $("#changeLimitAmount #save").click(function (evt) {
            evt.preventDefault();

            $.post($(this).attr("href"),
                $(this).parents("form").serializeObject(),
                function (data) {
                    if (data.Success) {
                        alert("操作成功!");
                    } else {
                        alert(data.Message); 
                    }
                    $("#changeLimitAmount").bindfrom({});
                    $("#accountInfo").trigger("accountLoaded");
                });
        });

        if ($("#changeLimitAmount #accountName").val()) {
            $("#changeLimitAmount #accountName").trigger("accountLoaded");
        }
    });
})(jQuery);
