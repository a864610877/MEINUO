//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ecard.Infrastructure;
using Ecard.Models;

//using Ecard.Mvc.Infrastructure;
//using Ecard.Mvc.Skinning;
using Ecard.Routing;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Moonlit;
using Oxite.Infrastructure;
//using Oxite.Mvc.Infrastructure;
//using Oxite.Mvc.Skinning;
using Oxite.Routing;
using System.Web.Hosting;
using Ecard.SqlServices;
using Ecard.Mvc.Infrastructure;
using Oxite.Mvc.Infrastructure;

namespace MicroMall.Models
{
    public class Constants
    {
        public const string SystemShopName = "000000000000000";
        public const string KeyOfDatabaseInstance = "database_instance";
        public const string Session_AccountToken = "accountToken";
    }
    /// <summary>
    /// </summary>
    public class ContainerFactory
    {
        public IUnityContainer GetEcardContainer()
        {
            IUnityContainer parentContainer = new UnityContainer();

            parentContainer
                .RegisterInstance(new AppSettingsHelper(ConfigurationManager.AppSettings))
                .RegisterInstance(RouteTable.Routes)
                .RegisterInstance(HostingEnvironment.VirtualPathProvider);

            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                parentContainer.RegisterInstance(connectionString.Name, connectionString.ConnectionString);
            }

            parentContainer
                .RegisterInstance<RouteCollection>(RouteTable.Routes)
                //.RegisterType<IPermissionService, PermissionService>()
                .RegisterType<IActionInvoker, EcardControllerActionInvoker>()
                .RegisterType<IFormsAuthentication, FormsAuthenticationWrapper>()
                //.RegisterType<ISkinEngine, VirtualPathProviderSkinEngine>() 
                .RegisterType<ITokenHelper, TokenHelper>()
                .RegisterInstance<II18N>(new I18N(HttpContext.Current.Server.MapPath("~/configs")))
                .RegisterType<I18NManager>()
                .RegisterType<IMenuService, XmlReflectionMenuService>()
                .RegisterType<IMembershipService, SqlMembershipService>()
                .RegisterType<ISiteService, SqlSiteService>()
                //.RegisterType<IShopService, SqlShopService>()
                .RegisterType<ITemporaryTokenKeyService, SqlTemporaryTokenKeyService>()
                .RegisterType<ILogService, SqlLogService>()
                .RegisterType<ISmsService, SqlSmsService>()
                .RegisterType<ICacheService, CompositeCacheService>()
                .RegisterType<ISpecificationService, SqlSpecificationService>()
                .RegisterType<ISpecificationDetailService, SqlSpecificationDetailService>()
                .RegisterType<ICommodityService, SqlCommodityService>()
                .RegisterType<IAccountService, SqlAccountService>()
                .RegisterType<IProvinceService, SqlProvinceService>()
                .RegisterType<ICityService, SqlCityService>()
                .RegisterType<IUserAddressService, SqlUserAddressService>()
                .RegisterType<IShoppingCartService, SqlShoppingCartService>()
                .RegisterType<ISetWeChatService, SqlSetWeChatService>()
                .RegisterType<ILog4netService, SqlLog4netService>()
                .RegisterType<IRecommendLogService, SqlRecommendLogService>()
                .RegisterType<IOperationPointLogService, SqlOperationPointLogService>()
                .RegisterType<IOrangeKeyAndopenIDService, SqlOrangeKeyAndopenIDService>()
                .RegisterType<IAdService, SqlAdService>()
                .RegisterType<IOrderService, SqlOrderService>()
                .RegisterType<IOrderDetailService, SqlOrderDetailService>()
                .RegisterType<IReviewService, SqlReviewService>()
                .RegisterType<IWithdrawService, SqlWithdrawService>()
                .RegisterType<IImageAdsService, SqlImageAdsService>()
                .RegisterType<IJuMeiMallService, SqlJuMeiMallService>()
                .RegisterType<IRebateService,SqlRebateService>()
                .RegisterType<IGradesService,SqlGradesService>()
                .RegisterType<IOperationAmountLogsService,SqlOperationAmountLogsService>()
                .RegisterType<IMessagesService, SqlMessagesService>()
                .RegisterType<IArticlesService,SqlArticlesService>()
                .RegisterType<ICommodityCategorysService, SqlCommodityCategorysService>()
                //.RegisterType<IPointRebateLogService, SqlPointRebateLogService>()
                .RegisterType<IRebateLogService,SqlRebateLogService>()
                .RegisterType<ISecondKillSetService,SqlSecondKillSetService>()
                .RegisterType<ISecondKillCommoditysService, SqlSecondKillCommoditysService>()
                .RegisterType<IPayOrderService,SqlPayOrderService>()
                .RegisterType<IDashboardItemRepository, DashboardItemRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IRegisterRoutes, EcardRoutes>(
                    new InjectionConstructor(
                        new ResolvedParameter<RouteCollection>(),
                        new ResolvedParameter<AppSettingsHelper>(),
                        new ResolvedParameter<Site>()
                        ));


            // TODO: CreateChildContainer?
            IUnityContainer oxiteContainer = parentContainer.CreateChildContainer();

            oxiteContainer.LoadConfiguration("ecard");

            oxiteContainer.RegisterInstance(oxiteContainer);

            return oxiteContainer;
        }
    }
}
