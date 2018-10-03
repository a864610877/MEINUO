using Ecard;
using Ecard.Models;
using Ecard.Requests;
using Ecard.Services;
using KodyCRM.Utility.QrCode;
using MicroMall.Models;
using MicroMall.Models.Orders;
using MicroMall.Models.PersonalCentre;
using MicroMall.Models.UserAddresss;
using MicroMall.Models.Withdraws;
using Microsoft.Practices.Unity;
using Senparc.Weixin.MP.AppStore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using WxPayAPI;
using System.Transactions;
using Common;
using Newtonsoft.Json;

namespace MicroMall.Controllers
{
    public class PersonalCentreController : Controller
    {
        //
        // GET: /PersonalCentre/

        public IAccountService IAccountService { get; set; }
        public IGradesService IGradesService { get; set; }
        public IRebateService IRebateService { get; set; }
        [Dependency, NoRender]
        public IOrderService IOrderService { get; set; }
        public IOrderDetailService IOrderDetailService { get; set; }
        public ICommodityService ICommodityService { get; set; }

        public IWithdrawService WithdrawService { get; set; }
        public IMessagesService IMessagesService { get; set; }
        public IOperationAmountLogsService OperationAmountLogsService { get; set; }
        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }

        private IUnityContainer _container { get; set; }
        [Dependency, NoRender]
        private ISiteService ISiteService { get; set; }
         [Dependency, NoRender]
        public IMembershipService MembershipService { get; set; }
        [Dependency, NoRender]
        public IUserAddressService UserAddressService { get; set; }
        [Dependency, NoRender]
        public ISecondKillCommoditysService ISecondKillCommoditysService { get; set; }

        public PersonalCentreController(IAccountService IAccountService, IGradesService IGradesService, IRebateService IRebateService, IUnityContainer _container,
            IOrderDetailService IOrderDetailService, ICommodityService ICommodityService, IOperationAmountLogsService OperationAmountLogsService,
            IWithdrawService WithdrawService, ISiteService ISiteService,IMessagesService IMessagesService)
        {
            this.IAccountService = IAccountService;
            this.IGradesService = IGradesService;
            this.IRebateService = IRebateService;
            this.IOrderDetailService = IOrderDetailService;
            this.ICommodityService = ICommodityService;
            this._container = _container;
            this.OperationAmountLogsService = OperationAmountLogsService;
            this.WithdrawService = WithdrawService;
            this.ISiteService = ISiteService;
            this.IMessagesService = IMessagesService;

        }

        public ActionResult Login(int id)
        {
            Session["accountId"] = id;
            return Content("成功");
        }
        public ActionResult Index(string code, string state)
        {
            try
            {
                var request = new PersonalIndex() { Code = 0 };
                if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
                {
                    return RedirectToAction("Index", "login");
                    //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
                }
                var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
                int userId = 0;
                int.TryParse(strId, out userId);
                if (userId <= 0)
                    return RedirectToAction("Index", "login");
                var user = MembershipService.GetUserById(userId);
                if (user == null)
                    return RedirectToAction("Index", "login");
                var account = IAccountService.GetByUserId(userId);
                if(account==null)
                    return Content("账号异常，请联系管理员！");

                //获取用户个人信息
                if (!string.IsNullOrEmpty(code))
                {
                    Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthAccessTokenResult result = null;
                    try
                    {
                        //var model = Iwx_interfaceService.GetModel(new KodyCRM.DomainModels.Query.Admin.wx_interfaceQuery());
                        result = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(WxPayConfig.APPID, WxPayConfig.APPSECRET, code);
                        OAuthUserInfo userInfo = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetUserInfo(result.access_token, result.openid);
                        user.DisplayName = userInfo.nickname;
                        user.Gender = userInfo.sex;
                        account.openID = userInfo.openid;
                        user.Photo = userInfo.headimgurl.Replace("/0", "/132");
                        MembershipService.UpdateUser(user);
                        IAccountService.Update(account);
                        
                    }
                    catch (Exception ex)
                    {
                        WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---出错:{0}---",ex.Message));
                        Session[SessionKeys.USERID] = null;
                        return Content("您拒绝了授权！");
                    }
                    if (result.errcode != ReturnCode.请求成功)
                    {
                        //return Json(new Result(-2, "请求失效,请重新进入购票"));
                        Session[SessionKeys.USERID] = null;
                        return Content("错误：" + result.errmsg);
                    }
                }
                request.photo = string.IsNullOrWhiteSpace(user.Photo) ? "" : user.Photo;
                request.name =user.DisplayName;
                request.grade = AccountGrade.GetName(account.grade);
                return View(request);
                //request.saleAmount = IAccountService.SaleAmount(user.accountId, 0);
                //request.rebateAmount = IRebateService.GetRebateAmount(user.accountId);
                //request.ID = user.accountId;
                //return Json(request);
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug("/PersonalCentreController/Index", ex.Message);
                Session[SessionKeys.USERID] = null;
                return Content("系统错误，请联系管理员！");
               // return Json(new ResultMessage() { Code = -1, Msg = "系统错误，请联系管理员" });
            }
        }

