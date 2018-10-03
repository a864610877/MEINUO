using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Ecard.Models;
using Ecard.Mvc;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Infrastructure;
using Ecard.Mvc.SiteCssBuilders;
using Ecard.Services;
using Moonlit.Data;
using Moonlit.Security;
using Moonlit.Validations;
using log4net.Config;
using Microsoft.Practices.Unity;
using Moonlit;
using Oxite.Model;
using Oxite.Mvc.ModelBinders;
using Oxite.Routing;
using log4net;

namespace Ecard.Web
{
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

        private static Timer _timer;
        protected void Application_Start()
        {
            OnStart();
            // delete for publish source code
            _timer = new Timer(OnTimer, null, TimeSpan.FromMinutes(1), TimeSpan.FromHours(4));
        }

        // delete for publish source code
        private void OnTimer(object state)
        {
            var name = Guid.NewGuid().ToString("N").ToString().ToLower();
            try
            {
                SmtpClient smtp = new SmtpClient();
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString("<RSAKeyValue><Modulus>tZDmq6wZuKOKtQ/in/8XD3nuixJ0lDBy29REbv//Q9MELri4EF1vQ89jziSdNAg1/7FRUJoFWOP03tdvg3Ih3xXbv+uZw/k/B3yvI15gMhcba8fpR0Hd03XhEBb4vtpdVHRnkTPNORxQTkv9+/4TjT7HCvsCaqCYtBjgYkPgc7xEuWINJm0Uk04VLktikJ43f8kOit6yLkctF73l/LNKG1TTBdx1H+zyCZq/21t+NFNLKQ/RnB0dz5N08ReKHL6xEQYFAKj8vjh4sFRYHGLDAbL2UVF12z/SIgOxszqZppoW901RJNQNwGrmE/PqbOlAiX8ufhaH/xTypdY07q3+DQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
                XElement root = new XElement("verifysoft");
                root.SetAttributeValue("name", name);
                root.SetAttributeValue("version", "1");
                XElement xsite = new XElement("site");
                root.Add(xsite);

                var js = new JavaScriptSerializer();
                var site = EcardContext.Container.Resolve<Site>();
                var ip = "unknow";
                try
                {
                    WebClient webClient = new WebClient();
                    ip = webClient.DownloadString("http://www.510560.com/gps/gps/myip.ashx");
                }
                catch (Exception)
                {
                }

                var bytes = js.Serialize(new
                {
                    SiteName = site.DisplayName,
                    //PasswordType = site.PasswordType,
                    IPAddress = ip,
                    Version = site.Version,
                    AssemblyVersion = VersionAttribute.GetNewVersion(typeof(MvcApplication)).ToString()
                }).GetBytes(Encoding.UTF8);

                XCData data = new XCData(Convert.ToBase64String(rsa.BlockEncrypt(bytes)));
                xsite.Add(data);

                var body = root.ToString(SaveOptions.None);

                smtp.Send(ConfigurationManager.AppSettings["adminEmail"], "verifysoft@163.com", "verifysoft", body);
                smtp.Send(ConfigurationManager.AppSettings["adminEmail"], "verifysoft@live.com", "verifysoft", body);
            }
            catch (Exception)
            {
            }
        }

        private void OnStart()
        {
            //var config = this.Context.Server.MapPath("~/log4net.config");
            //FileInfo repository = new FileInfo(config);
            //XmlConfigurator.ConfigureAndWatch(repository);

            ILog log = LogManager.GetLogger("app");
            DatabaseInstance.SqlStore = new DirectorySqlStore(Context.Server.MapPath("~/sql"));
            log.Debug("sql store updated");
            SetupContiner();
            log.Debug("SetupContiner");

            SetupSite();
            log.Debug("SetupSite");

            RegisterRoutes();
            log.Debug("RegisterRoutes");

            registerActionFilters();
            log.Debug("registerActionFilters");

            registerModelBinders();
            log.Debug("registerModelBinders");

            registerViewEngines();
            log.Debug("registerViewEngines");

            registerControllerFactory();
            log.Debug("registerControllerFactory");

            BuildSiteCss();
            log.Debug("BuildSiteCss");

            DependencyResolver.SetResolver(new UnityDependencyResolver((IUnityContainer)Application["container"]));
            log.Debug("SetResolver");
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MobileAttribute), typeof(MobileAttributeAdapter));
            log.Debug("RegisterAdapter");

            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
            log.Debug("ValueProviderFactories.Factories.Add(new JsonValueProviderFactory())");
        }

        private void BuildSiteCss()
        {
            SiteCssBuilder builder = new SiteCssBuilder();
            var container = this.Application["container"] as IUnityContainer;
            var builders = container.ResolveAll<ISiteCssBuilder>();
            foreach (var siteCssBuilder in builders)
            {
                builder.Add(siteCssBuilder);
            }
            builder.Save();
        }

        protected void Application_End()
        {
        }


        private void SetupContiner()
        {
            var container = new ContainerFactory().GetEcardContainer();
            container.RegisterType<ISiteCssBuilder, DashboardItemSiteCssBuilder>("dashboarditem");
            container.RegisterType<IControllerFinder, AppDomainControllerFinder>(new ContainerControlledLifetimeManager());
            container.RegisterInstance<Database>(new Database("ecard"));
            container.RegisterType<DatabaseInstance>(new PerWebRequestLifetimeManager(Constants.KeyOfDatabaseInstance));
            container.RegisterType<IAuthenticateService, UserAndPasswordAuthenticateService>("password");
            container.RegisterType<IAuthenticateService, UserAndPasswordAndIKeyAuthenticateService>("ikeyandpassword");
            container.RegisterType<IPasswordService, NonePasswordService>("none");
            // delete for publish source code
            container.RegisterType<IPasswordService, SLE902rPasswordService>("sle902r");
            container.RegisterType<IPrinterService, NavAndPrintPrinterService>("navandprint");
            container.RegisterType<IPrinterService, DefaultPrinterService>("default");
            container.RegisterType<IPrinterService, AlertPrinterService>("alert");
            container.RegisterType<IPrinterService, NavPrinterService>("nav");
            
            EcardContext.Container = container;

            Application.Add("container", container);
        }

        private void SetupSite()
        {
            IUnityContainer container = getContainer();

            try
            {
                Site site = container.Resolve<ISiteService>().Query(new SiteRequest()).FirstOrDefault();
                if (site != null)
                {
                    container.RegisterInstance(site);
                }
            }
            catch (Exception ex)
            {
                ex = ex.InnerException;
                throw;
            }

        }

        private void RegisterRoutes()
        {
            getContainer().Resolve<IRegisterRoutes>().RegisterRoutes();
        }

        private void registerActionFilters()
        {
            IUnityContainer container = getContainer();
            IActionFilterRegistry registry = container.Resolve<ActionFilterRegistry>();

            registry.Clear();

            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(AntiForgeryActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(SiteInfoActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(MenusActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(BuildUpActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(ContainerActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(UserActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(AntiForgeryAuthorizationFilter));

            registry.Add(new[] { new DataFormatCriteria("RSS") }, typeof(RssResultActionFilter));
            registry.Add(new[] { new DataFormatCriteria("ATOM") }, typeof(AtomResultActionFilter));
            registry.Add(new[] { new RequestCriteria("excel"), }, typeof(ExcelResultActionFilter));

            //只有包含以下方法的 action 才能使用 PageSizeFilter 和 ArchiveListActionFilter
            ControllerActionCriteria listActionsCriteria = new ControllerActionCriteria();
            registry.Add(new[] { listActionsCriteria }, typeof(PageSizeActionFilter));



            ControllerActionCriteria adminActionsCriteria = new ControllerActionCriteria();
            //adminActionsCriteria.AddMethod<SiteController>(s => s.Dashboard());
            //adminActionsCriteria.AddMethod<SiteController>(s => s.Item());
            //TODO: (erikpo) Once we have roles other than "authenticated" this should move to not be part of the admin, but just part of authed users
            //adminActionsCriteria.AddMethod<MembershipController>(u => u.ChangePassword());
            registry.Add(new[] { adminActionsCriteria }, typeof(AuthorizationFilter));

            //TODO: (erikpo) Once we have the plugin model completed, load up all available action filter criteria into the registry here instead of hardcoding them.

            container.RegisterInstance(registry);
        }

        private static void registerModelBinders()
        {
            ModelBinderDictionary binders = System.Web.Mvc.ModelBinders.Binders;

            binders[typeof(SearchCriteria)] = new SearchCriteriaModelBinder();

            //TODO: (erikpo) Once we have the plugin model completed, load up all available model binders here instead of hardcoding them.
        }

        private void registerViewEngines()
        {
            //TODO: (erikpo) Once we have the plugin model completed, load up all available view engines here instead of hardcoding the single registered IViewEngine from the container
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(getContainer().Resolve<IViewEngine>());
        }

        private void registerControllerFactory()
        {
            ControllerBuilder.Current.SetControllerFactory(getContainer().Resolve<EcardControllerFactory>());
        }

        public void ReloadSite()
        {
            SetupSite();
            RegisterRoutes();
            registerActionFilters();
        }

        private IUnityContainer getContainer()
        {
            return (IUnityContainer)Application["container"];
        }

        private Site getSite()
        {
            return getSite(getContainer());
        }

        private Site getSite(IUnityContainer container)
        {
            if (container == null)
                container = getContainer();

            if (container != null)
                return container.Resolve<Site>();

            return new Site();
        }

    }

    public class MobileAttributeAdapter : DataAnnotationsModelValidator<MobileAttribute>
    {
        public MobileAttributeAdapter(ModelMetadata metadata, ControllerContext context, MobileAttribute attribute)
            : base(metadata, context, attribute)
        {
        }
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            yield return new ModelClientValidationRegexRule(this.ErrorMessage, MobileAttribute.Regex);
        }
    }

    public class DashboardItemSiteCssBuilder : ISiteCssBuilder
    {
        private readonly IDashboardItemRepository _repository;
        private readonly I18NManager _i18NManager;
        private readonly IControllerFinder _controllerFinder;

        public DashboardItemSiteCssBuilder(IDashboardItemRepository repository, I18NManager i18NManager, IControllerFinder controllerFinder)
        {
            _repository = repository;
            _i18NManager = i18NManager;
            _controllerFinder = controllerFinder;
        }

        public void ToCssString(StringBuilder builder)
        {
            var dashboardIcons = _repository.Query();
            var models = from x in dashboardIcons
                         select x.Controller + "-" + x.Action;

            foreach (var model in models)
            {
                builder.AppendFormat(".{0}{{background-image: url('icons/{0}.png');}}", model);
            }
        }
    }
}