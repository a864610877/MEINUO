using Moonlit;

namespace Ecard.Services
{
    public class ShopRequest
    {
        private string _name;
        private string _nameWith;
        private string _displayNameWith;

        public string Name
        {
            get
            {
                return _name.NullIfEmpty();
            }
            set
            {
                _name = value;
            }
        }
        public bool? IsMobileAvailable { get; set; }
        public int? State { get; set; }
        public int? ShopId { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string NameWith
        {
            get
            {
                return _nameWith.NullIfEmpty();
            }
            set
            {
                _nameWith = value;
            }
        }

        public string DisplayNameWith
        {
            get
            {
                return _displayNameWith.NullIfEmpty();
            }
            set
            {
                _displayNameWith = value;
            }
        }

        public int[] ShopIds { get; set; }

        public bool? IsBuildIn { get; set; }
    }
}