        public ActionResult MyOrder(int orderState=0, int pageIndex = 1)
        {
            try
            {
                var request = _container.Resolve<ListOrders>();
                if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
                {
                    return RedirectToAction("Index", "login");
                    //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
                }
                var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
                int userId = 0;
                int.TryParse(strId, out userId);
                if (userId <= 0)
                    return RedirectToAction("Index", "login");
                request.Code = 0;
                request.orderState = orderState;
                request.Query(pageIndex, userId);
                return View(request);
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug("/PersonalCentreController/MyOrder", ex.Message);
                return Json(new ResultMessage() { Code = -1, Msg = "系统错误，请联系管理员" });
            }
        }

        [HttpPost]
        public ActionResult CancelOrder(int orderId)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var order = IOrderService.GetById(orderId);
            if (order != null)
            {
                if (order.orderState != OrderStates.awaitPay)
                    return Json(new ResultMessage() { Code = -1, Msg = "订单不可撤销！" });
                transaction.BeginTransaction();
                order.orderState = OrderStates.cancel;
                if (order.orderType == OrderType.normal)
                {
                    var details = IOrderDetailService.GetAllByOrderNo(order.orderNo).ToList();
                    if (details != null && details.Count() > 0)
                    {
                        foreach (var detail in details)
                        {
                            var commodit = ICommodityService.GetById(detail.commodityId);
                            if (commodit != null)
                            {
                                commodit.commodityInventory += detail.quantity;
                                commodit.sellQuantity -= detail.quantity;
                                commodit.sellQuantity1 -= detail.quantity;
                                ICommodityService.Update(commodit);
                            }
                        }
                    }
                }
                else if (order.orderType == OrderType.secondKill)
                {
                    var details = IOrderDetailService.GetAllByOrderNo(order.orderNo).ToList();
                    if (details != null && details.Count() > 0)
                    {
                        foreach (var detail in details)
                        {
                            var secondKillCommodity = ISecondKillCommoditysService.GetBycommodityId(detail.commodityId);
                            if (secondKillCommodity != null)
                            {
                                secondKillCommodity.payNum -= detail.quantity;
                                secondKillCommodity.surplusNum += detail.quantity;
                                ISecondKillCommoditysService.Update(secondKillCommodity);
                            }
                        }
                    }
                }
                //var commoditys = IOrderDetailService.GetAllOrderId(order.orderId);
                //if (commoditys != null && commoditys.ToList().Count > 0)
                //{
                //    foreach (var item in commoditys.ToList())
                //    {
                //        ICommodityService.AddStorage(item.commodityId, item.quantity);
                //        ICommodityService.MinussellQuantity(item.commodityId, item.quantity);
                //    }
                //}
               // var account = IAccountService.GetById(order.userId);
                //if (!string.IsNullOrWhiteSpace(account.openID))
                //{
                //    var message = new Fz_Messages();
                //    message.accountId = account.accountId;
                //    message.keyword1 = order.orderNo;
                //    message.keyword2 = "已取消";
                //    message.msg = "";
                //    message.msgType = MsgType.orderState;
                //    message.openId = account.openID;
                //    message.state = MessagesState.staySend;
                //    message.submitTime = DateTime.Now;
                //    IMessagesService.Insert(message);
                //}
                IOrderService.Update(order);
                transaction.Commit();
                return Json(new ResultMessage() { Code = 0 });
            }
            return Json(new ResultMessage() { Code = -1, Msg = "订单不存在！" });

        }
        [HttpPost]
        public ActionResult ConfirmReceipt(int orderId)
        {
            try
            {
                if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
                {
                    return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
                }
                var order = IOrderService.GetById(orderId);
                if (order != null)
                {
                    if (order.orderState != OrderStates.shipped)
                        return Json(new ResultMessage() { Code = -1, Msg = "订单不可确认收货！" });
                    order.orderState = OrderStates.complete;
                    transaction.BeginTransaction();
                    if (IOrderService.Update(order) > 0)
                    {
                        var account = IAccountService.GetById(order.userId);
                        //if (!string.IsNullOrWhiteSpace(account.openID))
                        //{
                        //    var message = new Fz_Messages();
                        //    message.accountId = account.accountId;
                        //    message.keyword1 = order.orderNo;
                        //    message.keyword2 = "订单完成";
                        //    message.msg = "";
                        //    message.msgType = MsgType.orderState;
                        //    message.openId = account.openID;
                        //    message.state = MessagesState.staySend;
                        //    message.submitTime = DateTime.Now;
                        //    IMessagesService.Insert(message);
                        //}
                        transaction.Commit();
                       // IRebateService.Rebate(order.orderId);
                        return Json(new ResultMessage() { Code = 0 });
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug("/PersonalCentreController/ConfirmReceipt", ex.Message);
                return Json(new ResultMessage() { Code = -1,Msg=ex.Message });
            }
        }
        [HttpPost]
        public ActionResult MyTeam(AccountSaleRequest query)
        {
            try
            {
                var request = _container.Resolve<ListTeams>();
                if (Session["accountId"] == null || Session["accountId"].ToString() == "")
                {
                    return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html?state=4" });
                }
                var strId = Session["accountId"].ToString();
                int accountId = 0;
                int.TryParse(strId, out accountId);
                if (accountId <= 0)
                    return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html?state=4" });
                query.Id = accountId;
                request.Code = 0;
                request.Query(query);
                return Json(request);
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug("/PersonalCentreController/MyTeam", ex.Message);
                return Json(new ResultMessage() { Code = -1, Msg = "系统错误，请联系管理员" });
            }
        }
        [HttpPost]
        public ActionResult MyAmountOperationLog(OperationAmountLogUserIdRequest query)
        {
            try
            {
                var request = _container.Resolve<ListAmountOperationLogs>();
                if (Session["accountId"] == null || Session["accountId"].ToString() == "")
                {
                    return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html?state=4" });
                }
                var strId = Session["accountId"].ToString();
                int accountId = 0;
                int.TryParse(strId, out accountId);
                if (accountId <= 0)
                    return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html?state=4" });
                query.userId = accountId;
                request.Code = 0;
                request.Query(query);
                return Json(request);
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug("/PersonalCentreController/MyAmountOperationLog", ex.Message);
                return Json(new ResultMessage() { Code = -1, Msg = "系统错误，请联系管理员" });
            }
        }
        [HttpPost]
        public ActionResult EditAddress(AddUserAddress request)
        {
            try
            {
                if (Session["accountId"] == null || Session["accountId"].ToString() == "")
                {
                    return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html?state=5" });
                }
                var strId = Session["accountId"].ToString();
                int accountId = 0;
                int.TryParse(strId, out accountId);
                if (accountId <= 0)
                    return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html?state=5" });
                request.Save(accountId);
                return Json(new ResultMessage() { Code = 0, Msg = "" });
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug("/PersonalCentreController/EditAddress", ex.Message);
                return Json(new ResultMessage() { Code = -1, Msg = "系统错误，请联系管理员" });
            }
        }
        
        public ActionResult MyAddress(UserAddressRequest request)
        {
            if (Request.Cookies[SessionKeys.USERID] == null)
                return RedirectToAction("Index","login");
            int userId = Convert.ToInt32(Request.Cookies[SessionKeys.USERID].Value);
            var q = _container.Resolve<ListAddresss>();
            q.Query(userId);
            return View(q);
        }

        [HttpPost]
        public ActionResult GetAddress(int id)
        {
            var request = _container.Resolve<AddUserAddress>();
            request.Ready(id);
            return Json(request);
        }
        [HttpPost]
        public ActionResult DelAddress(int id)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var request = _container.Resolve<AddUserAddress>();
            if (request.Del(id))
                return Json(new ResultMessage() { Code = 0 });
            return Json(new ResultMessage() { Code = -1, Msg = "删除失败" });
        }

        /// <summary>
        /// 获取用户余额
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserBalance()
        {
            decimal UserBalance = 0;
            var checkSession = RediToLogin(2);
            if (checkSession.Code == 110)
            {
                return Json(checkSession);
            }
            string OpenId = Session["openid"].ToString();
            var user = IAccountService.GetByopenID(OpenId);
            if (user != null)
            {
                UserBalance = user.amount;
                return Json(UserBalance);
            }
            return Json(new ResultMessage() { Code = -1, Msg = "网络错误，获取用户余额，请重试" });

        }

        /// <summary>
        /// 提交提现申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostWithdrawBalance(decimal withdrawAmt)
        {
            try
            {
                transaction.BeginTransaction();
                if (withdrawAmt == 0)
                {
                    return Json(new ResultMessage() { Code = -1, Msg = "金额不能为空或为0" });
                }
                if (withdrawAmt < 1)
                {
                    return Json(new ResultMessage() { Code = -1, Msg = "提现金额不能小于1元" });
                }

                var checkSession = RediToLogin(2);
                if (checkSession.Code == 110)
                {
                    return Json(checkSession);
                }
                string OpenId = Session["openid"].ToString();
                var user = IAccountService.GetByopenID(OpenId);
                if (user != null)
                {
                    if (user.amount < withdrawAmt)
                    {
                        return Json(new ResultMessage() { Code = -1, Msg = "余额不足" });
                    }
                    var withdrawModel = new Withdraw();
                    withdrawModel.amount = withdrawAmt;
                    withdrawModel.openId = OpenId;
                    withdrawModel.state = WithdrawStates.notaudit;
                    withdrawModel.userId = user.accountId;
                    withdrawModel.submitTime = DateTime.Now;
                    WithdrawService.Insert(withdrawModel);

                    var OperationAmountLogsModel = new fz_OperationAmountLogs();
                    OperationAmountLogsModel.amount = withdrawAmt;
                    OperationAmountLogsModel.userId = user.accountId;
                    OperationAmountLogsModel.type = OperationAmountType.expenditure;
                    OperationAmountLogsModel.category = OperationAmountCategory.withdraw;
                    OperationAmountLogsModel.submitTime = DateTime.Now;
                    OperationAmountLogsService.Insert(OperationAmountLogsModel);

                    user.amount = user.amount - withdrawAmt;
                    user.submitTime = DateTime.Now;
                    IAccountService.Update(user);


                    if (!string.IsNullOrWhiteSpace(OpenId))
                    {
                        var message = new Fz_Messages();
                        message.accountId = user.accountId;
                        message.openId = user.openID;
                        message.state = MessagesState.staySend;
                        message.submitTime = DateTime.Now;
                        message.msg = "你好,提现申请已经收到,待审核。";
                        message.keyword1 = user.name;
                        message.keyword2 = withdrawModel.submitTime.ToString();
                        message.keyword3 = withdrawModel.amount.ToString();
                        message.keyword4 = "微信";
                        //message.keyword5 = DateTime.Now.ToString();
                        message.msgType = MsgType.withdrawNotice;
                        IMessagesService.Insert(message);

                        transaction.Commit();
                        return Json(new ResultMessage() { Code = 0, Msg = "成功申请提现，前往查看提现状态" });
                    }

                    return Json(new ResultMessage() { Code = -1, Msg = "提现申请失败，请联系管理员" });
                    
                    
                }
                return Json(new ResultMessage() { Code = -1, Msg = "提现申请失败，请联系管理员" });
            }
            catch (Exception ex)
            {

                WxPayAPI.Log.Debug("/PersonalCentreController/PostWithdrawBalance", ex.Message);
                return Json(new ResultMessage() { Code = -1, Msg = "系统错误，请联系管理员" });
            }


        }

        /// <summary>
        /// 获取会员提现列表
        /// </summary>
        /// <param name="request">PageIndex</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWithdrawDetails(UserWithdrawRequest request)
        {
            var checkSession = RediToLogin(2);
            var WithdrawDtlList = new WithdrawDtls();

            if (checkSession.Code == 110)
            {
                return Json(checkSession);
            }
            string OpenId = Session["openid"].ToString();
            var recordSet = WithdrawService.Query(request, OpenId);
            if (recordSet != null && recordSet.ModelList != null)
            {
                WithdrawDtlList.WithdrawDtlList = recordSet.ModelList.Select(x => new WithdrawDtl()
                {
                    amount = x.amount.ToString(),
                    status = WithdrawStates.GetName(x.state),
                    Time = x.submitTime.ToString()

                }).ToList();
                WithdrawDtlList.TotalCount = recordSet.TotalCount;
                return Json(WithdrawDtlList);
            }
            return Json(new ResultMessage() { Code = -1, Msg = "系统错误，请联系管理员" });


        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public ResultMessage RediToLogin(int state)
        {
            if (Session["openid"] == null || Session["openid"].ToString() == "")
            {
                return new ResultMessage() { Code = 110, Msg = "/WeChatViews/login.html?state=" + state };//跳转
            }
            return new ResultMessage() { Code = 0, Msg = "" };//不跳转


        }

        [HttpPost]
        public ActionResult MyQrCode()
        {
            var aa = WxPayConfig.APPID;
            var site = ISiteService.Query(null).FirstOrDefault();
            if(site==null)
                return Json(new ResultMessage() { Code = -1, Msg = "获取二维码失败" });
            if (Session["accountId"] == null || Session["accountId"].ToString() == "")
            {
                return Json(new ResultMessage() { Code = -2, Msg ="/WeChatViews/login.html" });
            }
            var strId = Session["accountId"].ToString();
            int accountId = 0;
            int.TryParse(strId, out accountId);
            if (accountId <= 0)
                return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html" });
             var account = IAccountService.GetById(accountId);
             if (account == null) {
                 return Json(new ResultMessage() { Code = -2, Msg ="/WeChatViews/login.html" });
             }
             if (System.IO.File.Exists(Server.MapPath(account.qrCodeUrl)))
             {
                 return Json(new ResultMessage() { Code = 0, Msg = account.qrCodeUrl });
             }
             else
             {
                 string url = site.Url + "/WeChatViews/registered.html?id=" + account.accountId+"&dd";// + HttpUtility.UrlEncode("id=" + account.userId + "");
                 WxPayAPI.Log.Debug("qrcode-"+account.accountId+":", url);
                 var ms = new MemoryStream();
                 if (QRCodeHelper.GetQRCode(url, ms))
                 {
                     Image image = Image.FromStream(ms);
                     string path = "/QrCodeImages/" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                     image.Save(Server.MapPath(path));
                     account.qrCodeUrl = path;
                     IAccountService.Update(account);
                     return Json(new ResultMessage() { Code = 0, Msg = account.qrCodeUrl });
                 }
             }
             return Json(new ResultMessage() { Code = -1, Msg = "获取二维码失败"});
        }

        /// <summary>
        /// 设置默认地址Id
        /// </summary>
        /// <param name="id">地址Id</param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult SetDefAdd(int id)
        {
            if (Session["accountId"] == null || Session["accountId"].ToString() == "")
            {
                return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html?state=5" });
            }
            var strId = Session["accountId"].ToString();
            int accountId = 0;
            int.TryParse(strId, out accountId);
            var request = _container.Resolve<AddUserAddress>();
            if (request.SetDefAddress(id, accountId))
                return Json(new ResultMessage() { Code = 0 });
            return Json(new ResultMessage() { Code = -1, Msg = "设置默认地址失败" });

        }


        public ActionResult AddAddress()
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return RedirectToAction("Index", "login");
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            //var strId = Session[SessionKeys.USERID].ToString();
            //int userId = 0;
            //int.TryParse(strId, out userId);
            //if (userId <= 0)
            //    return RedirectToAction("Index", "login");
            //var user = MembershipService.GetUserById(userId);
            //if (user == null)
            //    return RedirectToAction("Index", "login");

            return View();
        }

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="recipients">收件人</param>
        /// <param name="mobile">电话</param>
        /// <param name="detailAddress">地址</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAddress(string recipients, string mobile, string expressArea, string detailAddress)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
           
            var request = _container.Resolve<AddUserAddress>();
            if (request.AddAddress(recipients, mobile, expressArea, detailAddress, userId))
                return Json(new ResultMessage() { Code = 0 });
            return Json(new ResultMessage() { Code = -1, Msg = "添加地址失败" });
        }



        public ActionResult EditAddres(int id)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return RedirectToAction("Index", "login");
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var addres = UserAddressService.GetById(id);
            return View(addres);
        }

        /// <summary>
        /// 修改地址
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="mobile"></param>
        /// <param name="detailAddress"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAddres(string recipients, string mobile, string expressArea, string detailAddress, int id)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
            var request = _container.Resolve<AddUserAddress>();
            if (request.MdyAddress(recipients, mobile, expressArea,detailAddress, id))
                return Json(new ResultMessage() { Code = 0 });
            return Json(new ResultMessage() { Code = -1, Msg = "修改地址失败" });

        }

        /// <summary>
        /// 绑定手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ActionResult BindMobile(string mobile)
        {
            if (Session["accountId"] == null || Session["accountId"].ToString() == "")
            {
                return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html?state=2" });
            }
            var strId = Session["accountId"].ToString();
            int accountId = 0;
            int.TryParse(strId, out accountId);
            if (accountId <= 0)
                return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html?state=2" });
            var user = IAccountService.GetById(accountId);
            if (user == null)
                return Json(new ResultMessage() { Code = -2, Msg = "/WeChatViews/login.html?state=2" });
            user.mobile = mobile;
            if (IAccountService.Update(user) > 0)
            {
                return Json(new ResultMessage() { Code = 0, Msg = "" });
            }
            return Json(new ResultMessage() { Code = -1, Msg = "绑定失败，请联系管理员！" });
        }

        public ActionResult UpdatePassword()
        {
            if (Request.Cookies[SessionKeys.USERID] == null)
                return RedirectToAction("Index", "login");
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePassword(string oldPwd,string newPwd)
        {
            if (Request.Cookies[SessionKeys.USERID] == null)
                return Json(new ResultMessage() { Code = -1, Msg = "请重新登录" });
            if (string.IsNullOrWhiteSpace(oldPwd))
                return Json(new ResultMessage() { Code = -1, Msg = "请输入旧密码" });
            if (string.IsNullOrWhiteSpace(newPwd))
                return Json(new ResultMessage() { Code = -1, Msg = "请输入新密码" });
            int userId = Convert.ToInt32(Request.Cookies[SessionKeys.USERID].Value);
            var user = MembershipService.GetUserById(userId);
            if (user != null)
            {
                 if(user.Password!=user.GetPassword(oldPwd))
                     return Json(new ResultMessage() { Code = -1, Msg = "旧密码错误！" });
                 user.SetPassword(newPwd);
                 MembershipService.UpdateUser(user);
                 return Json(new ResultMessage() { Code = 0, Msg = "修改成功" });
            }
            else
            {
                return Json(new ResultMessage() { Code = -1, Msg = "请重新登录" });
            }
            
        }

        /// <summary>
        /// 会员中心
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalInfo()
        {
            try
            {
                var request = new PersonalIndexExpress() { Code = 0 };
                if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
                {
                    return RedirectToAction("Index", "login");
                    //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
                }
                var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
                int userId = 0;
                int.TryParse(strId, out userId);
                if (userId <= 0)
                    return RedirectToAction("Index", "login");
                var user = MembershipService.GetUserById(userId);
                if (user == null)
                    return RedirectToAction("Index", "login");
                var account = IAccountService.GetByUserId(userId);
                if (account == null)
                    return Content("账号异常，请联系管理员！");
                var userAddreno = UserAddressService.GetById(account.defaultAddressId);

                request.photo = string.IsNullOrWhiteSpace(user.Photo) ? "" : user.Photo;
                request.name = user.DisplayName;
                request.grade = AccountGrade.GetName(account.grade);
                request.presentExp = account.presentExp;
                request.Genders = user.Gender.HasValue? user.Gender.Value==0? "其他" : user.Gender.Value==1?"男":"女":"其他";
                request.orangeKey = account.orangeKey;
                request.Mobile = user.Mobile;
                request.Email = user.Email;
                request.activatePoint = account.activatePoint;
                request.detailaddress = userAddreno!=null? userAddreno.detailedAddress:"";
                return View(request);
                //request.saleAmount = IAccountService.SaleAmount(user.accountId, 0);
                //request.rebateAmount = IRebateService.GetRebateAmount(user.accountId);
                //request.ID = user.accountId;
                //return Json(request);
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug("/PersonalCentreController/Index", ex.Message);
                return Content("系统错误，请联系管理员！");
                // return Json(new ResultMessage() { Code = -1, Msg = "系统错误，请联系管理员" });
            }

            
        }

        [HttpPost]
        public ActionResult UpdatePersonalInfo(string user,int sex,string email,string addreno)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return RedirectToAction("Index", "login");
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
            if (userId <= 0)
                return RedirectToAction("Index", "login");
            var userModel = MembershipService.GetUserById(userId);
            if (user == null)
                return RedirectToAction("Index", "login");
            var account = IAccountService.GetByUserId(userId);
            if (account == null)
                return Content("账号异常，请联系管理员！");
            userModel.Gender = sex;
            userModel.DisplayName = user;
            userModel.Email = email;
            int userAddressId = account.defaultAddressId;
            UserAddress address= UserAddressService.GetById(userAddressId);
            int state = -1; 
            if(address!=null)
            {
                address.detailedAddress = addreno;
                address.userId = userId;
                state = 0;
            }
            else
            {
                state = 1;
                address = new UserAddress()
                {
                    detailedAddress = addreno
                };
            }

            int defaultAddreId = 0;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if(state==0)
                    {
                        UserAddressService.Update(address);
                    }
                    else
                    {
                        defaultAddreId= UserAddressService.Insert(address);
                        account.defaultAddressId = defaultAddreId;
                        IAccountService.Update(account);
                    }
                    
                    MembershipService.UpdateUser(userModel);
                    scope.Complete();
                }
                return Json(new ResultMessage() { Code = 0, Msg = "添加成功" });
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug("/PersonalCentreController/Index", ex.Message);
                return Content("系统错误，请联系管理员！");
                throw;
            }
            
            

        }


        public ActionResult MyQR()
        {
            MyCode model = new MyCode() ;
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return RedirectToAction("Index", "login");
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site == null)
                return Json(new ResultMessage() { Code = -1, Msg = "获取二维码失败" });
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
            if (userId <= 0)
                return RedirectToAction("Index", "login");
            var user = MembershipService.GetUserById(userId);
            if (user == null)
                return RedirectToAction("Index", "login");
            var account = IAccountService.GetByUserId(userId);
            if (account == null)
                return Content("账号异常，请联系管理员！");
            model.CodeUrl =string.IsNullOrWhiteSpace(account.qrCodeUrl)?"/images/PosterBack.jpg":account.qrCodeUrl+"?"+DateTime.Now.ToString("yyyyMMddHHmmssffff");
            return View(model);
        }

        public ActionResult CodeBgImg()
        {

            //int userId = ((Models.Users.User)Session["user"]).Id;
            ////生成二维码对象   
            //CodeImg model = new CodeImg();
            //model.url = new QrCodeApi().GetQrcode(Appid, Appsecret, userId.ToString());
             CodeImg img = new CodeImg();
             if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return RedirectToAction("Index", "login");
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site == null)
                return Json(new ResultMessage() { Code = -1, Msg = "获取二维码失败" });
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
            if (userId <= 0)
                return RedirectToAction("Index", "login");
            var user = MembershipService.GetUserById(userId);
            if (user == null)
                return RedirectToAction("Index", "login");
            var account = IAccountService.GetByUserId(userId);
            if (account == null)
                return Content("账号异常，请联系管理员！");
            if (System.IO.File.Exists(Server.MapPath(account.qrCodeUrl)))
            {
                //img.url = account.qrCodeUrl;
                //return View(img);
            }
            else
            {

                // WxPayAPI.Log.Debug("qrcode-" + account.accountId + ":", url);
                //var ms = new MemoryStream();
                //if (QRCodeHelper.GetQRCode(url, ms))
                //{
                //    Image image = Image.FromStream(ms);
                //    string path = "/QrCodeImages/" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                //    image.Save(Server.MapPath(path));
                //    account.qrCodeUrl = path;
                //    IAccountService.Update(account);
                //}
                //img.url = account.qrCodeUrl;
            }
            string url = site.Url + "/Regists/Regist?orangeKey=" + account.orangeKey + "&dd";// + HttpUtility.UrlEncode("id=" + account.userId + "");
            var ms = new MemoryStream();
            if (QRCodeHelper.GetQRCode(url, ms))
            {
                Image image = Image.FromStream(ms);
                string path = "/QrCodeImages/" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                image.Save(Server.MapPath(path));
                //account.qrCodeUrl = path;
                //IAccountService.Update(account);
                img.url = path;
            }
            //return File(ms.ToArray(), "image/jpeg");  
           
            return View(img);
        }

        public ActionResult QrCode()
        {

            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return null;
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }

            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
            if (userId <= 0)
                return null;
            var user = MembershipService.GetUserById(userId);
            if (user == null)
                return null;
            var account = IAccountService.GetByUserId(userId);
            if (account == null)
                return null;
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site == null)
                return Json(new ResultMessage() { Code = -1, Msg = "获取二维码失败" });
            string url = site.Url + "/Regists/Regist?orangeKey=" + account.orangeKey + "&dd";// + HttpUtility.UrlEncode("id=" + account.userId + "");
            var ms = new MemoryStream();
            if (QRCodeHelper.GetQRCode(url, ms))
            {
                Image image = Image.FromStream(ms);
                string path = "/QrCodeImages/" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                image.Save(Server.MapPath(path));
                //account.qrCodeUrl = path;
                //IAccountService.Update(account);
                return File(ms.ToArray(), "image/jpeg"); 
            }
            return null;
           
        }

        public ActionResult UploadImg()
        {
            Map map = new Map();
            string data = Request["data"];
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].ToString() == "")
            {
                return RedirectToAction("Index", "login");
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site == null)
                return Json(new ResultMessage() { Code = -1, Msg = "获取二维码失败" });
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
            if (userId <= 0)
                return RedirectToAction("Index", "login");
            var user = MembershipService.GetUserById(userId);
            if (user == null)
                return RedirectToAction("Index", "login");
            var account = IAccountService.GetByUserId(userId);
            if (account == null)
                return Content("账号异常，请联系管理员！");

            string id = System.Guid.NewGuid().ToString().Replace("-", "");
            string url = decodeBase64ToImage(data, id);
            if (url != "")
            {
                account.qrCodeUrl = url;
                int result = IAccountService.Update(account);
                if (result>0)
                {
                    map.Add("message", "保存成功！");
                    map.Add("state", "success");
                }
                else
                {
                    map.Add("message", "保存失败，请与客服联系！");
                    map.Add("state", "error");
                }
            }
            return Content(JsonConvert.SerializeObject(map));
        }

        public ActionResult DeleteCodeImg(string id)
        {
            Map map = new Map();
            if (Session[SessionKeys.USERID] == null || Session[SessionKeys.USERID].ToString() == "")
            {
                return RedirectToAction("Index", "login");
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site == null)
                return Json(new ResultMessage() { Code = -1, Msg = "获取二维码失败" });
            var strId = Session[SessionKeys.USERID].ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
            if (userId <= 0)
                return RedirectToAction("Index", "login");
            var user = MembershipService.GetUserById(userId);
            if (user == null)
                return RedirectToAction("Index", "login");
            var account = IAccountService.GetByUserId(userId);
            if (account == null)
                return Content("账号异常，请联系管理员！");
            account.qrCodeUrl = "";
            int result = IAccountService.Update(account);
            if (result>0)
            {
                map.Add("message", "删除成功！");
                map.Add("state", "success");
            }
            else
            {
                map.Add("message", "删除失败，请与客服联系！");
                map.Add("state", "error");
            }
            return Content(JsonConvert.SerializeObject(map));
        }

        [HttpPost]
        public ActionResult CreatePoster()
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return Json(new ResultMessage() { Code = -1, Msg = "请重新登录" });
            }
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site == null)
                return Json(new ResultMessage() { Code = -1, Msg = "配置异常，请联系管理员" });
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
            if (userId <= 0)
                return Json(new ResultMessage() { Code = -1, Msg = "请重新登录" });
            var user = MembershipService.GetUserById(userId);
            if (user == null)
                return Json(new ResultMessage() { Code = -1, Msg = "账号异常，请联系管理员" });
            var account = IAccountService.GetByUserId(userId);
            if (account == null)
                return Json(new ResultMessage() { Code = -1, Msg = "账号异常，请联系管理员" });
            string url = site.Url + "/Regists/Regist?orangeKey=" + account.orangeKey + "&dd";// + HttpUtility.UrlEncode("id=" + account.userId + "");
            string path="";
            var ms = new MemoryStream();
            if (QRCodeHelper.GetQRCode(url, ms))
            {
                Image image = Image.FromStream(ms);
                path = "/QrCodeImages/Qr/" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                image.Save(Server.MapPath(path));
            }
            Image imgBack = System.Drawing.Image.FromFile(Server.MapPath("/images/PosterBack.jpg"));     //相框图片  
            Image img = System.Drawing.Image.FromFile(Server.MapPath(path));
            Bitmap map = new Bitmap(imgBack, new Size(imgBack.Width, imgBack.Height));//照片图片
            Graphics g = Graphics.FromImage(map);
            g.DrawImage(img, 370, 970, 235, 240);
            //g.DrawString(name.value, name.font, name.brushes, name.x, name.y);
            //g.DrawString(mobile.value, mobile.font, mobile.brushes, mobile.x, mobile.y);
            g.Flush();
            GC.Collect();
            var stream = new System.IO.MemoryStream();
            map.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            System.Drawing.Image ResourceImage = System.Drawing.Image.FromStream(stream);
            string path1 = "/QrCodeImages/Poster/" + user.UserId + ".jpg";
            ResourceImage.Save(Server.MapPath(path1));
            account.qrCodeUrl = path1;
            IAccountService.Update(account);
            return Json(new ResultMessage() { Code = 0, Msg = path1 });
        }

        /// <summary>
        /// 图片上传 Base64解码
        /// </summary>
        /// <param name="dataURL">Base64数据</param>
        /// <param name="path">保存路径</param>
        /// <param name="imgName">图片名字</param>
        /// <returns>返回一个相对路径</returns>
        public string decodeBase64ToImage(string dataURL, string imgName)
        {

            string path = "/QrCodeImages/";//图片上传目录
            string filename = "";//声明一个string类型的相对路径

            String base64 = dataURL.Substring(dataURL.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除

            System.Drawing.Bitmap bitmap = null;//定义一个Bitmap对象，接收转换完成的图片



            try//会有异常抛出，try，catch一下
            {



                String inputStr = base64;//把纯净的Base64资源扔给inpuStr,这一步有点多余

                byte[] arr = Convert.FromBase64String(inputStr);//将纯净资源Base64转换成等效的8位无符号整形数组

                System.IO.MemoryStream ms = new System.IO.MemoryStream(arr);//转换成无法调整大小的MemoryStream对象
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);//将MemoryStream对象转换成Bitmap对象
                ms.Close();//关闭当前流，并释放所有与之关联的资源
                bitmap = bmp;
                filename = path + "/" + imgName + "_code.png";//所要保存的相对路径及名字
                string tmpRootDir = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString()); //获取程序根目录 
                string imagesurl2 = tmpRootDir + filename.Replace(@"/", @"\"); //转换成绝对路径 
                bitmap.Save(imagesurl2, System.Drawing.Imaging.ImageFormat.Png);//保存到服务器路径
            }
            catch (Exception)
            {
            }
            return filename;//返回相对路径
        }


        public Bitmap GetImageFromBase64(string base64string)
        {
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }
        public string GetBase64FromImage(string imagefile)
        {
            string strbaser64 = "";
            try
            {
                Bitmap bmp = new Bitmap(imagefile);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                strbaser64 = Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }
            return strbaser64;
        }
    }
}
