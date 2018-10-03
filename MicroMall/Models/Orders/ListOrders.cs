using Ecard.Mvc;
using Ecard.Requests;
using Ecard.Services;
using MicroMall.Models.JuMeiMallIndex;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Orders
{
    public class ListOrders : ResultMessage
    {

        public ListOrders(IOrderService OrderService, IUnityContainer _container, ILog4netService ILog4netService)
        {
            this.OrderService = OrderService;
            this._container = _container;
            this.ILog4netService = ILog4netService;
        }
        [Dependency]
        private IOrderService OrderService { get; set; }
        [Dependency]
        public IOrderDetailService OrderDetailService { get; set; }
        [Dependency]
        private  IUnityContainer _container { get; set; }
         [Dependency]
        private ILog4netService ILog4netService { get; set; }
        //[Dependency]
        //public ISiteService SiteService { get; set; }

        //[Dependency]
        //public ILog4netService Log4netService { get; set; }

        //[Dependency]
        //public SecurityHelper _securityHelper { get; set; }
        public OrderResponse List { get; set; }

        public int orderState { get; set; }
        //public int PageIndex { get; set; }

        //public int PageSize { get; set; }

        public void Query(int pageIndex=1,int userId=0)
        {
            try
            {
                var imageUrlRequest = _container.Resolve<OperationJuMeiMall>();
                var request = new MicroMallOrderRequest();
                request.UserId = userId;
                request.PageIndex = pageIndex;
                request.PageSize = 1000;
                if (orderState > 0)
                    request.orderState = orderState;
                var query = OrderService.MicroMallQuery(request);
                var response = new OrderResponse();
                if (query != null)
                {
                    //var site = SiteService.Query(new SiteRequest()).FirstOrDefault();
                    response.PageIndex = pageIndex;
                    int TotalPage = Math.Max((query.TotalCount + request.PageSize - 1) / request.PageSize, 1);
                    if (pageIndex == TotalPage)
                    {
                        response.NextPage = 0;
                        response.PrePage = pageIndex - 1;
                    }
                    else if(pageIndex<TotalPage)
                    {
                        response.NextPage = pageIndex + 1;
                        response.PrePage = pageIndex - 1;
                    }
                    response.List = new List<ListOrder>();
                    foreach (var item in query.ModelList)
                    {
                        ListOrder orderModel = new ListOrder();
                        orderModel.orderNo = item.orderNo;
                        orderModel.submitTime = DateTime.Now;
                        orderModel.orderState = item.orderState;
                        orderModel.payState = item.payState;
                        //orderModel.Amount = item.amount+item.freight;
                        orderModel.Amount = item.amount;
                        orderModel.orderId = item.orderId;
                        orderModel.freight = item.freight;
                        
                        var OrderDetail = OrderDetailService.GetByOrderNo(item.orderNo);
                        if (OrderDetail != null)
                        {
                            List<CommodityDetail> listCommodityDetail = new List<CommodityDetail>();
                            foreach (var commdityDetail in OrderDetail)
                            {
                                orderModel.commodityName = commdityDetail.commodityName;
                                orderModel.ImageUrl = imageUrlRequest.GetFirstImage(commdityDetail.images);
                                orderModel.commodityId = commdityDetail.commodityId;
                                //var model = new CommodityDetail();
                                //model.commodityName = commdityDetail.commodityName;
                                //model.price = commdityDetail.price;
                                //model.image =imageUrlRequest.GetFirstImage(commdityDetail.images);
                                //model.quantity = commdityDetail.quantity;
                                //model.commodityId = commdityDetail.commodityId;
                                //model.specification = commdityDetail.specification;
                                //listCommodityDetail.Add(model);
                            }
                            orderModel.ListCommodityDetail = listCommodityDetail;
                        }
                        response.List.Add(orderModel);
                    }
                }
                List = response;
            }
            catch(Exception ex)
            {
                ILog4netService.Insert(ex);
                Code = -1;
                Msg = "系统错误";
            }
        }
    }
}