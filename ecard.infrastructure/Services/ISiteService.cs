using System.Data;

using System.Linq;
using Ecard.Models;
using Moonlit.Data;

namespace Ecard.Services
{
    public interface ISiteService
    {
        QueryObject<Site> Query(SiteRequest request);
        void Update(Site item);
    }

    public class SiteRequest
    {
    }
}