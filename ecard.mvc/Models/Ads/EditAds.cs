using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Ads
{
    public class EditAds: EcardModelListRequest<Ad>
    {

        [Dependency]
        [NoRender]
        public IAdService adService { get; set; }
        [NoRender]
        public int adId { get; set; }
        public string title { get; set; }
        public string ImageUrl { get; set; }
        public string link { get; set; }
        public int state { get; set; }
        public int rank { get; set; }

        public void Ready(int id) 
        {
            var ad = adService.GetById(id);
            if (ad!=null)
            {
                this.adId = ad.adId;
                this.title = ad.title;
                this.ImageUrl = ad.ImageUrl;
                this.link = ad.link;
                this.state = ad.State;
                this.rank = ad.rank;
            }
        }


        public ResultMsg Save()
        {
            ResultMsg result = new ResultMsg();
            var ad = adService.GetById(this.adId);
            if (ad!=null)
            {
                ad.title = this.title;
                ad.rank = this.rank;
                ad.link = this.link;
                ad.State = this.state;
                ad.ImageUrl = this.ImageUrl;
                adService.Update(ad);
                result.Code = 1;
                result.CodeText = "修改成功!";
                return result;
            }
            else
            {
                result.Code = 2;
                result.CodeText = "修改失败!";
                return result;
            }
        }
    }
}
