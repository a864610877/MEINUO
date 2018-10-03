using System.Web.Mvc;

namespace Oxite.Mvc.Infrastructure
{
    public interface ITokenHelper
    {
        string GetCurrentToken(Controller controller);
        void CreateToken(Controller controller);
    }
}