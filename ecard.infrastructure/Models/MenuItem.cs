using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Ecard.Models
{
    public class MenuItem : NamedEntity
    {
        public MenuItem()
        {
        }

        public MenuItem(string name, string displayName, string description, IPermissionAttribute permission)
            : this(name, displayName, description, permission, "", "", null)
        {
        }

        public int Level { get; set; }
        public MenuItem(string name, string displayName, string description, IPermissionAttribute permission, string controller, string action, string routeValues)
        {
            Controller = controller;
            Action = action;
            DisplayName = displayName;
            Tooltip = description;
            Permission = permission;
            Name = name;
            ParameterObject = (Dictionary<string, object>)(string.IsNullOrWhiteSpace(routeValues)
                                                ? new Dictionary<string, object>()
                                                : new JavaScriptSerializer().DeserializeObject(routeValues));
        }

        public string Tooltip { get; set; }
        public IPermissionAttribute Permission { get; set; }
        public Dictionary<string, object> ParameterObject { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public void Add(MenuItem menuItem)
        {
            _children.Add(menuItem); 
            menuItem._parent = this;
        }

        public MenuItem Parent
        {
            get
            {
                return _parent;
            }
        }

        private List<MenuItem> _children = new List<MenuItem>();
        private MenuItem _parent;

        public IEnumerable<MenuItem> Children
        {
            get { return _children; }
            set
            {
                foreach (var menuItem in value)
                {
                    this.Add(menuItem);
                }
            }
        }

        public bool IsSameAction(MenuItem other)
        {
            if (string.Equals(other.Controller, Controller, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(other.Action, Action, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        public MenuItem Clone()
        {
            return new MenuItem(Name, DisplayName, Tooltip, this.Permission)
                       {
                           Controller = this.Controller,
                           Action = Action,
                           ParameterObject = ParameterObject,
                           Level = Level
                       };
        }

        public MenuItem FirstActionMenu()
        {
            if (!string.IsNullOrWhiteSpace(this.Controller) && !string.IsNullOrWhiteSpace(Action))
                return this;
            foreach (var child in Children)
            {
                var m = child.FirstActionMenu();
                if (m != null)
                    return m;
            }
            return null;
        }
    }
}