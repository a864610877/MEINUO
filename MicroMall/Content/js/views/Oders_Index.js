$(function () {
    $(".productTitle").each(function () {
        if ($(this).height() == 16) {
            $(this).css("margin-top","-5.3rem");
        }
    })
    //状态栏点击事件
    $(".type ul li").click(function () {
        $(".type li").each(function () {
            var src = "/Content/img/" + $(this).attr("id") + "_1.png";
            $(this).find("img").attr("src",src);
            $(this).find("div").removeClass("select_btn_color");
        });
        var id = $(this).attr("id");
        $(this).find("img").attr("src", "/Content/img/" + id + "_2.png");
        $(this).find("div").addClass("select_btn_color");
    });
});