using System;
using System.Configuration;

namespace Ecard.Models
{
    public class OxiteSettings
    {
        public OxiteSettings()
        {
            ChangeCardAmount = Convert.ToDecimal(ReadValue("changeCardAmount", "2"));
            ChargeRate = Convert.ToDecimal(ReadValue("chargeRate", "0.02"));
        }

        private string ReadValue(string key, string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];
            return !string.IsNullOrEmpty(value) ? value : defaultValue;
        }

        /// <summary>
        /// 换卡金额
        /// </summary>
        public decimal ChangeCardAmount { get; set; }
        public decimal ChargeRate { get; set; }
    }
    public static class SiteExtensions
    {
        public static OxiteSettings GetSettings(this Site site)
        {
            return new OxiteSettings();
        }
    }
}
