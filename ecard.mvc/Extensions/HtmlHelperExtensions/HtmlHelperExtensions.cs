using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Ecard.Models;
using Ecard.Mvc.Models;
using Ecard.Mvc.ViewModels;
using Microsoft.Practices.Unity;
using Moonlit;
using Moonlit.Reflection;
using Oxite;

namespace Ecard.Mvc
{
    public static class WebViewPageExtensions
    {
        public static bool IsPartialPage(this WebViewPage page)
        {
            return page.Context.Request["isPartial"] != null;
        }
    }
    public static class HtmlHelperExtensions
    {
        private const string Global = "global";

        public static string GetLayout(this HtmlHelper htmlHelper, string layout)
        {
            return layout;
        }
        public static string GetFullArrayHtmlFieldId(this TemplateInfo templateInfo, int index, string propertyName)
        {
            return templateInfo.GetFullHtmlFieldId("") + "[" + index + "]" + "_" + propertyName;
        }
        public static string GetFullArrayHtmlFieldName(this TemplateInfo templateInfo, int index, string propertyName)
        {
            return templateInfo.GetFullHtmlFieldName("") + "[" + index + "]" + "." + propertyName;
        }
        public static MvcHtmlString HiddenPagerAndSortter<TModel>(this HtmlHelper<TModel> htmlHelper, bool pager = true, bool sorter = true)
            where TModel : IItemList
        {
            var pagerModel = htmlHelper.ViewData.Model.Items as IPageOfList;
            StringBuilder buffer = new StringBuilder();
            if (pager)
            {
                buffer.Append(htmlHelper.Hidden("PageSize", pagerModel.PageSize));
                buffer.Append(htmlHelper.Hidden("PageIndex", pagerModel.PageIndex));
            }
            if (sorter)
            {
                buffer.Append(htmlHelper.Hidden("OrderBy", pagerModel.OrderBy));
            }
            return MvcHtmlString.Create(buffer.ToString());
        }
        public static MenuItem GetCurrentMenu(this HtmlHelper<PageModel> htmlHelper, int level)
        {
            if (!htmlHelper.ViewData.Model.CurrentMenu.ContainsKey(level))
            {
                var controller = htmlHelper.ViewContext.RouteData.Values["controller"].ToString();
                var action = htmlHelper.ViewContext.RouteData.Values["action"].ToString();

                htmlHelper.ViewData.Model.CurrentMenu.Add(level, GetMenu(htmlHelper.ViewData.Model.Menus, controller, action, level));
            }
            return htmlHelper.ViewData.Model.CurrentMenu[level];
        }

        private static MenuItem GetMenu(IEnumerable<MenuItem> menuItems, string controller, string action, int level)
        {
            foreach (var menuItem in menuItems)
            {
                if (string.Equals(menuItem.Controller, controller, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(menuItem.Action, action, StringComparison.OrdinalIgnoreCase) &&
                    level == menuItem.Level
                    )
                    return menuItem;
                var item = GetMenu(menuItem.Children, controller, action, level);
                if (item != null)
                    return item;
            }
            return null;
        }

        public static MvcHtmlString Pager<TModel>(this HtmlHelper<TModel> htmlHelper)
            where TModel : IItemList
        {
            var pagerModel = htmlHelper.ViewData.Model.Items as IPageOfList;
            return htmlHelper.Partial(GetTemplateName("pager"), pagerModel);
        }
        public static MvcHtmlString SorterSelect<TModel>(this HtmlHelper<TModel> htmlHelper, object items)
            where TModel : IItemList
        {
            var pagerModel = htmlHelper.ViewData.Model.Items as IPageOfList;
            var dict = HtmlHelper.AnonymousObjectToHtmlAttributes(items);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var key in dict.Keys)
            {
                list.Add(new SelectListItem() { Selected = string.Equals(pagerModel.OrderBy, key, StringComparison.OrdinalIgnoreCase), Text = (string)dict[key], Value = key });
            }
            return htmlHelper.Partial(GetTemplateName("sorterselect"), list);
        }

        public static MvcHtmlString Pager<TModel>(this HtmlHelper<TModel> htmlHelper, IPageOfList pageOfList)
            where TModel : EcardModelReportRequest
        {
            return htmlHelper.Partial(GetTemplateName("pager"), pageOfList);
        }

