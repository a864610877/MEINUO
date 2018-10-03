namespace Ecard.Infrastructure
{
    public struct DealResult
    {
        public decimal Amount { get; set; }
        public int Point { get; set; }
        public int FreezeAmount { get; set; }
        public static DealResult operator +(DealResult r1, DealResult r2)
        {
            return new DealResult { Amount = r1.Amount + r2.Amount, Point = r1.Point + r2.Point, FreezeAmount = r1.FreezeAmount + r2.FreezeAmount };
        }
    }
}