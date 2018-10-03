function printTicket(text) {
    window.location = String.formatUrl("/utility/print?content={0}&printerPage=navPrinter&tm={1}", text.replace(/\n/ig, "汉字"), new Date());
} 