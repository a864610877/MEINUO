using Ecard.Models;
using Ecard.Requests;
using Ecard.Services;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.JuMeiMallIndex
{
    public class OperationJuMeiMall : LayoutModel
    {

        public ISiteService ISiteService { get; set; }
        public OperationJuMeiMall(ISiteService ISiteService)
        {
            this.ISiteService = ISiteService;
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site != null)
                url = site.imageUrl;
            this.TotalAmt = 0;
            this.Freight = 0;

        }
        string url = ""; //System.Configuration.ConfigurationManager.AppSettings["adminUrl"];

        [Dependency]
        public IJuMeiMallService JuMeiMallService { get; set; }
        [Dependency]
        public IOrderDetailService OrderDetailService { get; set; }

        [Dependency]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Dependency]
        public IOrderService OrderService { get; set; }

        [Dependency]
        public IMessagesService IMessagesService { get; set; }

        [Dependency]
        public IUserAddressService UserAddressService { get; set; }

        [Dependency]
        public IProvinceService ProvinceService { get; set; }

        [Dependency]
        public ICityService CityService { get; set; }
        [Dependency]
        public IAccountService IAccountService { get; set; }
        [Dependency]
        public IReviewService ReviewService { get; set; }
         [Dependency]
        public ISpecificationService SpecificationService { get; set; }
        [Dependency]
         public ICommodityCategorysService CommodityCategorysService { get; set; }
        [Dependency]
        public ICommodityService CommodityService { get; set; }
        [Dependency]
        public IMembershipService MembershipService { get; set; }
        [Dependency]
        public IRebateService RebateService { get; set; }
        [Dependency]
        public ISecondKillSetService SecondKillSetService { get; set; }
        [Dependency]
        public ISecondKillCommoditysService SecondKillCommoditysService { get; set; }

        /// <summary>
        /// 总额
        /// </summary>
        public decimal TotalAmt { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }
        public string GetFirstImage(string imageUrl)
        {
            string firstImg;

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                string[] sp = imageUrl.Split(',');
                if (sp.Count() > 0)
                    return firstImg = (url + sp[0]);
            }
            return null;



        }

        /// <summary>
        /// 图片详情图片
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public string GetSecondImage(string imageUrl)
        {
            string SecondImg;

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                string[] sp = imageUrl.Split(',');
                if (sp.Count() > 0)
                    return SecondImg = (url + sp[1]);
            }
            return null;
        }
        /// <summary>
        /// 图片详情图片
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public List<string> GetSecondImageList(string imageUrl)
        {
            List<string> SecondImg = new List<string>();

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                string[] sp = imageUrl.Split(',');
                if (sp.Count() > 0)
                {
                    for (var i = 3; i < sp.Count(); i++)
                    {
                        if (!sp[i].Contains("shopdefault.jpg") && !sp[i].Contains("undefined")&&sp[i]!="")
                          SecondImg.Add(url + sp[i]);
                    }
                }
                    
            }
            return SecondImg;



        }
        public string GetThirdImage(string imageUrl)
        {
            string SecondImg;

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                string[] sp = imageUrl.Split(',');
                if (sp.Count() > 0)
                    return SecondImg = (url + sp[2]);
            }
            return null;



        }


        public ResultMessage SaveShoppingCart(int commodityId, int quantity, int accountId, string Specification)
        {
            var tran = TransactionHelper.BeginTransaction();
            try
            {
                //int accountId = 0;
                var commodity = JuMeiMallService.GetById(commodityId);
                if (commodity == null)
                    return new ResultMessage() { Code = -1, Msg = "商品不存在！" };
                if (commodity.commodityState == CommodityStates.soldOut)
                    return new ResultMessage() { Code = -1, Msg = "商品已下架！" };
                if (commodity.commodityInventory < quantity)
                    return new ResultMessage() { Code = -1, Msg = "库存不足！" };
                //var GetUserInfo = JuMeiMallService.GetUserInfoByOpenId(openId);
                //if (GetUserInfo != null)
                //{
                //accountId = GetUserInfo.accountId;
                //}
                var Cart = JuMeiMallService.GetByAccountIdAndCommodityId(accountId, commodityId, Specification);
                if (Cart != null)
                {
                    Cart.quantity = quantity;
                    //Cart.specification = specification;
                    Cart.submitTime = DateTime.Now;
                    JuMeiMallService.UpdateCart(Cart);
                }
                else
                {
                    var item = new ShoppingCart();
                    item.commodityId = commodity.commodityId;
                    item.quantity = quantity;
                    item.specification = Specification;
                    item.submitTime = DateTime.Now;
                    item.userId = accountId;
                    JuMeiMallService.InsertCart(item);

                }
                tran.Commit();
                return new ResultMessage() { Code = 0, Msg = "成功加入购物车" };
            }
            catch (Exception ex)
            {
                logService.Insert(ex);
                return new ResultMessage() { Code = -1, Msg = "加入购物车失败！请稍后再试" };
            }
            finally { tran.Dispose(); }
        }


        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="quantity"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ResultMessage SaveOrder(string[] strArray, int userId)
        {
            var tran = TransactionHelper.BeginTransaction();
            try
            {
                Order model = new Order();   ///订单
                ///                        
                //int accountId = 0;
                decimal amount = 0;
                decimal point = 0;

                string dateTime = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
                var GetUserInfo = MembershipService.GetUserById(userId);// IAccountService.get(accountId);
                //var GetUserInfo = JuMeiMallService.GetUserInfoByOpenId(openId);

                if (GetUserInfo != null)
                {
                    //accountId = GetUserInfo.accountId;
                    ///订单
                    model.orderNo = dateTime + userId;//流水号

                }
                bool isFirs = false;
                var Firs = OrderService.MicroMallQuery(new MicroMallOrderRequest() { PageSize = 1, UserId = GetUserInfo.UserId, OrderType = 1 });
                if (Firs == null || Firs.ModelList == null || Firs.ModelList.Count <= 0)
                    isFirs = true;

                foreach (var item in strArray)//1.commodityId 2.quantity 3.shoppingCartId
                {
                    //将数组再分割为单个值
                    string[] strArray2 = item.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (strArray2 != null && strArray2.Length > 0)
                    {
                        int[] intArray = Array.ConvertAll<string, int>(strArray2, s => int.Parse(s));
                        int commodityId = intArray[0];
                        int quantity = intArray[1];
                        int shoppingCartId = intArray[2];


                        if (commodityId == 0 || shoppingCartId == 0)
                        {
                            return new ResultMessage() { Code = -1, Msg = "请选择商品，若多次无效请联系管理员！" };
                        }
                        if (quantity == 0)
                        {
                            return new ResultMessage() { Code = -1, Msg = "请选择商品数量！" };
                        }


                        ///订单详情
                        var commodity = JuMeiMallService.GetById(commodityId);
                        if (commodity == null)
                            return new ResultMessage() { Code = -1, Msg = "商品不存在,请重新生成订单!" };
                        if (commodity.commodityState == CommodityStates.soldOut)
                            return new ResultMessage() { Code = -1, Msg = "商品已下架,请重新生成订单!" };


                        if (commodity != null)
                        {

                            if (commodity.commodityInventory < quantity)
                                return new ResultMessage() { Code = -1, Msg = string.Format("库存不足:{0}", commodity.commodityName) };


                            var orderDetail = new OrderDetail();
                            orderDetail.commodityId = commodity.commodityId;
                            orderDetail.commodityName = commodity.commodityName;
                            orderDetail.orderNo = model.orderNo;
                            orderDetail.price = commodity.commodityPrice;
                            orderDetail.quantity = quantity;
                            orderDetail.point=commodity.commodityJifen;


                            var ShoppingCart = ShoppingCartService.GetById(shoppingCartId);
                            string specification = "";
                            if (ShoppingCart != null)
                            {
                                specification = ShoppingCart.specification;
                            }
                            else
                            {
                                return new ResultMessage() { Code = -1, Msg = "购物车商品不存在，请重新加入购物车!" };

                            }
                            orderDetail.specification = specification;
                            orderDetail.amount = orderDetail.price * orderDetail.quantity;
                            amount += orderDetail.price * orderDetail.quantity;
                            point += orderDetail.point * orderDetail.quantity;
                            ///插入订单详情
                            OrderDetailService.Insert(orderDetail);



                            //运费
                            model.freight = commodity.commodityFreight;

                            //商品库存减少

                            commodity.commodityInventory = commodity.commodityInventory - quantity;
                            commodity.sellQuantity = commodity.sellQuantity + quantity;
                            commodity.sellQuantity1 = commodity.sellQuantity1 + quantity;


                            JuMeiMallService.Update(commodity);
                            ///删除购物车数据
                            if (ShoppingCart != null)
                            {
                                ShoppingCartService.Delete(ShoppingCart);
                            }
                        }


                    }
                }



                //订单
                model.orderState = OrderStates.awaitPay;
                model.payState = PayStates.non_payment;
                model.submitTime = DateTime.Now;
                model.orderType = OrderType.normal;
                //model.distributionType = DistributionWay.kuaidi;
                model.userId = userId;
                //model.distributionstate = DistributionStates.unfilled;
                model.amount = amount;
                model.point = point;
                model.payAmount = 0;
                model.IsFirs = isFirs;
                //用户地址首次为空，待用户选择provinceId，cityId，detailedAddress


                OrderService.Insert(model);

                //var GetOrderService = OrderService.GetByOrderNo(model.orderNo);
                //if (GetOrderService != null)
                //{
                //    var UpdDtl = OrderDetailService.GetByOrderNo(model.orderNo);
                //    if (UpdDtl != null)
                //    {
                //        OrderDetailService.UpdateOrderDetailOrderId(GetOrderService.item.orderId, model.orderNo);

                //    }
                //}


                //if (accountId != 0)
                //{
                //    var message = new Fz_Messages();
                //    message.accountId = GetUserInfo.accountId;
                //    message.openId = GetUserInfo.openID;
                //    message.state = MessagesState.staySend;
                //    message.submitTime = DateTime.Now;
                //    message.keyword1 = model.orderNo;
                //    message.keyword2 = "等待付款";
                //    message.msgType = MsgType.orderState;
                //    IMessagesService.Insert(message);
                //}

                tran.Commit();
                
                return new ResultMessage() { Code = 0, Msg = model.orderNo };
            }
            catch (Exception ex)
            {
                logService.Insert(ex);
                return new ResultMessage() { Code = -1, Msg = "生成订单失败！请稍后再试" };
            }
            finally { tran.Dispose(); }
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ListOrderDetail GetOrders(string orderNo)
        {
            var model = new ListOrderDetail();
            decimal totalAmt = 0;
            var orderInfo = OrderService.GetOrderNo(orderNo);
            if (orderInfo == null)
            {
                return null;
            }
            else
            {
                
                model.payState = orderInfo.orderState;
                model.recipients = orderInfo.recipients == null ? " " : orderInfo.recipients;
                model.detailedAddress = orderInfo.detailedAddress == null ? " " : orderInfo.detailedAddress; 
                model.moblie = orderInfo.moblie == null ? " " : orderInfo.moblie; 
                model.Remark = orderInfo.remark == null ? " " : orderInfo.remark;
                model.orderState = orderInfo.orderState;
                model.Freight = orderInfo.freight;
                model.ExpressCompany = orderInfo.ExpressCompany == null ? " " : orderInfo.ExpressCompany;
                model.ExpressNumber = orderInfo.ExpressNumber == null ? " " : orderInfo.ExpressNumber;
                model.UserAddressId = orderInfo.UserAddressId;
                model.orderNo = orderInfo.orderNo;

            }
            var orderDtlInfo = OrderDetailService.GetAllByOrderNo(orderNo).ToList();
            if (orderDtlInfo != null)
            {
                foreach (var item in orderDtlInfo)
                {
                    var dtlModel = new OrderDetailsModel();
                    var commodity = JuMeiMallService.GetById(item.commodityId);
                    if (commodity == null)
                        return null;
                    if (commodity.commodityState == CommodityStates.soldOut)
                        return null;
                    //数量

                    dtlModel.Image = GetFirstImage(commodity.images);

                    dtlModel.Id = item.commodityId;

                    dtlModel.Price = item.price;

                    dtlModel.quantity = item.quantity;

                    dtlModel.Title = commodity.commodityName;

                    dtlModel.specification = item.specification;
                   // totalAmt += commodity.commodityPrice * item.quantity;
                    //freight = commodity.commodityFreight;
                    model.OrderDetailsList.Add(dtlModel);
                }
            }
            model.TotalAmt = orderInfo.amount;
            model.Freight = orderInfo.freight;
            var GetAddressInfo = UserAddressService.GetByAccountId(orderInfo.userId);
            if (GetAddressInfo != null && GetAddressInfo.ModelList != null && GetAddressInfo.ModelList.Count > 0)
            {
                //地址列表赋值
                model.ListUserAddress = GetAddressInfo.ModelList.ToList().Select(x => new UserAddress()
                {
                    userAddressId = x.userAddressId,
                    moblie = x.moblie,
                    recipients = x.recipients,
                    detailedAddress = x.detailedAddress,
                    //city = CityService.GetById(x.cityId) == null ? "" : CityService.GetById(x.cityId).Name,
                    //province = ProvinceService.GetById(x.provinceId) == null ? "" : ProvinceService.GetById(x.provinceId).Name,
                    //ProvinceName = x.provinceName,
                    //zipCode = x.zipCode


                }).ToList();

                return model;
            }
            return model;

        }

        /// <summary>
        /// 付款后更新订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="userAddressId"></param>
        /// <returns></returns>
        public ResultMessage UpdOrderPaid(string orderNo, int userAddressId, string remark, decimal totalAmt)
        {
            var tran = TransactionHelper.BeginTransaction();
            try
            {

                var userAddress = UserAddressService.GetById(userAddressId);
                if (userAddress == null)
                    return new ResultMessage() { Code = -1, Msg = "订单数据地址录入有误，请截图并联系管理员，订单号：" + orderNo };

                var orderInfo = OrderService.GetOrderNo(orderNo);
                if (orderInfo == null)
                    return new ResultMessage() { Code = -1, Msg = "订单数据丢失，请截图并联系管理员，订单号：" + orderNo };
                //if (totalAmt != orderInfo.amount + orderInfo.freight)
                //{
                //    return new ResultMessage() { Code = -1, Msg = "订单金额与订单不一致，请截图并联系管理员，订单号：" + orderNo };
                //}
                orderInfo.detailedAddress = userAddress.provinceName+" "+userAddress.detailedAddress;
                orderInfo.provinceId = userAddress.provinceId;
                orderInfo.cityId = userAddress.cityId;
                //orderInfo.orderState = OrderStates.paid;
                //orderInfo.payState = PayStates.paid;
                //orderInfo.submitTime = DateTime.Now;
                orderInfo.recipients = userAddress.recipients;
                orderInfo.moblie = userAddress.moblie;
                orderInfo.remark = remark;
                orderInfo.zipCode = userAddress.zipCode;
                orderInfo.distributionstate = DistributionStates.unfilled;
                //orderInfo.payType = PayTypes.weChatPayment;
                orderInfo.payAmount = orderInfo.amount+orderInfo.freight;
                orderInfo.UserAddressId = userAddress.userAddressId;
                OrderService.Update(orderInfo);

                tran.Commit();
                //TransactionHelper.Commit();
                return new ResultMessage() { Code = 0, Msg = orderNo };
            }
            catch (Exception ex)
            {

                logService.Insert(ex);
                return new ResultMessage() { Code = -1, Msg = "订单数据录入有误，请截图并联系管理员，订单号：" + orderNo };
            }
            finally { tran.Dispose(); }

        }

        /// <summary>
        /// 购物车列表
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ListCartDetail GetShoppingCart(int userId)
        {
            //var tran = TransactionHelper.BeginTransaction();
            var model = new ListCartDetail();
            try
            {
                var GetShoppingCart = ShoppingCartService.GetByAccountId(userId);//检测完毕后第二次获取信息
                if (GetShoppingCart != null && GetShoppingCart.ModelList != null && GetShoppingCart.ModelList.Count > 0)
                {
                    foreach (var item in GetShoppingCart.ModelList)
                    {
                        var model1 = new CartDetailsModel();
                        var comm = CommodityService.GetById(item.commodityId);
                        if (comm != null)
                        {
                            model1.commodityId = comm.commodityId;
                            model1.Freight = comm.commodityFreight;
                            model1.Id = item.shoppingCartId;
                            model1.Image = GetFirstImage(comm.images);
                            model1.Price = comm.commodityPrice;
                            model1.quantity = item.quantity;
                            model1.CommodityStock = comm.commodityInventory;
                            model1.specification = item.specification;
                            model1.Title = comm.commodityName;
                            model.CartDetailsList.Add(model1);
                            //model.TotalAmt += model1.Price * model1.quantity;
                            //model.Freight += model1.Freight;
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {

                logService.Insert(ex);
                return null;
            }

            return model;

        }

        /// <summary>
        /// 获取购物车商品信息
        /// </summary>
        /// <param name="commodityId"></param>
        /// <param name="type">1:图像 2：销售价 3：原价 4:名称 5:运费</param>
        /// <returns></returns>
        public T GetByComId<T>(int commodityId, int type = 0)
        {

            var commodity = JuMeiMallService.GetById(commodityId);
            if (commodity != null)
            {
                if (type == 1)
                {
                    object Image = commodity.images;
                    return (T)Image;
                }
                else if (type == 2)
                {
                    TotalAmt += commodity.commodityPrice;//购物车总金额
                    object commodityPrice = commodity.commodityPrice;
                    return (T)commodityPrice;
                }
                else if (type == 3)
                {
                    object commodityPrice1 = commodity.commodityPrice1;
                    return (T)commodityPrice1;
                }
                else if (type == 4)
                {
                    object commodityName = commodity.commodityName;
                    return (T)commodityName;
                }
                else if (type == 5)
                {
                    if (commodity.commodityFreight > Freight)//运费
                    {
                        Freight = commodity.commodityFreight;
                    }
                    object commodityFreight = commodity.commodityFreight;
                    return (T)commodityFreight;
                }

            }
            object flag = 0;
            return (T)flag;


        }

        public ResultMessage UpdShoppingCartQuantity(int commodityId, int shoppingCartId, int quantity)
        {
            try
            {
                //TransactionHelper.BeginTransaction();
                var ShoppingCart = ShoppingCartService.GetById(shoppingCartId);
                if (ShoppingCart != null)
                {
                    var commodity = JuMeiMallService.GetById(commodityId);
                    if (commodity != null)//检测商品信息
                    {
                        if (commodity.commodityState == CommodityStates.soldOut)
                            return new ResultMessage() { Code = -1, Msg = "商品已下架,请重新生成订单!" };

                    }
                    else
                    {
                        return new ResultMessage() { Code = -1, Msg = "商品不存在,请重新生成购物车!" };

                    }



                    if (commodity.commodityInventory < quantity)
                    {
                        ShoppingCart.quantity = commodity.commodityInventory;
                        ShoppingCartService.Update(ShoppingCart);
                        TransactionHelper.Commit();
                        return new ResultMessage() { Code = -1, Msg = string.Format("{0}库存不足,此商品库存剩余{1}件", commodity.commodityName, commodity.commodityInventory), quantity = commodity.commodityInventory };
                    }

                    ShoppingCart.quantity = quantity;
                    ShoppingCartService.Update(ShoppingCart);


                }
                // TransactionHelper.Commit();
                return new ResultMessage() { Code = 0, Msg = "购物车修改数量成功" };

            }
            catch (Exception ex)
            {
                logService.Insert(ex);
                return new ResultMessage() { Code = -1, Msg = "购物车数量修改-网络错误，请稍后重试..." };
            }



        }

        /// <summary>
        /// 获取地址列表
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public AddressListsModel GetAddressList(int accountId)
        {

            var model = new AddressListsModel();
            var GetUserInfo = IAccountService.GetById(accountId);//获取userId
            if (GetUserInfo != null)
            {

                // accountId = GetUserInfo.accountId;
                if (GetUserInfo.defaultAddressId != 0)
                {
                    model.defaultAddressId = GetUserInfo.defaultAddressId;
                    var DefGetAddressInfo = UserAddressService.GetById(model.defaultAddressId);
                    if (DefGetAddressInfo != null)
                    {
                        model.defaultDetailedAddress = DefGetAddressInfo.detailedAddress;
                        model.defaultMoblie = DefGetAddressInfo.moblie;
                        model.defaultProvinceName = DefGetAddressInfo.provinceName == null ? " " : DefGetAddressInfo.provinceName;
                        model.defaultRecipients = DefGetAddressInfo.recipients;
                    }
                    else
                    {
                        model.defaultAddressId = 0;
                    }

                }
                else
                {
                    model.defaultAddressId = 0;
                }
            }

            var GetAddressInfo = UserAddressService.GetByAccountId(accountId);
            if (GetAddressInfo != null && GetAddressInfo.ModelList != null && GetAddressInfo.ModelList.Count > 0)
            {
                //地址列表赋值
                model.AddressList = GetAddressInfo.ModelList.ToList().Select(x => new AddressListModel()
                {
                    userAddressId = x.userAddressId,
                    moblie = x.moblie,
                    recipients = x.recipients,
                    detailedAddress = x.detailedAddress,
                    city = CityService.GetById(x.cityId) == null ? "" : CityService.GetById(x.cityId).Name,
                    province = ProvinceService.GetById(x.provinceId) == null ? "" : ProvinceService.GetById(x.provinceId).Name,
                    ProvinceName = x.provinceName,
                    zipCode = x.zipCode


                }).ToList();

                return model;
            }
            return null;

        }

        /// <summary>
        /// 删除购物车
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ResultMessage DelShoppingCartById(int[] ids)
        {
            var tran = TransactionHelper.BeginTransaction();
            try
            {

                foreach (var item in ids)
                {
                    var DelModel = ShoppingCartService.GetById(item);
                    if (DelModel != null)
                    {
                        ShoppingCartService.Delete(DelModel);
                    }

                }


                tran.Commit();
                //TransactionHelper.Commit();

                return new ResultMessage() { Code = 0, Msg = "购物车已清除" };
            }
            catch (Exception ex)
            {
                tran.Dispose();
                logService.Insert(ex);
                return new ResultMessage() { Code = -1, Msg = "购物车清除失败-网络错误，请稍后重试..." };
            }



        }

        public string GetCommidityIdList(string OrderNo)
        {
            var commidityList = "";
            var orderDtlList = OrderDetailService.GetByOrderNo(OrderNo);
            if (orderDtlList != null)
            {
                foreach (var item in orderDtlList)
                {
                    commidityList += item.commodityId + ",";
                }
            }
            return commidityList;




        }
        //public ResultMessage SaveOrderDetailsSpecification(int commodityId, string orderNo, string liuyanContent)
        //{

        //    try
        //    {
        //        TransactionHelper.BeginTransaction();
        //        var model = OrderDetailService.GetBycommodityIdAndOrderNo(commodityId, orderNo);
        //        if (model !=null)
        //        {
        //            model.remark = liuyanContent;
        //            OrderDetailService.Update(model);
        //            TransactionHelper.Commit();
        //            return new ResultMessage() { Code = 0, Msg = "留言录入成功!" };
        //        }
        //        return new ResultMessage() { Code = -1, Msg = "留言录入失败,请稍后重试!" };



        //    }
        //    catch (Exception ex)
        //    {
        //        logService.Insert(ex);
        //        return new ResultMessage() { Code = -1, Msg = "留言录入失败,请稍后重试!" };
        //    }




        //}

        public ResultMessage SaveGDtlOrder(int commodityId, int quantity, int userId, string Specification)
        {
            var tran = TransactionHelper.BeginTransaction();
            try
            {

                decimal amount = 0;
                decimal point = 0;
                ///订单
                Order model = new Order();
                string dateTime = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
                var GetUserInfo = MembershipService.GetUserById(userId);

                if (GetUserInfo != null)
                {
                    //accountId = GetUserInfo.accountId;
                    ///订单
                    model.orderNo = dateTime + userId;
                }
                bool isFirs = false;
                var Firs = OrderService.MicroMallQuery(new MicroMallOrderRequest() { PageSize=1,UserId=GetUserInfo.UserId, OrderType=1});
                if (Firs == null||Firs.ModelList==null||Firs.ModelList.Count<=0)
                    isFirs = true;
                ///订单详情
                var commodity = JuMeiMallService.GetById(commodityId);
                if (commodity == null)
                    return new ResultMessage() { Code = -1, Msg = "商品不存在,请重新生成订单!" };
                if (commodity.commodityState == CommodityStates.soldOut)
                    return new ResultMessage() { Code = -1, Msg = "商品已下架,请重新生成订单!" };
                if (commodity != null)
                {
                    if (commodity.commodityInventory < quantity)
                        return new ResultMessage() { Code = -1, Msg = string.Format("库存不足:{0}", commodity.commodityName) };
                    var orderDetail = new OrderDetail();
                    orderDetail.commodityId = commodity.commodityId;
                    orderDetail.commodityName = commodity.commodityName;
                    orderDetail.orderNo = model.orderNo;
                    orderDetail.price = commodity.commodityPrice;
                    orderDetail.quantity = quantity;
                    orderDetail.specification = Specification;
                    orderDetail.point = commodity.commodityJifen;
                    orderDetail.amount = orderDetail.price;
                    amount += orderDetail.price * orderDetail.quantity;
                    point += orderDetail.point * orderDetail.quantity;
                    ///插入订单详情
                    OrderDetailService.Insert(orderDetail);
                }

                //运费
                model.freight = commodity.commodityFreight;

                //商品库存减少
                commodity.commodityInventory = commodity.commodityInventory - quantity;
                commodity.sellQuantity = commodity.sellQuantity + quantity;
                commodity.sellQuantity1 = commodity.sellQuantity1 + quantity;


                JuMeiMallService.Update(commodity);


                //订单
                model.orderState = OrderStates.awaitPay;
                model.payState = PayStates.non_payment;
                model.submitTime = DateTime.Now;
                model.orderType = OrderType.normal;
                //model.distributionType = DistributionWay.kuaidi;
                model.userId = userId;
                //model.distributionstate = DistributionStates.unfilled;
                model.amount = amount;
                model.point = point;
                model.payAmount = 0;
                model.IsFirs = isFirs;
                //用户地址首次为空，待用户选择provinceId，cityId，detailedAddress


              model.orderId= OrderService.Insert(model);

                var GetOrderService = OrderService.GetByOrderNo(model.orderNo);
                if (GetOrderService != null)
                {
                    var UpdDtl = OrderDetailService.GetByOrderNo(model.orderNo);
                    if (UpdDtl != null)
                    {
                        OrderDetailService.UpdateOrderDetailOrderId(GetOrderService.item.orderId, model.orderNo);

                    }
                }


                //if (!string.IsNullOrWhiteSpace(GetUserInfo))
                //{
                //    var message = new Fz_Messages();
                //    message.accountId = GetUserInfo.accountId;
                //    message.openId = GetUserInfo.openID;
                //    message.state = MessagesState.staySend;
                //    message.submitTime = DateTime.Now;
                //    message.keyword1 = model.orderNo;
                //    message.keyword2 = "等待付款";
                //    message.msgType = MsgType.orderState;
                //    IMessagesService.Insert(message);
                //}
                tran.Commit();
               // RebateService.Rebate2(model.orderId);
                return new ResultMessage() { Code = 0, Msg = model.orderNo };
            }
            catch (Exception ex)
            {
                logService.Insert(ex);
                return new ResultMessage() { Code = -1, Msg = "生成订单失败！请稍后再试" };
            }
            finally { tran.Dispose(); }
        }
        public ResultMessage SecondKillGDtlOrder(int id,int userId)
        {
            
            var set = SecondKillSetService.GetFirst();
            if (set == null)
                return new ResultMessage() { Code = -3, Msg = "当前没有秒杀" };
            if (set.state != SecondKillSetState.Normal)
            {
                return new ResultMessage() { Code = -3, Msg = "当前没有秒杀" };
            }
            if (set.startTime > DateTime.Now)
                return new ResultMessage() { Code = -3, Msg = "秒杀还没开始" };
            if (DateTime.Now >= set.endTime)
                return new ResultMessage() { Code = -3, Msg = "秒杀已结束" };
            var tran = TransactionHelper.BeginTransaction();
            var secondKillCommunity = SecondKillCommoditysService.GetById(id);
            if (secondKillCommunity == null)
            {
                return new ResultMessage() { Code = -3, Msg = "秒杀商品不存在" };
            }
            if (secondKillCommunity.surplusNum <= 0)
                return new ResultMessage() { Code = -3, Msg = "已买完" };
           

            try
            {
                secondKillCommunity.surplusNum -= 1;
                secondKillCommunity.payNum += 1;
                SecondKillCommoditysService.Update(secondKillCommunity);
                decimal amount = 0;
                decimal point = 0;
                ///订单
                Order model = new Order();
                string dateTime = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
                var GetUserInfo = MembershipService.GetUserById(userId);
                if (GetUserInfo != null)
                {
                    //accountId = GetUserInfo.accountId;
                    ///订单
                    model.orderNo = dateTime + userId;
                }
                //bool isFirs = false;
                //var Firs = OrderService.MicroMallQuery(new MicroMallOrderRequest() { PageSize = 1, UserId = GetUserInfo.UserId });
                //if (Firs == null || Firs.ModelList == null || Firs.ModelList.Count <= 0)
                //    isFirs = true;
                ///订单详情
                var commodity = JuMeiMallService.GetById(secondKillCommunity.commodityId);
                if (commodity == null)
                    return new ResultMessage() { Code = -1, Msg = "商品不存在,请重新下订单!" };
                if (commodity.commodityState == CommodityStates.soldOut)
                    return new ResultMessage() { Code = -1, Msg = "商品已下架,请重新下订单!" };
                if (commodity != null)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.commodityId = commodity.commodityId;
                    orderDetail.commodityName = commodity.commodityName;
                    orderDetail.orderNo = model.orderNo;
                    orderDetail.price = secondKillCommunity.price;
                    orderDetail.quantity = 1;
                    //orderDetail.specification = Specification;
                    orderDetail.point = 0;// commodity.commodityJifen;
                    orderDetail.amount = secondKillCommunity.price;
                    amount += orderDetail.price * orderDetail.quantity;
                    //point += orderDetail.point * orderDetail.quantity;
                    ///插入订单详情
                    OrderDetailService.Insert(orderDetail);
                }

                //运费
                model.freight = commodity.commodityFreight;

                //商品库存减少
                //commodity.commodityInventory = commodity.commodityInventory - quantity;
                commodity.sellQuantity = commodity.sellQuantity + 1;
                commodity.sellQuantity1 = commodity.sellQuantity1 + 1;
                JuMeiMallService.Update(commodity);
                //订单
                model.orderState = OrderStates.awaitPay;
                model.payState = PayStates.non_payment;
                model.submitTime = DateTime.Now;
                model.orderType = OrderType.secondKill;
                //model.distributionType = DistributionWay.kuaidi;
                model.userId = userId;
                //model.distributionstate = DistributionStates.unfilled;
                model.amount = amount;
                model.point = point;
                model.payAmount = 0;
                model.IsFirs = false;
                //用户地址首次为空，待用户选择provinceId，cityId，detailedAddress


                model.orderId = OrderService.Insert(model);

                var GetOrderService = OrderService.GetByOrderNo(model.orderNo);
                if (GetOrderService != null)
                {
                    var UpdDtl = OrderDetailService.GetByOrderNo(model.orderNo);
                    if (UpdDtl != null)
                    {
                        OrderDetailService.UpdateOrderDetailOrderId(GetOrderService.item.orderId, model.orderNo);

                    }
                }
                //if (!string.IsNullOrWhiteSpace(GetUserInfo))
                //{
                //    var message = new Fz_Messages();
                //    message.accountId = GetUserInfo.accountId;
                //    message.openId = GetUserInfo.openID;
                //    message.state = MessagesState.staySend;
                //    message.submitTime = DateTime.Now;
                //    message.keyword1 = model.orderNo;
                //    message.keyword2 = "等待付款";
                //    message.msgType = MsgType.orderState;
                //    IMessagesService.Insert(message);
                //}
                tran.Commit();
                // RebateService.Rebate2(model.orderId);
                return new ResultMessage() { Code = 0, Msg = model.orderNo };
            }
            catch (Exception ex)
            {
                logService.Insert(ex);
                return new ResultMessage() { Code = -1, Msg = "生成订单失败！请稍后再试" };
            }
            finally { tran.Dispose(); }
        }
    }
}