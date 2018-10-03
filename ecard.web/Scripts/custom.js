$(document).ready(function () {

    // Navigation menu

    $('ul#navigation').superfish({
        delay: 1000,
        animation: { opacity: 'show', height: 'show' },
        speed: 'fast',
        autoArrows: true,
        dropShadows: false
    });

    $('ul#navigation li').hover(function () {
        $(this).addClass('sfHover2');
    },
    function () {
        $(this).removeClass('sfHover2');
    });

    // Accordion
    $("#accordion, #accordion2").accordion({ header: "h3" });

    // Tabs
    $('#tabs, #tabs2, #tabs5').tabs();

    // Dialog           


    // Login Dialog Link
    $('#login_dialog').click(function () {
        $('#login').dialog('open');
        return false;
    });

    // Login Dialog         
    $('#login').dialog({
        autoOpen: false,
        width: 300,
        height: 230,
        bgiframe: true,
        modal: true,
        buttons: {
            "Login": function () {
                $(this).dialog("close");
            },
            "Close": function () {
                $(this).dialog("close");
            }
        }
    });

    // Dialog Link
    $('#dialog_link').click(function () {
        $('#dialog').dialog('open');
        return false;
    });

    // Dialog auto open         
    $('#welcome').dialog({
        autoOpen: true,
        width: 470,
        height: 180,
        bgiframe: true,
        modal: true,
        buttons: {
            "View Admintasia V1.0": function () {
                $(this).dialog("close");
            }
        }
    });

    // Dialog auto open         
    $('#welcome_login').dialog({
        autoOpen: true,
        width: 370,
        height: 430,
        bgiframe: true,
        modal: true,
        buttons: {
            "Proceed to demo !": function () {
                window.location = "index.php";
            }
        }
    });

    // Datepicker 
    $('.datepicker').datepicker({
        dateFormat: "yy-mm-dd",  //设置日期格式
        changeMonth: true,   //是否提供月份选择
        changeYear: true,    //是否提供年份选择
        dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],  //日期简写名称
        monthNamesShort: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月']  //月份简写名称
    });
    //Hover states on the static widgets
    $('#dialog_link, ul#icons li').hover(
        function () { $(this).addClass('ui-state-hover'); },
        function () { $(this).removeClass('ui-state-hover'); }
    );

    //Sortable

    $(".column").sortable({
        connectWith: '.column'
    });

    //Sidebar only sortable boxes
    $(".side-col").sortable({
        axis: 'y',
        connectWith: '.side-col'
    });

    $(".portlet").addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
        .find(".portlet-header")
            .addClass("ui-widget-header")
            .prepend('<span class="ui-icon ui-icon-circle-arrow-s"></span>')
            .end()
        .find(".portlet-content");

    $(".portlet-header .ui-icon").click(function () {
        $(this).toggleClass("ui-icon-circle-arrow-n");
        $(this).parents(".portlet:first").find(".portlet-content").slideToggle();
    });

    $(".column").disableSelection();

    $("tbody tr:odd ").addClass("odd");
    $("tbody tr:even ").addClass("even");
    $(".header").append('<span class="ui-icon ui-icon-carat-2-n-s"></span>');
    $("a.sorter").live("click", function (evt) {
        evt.preventDefault();
        var property = $(this).attr("href");
        if (property == "#") return;
        var form = $($(this).parents("form")[0]);
        $("#OrderBy", form).val(property);
        form.trigger("submit");
    });
    $("select.sorter").live("change", function (evt) {
        evt.preventDefault();
        var property = $(this).val();
        if (property == "#") return;
        var form = $($(this).parents("form")[0]);
        $("#OrderBy", form).val(property);
        form.trigger("submit");
    });
    $("a.pageindex").live("click", function (evt) {
        evt.preventDefault();
        var property = $(this).attr("href");
        if (property == "#" || property == "0") return;

        var form = $(this).attr("data-target") || $($(this).parents("form")[0]);
        $("#PageIndex", form).val(property);
        form.trigger("submit");
    });
    $("input.pageindex").live("change", function (evt) {
        evt.preventDefault();
        var val = parseInt($(this).val());
        if (val == 0) return;

        var form = $($(this).parents("form")[0]);
        $("#PageIndex", form).val(val);
        form.trigger("submit");
    });

    $(".pageindexchange").live("change", function (evt) {
        evt.preventDefault();
        var val = parseInt($(this).val());
        if (val == 0) return;

        var form = $($(this).parents("form")[0]);
        $("#PageIndex", form).val(val);
        form.trigger("submit");
    });
    $(".pagesize").live("change", function (evt) {
        evt.preventDefault();
        var val = parseInt($(this).val());
        if (val == 0) return;

        var form = $($(this).parents("form")[0]);
        $("#PageSize", form).val(val);
        form.trigger("submit");
    });
    $(":submit").live("click", function (evt) {
        evt.preventDefault();
        var form = $($(this).parents("form")[0]);
        var parser = $.urlParser.parse(form.attr("action"));
        $("#PageIndex", form).remove();
        form.attr("action", parser.assemble());
        form.trigger("submit");
    });
    var data_canceled = "canceled";
    var data_actionbefore = "data_actionbefore";
    $("[data-confirm]").live("click", function (evt) {
        var $this = $(this);
        if ($this.attr("data-confirm").length == 0 || confirm($this.attr("data-confirm"))) return true;
        evt.preventDefault();
        evt.stopPropagation();
        $this.data(data_canceled, true);

        setTimeout(function () {
            $this.removeData(data_canceled);
        }, 0);
    });
    $.fn.setCancel = function () {
        var $this = $(this);
        $this.data(data_canceled, true);
        window.setTimeout(function () { $this.removeData(data_canceled); }, 0);
    };
    $(".post").live("click", function (evt) {
        evt.preventDefault();
        var canceled = $(this).data(data_canceled) || false;
        if (canceled) return;
        var form = $($(this).parents("form")[0]);
        alert($(this).attr("href"));
        form.data(data_actionbefore, form.attr("action"));
        form.attr("action", $(this).attr("href"));
        form.trigger("submit");
        setTimeout(function () {
            form.attr("action", form.data(data_actionbefore));
            form.removeData(data_actionbefore);
        }, 0);
    });
    function validate(form) {
        var validationInfo = $(form).data(data_validation);
        return !validationInfo || !validationInfo.validate || validationInfo.validate();
    }
    var data_validation = "unobtrusiveValidation";

    $("form:not([data-ajax])").live("submit", function (evt) {
        if (!validate(this)) {
            evt.preventDefault();
            return;
        }
    });
    $(".ckall").click(function () {
        var table = $(this).parents("table")[0];

        $(".ckitem", table).attr("checked", $(this).attr("checked"));
    });

    $(".ui-click-hidden").click(function () {
        $(this).animate({
            opacity: 0.0,
            height: 0
        }, 300, function () { $(this).remove(); });
    });
    $("[data-smscode]").click(function (evt) {
        evt.preventDefault();
        var mobile = $($(this).attr("data-smscode")).val();
        if (!mobile) {
            alert("请先输入手机号码！");
            return;
        }
        var displayName = $($(this).attr("data-smsuser")).val();
        var $this = $(this);
        var html = $this.text();
        $this.text("校验中...");
        $.get(String.format("/Utility/SendSmsCode?number={0}&username={1}&tm={2}", mobile, displayName, new Date()), function (data) {
            alert("发送验证码成功！");
            $this.text(html);
        });
    });
});

