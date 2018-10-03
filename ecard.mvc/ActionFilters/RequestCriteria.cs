using System;

namespace Ecard.Mvc.ActionFilters
{
    public class RequestCriteria : IActionFilterCriteria
    {
        private const string Key = "export";
        private readonly string _value;

        public RequestCriteria(string value)
        {
            _value = value;
        }

        public bool Match(ActionFilterRegistryContext context)
        {
            return string.Equals(_value, context.ControllerContext.RequestContext.HttpContext.Request[Key], StringComparison.OrdinalIgnoreCase);
        }
    }
}