function printTicket(text) {
    alert(text);
    var w = window.open(String.formatUrl("/utility/print?content={0}&printerPage=printer&tm={1}", text, new Date()), "_blank");
}