using System.Linq;
using Ecard.Models;

namespace Ecard
{
    public interface IPermissionService
    {
        IQueryable<Permission> QueryPermissions(UserType userType);//UserType userType
    }
}