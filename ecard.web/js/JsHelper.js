//var readAccount = function () {
//    return { accountName: "", accountToken: "11111111" };
//};
//var writeAccount = function (account) {
//    return true;
//};
(function ($) {
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

    $.extend($, {

        submitClicks: function (obj) {
            var PageIndex, PageSize;
            if (obj.getAttribute("value") > 0) {
                PageIndex = obj.getAttribute("value");
            } else if (obj.getAttribute("value") == "prev") {
                PageIndex = parseInt($(".current a").attr("value")) - 1;
            } else if (obj.getAttribute("value") == "next") {
                PageIndex = parseInt($(".current a").attr("value")) + 1;
            }
            index = PageIndex;
            if (pageS > 10) {
                PageSize = pageS;
            }
            var Datas = cearteJson(PageIndex, PageSize);
            $.ajax({
                url: "/Role/ListPost",
                data: Datas,
                type: "post",
                //cache: false, 
                dataType: "json",
                async: false,
                success: function (data) {

                    getData(data);
                },
                error: function () {
                    window.location.href = "/Home/UserError";
                }
            })
        }
    })


    $.extend($, {

        GetstrTrs: function (data, listVal, CheckBoxS) {
            $("#tbodysNum tr").remove();
            if (data.tables.length > 0) {

                var strTr = "";

                for (var i = 0; i < data.tables.length; i++) {
                    if (i % 2 == 0) {
                        strTr += "<tr>";
                    } else {
                        strTr += "<tr class='odd'>";
                    }
                    var item = data.tables[i];
                    for (var j = 0; j < listVal.length; j++) {
                        if (CheckBoxS != null && CheckBoxS != "undefined" && item.hasOwnProperty(listVal[0]) && j == 0) {
                            if (listVal[0] == CheckBoxS)
                                strTr += "<td><input name='" + listVal[0] + "' type='checkbox' value='" + item[listVal[0]] + "' /></td>";
                        }
                        else {
                            if (item.hasOwnProperty(listVal[j])) {
                                if (!isNaN(item[listVal[j]])) {
                                    strTr += "<td>" + (item[listVal[j]] != null ? item[listVal[j]] : " ") + "</td>";
                                }
                                else if (item[listVal[j]].hasOwnProperty("Action")) {
                                    var AccountsName = item[listVal[j]];
                                    if (AccountsName != null && AccountsName != "undefined") {
                                        strTr += "<td><a href=/" + AccountsName.Controller + "/" + AccountsName.Action + "/" + AccountsName.Key + ">" + AccountsName.Text + "</a></td>"
                                    }
                                }
                                else {
                                    if (item[listVal[j]] != null && item[listVal[j]] != "undefined" && item[listVal[j]].indexOf("/Date") == 0) {
                                        strTr += "<td>" + ChangeDateFormat(item[listVal[j]]) + "</td>";

                                    } else {
                                        strTr += "<td>" + (item[listVal[j]] != null ? item[listVal[j]] : " ") + "</td>";
                                    }
                                }

                            }
                        }
                    }

                }
                $("#tbodysNum").append(strTr);

            }
            if (data.html) {
                $("#pagehtml").html("");
                $("#pagehtml").append(data.html);
            }
        },
        GetstrTrs1: function (data, listVal, CheckBoxS) {

            $("#tbodysNum tr").remove();
            if (data.tables.length > 0) {

                var strTr = "";

                for (var i = 0; i < data.tables.length; i++) {
                    if (i % 2 == 0) {
                        strTr += "<tr>";
                    } else {
                        strTr += "<tr class='odd'>";
                    }
                    var item = data.tables[i];
                    for (var j = 0; j < listVal.length; j++) {
                        if (CheckBoxS != null && CheckBoxS != "undefined" && item.hasOwnProperty(listVal[0]) && j == 0) {
                            if (listVal[0] == CheckBoxS)
                                strTr += "<td><input name='" + listVal[0] + "' onclick='smallChange(this)' type='checkbox' value='" + item[listVal[0]] + "' /></td>";
                        }
                        else {
                            if (item.hasOwnProperty(listVal[j])) {
                                if (!isNaN(item[listVal[j]])) {
                                    strTr += "<td><p>" + (item[listVal[j]] != null ? item[listVal[j]] : " ") + "</p></td>";
                                }
                                else if (item[listVal[j]].hasOwnProperty("Action")) {
                                    var AccountsName = item[listVal[j]];
                                    if (AccountsName != null && AccountsName != "undefined") {
                                        strTr += "<td><a href=/" + AccountsName.Controller + "/" + AccountsName.Action + "/" + AccountsName.Key + ">" + AccountsName.Text + "</a></td>"
                                    }
                                }
                                else {
                                    if (item[listVal[j]] != null && item[listVal[j]] != "undefined" && item[listVal[j]].indexOf("/Date") == 0) {
                                        strTr += "<td><p>" + ChangeDateFormat(item[listVal[j]]) + "</p></td>";

                                    } else {
                                        strTr += "<td><p>" + (item[listVal[j]] != null ? item[listVal[j]] : " ") + "</p></td>";
                                    }
                                }

                            }
                        }
                    }

                }
                $("#tbodysNum").append(strTr);

            }
            if (data.html) {
                $("#pagehtml").html("");
                $("#pagehtml").append(data.html);
            }
        }
    })

}(jQuery));



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
//查询数据
function submitClicksUpdate() {
    var PageIndex, PageSize;
    if ($(".current a").attr("value") > 0) {
        PageIndex = $(".current a").attr("value");
    }
    if ($(".selectSize").val() < 10) {
        PageSize = 10;
    } else {
        PageSize = $(".selectSize").val();
    }
    index = PageIndex;
    if (pageS > 10) {
        PageSize = pageS;
    }
    AjaxGetData(PageIndex, PageSize);

}
//翻页数据
function submitClicks(obj) {
    var PageIndex, PageSize;
    if (obj.getAttribute("value") > 0) {
        PageIndex = obj.getAttribute("value");
    } else if (obj.getAttribute("value") == "prev") {
        PageIndex = parseInt($(".current a").attr("value")) - 1;
    } else if (obj.getAttribute("value") == "next") {
        PageIndex = parseInt($(".current a").attr("value")) + 1;
    }
    index = PageIndex;
    if (pageS > 10) {
        PageSize = pageS;
    }
    AjaxGetData(PageIndex, PageSize);

}

