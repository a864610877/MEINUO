using Ecard;
using Ecard.Services;
using Moonlit.Data;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;

namespace MicroMall.WeChat
{
    public partial class Pay : System.Web.UI.Page
    {
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info(this.GetType().ToString(), "page load");
            if (!IsPostBack)
            {
                var database = new Database("ecard");
                DatabaseInstance _databaseInstance = new DatabaseInstance(database);
                string orderNo = Request.QueryString["orderNo"];

                if (orderNo == "SUCCESS")
                {
                    tz.InnerHtml = SuccessHtml();
                    return;
                }
                //检测是否给当前页面传递了相关参数
                if (string.IsNullOrEmpty(orderNo))
                {
                    tz.InnerHtml = ErrorHtml("订单号不存在！");
                    //Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                    Log.Error(this.GetType().ToString(), string.Format("未找到订单号{0}", orderNo));
                    //submit.Visible = false;
                    return;
                }
                var sql = "select * from fz_Orders where orderNo=@orderNo";
                var item = new QueryObject<Ecard.Models.Order>(_databaseInstance, sql, new { orderNo = orderNo }).FirstOrDefault();
                if (item == null)
                {
                    tz.InnerHtml = ErrorHtml("订单号不存在！");
                    //Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                    Log.Error(this.GetType().ToString(), string.Format("未找到订单号{0}", orderNo));
                    //submit.Visible = false;
                    return;
                }
                if (item.payState == Ecard.Models.PayStates.non_payment)
                {
                    tz.InnerHtml = PayView(item.orderNo, item.payAmount.ToString());
                    Log.Debug(this.GetType().ToString(), string.Format("订单正常{0}", orderNo));
                    return;
                }
                else
                {
                    tz.InnerHtml = SuccessHtml();
                    return;
                }

                //string openid = "oOAh6wRzOp4lcE6GHlKicsmUIlzs"; //Request.QueryString["openid"];
                //string total_fee = "1"; //Request.QueryString["total_fee"];
                ////检测是否给当前页面传递了相关参数
                ////if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(total_fee))
                ////{
                ////    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                ////    Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
                ////    submit.Visible = false;
                ////    return;
                ////}

                ////若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                //JsApiPay jsApiPay = new JsApiPay(this);
                //jsApiPay.openid = openid;
                //jsApiPay.total_fee = int.Parse(total_fee);

                ////JSAPI支付预处理
                //try
                //{
                //    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                //    wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                //    Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                //    //在页面上显示订单信息
                //    Response.Write("<span style='color:#00CD00;font-size:20px'>订单详情：</span><br/>");
                //    Response.Write("<span style='color:#00CD00;font-size:20px'>" + unifiedOrderResult.ToPrintStr() + "</span>");

                //}
                //catch (Exception ex)
                //{
                //    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");
                //    //submit.Visible = false;
                //}
            }
        }


        public string ErrorHtml(string msg)
        {
            string html = "<div class=\"zhifu_wrap\"> <img src=\"/images/backpay_no.png\" />";
            html += " <h3>"+msg+"！</h3>";
            html += "<button type=\"button\" onclick=\"window.location.href = '/home/index'\" class=\"am-btn am-btn-success am-btn-block am-radius\">返回首页</button></div>";
            return html;
        }

        public string SuccessHtml()
        {
            string html = "<div class=\"zhifu_wrap\"> <img src=\"/images/backpay_yes.png\" />";
            html += " <h3>支付成功！</h3>";
            html += "<button type=\"button\" onclick=\"window.location.href = '/home/index'\" class=\"am-btn am-btn-success am-btn-block am-radius\">返回首页</button></div>";
            return html;
        }

        public string PayView(string orderNo,string amount)
        {
            string html = " <input type=\"hidden\" data-type=\"orderNo\" value=\""+orderNo+"\" />";
            html += " <ul class=\"querenzhifu\">";
            html += " <li>订单编号："+orderNo+"</li>";
            html += "<li>支付金额：<span>¥"+amount+"</span></li>";
            html += " <li><button type=\"button\" id=\"awayPay\" class=\"am-btn am-btn-warning am-btn-block am-radius\">立刻支付</button></li>";
            html += "</ul>";
            return html;
        }
    }
}