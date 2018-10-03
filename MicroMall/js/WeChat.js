//wx.config({
//    debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
//    appId: '', // 必填，公众号的唯一标识
//    timestamp:'' , // 必填，生成签名的时间戳
//    nonceStr: '', // 必填，生成签名的随机串
//    signature: '',// 必填，签名，见附录1
//    jsApiList: [] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
//});
var dataForWeixin = {
    appId: "",
    timestamp: "",
    nonceStr: "",
    signature: "",
    ImageUrl: "",
};
var httpurl = location.href.split('#')[0];
$.ajax({
    url: "/Weixin/WebConfing",
    data: { url: httpurl },
    type: "post",
    dataType: "json",
    async: false,
    success: function (data) {
        if (data != null) {
            alert("加载成功");
            dataForWeixin.appId = data.appId;
            dataForWeixin.nonceStr = data.nonceStr;
            dataForWeixin.signature = data.signature;
            dataForWeixin.timestamp = data.timestamp;
            dataForWeixin.ImageUrl = data.ImageUrl;
        }
    },
    error: function () {
        alert("网络错误");
    }
});

wx.config({
    debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
    appId: dataForWeixin.appId, // 必填，公众号的唯一标识
    timestamp: dataForWeixin.timestamp, // 必填，生成签名的时间戳
    nonceStr: dataForWeixin.nonceStr, // 必填，生成签名的随机串
    signature: dataForWeixin.signature,// 必填，签名，见附录1
    jsApiList: ['onMenuShareTimeline'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
});

