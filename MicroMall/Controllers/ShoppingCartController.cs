using MicroMall.Models.ShoppingCarts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroMall.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IUnityContainer _container;
        public ShoppingCartController(IUnityContainer container)
        {
            _container = container;
            //_loadIndex =loadIndex;
            //CommodityService = commodityService;
        }
        //
        // GET: /ShoppingCart/

        public ActionResult List(ListShoppingCarts request)
        {
            request.Query();
            return View(request);
        }
        [HttpPost]
        public ActionResult AddCart(int id, int num, string attribute)
        {
            var request = _container.Resolve<AddShoppingCarts>();
            request.attribute = attribute;
            request.id = id;
            request.num = num;
            return Json(request.Save());
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var request = _container.Resolve<ListShoppingCarts>();

            return Json(request.Delete(id));
        }

        //public ActionResult MyCart()
        //{

        //}
    }
}
