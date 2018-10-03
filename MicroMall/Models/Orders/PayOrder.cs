using Ecard.Models;
using Ecard.Services;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Orders
{
    public class PayOrder : LayoutModel
    {

        [Dependency]
        public IOrderService OrderService { get; set; }
        [Dependency]
        public IOrderDetailService OrderDetailService { get; set; }
        [Dependency]
        public ICommodityService CommodityService { get; set; }
        [Dependency]
        public IOperationPointLogService OperationPointLogService { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 支付积分
        /// </summary>
        public decimal payPoint { get; set; }

        public int provinceId { get; set; }
        public int cityId { get; set; }
        
        public string detailedAddress { get; set; }

        public string recipients { get; set; }

        public string moblie { get; set; }

        public int addessId { get; set; }

        public int payType { get; set; }

        public ResultMessage Save()
        {
            TransactionHelper.BeginTransaction();
            Load();
            if (UserInformation == null)
                return new ResultMessage() { Code = -1, Msg = "请重新登录" };
            if(Account==null)
                return new ResultMessage() { Code = -1, Msg = "账户异常" };
            if (provinceId == 0 || cityId == 0 || string.IsNullOrWhiteSpace(detailedAddress) || string.IsNullOrWhiteSpace(recipients) || string.IsNullOrWhiteSpace(moblie))
                return new ResultMessage() { Code = -1, Msg = "请完善收货地址，收件人信息" };
            var item = OrderService.GetOrderNo(orderNo);
            if(item==null)
                return new ResultMessage() { Code = -1, Msg = "订单异常" };
            if (item.orderState != OrderStates.awaitPay)
                return new ResultMessage() { Code = -1, Msg = "订单已完成付款" };
            if(payType!=PayTypes.cashOnDelivery&&payType!=PayTypes.weChatPayment)
                return new ResultMessage() { Code = -1, Msg = "支付方式异常" };
            var point=Account.presentExp + Account.activatePoint;
            if (point < payPoint)
                return new ResultMessage() { Code = -2, Msg = point.ToString() };//积分不足
            decimal payAmount = item.amount;//需要付款金额
            decimal dkje =(Account.activatePoint+Account.presentExp) * PointRatio;//打算抵扣的金额
            decimal dk = 0;//抵扣了多少金额
            if (dkje >= item.amount)
            {
                dk = item.amount;
            }
            else
            {
                dk = dkje;
            }
            payAmount -= dk;
            item.payAmount = payAmount;
            decimal presentExp = 0;
            decimal activatePoint = 0;
            decimal dkPoint=0;
            if(dk>0)
                dkPoint = dk / PointRatio;
            item.point = dkPoint;
            if (item.payAmount == 0 || payType == PayTypes.cashOnDelivery)
            {
                if (dkPoint > 0)
                {
                    if (Account.presentExp >= dkPoint)
                    {
                        presentExp = dkPoint;
                        Account.presentExp -= dkPoint;
                    }
                    else
                    {
                      decimal cc = dkPoint - Account.presentExp;
                      Account.activatePoint -= cc;
                      activatePoint = cc;
                      presentExp = Account.presentExp;
                      Account.presentExp = 0;
                    }
                    OperationPointLog pointlog = new OperationPointLog();
                    pointlog.account ="";
                    pointlog.point =-dkPoint;
                    pointlog.remark = string.Format("消费抵扣");
                    pointlog.submitTime = DateTime.Now;
                    pointlog.userId =UserInformation.UserId;
                    OperationPointLogService.Insert(pointlog);
                    AccountService.Update(Account);
                }
                item.payState = PayStates.paid;
                item.orderState = OrderStates.paid;
                item.activatePoint = activatePoint;
                item.presentExp = presentExp;
                item.distributionstate = DistributionStates.unfilled;
            }
            item.UserAddressId = addessId;
            item.provinceId = provinceId;
            item.cityId = cityId;
            item.detailedAddress = detailedAddress;
            item.recipients = recipients;
            item.moblie = moblie;
            item.payType = payType;
            OrderService.Update(item);
            TransactionHelper.Commit();
            return new ResultMessage() { Code = 0, Msg = item.orderNo };   
           
        }

        public void PayDiscount(Account account,decimal point,decimal amount)
        {
            var zsje = account.presentExp * PointRatio;//赠送金额
            var jhje = account.activatePoint * PointRatio;//激活金额
            var dkje = point * PointRatio;//打算抵扣的金额
            decimal dk=0;//抵扣了多少
            if (dkje >= amount)
            {
                dk = amount;
            }
            else
            {
                dk = dkje;
            }
          
        }
    }
}