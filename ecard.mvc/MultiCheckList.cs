using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Mvc
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class MultiCheckListAttribute : UIHintAttribute
    {
        public MultiCheckListAttribute()
            : base("MultiCheckList")
        {
        }
    }
}
