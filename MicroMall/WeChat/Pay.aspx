<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pay.aspx.cs" Inherits="MicroMall.WeChat.Pay" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<meta name="HandheldFriendly" content="True">
<meta name="MobileOptimized" content="320">
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0, user-scalable=no">
<head>
    <title>支付通知</title>
    <script src="/assets/js/jquery.min.js"></script>
    <script src="/assets/js/amazeui.js"></script>
    <link href="/assets/css/amazeui.css" rel="stylesheet" />
    <link href="/css/StyleSheet.css" rel="stylesheet"/><!--新建样式表 -->
    <script src="/js/Method.js"></script>
    <script src="/js/Shopping.js"></script>
</head>
<body>

<header data-am-widget="header" class="am-header am-header-default am-header-fixed ">
    <div class="am-header-left am-header-nav">
        <a href="javascript:history.go(-1)" class="am-icon-arrow-left">
        </a>
    </div>
    <h1 class="am-header-title">
        支付通知
    </h1>
</header>

<section class="warp_content" runat="server" id="tz">
    <div class="zhifu_wrap"    >
        <img   src="images/backpay_yes.png" />
        <!--<img src="images/backpay_no.png" />-->
        <h3>支付成功<!--支付失败--></h3>
        <p >支付金额： ¥888.00 <!--微信余额不足--></p>
        <button   type="button" class="am-btn am-btn-success am-btn-block am-radius">确认返回</button>
    </div>
</section>

</body>
</html>
<%--<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html;charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/> 
    <title>微信支付样例-JSAPI支付</title>
</head>

           <script type="text/javascript">

               //调用微信JS api 支付
               function jsApiCall()
               {
                   WeixinJSBridge.invoke(
                   'getBrandWCPayRequest',
                   <%=wxJsApiParam%>,//josn串
                    function (res)
                    {
                        WeixinJSBridge.log(res.err_msg);
                        alert(res.err_code + res.err_desc + res.err_msg);
                    }
                    );
               }

               function callpay()
               {
                   if (typeof WeixinJSBridge == "undefined")
                   {
                       if (document.addEventListener)
                       {
                           document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                       }
                       else if (document.attachEvent)
                       {
                           document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                           document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                       }
                   }
                   else
                   {
                       jsApiCall();
                   }
               }
               
     </script>

<body>
    <form runat="server">
        <div>
            <%=wxJsApiParam%>
        </div>
        <br/>
	    <div align="center">
		    <br/><br/><br/>
            <asp:Button ID="submit" runat="server" Text="立即支付" OnClientClick="callpay()" style="width:210px; height:50px; border-radius: 15px;background-color:#00CD00; border:0px #FE6714 solid; cursor: pointer;  color:white;  font-size:16px;" />
	    </div>
    </form>
</body>
</html>--%>