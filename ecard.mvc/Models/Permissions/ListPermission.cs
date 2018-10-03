using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecard.Models;

namespace Ecard.Mvc.Models.Permissions
{
    public class ListPermission : IKeyObject ,ICategory
    {
        private readonly Permission _innerObject;

        [NoRender]
        public Permission InnerObject
        {
            get { return _innerObject; }
        }

        public ListPermission(Permission innerObject)
        {
            _innerObject = innerObject;
        }

        [NoRender]
        public int PermissionId
        {
            get { return InnerObject.PermissionId; }
        }

        public string Name
        {
            get { return InnerObject.Name; }
            set { InnerObject.Name = value; }
        }

        public string DisplayName
        {
            get { return InnerObject.DisplayName; }
            set { InnerObject.DisplayName = value; }
        }
        int IKeyObject.Id
        {
            get { return PermissionId; }
        }
        [NoRender]
        public string Category
        {
            get { return InnerObject.Category; }
        }
    }

}
