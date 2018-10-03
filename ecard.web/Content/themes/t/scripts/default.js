(function ($) {
    $.fn.tab = function () {
        var data_key = "__________fn____tabs___";
        if ($(this).data(data_key))
            return this;
        $(this).data(data_key, true);
        var panel = $(this);
        $("li a", this).each(function () {
            $($(this).attr("href")).addClass("tabs-content");
            if (!$(this).parent().hasClass("on"))
                $($(this).attr("href")).hide(0);
        }).click(function () {
            var $this = $(this);

            $($("li.on a", panel).attr("href")).hide("clip", "fast", function () {
                $("li.on", panel).removeClass("on");
                $($this.attr("href")).show("clip", "fast");
                $this.parent().addClass("on");
            });
        });

    };
    $.fn.displayContent = function (effect, duration, view, callback) {
        $(this).hide(effect, duration, function () {
            $(this).html(view).show(effect, duration);
            if (callback) callback();
        });
    };

    $("a.get").live("click", function (evt) {
        evt.preventDefault();
        if ($(this).hasClass("disabled"))
            return;
        var target = $($(this).attr("target"));
        $.get($(this).attr("href"), function (view) {
            target.html(view);
        });
    });
    $("a.post").live("click", function (evt) {
        evt.preventDefault();
        if ($(this).hasClass("disabled"))
            return;
        var target = $($(this).attr("target"));
        var form = $(this).parents("form");
        var data = form.serializeArray();
        $.post($(this).attr("href"), data, function (view) {
            target.html(view);
        });
    });

    /* menu */
    $(".nav_tlink").live("click", function (evt) {
        evt.preventDefault();
        $("#mainnav .nav_tlink_on").removeClass("nav_tlink_on");
        $(this).parent().addClass("nav_tlink_on");
    });

    /* read account */
    $.accountEditor = function (accountName, token, reset) {
        if ($("[name='accountInputCtrl']", accountName.parent()).length > 0)
            return this;
        accountName.parent().append("<input type='text' name='accountInputCtrl' />");
        var editor = $("[name='accountInputCtrl']", accountName.parent());
        editor.css("position", "absolute");
        editor.css("left", "-10000");
        accountName.attr("readonly", "readonly");
        editor.bind("change", function (evt) {
            evt.preventDefault();
            token.val(editor.val().substring(16, 24));
            accountName.val(editor.val().substring(0, 16)).trigger("accountLoaded");
            $(this).val("");
        });
        if (reset) {
            reset.live("click", function (evt) {
                evt.preventDefault();
                token.val("");
                accountName.val("");
                editor.focus();
            });
        }
        editor.focus();
    };
    $.fn.disabled = function (b) {
        if (b) {
            $(this).addClass("disabled");
            $(this).attr("disabled", "disabled");
        } else {
            $(this).removeClass("disabled");
            $(this).removeAttr("disabled");
        }
    };
    $.setupGlobal = function (func) {
        $(function () {
            func();
        });

        $(document).ajaxSuccess(function () {
            func();
        });
    };
    $.setupGlobal(function () {
        $(".focused").focus();
        $(".tab").tab();
    });
})(jQuery);


(function ($) {
    // apis
    var EcardSvc= function () {

    };
    EcardSvc.prototype.loadAccount = function(accountName, accountToken, callback) {
        var url = String.formatUrl("/account/account?accountName={0}&accountToken={1}", accountName, accountToken);
        $.get(url, function(v) {
            var success = v.indexOf("accountNoFound")<0;
            callback({success:success, view:v});
        });
    };
    EcardSvc.prototype.getAccountLimit  = function(accountName, accountToken, callback) {
        var url = String.formatUrl("/account/getAccountLimit?accountName={0}&accountToken={1}", accountName, accountToken); 
        $.get(url, function(data) {
            callback({success:true, data:data});
        });
    };
    window.ecardSvc = new EcardSvc();
})(jQuery);
