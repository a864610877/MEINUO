var readAccount = function () {
    var yle300 = document.getElementById('yle300');
    var iRet = yle300.ReadCard(0, 2);

    if (iRet != 0) {
        alert("ReadCard方法调用控件发生错误，错误码：" + iRet);
        return null;
    }
    var s = yle300.GetTrackData();
    return { accountName: s.substring(0, 16), accountToken: s.substring(16, 24) };
};
var writeAccount = function (account) {
    var yle300 = document.getElementById('yle300');
    var iRet = yle300.WriteCard(0, 2, account.accountName + account.accountToken);

    if (iRet != 0) {
        alert("WriteCard方法调用控件发生错误，错误码：" + iRet);
        return false;
    }
    return true;
};
$(function () {
    if ($("#yle300").length == 0)
    //        yle300 = new ActiveXObject("clsid:BEE33C37-60FA-4DC9-B228-895F27CA37B7");
    //yle300 = new ActiveXObject("CYLE300_Ocx.CYLE300_OcxClass");
        $("body").append($('<OBJECT id="yle300" classid="clsid:BEE33C37-60FA-4DC9-B228-895F27CA37B7" codebase="/components/YLE300_Ocx_Setup.EXE" width="8" height="8"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="211"><PARAM NAME="_ExtentY" VALUE="211"><PARAM NAME="_StockProps" VALUE="0"></OBJECT>'));
});
(function ($) {
})(jQuery);
