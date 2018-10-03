using System;
using Moonlit;

namespace Ecard.Services
{
    public class ShopDealLogRequest
    {
        public int? State { get; set; }
        public int? ShopId { get; set; }

        public DateTime? SubmitDateMin { get; set; }

        public DateTime? SubmitDateMax { get; set; }
         
        public string AccountName
        {
            get { return _accountName.NullIfEmpty(); }
            set { _accountName = value; }
        }

        public string SerialNo
        {
            get
            {
                return _serialNo.NullIfEmpty();
            }
            set
            {
                _serialNo = value;
            }
        }

        public bool? IsLiquidate { get; set; }

        public int? AccountShopId { get; set; }


        private string _accountName;
        private string _serialNo;
    }
}