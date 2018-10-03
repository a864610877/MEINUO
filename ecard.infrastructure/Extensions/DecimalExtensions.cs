using System;

namespace Ecard
{
    public static class DecimalExtensions
    {
        public static decimal ToRound(this decimal value)
        {
            return value.ToRound(2);
        }
        public static decimal ToRound(this decimal value, int pos)
        {
            var s = value.ToString("f" + pos);
            return Convert.ToDecimal(s);
        }
    }
}