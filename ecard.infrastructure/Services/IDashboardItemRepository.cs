using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ecard.Models;
using Moonlit;

namespace Ecard.Services
{
    public interface IDashboardItemRepository
    {
        IList<DashboardItem> Query();
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class DashboardItemAttribute : Attribute
    {
    }
}