var ActionReturnUrl = "";
$(function () {
    if ($("[data-confirm]").length > 0) {
        $("[data-confirm]").live("click", function (evt) {
            var $this = $(this);
            if ($this.attr("data-confirm").length == 0) return true;
            var hre = $this.attr("onclick");
            var str = hre.toString().split("'")
            ActionReturnUrl = str[3];
            if (str[1] == "Edit" || str[1] == "Create" || str[1] == "Print") {
                window.location.href = ActionReturnUrl;
                return true;
            }
            $(".tipaa p").text($this.attr("data-confirm"));
            $(".ShowHide").fadeIn(100);
            $(".tipaa").fadeIn(200);
        });
    }
    
})

function Sures() {
    var PageIndex = $("#PageIndex").val();
    var PageSize = $("#PageSize").val();
    if (ActionReturnUrl != "") {
        var url = ActionReturnUrl + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize;
        var form = $($("#main").find("form")[0]);
        form.attr("action", url);
        form.trigger("submit");
    }
}


//确认操作
function Sure() {
    if (ActionUrl != "" && ActionName != null) {
        $.ajax({
            url: ActionUrl,
            data: { strIds: jsonData },
            type: "post",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.Code == 1) {
                    $(".tipinfo2 img").attr("src", "../../images/Succeed01.png");
                }
                else {
                    $(".tipinfo2 img").attr("src", "../../images/error02.png");
                }

                $(".tipright2 P").text(data.CodeText);
                $(".ShowHide").fadeIn(100);
                $(".tip2").fadeIn(200);
            }
        })
    }
}

//
function Sure_Args(obj) {
    var jsonVal = jQuery.parseJSON(obj)
    if (ActionUrl != "" && ActionName != null) {
        $.ajax({
            url: ActionUrl,
            data: jsonVal,
            type: "post",
            dataType: "json",
            async: false,
            success: function (data) {

                if (data.Code == 1) {
                    $(".tipinfo2 img").attr("src", "../../images/Succeed01.png");
                }
                else {
                    $(".tipinfo2 img").attr("src", "../../images/error02.png");
                }
                $(".tipright2 P").text(data.CodeText);
            }
        })
    }
}

function returnBack(obj) {
    submitClicksUpdate();
    $(".tip2").fadeOut(200);
    $("#btn_Sure").attr("onClick", "Sure()");
}
var data_canceled = "canceled";
//全选 
function selectAll() {
    var selectBox = $("#tbodysNum").find("input[type='checkbox']");

    var CkAllStatus = $("#ckAll").attr("checked");
    var checked = selectBox.attr("checked");
    if (CkAllStatus) {
        selectBox.attr("checked", true);
    }
    else {
        selectBox.attr("checked", false);
    }
}
//选中的角色
function getChecks() {
    var ids = "";
    var selectBox = $("input[type='checkbox']:checked");
    for (var i = 0; i < selectBox.length; i++) {
        var items = $(selectBox[i]).val();
        if (items != "" && items != null && items != "on") {
            if (i == selectBox.length - 1) {
                ids += items;
            }
            else {
                ids += items + ',';
            }
        }
    }
    return ids;
}
//时间格式
function ChangeDateFormat(cellval) {
    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    if (date.getHours() > 0) {
        var hh = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var mm = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var ss = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        return date.getFullYear() + "-" + month + "-" + currentDate + " " + hh + ":" + mm + ":" + ss;
    } else {
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }
}
function actionLink(link) {
    window.location.href = link;
}
//撤销
var data_canceled = "canceled";
var data_actionbefore = "data_actionbefore";
$.fn.setCancel = function () {
    var $this = $(this);
    $this.data(data_canceled, true);
    window.setTimeout(function () { $this.removeData(data_canceled); }, 0);
};