
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Ecard
{
    public static class DateTimeExtensions
    {
        public static string ToColumnDate(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");

        }
        public static string ToColumnDate(this DateTime? dateTime)
        {
            if (dateTime == null) return "";
            return dateTime.Value.ToColumnDate();
        }
        public static string ToStringForEdit(this DateTime dateTime)
        {
            return string.Format("{0} {1}", dateTime.ToShortDateString(), dateTime.ToShortTimeString());
        }

        public static string Format(this DateTime dateTime)
        {
            if (dateTime == DateTime.Parse("1901-01-01 01:02:03"))
                return "";
            return string.Format("{0:yyyy-MM-dd}", dateTime);
        }

        public static string FormatForTime(this DateTime dateTime)
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", dateTime);
        }


        public static string FormatForXml(this DateTime dateTime)
        {
            return string.Format("{0:yyyy-MM-ddTHH:mm:ss}", dateTime);
        }

        public static string ToStringForFeed(this DateTime dateTime)
        {
            return dateTime.ToString("R");
        }

        public static int ToDbMonth(this DateTime dateTime)
        {
            return dateTime.Year * 100 + dateTime.Month;
        }
        public static DateTime FromDbMonth(this int dateTime)
        {
            return DateTime.Parse(dateTime / 100 + "-" + dateTime % 100);
        }
        public static IList<T> ToPaged<T>(this IQueryable<T> myQuery, int pageIndex, int pageSize, string sort)
        {
            var items = myQuery;
            if (myQuery != null)
            {
                if (pageSize > 0)
                {
                    items = items.OrderBy(sort).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    return items.ToList();
                }
            }
            return null;
        }
    }

}