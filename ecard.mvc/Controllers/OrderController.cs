using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Orders;
using Ecard.Mvc.Models.Shoping;
using Ecard.Mvc.ViewModels;
using Ecard.Requests;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class OrderController : Controller
    {

        private readonly IUnityContainer _unityContainer;

        [Dependency, NoRender]
        public IOrderDetailService orderDetailService { get; set; }
        [Dependency, NoRender]
        public IOrderService IOrderService { get; set; }

        [Dependency, NoRender]
        public ICityService cityService { get; set; }

        [Dependency, NoRender]
        public IProvinceService provinceService { get; set; }
        [Dependency, NoRender]
        public IMessagesService IMessagesService { get; set; }

        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }
        [Dependency, NoRender]
        public IAccountService IAccountService { get; set; }
        public OrderController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [CheckPermission(Permissions.OrderList)]
        [Ecard.Services.DashboardItem]
        public ActionResult OrderList(ListOrders request)
        {
            request.Query();
            return View(request);
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(int  id) 
        {

            var model = _unityContainer.Resolve<OrderDetailEdit>();
            model.Ready(id);
            var list = orderDetailService.QueryAll().ToList();
            var selectList = list.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString(),Selected=x.Id.ToString()==model.order.ExpressCompany });
            ViewData["ExpressCompany"] = selectList;

            var list1 = provinceService.Query().ToList();
            var selectList1 = list1.Select(x => new SelectListItem { Text = x.Name, Value = x.ProvinceId.ToString(),Selected=x.ProvinceId==model.order.provinceId});
            ViewData["province"] = selectList1;

            var list2 = cityService.Query(model.order.provinceId).ToList();
            var selectList2 = list2.Select(x => new SelectListItem { Text = x.Name, Value = x.CityId.ToString(), Selected = x.CityId == model.order.cityId });
            ViewData["city"] = selectList2;

            var items = from x in model.distributionstate.Items
                        select new SelectListItem() { Selected = model.distributionstate.Key == x.Key, Text = x.Name, Value = model.distributionstate.GetKey(x.Key) };
            ViewData["distributionstate"] = items;

            var items1 = from x in model.distributionType.Items
                         select new SelectListItem() { Selected = model.distributionType.Key == x.Key, Text = x.Name, Value = model.distributionType.GetKey(x.Key) };
            ViewData["distributionType"] = items1;
            var items2 = from x in model.payState.Items
                         select new SelectListItem() { Selected = model.payState.Key == x.Key, Text = x.Name, Value = model.payState.GetKey(x.Key) };
            ViewData["payState"] = items2;
            var items3 = from x in model.orderState.Items
                         select new SelectListItem() { Selected = model.orderState.Key == x.Key, Text = x.Name, Value = model.orderState.GetKey(x.Key) };
            ViewData["orderState"] = items3;
            var items4 = from x in model.payType.Items
                         select new SelectListItem() { Selected = model.payType.Key == x.Key, Text = x.Name, Value = model.payType.GetKey(x.Key) };
            ViewData["payType"] = items4;

            return View(model);
        }

        /// <summary>
        /// 根据省份ID获取所属城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetCityByProvinceId(int id) 
        {
            var citys = cityService.Query(id).ToList();
            return Json(citys);
        }

        /// <summary>
        /// 删除订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id) 
        {
            ResultMsg result = new ResultMsg();

           var orderdetail= orderDetailService.GetById(id);
           if (orderdetail!=null)
           {
               orderDetailService.Delete(orderdetail);
               result.Code = 1;

               result.CodeText = "删除成功!";
               return Json(result);
           }
           else
           {
               result.Code = 2;

               return Json("删除失败！");
           }
        }

        [HttpPost]
        /// <summary>
        /// 编辑订单
        /// </summary>
        /// <returns></returns>
        public ActionResult EditOrder(OrderEdit strIds) 
        {
            var mes = strIds.Save();
            return Json(mes);
        }


        public ActionResult AjaxList(OrderRequest request)
        {
            var create = _unityContainer.Resolve<ListOrders>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }
        public ActionResult AjaxList1(OrderDetailRequest request)
        {
            var create = _unityContainer.Resolve<OrderDetailEdit>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }

        [CheckPermission(Permissions.ApplyRefundOrderList)]
        public ActionResult ApplyRefundList(ListApplyRefundOrders request)
        {
            request.Query();
            return View(request);
        }

        [HttpPost]
        public ActionResult AjaxApplyRefundList(OrderRequest request)
        {
            var create = _unityContainer.Resolve<ListApplyRefundOrders>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }

        [HttpPost]
        public ActionResult AgreeRefund(int orderId)
        {
            var model = _unityContainer.Resolve<OrderDetailEdit>();
            return Json(model.Agree(orderId));
        }
        [HttpPost]
        public ActionResult Ship(int orderId, string expressCompany, string expressNumber)
        {
            var tran=transaction.BeginTransaction();
            try
            {
                var order = IOrderService.GetById(orderId);
                if (order == null)
                    return Json(new ResultMsg() { Code = -1, CodeText = "订单不存在！" });
                if (order.orderState != OrderStates.paid)
                    return Json(new ResultMsg() { Code = -1, CodeText = "订单不是已付款状态！" });
                order.ExpressCompany = expressCompany;
                order.ExpressNumber = expressNumber;
                order.orderState = OrderStates.shipped;
                order.distributionstate = DistributionStates.shipped;
                order.ShipTime = DateTime.Now;
                IOrderService.Update(order);
                //var account = IAccountService.GetByUserId(order.userId);
                //if (!string.IsNullOrWhiteSpace(account.openID))
                //{
                //    var message = new Fz_Messages();
                //    message.accountId = account.accountId;
                //    message.keyword1 = order.orderNo;
                //    message.keyword2 = "已发货";
                //    message.msg = "物流信息 快递公司:" + order.ExpressCompany + " 快递单号:" + order.ExpressNumber;
                //    message.msgType = MsgType.orderState;
                //    message.openId = account.openID;
                //    message.state = MessagesState.staySend;
                //    message.submitTime = DateTime.Now;
                //    IMessagesService.Insert(message);
                //}
                tran.Commit();
                return Json(new ResultMsg() { Code = 0 });
            }
            catch (Exception ex)
            {
                return Json(new ResultMsg() { Code = -1, CodeText = ex.Message });
            }
            finally
            {
                tran.Dispose();
            }
        }
        [HttpPost]
        public ActionResult cancelOrder(int orderId)
        {
            try
            {
                var order = IOrderService.GetById(orderId);
                if (order == null)
                    return Json(new ResultMsg() { Code = -1, CodeText = "订单不存在！" });
                if (order.orderState == OrderStates.complete || order.orderState == OrderStates.cancel)
                    return Json(new ResultMsg() { Code = -1, CodeText = "订单不可取消！" });
                order.orderState = OrderStates.cancel;
                IOrderService.Update(order);
                //var account = IAccountService.GetByUserId(order.userId);
                //if (!string.IsNullOrWhiteSpace(account.openID))
                //{
                //    var message = new Fz_Messages();
                //    message.accountId = account.accountId;
                //    message.keyword1 = order.orderNo;
                //    message.keyword2 = "已取消";
                //    message.msg = "";
                //    message.msgType = MsgType.orderState;
                //    message.openId = account.openID;
                //    message.state = MessagesState.staySend;
                //    message.submitTime = DateTime.Now;
                //    IMessagesService.Insert(message);
                //}
                return Json(new ResultMsg() { Code = 0 });
            }
            catch (Exception ex)
            {
                return Json(new ResultMsg() { Code = -1, CodeText = ex.Message });
            }
        }
    }
}
