using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface ICommoditySalesService
    {
        //int Insert(CommoditySales item);
        //int CanCel(int commoditySalesId);
        DataTables<CommoditySalesStatistics> Query(CommoditySalesStatisticsRequest request);
        
    }


    public class CommoditySalesStatistics
    {
        public int commodityId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string commodityName { get; set; }
        /// <summary>
        /// 总销售数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 总销售额
        /// </summary>
        public decimal amount { get; set; }
       
        /// <summary>
        /// 总返佣
        /// </summary>
        public decimal RebateAmount { get; set; }
    }
}
