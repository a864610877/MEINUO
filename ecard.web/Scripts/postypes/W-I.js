var keyValue = "FFFFFFFFFFFFFFFF";
window.port = 0;
var st;
var lSnr;
var readAccount = function () {
    var count = new Date().getTime();
    st = T8U.dc_init(100, 115200);
    if (st <= 0) {
        alert("dc_init error!");
        return;
    }
    T8U.dc_beep(10);
    st = T8U.dc_card(0, lSnr);
    while (st != 0) {
        var d = new Date().getTime();
        if (count + 10000 < d)
        { break; }
        st = T8U.dc_card(0, lSnr);

    }

    st = T8U.dc_authentication(4, 1);
    if (st != 0)
    { T8U.dc_exit(); alert("读卡异常"); return; }
    st = T8U.dc_read(4);
    if (st != 0) {
        alert("读卡失败");
        T8U.dc_exit();
        return;
    }
    T8U.dc_beep(10);
    var accountName = T8U.get_bstrRBuffer; //T8U.get_bstrRBuffer_asc;
    //accountName = accountName.replace("/0*(\d+)/", "$1");
    T8U.put_bstrSBuffer = "OK\0";
    T8U.dc_disp_str();
    st = T8U.dc_read(5);
    if (st != 0) {
        T8U.dc_exit();
        alert("读卡失败");
        return;
    }
    T8U.dc_beep(10);
    var accountToken = T8U.get_bstrRBuffer; //T8U.get_bstrRBuffer_asc;
    accountToken = accountToken.substring(0, 8);
    T8U.dc_exit();
    return { accountName: accountName, accountToken: accountToken };
};
var writeAccount = function (account) {
    var count = new Date().getTime();
//    if (account.accountToken.length < 16) {
//        for (var i = 0; i < 16; i++) {
//            account.accountToken =  account.accountToken+"0";
//            if (account.accountToken.length == 16)
//                break;
//        }
//    }
//    if (account.accountName.length < 16) {
//        for (var i = 0; i < 16; i++) {
//            account.accountName = "d"+account.accountName;
//            if (account.accountName.length == 16)
//                break;
//        }
//   }
    st = T8U.dc_init(100, 115200); //usb
    if (st <= 0) {
        alert("dc_init error!");
        return false;
    }
    T8U.dc_beep(10);
    st = T8U.dc_card(0, lSnr);
    while (st != 0) {
        var d = new Date().getTime();
        if (count + 10000 < d)
        { break; }
        st = T8U.dc_card(0, lSnr);

    }
    st = T8U.dc_authentication(4, 1);
    if (st != 0) {
        T8U.dc_exit();
        alert("写卡异常"); return;
    }

    T8U.put_bstrSBuffer = account.accountName;
    st = T8U.dc_write(4);
    if (st != 0) { T8U.dc_exit(); alert("写卡异常"); return false; }
    T8U.dc_beep(10);
    T8U.put_bstrSBuffer = account.accountToken;
    st = T8U.dc_write(5);
    if (st != 0) { T8U.dc_exit(); alert("写卡异常"); return false; }
    T8U.dc_beep(10);
    T8U.put_bstrSBuffer = "OK\0";
    T8U.dc_disp_str();
    T8U.dc_exit();
    return true;
};
$(function () {
    if ($("#T8U").length == 0)
        $("body").append($('<OBJECT id="T8U" classid="clsid:638B238E-EB84-4933-B3C8-854B86140668" codebase="components/comRD800.dll" >  </OBJECT> '));
});