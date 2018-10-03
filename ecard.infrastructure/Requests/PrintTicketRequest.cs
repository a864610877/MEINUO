using Moonlit;

namespace Ecard.Services
{
    public class PrintTicketRequest
    {
        private string _accountName;
        private string _serialNo;

        public string AccountName
        {
            get { return _accountName.NullIfEmpty(); }
            set { _accountName = value; }
        }

        public int? LogType { get; set; }

        public string SerialNo
        {
            get
            {
                return  _serialNo.NullIfEmpty();
            }
            set
            {
                _serialNo = value;
            }
        }
    }
}