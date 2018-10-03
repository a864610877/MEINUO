using System;

namespace Ecard
{
    public static class ValueExtensions
    {
        public static bool IsValidity(this DateTime time)
        {
            return time > DateTime.Parse("1900-01-01");
        }
    }
}