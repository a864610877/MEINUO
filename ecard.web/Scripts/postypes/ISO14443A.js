var keyValue = "FFFFFFFFFFFFFFFF";
window.port = 0;
var readAccount = function () {
    var curPort = window.port || jQuery.cookie("iso14443_port");
    if (window.port == 0) {
        curPort = prompt("请输入您的COM号, 如需重新设置，请刷新页面。该提示每次进入页面后会出现。", curPort);
        curPort = parseInt(curPort);
        window.port = curPort;
        jQuery.cookie("iso14443_port", curPort);
    }
    var obj = document.getElementById('ISO14443A');
    // kh, qh, password, text
    var iRet = obj.ReadCard(curPort, "01", "04", "111");

    if (iRet != 0) {
        alert("ReadCard方法调用控件发生错误，错误码：" + iRet);
        return null;
    }
    var accountName = obj.GetTrackData();

    iRet = obj.ReadCard(curPort, "01", "05", "111");

    if (iRet != 0) {
        alert("ReadCard方法调用控件发生错误，错误码：" + iRet);
        return null;
    }
    var accountToken = obj.GetTrackData();
    return { accountName: accountName, accountToken: accountToken };
};
var writeAccount = function (account) {
    var curPort = window.port || jQuery.cookie("iso14443_port");
    if (window.port == 0) {
        curPort = prompt("请输入您的COM号, 如需重新设置，请刷新页面。该提示每次进入页面后会出现。", curPort);
        curPort = parseInt(curPort);
        window.port = curPort;
        jQuery.cookie("iso14443_port", curPort);
    }

    var obj = document.getElementById('ISO14443A');
    var iRet = obj.WriteCard(curPort, "01", "04", "111", account.accountName);
    if (iRet != 0) {
        alert("WriteCard方法调用控件发生错误，错误码：" + iRet);
        return false;
    }
    iRet = obj.WriteCard(curPort, "01", "05", "111", account.accountToken);

    if (iRet != 0) {
        alert("WriteCard方法调用控件发生错误，错误码：" + iRet);
        return false;
    }
    return true;
};
$(function () {
    if ($("#ISO14443A").length == 0)
        $("body").append($('<OBJECT id="ISO14443A" classid="clsid:895FFEFB-5AC2-42E6-97EF-B81C762A9859" codebase="/components/ISO14443A_setup.exe#version=1,0,0,0"  width="8" height="8">  </OBJECT> '));
});
(function ($) {
})(jQuery);