        public static MvcHtmlString Submit(this HtmlHelper htmlHelper, string title)
        {
            return MvcHtmlString.Create(string.Format("<input type='submit' class='scbtn' style='height: 29px;' value='{0}' />", title));
        }

        public static MvcHtmlString LabelLocalize(this HtmlHelper htmlHelper, PropertyDescriptor expression)
        {
            var model = new LabelModel();
            model.FieldName = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(expression.PropertyName);
            model.Value = expression.Name;
            return htmlHelper.Partial(GetTemplateName("label"), model);
        }

        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string action, string controller, object routeValues, bool textEnabled, string template, bool? isPost = null)
        {
            action = action ?? htmlHelper.ViewContext.RouteData.Values["action"] as string;
            controller = controller ?? htmlHelper.ViewContext.RouteData.Values["controller"] as string;
            var controllerFinder = ExtensionsHelper.Resolve<IControllerFinder>();

            Type controllerType = controllerFinder.FindController(controller);
            if (controllerType == null) return MvcHtmlString.Empty;

            MethodInfo method = controllerType.GetMethods().FirstOrDefault(x => string.Equals(x.Name, action, StringComparison.InvariantCultureIgnoreCase));
            if (method == null) return MvcHtmlString.Empty;

            isPost = isPost ?? method.GetAttribute<HttpPostAttribute>(false) != null;
            ViewModelDescriptor controllerTypeDesc = ViewModelDescriptor.GetTypeDescriptor(controllerType);

            MethodDescriptor methodDesc = controllerTypeDesc.GetMethod(action);
            if (methodDesc == null) return MvcHtmlString.Empty;

            User user = EcardContext.Container.Resolve<SecurityHelper>().GetCurrentUser();
            if (user != null)
            {
                if (!methodDesc.Permission.Check(user))
                    return MvcHtmlString.Empty;
            }

            string text = textEnabled ? methodDesc.Name : "";
            string icon = methodDesc.ToolbarIcon;
            string description = methodDesc.Description;
            string confirm = methodDesc.Confirm;
            RouteValueDictionary routeValueDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            if (routeValues is RouteValueDictionary)
                routeValueDictionary = (RouteValueDictionary)routeValues;


            return htmlHelper.Partial(GetTemplateName(template), new ActionModel
                                                                     {
                                                                         IsPost = (bool)isPost,
                                                                         ConfirmMessage = confirm,
                                                                         Text = text,
                                                                         Icon = icon,
                                                                         Description = description,
                                                                         Action = action,
                                                                         Controller = controller,
                                                                         RouteValues = routeValueDictionary
                                                                     });
        }

        public static MvcHtmlString ToolbarButton(this HtmlHelper htmlHelper, string action, string controller = null, object routeValues = null, bool? isPost = null)
        {
            return Link(htmlHelper, action, controller, routeValues, false, "toolbarbutton", isPost);
        }
        public static MvcHtmlString ToolbarButton2(this HtmlHelper htmlHelper, string action, string controller = null, object routeValues = null, bool? isPost = null)
        {
            return Link(htmlHelper, action, controller, routeValues, false, "toolbarButton2", isPost);
        }
        public static MvcHtmlString ActionButton(this HtmlHelper htmlHelper, ActionMethodDescriptor descriptor)
        {
            return ActionButton(htmlHelper, descriptor.Action, descriptor.Controller, descriptor.Routes,
                                descriptor.IsPost);
        }
        public static MvcHtmlString ActionButton(this HtmlHelper htmlHelper, string action, string controller = null, object routeValues = null, bool? isPost = null)
        {
            return Link(htmlHelper, action, controller, routeValues, true, "actionlink", isPost);
        }
        public static MvcHtmlString ToolbarOfList(this HtmlHelper htmlHelper, IEnumerable<ActionMethodDescriptor> descriptors)
        {
            return htmlHelper.Partial(GetTemplateName("ToolbarOfList"), descriptors);
        }

        public static MvcHtmlString ToolbarOfItem(this HtmlHelper htmlHelper, IEnumerable<ActionMethodDescriptor> descriptors)
        {
            return htmlHelper.Partial(GetTemplateName("ToolbarOfItem"), descriptors);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string action, string controller = null, object routeValues = null)
        {
            return Link(htmlHelper, action, controller, routeValues, true, "actionLink");
        }

