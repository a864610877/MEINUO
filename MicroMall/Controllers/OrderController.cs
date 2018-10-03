//using MicroMall.Models.Orders;
//using Microsoft.Practices.Unity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace MicroMall.Controllers
//{
//    public class OrderController : Controller
//    {

//        private readonly IUnityContainer _container;

//        public OrderController(IUnityContainer container)
//        {
//            _container = container;
//        }
//        public ActionResult Index()
//        {
//           return View();
//        }
        
//        public ActionResult List(ListOrders request)
//        {
//            request.Query(0,1);
//            return View(request);
//        }

//        [HttpPost]
//        public ActionResult AjaxList(int orderState, int pageIndex)
//        {
//            var request = _container.Resolve<ListOrders>();
//            var item = request.Query(orderState, pageIndex);
//            return Json(item);
//        }

//        public ActionResult OrderSubmitm(string orderNo)
//        {
//            var request = _container.Resolve<OrderSubmitm>();
//            request.Load();
//            request.Ready(orderNo);
//            return View(request);
//        }

//        [HttpPost]
//        public ActionResult OrderSubmitm(OrderSubmitm request)
//        {
//            return Json(request.Save());
//        }


//        public ActionResult OrderDetail(string orderNo)
//        {
//            var request = _container.Resolve<OrderSubmitm>();
//            request.Load();
//            request.Ready(orderNo);
//            return View(request);
//        }
//        [HttpPost]
//        public ActionResult DeleteOrderDetail(DeleteOrderDetail request)
//        {
//            return Json(request.Save());
//        }

//        [HttpPost]
//        public ActionResult PayOrder(PayOrder request)
//        {
//            return Json(request.Save());
//        }

//        public ActionResult PayView(PayView request)
//        {
//            request.Ready();
//            return View(request);
//        }

//        public ActionResult PaySuccess()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ActionResult ConfirmationGoods(string orderNo)
//        {
//            var request = _container.Resolve<OrderSubmitm>();
//            return Json(request.ConfirmationGood(orderNo));
//        }
//        [HttpPost]
//        public ActionResult Refund(string orderNo)
//        {
//            var request = _container.Resolve<OrderSubmitm>();
//            return Json(request.Refund(orderNo));
//        }

//    }
//}
