using MicroMall.Models;
using MicroMall.Models.JuMeiMallIndex;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace MicroMall.Controllers
{
    /// <summary>
    /// zybkej@163.com
    ///brant450370
    ///商户号
    ///1298328701@1298328701
    ///840168
    /// </summary>


    public class WeChatPaymentController : BaseController
    {
        private readonly IUnityContainer _container;
        JsApiPay jsApiPay = new JsApiPay();
        public WeChatPaymentController(IUnityContainer _container)
        {
            this._container = _container;


        }
        // GET: /WeChatPayment/

        /// <summary>
        /// 获取code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult getCode()
        {
            object objResult = "";
            if (Session["url"] != null)
            {
                objResult = Session["url"].ToString();
            }
            else
            {
                objResult = "url为空。";
            }
            return Json(objResult);
        }

        /// <summary>
        /// 通过code换取网页授权access_token和openid的返回数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult getWxInfo(string code)
        {
            object objResult = "";
            string strCode = code;
            if (Session["access_token"] == null || Session["openid"] == null)
            {
                jsApiPay.GetOpenidAndAccessTokenFromCode(strCode);
            }
            string strAccess_Token = Session["access_token"].ToString();
            string strOpenid = Session["openid"].ToString();
            objResult = new { openid = strOpenid, access_token = strAccess_Token };
            return Json(objResult);
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeterRecharge(string totalfee)
        {
            object objResult = "";
            string strTotal_fee = totalfee;
            string strFee = (double.Parse(strTotal_fee) * 100).ToString();

            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            jsApiPay.openid = Session["openid"].ToString();
            jsApiPay.total_fee = int.Parse(strFee);

            //JSAPI支付预处理
            try
            {
                string strBody = "菲爱仕支付";//商品描述
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(strBody);
                WxPayData wxJsApiParam = jsApiPay.GetJsApiParameters2();//获取H5调起JS API参数，注意，这里引用了官方的demo的方法，由于原方法是返回string的，所以，要对原方法改为下面的代码，代码在下一段

                ModelForOrder aOrder = new ModelForOrder()
                {
                    appId = wxJsApiParam.GetValue("appId").ToString(),
                    nonceStr = wxJsApiParam.GetValue("nonceStr").ToString(),
                    packageValue = wxJsApiParam.GetValue("package").ToString(),
                    paySign = wxJsApiParam.GetValue("paySign").ToString(),
                    timeStamp = wxJsApiParam.GetValue("timeStamp").ToString(),
                    msg = "成功下单,正在接入微信支付."
                };
                objResult = aOrder;
            }
            catch (Exception ex)
            {

                ModelForOrder aOrder = new ModelForOrder()
                {
                    appId = "",
                    nonceStr = "",
                    packageValue = "",
                    paySign = "",
                    timeStamp = "",
                    msg = "下单失败,请重试,若多次失败,请联系管理员."
                };
                objResult = aOrder;
            }
            return Json(objResult);
        }







    }
    public class ModelForOrder
    {
        public string appId { get; set; }
        public string timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string packageValue { get; set; }
        public string paySign { get; set; }

        public string msg { get; set; }
    }
}
