using Ecard;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc;
using Ecard.Mvc.ActionFilters;
using Ecard.Services;
//using Ecard.Mvc.ActionFilters;
using log4net;
using log4net.Config;
using MicroMall.Models;
using MicroMall.Models.InvestGames;
using Microsoft.Practices.Unity;
using Moonlit.Data;
using Moonlit.Validations;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using WxPayAPI;

namespace MicroMall
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            EndRequest += new EventHandler(MvcApplication_EndRequest);
        }

        void MvcApplication_EndRequest(object sender, EventArgs e)
        {
            IUnityContainer container = (IUnityContainer)Application["container"];
            if (container != null)
            {
                using (var instance = this.Context.Items[Constants.KeyOfDatabaseInstance] as IDisposable)
                {

                }
            }
        }
        private  Timer _timer;
        private ILog log;
        System.Timers.Timer messageTimer;
        System.Timers.Timer AutomaticTimer;
        /// <summary>
        /// 取消订单
        /// </summary>
        System.Timers.Timer AutoOrderCancelmaticTimer;

        private IMessagesService IMessagesService { get; set; }
        private ISiteService ISiteService { get; set; }
        public IOrderService IOrderService { get; set; }
        public IOrderDetailService IOrderDetailService { get; set; }

        public ICommodityService ICommodityService { get; set; }

        public ISecondKillCommoditysService ISecondKillCommoditysService { get; set; }

        public DatabaseInstance _databaseInstance { get; set; }
        public IRebateService IRebateService { get; set; }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            OnStart();
        }
        private IUnityContainer getContainer()
        {
            return (IUnityContainer)Application["container"];
        }
        private void OnStart()
        {
            //var config = this.Context.Server.MapPath("~/log4net.config");
            //FileInfo repository = new FileInfo(config);
            //XmlConfigurator.ConfigureAndWatch(repository);

            log = LogManager.GetLogger("app");
            DatabaseInstance.SqlStore = new DirectorySqlStore(Context.Server.MapPath("~/sql"));
            log.Debug("sql store updated");
            SetupContiner();
            log.Debug("SetupContiner");

            //SetupSite();
            //log.Debug("SetupSite");

            //RegisterRoutes();
            //log.Debug("RegisterRoutes");

            registerActionFilters();
            log.Debug("registerActionFilters");

            //registerModelBinders();
            //log.Debug("registerModelBinders");

            //registerViewEngines();
            //log.Debug("registerViewEngines");

            registerControllerFactory();
            log.Debug("registerControllerFactory");

            //_time = new Timer(OnTime, null, TimeSpan.FromMinutes(5));
           
            //BuildSiteCss();
            //log.Debug("BuildSiteCss");

            DependencyResolver.SetResolver(new UnityDependencyResolver((IUnityContainer)Application["container"]));
            log.Debug("SetResolver");
            //DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MobileAttribute), typeof(MobileAttributeAdapter));
            //log.Debug("RegisterAdapter");
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
            log.Debug("ValueProviderFactories.Factories.Add(new JsonValueProviderFactory())");
            
            OnTimer(null);
            //int time = 60 * 5 * 1000;
            //_timer = new Timer(OnTimer, null, 0, time);

            var container = getContainer();
            _databaseInstance = container.Resolve<DatabaseInstance>();
            IOrderService = container.Resolve<IOrderService>();
            IOrderDetailService = container.Resolve<IOrderDetailService>();
            ICommodityService = container.Resolve<ICommodityService>();
            ISecondKillCommoditysService = container.Resolve<ISecondKillCommoditysService>();



            #region 开启会员提醒
            //messageTimer = new System.Timers.Timer();
            //messageTimer.Interval = 1000*30;
            //messageTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnMessageTimer); //到达时间的时候执行事件；
            //messageTimer.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            //messageTimer.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；
            //messageTimer.Start();
            #endregion
            #region 开启自动收货
            //AutomaticTimer = new System.Timers.Timer();
            //AutomaticTimer.Interval = 1000 * 60*60*6;
            //AutomaticTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnMessageTimer); //到达时间的时候执行事件；
            //AutomaticTimer.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            //AutomaticTimer.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；
            //AutomaticTimer.Start();
            #endregion
            #region 开启自动取消订单
            AutoOrderCancelmaticTimer = new System.Timers.Timer();
            AutoOrderCancelmaticTimer.Interval = 1000 * 30;
            AutoOrderCancelmaticTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnOrderCancelTimer); //到达时间的时候执行事件；
            AutoOrderCancelmaticTimer.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            AutoOrderCancelmaticTimer.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；
            AutoOrderCancelmaticTimer.Start();
            #endregion
        }

        private void SetupContiner()
        {
            var container = new ContainerFactory().GetEcardContainer();
            //container.RegisterType<ISiteCssBuilder, DashboardItemSiteCssBuilder>("dashboarditem");
            container.RegisterType<IControllerFinder, AppDomainControllerFinder>(new ContainerControlledLifetimeManager());
            container.RegisterInstance<Database>(new Database("ecard"));
            container.RegisterType<DatabaseInstance>(new PerWebRequestLifetimeManager(Constants.KeyOfDatabaseInstance));
            container.RegisterType<IAuthenticateService, UserAndPasswordAuthenticateService>("password");
            //container.RegisterType<IAuthenticateService, UserAndPasswordAndIKeyAuthenticateService>("ikeyandpassword");
            //container.RegisterType<IPasswordService, NonePasswordService>("none");
            // delete for publish source code
            //container.RegisterType<IPasswordService, SLE902rPasswordService>("sle902r");
            //container.RegisterType<IPrinterService, NavAndPrintPrinterService>("navandprint");
            //container.RegisterType<IPrinterService, DefaultPrinterService>("default");
            //container.RegisterType<IPrinterService, AlertPrinterService>("alert");
            //container.RegisterType<IPrinterService, NavPrinterService>("nav");

            EcardContext.Container = container;

            Application.Add("container", container);
        }

        private void registerControllerFactory()
        {
            ControllerBuilder.Current.SetControllerFactory(getContainer().Resolve<EcardControllerFactory>());
        }

        private void registerActionFilters()
        {
            IUnityContainer container = getContainer();
            IActionFilterRegistry registry = container.Resolve<ActionFilterRegistry>();

            registry.Clear();

            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(AntiForgeryActionFilter));
            //registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(SiteInfoActionFilter));
            //registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(MenusActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(BuildUpActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(ContainerActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(UserActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(AntiForgeryAuthorizationFilter));

            //registry.Add(new[] { new DataFormatCriteria("RSS") }, typeof(RssResultActionFilter));
            //registry.Add(new[] { new DataFormatCriteria("ATOM") }, typeof(AtomResultActionFilter));
            //registry.Add(new[] { new RequestCriteria("excel"), }, typeof(ExcelResultActionFilter));

            //只有包含以下方法的 action 才能使用 PageSizeFilter 和 ArchiveListActionFilter
            ControllerActionCriteria listActionsCriteria = new ControllerActionCriteria();
            //registry.Add(new[] { listActionsCriteria }, typeof(PageSizeActionFilter));



            ControllerActionCriteria adminActionsCriteria = new ControllerActionCriteria();
            //adminActionsCriteria.AddMethod<SiteController>(s => s.Dashboard());
            //adminActionsCriteria.AddMethod<SiteController>(s => s.Item());
            //TODO: (erikpo) Once we have roles other than "authenticated" this should move to not be part of the admin, but just part of authed users
            //adminActionsCriteria.AddMethod<MembershipController>(u => u.ChangePassword());
            registry.Add(new[] { adminActionsCriteria }, typeof(AuthorizationFilter));

            //TODO: (erikpo) Once we have the plugin model completed, load up all available action filter criteria into the registry here instead of hardcoding them.

            container.RegisterInstance(registry);
        }

        public void OnTimer(object state)
        {
            var container = getContainer();
            if (container == null)
            {
                log = LogManager.GetLogger("app");
                DatabaseInstance.SqlStore = new DirectorySqlStore(Context.Server.MapPath("~/sql"));
                log.Debug("sql store updated");
                SetupContiner();
                log.Debug("SetupContiner");

                //SetupSite();
                //log.Debug("SetupSite");

                //RegisterRoutes();
                //log.Debug("RegisterRoutes");

                registerActionFilters();
                log.Debug("registerActionFilters");

                //registerModelBinders();
                //log.Debug("registerModelBinders");

                //registerViewEngines();
                //log.Debug("registerViewEngines");

                registerControllerFactory();
                log.Debug("registerControllerFactory");

                //_time = new Timer(OnTime, null, TimeSpan.FromMinutes(5));

                //BuildSiteCss();
                //log.Debug("BuildSiteCss");

                DependencyResolver.SetResolver(new UnityDependencyResolver((IUnityContainer)Application["container"]));
                log.Debug("SetResolver");
                //DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MobileAttribute), typeof(MobileAttributeAdapter));
                //log.Debug("RegisterAdapter");
                ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
                log.Debug("ValueProviderFactories.Factories.Add(new JsonValueProviderFactory())");
            }
            var SetWeChatService = container.Resolve<ISetWeChatService>();
            setWechat = SetWeChatService.GetById(1);
            if(setWechat==null)
            {
               log.Debug(string.Format("请先设置微信公众号信息!"));
                return;
            }
            AccessTokenContainer.Register(setWechat.appID,setWechat.AppSecret);
            log.Debug(string.Format("AccessTokenContainer注册成功！"));
            //if (string.IsNullOrWhiteSpace(setWechat.access_token))
            //{
            //    var nowdate = DateTime.Now;
            //    string url = "https://api.weixin.qq.com/cgi-bin/token";
            //    string param = string.Format("grant_type=client_credential&appid={0}&secret={1}", setWechat.appID, setWechat.AppSecret);
            //    var reult = HttpSend.getSend(url, param);
            //    var model = JsonConvert.DeserializeObject<MessageModel>(reult);
            //    if (!string.IsNullOrWhiteSpace(model.access_token))
            //    {
            //        var set = SetWeChatService.GetById(1);
            //        set.access_token = model.access_token;
            //        set.overtime = nowdate.AddSeconds(model.expires_in);
            //        SetWeChatService.Update(set);
            //        setWechat = set;
            //        CachePools.AddCache(CacheKeys.access_token, setWechat.access_token);
            //    }
            //    else
            //    {
            //        log.Debug(string.Format("更新access_token失败:{0}", reult));
            //    }
            //}
            //else
            //{
            //    var now = DateTime.Now.AddMinutes(-10);
            //    if (now >= setWechat.overtime)
            //    {
            //        var nowdate = DateTime.Now;
            //        string url = "https://api.weixin.qq.com/cgi-bin/token";
            //        string param = string.Format("grant_type=client_credential&appid={0}&secret={1}", setWechat.appID, setWechat.AppSecret);
            //        var reult = HttpSend.getSend(url, param);
            //        var model = JsonConvert.DeserializeObject<MessageModel>(reult);
            //        if (!string.IsNullOrWhiteSpace(model.access_token))
            //        {
            //            var set = SetWeChatService.GetById(1);
            //            set.access_token = model.access_token;
            //            set.overtime = nowdate.AddSeconds(model.expires_in);
            //            SetWeChatService.Update(set);
            //            setWechat = set;
            //            CachePools.AddCache(CacheKeys.access_token, setWechat.access_token);
            //        }
            //        else
            //        {
            //            log.Debug(string.Format("更新access_token失败:{0}", reult));
            //        }
            //    }
            //}
            //if(CachePools.GetData(CacheKeys.access_token)==null)
            //    CachePools.AddCache(CacheKeys.access_token, setWechat.access_token);
            
            //_time.Start();
        }

        public SetWeChat setWechat { get; set; }

        public void OnMessageTimer(object source, System.Timers.ElapsedEventArgs e)
        { 
            var container = getContainer();
            var IMessagesService = container.Resolve<IMessagesService>();
            var ISiteService = container.Resolve<ISiteService>();
            //var SetWeChatService = container.Resolve<ISetWeChatService>();
            //setWechat = SetWeChatService.GeByFirst();
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site == null)
            {
                log.Debug(string.Format("请先设置配置信息!"));
                return;
            }
            if (setWechat == null)
            {
                log.Debug(string.Format("请先设置微信公众号信息!"));
                return;
            }
            var list = IMessagesService.GetSatySend().ToList();
            if (list != null)
            {
                foreach (var item in list)
                {
                    try
                    {
                        if (item.msgType == MsgType.orderState)
                        {
                            //订单状态更新消息
                            TemplateData model = new TemplateData();
                            model.first = new TemplateDataItem("订单通知");
                            model.OrderSn = new TemplateDataItem(item.keyword1);
                            model.keyword1 = new TemplateDataItem(item.keyword1);
                            model.keyword2 = new TemplateDataItem(item.keyword2);
                            model.OrderStatus = new TemplateDataItem(item.keyword2);
                            model.keyword3 = new TemplateDataItem(DateTime.Now.ToString());
                            if (!string.IsNullOrWhiteSpace(item.msg))
                                model.remark = new TemplateDataItem(item.msg);
                            //GFVUrVI-d-0-aDx8hJKAde3zR0SDkHJv8t0jG0fUP4s
                           // var send = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(WxPayConfig.APPID, item.openId, "vfNVyuNt24sZLlbNjcmW_BznxLRYTde68M0BYIKrAcE", "#FFFFFF", site.Url + "/WeChatViews/MyOrder.html", model);
                            var send = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(WxPayConfig.APPID, item.openId, "vfNVyuNt24sZLlbNjcmW_BznxLRYTde68M0BYIKrAcE", "#FFFFFF", site.Url + "/Weixin/OAuth2url?orderNo=" + item.keyword1, model);
                            if (send.errcode == 0)
                            {
                                item.sendTime = DateTime.Now;
                                item.state = MessagesState.completed;
                                IMessagesService.Update(item);
                            }
                        }
                        else if (item.msgType == MsgType.withdrawNotice)
                        {
                            //提现通知
                            TemplateData model = new TemplateData();
                            model.first = new TemplateDataItem("提现通知");
                            model.keyword1 = new TemplateDataItem(item.keyword1);
                            model.keyword2 = new TemplateDataItem(item.keyword2);
                            model.keyword3 = new TemplateDataItem(item.keyword3);
                            model.keyword4 = new TemplateDataItem(item.keyword4);
                            if (!string.IsNullOrWhiteSpace(item.msg))
                                model.remark = new TemplateDataItem(item.msg);
                            var send = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(WxPayConfig.APPID, item.openId, "L5r_Ptv2SXdmDNsbEFhkl1mHL6PxuOSpJmKSImdbpho", "#FFFFFF", site.Url + "/WeChatViews/PersonalCentre.html", model);
                            if (send.errcode == 0)
                            {
                                item.sendTime = DateTime.Now;
                                item.state = MessagesState.completed;
                                IMessagesService.Update(item);
                            }
                        }
                        else if (item.msgType == MsgType.withdrawReview)
                        {
                            //提现审核通知
                            TemplateData model = new TemplateData();
                            model.first = new TemplateDataItem(item.msg);
                            model.keyword1 = new TemplateDataItem(item.keyword1);
                            model.keyword2 = new TemplateDataItem(item.keyword2);
                            model.keyword3 = new TemplateDataItem(item.keyword3);
                            model.keyword4 = new TemplateDataItem(item.keyword4);
                            model.keyword5 = new TemplateDataItem(item.keyword5);
                            var send = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(WxPayConfig.APPID, item.openId, "qbzOD6TutQR-Qx0ZsYo9MUvZvj2SMeizbcAkzNEIUjU", "#FFFFFF", site.Url + "/WeChatViews/PersonalCentre.html", model);
                            if (send.errcode == 0)
                            {
                                item.sendTime = DateTime.Now;
                                item.state = MessagesState.completed;
                                IMessagesService.Update(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    { 
                       // WxPayAPI.Log.Debug("模版消息：", ex.Message);
                    }
                }
            }
        }

        public void OnOrderCancelTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            string sql = "select * from fz_Orders where orderState=1 and DATEDIFF(mi,submitTime,GETDATE())>30";
            var list = new QueryObject<Order>(_databaseInstance, sql, null).ToList();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                   var tran=_databaseInstance.BeginTransaction();
                   try
                   {
                       var order = IOrderService.GetById(item.orderId);
                       if (order.orderState == OrderStates.awaitPay)
                       {
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
                           order.orderState = OrderStates.cancel;
                           order.completeTime = DateTime.Now;
                           IOrderService.Update(order);
                           tran.Commit();
                       }

                   }
                   catch (Exception ex)
                   {
                       tran.Rollback();
                       log.Debug(string.Format("取消订单出错：{0}!", ex.Message));
                   }
                   finally
                   {
                       tran.Dispose();
                   }
                   
                }
            }
            
        }

        public void OnAutomaticReceiptTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var container1 = getContainer();
                var IMessagesService = container1.Resolve<IMessagesService>();
                var IOrderService = container1.Resolve<IOrderService>();
                var IOrderDetailService = container1.Resolve<IOrderDetailService>();
                var IRebateService = container1.Resolve<IRebateService>();
                var ISiteService = container1.Resolve<ISiteService>();
                var IAccountService = container1.Resolve<IAccountService>();
                var TransactionHelper = container1.Resolve<TransactionHelper>();
                var site = ISiteService.Query(null).FirstOrDefault();
                if (site == null)
                {
                    log.Debug(string.Format("请先设置配置信息!"));
                    return;
                }
                var list = IOrderService.GetOrder();
                if (list == null)
                    return;
                foreach (var item in list)
                {
                    var order = IOrderService.GetById(item.orderId);
                    if (order != null)
                    {
                        
                        if (order.orderState != OrderStates.shipped)
                            continue;
                        if (order.ShipTime == null)
                            continue;
                        if(order.ShipTime.Value.AddDays(8)<DateTime.Now)
                            continue;
                        order.orderState = OrderStates.complete;
                        TransactionHelper.BeginTransaction();
                        if (IOrderService.Update(order) > 0)
                        {
                            var account = IAccountService.GetById(order.userId);
                            if (!string.IsNullOrWhiteSpace(account.openID))
                            {
                                var message = new Fz_Messages();
                                message.accountId = account.accountId;
                                message.keyword1 = order.orderNo;
                                message.keyword2 = "订单完成";
                                message.msg = "";
                                message.msgType = MsgType.orderState;
                                message.openId = account.openID;
                                message.state = MessagesState.staySend;
                                message.submitTime = DateTime.Now;
                                IMessagesService.Insert(message);
                            }
                            TransactionHelper.Commit();
                            IRebateService.Rebate(order.orderId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug("OnAutomaticReceiptTimer", ex.Message);
            }
        }

        //private void registerActionFilters()
        //{
        //    IUnityContainer container = getContainer();
        //    IActionFilterRegistry registry = container.Resolve<ActionFilterRegistry>();

        //    registry.Clear();

        //    registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(AntiForgeryActionFilter));
        //    registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(SiteInfoActionFilter));
        //    //registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(MenusActionFilter));
        //    registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(BuildUpActionFilter));
        //    registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(ContainerActionFilter));
        //    //registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(UserActionFilter));
        //    registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(AntiForgeryAuthorizationFilter));

        //    registry.Add(new[] { new DataFormatCriteria("RSS") }, typeof(RssResultActionFilter));
        //    registry.Add(new[] { new DataFormatCriteria("ATOM") }, typeof(AtomResultActionFilter));
        //    //registry.Add(new[] { new RequestCriteria("excel"), }, typeof(ExcelResultActionFilter));

        //    //只有包含以下方法的 action 才能使用 PageSizeFilter 和 ArchiveListActionFilter
        //    ControllerActionCriteria listActionsCriteria = new ControllerActionCriteria();
        //    registry.Add(new[] { listActionsCriteria }, typeof(PageSizeActionFilter));



        //    ControllerActionCriteria adminActionsCriteria = new ControllerActionCriteria();
        //    //adminActionsCriteria.AddMethod<SiteController>(s => s.Dashboard());
        //    //adminActionsCriteria.AddMethod<SiteController>(s => s.Item());
        //    //TODO: (erikpo) Once we have roles other than "authenticated" this should move to not be part of the admin, but just part of authed users
        //    //adminActionsCriteria.AddMethod<MembershipController>(u => u.ChangePassword());
        //    registry.Add(new[] { adminActionsCriteria }, typeof(AuthorizationFilter));

        //    //TODO: (erikpo) Once we have the plugin model completed, load up all available action filter criteria into the registry here instead of hardcoding them.

        //    container.RegisterInstance(registry);
        //}
    }

    /// <summary>
    /// 模版
    /// </summary>
    public class TemplateData
    {
        public TemplateData()
        {
            remark = new TemplateDataItem("感谢您的使用，请继续关注支持我们！");
        }
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TemplateDataItem keyword4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TemplateDataItem keyword5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TemplateDataItem remark { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public TemplateDataItem OrderSn { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public TemplateDataItem OrderStatus { get; set; }
    }
}