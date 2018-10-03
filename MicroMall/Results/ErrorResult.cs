using System.Web.Mvc;
using Oxite.Mvc.Extensions;

namespace Ecard.Mvc
{
    public class ErrorResult : ViewResult
    {
        public ErrorResult(string message)
        {
            this.ViewName = "Error";
            ViewData.SetMessage(message);
        }
    }
}