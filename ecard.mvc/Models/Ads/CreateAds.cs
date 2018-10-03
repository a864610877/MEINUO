using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Ads
{
    public class CreateAds : Ad
    {
        [Dependency, NoRender]
        public IAdService adService { get; set; }

        public ResultMsg Create()
        {
            ResultMsg result = new ResultMsg();
            Ad ad = new Ad();

            try
            {
                ad.State = this.State;
                ad.rank = this.rank;
                ad.title = this.title;
                ad.link = this.link;
                ad.submitTime = DateTime.Now;
                ad.ImageUrl = this.ImageUrl;
                adService.Insert(ad);
                result.Code = 1;
                result.CodeText = "发布成功!";
                return result;
            }
            catch (Exception)
            {
                result.Code = 2;
                result.CodeText = "系统错误!";
                return result;
            }

        }
    }
}
