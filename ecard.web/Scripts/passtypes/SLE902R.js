// delete for publish source code
var inputPassword = function (completed, timeout) {
    var passwordToken;
    var obj = document.getElementById('keybd');

    var passwordDlg = $("#dialog-password-kb");
    passwordDlg.data("complated", completed);
    passwordDlg.bindfrom({});
    passwordDlg.dialog({
        title: "请输入密码",
        resizable: false,
        close: false,
        autoOpen: true,
        width: 400,
        modal: true
    });

    var callback2 = function () {
        var iRet = obj.Read(passwordToken.RsaModulus, passwordToken.RsaExponent, passwordToken.ChallengeData);
        if (iRet == 1) {
            window.setTimeout(callback2, 200);
            return;
        }
        if (iRet == 0) {
            passwordDlg.dialog("close");
            completed(obj.GetTrackData());
            return;
        }
        alert("读取密码错误(" + iRet + ")!");
    };
    var callback = function () {

        var r = obj.open(0, 9600, 1);
        if (r != 0) {
            alert("打开串口失败(" + r + ")！");
            return;
        }
        window.setTimeout(callback2, 200);
    };

    $("#dialog-password-kb #pwd_cfm_text").text("获取密钥");
    $.get("/utility/GetPasswordToken?tm=" + new Date(), function (d) {
        if (d.Success) {
            passwordToken = d;
            $("#dialog-password-kb #pwd_cfm_text").text("等待客户输入密码");
            window.setTimeout(callback, 0);
        } else {
            passwordDlg.dialog("close");
            (timeout || function () { })();
            alert(d.Message);
        }
    });
};
var confirmPassword = function (completed, timeout) {
    var passwordToken;
    var passwordDlg = $("#dialog-password-kb");

    passwordDlg.data("complated", completed);
    passwordDlg.bindfrom({});
    passwordDlg.dialog({
        title: "等待输入密码",
        resizable: false,
        close: false,
        autoOpen: true,
        width: 400,
        modal: true
    });
    var callback = function () {
        var obj = document.getElementById('keybd');

        var r = obj.open(0, 9600, 1);
        if (r != 0) {
            alert("打开串口失败(" + r + ")！");
            return;
        }

        var password1;
        var password2;

        var callback2 = function () {
            var iRet = obj.Read(passwordToken.RsaModulus, passwordToken.RsaExponent, passwordToken.ChallengeData);
            if (iRet == 1) {
                window.setTimeout(callback2, 200);
                return;
            }
            if (iRet == 0) {
                passwordDlg.dialog("close");
                password2 = obj.GetTrackData();

                (completed || function () { })(password1, password2);
                return;
            }
            alert("读取密码错误(" + iRet + ")!");
            passwordDlg.dialog("close");
            (timeout || function () { })();
        };
        var callback1 = function () {
            var iRet = obj.Read(passwordToken.RsaModulus, passwordToken.RsaExponent, passwordToken.ChallengeData);
            if (iRet == 1) {
                window.setTimeout(callback1, 200);
                return;
            }
            if (iRet == 0) {
                password1 = obj.GetTrackData();

                var r = obj.open(0, 9600, 2);
                if (r != 0) {
                    alert("打开串口失败(" + r + ")！");
                }
                else {
                    window.setTimeout(callback2, 200);
                    return;
                }
            }
            alert("读取密码错误(" + iRet + ")!");
            passwordDlg.dialog("close");
            (timeout || function () { })();
        };
        window.setTimeout(callback1, 0);
    };

    $("#dialog-password-kb #pwd_cfm_text").text("获取密钥");
    $.get("/utility/GetPasswordToken?tm=" + new Date(), function (d) {
        if (d.Success) {
            passwordToken = d;
            $("#dialog-password-kb #pwd_cfm_text").text("等待客户输入密码");
            window.setTimeout(callback, 0);
        } else {
            passwordDlg.dialog("close");
            (timeout || function () { })();
            alert(d.Message);
        }
    });
};
$(function () {
    if ($("#dialog-password-kb").length == 0)
        $("body").append($('<div id="dialog-password-kb" class="hastable" style="display: none"><p><h3 id="pwd_cfm_text">等待客户输入密码！</h3></p> </div'));

    if ($("#keybd").length == 0) {
        $("body").append($('<OBJECT id="keybd" classid="clsid:7269EB0E-744E-4BA5-9EAA-0CC327B69BAD" codebase="/components/sle902r_setup.exe#version=1,0,2,0" width="8" height="8">  </OBJECT> '));
    }
    //        $("body").append($('<OBJECT id="keybd" classid="clsid:50F38DF2-914E-463F-86AB-F436D93766B7" codebase="/components/sle902r_setup.exe#version=1,0,0,1"  width="8" height="8">  </OBJECT> '));
});
(function ($) {
})(jQuery);
