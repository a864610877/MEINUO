using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Ecard;
using Ecard.Models;
using Ecard.Requests;
using Ecard.Services;
using MicroMall.Models;
using MicroMall.Models.ImageAds;
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
    /// 欣美聚诚商城首页
    /// </summary>
    public class JuMeiMallController : Controller
    {
        //
        // GET: /JuMeiMall/
        private readonly IUnityContainer _container;
        private readonly ILog4netService Log4netService;

        private readonly ISiteService ISiteService;
        private readonly ICommodityService ICommodityService;
        private readonly ICommodityCategorysService ICommodityCategorysService;
        private readonly ISiteService SiteService;
        private readonly IOrderService IOrderService;
        private readonly IMembershipService MembershipService;
        private readonly IAccountService AccountService;
        private readonly ISecondKillSetService SecondKillSetService;
        private readonly ISecondKillCommoditysService SecondKillCommoditysService;
        private readonly TransactionHelper TransactionHelper;
        //private readonly
        string url = "";
        JsApiPay jsApiPay = new JsApiPay();
        public JuMeiMallController(IUnityContainer _container, ILog4netService Log4netService, ISiteService ISiteService,
            ICommodityService ICommodityService, ICommodityCategorysService ICommodityCategorysService, ISiteService SiteService
            , IOrderService IOrderService, IMembershipService MembershipService, IAccountService AccountService
            , ISecondKillSetService SecondKillSetService, ISecondKillCommoditysService SecondKillCommoditysService, TransactionHelper TransactionHelper)
        {
            this._container = _container;
            this.Log4netService = Log4netService;
            this.ISiteService = ISiteService;
            this.ICommodityService = ICommodityService;
            this.ICommodityCategorysService = ICommodityCategorysService;
            this.ISiteService = ISiteService;
            this.IOrderService = IOrderService;
            this.MembershipService = MembershipService;
            this.AccountService = AccountService;
            this.SecondKillSetService = SecondKillSetService;
            this.SecondKillCommoditysService = SecondKillCommoditysService;
            this.TransactionHelper = TransactionHelper;
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site != null)
                url = site.imageUrl;
        }

        /// <summary>
        /// Fas首页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
       
        public ActionResult JuMeiMallIndex()
        {


            var request = _container.Resolve<OperationJuMeiMall>();

            var model = new JuMeiMallListModel();
            var listMall = new JuMeiMallListModel();
            var CategoryRecordSet = request.JuMeiMallService.FASQuery();
            if (CategoryRecordSet != null)
            {
                model.CateMallList = CategoryRecordSet.Select(x => new CateMallModel()
                {
                    Categoryname = x.name,
                    commodityCategoryId = x.commodityCategoryId
                }).ToList();
                //foreach (var item in CategoryRecordSet)
                //{
                var GoodsRecordset = request.JuMeiMallService.QueryHotSale();
                    if (GoodsRecordset != null)
                    {
                        listMall.JuMeiMallExList = GoodsRecordset.Select(x => new JuMeiMallModelExpress()
                        {
                            commodityCategoryId = x.commodityCategoryId,
                            commodityId = x.commodityId,
                            commodityName = x.commodityName,
                            commodityPrice = x.commodityPrice,
                            commodityRank = x.commodityRank,
                            images = request.GetFirstImage(x.images),
                            sellQuantity = x.sellQuantity,
                            submitTime = x.submitTime.ToString(),
                            commodityJifen=x.commodityJifen,

                            commodityRemark = x.commodityRemark == null ? " " : x.commodityRemark

                        }).ToList();
                        model.JuMeiMallExList.AddRange(listMall.JuMeiMallExList);
                    }

                //}


            }

            var request2 = _container.Resolve<OperationImageAds>();

            var recordSet = request2.ImageAdsService.Query();
            if (recordSet != null)
            {
                model.AdsList = recordSet.ToList().Select(x => new ImageAdsModel()
                {
                    adId = x.adId,
                    ImageUrl = url + x.ImageUrl,
                    link = x.link
                }).ToList();
            }

            return View(model);
            return Json(model);


        }

        [HttpPost]
        public ActionResult JuMeiMallAppend(JuMeiMallIndexRequest query)
        {

            var request = _container.Resolve<OperationJuMeiMall>();

            var model = new JuMeiMallListModel();
            var recordSet = request.JuMeiMallService.Query(query);
            if (recordSet != null && recordSet.ModelList != null && recordSet.ModelList.Count > 0)
            {
                model.JuMeiMallList = recordSet.ModelList.ToList().Select(x => new JuMeiMallModel()
                {
                    commodityId = x.commodityId,
                    commodityName = x.commodityName,
                    images = request.GetFirstImage(x.images),
                    commodityPrice = x.commodityPrice,
                    commodityRank = x.commodityRank,
                    sellQuantity = x.sellQuantity,
                    submitTime = x.submitTime.ToString()

                }).ToList();
                model.totalCount = recordSet.ModelList.Count;

            }


            return Json(model);


        }

        public ActionResult JuMeiMallSearchIndex(JuMeiMallSearchRequest query)
        {
            var request = _container.Resolve<OperationJuMeiMall>();
            var model = new JuMeiMallListModel();
            query.PageSize = 500;
            var category = request.CommodityCategorysService.GetById(query.commodityCategoryId);
            if (category != null)
            {
                model.categoryName = category.name;
            }
            var recordSet = request.JuMeiMallService.QueryList(query);
            if (recordSet != null && recordSet.TotalCount > 0)
            {
                model.JuMeiMallExList = recordSet.ModelList.ToList().Select(x => new JuMeiMallModelExpress()
                {
                    commodityId = x.commodityId,
                    commodityName = x.commodityName,
                    images = request.GetFirstImage(x.images),
                    commodityPrice = x.commodityPrice,
                    commodityRank = x.commodityRank,
                    sellQuantity = x.sellQuantity,
                    submitTime = x.submitTime.ToString(),
                    commodityRemark = x.commodityRemark == null ? " " : x.commodityRemark,
                    commodityJifen = x.commodityJifen,
                   

                }).ToList();
                model.totalCount = recordSet.TotalCount;
                model.pageIndex = query.PageIndex;
            }

            return View(model);
           // return Json(model);

        }

        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="commodityId"></param>
        /// <returns></returns>
        
        public ActionResult GoodsDetails(int id)
        {
            //var model = new GoodsDetailsModel();
            var request = _container.Resolve<OperationJuMeiMall>();
            GoodsDetailsExpress model = new Ecard.Services.GoodsDetailsExpress();
            var recordSet = request.JuMeiMallService.GetDetailsExById(id);
            if (recordSet != null)
            {
                model = (GoodsDetailsExpress)recordSet;
                var list = request.ReviewService.GetGoodsReviewNew(model.commodityId);
                if (list != null)
                    model.reviewCount = list.Count;
                model.listReviewNew = list;
                string url = "";
                var site = ISiteService.Query(null).FirstOrDefault();
                if (site != null)
                    url = site.adminUrl;
                model.listImg = request.GetSecondImageList(recordSet.images);
                model.commodityDetails = recordSet.commodityDetails.Replace("/MicroMalls/CommodityImages", url + "/MicroMalls/CommodityImages");
                model.sellQuantity = recordSet.sellQuantity;
                var listAattribute = new List<SpecificationAndSpecificationDetail>();
                if (!string.IsNullOrWhiteSpace(model.specificationId))
                {
                    string[] spIds = model.specificationId.Split(',');
                    for (int i = 0; i < spIds.Count(); i++)
                    {
                        int specificationId = 0;
                        int.TryParse(spIds[i], out specificationId);
                        var specification = request.SpecificationService.GetSpecificationAndSpecificationDetailById(specificationId);
                        if (specification != null)
                            listAattribute.Add(specification);
                    }
                    //return new CommodityDetail(item, ImageUrl, list,null);
                }
                model.listAattribute = listAattribute;
            }

            return View(model);
        }

        /// <summary>
        /// 加入购物车
        /// </summary>
        /// <param name="commodityId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSoppingCart(int commodityId, int quantity, string Specification)
        {
            try
            {
                var request = _container.Resolve<OperationJuMeiMall>();
                var checkSession = RediToLogin(1);
                if (checkSession.Code == 110)
                {
                    return Json(checkSession);
                }
                int accountId = 0;
                int.TryParse(Request.Cookies[SessionKeys.USERID].Value.ToString(), out accountId);
                //string OpenId = Session["openid"].ToString();
                var model = request.SaveShoppingCart(commodityId, quantity, accountId, Specification);
                return Json(model);
            }
            catch (Exception ex)
            {

                WxPayAPI.Log.Info(this.GetType().ToString(), string.Format(ex.Message, ex.Source, ex.StackTrace));
                return Json(new ResultMessage() { Code = -1, Msg = "网络错误，加入购物车失败，请稍后重试！" });
            }


        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult BuyDetails(string orderNo)
        {
            try
            {
                //return View();
                if (!string.IsNullOrWhiteSpace(orderNo))
                {
                    var request = _container.Resolve<OperationJuMeiMall>();
                    var checkSession = RediToLogin(1);
                    if (checkSession.Code == 110)
                    {
                        return RedirectToAction("index","login");
                    }
                    //string accountId = Session["accountId"].ToString();
                    var model = request.GetOrders(orderNo);
                    if (model == null)
                    {
                        return Json(new ResultMessage() { Code = -1, Msg = "订单商品金额已更新或已支付，请确认或重新下单！" });
                    }
                    return View(model);
                }
                else
                {
                    return Json("订单商品号为空，请重试");
                }


            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Info(this.GetType().ToString(), string.Format(ex.Message, ex.Source, ex.StackTrace));
                return Json("生成订单失败，请稍后再试");
            }

        }

        /// <summary>
        /// 购物车生成订单
        /// </summary>
        /// <param name="commodityIdList">1.commodityId 2.quantity 3.shoppingCartId</param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateOrder(string commodityIdList)//string commodityId, int quantity = 0)
        {
            try
            {
                //商品Id
                //string[] strArray = commodityId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //if (strArray != null && strArray.Length > 0)
                //{
                //    int[] intArray = Array.ConvertAll<string, int>(strArray, s => int.Parse(s));
                if (!string.IsNullOrWhiteSpace(commodityIdList))
                {
                    //第一次筛选
                    string[] strArray = commodityIdList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (strArray != null && strArray.Length > 0)
                    {
                        var request = _container.Resolve<OperationJuMeiMall>();
                        var checkSession = RediToLogin(1);
                        if (checkSession.Code == 110)
                        {
                            return Json(checkSession);
                        }
                        int userId = 0;
                        int.TryParse(Request.Cookies[SessionKeys.USERID].Value.ToString(), out userId);
                        // string OpenId = Session["openid"].ToString();
                        var model = request.SaveOrder(strArray, userId);
                        return Json(model);



                    }
                    else
                    {
                        return Json(new ResultMessage() { Code = -1, Msg = "请选择商品" });
                    }


                }
                else
                {
                    return Json(new ResultMessage() { Code = -1, Msg = "请选择商品" });
                }


            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Info(this.GetType().ToString(), string.Format(ex.Message, ex.Source, ex.StackTrace));
                return Json(new ResultMessage() { Code = -1, Msg = "生成订单失败，请稍后再试" });
            }


        }
        /// <summary>
        /// 商品详情页生成订单GoodsDetail.html-单个
        /// </summary>
        /// <param name="commodityId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateGDtlOrder(int commodityId, int quantity, string Specification)//string commodityId, int quantity = 0)
        {
            try
            {
                //商品Id
                //string[] strArray = commodityId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //if (strArray != null && strArray.Length > 0)
                //{
                //    int[] intArray = Array.ConvertAll<string, int>(strArray, s => int.Parse(s));
                if (commodityId > 0 && quantity > 0)
                {
                    var request = _container.Resolve<OperationJuMeiMall>();
                    var checkSession = RediToLogin(1);
                    if (checkSession.Code == 110)
                    {
                        return Json(checkSession);
                    }
                    int accountId = 0;
                    int.TryParse(Request.Cookies[SessionKeys.USERID].Value.ToString(), out accountId);
                    var model = request.SaveGDtlOrder(commodityId, quantity, accountId, Specification);
                    return Json(model);

                }
                else
                {
                    return Json(new ResultMessage() { Code = -1, Msg = "请选择商品参数" });
                }


            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Info(this.GetType().ToString(), string.Format(ex.Message, ex.Source, ex.StackTrace));
                return Json(new ResultMessage() { Code = -1, Msg = "生成订单失败，请稍后再试" });
            }


        }
        /// <summary>

        /// 商品留言设置

        /// </summary>
        /// <param name="commodityId">商品ID</param>
        /// <param name="orderNo">订单No</param>
        /// <param name="liuyanContent">留言内容</param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult SetOrderDetailsSpecification(string commodityId, string orderNo, string liuyanContent)
        //{

        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(liuyanContent) && !string.IsNullOrWhiteSpace(orderNo) && liuyanContent.Length <=100)
        //        {
        //            int id = 0;
        //            int.TryParse(commodityId, out id);
        //            if (id > 0)
        //            {
        //                var request = _container.Resolve<OperationJuMeiMall>();
        //                var checkSession = RediToLogin(1);
        //                if (checkSession.Code == 110)
        //                {
        //                    return Json(checkSession);
        //                }
        //                string OpenId = Session["openid"].ToString();
        //                var model = request.SaveOrderDetailsSpecification(id, orderNo, liuyanContent);
        //                return Json(model);
        //            }
        //            else
        //            {
        //                return Json(new ResultMessage() { Code = -1, Msg = "订单数据有误，请稍后再试" });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new ResultMessage() { Code = 0, Msg = "" });
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        WxPayAPI.Log.Info(this.GetType().ToString(), string.Format(ex.Message, ex.Source, ex.StackTrace));
        //        return Json(new ResultMessage() { Code = -1, Msg = "留言录入失败，请稍后再试" });
        //    }


        //}
        /// <summary>
        /// 实现微信支付
        /// </summary>
        /// <param name="totalfee"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeterRecharge(string orderNo)
        {

            object objResult = "";
            if (string.IsNullOrWhiteSpace(orderNo))
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
                return Json(objResult);
            }

            var checkSession = RediToLogin(1);
            if (checkSession.Code == 110)
            {
                return Json(checkSession);
            }
            int userId = 0;
            int.TryParse(Session[SessionKeys.USERID].ToString(), out userId);

            var user = MembershipService.GetUserById(userId);
            if (user == null)
            {
                ModelForOrder aOrder = new ModelForOrder()
                {
                    appId = "",
                    nonceStr = "",
                    packageValue = "",
                    paySign = "",
                    timeStamp = "",
                    msg = "下单失败,请重试,若多次失败,请联系管理员1."
                };
                objResult = aOrder;
                return Json(objResult);
            }

            var account = AccountService.GetByUserId(user.Id);
            if (account == null)
            {
                ModelForOrder aOrder = new ModelForOrder()
                {
                    appId = "",
                    nonceStr = "",
                    packageValue = "",
                    paySign = "",
                    timeStamp = "",
                    msg = "下单失败,请重试,若多次失败,请联系管理员2."
                };
                objResult = aOrder;
                return Json(objResult);
            }
            var orderNoRec = IOrderService.GetOrderNo(orderNo);
            if (orderNoRec == null)
            {
                ModelForOrder aOrder = new ModelForOrder()
                {
                    appId = "",
                    nonceStr = "",
                    packageValue = "",
                    paySign = "",
                    timeStamp = "",
                    msg = "下单失败,请重试,若多次失败,请联系管理员3."
                };
                objResult = aOrder;
                return Json(objResult);
            }

            //if (orderNoRec.amount + orderNoRec.freight != totalfee)
            //{
            //    ModelForOrder aOrder = new ModelForOrder()
            //    {
            //        appId = "",
            //        nonceStr = "",
            //        packageValue = "",
            //        paySign = "",
            //        timeStamp = "",
            //        msg = "下单失败,金额不正确，请重试，若多次失败,请联系管理员."
            //    };
            //    objResult = aOrder;
            //    return Json(objResult);
            //}


            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            if (string.IsNullOrWhiteSpace(account.openID))
            {
                ModelForOrder aOrder = new ModelForOrder()
                {
                    appId = "",
                    nonceStr = "",
                    packageValue = "",
                    paySign = "",
                    timeStamp = "",
                    msg = "你还没有授权，请重新登录授权"
                };
                objResult = aOrder;
                return Json(objResult);
            }
            jsApiPay.openid = account.openID;
            jsApiPay.total_fee = (int)(orderNoRec.amount + orderNoRec.freight * 100);

            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(orderNo);
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
                WxPayAPI.Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam + ",openid:" + jsApiPay.openid + ",orderNo:" + orderNo + ",appId:" + aOrder.appId + ",nonceStr:" + aOrder.nonceStr + ",packageValue:" + aOrder.packageValue + ",paySign:" + aOrder.paySign + ",timeStamp:" + aOrder.timeStamp);
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Error(this.GetType().ToString(), ex.Message.ToString());
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

        /// <summary>
        /// 支付成功后修改订单数据
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="userAddressId">用户地址</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateOrdersPaid(string orderNo, int userAddressId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderNo) || userAddressId == 0)
                {
                    return Json(new ResultMessage() { Code = -1, Msg = "订单数据录入有误，请截图并联系管理员，订单号：" + orderNo });
                }

                int userId = 0;
                int.TryParse(Request.Cookies[SessionKeys.USERID].Value.ToString(), out userId);

                var account = AccountService.GetByUserId(userId);
                if (account == null)
                {
                    return Json(new ResultMessage() { Code = -1, Msg = "请重新登录" });
                }

                var request = _container.Resolve<OperationJuMeiMall>();
                var model = request.UpdOrderPaid(orderNo, userAddressId, "", 0);
                if (model.Code != 0)
                    return Json(model);
                if(string.IsNullOrWhiteSpace(account.openID))
                    return Json(new ResultMessage() { Code = -1, Msg = "你还没有授权,请重新登录进行授权" });
                var order = IOrderService.GetOrderNo(orderNo);
                JsApiPay jsApiPay = new JsApiPay();
                jsApiPay.openid = account.openID;
                jsApiPay.total_fee = (int)(order.payAmount * 100);
                try
                {
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(order.orderNo);
                    string wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                    WxPayAPI.Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                    return Json(new ResultMessage() { Code = 0, Msg = wxJsApiParam });

                }
                catch (Exception ex)
                {
                    WxPayAPI.Log.Error(this.GetType().ToString(), ex.Message.ToString());
                    return Json(new ResultMessage() { Code = -1, Msg = ex.Message.ToString() });
                }
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Info(this.GetType().ToString(), string.Format(ex.Message, ex.Source, ex.StackTrace));
                return Json(new ResultMessage() { Code = -1, Msg = "订单数据录入有误，请截图并联系管理员，订单号：" + orderNo });
            }

        }

        /// <summary>
        /// 购物车列表
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult GetShoppingCart()
        {
            if (Request.Cookies[SessionKeys.USERID] == null)
                return RedirectToAction("Index", "login",new { url= "/JuMeiMall/GetShoppingCart" });
            int userId = Convert.ToInt32(Request.Cookies[SessionKeys.USERID].Value);
            var request = _container.Resolve<OperationJuMeiMall>();
            var recordSet = request.GetShoppingCart(userId);
            return View(recordSet);
        }


        /// <summary>
        /// 购物车修改数量
        /// </summary>
        /// <param name="commodityId"></param>
        /// <param name="shoppingCartId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdShoppingCartQuantity(int commodityId, int shoppingCartId, int quantity)//(int commodityId, int shoppingCartId, int type)
        {
            var checkSession = RediToLogin(1);
            if (checkSession.Code == 110)
            {
                return Json(checkSession);
            }
            if (quantity < 1)
            {
                return Json(new ResultMessage() { Code = -1, Msg = "商品数量不能小于1件" });
            }
            var request = _container.Resolve<OperationJuMeiMall>();
            var recordSet = request.UpdShoppingCartQuantity(commodityId, shoppingCartId, quantity);
            if (recordSet != null)
            {

                return Json(recordSet);
            }
            return Json(new ResultMessage() { Code = -1, Msg = "购物车修改数量-网络错误，请稍后重试" });

        }
        /// <summary>
        /// 获取收货地址列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAddressList()
        {
            var checkSession = RediToLogin(1);
            if (checkSession.Code == 110)
            {
                return Json(checkSession);
            }
            int accountId = 0;
            int.TryParse(Session["accountId"].ToString(), out accountId);

            var request = _container.Resolve<OperationJuMeiMall>();
            var recordSet = request.GetAddressList(accountId);
            if (recordSet != null)
            {

                return Json(recordSet);
            }


            return Json(new ResultMessage() { Code = -1, Msg = "收货地址数据有误，请尝试重新刷新页面" });




        }

        /// <summary>
        /// 删除购物车
        /// </summary>
        /// <param name="shoppingCartIdList">Id集合</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelShoppingCartById(string shoppingCartIdList)
        {
            try
            {
                //商品Id
                string[] strArray = shoppingCartIdList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray != null && strArray.Length > 0)
                {
                    int[] intArray = Array.ConvertAll<string, int>(strArray, s => int.Parse(s));
                    if (intArray != null && intArray.Length > 0)
                    {
                        var request = _container.Resolve<OperationJuMeiMall>();
                        var checkSession = RediToLogin(1);
                        if (checkSession.Code == 110)
                        {
                            return Json(checkSession);
                        }
                        //string OpenId = Session["openid"].ToString();
                        var model = request.DelShoppingCartById(intArray);
                        return Json(model);
                    }
                    return Json(new ResultMessage() { Code = -1, Msg = "商品清除获取失败，请重试" });
                }
                else
                {
                    return Json(new ResultMessage() { Code = -1, Msg = "购物车商品号为空，请重试" });
                }


            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Info(this.GetType().ToString(), string.Format(ex.Message, ex.Source, ex.StackTrace));
                return Json(new ResultMessage() { Code = -1, Msg = "商品清除获取失败，请稍后再试，若多次失败请联系管理员" });
            }

        }

        /// <summary>
        /// 获取订单的商品Id
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCommidityId(string orderNo)
        {
            var checkSession = RediToLogin(2);
            if (checkSession.Code == 110)
            {
                return Json(checkSession);
            }
            //var openId = Session["openid"].ToString();
            var request = _container.Resolve<OperationJuMeiMall>();
            var recordSet = request.GetCommidityIdList(orderNo);
            if (recordSet != null && recordSet.Length > 0)
            {

                return Json(recordSet);
            }


            return Json(new ResultMessage() { Code = -1, Msg = "商品号为空，请联系管理员或重新下单" });




        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public ResultMessage RediToLogin(int state)
        {
            //Request.Cookies

            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].ToString() == "")
            {
                return new ResultMessage() { Code = 110, Msg = "/Login/Index" };//跳转
            }
            //if (Session[SessionKeys.USERID] == null || Session[SessionKeys.USERID].ToString() == "")
            //{
            //    return new ResultMessage() { Code = 110, Msg = "/Login/Index" };//跳转
            //}
            return new ResultMessage() { Code = 0, Msg = "" };//不跳转


        }
        [HttpPost]
        public ActionResult GetCommodityByCategory(int categoryId)
        {
            var model = new ListCommodityByCategorys();
            var cs = ICommodityCategorysService.GetById(categoryId);
            if (cs == null)
                return null;
            var site = ISiteService.Query(new SiteRequest()).FirstOrDefault();
            if (site != null)
                model.ImageUrl = site.imageUrl;
            model.CategoryName = cs.name;
            CommodityRequest request = new CommodityRequest();
            request.commodityCategoryId = categoryId;
            var data = ICommodityService.Query(request);
            if (data != null && data.ModelList != null)
            {
                model.List = data.ModelList.Select(x => new ListCommodityByCategory()
                {
                    commodityId = x.commodityId,
                    commodityName = x.commodityName,
                    commodityPrice = x.commodityPrice,
                    commodityRemark = x.commodityRemark,
                    sellQuantity = x.sellQuantity,
                    ImgUrl = x.images
                }).ToList();
            }
            return Json(model);
        }

        /// <summary>
        /// 支付宝支付
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlipayOrder(string orderNo)
        {
            IAopClient client = new DefaultAopClient(AlipayConfig.URL, AlipayConfig.APPID, AlipayConfig.APP_PRIVATE_KEY, "json", "1.0", "RSA", AlipayConfig.ALIPAY_PUBLIC_KEY, AlipayConfig.CHARSET, true);
            ////实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.Body = "我是测试数据";
            model.Subject = "App支付测试DoNet";
            model.TotalAmount = "0.01";
            model.ProductCode = "QUICK_MSECURITY_PAY";
            model.OutTradeNo = "sadfa" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            // model.TimeoutExpress = "30m";        
            request.SetBizModel(model);
            request.SetNotifyUrl(AlipayConfig.NOTIFY_URL);
            //这里和普通的接口调用不同，使用的是sdkExecute

            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            return Json(new ResultMessage() { Code = 0, Msg = HttpUtility.HtmlEncode(response.Body) });
            //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
            // Response.Write(HttpUtility.HtmlEncode(response.Body));
            //log4netHelper.Info(string.Format("支付宝订单参数：{0}", HttpUtility.HtmlDecode(response.Body)));
            //bool flag = AlipaySignature.RSACheckV1(GetRequestPost(), AlipayConfig.APP_PUBLIC_KEY, AlipayConfig.CHARSET, "RSA", false);
            //log4netHelper.Info(string.Format("验签结果：{0}", flag));
            //return Json(new Result(0, HttpUtility.HtmlEncode(response.Body)));
            //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
        }


        public ActionResult SecondKill()
        {
            var request = new SecondKillModel();

            var set=SecondKillSetService.GetFirst();
            if (set != null)
            {
                request.endTime = set.endTime;
                request.startTime = set.startTime;
                request.state = set.state;
            }
            var list = SecondKillCommoditysService.Query();
            if (list != null)
                request.ListCommodity = list.Select(x => 
                    new SecondKillCommodityss() 
                    { 
                      commodityId = x.commodityId,
                      id=x.id,
                      commodityName=x.commodityName,
                      price=x.price,
                      surplusNum=x.surplusNum,
                      yPrice=x.commodityPrice,
                      img = GetFirstImage(x.images)
                    }).ToList();

            return View(request);
        }

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

        [HttpPost]
        public ActionResult SecondKillGDtlOrder(int id)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].ToString() == "")
            {
                return Json(new ResultMessage() { Code = -2, Msg = "/Login/Index" });//跳转
            }
            int userId = Convert.ToInt32(Request.Cookies[SessionKeys.USERID].Value);
            var request = _container.Resolve<OperationJuMeiMall>();
            var result = request.SecondKillGDtlOrder(id, userId);
            return Json(result);
        }
    }
}
