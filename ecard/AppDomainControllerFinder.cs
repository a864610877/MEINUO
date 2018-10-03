using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Ecard.Services;
using Moonlit;

namespace Ecard.Models
{
    public class XmlReflectionMenuService : IMenuService
    {
        private readonly I18NManager _i18NManager;
        private readonly IControllerFinder _controllerFinder;
        private static object _menuItemsLocker = new object();
        private static List<MenuItem> _menuItems;
        private static System.IO.FileSystemWatcher _watcher;
        public XmlReflectionMenuService(I18NManager i18NManager, IControllerFinder controllerFinder)
        {
            _i18NManager = i18NManager;
            _controllerFinder = controllerFinder;
        }

        void _watcher_Renamed(object sender, RenamedEventArgs e)
        {
            RemoveCacheFromFileName(e.Name);
            RemoveCacheFromFileName(e.OldName);
        }

        private void RemoveCacheFromFileName(string name)
        {
            if (string.Equals(name, "menu.config", StringComparison.InvariantCultureIgnoreCase))
                _menuItems = null;
        }

        void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            RemoveCacheFromFileName(e.Name);
        }
        public List<MenuItem> GetMenus(User user)
        {
            var menuItems = _menuItems;
            if (menuItems == null)
            {
                lock (_menuItemsLocker)
                {
                    menuItems = _menuItems;
                    if (menuItems == null)
                    {
                        var menuFileName = HttpContext.Current.Server.MapPath("~/menu.config");
                        if (_watcher == null)
                        {
                            _watcher = new FileSystemWatcher(Path.GetDirectoryName(menuFileName));
                            _watcher.Changed += new FileSystemEventHandler(Watcher_Changed);
                            _watcher.Created += new FileSystemEventHandler(Watcher_Changed);
                            _watcher.Deleted += new FileSystemEventHandler(Watcher_Changed);
                            _watcher.Renamed += new RenamedEventHandler(_watcher_Renamed);
                            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
                            _watcher.EnableRaisingEvents = true;
                        }

                        if (!File.Exists(menuFileName))
                            throw new FileNotFoundException(string.Format("please check file {0}", menuFileName));
                        XDocument xdoc = XDocument.Load(menuFileName);
                        MenuItem menuItem = AddChildren(xdoc.Root, 0);
                        menuItems = menuItem.Children.ToList();
                        _menuItems = menuItems;
                    }
                }
            }
            var query = CreateMenuItemsForUser(user, menuItems);
            return query.ToList();
        }

        private IEnumerable<MenuItem> CreateMenuItemsForUser(User user, IEnumerable<MenuItem> menuItems)
        {
            menuItems = menuItems.ToList();
            foreach (var menuItem in menuItems)
            {
                if (!menuItem.Permission.Check(user))
                    continue;

                var children = CreateMenuItemsForUser(user, menuItem.Children).ToList();
                if (children == null || children.Count() == 0 && string.IsNullOrEmpty(menuItem.Controller))
                    continue;

                MenuItem newItem = menuItem.Clone();
                newItem.Children = children;
                yield return newItem;
            }
        }

        private MenuItem AddChildren(XElement parent, int level)
        {
            var children = parent.Elements("node");
            if (children.Count() == 0)
            {
                var controllerType = _controllerFinder.FindController((string)parent.Attribute("controller"));
                if (controllerType == null) return null;

                var typeDescriptor = ViewModelDescriptor.GetTypeDescriptor(controllerType);
                var actionMethod = typeDescriptor.GetMethod((string)parent.Attribute("action"));
                if (actionMethod == null) return null;


                return new MenuItem("", (string)parent.Attribute("title") ?? actionMethod.Name,
                                            actionMethod.Description, actionMethod.Permission,
                                            (string)parent.Attribute("controller"), (string)parent.Attribute("action"),
                                            (string)parent.Attribute("parameterObject")) { Level = level };

            }
            else
            {
                var menuItem = new MenuItem((string)parent.Attribute("name"), (string)parent.Attribute("title"),
                                                     (string)parent.Attribute("desc"), new Allow()) { Level = level };
                if (parent.Attribute("controller") != null)
                {
                    var controllerType = _controllerFinder.FindController((string)parent.Attribute("controller"));
                    if (controllerType != null)
                    {
                        var typeDescriptor = ViewModelDescriptor.GetTypeDescriptor(controllerType);
                        var actionMethod = typeDescriptor.GetMethod((string)parent.Attribute("action"));
                        if (actionMethod == null) return null;

                        menuItem = new MenuItem("", (string)parent.Attribute("title") ?? actionMethod.Name,
                                                    actionMethod.Description, actionMethod.Permission,
                                                    (string)parent.Attribute("controller"), (string)parent.Attribute("action"),
                                                    (string)parent.Attribute("parameterObject")) { Level = level };
                    }
                }
                foreach (var xnode in children)
                {
                    var childMenuItem = AddChildren(xnode, level + 1);
                    if (childMenuItem != null)
                        menuItem.Add(childMenuItem);
                }
                return menuItem;
            }
        }
    }
    public class AppDomainControllerFinder : IControllerFinder
    {
        private Dictionary<string, Type> _name2ControllerType;
        private object _locker = new object();
        public Type FindController(string controllerName)
        {
            if (!controllerName.EndsWith("Controller"))
                controllerName += "Controller";

            if (_name2ControllerType == null)
            {
                lock (_locker)
                {
                    if (_name2ControllerType == null)
                    {
                        var dict = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

                        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            try
                            {
                                foreach (var exportedType in assembly.GetExportedTypes())
                                {
                                    if (typeof(Controller).IsAssignableFrom(exportedType))
                                    {
                                        dict.Add(exportedType.Name, exportedType);
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        _name2ControllerType = dict;
                    }
                }
            }
            if (_name2ControllerType.ContainsKey(controllerName))
                return _name2ControllerType[controllerName];
            return null;
        }
    }
}