using System;
using Ecard.Models;

namespace Ecard.Mvc.Models.AdminUsers
{
    public class ListAdminUser : IKeyObject
    {
        private readonly AdminUser _innerObject;

        [NoRender]
        public AdminUser InnerObject
        {
            get { return _innerObject; }
        }

        public ListAdminUser()
        {
            _innerObject = new AdminUser();
        }

        public ListAdminUser(AdminUser adminUser)
        {
            _innerObject = adminUser;
        }

        [NoRender]
        public int UserId
        {
            get { return InnerObject.UserId; }
        }
        public string Name
        {
            get { return InnerObject.Name; }
        }

        public DateTime? LastSignInTime
        {
            get { return InnerObject.LastSignInTime; }
        }
        public string DisplayName
        {
            get { return InnerObject.DisplayName; }
        }

        public DateTime? BirthDate
        {
            get { return InnerObject.BirthDate; }
        }

        public string Email
        {
            get { return InnerObject.Email; }
        }

        public string State
        {
            get { return ModelHelper.GetBoundText(InnerObject, x => x.State); }
        }
        public bool? IsSale
        {
            get { return InnerObject.IsSale; }
        }
        [NoRender]
        public string IsSaleStr { get; set; }
        
        int IKeyObject.Id
        {
            get { return UserId; }
        }
        [NoRender]
        public string boor { get; set; }
    }
}