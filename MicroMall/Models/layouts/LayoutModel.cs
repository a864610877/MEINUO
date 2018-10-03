using Ecard;
using Ecard.Models;
using Ecard.Mvc;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.layouts
{
    public class LayoutModel
    {

        public LayoutModel()
        {
           
        }

        [Dependency]
        public ISiteService SiteService { get; set; }
         [Dependency]
        public ISetWeChatService SetWeChatService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Dependency]
        public ILog4netService logService { get; set; }
         [Dependency]
        public IAccountService AccountService { get; set; }
        [Dependency]
        public IProvinceService ProvinceService { get; set; }
        [Dependency]
        public ICityService CityService { get; set; }
        [Dependency]
        public IRecommendLogService RecommendLogService { get; set; }
        [Dependency]
        public TransactionHelper TransactionHelper { get; set; }
        protected User _user { get; set; }
        /// <summary>
        /// 登录会员信息
        /// </summary>
        public Account Account { get; set; }
        /// <summary>
        /// 登录用户信息
        /// </summary>
        public User UserInformation { get { return _user; } }

        public string QrCodeUrl { get; set; }
        /// <summary>
        /// 全局图片域名路径
        /// </summary>
        public string ImageUrl { get;set; }
        /// <summary>
        /// 积分兑换比例
        /// </summary>
        public decimal PointRatio { get; set; }
        /// <summary>
        /// 推荐人返利比例
        /// </summary>
        public decimal RebateRatio { get; set; }

        [Dependency]
        public SecurityHelper _securityHelper { get; set; }

       

        //[Dependency]
        //public ISiteService SiteService { get; set; }

        public string LogonToken { get { return Guid.NewGuid().ToString("N"); } }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Load()
        {
            var user = _securityHelper.GetCurrentUser();
            if (user != null)
                _user = user.CurrentUser;

            if(_user!=null)
            {
                var account = AccountService.GetByUserId(_user.UserId);
                if(account!=null)
                {
                    if(string.IsNullOrWhiteSpace(account.ticket))
                    {
                        var set = SetWeChatService.GetById(1);
                        var access_token = AccessTokenContainer.GetToken(set.appID);
                        var reult= Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.CreateByStr(access_token, account.orangeKey,5000);
                        account.ticket = reult.ticket;
                        account.qrCodeUrl = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.GetShowQrCodeUrl(account.ticket);
                        AccountService.Update(account);
                    }
                    else if(string.IsNullOrWhiteSpace(account.qrCodeUrl))
                    {
                        account.qrCodeUrl = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.GetShowQrCodeUrl(account.ticket);
                        AccountService.Update(account);
                    }
                    QrCodeUrl = account.qrCodeUrl;
                    Account = account;
                }
            }
            Url();
        }

        

        /// <summary>
        /// 获取所有的省
        /// </summary>
        /// <returns></returns>
        public List<Province> GetProvinceAll()
        {
            var list = ProvinceService.Query().ToList();
            return list;
        }
        /// <summary>
        ///根据省获取市
        /// </summary>
        /// <param name="ProvinceId"></param>
        /// <returns></returns>
        public List<City> GetCity(int ProvinceId)
        {
            var list = CityService.Query(ProvinceId).ToList();
            return list;
        }

        public string GetProvinceName(int id)
        {
            var item = ProvinceService.GetById(id);
            if (item != null)
                return item.Name;
            return "";
        }

        public string GetCityName(int id)
        {
            var item = CityService.GetById(id);
            if(item!=null)
                return item.Name;
            return "";
        }
        /// <summary>
        /// 获取用户推荐人数
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public int GetRecommendCount(string UserName)
        {
            return RecommendLogService.RecommendCount(UserName);
        }

        public void Url()
        {
            var site = SiteService.Query(new SiteRequest()).FirstOrDefault();
            if (site != null)
            {
                ImageUrl = site.imageUrl;
                PointRatio = site.PointRatio;
                RebateRatio = site.RebateRatio;
            }
        }
    }
}