        private static MvcHtmlString Th<TModel>(this HtmlHelper<TModel> htmlHelper, PropertyDescriptor property, IPageOfList pagedList, object htmlAttributes = null,
                                                bool defaultSort = false)
        {
            var th = new ThModel
                         {
                             Text = property.ShortName,
                         };
            if (pagedList != null && !string.IsNullOrEmpty(property.Sort))
            {
                if (pagedList.OrderBy == property.Sort)
                {
                    th.Sort = property.Sort + " desc";
                    th.Directory = false;
                }
                else if (pagedList.OrderBy.StartsWith(property.Sort))
                {
                    th.Sort = property.Sort;
                    th.Directory = true;
                }
                else
                {
                    th.Sort = property.Sort;
                }
            }
            return htmlHelper.Partial(GetTemplateName("th"), th, ToViewData(htmlAttributes));
        }

        public static MvcHtmlString Ths<TModel>(this HtmlHelper<TModel> htmlHelper, EcardModelReportRequest request)
        {
            var helper = EcardContext.Container.Resolve<SecurityHelper>();
            User currentUser = helper.GetCurrentUser().CurrentUser;
            IPageOfList pagedList = null;

            pagedList = request;

            var buffer = new StringBuilder();
            foreach (
                PropertyDescriptor propertyDescriptor in
                    ViewModelDescriptor.GetTypeDescriptor(request.GetType()).Properties.Where(x => x.Permission.Check(currentUser) && x.Show).OrderBy(x => x.Order))
            {
                buffer.Append(htmlHelper.Th(propertyDescriptor, pagedList).ToHtmlString());
            }
            return MvcHtmlString.Create(buffer.ToString());
        }

        public static MvcHtmlString Ths<TModel>(this HtmlHelper<TModel> htmlHelper, IEnumerable collection, Type itemType)
        {
            var helper = EcardContext.Container.Resolve<SecurityHelper>();
            User currentUser = helper.GetCurrentUser().CurrentUser;
            IPageOfList pagedList = null;

            pagedList = collection as IPageOfList;

            var buffer = new StringBuilder();
            foreach (
                PropertyDescriptor propertyDescriptor in
                    ViewModelDescriptor.GetTypeDescriptor(itemType).Properties.Where(x => x.Permission.Check(currentUser) && x.Show).OrderBy(x => x.Order))
            {
                buffer.Append(htmlHelper.Th(propertyDescriptor, pagedList).ToHtmlString());
            }
            return MvcHtmlString.Create(buffer.ToString());
        }

        public static MvcHtmlString Tds<TModel>(this HtmlHelper<TModel> htmlHelper, object item, Type itemType)
        {
            var helper = EcardContext.Container.Resolve<SecurityHelper>();
            User currentUser = helper.GetCurrentUser();
            var buffer = new StringBuilder();
            foreach (
                PropertyDescriptor propertyDescriptor in
                    ViewModelDescriptor.GetTypeDescriptor(itemType).Properties.Where(x => x.Permission.Check(currentUser) && x.Show).OrderBy(x => x.Order))
            {
                buffer.Append(htmlHelper.Td(propertyDescriptor, item).ToHtmlString());
            }
            return MvcHtmlString.Create(buffer.ToString());
        }
        public static int PropertyCount(this HtmlHelper htmlHelper, Type itemType)
        {
            var helper = EcardContext.Container.Resolve<SecurityHelper>();
            User currentUser = helper.GetCurrentUser();
            var buffer = new StringBuilder();
            int count =
                ViewModelDescriptor.GetTypeDescriptor(itemType).Properties.Where(
                    x => x.Permission.Check(currentUser) && x.Show).OrderBy(x => x.Order).Count();

            return count;
        }

