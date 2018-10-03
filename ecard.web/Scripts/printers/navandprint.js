function printTicket(text) {
    alert(text);
    $("title").text("@" + text.replace(/\n/ig, "#").replace(/\r/ig, ""));
} 