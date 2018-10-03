using Ecard.Services;
using MicroMall.Models.UserAddresss;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroMall.Controllers
{
    public class UserAddressController : Controller
    {
       private readonly IUnityContainer _container;

       private readonly IUserAddressService _userAddressService;
       public UserAddressController(IUnityContainer container, IUserAddressService userAddressService)
       {
           _container = container;
           _userAddressService = userAddressService;
       }

       public ActionResult GetUserAddress(int id)
       {
           var item = _userAddressService.GetById(id);
           return Json(item);
       }
       [HttpPost]
       public ActionResult Save(AddUserAddress request)
       {
           //request.Save();
           return Json("");
       }

    }
}
