using System;
using System.Collections.Generic;
using Ecard.Models;
using Moonlit.Reflection;

namespace Ecard.Services
{
    public class DashboardItemRepository : IDashboardItemRepository
    {   
        static List<DashboardItem> _items;
        static readonly object ItemsLocker = new object();

        public IList<DashboardItem> Query()
        {
            lock (ItemsLocker)
            {
                if (_items == null)
                {
                    _items = new List<DashboardItem>();

                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var assembly in assemblies)
                    {
                        try
                        {
                            foreach (var type in assembly.GetExportedTypes())
                            {
                                foreach (var methodInfo in type.GetMethods())
                                {
                                    var dashboardItemAttr = methodInfo.GetAttribute<DashboardItemAttribute>(false);
                                    if (dashboardItemAttr != null)
                                        _items.Add(new DashboardItem() { Controller = type.Name.Substring(0, type.Name.Length - 10), Action = methodInfo.Name });
                                }
                            }
                        }
                        catch 
                        {
                        }
                    }
                }
                return _items;
            }
        }
    }
}