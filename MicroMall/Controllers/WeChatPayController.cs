using Ecard.Models;
using Ecard.Mvc;
using Ecard.Services;
using MicroMall.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace MicroMall.Controllers
{
    public class WeChatPayController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly SecurityHelper _securityHelper;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        public User _user { get; set; }

        public Account _account { get; set; }


        public WeChatPayController(IUnityContainer container, SecurityHelper securityHelper, IAccountService accountService, IOrderService orderService)
        {
            _securityHelper = securityHelper;
            _accountService=accountService;
            _container = container;
            _orderService = orderService;
          var user = _securityHelper.GetCurrentUser();
          if (user != null)
          {
              _user = user;
              var account = _accountService.GetByUserId(_user.UserId);
              if (account != null)
                  _account = account;
          }
             
        }
        [HttpPost]
        public ActionResult PlaceOrder(string orderNo)
        {
            if (_account == null)
                return Json(new ResultMessage() { Code = -1, Msg = "您还没有登录" });
            var order = _orderService.GetOrderNo(orderNo);
            if(order==null)
                return Json(new ResultMessage() { Code = -1, Msg = "订单不存在" });
            if(order.payState!=PayStates.non_payment)
                return Json(new ResultMessage() { Code = -1, Msg = "订单状态不正确" });
            JsApiPay jsApiPay = new JsApiPay();
            jsApiPay.openid = _account.openID;
            jsApiPay.total_fee = (int)(order.payAmount * 100);
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(order.orderNo);
                string wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                WxPayAPI.Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                return Json(new ResultMessage() { Code = 0, Msg = wxJsApiParam });
                //在页面上显示订单信息
                //Response.Write("<span style='color:#00CD00;font-size:20px'>订单详情：</span><br/>");
                //Response.Write("<span style='color:#00CD00;font-size:20px'>" + unifiedOrderResult.ToPrintStr() + "</span>");

            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Error(this.GetType().ToString(), ex.Message.ToString());
                return Json(new ResultMessage() { Code = -1, Msg = ex.Message.ToString() });
                //Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");
                //submit.Visible = false;
            }
        }
    }
}