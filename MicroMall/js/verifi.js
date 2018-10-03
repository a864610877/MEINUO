//手机号验证
function checkMobile(mobile) {
    if (!(/^1[34578]\d{9}$/.test(mobile))) {
        return false;
    }
    return true;
}