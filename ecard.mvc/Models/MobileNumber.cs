namespace Ecard.Mvc.Models
{
    public class MobileNumber
    {
        public MobileNumber()
        {
            
        }

        public MobileNumber(string number, bool isMobileAvailable)
        {
            Number = number;
            IsMobileAvailable = isMobileAvailable;
        }

        public string Number { get; set; }
        public bool IsMobileAvailable { get; set; }
    }
}