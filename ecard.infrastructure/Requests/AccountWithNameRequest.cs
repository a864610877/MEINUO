namespace Ecard.Services
{
    public class AccountWithNameRequest
    {
        public int[] States { get; set; }

        public string Name { get; set; }

        public string OwnerDisplayName { get; set; }

        public string OwnerMobile { get; set; }
    }
}