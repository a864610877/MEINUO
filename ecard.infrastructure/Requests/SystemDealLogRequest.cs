using System;
using Moonlit;

namespace Ecard.Services
{
    public class SystemDealLogRequest
    {
        private string _userName;
        public int? DealType { get; set; }

        public DateTime? SubmitTimeMin { get; set; }

        private DateTime? _submitTimeMax;
        private string _serialNo;

        public DateTime? SubmitTimeMax
        {
            get { return _submitTimeMax == null ? (DateTime?) null : _submitTimeMax.Value.Date.AddDays(1); }
            set { _submitTimeMax = value; }
        }

        public string UserName
        {
            get
            {
                return string.IsNullOrEmpty(_userName) ? null : _userName;
            }
            set
            {
                _userName = value;
            }
        }

        public int? State { get; set; }
        public int? SystemDealLogId { get; set; }

        public string SerialNo
        {
            get {
                return _serialNo.NullIfEmpty();
            }
            set {
                _serialNo = value;
            }
        }
    }
}