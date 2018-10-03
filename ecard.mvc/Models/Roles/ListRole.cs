using System;
using Ecard.Models;
using Ecard.Mvc.ViewModels; 

namespace Ecard.Mvc.Models.Roles
{
    public class ListRole :EcardModel, IKeyObject
    {
        private readonly Role _innerObject;

        [NoRender]
        public Role InnerObject
        {
            get { return _innerObject; }
        }

        public ListRole()
        {
            _innerObject = new Role();
        }

        public ListRole(Role adminUser)
        {
            _innerObject = adminUser;
        }

        [NoRender]
        public int RoleId
        {
            get { return InnerObject.RoleId; }
        }
        public string Name
        {
            get { return InnerObject.Name; }
        }

        public string DisplayName
        {
            get { return InnerObject.DisplayName; }
        }

        public string State
        {
            get { return ModelHelper.GetBoundText(InnerObject, x => x.State); }
        }

        int IKeyObject.Id
        {
            get { return RoleId; }
        }
        [NoRender]
        public string boor { get; set; }
    }
}