/* Tooltip */

$(function () {
    $('.tooltip').betterTooltip({ speed: 250, delay: 300 });
});

/* Theme changer - set cookie */

$(function () {

    $("link[title='style']").attr("href", "css/styles/default/ui.css");
    $('a.set_theme').click(function () {
        var theme_name = $(this).attr("id");
        $("link[title='style']").attr("href", "css/styles/" + theme_name + "/ui.css");
        $.cookie('theme', theme_name);
        $('a.set_theme').css("fontWeight", "normal");
        $(this).css("fontWeight", "bold");
    });

    var theme = $.cookie('theme');

    if (theme == 'default') {
        $("link[title='style']").attr("href", "css/styles/default/ui.css");
    };

    if (theme == 'light_blue') {
        $("link[title='style']").attr("href", "css/styles/light_blue/ui.css");
    };


    /* Layout option - Change layout from fluid to fixed with set cookie */

    $("#fluid_layout a").click(function () {
        $("#fluid_layout").hide();
        $("#fixed_layout").show();
        $("#page-wrapper").removeClass('fixed');
        $.cookie('layout', 'fluid');
    });

    $("#fixed_layout a").click(function () {
        $("#fixed_layout").hide();
        $("#fluid_layout").show();
        $("#page-wrapper").addClass('fixed');
        $.cookie('layout', 'fixed');
    });

    var layout = $.cookie('layout');

    if (layout == 'fixed') {
        $("#fixed_layout").hide();
        $("#fluid_layout").show();
        $("#page-wrapper").addClass('fixed');
    };

    if (layout == 'fluid') {
        $("#fixed_layout").show();
        $("#fluid_layout").hide();
        $("#page-wrapper").addClass('fluid');
    };

    $("#main-content").css("min-height", (window.screen.height - 420) + "px");
});
 