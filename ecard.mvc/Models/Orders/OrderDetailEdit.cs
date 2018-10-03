using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecard.Models;
using Microsoft.Practices.Unity;
using Ecard.Services;
using Ecard.Mvc.ViewModels;
using System.Web.Mvc;
using Ecard.Requests;
using WxPayAPI;

namespace Ecard.Mvc.Models.Orders
{
    public class OrderDetailEdit : EcardModelListRequest<ListOrderDetail>
    {

        /// <summary>
        /// 订单信息
        /// </summary>
        public Order order { get; set; }

        /// <summary>
        /// 配送状态
        /// </summary>
        private Bounded _distributionstate;

        public Bounded distributionstate
        {
            get
            {
                if (_distributionstate == null)
                    _distributionstate = Bounded.Create<Order>("distributionstate", order.distributionstate);
                return _distributionstate;
            }
            set { _distributionstate = value; }
        }
        /// <summary>
        /// 配送方式
        /// </summary>
        private Bounded _distributionType;

        public Bounded distributionType
        {
            get
            {
                if (_distributionType == null)
                    _distributionType = Bounded.Create<Order>("distributionType", Convert.ToInt32(order.distributionType));
                return _distributionType;
            }
            set { _distributionType = value; }
        }

        /// <summary>
        /// 支付状态
        /// </summary>
        private Bounded _payState;

        public Bounded payState
        {
            get
            {
                if (_payState == null)
                    _payState = Bounded.Create<Order>("payState", order.payState);
                return _payState;
            }
            set { _payState = value; }
        }
        /// <summary>
        /// 订单状态
        /// </summary>
        private Bounded _orderState;

        public Bounded orderState
        {
            get
            {
                if (_orderState == null)
                    _orderState = Bounded.Create<Order>("orderState", order.orderState);
                return _orderState;
            }
            set { _orderState = value; }
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        private Bounded _payType;

        public Bounded payType
        {
            get
            {
                if (_payType == null)
                    _payType = Bounded.Create<Order>("payType", order.payType);
                return _payType;
            }
            set { _payType = value; }
        }

        [Dependency]
        [NoRender]
        public IOrderService orderService { get; set; }
        [Dependency]
        [NoRender]
        public IAccountService AccountService { get; set; }
        [Dependency]
        [NoRender]
        public IOperationPointLogService OperationPointLogService { get; set; }
        [Dependency]
        [NoRender]
        public TransactionHelper TransactionHelper { get; set; }

        [Dependency]
        [NoRender]
        public IOrderDetailService orderDetailService { get; set; }
        [Dependency]
        [NoRender]
        public ICommodityService commodityService { get; set; }
        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListOrderDetail item)
        {
            yield return new ActionMethodDescriptor("Delete", null, new { id = item.orderDetailId });
        }

        public void Ready(int  id)
        {
            try
            {
                order = orderService.GetById(id);
                var request = new OrderDetailRequest();
                request.orderNo =order.orderNo;
                var query = orderDetailService.GetByOrderId(request);
                if (query != null)
                {
                    List = query.ModelList.Select(x => new ListOrderDetail(x)).ToList();
                    pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
                }
                else
                {
                    List = new List<ListOrderDetail>();
                    pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public List<ListOrderDetail> AjaxQuery(OrderDetailRequest request)
        {

            var data = new List<ListOrderDetail>();
            var query = orderDetailService.GetByOrderId(request);
            if (query != null)
            {
                data = query.ModelList.Select(x => new ListOrderDetail(x)).ToList();
                foreach (var item in data)
                {
                    item.boor += "<a href='#' onclick=OperatorThis('Delete','/Order/Delete/" + item.orderDetailId + "') class='tablelink'>删除 &nbsp;</a> ";
                }
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);

            }
            else
            {
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
            return data;
        }


        public ResultMsg Agree(int id)
        {
            var item = orderService.GetById(id);
            if(item!=null&&item.orderState==OrderStates.applyRefund)
            {
                var account = AccountService.GetByUserId(item.userId);
                if (account == null)
                    return new ResultMsg() { Code = -1, CodeText = "会员账户异常" };
                JsApiPay jsApiPay = new JsApiPay();
                jsApiPay.openid = account.openID;
                jsApiPay.total_fee = (int)(item.payAmount * 100);
                try
                {
                    //var orderNo = WxPayApi.GenerateOutTradeNo();
                    if (jsApiPay.total_fee > 0&& !string.IsNullOrWhiteSpace(item.orderNo))
                    {
                        WxPayData unifiedResult = jsApiPay.GetUnifiedTransferResult(item.orderNo, "", "订单取消");
                        if (!unifiedResult.GetValue("return_code").Equals("SUCCESS"))
                        {
                            WxPayAPI.Log.Error(this.GetType().ToString(), "UnifiedTransfer response error!");
                            return new ResultMsg() { Code = -1, CodeText = "订单取消失败" };
                        }
                        if (!unifiedResult.IsSet("mch_appid") || !unifiedResult.IsSet("mchid"))
                        {
                            WxPayAPI.Log.Error(this.GetType().ToString(), "UnifiedTransfer response error!");
                            return new ResultMsg() { Code = -1, CodeText = "订单取消失败" };
                        }
                    }
                    item.orderState = OrderStates.refund;
                    TransactionHelper.BeginTransaction();
                    if(item.point>0)
                    {
                        account.presentExp += item.presentExp;
                        account.activatePoint += item.activatePoint;
                        OperationPointLog log = new OperationPointLog();
                        log.account = "";
                        log.point = item.point;
                        log.remark = "订单取消";
                        log.submitTime = DateTime.Now;
                        log.userId = account.userId;
                        OperationPointLogService.Insert(log);
                        AccountService.Update(account);
                    }
                    orderService.Update(item);
                    TransactionHelper.Commit();
                    return new ResultMsg() { Code = 0 };
                }
                catch (Exception ex)
                {
                    WxPayAPI.Log.Error(this.GetType().ToString(), ex.Message.ToString());
                    return new ResultMsg() { Code = -1, CodeText = ex.Message.ToString() };
                }
            }
            return new ResultMsg() { Code = -1, CodeText = "订单不存在或者状态不正常" };
        }
    }
}
