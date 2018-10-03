using Ecard.Models;
using Ecard.Mvc;
using Ecard.Requests;
using Ecard.Services;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Orders
{
    public class OrderSubmitm : LayoutModel
    {
        public Order item { get; set; }
        /// <summary>
        /// 抵扣金额
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 应支付金额
        /// </summary>
        public decimal payAmount { get; set; }
      

        public List<OrderDetail> orderDetails { get; set; }

        public List<SelectModel> ListUserAddress { get; set; }
        public List<SelectModel> ListProvince { get; set; }

        public List<SelectModel> ListCity { get; set; }

        public List<CommodityAttribute> Commoditys{ get; set; }

        [Dependency]
        public IOrderService OrderService { get; set; }
        [Dependency]
        public IOrderDetailService OrderDetailService { get; set; }
        [Dependency]
        public ICommodityService CommodityService { get; set; }
         [Dependency]
        public IUserAddressService UserAddressService { get; set; }
         [Dependency]
         public IShoppingCartService ShoppingCartService { get; set; }
        [Dependency]
         public IOperationPointLogService OperationPointLogService { get; set; }
        

        public ResultMessage Save()
        {
            var user = _securityHelper.GetCurrentUser();
            if(user!=null)
            {
                decimal dk=1;
                if (Commoditys != null && Commoditys.Count > 0)
                {
                    Order model = new Order();
                    model.orderNo = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now)+user.CurrentUser.UserId;
                    model.orderState = OrderStates.awaitPay;
                    model.payState = PayStates.non_payment;
                    model.submitTime = DateTime.Now;
                    model.userId = user.CurrentUser.UserId;
                    model.distributionstate = DistributionStates.unfilled;
                    decimal amount = 0;
                    //model.orderId = OrderService.Insert(model);
                    TransactionHelper.BeginTransaction();
                    foreach(var item in Commoditys)
                    {
                        var Commodity = CommodityService.GetById(item.commodityId);
                        if(Commodity!=null)
                        {
                            if (Commodity.commodityInventory < item.Num)
                                return new ResultMessage() { Code = -1, Msg = string.Format("{0}库存不足", item.commodityName) };
                            var orderDetail = new OrderDetail();
                            orderDetail.commodityId = Commodity.commodityId;
                            orderDetail.commodityName = Commodity.commodityName;
                            orderDetail.orderNo = model.orderNo;
                            orderDetail.price = Commodity.commodityPrice;
                            orderDetail.quantity = item.Num;
                            orderDetail.specification = item.Attribute;
                            amount += orderDetail.price * orderDetail.quantity;
                            OrderDetailService.Insert(orderDetail);
                        }
                        if(item.shoppingCartId>0)
                        {
                            var shoppingCart = ShoppingCartService.GetById(item.shoppingCartId);
                            ShoppingCartService.Delete(shoppingCart);
                        }
                    }
                    model.amount=amount;
                    model.payAmount =0;
                    OrderService.Insert(model);
                    TransactionHelper.Commit();
                    return new ResultMessage() { Code = 0, Msg = model.orderNo };
                }
                else
                {
                    return new ResultMessage() { Code = -1, Msg = "请选择商品！" };
                }
            }
            else
            {
                return new ResultMessage() { Code = -1, Msg = "您还没有登录！" };
            }
        }

        public void Ready(string orderNo)
        {
            Load();
            var user =UserInformation; //_securityHelper.GetCurrentUser();
            if (user != null)
            {
                var order = OrderService.GetByOrderNo(orderNo);
                if (orderNo != null)
                {
                    item = order.item;
                    orderDetails = order.OrderDetails;
                    if(orderDetails!=null)
                    {
                        foreach(var od in orderDetails)
                        {
                            quantity += od.quantity;
                            //amount += od.price * od.quantity;
                        }
                    }
                }
                var request=new UserAddressRequest();
                request.UserId=user.UserId;
                request.PageSize=20;
                var Address = UserAddressService.GetByUserId(request);
                if(Address!=null&&Address.TotalCount>0)
                {
                    ListUserAddress = Address.ModelList.Select(x => new SelectModel() { Id = x.userAddressId, Name = GetAddress(x) }).ToList();
                }
                 if(item.provinceId>0)
                     ListCity = GetCity(item.provinceId).Select(x => new SelectModel() {Id=x.CityId,Name=x.Name }).ToList();
                 Discount = Math.Round(CountDiscount(Account.presentExp + Account.activatePoint, order.item.amount), 2);
                 payAmount = order.item.amount - Discount;
            }
            ListProvince = GetProvinceAll().Select(x => new SelectModel() {Id=x.ProvinceId,Name=x.Name }).ToList();
           

           
        }

        public decimal  CountDiscount(decimal userPoint,decimal amount)
        {

            decimal payAmount = amount;//需要付款金额
            decimal dkje = userPoint * PointRatio;//打算抵扣的金额
            decimal dk = 0;//抵扣了多少金额
            if (dkje >= item.amount)
            {
                dk = amount;
            }
            else
            {
                dk = dkje;
            }
            //payAmount -= dk;
            //item.payAmount = payAmount;
            decimal dkPoint = 0;
            if (dk > 0)
                dkPoint = dk / PointRatio;
            return dk;
            //if (Account.presentExp >= dkPoint)
            //{
            //    Account.presentExp -= dkPoint;
            //}
            //else
            //{

            //    decimal cc = dk - Account.presentExp;
            //    Account.activatePoint -= cc;
            //    Account.presentExp = 0;
            //}

            //if(userPoint<=amount)
            //{
            //    return userPoint;
            //}
            //else 
            //{
            //    return amount;
            //}
        }

        public string GetAddress(UserAddress item)
        {
            string address = "";
            if(item.detailedAddress.Length>=10)
            {
                address = item.detailedAddress.Substring(0, 8)+"...";
            }
            return string.Format("{0}|{1}|{2}", address, item.recipients, item.moblie);
        }


        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ResultMessage ConfirmationGood(string orderNo)
        {
            Load();
            if (UserInformation == null)
                return new ResultMessage() { Code = -1, Msg = "请重新登录" };
            TransactionHelper.BeginTransaction();
            var order = OrderService.GetOrderNo(orderNo);
            if (order == null)
                return new ResultMessage() { Code = -1, Msg = "订单不存在" };
            if (order.orderState !=OrderStates.shipped)
                return new ResultMessage() { Code = -1, Msg = "订单状态不正确" };
            order.orderState = OrderStates.complete;
            if (Account != null)
            {
                if (Account.salerId > 0)
                {
                    var amount = (order.payAmount + order.point) * RebateRatio;
                    if (amount > 0)
                    {
                        var salerAccount = AccountService.GetByUserId(Account.salerId);
                        if (salerAccount != null)
                        {

                            salerAccount.activatePoint += amount;
                            OperationPointLog log = new OperationPointLog();
                            log.account = "activatePoint";
                            log.point = amount;
                            log.remark = string.Format("{0}消费", UserInformation.Name);
                            log.submitTime = DateTime.Now;
                            log.userId = salerAccount.userId;
                            OperationPointLogService.Insert(log);
                            AccountService.Update(salerAccount);
                        }
                    }
                }
            }
            OrderService.Update(order);
            TransactionHelper.Commit();
            return new ResultMessage() { Code = 0 };
           
        }

        public ResultMessage Refund(string orderNo)
        {
            Load();
            if (UserInformation == null)
                return new ResultMessage() { Code = -1, Msg = "请重新登录" };
            TransactionHelper.BeginTransaction();
            var order = OrderService.GetOrderNo(orderNo);
            if (order == null)
                return new ResultMessage() { Code = -1, Msg = "订单不存在" };
            if (order.orderState != OrderStates.complete&&order.orderState!=OrderStates.paid)
                return new ResultMessage() { Code = -1, Msg = "订单状态不正确" };
            order.orderState = OrderStates.applyRefund;
            //if (Account != null)
            //{
            //    if (Account.salerId > 0)
            //    {
            //        var salerAccount = AccountService.GetByUserId(Account.salerId);
            //        if (salerAccount != null)
            //        {
            //            var amount = order.payAmount * RebateRatio;
            //            salerAccount.activatePoint += amount;
            //            OperationPointLog log = new OperationPointLog();
            //            log.account = "activatePoint";
            //            log.point = amount;
            //            log.remark = string.Format("{0}消费", UserInformation.Name);
            //            log.submitTime = DateTime.Now;
            //            log.userId = UserInformation.UserId;
            //            OperationPointLogService.Insert(log);
            //        }
            //    }
            //}
            OrderService.Update(order);
            TransactionHelper.Commit();
            return new ResultMessage() { Code = 0 };
        }
        
    }
}