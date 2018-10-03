using Ecard.Models;
using Ecard.Requests;
using Ecard.Services;
using MicroMall.Models.Homes;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Commoditys
{
    public class OperationCommodity : LayoutModel
    {
        public OperationCommodity()
        {
            //Url();
        }

        public void Ready(int id)
        {
            item = GetByCommodity(id);
        }

        public CommodityDetail item { get; set; }

        [Dependency]
        public ICommodityService CommodityService { get; set; }
         [Dependency]
        public ISpecificationService SpecificationService { get; set; }
         [Dependency]
         public IReviewService ReviewService { get; set; }
        [Dependency]
         public IOrderService OrderService { get; set; }
        //public  

        public dataJson GetCommodity(int start, int count)
        {
            try
            {
                int end = start + count;
                var tables = CommodityService.Query(start,end);
                if (tables.TotalCount > 0)
                {
                    var list = tables.ModelList.Select(x => new CommodityModel(x, ImageUrl)).ToList();
                    return new dataJson() { count = count, events = list, start = start, total = tables.TotalCount };
                }
                return new dataJson() { count = count, events = null, start = start, total = tables.TotalCount };
            }
            catch (Exception ex)
            {
                logService.Insert(ex);
                return null;
            }
        }

        public CommodityDetail GetByCommodity(int id)
        {
           // Load();
            var item = CommodityService.GetById(id);
            if(item!=null&&item.commodityState==CommodityStates.putaway)
            {
                var list = new List<SpecificationAndSpecificationDetail>();
                if(!string.IsNullOrWhiteSpace(item.specificationId))
                {
                    string[] spIds = item.specificationId.Split(',');
                    for (int i = 0; i < spIds.Count(); i++)
                    {
                        int specificationId = 0;
                        int.TryParse(spIds[i], out specificationId);
                        var specification = SpecificationService.GetSpecificationAndSpecificationDetailById(specificationId);
                        if (specification != null)
                            list.Add(specification);
                    }
                    //return new CommodityDetail(item, ImageUrl, list,null);
                }
                //int userId = 0;
                //var user = _securityHelper.GetCurrentUser();
                //if (user != null)
                //    userId = user.CurrentUser.UserId;
                //var request = new ReviewRequest();
                //request.CommodityId = item.commodityId;
                //request.State = ReviewStates.Show;
                //request.UserId = userId;
                ////request.State = ReviewStates.Show;
                //ListReviewView ListRv = new ListReviewView();
                //var Review = ReviewService.MicroMallQuery(request);
                //if(Review!=null)
                //{
                //    ListRv.TotalCount = Review.TotalCount;
                //    int TotalPage = Math.Max((Review.TotalCount + request.PageSize - 1) / request.PageSize, 1);
                //    if (1 == TotalPage)
                //    {
                //        ListRv.NextPage = 0;
                //    }
                //    else if (1 < TotalPage)
                //    {
                //        ListRv.NextPage = 2;
                //    }
                //    ListRv.List = Review.ModelList.Select(x => new ReviewView() {Content=x.Content, ReviewId=x.ReviewId, SubmitTime=x.SubmitTime.ToString(), UserId=x.UserId, UserName=x.UserName }).ToList();
                //}
                var site = SiteService.Query(new SiteRequest()).FirstOrDefault();
                if (site != null)
                    ImageUrl = site.imageUrl;
                return new CommodityDetail(item, ImageUrl, list, null);
            }
            return new CommodityDetail(null, "", null,null);
        }

        public ResultMessage AddReview(string content, int commodityId, string orderNo,int userId)
        {
            //var user = _securityHelper.GetCurrentUser();
            //if (user == null)
            //    return new ResultMessage() { Code = -1, Msg = "您还没有登录" };
            var item = CommodityService.GetById(commodityId);
            if(item==null)
                return new ResultMessage() { Code = -1, Msg = "商品不存在" };
            Review model = new Review();
            model.CommodityId = commodityId;
            model.Content = content;
            model.State = ReviewStates.StayCheck;
            model.SubmitTime = DateTime.Now;
            model.UserId = userId;
            model.ReviewId=ReviewService.Insert(model);
            var order = OrderService.GetOrderNo(orderNo);
            if (order != null && order.orderState == OrderStates.complete)
            {
                order.orderState = OrderStates.completePJ;
                OrderService.Update(order);
            }
            return new ResultMessage() { Code = 0,Msg=model.ReviewId.ToString() };
        }

        public ResultMessage DeleteReview(int ReviewId)
        {
            var item = ReviewService.GetById(ReviewId);
            if (item != null)
                ReviewService.Delete(item);
            return new ResultMessage() { Code = 0 };
        }

        public ListReviewView GetReview(int commodityId, int PageIndex)
        {
            int userId=0;
            var user = _securityHelper.GetCurrentUser();
            if (user != null)
                userId = user.CurrentUser.UserId;
            var request = new ReviewRequest();
            request.CommodityId = commodityId;
            request.PageIndex = PageIndex;
            request.State = ReviewStates.Show;
            request.UserId = userId;
            ListReviewView ListRv = new ListReviewView();
            var Review = ReviewService.MicroMallQuery(request);
            if(Review!=null)
            {
                ListRv.TotalCount = Review.TotalCount;
                int TotalPage = Math.Max((Review.TotalCount + request.PageSize - 1) / request.PageSize, 1);
                if (PageIndex == TotalPage)
                {
                    ListRv.NextPage = 0;
                }
                else if (PageIndex < TotalPage)
                {
                    ListRv.NextPage = PageIndex+1;
                }
                ListRv.List = Review.ModelList.Select(x => new ReviewView() { Content = x.Content, ReviewId = x.ReviewId, SubmitTime = x.SubmitTime.ToString(), UserId = x.UserId, UserName = x.UserName }).ToList();              
            }
            return ListRv;
        }
    }
}