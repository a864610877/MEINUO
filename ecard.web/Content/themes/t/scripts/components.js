var printTicket2 = function (text) {
    document.getElementById("printer2").printTicket(text.replace(/\n\n/g, "\n'\n"));
};
$(function() { 
    $("body").append('<div id="flashContent" name="flashContent"></div>');

    swfobject.embedSWF(
        "/components/TicketPrinter.swf", "flashContent",
        "1px", "1px",
        "11.1.0", "playerProductInstall.swf",
        { }, { }, { id: "printer2", name: "printer2"  });
});