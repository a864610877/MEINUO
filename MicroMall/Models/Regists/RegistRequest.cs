using Ecard;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Services;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Regists
{
    public class RegistRequest : LayoutModel
    {
        public string orangeKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string openID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 省id
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string PasswordConfirm { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }
        public void Set(RegistModel model)
        {
            //this.Address = model.Address;
            //this.CityId = model.CityId;
            //this.Email = model.Email;
            this.Mobile = model.Mobile;
            //this.Name = model.Name;
            //this.openID = model.openID;
            this.orangeKey = model.orangeKey;
            this.Password = model.Password;
            this.PasswordConfirm = model.PasswordConfirm;
            //this.ProvinceId = model.ProvinceId;
            //this.Sex = model.Sex;
        }
       public List<Province> Provinces { get; set; }
       [Dependency]
        public IAccountService AccountService { get; set; }
        [Dependency]
        public IOrangeKeyAndopenIDService OrangeKeyAndopenIDService { get; set; }
        [Dependency]
        public IMembershipService MembershipService { get; set; }
        //[Dependency]
        //public ISiteService SiteService { get; set; }
        [Dependency]
        public IRecommendLogService RecommendLogService { get; set; }
        [Dependency]
        public IOperationPointLogService OperationPointLogService { get; set; }
        [Dependency]
        public ILog4netService Log4netService { get; set; }
        //[Dependency]
        //public IProvinceService ProvinceService { get; set; }

        //[Dependency]
        //public ISetWeChatService SetWeChatService { get; set; }





        public void Ready(string orangeKey)
        {
            //var item = AccountService.GetByorangeKey(orangeKey); //OrangeKeyAndopenIDService.GetByopenID(openId);
            //if(item!=null)
            //{
            this.orangeKey = orangeKey;
                //openID = item.openID;
            //}
            //else
            //{
            //    orangeKey = "";
            //}
            //openID = "123123213";
            //Provinces = new List<Province>();
           // Provinces = ProvinceService.Query().ToList();
           // Load();
        }

        //public CreateQrCodeResult CreateQrCode()
        //{
        //    var set = SetWeChatService.GetById(1);
        //    var access_token = AccessTokenContainer.GetToken(set.appID);
        //    return Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.Create(access_token, 0, 001);

        //}

        public ResultMessage Save()
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(openID))
                //    return new ResultMessage() { Code = -1, Msg = "请重新进入页面！" };
                //var account = AccountService.GetByopenID(openID);
                //if (account != null)
                //    return new ResultMessage() { Code = -1, Msg = "你已注册！" };
                var user = MembershipService.GetByMobile(Mobile);
                if (user != null)
                    return new ResultMessage() { Code = -1, Msg = "手机号码已注册，请更换！" };
                if (string.IsNullOrWhiteSpace(Password))
                    return new ResultMessage() { Code = -1, Msg = "请输入密码！" };
                if (Password != PasswordConfirm)
                    return new ResultMessage() { Code = -1, Msg = "两次输入密码不一致！" };
                var site = SiteService.Query(new SiteRequest()).FirstOrDefault();
                int salerId = 0;
                Account saleAccont = null;
                if (!string.IsNullOrWhiteSpace(orangeKey))
                {
                    saleAccont = AccountService.GetByorangeKey(orangeKey);
                    if (saleAccont == null)
                        return new ResultMessage() { Code = -2, Msg = "推荐码不存在！" };
                    salerId = saleAccont.accountId;
                }
                TransactionHelper.BeginTransaction();
                AccountUser modelUser = new AccountUser();
                modelUser.Address = Address;
                modelUser.DisplayName = Name;
                modelUser.Email = Email;
                modelUser.Gender = Sex;
                modelUser.Mobile = Mobile;
                modelUser.Name = Mobile;
                modelUser.SetPassword(Password);
                modelUser.State = UserStates.Normal;
                MembershipService.CreateUser(modelUser);
                Account modelAccount = new Account();
                //var QrCodeResult = CreateQrCode();
                //modelAccount.ticket = QrCodeResult.ticket;
                modelAccount.activatePoint = 0;
                //modelAccount.notActivatePoint = 0;
                modelAccount.orangeKey = modelUser.UserId.ToString().PadLeft(modelUser.UserId.ToString().Length+2,'0');
                //modelAccount.payPoint = 0;
                modelAccount.openID = openID;
                //modelAccount.presentExp = site.givePoint;
                modelAccount.salerId = salerId;
                modelAccount.state = AccountStates.Normal;
                modelAccount.submitTime = DateTime.Now;
                modelAccount.userId = modelUser.UserId;
                modelAccount.grade = AccountGrade.not;
                //modelAccount.withdrawPoint = 0;
                
                AccountService.Insert(modelAccount);
                //OperationPointLog log = new OperationPointLog();
                //log.account = OperationPointLogTypes.presentExp;
                //log.point = site.givePoint;
                //log.remark = "注册赠送";
                //log.submitTime = DateTime.Now;
                //log.userId = modelAccount.userId;
                //OperationPointLogService.Insert(log);
                if (salerId > 0)
                {
                    RecommendLog recommendlog = new RecommendLog();
                    recommendlog.remark = string.Format("推荐了{0}", Mobile);
                    recommendlog.salerId = saleAccont.userId;
                    recommendlog.submitTime = DateTime.Now;
                    recommendlog.userId = modelUser.UserId;
                    recommendlog.userName = Mobile;
                    RecommendLogService.Insert(recommendlog);
                    //if (site.recommendPoint > 0)
                    //{
                    //    //saleAccont.notActivatePoint += site.recommendPoint;
                    //    OperationPointLog pointlog = new OperationPointLog();
                    //    pointlog.account = OperationPointLogTypes.notActivatePoint;
                    //    pointlog.point = site.recommendPoint;
                    //    pointlog.remark = string.Format("推荐会员{0}", modelUser.DisplayName);
                    //    pointlog.submitTime = DateTime.Now;
                    //    pointlog.userId = saleAccont.userId;
                    //    OperationPointLogService.Insert(pointlog);
                    //    AccountService.Update(saleAccont);
                    //}
                }
                TransactionHelper.Commit();
                return new ResultMessage() { Code = 0 };
            }
            catch(Exception ex)
            {
                Log4netService.Insert(ex);
                return new ResultMessage() { Code = -1, Msg = "系统错误!" };
            }
        }

    }
}