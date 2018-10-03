//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Web.Mvc;
using Ecard.Models;
using Oxite.Mvc.Infrastructure;

namespace Ecard.Mvc.ActionFilters
{
    /// <summary>
    /// 防止重放 攻击
    /// 如果 route 中标记当前  route 需要验证 防重放，并且是 post 数据，则校验.
    /// 校验规则：时间没有被修改过，并且时间为 60 分钟以内
    /// </summary>
    public class AntiForgeryAuthorizationFilter : IAuthorizationFilter
    {
        private readonly Site site;

        public AntiForgeryAuthorizationFilter(Site site)
        {
            this.site = site;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!(filterContext.RouteData.Values["validateAntiForgeryToken"] is bool
                && (bool)filterContext.RouteData.Values["validateAntiForgeryToken"]
                && filterContext.HttpContext.Request.HttpMethod == "POST"
                && filterContext.RequestContext.HttpContext.Request.IsAuthenticated))
            {
                return;
            }

            string salt = site.SiteId.ToString();

            AntiForgeryToken antiForgeryToken = new AntiForgeryToken(salt);

            long ticks;

            string formTicks = filterContext.HttpContext.Request.Cookies.Get(AntiForgeryToken.TicksName) != null
                ? filterContext.HttpContext.Request.Cookies.Get(AntiForgeryToken.TicksName).Value
                : null;

            string formHash = filterContext.HttpContext.Request.Form[AntiForgeryToken.TokenName];

            if (string.IsNullOrEmpty(formTicks) || !long.TryParse(formTicks, out ticks) || string.IsNullOrEmpty(formHash))
            {
                throw new Exception("Bad Anti-Forgery Token");
            }

            TimeSpan timeOffset = new TimeSpan(antiForgeryToken.Ticks - ticks);

            // todo: (nheskew)drop the time span into some configurable property
            // and handle the "exception" better than just throwing one. ideally we should give the form back, populated with the same data, with a 
            // message saying something like "fall asleep at your desk? writing a novel? try that submission again!"
            if (!(AntiForgeryToken.GetHash(salt, ticks.ToString()) == formHash && timeOffset.TotalMinutes < 60))
            {
                throw new Exception("Bad Anti-Forgery Token");
            }
        }
    }
}
