namespace Ecard.Mvc.Models
{
    public class ApplyToModel
    {
        public ApplyToModel(int value)
        {
            this.EnabledRecharging = (value & 0x00001) != 0;
            this.EnabledOpenAccount = (value & 0x00002) != 0;
            this.EnabledShopDealAccount = (value & 0x00004) != 0;
            this.EnabledShop = (value & 0x00008) != 0;
        }
        public ApplyToModel()
        {
        }
        public bool EnabledRecharging { get; set; }
        public bool EnabledOpenAccount { get; set; }
        public bool EnabledShopDealAccount { get; set; }
        public bool EnabledDistributor  { get; set; }

        public bool EnabledShop { get; set; }

        public int GetValue()
        {
            int v = 0;
            if (EnabledRecharging)
                v |= 0x00001;
            if (EnabledOpenAccount)
                v |= 0x00002;
            if (EnabledShopDealAccount)
                v |= 0x00004;
            if (EnabledShop)
                v |= 0x00008;
            return v;
        }
    }
}