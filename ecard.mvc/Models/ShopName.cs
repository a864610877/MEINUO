namespace Ecard.Mvc.Models
{
    [ValidatePropertyName("Value")]
    public class ShopName
    {
        public string Value { get; set; }
        public int Length { get; set; }
        public static implicit operator string(ShopName accountName)
        {
            return accountName != null ? accountName.Value : string.Empty;
        }
    }
}