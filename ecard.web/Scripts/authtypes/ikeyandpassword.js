function hashToken(val) {
    try {var ikeyserver = document.getElementById('ikeyserver');
        return ikeyserver.HashToken(val, 0x2012, 0);
    } catch (e) { 
        return "";
    }   
}
function writeUserToken(token) {
    var ikeyserver = document.getElementById('ikeyserver');
    try {
        var d = ikeyserver.WriteKey(token, 0x2012, 0);
        if (d != 0)
            throw new "客户端写狗失败"; 
    } catch(e) {
        alert(e);
    }   
}
$(function () {
    if ($("#ikeyserver").length == 0)
        $("body").append($('<OBJECT id="ikeyserver" classid="clsid:5396FE4D-7E43-4EFD-80ED-D9E0430BDC7A" codebase="/components/ikeyweb_setup.exe#version=1,0,1,0"   width="8" height="8">  </OBJECT> '));
});