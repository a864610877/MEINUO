using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;
using Moonlit.Text;
using Moonlit.Validations;

namespace Ecard.Mvc.Models.AdminUsers
{
    public class AdminUserModelBase : ViewModelBase
    {
        private AdminUser _innerObject;

        public AdminUserModelBase()
        {
            _innerObject = new AdminUser();
        }

        private Bounded _posNameBounded;

        public Bounded PosName
        {
            get
            {
                if (_posNameBounded == null)
                {
                    _posNameBounded = Bounded.CreateEmpty("PosNameId", 0);
                }
                return _posNameBounded;
            }
            set { _posNameBounded = value; }
        }
        public AdminUserModelBase(AdminUser adminUser)
        {
            _innerObject = adminUser;
        }

        [NoRender]
        public AdminUser InnerObject
        {
            get { return _innerObject; }
        }
        [DataType(DataType.Date)]
        public DateTime? BirthDate
        {
            get { return InnerObject.BirthDate; }
            set { InnerObject.BirthDate = value; }
        }

        [Required(ErrorMessage = "请输入姓名")]
        [StringLength(40)]
        public string DisplayName
        {
            get { return _innerObject.DisplayName; }
            set { _innerObject.DisplayName = value.TrimSafty(); }
        }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "请输入电子邮件")]
        public string Email
        {
            get { return _innerObject.Email; }
            set { _innerObject.Email = value.TrimSafty(); }
        }

        [Mobile]
        public string Mobile
        {
            get { return _innerObject.Mobile; }
            set { _innerObject.Mobile = value.TrimSafty(); }
        }

        //public bool IsSale
        //{
        //    get { return InnerObject.IsSale ?? false; }
        //    set { InnerObject.IsSale = value; }
        //}
        [Dependency]
        [NoRender]
        public IMembershipService MembershipService { get; set; }
        protected void SetInnerObject(AdminUser user)
        {
            _innerObject = user;
        }

        protected void OnReady()
        {
            //var shop = ShopService.GetByName(Constants.SystemShopName);
            //var posList = PosEndPointService.Query(new PosEndPointRequest() { ShopId = shop.ShopId });
            //this.PosName.Bind(posList.Select(x => new IdNamePair() { Key = x.PosEndPointId, Name = x.DisplayName }), true);
            //var currentPos = posList.FirstOrDefault(x => x.CurrentUserId == InnerObject.UserId);
            //if (currentPos != null)
            //    PosName.Key = currentPos.PosEndPointId;
        }

        protected void OnSaved(AdminUser obj)
        {
            //var shop = ShopService.GetByName(Constants.SystemShopName);
            //var posList = PosEndPointService.Query(new PosEndPointRequest() { ShopId = shop.ShopId });
            //var pos = posList.FirstOrDefault(x => x.PosEndPointId == (int)this.PosName);
            //if (pos != null)
            //{
            //    PosEndPointService.UpdateCurrenUserId(pos.PosEndPointId, obj.UserId);
            //}
        }
    }
}