using Moonlit;

namespace Ecard.Services
{
    public class LogRequest
    {
        private string _contentWith;
        private string _userName;
        public int? LogType { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string ContentWith
        {
            get
            {
                return _contentWith.NullIfEmpty();
            }
            set
            {
                _contentWith = value;
            }
        }

        public string UserName
        {
            get
            {
                return _userName.NullIfEmpty();
            }
            set
            {
                _userName = value;
            }
        }
    }
}