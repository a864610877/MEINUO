using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Ecard;
using Ecard.Models;
using MicroMall.Models;
using Moonlit.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MicroMall.NotifyUrl
{
    public partial class Alipay : System.Web.UI.Page
    {
        private Database _database;
        private DatabaseInstance _databaseInstance;
        protected void Page_Load(object sender, EventArgs e)
        {
            var orderNo = Request.QueryString["orderNo"];
            var userAddressId = Request.QueryString["userAddressId"];
            if (string.IsNullOrWhiteSpace(orderNo))
            {
                Response.Write("请求错误");
                Response.End();
                return;
            }
            if (string.IsNullOrWhiteSpace(userAddressId))
            {
                Response.Write("请选择收货地址");
                Response.End();
                return;
            }
            _database = new Database("ecard");
            using (_databaseInstance = new DatabaseInstance(_database))
            {
                var address = _databaseInstance.Query<Order>("select * from fz_UserAddress where userAddressId=@userAddressId", new { userAddressId = userAddressId }).FirstOrDefault();
                if (address == null)
                {
                    Response.Write("收货地址不存在，请重新付款");
                    Response.End();
                    return;
                }
                    
                var order = _databaseInstance.Query<Order>("select * from fz_Orders where orderNo=@orderNo", new { orderNo = orderNo }).FirstOrDefault();
                if (order == null)
                {
                    Response.Write("订单不存在");
                    Response.End();
                    return;
                }
                if (order.orderState != OrderStates.awaitPay)
                {
                    Response.Write("订单不是待付款状态");
                    Response.End();
                    return;
                }
                order.payAmount = order.amount + order.freight;
                order.detailedAddress = address.detailedAddress;
                order.moblie = address.moblie;
                order.recipients = address.recipients;
                order.UserAddressId = address.UserAddressId;
                if (_databaseInstance.Update(order, "fz_Orders")<=0)
                {
                    Response.Write("付款失败");
                    Response.End();
                    return;
                }
                IAopClient client = new DefaultAopClient(AlipayConfig.URL, AlipayConfig.APPID, AlipayConfig.APP_PRIVATE_KEY, "json", "1.0", "RSA", AlipayConfig.ALIPAY_PUBLIC_KEY, AlipayConfig.CHARSET, false);
                AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
                request.SetReturnUrl("http://wx.fislive.com/WeChatViews/i.html");
                request.SetNotifyUrl("http://wx.fislive.com/NotifyUrl/AlipayNotifyUrl.aspx");
                request.BizContent = "{" +
                "    \"body\":\"菲爱仕商城\"," +
                "    \"subject\":\"菲爱仕商城\"," +
                "    \"out_trade_no\":\"" + orderNo + "\"," +
                "    \"timeout_express\":\"90m\"," +
                "    \"total_amount\":" + order.payAmount + "," +
                "    \"product_code\":\"QUICK_WAP_WAY\"" +
                "  }";
                AlipayTradeWapPayResponse response = client.pageExecute(request);
                string form = response.Body;
                WxPayAPI.Log.Info("支付宝请求参数：", form);
                Response.Write(form);
                

            }
            //IAopClient client = new DefaultAopClient(AlipayConfig.URL, AlipayConfig.APPID, AlipayConfig.APP_PRIVATE_KEY, "json", "1.0", "RSA", AlipayConfig.ALIPAY_PUBLIC_KEY, AlipayConfig.CHARSET, false);
            //////实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            //AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            ////SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            //AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            ////model.m
            //model.Body = "我是测试数据";
            //model.Subject = "App支付测试DoNet";
            //model.TotalAmount = "0.01";
            //model.ProductCode = "QUICK_WAP_PAY";
            //model.OutTradeNo = "sadfa" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //// model.TimeoutExpress = "30m";        
            //request.SetBizModel(model);
            //request.SetNotifyUrl(AlipayConfig.NOTIFY_URL);
            ////这里和普通的接口调用不同，使用的是sdkExecute
            //string from = client.pageExecute(request).Body;
            ////AlipayTradeAppPayResponse response = client.SdkExecute(request);
            //Response.AddHeader("content-type", "text/html");
            //Response.AddHeader("charset","UTF-8");
            //Response.Write(from);
            
           
        }
    }
}