using Aop.Api.Util;
using Com.Alipay;
using Ecard;
using Ecard.Models;
using Ecard.Services;
using MicroMall.Models;
using Moonlit.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MicroMall.NotifyUrl
{
    public partial class AlipayNotifyUrl : System.Web.UI.Page
    {
        private Database _database;
        protected void Page_Load(object sender, EventArgs e)
        {
               WxPayAPI.Log.Info(this.GetType().ToString(), "--异步回调处理开始--");
                Dictionary<string, string> data = GetRequestPost();
                string notify_id = Request.Form["notify_id"];//获取notify_id
                string sign = Request.Form["sign"];//获取sign
                string SerialNo = Request.Form["out_trade_no"];
                string TradeNo = Request.Form["trade_no"];
                string price = Request.Form["buyer_pay_amount"];
                string trade_status = Request.Form["trade_status"];
                string gmt_payment = Request.Form["notify_time"];
                WxPayAPI.Log.Info("Form:", AlipaySignature.GetSignContent(data));
                if (notify_id == null || notify_id == "")
                {
                    WxPayAPI.Log.Info("","参数错误");
                    return;
                }
                Notify aliNotify = new Notify();
                if (aliNotify.GetResponseTxt(notify_id) != "true")
                {
                    WxPayAPI.Log.Info("", "参数错误");
                    return;
                }
                WxPayAPI.Log.Info("订单号：", SerialNo);
                bool flag = aliNotify.GetSignVeryfy(data, sign); //AlipaySignature.RSACheckV1(sPara, AlipayConfig.ALIPAY_PUBLIC_KEY, AlipayConfig.CHARSET, "RSA", false);
               // bool checkSign = AlipaySignature.RSACheckV2(data, AlipayConfig.ALIPAY_PUBLIC_KEY);
                if (flag)
                {
                    WxPayAPI.Log.Info("", "验签成功");
                    _database = new Database("ecard");
                    using (var _databaseInstance = new DatabaseInstance(_database))
                    {
                        var sql = "select * from fz_Orders where orderNo=@orderNo";
                        var item = new QueryObject<Ecard.Models.Order>(_databaseInstance, sql, new { orderNo = SerialNo }).FirstOrDefault();
                        if (item != null)
                        {
                            if (item.payState == Ecard.Models.PayStates.non_payment && item.orderState == Ecard.Models.OrderStates.awaitPay)
                            {
                                WxPayAPI.Log.Info("payAmount", item.payAmount.ToString());
                                WxPayAPI.Log.Info("price", price);
                                decimal amount = 0;
                                decimal.TryParse(price, out amount);
                                WxPayAPI.Log.Info("amount", amount.ToString());
                                if (item.payAmount == amount)
                                {
                                    WxPayAPI.Log.Info("金额", "成功");
                                    _databaseInstance.BeginTransaction();
                                    item.payState = Ecard.Models.PayStates.paid;
                                    item.orderState = Ecard.Models.OrderStates.paid;
                                    item.payType = PayTypes.Alipay;
                                    item.submitTime = DateTime.Now;
                                    _databaseInstance.Update(item, "fz_Orders");



                                    var sql2 = "select * from fz_Accounts where accountId=@accountId";
                                    var item2 = new QueryObject<Ecard.Models.Account>(_databaseInstance, sql2, new { accountId = item.userId }).FirstOrDefault();
                                    if (item2 != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(item2.openID))
                                        {
                                            var message = new Fz_Messages();
                                            message.accountId = item2.accountId;
                                            message.openId = item2.openID;
                                            message.state = MessagesState.staySend;
                                            message.submitTime = DateTime.Now;
                                            message.keyword1 = item.orderNo;
                                            message.keyword2 = "已付款";
                                            message.msgType = MsgType.orderState;
                                            _databaseInstance.Insert(message, "Fz_Messages");
                                        }
                                    }

                                    _databaseInstance.Commit();
                                }
                            }
                        }

                    }
                    WxPayAPI.Log.Info("交易", "成功");
                    //交易成功
                    Response.Write("success");
                }
                else
                {
                    WxPayAPI.Log.Info("", "验签失败");
                }
                
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArraytemp = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArraytemp.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in sArraytemp)
            {
                sArray.Add(temp.Key, temp.Value);
            }
            return sArray;
        }

    }
}