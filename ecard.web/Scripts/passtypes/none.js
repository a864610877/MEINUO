var inputPassword = function (obj) {
    if ($("#dialog-password").length == 0) {
        $("body").append($('<div id="dialog-password" class="tipDiv"  style="height: 200px;"><div class="tipDivTop"><span>密码检验</span><a onclick=closeDiv(this)></a></div><div class="InputPwdStyle"><label>请输入密码<label>&nbsp;&nbsp;<input id="passwordUserInputed" type="password"  data-field="password" class="scinput"/>&nbsp;&nbsp;<input type="button" class="scbtn" value="确定" onclick="OperatorDo('+obj+')"></div></div>'))
//        $("#dialog-password #passwordUserInputed").bind("keydown", function (evt) {
//            if (evt.keyCode == "13") {
//                $("#dialog-password").fadeOut(100);
//                var complated = $("#dialog-password").data("complated");
//                var pwd = $(this).val();
//                $(this).val("");
//                complated(pwd);
//            }
//        });
    }
    var passwordDlg = $("#dialog-password");
//    passwordDlg.data("complated", completed || function () { });
//    passwordDlg.data("canceled", timeout || function () { });
//    passwordDlg.bindfrom({}); /// <reference path="none.js" />

    passwordDlg.fadeIn(100);
};
function OperatorDo(obj) {
    var pwd = $("#passwordUserInputed").val();
     var pwd2 = $("#passwordUserInputedConfirm").val();
    switch (obj) {
        case 1:
            doGetGift(pwd);
            break;
        case 2:
            publishCard(pwd,pwd2);
            break;
    } 
}



var inputPassword_new = function (completed, timeout) {
    if ($("#dialog-password-new").length == 0) {
        $("body").append($('<div id="dialog-password-new" class="tipDiv"  style="height: 200px;"><div class="tipDivTop"><span>密码检验</span><a onclick=closeDiv(this)></a></div><div class="InputPwdStyle"><label>请输入密码<label>&nbsp;&nbsp;<input id="passwordUserInputed" type="password"  data-field="password" class="scinput"/>&nbsp;&nbsp;<input type="button" id="check_NewPwd" class="scbtn" value="确定" ></div></div>'));

        $("#check_NewPwd").bind("click", function () {

            //                $("#dialog-password").dialog("close");
                fideOutDiv();
                var complated = $("#dialog-password-new").data("complated");
                var pwd = $("#passwordUserInputed").val();
                $("#passwordUserInputed").val("");
                complated(pwd); 
        });
    }
    var passwordDlg = $("#dialog-password-new");
    passwordDlg.data("complated", completed || function () { });
    passwordDlg.data("canceled", timeout || function () { });
    passwordDlg.bindfrom({});
    $(".ShowHide").fadeIn(100);
    passwordDlg.fadeIn(100);
};
var confirmPassword = function (completed, timeout) {
    if ($("#dialog-password-confirm").length == 0) {
        $("body").append($('<div id="dialog-password-confirm" class="tipDiv" style="height: 300px; "><div class="tipDivTop"><span>初始化密码</span><a onclick=closeDiv(this)></a></div><table class="InputPwdStyle"><tr><td><label>请输入密码</label></td><td><input id="passwordUserInputed" type="password" class="scinput" data-field="password"></td></tr><tr><td><label>请再次输入密码</label></td><td><input id="passwordUserInputedConfirm" type="password" class="scinput" data-field="password"></td></tr><tr><td></td><td><input id="checkPwd" type="button" class="scbtn" value="确定"></td></tr></table></div'));
 
        $("#checkPwd").bind("click", function () { 
                if ($("#dialog-password-confirm #passwordUserInputed").val() != $("#dialog-password-confirm #passwordUserInputedConfirm").val()) {
                    alert("两次输入的密码不一样！");
                    var canceled = $("#dialog-password-confirm").data("canceled");
                    canceled();
                    return;
                }
                var complated = $("#dialog-password-confirm").data("complated");
                var pwd1 = $("#passwordUserInputed").val();
                $("#passwordUserInputed").val("");
                var pwd2 = $("#passwordUserInputedConfirm").val();
                $("#passwordUserInputedConfirm").val("");
                complated(pwd1, pwd2); 
        });
  }
        var passwordDlg = $("#dialog-password-confirm");
        passwordDlg.data("complated", completed || function () { });
        passwordDlg.data("canceled", timeout || function () { });
        passwordDlg.bindfrom({});
    $(".ShowHide").fadeIn(100);
    passwordDlg.fadeIn(100);
};
function closeDiv(obj) {
    $(obj).parent().parent().fadeOut(100);
    $(".ShowHide").fadeOut(100);
}

 