using Ecard.Models;
using Ecard.Mvc;
using Ecard.Services;
using MicroMall.Models.Commoditys;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperExtensions
    {
        //[Dependency]
        //public static IProvinceService ProvinceService { get; set; }
        //[Dependency]
        //public static ICityService CityService { get; set; }

        //[Dependency]
        //public SecurityHelper _securityHelper { get; set; }

        //public 
        public static MvcHtmlString ListAttribute(this HtmlHelper helper, List<SpecificationAndSpecificationDetail> list,string imageUrl)
        {
            StringBuilder html = new StringBuilder();
            if(list!=null)
            {
                string AttributeName = "Attribute_";
                int i = 1;
                foreach(var item in list)
                {
                    if (item.model != null && item.list != null&&item.list.Count>0)
                    {
                        html.Append("<dl>");
                        html.Append(string.Format("<dt>{0}:</dt>", item.model.Name));
                        string IdName = AttributeName + i;
                        html.Append("<dd><ul id=\"" + IdName + "\">");
                        foreach (var model in item.list)
                        {
                            string li = "";
                            var name = "a_" + i;
                            if (item.model.Type == Types.text)
                            {
                                 li = "<li><a name=\""+name+"\" href=\"javascript:;\" title=\"" + model.describe + "\">" + model.value + "</a></li>";
                            }
                            else
                            {
                              string url = imageUrl + "/CommodityImages/" + model.value;
                              li = "<li><a name=\"" + name + "\" href=\"javascript:;\" title=\"" + model.describe + "\"><img src=\"" + url + "\" width=\"33\" height=\"33\" /></a></li>";
                            }
                            html.Append(li);
                        }
                        html.Append("</dl>");
                        i++;
                    }
                }
            }
            return new MvcHtmlString(html.ToString());
        }

        public static MvcHtmlString ShippingAddress(this HtmlHelper helper,Order item)
        {
            if (item == null)
                return new MvcHtmlString("");
            if(item.provinceId<=0||item.cityId<=0||string.IsNullOrWhiteSpace(item.detailedAddress)||string.IsNullOrWhiteSpace(item.recipients)||string.IsNullOrWhiteSpace(item.moblie))
            {
                string html = "<div class='addressnaniu'><button type='button' naem='add' class='am-btn am-btn-primary am-btn-block  am-round' id='doc-prompt-toggle'>添加收货地址</button></div>";
                return new MvcHtmlString(html);
            }
            else
            {
                string ProvinceCity = "";
                string ProvinceName = GetProvinceName(item.provinceId);
                string CityName = GetCityName(item.cityId);
                if (ProvinceName == CityName)
                    ProvinceCity = ProvinceName;
                else
                {
                    ProvinceCity = ProvinceName + CityName;
                }

                string html = "<h2 class='dingdan_h2'>送货地址</h2><div class=\"address\">";
                html += "<h3>收货人：" + item.recipients + "</h3>";
                html += "<p class='address_tel '>手机："+item.moblie+"</p>";
                html += string.Format("<p class='address_xx'>收货地址：{0}{1}</p>", ProvinceCity.ToString(), item.detailedAddress);
                html += "  <button type=\"button\" class=\"am-btn am-btn-primary am-radius am-btn-xs\" id=\"doc-prompt-toggle\">编辑</button></div>";
                return new MvcHtmlString(html);
            }
        }

        public static MvcHtmlString ListOrederDetail(this HtmlHelper helper, List<OrderDetail> OrderDetails, string imageUrl)
        {
            StringBuilder html = new StringBuilder();
            if (OrderDetails != null && OrderDetails.Count() > 0)
            {
                var CommodityService = ExtensionsHelper.Resolve<ICommodityService>();
                foreach (var item in OrderDetails)
                {
                    string img = "";
                    var model = CommodityService.GetById(item.commodityId);
                    if (model != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.images))
                        {
                            string[] sp = model.images.Split(',');
                            if (sp.Count() > 0)
                                img = sp[0];
                        }
                        var id = "commodity_" + model.commodityId;
                        html.Append("<div class='myfavorites'>"); //
                        html.Append(string.Format("<div class='myfavorites_img'><img class='am-radius am-img-thumbnail' alt='140*140' src='{0}' width='85' height='85' /></div>", imageUrl + "/CommodityImages/" + img));
                        html.Append(string.Format("<div class='myfavorites_top'> <input type='hidden' value=\"{0}\" data-type=\"commodityId\" />", model.commodityId));
                        html.Append("<div class='myfavorites_title' onclick=\"window.location.href='/Commodity/CommodityDetail?id=" + model.commodityId + "'\"><h3>" + model.commodityName + "</h3><p>" + model.commodityRemark + "</p></div>");
                        html.Append(string.Format("<div class='myfavorites_sj'><input type='hidden' data-type=\"price\" value=\"{0}\" /><span class='jiage'>￥{0}</span>", item.price));
                        html.Append(string.Format("  <span class=\"shuliang\">数量：" + item.quantity + "</span></div>"));//<a href=\"javascript:;\" onclick=\"minus(this)\"  class=\"inputcheckbox\">-</a>
                        //html.Append(string.Format("<div class=\"count-input\"><input type=\"text\" class=\"num\"  value=\"{0}\" readonly></div> </span></div>", item.quantity));    //<a href=\"javascript:;\"  onclick=\"add(this)\" class=\"inputcheckbox\">+</a>                    
                        html.Append(string.Format("<div class=\"shuxingzhi\">"));
                        if (!string.IsNullOrWhiteSpace(item.specification))
                        {
                            string[] sp = item.specification.Split(',');
                            if(sp.Count()>0)
                            {
                               foreach(var str in sp)
                               {
                                   if (!string.IsNullOrWhiteSpace(str))
                                       html.Append("<span>"+str+"</span>");
                               }
                            }
                        }
                        html.Append("</div>");
                        html.Append("</div>");
                        //html.Append("<a href=\"javascript:;\" class=\"am-close am-close-alt am-icon-times shanchu\" title=\"删除\"></a>");
                        html.Append("</div>");
                    }
                }
            }
            return new MvcHtmlString(html.ToString());
        }

        public static MvcHtmlString OrederDetail(this HtmlHelper helper, List<OrderDetail> OrderDetails, string imageUrl)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<ul class=\"dingdanshangping\">");
            if (OrderDetails != null && OrderDetails.Count() > 0)
            {
                var CommodityService = ExtensionsHelper.Resolve<ICommodityService>();
                foreach (var item in OrderDetails)
                {
                    string img = "";
                    var model = CommodityService.GetById(item.commodityId);
                    if (model != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.images))
                        {
                            string[] sp = model.images.Split(',');
                            if (sp.Count() > 0)
                                img = sp[0];
                        }
                        html.Append("<li><div class=\"list_pro\">");
                        html.Append(string.Format("<a href=\"{0}\"><img src=\"{1}\" width=\"80\" /></a>", "/Commodity/CommodityDetail?id=" + item.commodityId, imageUrl + "/CommodityImages/" + img));
                        html.Append(string.Format(" <p><a href=\"{0}\">{1}</a></p>", "/Commodity/CommodityDetail?id=" + item.commodityId,item.commodityName));
                        html.Append("</div>");
                        html.Append("<p class=\"shijifukuan\">");
                        if (!string.IsNullOrWhiteSpace(item.specification))
                        {
                            string[] sp = item.specification.Split(',');
                            if (sp.Count() > 0)
                            {
                                foreach (var str in sp)
                                {
                                    if (!string.IsNullOrWhiteSpace(str))
                                        html.Append("<span>" + str + "</span>");
                                }
                            }
                        }
                        html.Append("</p>");
                        html.Append("<p class=\"shijifukuan\"><span class=\"zhifujingea\">金额：￥" + item.price + "</span><span>数量：" + item.quantity + "</span></p></li>");
                    }
                }
            }
            html.Append("</ul>");
            return new MvcHtmlString(html.ToString());
        }

        public static MvcHtmlString Specification(this HtmlHelper helper, string specification)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='shuxingzhi'>"); 
            if(!string.IsNullOrWhiteSpace(specification))
            {
               string[] sp = specification.Split(',');
               for (int i = 0; i < sp.Count(); i++)
               {
                   if(sp[i]!="")
                     sb.Append("<span>" + sp[i] + "</span>");
               }
            }
            sb.Append("</div>");
            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString ReviewView(this HtmlHelper helper,List<ReviewView> List)
        {
            StringBuilder sb = new StringBuilder();
           if(List!=null)
           {
               foreach(var item in List)
               {
                   sb.Append("<li class=\"am-comment\">");
                   sb.Append("<article class=\"am-comment\">");
                   sb.Append("<a href=\"#link-to-user-home\"><img src=\""+item.Url+"\" onerror='this.src=\"/images/default02.png\"'  class=\"am-comment-avatar\" width=\"48\" height=\"48\" /></a>");
                   sb.Append("<div class=\"am-comment-main\">");
                   sb.Append("<header class=\"am-comment-hd\">");
                   sb.Append(" <div class=\"am-comment-meta\">");
                   sb.Append("<a href=\"#link-to-user\" class=\"am-comment-author\">" + item.UserName + "</a>");
                   var security = ExtensionsHelper.Resolve<SecurityHelper>();
                   if (security!=null)
                   {
                       var user = security.GetCurrentUser();
                       if(user!=null)
                       {
                           if(user.CurrentUser.UserId==item.UserId)
                           {
                              sb.Append(" <button type=\"button\" data-type='deleteReview' data-ReviewId='"+item.ReviewId+"' class=\"am-close\">&times;</button>");
                           }
                       }
                   }
                   sb.Append("</div>");
                   sb.Append("</header>");
                   sb.Append(" <div class=\"am-comment-bd\">" + item.Content + "</div>");
                   sb.Append("<div class=\"reviews_time\"><time   class=\"reviewstime\">"+item.SubmitTime+"</time></div>");
                   sb.Append("</div>");
                   sb.Append("</article>");
                   sb.Append("</li>");
               }
           }
           return new MvcHtmlString(sb.ToString());
        }

    
        private static string GetProvinceName(int id)
        {
            var ProvinceService = ExtensionsHelper.Resolve<IProvinceService>();
            var item = ProvinceService.GetById(id);
            if (item != null)
                return item.Name;
            return "";
        }

        private static string GetCityName(int id)
        {
           var CityService = ExtensionsHelper.Resolve<ICityService>();
           var item = CityService.GetById(id);
           if (item != null)
               return item.Name;
           return "";
        }
    }


    public static class ExtensionsHelper
    {
        public static T Resolve<T>()
        {
            var container = (IUnityContainer)HttpContext.Current.Application["container"];
            return container.Resolve<T>();
        }

        //public static string Localize(string objectKey, string category, string defaultValue)
        //{
        //    var container = (IUnityContainer)HttpContext.Current.Application["container"];
        //    var manager = container.Resolve<I18NManager>();
        //    return manager.Get(objectKey, category, defaultValue);
        //}

        //public static string Message(string objectKey)
        //{
        //    return Localize("messages", objectKey, objectKey);
        //}
    }
}