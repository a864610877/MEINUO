/// <reference path="jquery.js"/>
String.format = function (f) {
    for (var i = 1; i < arguments.length; i++) {
        var partnern = "\\{" + (i - 1) + "(:(\\S+?))?\\}";
        var reg = new RegExp(partnern, "g");
        var matchValue = f.match(reg);
        if (matchValue)
            for (var k = 0; k < matchValue.length; k++) {
                var s = matchValue[k];

                var p = s.indexOf(":");
                if (p > 0) {
                    var key = s.substring(p + 1, s.length - 1);
                    f = f.replace(s, arguments[i][key]);
                }
                else
                    f = f.replace(s, arguments[i]);
            };
    }
    return f;
};
String.formatUrl = function (f) {
    var args = [];
    args.push(f);
    for (var i = 1; i < arguments.length; i++) {
        args.push(encodeURIComponent(arguments[i]));
    }
    return String.format.apply(this, args);
};
String.prototype.ltrim = function () {
    return this.replace(/^[\s]*/gi, "");
};

String.prototype.rtrim = function () {
    return this.replace(/[\s]*$/gi, "");
};

String.prototype.trim = function () {
    return this.ltrim().rtrim();
};
Date.fromIdentity = function (identity) {
    if (identity.length == 15) {
        return String.format("19{0}-{1}-{2}", identity.substring(6, 8), identity.substring(8, 10), identity.substring(10, 12));
    }
    if (identity.length == 18) {
        return String.format("{0}-{1}-{2}", identity.substring(6, 10), identity.substring(10, 12), identity.substring(12, 14));
    }
    return "";
};
(function ($) {
    $(":file").live("change", function () {
        var name = $(this).attr("name");
        //        $(String.format("[name={0}]", name)).attr("src", $(this).val()).show();
    });
    var setBinders = {
        "input": function (ele, value) {
            if ($(ele).attr("type").toLowerCase() == "checkbox")
                $(ele).attr("checked", value == undefined ? false : value);
            else
                $(ele).val(value);
        },
        "select": function (ele, value) {
            $(ele).val(value);
        },
        "textarea": function (ele, value) {
            $(ele).val(value);
        },
        "img": function (ele, value) {
            $(ele).attr("src", value);
        }
    };
    var getBinders = {
        "input": function (ele) {
            if ($(ele).attr("type").toLowerCase() == "checkbox")
                return $(ele).attr("checked");
            return $(ele).val();
        },
        "select": function (ele) {
            return $(ele).val();
        },
        "textarea": function (ele) {
            return $(ele).val();
        }
    };
    $.fn.bindfrom = function (data) {
        $("[data-field]", this).each(function () {
            var field = $(this).attr("data-field");
            var value = data[field];
            if ($(this).attr("data-field-format"))
                value = String.format($(this).attr("data-field-format"), value);
            if (setBinders[$(this).attr("tagName").toLowerCase()])
                setBinders[$(this).attr("tagName").toLowerCase()](this, value);
            else
                $(this).text(value);
        });
    };
    $.fn.bindto = function (data) {
        data = data || {};
        $("[data-field]", this).each(function () {
            var field = $(this).attr("data-field");
            if (getBinders[$(this).attr("tagName").toLowerCase()])
                data[field] = getBinders[$(this).attr("tagName").toLowerCase()](this);
            else
                data[field] = $(this).text();
        });
        return data;
    };

    $("[data-change-callback]").live("change", function () {
        var url = String.format($(this).attr("data-change-callback"), $(this).val());
        if (!url || url.length == 0) return;
        var name = $(this).attr("name");
        $.get(url, function (data) {
            $("[data-callback-accepter=" + name + "]").html(data);
        });
    });
})(jQuery);
$(function () {
    $("[data-change-callback]").each(function () {
        var url = String.format($(this).attr("data-change-callback"), $(this).val());
        if (!url || url.length == 0) return;
        var name = $(this).attr("name");
        $.get(url, function (data) {
            $("[data-callback-accepter=" + name + "]").html(data);
        });
    });
})