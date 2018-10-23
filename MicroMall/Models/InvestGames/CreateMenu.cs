using Ecard.Infrastructure;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WxPayAPI;

namespace MicroMall.Models.InvestGames
{
    public class CreateMenu
    {

        [Dependency]
        public ILog4netService logService { get; set; }
        [Dependency]
        public ISetWeChatService SetWeChatService { get; set; }
        [Dependency]
        public IAccountService AccountService { get; set; }
        public void Create()
        {
            try
            {
                var access_token = CachePools.GetData(CacheKeys.access_token);
                if (access_token == null)
                {
                    logService.Insert("access_token为null!");
                    return;
                }
                access_token = access_token.ToString();
                string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token="+ access_token;
                string param = setMenu(); //string.Format("grant_type=client_credential&appid={0}&secret={1}", setWechat.appID, setWechat.AppSecret);
                var reult = HttpSend.postSend(url, param);
                var model = JsonConvert.DeserializeObject<MessageModel>(reult);
                if (model.errcode == 0)
                {
                    logService.Insert("菜单创建成功！");
                }
                else
                {
                    logService.Insert(string.Format("创建失败:{0}", model.errmsg));
                }
            }
            catch(Exception ex)
            {
                logService.Insert(ex);
            }
        }

        public string setMenu()
        {
            string menu1 = HttpUtility.HtmlEncode("我的菜单");
            string menu2 = HttpUtility.HtmlEncode("商城");
            string menu3 = HttpUtility.HtmlEncode("进入商城");
            string str = "{ \"button\":[{\"name\":\"My Menu\",\"sub_button\":[{\"type\":\"view\",\"name\":\"My xx\",\"url\":\"http://tecent.fuzhongcs.com/scroll/index.html\" },{\"type\":\"click\",\"name\":\"My dd\",\"key\":\"0002\"}]},{ \"name\":\"商城\",\"sub_button\":[{\"name\":\"进入商城\",\"type\":\"view\",\"url\":\"http://tecent.fuzhongcs.com/shop/shop.html\"}] }]}";
            return str;
        }

        public string MpCreate()
        {
            //var set = SetWeChatService.GetById(1);
            var access_token = AccessTokenContainer.GetToken(WxPayConfig.APPID);// Senparc.Weixin.MP.CommonAPIs.CommonApi.GetToken(set.appID, set.AppSecret).access_token;          
            ButtonGroup bg = new ButtonGroup();
            bg.button.Add(new SingleViewButton
            { 
              name="微信商城",
              type = ButtonType.view.ToString(),
              url =System.Configuration.ConfigurationManager.AppSettings["wxUrl"].ToString(),
            });
            bg.button.Add(new SingleViewButton()
            {
                name = "秒杀专区",
                type = ButtonType.view.ToString(),
                url = System.Configuration.ConfigurationManager.AppSettings["wxUrl1"].ToString(),
            });
            var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.CreateMenu(access_token, bg);
            return result.errmsg;
        }

        public CreateQrCodeResult CreateQrCode(Stream image)
        {
            var set = SetWeChatService.GetById(1);
            var access_token = AccessTokenContainer.GetToken(set.appID);
            return Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.Create(access_token,0,001);
            
        }

        public void GetQrCode()
        {


            //Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.ShowQrCode(reult.ticket, image);
        }
    }
  
}