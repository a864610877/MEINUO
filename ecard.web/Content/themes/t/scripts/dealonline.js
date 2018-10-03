(function ($) {
    $.setupGlobal(function () {
        $.accountEditor(
            $("#accountName"),
            $("#accountToken"),
            $("#changeLimitAmount #btnReadAccount"));
    });
    /* print ticket */
    $("#ticketPrinter").live("click", function (evt) {
        evt.preventDefault();
        printTicket2($(this).attr("href"));
    });
    /* shop of pay */
    $("#payShop #shopName").live("keyup", function (evt) {
        $("#next").addClass("disabled");
    }).live("keydown", function (evt) {
        if (evt.keyCode == 13) {
            if (!$("#next").hasClass("disabled")) {
                evt.preventDefault();
                $("#next").trigger("click");
            } else {
                evt.preventDefault();
                var $this = $(this);
                $this.addClass("loading");
                $.get("/dealonline/shops?shopName=" + $(this).val(), function (view) {
                    $("#shops").displayContent("clip", 300, view, function () {
                        $this.removeClass("loading");

                        if ($("#shops .select a").length == 1) {
                            $("#shops .select a").trigger("click");
                        }
                    });
                });
            }
        }
    });
    $("#payShop #shops .select a").live("click", function (evt) {
        evt.preventDefault();
        $("#payShop #shopName").val($(this).attr("href"));

        $.get("/dealonline/shop?shopName=" + $(this).attr("href"), function (view) {
            var panel = $(view);
            $("#shop").hide("drop", 200, function () {
                $("#shop").empty().append(panel).show("drop", 200);
            });
            $("#next").removeClass("disabled");
        });
    });

    /* account of pay */

    var enableNextInAccountOfPay = function (enable) {
        if (enable && parseFloat($("#payAccount #amount").val()) > 0 && $("#payAccount .accountNoFound").length == 0) {
            $("#next").removeClass("disabled");
        } else {
            $("#next").addClass("disabled");
        }
    };

    var loadAccountInPay = function (proxy) {
        $.get("/account/account?accountName=" + $(proxy).val() + "&accountToken=" + $("#accountToken").val(), function (view) {
            $("#payAccount #account").displayContent("drop", 200, view, function () {
                enableNextInAccountOfPay(true);
                $("#payAccount #amount").focus();
            });
        });
    };
    $("#payAccount #accountName").live("accountLoaded", function (evt) {
        evt.preventDefault();
        loadAccountInPay(this);
    }).live("keydown", function (evt) {
        if (evt.keyCode == 13) {
            evt.preventDefault();
            if ($("#next").hasClass("disabled")) {
                loadAccountInPay(this);
            } else {
                $("#next").trigger("click");
            }
        }
    }).live("keyup keypress", function (evt) {
        if ((evt.type == "keyup" && (evt.keyCode == 46 || evt.keyCode == 86 || evt.keyCode == 88 || evt.keyCode == 8)) || evt.type == "press")
            enableNextInAccountOfPay(false);
    });
    $("#payAccount #amount").live("keyup", function () {
        enableNextInAccountOfPay(true);
    }).live("keydown", function (evt) {
        if (evt.keyCode == 13 && !$("#next").hasClass("disabled")) {
            evt.preventDefault();
            $("#next").trigger("click");
        }
    });

    /* confirm Of Pay */

    $("#payDone").live("click", function (evt) {
        evt.preventDefault();
        var target = $($(this).attr("target"));
        var form = $(this).parents("form");
        var data = form.serializeObject();
        inputPassword(function (password) {
            data.Password = password;
            $.post("/dealonline/paydone", data, function (view, status, xhr) {
                var contentType = xhr.getResponseHeader("Content-Type") || "text/html";
                if (contentType.toLowerCase().indexOf("application/json") !== -1) {
                    alert(view.Message);
                    return;
                }

                target.html(view);
            });
        });
    });

    /* deal of canceppay */
    $("#cancelPaySerialNo #serverSerialNo").live("change", function (evt) {
        evt.preventDefault();
        var $this = $(this);
        $this.addClass("loading");
        $.get("/dealonline/deallog?serverSerialNo=" + $(this).val(), function (view) {
            $("#deal").displayContent("clip", 300, view, function () {
                $this.removeClass("loading");
                if ($("#dealNonFound").length == 0)
                    $("#next").removeClass("disabled");
            });
        });
    }).live("keyup", function (evt) {
        $("#next").addClass("disabled");
    }).live("keydown", function (evt) {
        if (evt.keyCode == 13 && !$("#next").hasClass("disabled")) {
            evt.preventDefault();
            $("#next").trigger("click");
        }
    });

    /* account of cancel pay */

    var enableNextInAccountOfCancelPay = function (enable) {
        if (enable && $("#cancelPayAccount .accountNoFound").length == 0) {
            $("#next").removeClass("disabled");
        } else {
            $("#next").addClass("disabled");
        }
    };

    var loadAccountInCancelPay = function (proxy, callback) {
        $.get("/account/account?accountName=" + $(proxy).val() + "&accountToken=" + $("#accountToken").val(), function (view) {
            $("#cancelPayAccount #account").displayContent("drop", 200, view, function () {
                enableNextInAccountOfPay(true);
                if (callback) callback();
            });
        });
    };
    $("#cancelPayAccount #accountName").live("accountLoaded", function (evt) {
        evt.preventDefault();
        loadAccountInCancelPay(this, function () {
            enableNextInAccountOfCancelPay(true);
        });
    }).live("keydown", function (evt) {
        if (evt.keyCode == 13) {
            evt.preventDefault();
            if ($("#next").hasClass("disabled")) {
                loadAccountInCancelPay(this, function () {
                    enableNextInAccountOfCancelPay(true);
                });
            } else {
                $("#next").trigger("click");
            }
        }
    }).live("keyup keypress", function (evt) {
        if ((evt.type == "keyup" && (evt.keyCode == 46 || evt.keyCode == 86 || evt.keyCode == 88 || evt.keyCode == 8)) || evt.type == "press")
            enableNextInAccountOfCancelPay(false);
    });
    $("#cancelPayAccount #amount").live("keyup", function () {
        enableNextInAccountOfCancelPay(true);
    }).live("keydown", function (evt) {
        if (evt.keyCode == 13 && !$("#next").hasClass("disabled")) {
            evt.preventDefault();
            $("#next").trigger("click");
        }
    });
    $("#cancelPayAccount").live("keydown", function (evt) {
        if (evt.keyCode == 13) {
            if (!$("#next").hasClass("disabled")) {
                $("#next").trigger("click");
            }
        }
    })
    /* confirm of cancel pay */
    $("#cancelPayDone").live("click", function (evt) {
        evt.preventDefault();
        var target = $($(this).attr("target"));
        var form = $(this).parents("form");
        var data = form.serializeObject();
        inputPassword(function (password) {
            data.Password = password;
            $.post("/dealonline/cancelpaydone", data, function (view, status, xhr) {
                var contentType = xhr.getResponseHeader("Content-Type") || "text/html";
                if (contentType.toLowerCase().indexOf("application/json") !== -1) {
                    alert(view.Message);
                    return;
                }

                target.html(view);
            });
        });
    });
})(jQuery);