        public static MvcHtmlString Th<TModel>(this HtmlHelper<TModel> htmlHelper, Ths ths, object htmlAttributes = null)
        {
            if (ths == Mvc.Ths.Select)
            {
                return htmlHelper.Partial(GetTemplateName("th_select"), null);
            }
            ThModel th = GetThModel(ths);
            var vd = new ViewDataDictionary();
            vd.Merge(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return htmlHelper.Partial(GetTemplateName("th"), th, ToViewData(htmlAttributes));
        }

        public static MvcHtmlString Th(this HtmlHelper htmlHelper, ThModel th)
        {
            return htmlHelper.Partial(GetTemplateName("th"), th, new ViewDataDictionary());
        }

        public static MvcHtmlString Td<TModel>(this HtmlHelper<TModel> htmlHelper, Ths ths, int key, string category = null)
        {
            if (ths == Mvc.Ths.Select)
            {
                ViewDataDictionary vd = new ViewDataDictionary();
                vd.Add("category", category);
                return htmlHelper.Partial(GetTemplateName("td_select"), key, vd);
            }
            throw new NotSupportedException();
        }

        //#region OpenSearchOSDXLink

        //public static string OpenSearchOSDXLink<TModel>(this HtmlHelper<TModel> htmlHelper) where TModel : OxiteModel
        //{
        //    OxiteModel model = htmlHelper.ViewData.Model;

        //    if (model.Site.IncludeOpenSearch && htmlHelper.ViewContext.HttpContext.Request.UserAgent.Contains("Windows NT 6.1"))
        //    {
        //        UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

        //        return htmlHelper.Link(string.Format(model.Localize("Search.WindowsSearch", "Search {0} in Windows"), model.Site.DisplayName), urlHelper.AppPath(urlHelper.OpenSearchOSDX()), new { @class = "windowsSearch" });
        //    }

        //    return "";
        //}

        //#endregion

        public static string RouteUrl(this HtmlHelper htmlHelper, string action, string controller, object routeValues)
        {
            RouteValueDictionary rvd = routeValues.ToRouteValueDictionary(controller, action);

            return new UrlHelper(htmlHelper.ViewContext.RequestContext).RouteUrl(rvd);
        }

        public static string RouteUrl<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> expression, object routeValues)
        {
            RouteValueDictionary rvd = expression.ToRouteValueDictionary(routeValues);

            return new UrlHelper(htmlHelper.ViewContext.RequestContext).RouteUrl(rvd);
        }

        private static ViewDataDictionary ToViewData(object obj)
        {
            var ve = new ViewDataDictionary();
            ve.Merge(HtmlHelper.AnonymousObjectToHtmlAttributes(obj));
            return ve;
        }

        private static ThModel GetThModel(Ths ths)
        {
            switch (ths)
            {
                case Mvc.Ths.Operation:
                    return new ThModel { Text = ExtensionsHelper.Localize(Global, "operation", "operation") };
                default:
                    throw new ArgumentOutOfRangeException("ths");
            }
        }

        private static MvcHtmlString Td<TModel>(this HtmlHelper<TModel> htmlHelper, PropertyDescriptor property, object item)
        {
            object value = property.GetValue(item);

            if (value != null)
            {
                var type = value.GetType();
                if (Type.GetTypeCode(type) != TypeCode.Object || (type.IsGenericType && type.GetGenericTypeDefinition() != typeof(Nullable<>)))
                    value = property.FormatedShortName(value);
            }
            return htmlHelper.Partial(GetTemplateName("td"), value ?? "");
        }

        private static string GetTemplateName(string name)
        {
            return "templates/" + name;
        }
        public static MvcHtmlString ExplainLocalize(this HtmlHelper htmlHelper, PropertyDescriptor expression)
        {
            var model = new LabelModel();
            model.FieldName = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(expression.PropertyName);
            model.Value = expression.Explain;
            return htmlHelper.Partial("EditorTemplates/Explain", model);
        }

       

        #region RenderCssFile

        public static void RenderCssFile<TModel>(this HtmlHelper<TModel> htmlHelper, string path) where TModel : EcardModel
        {
            htmlHelper.RenderCssFile(path, null);
        }

        public static void RenderCssFile<TModel>(this HtmlHelper<TModel> htmlHelper, string path, string releasePath) where TModel : EcardModel
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

#if DEBUG
#else
            if (!string.IsNullOrEmpty(releasePath))
                path = releasePath;
#endif

            if (!(path.StartsWith("http://") || path.StartsWith("https://")))
            {
                var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

                path = !path.StartsWith("/")
                           ? urlHelper.CssPath(path, htmlHelper.ViewData.Model)
                           : urlHelper.AppPath(path);
            }

