using System;
using System.Collections.Generic;
using System.Linq;
using Ecard.Models;
using Oxite.Model;

namespace Oxite.Repositories
{ 
    public class DealSummaryForPos
    {
        public string PosName { get; set; }
        public int DealType { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
    }
    public class DealItemSummaryForPosRequest
    {
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public int ShopId { get; set; }
        public bool HasPos { get; set; }
        public int AccountShopId { get; set; }
        public DealItemSummaryForPosRequest()
        {
            AccountShopId = -1;
        }
    }
    public class DealItemQueryRequest
    {
        public DealItemQueryRequest()
        {
            DealTypes = new List<int>();
            Date1 = DateTime.Parse("1900-1-1");
            Date2 = DateTime.Parse("9900-1-1");
            AccountShopId = -1;
        }
        public int AccountShopId { get; set; }
        public int ShopId { get; set; }
        public int State { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public string AccountName { get; set; }
        public string PosName { get; set; }
        public int DealType { get; set; }

        public int AccountId { get; set; }

        public string ShopName { get; set; }
        public List<int> DealTypes { get; set; }

        public int Addin { get; set; }
    }
}