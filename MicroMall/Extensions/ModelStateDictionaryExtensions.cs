using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Ecard.Validation;
using Oxite.Validation;

namespace Ecard.Mvc
{
    public static class ModelStateDictionaryExtensions
    {
        public static bool IsValid(this ModelStateDictionary modelStateDictionary, IValidator validator, string prefixName = "Item")
        {
            if (!modelStateDictionary.IsValid) return false;
            foreach (var validation in validator.Validate())
            {
                var propertyName = (!string.IsNullOrEmpty(prefixName) ? prefixName + "." : "") + validation.PropertyName;
                modelStateDictionary.AddModelError(propertyName, validation.Error);
            }
            return modelStateDictionary.IsValid;
        }
    }
}