            htmlHelper.ViewContext.HttpContext.Response.Write(
                htmlHelper.HeadLink("stylesheet", path, "text/css", "")
                );
        }

        #endregion

        #region RenderFavIcon

        public static void RenderFavIcon<TModel>(this HtmlHelper<TModel> htmlHelper) where TModel : EcardModel
        {
            EcardModel model = htmlHelper.ViewData.Model;

            if (!string.IsNullOrEmpty(model.Site.FavIconUrl))
            {
                var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

                htmlHelper.ViewContext.HttpContext.Response.Write(htmlHelper.HeadLink("shortcut icon", urlHelper.AppPath(model.Site.FavIconUrl), null, null));
            }
        }

        #endregion

        #region RenderFeedDiscovery

        public static void RenderFeedDiscoveryRss(this HtmlHelper htmlHelper, string title, string url)
        {
            htmlHelper.RenderFeedDiscovery(title, url, "application/rss+xml");
        }

        public static void RenderFeedDiscoveryAtom(this HtmlHelper htmlHelper, string title, string url)
        {
            htmlHelper.RenderFeedDiscovery(title, url, "application/atom+xml");
        }

        public static void RenderFeedDiscovery(this HtmlHelper htmlHelper, string title, string url, string type)
        {
            htmlHelper.ViewContext.HttpContext.Response.Write(
                htmlHelper.HeadLink("alternate", url, type, title)
                );
        }

        #endregion

        #region RenderLiveWriterManifest

        public static void RenderLiveWriterManifest(this HtmlHelper htmlHelper)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            htmlHelper.RenderLiveWriterManifest(urlHelper.AppPath("~/LiveWriterManifest.xml"));
        }

        public static void RenderLiveWriterManifest(this HtmlHelper htmlHelper, string path)
        {
            htmlHelper.ViewContext.HttpContext.Response.Write(
                htmlHelper.HeadLink(
                    "wlwmanifest",
                    path,
                    "application/wlwmanifest+xml",
                    ""
                    )
                );
        }

        #endregion

        #region RenderOpenSearch

        public static void RenderOpenSearch<TModel>(this HtmlHelper<TModel> htmlHelper) where TModel : EcardModel
        {
            EcardModel model = htmlHelper.ViewData.Model;

            if (model.Site.IncludeOpenSearch)
            {
                var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

                htmlHelper.ViewContext.HttpContext.Response.Write(htmlHelper.HeadLink("search", urlHelper.AbsolutePath(urlHelper.OpenSearch()),
                                                                                      "application/opensearchdescription+xml",
                                                                                      string.Format(model.Localize("SearchFormat", "{0} Search"), model.Site.DisplayName)));
            }
        }

        #endregion

        #region RenderRsd

        public static void RenderRsd(this HtmlHelper htmlHelper, string areaName)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            htmlHelper.ViewContext.HttpContext.Response.Write(
                htmlHelper.HeadLink(
                    "EditURI",
                    urlHelper.AbsolutePath(urlHelper.Rsd(areaName)),
                    "application/rsd+xml",
                    "RSD"
                    )
                );
        }

        #endregion

        #region RenderScriptVariable

        public static void RenderScriptVariable(this HtmlHelper htmlHelper, string name, object value)
        {
            const string scriptVariableFormat = "window.{0} = {1};";
            string script;

            if (value != null)
            {
                var dcjs = new DataContractJsonSerializer(value.GetType());

                using (var ms = new MemoryStream())
                {
                    dcjs.WriteObject(ms, value);

                    script = string.Format(scriptVariableFormat, name, Encoding.Default.GetString(ms.ToArray()));

                    ms.Close();
                }
            }
            else
            {
                script = string.Format(scriptVariableFormat, name, "null");
            }

            htmlHelper.ViewContext.HttpContext.Response.Write(script);
        }

        #endregion

        #region Private Methods

        private static string fieldWithLabel<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<string> renderText, string labelInnerHtml, bool enabled)
            where TModel : EcardModel
        {
            var output = new StringBuilder(200);

            if (!string.IsNullOrEmpty(labelInnerHtml))
            {
                var builder = new TagBuilder("label");

                builder.Attributes["for"] = name;
                builder.InnerHtml = labelInnerHtml;

                output.Append(builder.ToString());
                output.Append(" ");
            }

            output.Append(renderText());

            return output.ToString();
        }

        #endregion

        #region HeadLink

        public static string HeadLink(this HtmlHelper htmlHelper, string rel, string href, string type, string title)
        {
            return htmlHelper.HeadLink(rel, href, type, title, null);
        }

        public static string HeadLink(this HtmlHelper htmlHelper, string rel, string href, string type, string title, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("link");

            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (!string.IsNullOrEmpty(rel))
            {
                tagBuilder.MergeAttribute("rel", rel);
            }
            if (!string.IsNullOrEmpty(href))
            {
                tagBuilder.MergeAttribute("href", href);
            }
            if (!string.IsNullOrEmpty(type))
            {
                tagBuilder.MergeAttribute("type", type);
            }
            if (!string.IsNullOrEmpty(title))
            {
                tagBuilder.MergeAttribute("title", title);
            }

            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        #endregion

        #region Image

        public static string Image(this HtmlHelper helper, string src, string alt, IDictionary<string, object> htmlAttributes)
        {
            var url = new UrlHelper(helper.ViewContext.RequestContext);
            string imageUrl = url.Content(src);
            var imageTag = new TagBuilder("img");

            if (!string.IsNullOrEmpty(imageUrl))
            {
                imageTag.MergeAttribute("src", imageUrl);
            }

            if (!string.IsNullOrEmpty(alt))
            {
                imageTag.MergeAttribute("alt", alt);
            }

            imageTag.MergeAttributes(htmlAttributes, true);

            if (imageTag.Attributes.ContainsKey("alt") && !imageTag.Attributes.ContainsKey("title"))
            {
                imageTag.MergeAttribute("title", imageTag.Attributes["alt"] ?? "");
            }

            return imageTag.ToString(TagRenderMode.SelfClosing);
        }

        public static string Image(this HtmlHelper helper, string name, string src, string alt, bool upload, int width, int height, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            string imageUrl = string.Empty;
            if (!string.IsNullOrEmpty(src))
            {
                var url = new UrlHelper(helper.ViewContext.RequestContext);
                imageUrl = url.Content(src);
            }
            var imageTag = new TagBuilder("img");
            imageTag.MergeAttribute("id", name);
            imageTag.MergeAttribute("name", name);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                imageTag.MergeAttribute("src", imageUrl + "?" + Guid.NewGuid().ToString("N"));
            }
            if (upload)
            {
                imageTag.MergeAttribute("class", "__uploader");
            }
            if (width != 0 && height != 0)
            {
                imageTag.MergeAttribute("__arg", "img_" + width + "x" + height);
            }
            if (width != 0)
                htmlAttributes["width"] = width;
            if (height != 0)
                htmlAttributes["height"] = height;
            if (!string.IsNullOrEmpty(alt))
            {
                imageTag.MergeAttribute("alt", alt);
            }

            imageTag.MergeAttributes(htmlAttributes, true);

            if (imageTag.Attributes.ContainsKey("alt") && !imageTag.Attributes.ContainsKey("title"))
            {
                imageTag.MergeAttribute("title", imageTag.Attributes["alt"] ?? "");
            }

            return imageTag.ToString(TagRenderMode.SelfClosing);
        }

        public static string GetImageUrl(this HtmlHelper helper, string pictureName)
        {
            if (string.IsNullOrEmpty(pictureName))
                return "";
            return GlobalEnv.GetImageUrls(pictureName);
        }

        #endregion
    }

    public class LabelModel
    {
        public string Value { get; set; }
        public string FieldName { get; set; }
    }

    public enum Ths
    {
        Select,
        Operation
    }

    public static class ExtensionsHelper
    {
        public static T Resolve<T>()
        {
            var container = (IUnityContainer)HttpContext.Current.Application["container"];
            return container.Resolve<T>();
        }

        public static string Localize(string objectKey, string category, string defaultValue)
        {
            var container = (IUnityContainer)HttpContext.Current.Application["container"];
            var manager = container.Resolve<I18NManager>();
            return manager.Get(objectKey, category, defaultValue);
        }

        public static string Message(string objectKey)
        {
            return Localize("messages", objectKey, objectKey);
        }
    }
}