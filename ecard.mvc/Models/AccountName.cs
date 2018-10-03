namespace Ecard.Mvc.Models
{
    [ValidatePropertyName("Value")]
    public class AccountName
    {  
        public string Value { get; set; }
        public int Length { get; set; }
        public static implicit operator string(AccountName accountName)
        {
            return accountName != null ? accountName.Value : string.Empty;
        }
    }
}